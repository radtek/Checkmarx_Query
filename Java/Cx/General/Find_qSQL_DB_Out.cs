CxList methods = Find_Methods();

CxList qSqlQuery = methods.FindByMemberAccess("QSqlQuery.*");
List <string> rawQueryMethods = new List<string>{"result", "record"};
CxList queryMethods = qSqlQuery.FindByShortNames(rawQueryMethods);

CxList assignees = queryMethods.GetAssignee();
CxList methodsWithoutAssignee = queryMethods - assignees.GetAssigner();

result.Add(assignees);
result.Add(methodsWithoutAssignee);