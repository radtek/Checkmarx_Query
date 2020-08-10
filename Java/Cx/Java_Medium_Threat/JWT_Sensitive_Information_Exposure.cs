List < string > sinkNames = new List<string> { "setClaims", "claim", "addClaims", "setPayload" };

CxList methods = Find_Methods();
CxList jwtsBuildRef = methods.FindByMemberAccess("Jwts.builder");
CxList passwords = Find_All_Passwords();
CxList sinks = methods.FindByShortNames(sinkNames).InfluencedBy(jwtsBuildRef).GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly);

result = sinks.InfluencedBy(passwords).ReduceFlow(CxList.ReduceFlowType.ReduceSmallFlow);