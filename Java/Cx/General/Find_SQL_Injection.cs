if (param.Length == 1)
{

	CxList inputs = param[0] as CxList;
	CxList db = Find_SQL_DB_In();

	CxList sanitized = Find_SQL_Sanitize();

	result = inputs.InfluencingOnAndNotSanitized(db, sanitized);
}