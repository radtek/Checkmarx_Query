CxList unknownReference = Find_UnknownReference();
CxList methods = Find_Methods();
CxList possibleRequests = unknownReference;
possibleRequests.Add(methods);

List<string> requestMembers = new List<string>{
	"getFromAttribute", "getParam", "params", "query",
	"getHeader", "headers", "getCookie", "cookieMap"
};
List<string> routingContextMembers = new List<string>{
	"queryParams", "queryParam",
	"getBody", "body",
	"getBodyAsJson", "bodyAsJson",
	"getBodyAsJsonArray", "bodyAsJsonArray",
	"getBodyAsString", "bodyAsString"
};

// gets the routingContext from the handler
CxList lambdaParam = All.GetParameters(methods.FindByShortName("handler"));
CxList routingContexts = All.GetParameters(lambdaParam);
routingContexts = unknownReference.FindAllReferences(routingContexts);
routingContexts.Add(unknownReference.FindByShortName("it").GetByAncs(lambdaParam));
result.Add(routingContexts.GetMembersOfTarget().FindByShortNames(routingContextMembers));
result.Add(routingContexts.GetRightmostMember().FindByShortNames(requestMembers));

// gets the httpServerResponses
CxList httpServerResponses = possibleRequests.FindByShortName("request");
httpServerResponses = httpServerResponses.GetMembersOfTarget().FindByShortNames(requestMembers);
result.Add(httpServerResponses);