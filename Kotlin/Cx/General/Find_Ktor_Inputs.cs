CxList members = Find_MemberAccesses();
CxList unkRefs = Find_UnknownReference();
CxList methods = Find_Methods();

// HttpClient reads
string[] httpMethods = new string[]{"HttpClient.get", "HttpClient.put", "HttpClient.post", 
		"HttpClient.delete", "HttpClient.head", "HttpClient.patch", "HttpClient.options", "HttpClient.request"};
CxList httpReads = methods.FindByMemberAccesses(httpMethods);
result.Add(httpReads);
// WebSocket httpClient methods
CxList httpClientWs = methods.FindByMemberAccess("HttpClient.ws");
CxList wsReads = methods.FindByMemberAccess("incoming.receive");
result.Add(wsReads.GetByAncs(httpClientWs));

// ApplicationCall items
CxList callRequests = members.FindByMemberAccess("call.request");

CxList allReqRefs = All.NewCxList();
allReqRefs.Add(callRequests);
allReqRefs.Add(unkRefs.FindAllReferences(callRequests.GetAssignee()));

CxList requestMembers = allReqRefs.GetMembersOfTarget();

// Add members that can be accessed using [] (or get)
List<string> reqMembers = new List<string> {"headers", "queryParameters", "cookies"};
CxList inputMembers = requestMembers.FindByShortNames(reqMembers);
result.Add(inputMembers);
result.Add(inputMembers.GetMembersOfTarget());

// Add header methods names
List<string> headersMethodsNames = new List<string> {"accept*", "authorization", "cacheControl", "contentType", 
		"header", "host", "location", "path", "queryString", "userAgent", "receive"};
result.Add(requestMembers.FindByShortNames(headersMethodsNames));

// call.receiveParameters method
CxList recParamsGets = methods.FindByMemberAccess("call.receiveParameters").GetMembersOfTarget().FindByShortName("get");
// call.parameters array accesses
CxList parameterGets = members.FindByMemberAccess("call.parameters").GetMembersOfTarget().FindByShortName("get");

result.Add(recParamsGets);
result.Add(parameterGets);

// call.receive method
result.Add(methods.FindByMemberAccess("call.receive"));