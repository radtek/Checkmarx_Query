CxList socket = Find_Socket_Resource();
CxList sinks = All.GetByAncs(socket);
CxList forNames = Find_Methods().FindByMemberAccess("Class.forName");
forNames -= All.GetParameters(forNames, 1).GetAncOfType(typeof(MethodInvokeExpr));
CxList sanitizers = Find_CollectionAccesses();
sanitizers.Add(All.GetParameters(forNames, 0));
CxList inputs = Find_Interactive_Inputs();
CxList paths = inputs.InfluencingOnAndNotSanitized(sinks, sanitizers);
result = paths.ReduceFlow(CxList.ReduceFlowType.ReduceSmallFlow);