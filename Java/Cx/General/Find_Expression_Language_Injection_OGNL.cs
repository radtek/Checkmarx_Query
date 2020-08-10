if (param.Length == 1)
{
	CxList inputs = param[0] as CxList;

	CxList methods = Find_Methods();
	CxList outputs = Find_OGNL_Outputs();
	CxList sanitizers = All.NewCxList();

	sanitizers.Add(Find_HashSanitize());
	sanitizers.Add(Find_Base64Encoders());
	sanitizers.Add(Find_HexEncoders());
	sanitizers.Add(Find_CollectionAccesses());
	sanitizers.Add(Find_Integers());

	//Hexadecimal encode
	CxList encodeHex = All.FindByMemberAccess("Hex.encode*", true);
	sanitizers.Add(encodeHex);

	// Exclude getAtribute Parameter
	CxList getAttribute = methods.FindByName("request.getAttribute");
	getAttribute.Add(methods.FindByMemberAccess("HttpServletRequest.getAttribute"));
	sanitizers -= All.GetParameters(getAttribute);
	result.Add(inputs.InfluencingOnAndNotSanitized(outputs, sanitizers));
}