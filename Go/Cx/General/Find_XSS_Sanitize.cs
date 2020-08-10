// Html Sanitizers
CxList htmlMethods = Find_Members_HTML();

// Find_HtmlTemplate_Sanitizers
// https://golang.org/pkg/html/template/
CxList htmlTemplateMethods = Find_Members_HTMLTemplate();
string[] templateCreation = new string[]{
	"html/template.New", "html/template.ParseFiles", "html/template.ParseGlob", "html/template.Must"
};
CxList htmlTemplateCreateMethods = All.FindByMemberAccesses(templateCreation);
CxList htmlTemplates = htmlTemplateCreateMethods.GetAssignee();
htmlTemplateMethods.Add(All.FindAllReferences(htmlTemplates).GetMembersOfTarget().FindByShortName("ExecuteTemplate"));

// Find_TextTemplate_Sanitizers
CxList textTemplateMethods = Find_Members_TextTemplate();

// Find_EncodingJson_Sanitizers
CxList encodingJson = Find_Members_Encoding();
CxList encodingJsonMethods = encodingJson.FindByShortName("HTMLEscape");	

CxList assignees = encodingJson.FindByShortName("NewEncoder").GetAssignee();
CxList references = All.FindAllReferences(assignees);
CxList members = references.GetMembersOfTarget().FindByShortName("SetEscapeHTML");

// Search for 'SetEscapeHTML' members that are influenced by true values:
// can be true constant, a boolean variable with true value or a funtion returning true
CxList trueTokens = All.FindByType(typeof(BooleanLiteral)).FindByName("true", true);
CxList influencedByTrue = members.InfluencedBy(trueTokens);
CxList reducedFlow = influencedByTrue.ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow);
// The end nodes are the 'SetEscapeHTML' members to find.
CxList endNodes = reducedFlow.GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly);

encodingJsonMethods.Add(endNodes);

CxList requests = Find_UnknownReferences().FindByPointerType("http.Request");
CxList httpRequestSanitizedMembers = requests.GetMembersOfTarget().FindByShortName("Host");

result = Find_Integers();
result.Add(htmlMethods);
result.Add(htmlTemplateMethods);
result.Add(textTemplateMethods);
result.Add(encodingJsonMethods);
result.Add(httpRequestSanitizedMembers);
result.Add(Find_WhiteListSanitizers());