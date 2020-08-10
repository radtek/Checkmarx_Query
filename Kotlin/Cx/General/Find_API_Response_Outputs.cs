CxList members = Find_MemberAccesses();
CxList methods = Find_Methods();
CxList ktorOutputs = Find_Ktor_Outputs();
CxList strings = Find_Strings();
CxList unkRefs = Find_UnknownReference();

// By default any .respond call will have text/plain as contentType
CxList ktorRespond = ktorOutputs.FindByShortName("respond");
result.Add(ktorRespond);

// Find responses which explicitly set the right content-type 
CxList jsonContentTypes = members.FindByMemberAccess("ContentType.Application")
	.GetMembersOfTarget().FindByShortNames(new List<string>{"Json", "Xml"});
jsonContentTypes.Add(members.FindByMemberAccess("ContentType.Text")
	.GetMembersOfTarget().FindByShortNames(new List<string>{"JavaScript", "Xml", "Plain"}));

result.Add(ktorOutputs.FindByParameters(jsonContentTypes));

// Vertx api outputs (either marked with known content-type or returning json)
List<string> jsonContentTypesStr = new List<string> {
		"application/json", "application/xml",
		"text/javascript", "text/xml", "text/plain"
		};

CxList sanitizerParams = strings.FindByShortNames(jsonContentTypesStr, false);
CxList putHeaderMethod = sanitizerParams.GetAncOfType(typeof(MethodInvokeExpr)).FindByShortName("putHeader");
sanitizerParams = sanitizerParams.GetByAncs(putHeaderMethod);


CxList vertxOutputs = Find_Vertx_Outputs();
CxList jsonObjects = methods.FindByMemberAccesses(new string[] {"Json.obj", "Json.array"});
sanitizerParams.Add(jsonObjects);
sanitizerParams.Add(unkRefs.FindByTypes(new string[] {"JsonObject", "JsonArray"}));

// Heuristic: classes marked with "DataObject" attribute can have a toJson method 
// which is detected by Vertx as a serializer
sanitizerParams.Add(methods.FindByShortName("toJson"));

// Nodes of the fluent api call after the sanitizer
CxList vertxApi = vertxOutputs.DataInfluencedBy(sanitizerParams)
	.GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly);
// Nodes of the fluent api call before the sanitizer
vertxApi.Add(sanitizerParams.GetFathers().GetAncOfType(typeof(MethodInvokeExpr))
	.DataInfluencedBy(vertxOutputs).GetStartAndEndNodes(CxList.GetStartEndNodesType.StartNodesOnly));

result.Add(vertxApi);