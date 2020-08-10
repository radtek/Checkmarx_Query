CxList inputs = Find_Interactive_Inputs();
CxList outputs = Find_Interactive_Outputs();

// Delete "header" from the outputs because is not a case of Reflected XSS 
CxList methods = Find_Methods();
outputs -= methods.FindByShortName("header");

CxList sanitized = Find_XSS_Sanitize();

result = inputs.InfluencingOnAndNotSanitized(outputs, sanitized).ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow);