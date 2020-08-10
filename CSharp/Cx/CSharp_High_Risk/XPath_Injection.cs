CxList XPath = Find_XPath_Output();
CxList inputs = Find_Interactive_Inputs();
CxList sanitized = Find_XPath_Injection_Sanitizers();

//add as sanitizer replace of problematic paths - .. or "/" or "\"
CxList replace = All.FindByName("*.Replace*");

replace = replace.FindByType(typeof(MemberAccess)) + 
	replace.FindByType(typeof(MethodInvokeExpr));

replace = replace.FindByParameterValue(0, "..", BinaryOperator.IdentityEquality) + 
	replace.FindByParameterValue(0, "\\", BinaryOperator.IdentityEquality) +
	replace.FindByParameterValue(0, "/", BinaryOperator.IdentityEquality);

replace -= replace.FindByParameterValue(1, "..", BinaryOperator.IdentityEquality) +
	replace.FindByParameterValue(1, "\\", BinaryOperator.IdentityEquality) + 
	replace.FindByParameterValue(1, "//", BinaryOperator.IdentityEquality);
sanitized.Add(replace);
sanitized -= XPath; 
result = XPath.InfluencedByAndNotSanitized(inputs, sanitized).ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow);