CxList methods = Find_Methods();

CxList qSqlQuery = methods.FindByMemberAccess("QSqlQuery.*");
List <string> rawQueryMethods = new List<string>{"exec", "execBatch"};
result = qSqlQuery.FindByShortNames(rawQueryMethods);