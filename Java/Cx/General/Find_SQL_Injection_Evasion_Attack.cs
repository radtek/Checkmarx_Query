if (param.Length == 1)
{
	CxList inputs = param[0] as CxList;
	CxList decode = Find_Decode_SQL_Injection_Evasion_Attack()
		.DataInfluencedBy(inputs);

	CxList sanitize = Find_SQL_Sanitize();
	CxList db = Find_SQL_DB_In();

	result = db.InfluencedByAndNotSanitized(decode, sanitize);
}