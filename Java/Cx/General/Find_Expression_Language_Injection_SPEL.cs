if (param.Length == 1)
{

	CxList inputs = param[0] as CxList;

	CxList methods = Find_Methods();
	CxList outputs = Find_SPEL_Outputs();
	CxList sanitizers = All.NewCxList();

	sanitizers.Add(Find_HashSanitize());
	sanitizers.Add(Find_Base64Encoders());
	sanitizers.Add(Find_HexEncoders());
	sanitizers.Add(Find_CollectionAccesses());
	sanitizers.Add(Find_Integers());


	result.Add(inputs.InfluencingOnAndNotSanitized(outputs, sanitizers));
}