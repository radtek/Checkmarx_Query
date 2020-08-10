CxList dbOut = Find_DB_Out();
CxList dbIn = Find_SQL_DB_In();

CxList sanitize = Find_Sanitize();
sanitize.Add(Find_Sanitize_Django_ORM());

result = dbOut.InfluencingOnAndNotSanitized(dbIn, sanitize);