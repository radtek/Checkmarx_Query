CxList db = Find_DB_Output();
CxList inputs = Find_Interactive_Inputs() + Find_Header_Inputs();
CxList sanitized = Sanitize() + Find_Test_Code();

result = db.InfluencedByAndNotSanitized(inputs, sanitized);
result = result.ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow);

result -= Find_Test_Code();