CxList methods = Find_Methods();

// org.owasp.encoder.Encode
List<string> methodsOWASP = new List<string> {
	"forCssString",
	"forCssUrl",
	"forHtml",
	"forHtmlAttribute",
	"forHtmlUnquotedAttribute",
	"forXml",
	"forXmlAttribute"};
CxList owaspEncode = methods.FindByMemberAccess("Encode.for*").FindByShortNames(methodsOWASP);

result = owaspEncode;
result.Add(Find_Sanitize());
result.Add(Find_Dead_Code_AbsInt());
result.Add(Get_ESAPI().FindByMemberAccess("Encoder.encodeForSQL"));

// Sanitize Query, Insert, etc from exposed
CxList exposedIn = Find_Exposed_In();
List<string> sanMethodNames = new List<string>{
	"Query", "*InsertStatement", "*UpdateStatement",
	"*SelectStatement", "DeleteStatement"
};
CxList sanMethods = methods.FindByShortNames(sanMethodNames).GetByAncs(exposedIn);
result.Add(sanMethods.GetParameters(exposedIn));

CxList toRemove = Find_SQL_DB_In();
toRemove.Add(Find_DB());
toRemove.Add(Find_Plain_DB());

// Add vertx params (except first)
CxList vertxOutputs = Find_Vertx_DB_In();
CxList vertxAllParams = All.GetParameters(vertxOutputs);
CxList vertxFirstParam = All.GetParameters(vertxOutputs, 0);
result.Add(vertxAllParams - vertxFirstParam);

result -= toRemove;