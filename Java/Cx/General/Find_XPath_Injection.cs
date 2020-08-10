if (param.Length == 1)
{
	CxList inputs = param[0] as CxList;
	CxList xPath = Find_XPath_Outputs();
	CxList sanitized = Find_XPath_Sanitize();   
	result = xPath.InfluencedByAndNotSanitized(inputs, sanitized);
	result = result.ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow);
}