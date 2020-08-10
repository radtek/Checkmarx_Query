CxList methods = Find_Methods();

CxList sForceService = methods.FindByMemberAccess("SForceService.*");
List <string> rawQueryMethods = new List<string>{"query", "queryAll", "queryMore", "search"};
CxList queryMethods = sForceService.FindByShortNames(rawQueryMethods);

CxList assignees = queryMethods.GetAssignee();
CxList methodsWithoutAssignee = queryMethods - assignees.GetAssigner();

result.Add(assignees);
result.Add(methodsWithoutAssignee);