CxList inputs = Find_Interactive_Inputs();
CxList outputs = Find_XSS_Outputs();

CxList sanitized = Find_XSS_Sanitize();
// Add API outputs as sanitizers
sanitized.Add(Find_API_Response_Outputs());
// Remove dead code
sanitized.Add(Find_Dead_Code_AbsInt());

result = inputs.InfluencingOnAndNotSanitized(outputs, sanitized, CxList.InfluenceAlgorithmCalculation.NewAlgorithm);
result = result.ReduceFlow(CxList.ReduceFlowType.ReduceSmallFlow);