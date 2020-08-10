CxList inputs = Find_Interactive_Inputs();
CxList codeOutputs = Find_Code_Injection_Outputs();
CxList sanitize = Find_General_Sanitize();

result = inputs.InfluencingOnAndNotSanitized(codeOutputs, sanitize, CxList.InfluenceAlgorithmCalculation.NewAlgorithm);