if (param.Length == 1)
{
	CxList methodDecl = Find_MethodDeclaration();
	CxList paramDecls = Find_ParamDeclaration();

	// inputs
	CxList inputs = param[0] as CxList;
	// inputs (commands) passed from the console
	inputs.Add(paramDecls.GetParameters(methodDecl.FindByName("*.main")));
	
	// sinks
	CxList commandArg = Find_Command_Arguments();

	// sanitizers
	CxList sanitize = Find_Command_Injection_Sanitize();

	// add String.replace() and String.replaceAll as sanitizers
	CxList methods = Find_Methods();
	sanitize.Add(methods.FindByShortName("replace*", false));

	// result
	result = commandArg.InfluencedByAndNotSanitized(inputs, sanitize, CxList.InfluenceAlgorithmCalculation.NewAlgorithm);
	result = result.ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow);
}