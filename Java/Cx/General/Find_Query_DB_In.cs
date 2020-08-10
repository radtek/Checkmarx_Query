CxList methods = Find_Methods();
CxList queryCommands = Get_Query().GetMembersOfTarget();
CxList query = methods.FindByMemberAccess("Query.*");

List<string> queryMethodsName = new List<string> {"getSingleResult", "getResultList", "executeUpdate", "getFirstResult"};

result.Add(query.FindByShortNames(queryMethodsName));
result.Add(queryCommands.FindByShortNames(queryMethodsName));