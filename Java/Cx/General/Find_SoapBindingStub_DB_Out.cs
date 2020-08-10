CxList methods = Find_Methods();

CxList soapBindingStub = methods.FindByMemberAccess("SoapBindinbStub.*");
List<string> rawQueryMethods = new List<string>{"query", "queryAll", "queryMore", "search"};
CxList queryMethods = soapBindingStub.GetMembersOfTarget().FindByShortNames(rawQueryMethods);

CxList assignees = queryMethods.GetAssignee();
CxList methodsWithoutAssignee = queryMethods - assignees.GetAssigner();

result.Add(assignees);
result.Add(methodsWithoutAssignee);