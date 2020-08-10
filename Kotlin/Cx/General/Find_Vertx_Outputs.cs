CxList unknownReference = Find_UnknownReference();
CxList methods = Find_Methods();
CxList unkRefs = Find_UnknownReference();

List<string> responseMembers = new List<string>{
		"end", 
		"endAwait",
		"push", 
		"pushAwait", 
		"putHeader", 
		"sendFile", 
		"sendFileAwait", 
		"setStatusMessage", 
		"write", 
		"writeAwait"};

// gets the routingContext from the handler
CxList routingContexts = All.GetParameters(All.GetParameters(methods.FindByShortName("handler")));
// add requesthandler handler
routingContexts.Add(All.GetParameters(All.GetParameters(methods.FindByShortName("requestHandler"), 0)));
routingContexts = unknownReference.FindAllReferences(routingContexts);

CxList httpResponses = methods.FindAllReferences(routingContexts.GetMembersOfTarget().FindByShortName("response"));
httpResponses.Add(unkRefs.FindAllReferences(httpResponses.GetAssignee()));

CxList methodsInvokesInHttpResponses = methods.FindByShortNames(responseMembers)
	.InfluencedBy(httpResponses).GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly);

result.Add(methodsInvokesInHttpResponses);