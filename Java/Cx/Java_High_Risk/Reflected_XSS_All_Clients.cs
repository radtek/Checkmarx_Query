CxList inputs = Find_Interactive_Inputs() - Find_Properties_Input();
CxList outputs = Find_XSS_Outputs();
CxList methods = Find_Methods();

CxList sanitized = All.NewCxList();
sanitized.Add(Find_XSS_Sanitize(),
	Find_DB_In(),
	Find_Files_Open(),
	Find_API_Response_Outputs());
sanitized -= Find_Decode_Encode(outputs);

// addCookie sets the cookie in the headers response, not in the body
sanitized.Add(methods.FindByMemberAccess("ServletResponse.addCookie"));
// Remove Cookie.getName which are not tainted by new Cookie(input, value)
CxList newCookies = Find_Object_Create().FindByShortName("Cookie");
CxList notInfluencedNewCookies = newCookies - newCookies.InfluencedBy(inputs);
CxList cookieMethodRefs = Find_UnknownReference().FindAllReferences(notInfluencedNewCookies.GetAssignee()).GetMembersOfTarget();
CxList getsToRemove = cookieMethodRefs.FindByShortName("getName");
CxList cookieGetValue = cookieMethodRefs.FindByShortName("getValue");
CxList cookieSetValue = cookieMethodRefs.FindByShortName("setValue");
// Remove getValue that is not tainted by either constructor or setValue
getsToRemove.Add(cookieGetValue.NotInfluencedBy(cookieSetValue));
sanitized.Add(getsToRemove);

// Remove dead code
sanitized.Add(Find_Dead_Code_AbsInt());

result = inputs.InfluencingOnAndNotSanitized(outputs, sanitized, CxList.InfluenceAlgorithmCalculation.NewAlgorithm).ReduceFlowByPragma();
result = result.ReduceFlow(CxList.ReduceFlowType.ReduceSmallFlow);