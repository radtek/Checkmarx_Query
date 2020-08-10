if (param.Length == 1)
{
	CxList inputs = param[0] as CxList;
	CxList exec = Find_Command_Injection_Outputs();
	CxList sanitize = Find_Command_Injection_Sanitize();

	result = exec.InfluencedByAndNotSanitized(inputs, sanitize, CxList.InfluenceAlgorithmCalculation.NewAlgorithm)
		+ exec * inputs;

	result = result.ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow);
}