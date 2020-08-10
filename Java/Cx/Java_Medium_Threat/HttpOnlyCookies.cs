CxList httpOnlyCookies = All.NewCxList();
CxList cookiesDefinition = Find_Cookie_Declaration();
CxList methods = Find_Methods();

CxList setHeader = Find_Methods().FindByMemberAccess("HttpServletResponse.setHeader");
//Find if cookie is being set as a HttpServletResponse.setHeader("Set-Cookie",...)
//Second Parameter Cases:
//string literal
//unknown references
CxList strings = Find_Strings();
CxList setCookie = strings.GetParameters(setHeader, 0).FindByShortName("Set-Cookie");
setHeader = setHeader.FindByParameters(setCookie);
CxList httponlyall = strings.FindByShortNames(new List<string>{"*HttpOnly*", "*httpOnly*"});
CxList secondParam = All.GetParameters(setHeader, 1);
CxList allUnderParam = All.GetByAncs(secondParam);
CxList ur = allUnderParam * Find_UnknownReference();
CxList rightSide = All.FindByAssignmentSide(CxList.AssignmentSide.Right);
CxList httpOnly = httponlyall.GetByAncs(rightSide);
CxList ok = setHeader.FindByParameters(ur.DataInfluencedBy(httpOnly));
CxList existent = httponlyall * allUnderParam;
ok.Add(setHeader.FindByParameters(existent));
setHeader -= ok;

CxList unsecuredCookies = All.NewCxList();
// Sanitize cookiesDefinition

if (Find_HttpOnlyCookies_In_Config().Count != 0 || Find_HttpOnlyCookies_In_Config("true").Count == 0)
{
	httpOnlyCookies = Find_Set_HttpOnly_To_True();
	unsecuredCookies = cookiesDefinition - cookiesDefinition.FindDefinition(httpOnlyCookies.GetTargetOfMembers());
} 
else 
{
	httpOnlyCookies = Find_Set_HttpOnly_To_True(Find_BooleanLiteral().FindByShortName("false"));
	unsecuredCookies = cookiesDefinition.FindDefinition(httpOnlyCookies.GetTargetOfMembers());	
}

CxList getCookies = methods.FindByMemberAccess("request.getCookies");
unsecuredCookies -= unsecuredCookies.InfluencedBy(getCookies);

CxList addCookie = methods.FindByMemberAccess("response.addCookie");
result = addCookie.InfluencedBy(unsecuredCookies);

result.Add(setHeader);