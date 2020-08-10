// This query aims to find http.SetCookie methods where the cookie has unsafe flags
if ( param.Length == 1 )
{	
	// The cookie flag
	string flag = param[0] as string;

	CxList methods = Find_Methods();
	CxList unkRefs = Find_UnknownReference();
	CxList trueAbsctractValues = Find_True_Abstract_Value();
	CxList falseAbstractValues = Find_False_Abstract_Value();
	string[] cookieTypes = new string[]{"http.Cookie", "Cookie"};

	// Find cookie creations and declarations
	CxList cookieCreations = Find_ObjectCreations().FindByTypes(cookieTypes);
	CxList cookieDecls = Find_Declarators().FindByTypes(cookieTypes);

	// Find cookies with unsafe flag
	CxList unkRefsCookies = unkRefs.FindAllReferences(cookieDecls);
	CxList members = unkRefsCookies.GetMembersOfTarget();
	CxList membersWithFlag = members.FindByShortName(flag);
	CxList sanitizers = members - membersWithFlag;

	// Find cookie declarators without flag set
	CxList allRefsCookiesWithFlag = unkRefsCookies.FindAllReferences(membersWithFlag.GetTargetOfMembers());
	CxList cookiesWithoutFlag = cookieDecls - cookieDecls.FindAllReferences(allRefsCookiesWithFlag);

	// Remove safe cookies
	CxList safeMembersWithFlag = membersWithFlag.DataInfluencedBy(trueAbsctractValues);
	membersWithFlag -= safeMembersWithFlag;

	// Find safe cookie object creations
	CxList safeObjCreationRefs = unkRefs.FindAllReferences(cookieCreations.GetAssignee()).DataInfluencedBy(safeMembersWithFlag);
	CxList safeObjCreations = cookieDecls.FindDefinition(safeObjCreationRefs).GetAssigner();
	CxList unsafeCookieCreations = cookieCreations - safeObjCreations;

	// Find http responses 
	CxList httpSetCookieMethods = methods.FindByMemberAccess("net/http.SetCookie");

	// Calculate unsafe flows
	result = httpSetCookieMethods.InfluencedByAndNotSanitized(falseAbstractValues, sanitizers);
	cookiesWithoutFlag.Add(unsafeCookieCreations);
	result.Add(httpSetCookieMethods.DataInfluencedBy(cookiesWithoutFlag));

	// Remove redundant flows
	result = result.ReduceFlow(CxList.ReduceFlowType.ReduceSmallFlow);
}