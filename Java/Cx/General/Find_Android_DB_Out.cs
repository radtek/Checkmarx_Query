CxList methods = Find_Methods();
CxList SQLiteDB = methods.FindByMemberAccess("SqLiteDataBase.*");
CxList queryMethods = All.NewCxList();
List<string> queryMethodsName = new List<string>{"query*", "rawQuery*"};
queryMethods.Add(SQLiteDB.FindByShortNames(queryMethodsName));

CxList assignees = queryMethods.GetAssignee();
CxList methodsWithoutAssignee = queryMethods - assignees.GetAssigner();

result.Add(assignees);
result.Add(methodsWithoutAssignee);