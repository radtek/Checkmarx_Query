CxList sensitiveInformation = Find_Personal_Info();
CxList passwordRelatedNodes = Password_Privacy_Violation_List();
CxList methods = Find_Methods();

CxList queryStringGetMethods = methods.FindByMemberAccess("*ServletRequest.getParameter");
queryStringGetMethods.Add(methods.FindByMemberAccess("*ServletRequest.getParameterValues"));
CxList queryStringMaps = methods.FindByMemberAccess("*ServletRequest.getParameterMap");
queryStringGetMethods.Add(queryStringMaps);

CxList getMethods = methods.FindByShortName("get");
CxList getMethodsForSensitiveInfo = getMethods.FindByParameters(passwordRelatedNodes);

result = queryStringGetMethods.DataInfluencingOn(sensitiveInformation).ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow);
result.Add(passwordRelatedNodes.DataInfluencingOn(queryStringGetMethods));
result.Add(queryStringMaps.GetMembersOfTarget() * getMethodsForSensitiveInfo);