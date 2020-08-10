CxList methods = Find_Methods();

List<string> vulnMethods = new List<string>{"exec", "execInBatch"};

CxList transMethods = methods.FindByShortName("transaction");
CxList execMethods = methods.FindByShortNames(vulnMethods);

result.Add(execMethods.GetByAncs(transMethods));

CxList trManager = methods.FindByMemberAccess("TransactionManager.current*");
result.Add(trManager.GetMembersOfTarget().FindByShortNames(vulnMethods));