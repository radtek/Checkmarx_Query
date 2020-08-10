if (param.Length == 1)
{

	CxList inputs = param[0] as CxList;

	CxList db = Find_SQL_DB_In();
	CxList db_not_in_try = Find_Improper_Exception_Handling(db);
	CxList db_in_try = db - db_not_in_try;
	CxList sanitized = Find_SQL_Sanitize();

	result = inputs.InfluencingOnAndNotSanitized(db_in_try, sanitized);
	result = result.ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow);
}