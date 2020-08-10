if (param.Length == 1)
{
	CxList inputs = param[0] as CxList;
	CxList sanitize = Find_LDAP_Sanitize(); 
	CxList outputs = Find_LDAP_Outputs();

	result = outputs.InfluencedByAndNotSanitized(inputs, sanitize);
}