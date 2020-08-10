// This query is composed of three parts:
// 1. Potential inputs -> Outputs
// 2. Inputs -> Potential outputs
// 3. Potential inputs -> Potential outputs

CxList sanitize = Find_XSS_Sanitize();

CxList inputs = Find_DB_Out();
inputs.Add(Find_FileSystem_Read());
inputs.Add(Find_Read_NonDB());

CxList potentialInputs = Find_Potential_Inputs();

CxList outputs = Find_XSS_Outputs();

CxList potentialOutputs = Find_Potential_Outputs() - Find_Header_Outputs();

// 1
result = inputs.InfluencingOnAndNotSanitized(potentialOutputs, sanitize);

// 2
result.Add(potentialInputs.InfluencingOnAndNotSanitized(outputs, sanitize));

// 3
result.Add(potentialInputs.InfluencingOnAndNotSanitized(potentialOutputs, sanitize));

result.ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow);