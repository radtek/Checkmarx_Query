CxList db = base.Find_SQL_DB_In();
CxList inputs = base.Find_Interactive_Inputs();
CxList sanitized = base.Find_SQL_Sanitize();
//解决误报问题
sanitized.Add(All.FindByName("SQL_Injection.fix").FindByType(typeof(MethodInvokeExpr)));

//解决漏报问题
inputs.Add(All.FindByName("SQL_Injection.sqlQuery_4.req.getParameter") +
	All.FindAllReferences(All.FindDefinition(All.FindByName("SQL_Injection.sqlQuery_4.req.getParameter"))) +
	All.FindAllReferences(All.FindByName("SQL_Injection.sqlQuery_4.req.getParameter")));

result = inputs.InfluencingOnAndNotSanitized(db, sanitized);