string[] types = new string[]{"Statement", "*.Statement", "PreparedStatement*", "*.PreparedStatement" };
CxList statement = All.FindByTypes(types);
List<string> statementMethods = new List<string>{"execute*", "getMoreResults", "getResultSet"};

CxList callableStatements = All.NewCxList();
callableStatements.Add(All.FindByType("CallableStatement"));
callableStatements.Add(All.FindByType("*.CallableStatement"));
List<string> callableStatementMethods = new List<string>{"execute*", "get*"};


CxList queryMethods = statement.GetMembersOfTarget().FindByShortNames(statementMethods);
queryMethods.Add(callableStatements.GetMembersOfTarget().FindByShortNames(callableStatementMethods));


CxList assignees = queryMethods.GetAssignee();
CxList methodsWithoutAssignee = queryMethods - assignees.GetAssigner();

result.Add(assignees);
result.Add(methodsWithoutAssignee);