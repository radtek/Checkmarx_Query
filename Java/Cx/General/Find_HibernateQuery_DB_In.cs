CxList methods = Find_Methods();
CxList queryCommands = Get_Query().GetMembersOfTarget();
CxList query = methods.FindByMemberAccess("Query.*");

List<string> queryMethodNames = new List<string> {"iterate","list","scroll","uniqueResult"};

result.Add(query.FindByShortNames(queryMethodNames));
result.Add(queryCommands.FindByShortNames(queryMethodNames));