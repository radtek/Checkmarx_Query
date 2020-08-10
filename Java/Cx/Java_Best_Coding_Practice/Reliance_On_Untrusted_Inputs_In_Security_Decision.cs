CxList sanitizer = Find_Sanitize();
sanitizer -= sanitizer.FindByShortName("setAttribute");

CxList urlEncodeSanitizer = Find_URL_Encode();
sanitizer.Add(urlEncodeSanitizer);

CxList inputs = Find_Source_Of_Security_Decision();
//System.getProperties and getString should not be considered as input
string [] systemsP = new string[]{"System.getProperty", "System.getProperties"};
CxList listSystemGetPropertiesInInputs = inputs.FindByMemberAccesses(systemsP);
CxList getStrings = inputs.FindByShortName("getString");
inputs -= listSystemGetPropertiesInInputs;
inputs -= getStrings;

// calculate sink of security
CxList sink = Find_Sink_Of_Security_Decision();

// search for If conditions (all the elements in the condition)
CxList conditions = All.NewCxList();
CxList ifAllCond = Find_Ifs();
foreach(CxList curIf in ifAllCond)
{
	try
	{
		IfStmt ifStmt = curIf.TryGetCSharpGraph<IfStmt>();
		CxList cond = All.NewCxList();
		cond.Add(ifStmt.Condition.NodeId, ifStmt.Condition);
		conditions.Add(All.GetByAncs(cond));
	}
	catch (Exception e)
	{
		cxLog.WriteDebugMessage(e.Message);
	}
}

//now get the variables, parameters and member accesses from all the elements in the conditions of ifs
CxList relevant_objects = All.NewCxList();
CxList allVariables = Find_UnknownReference();
allVariables.Add(Find_Methods());
relevant_objects.Add(allVariables);
relevant_objects.Add(Find_Params());
relevant_objects.Add(Find_MemberAccess());

conditions = conditions * relevant_objects;

// first part. path from input to conditions with sanitizer
CxList part1 = inputs.InfluencingOnAndNotSanitized(conditions, sanitizer);

//second part. path from conditions (ifs) to get all variables inside to sinks
CxList sinksInIfs = sink.GetByAncs(ifAllCond);
foreach (CxList flows in part1.GetCxListByPath()){
	
	if(flows.GetStartAndEndNodes(CxList.GetStartEndNodesType.AllNodes).Count <= 5){
			
		CxList ifAct = flows.GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly);
		CxList ifActual = ifAct.GetAncOfType(typeof(IfStmt));
		CxList variables = allVariables.GetByAncs(ifActual);
		CxList sinkActual = sinksInIfs.GetByAncs(ifActual);
		variables -= sinkActual;
		CxList part2 = variables.InfluencingOnAndNotSanitized(sinkActual, sanitizer);
		if(part2.Count > 0){
			CxList final = flows.ConcatenateAllPaths(part2);
			result.Add(final);
		}
	}		
}