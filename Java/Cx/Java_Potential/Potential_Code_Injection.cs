CxList inputs = Find_Potential_Inputs();
CxList code = Find_Code_Injection_Outputs();
CxList sanitize = Find_General_Sanitize();
sanitize.Add(Find_General_Sanitize_Injection());

result = inputs.InfluencingOnAndNotSanitized(code, sanitize);