CxList inputs = Find_Interactive_Inputs();
CxList outputs = Find_XSS_Outputs();

//自定义部分
inputs -= All.FindByShortName("getContentType").FindByType(typeof(MethodInvokeExpr)); 

CxList sanitized = Find_XSS_Sanitize() + Find_DB_In() + Find_Files_Open() - Find_Decode_Encode(outputs);

result = inputs.InfluencingOnAndNotSanitized(outputs, sanitized, CxList.InfluenceAlgorithmCalculation.NewAlgorithm).ReduceFlowByPragma();
result = result.ReduceFlow(CxList.ReduceFlowType.ReduceSmallFlow);