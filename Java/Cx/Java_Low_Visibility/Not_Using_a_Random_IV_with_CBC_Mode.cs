CxList createExpr = Find_ObjectCreations().FindByName("*IvParameterSpec*");

CxList paramsExpr = All.GetParameters(createExpr, 0);
paramsExpr -= paramsExpr.FindByType(typeof(Param));

// Remove results that are in DECRYPT_MODE
CxList memberAccesses = Find_MemberAccesses();
CxList methodDecryptMode = memberAccesses.FindByShortName("DECRYPT_MODE").GetAncOfType(typeof(MethodInvokeExpr)).InfluencedBy(paramsExpr);
CxList methodFirstNode = methodDecryptMode.GetStartAndEndNodes(CxList.GetStartEndNodesType.StartNodesOnly);
paramsExpr -= methodFirstNode;

CxList random = All.FindByType("*Random") ;
random.Add(All.FindByMemberAccess("Math.Random"));

CxList paramsExprRefs = All.FindAllReferences(paramsExpr);

CxList flow = All.DataInfluencedBy(random).GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly);
CxList paramsAncs = paramsExprRefs.GetByAncs(flow);

CxList res = paramsExpr - paramsExpr.FindAllReferences(paramsAncs);
result.Add(res);