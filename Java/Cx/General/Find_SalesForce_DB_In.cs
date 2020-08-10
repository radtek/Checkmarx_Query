CxList methods = Find_Methods();

CxList sForceService = methods.FindByMemberAccess("SForceService.*");
List <string> rawQueryMethods = new List<string>{"query", "queryAll", "search"};
CxList sForceServiceMethods = sForceService.FindByShortNames(rawQueryMethods);
result = sForceServiceMethods;