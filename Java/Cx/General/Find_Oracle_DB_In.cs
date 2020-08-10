string[] types = new string[]{"Statement", "*.Statement", "PreparedStatement*", "*.PreparedStatement" };
CxList statement = All.FindByTypes(types);
List<string> statementMethods = new List<string>{"execute*", "getMoreResults", "getResultSet"};

CxList callableStatements = All.NewCxList();
callableStatements.Add(All.FindByType("CallableStatement"));
callableStatements.Add(All.FindByType("*.CallableStatement"));
List<string> callableStatementMethods = new List<string>{"execute*", "get*"};
	
result = statement.GetMembersOfTarget().FindByShortNames(statementMethods);
result.Add(callableStatements.GetMembersOfTarget().FindByShortNames(callableStatementMethods));