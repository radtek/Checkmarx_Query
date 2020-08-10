CxList methods = Find_Methods();

CxList soapBindingStub = methods.FindByMemberAccess("SoapBindinbStub.*");
List <string> rawQueryMethods = new List<string>{"query", "queryAll", "search"};
CxList soapBindingStubMethods = soapBindingStub.GetMembersOfTarget().FindByShortNames(rawQueryMethods);
result = soapBindingStubMethods;