// Cross Site Scripting when using the Common Gateway Interface 
// and the input came from The request call

CxList inputs = All.NewCxList();
CxList outputs = All.NewCxList();
CxList sanitize = All.NewCxList();

List<string> cgiMethods = new List<string>{"Request","RequestFromMap"};
inputs = All.FindByMemberAccess("net/http/cgi.*").FindByShortNames(cgiMethods);
inputs.Add(All.FindByMemberAccess("http.Request", "FormValue"));
CxList httpRequest = All.FindByType("http.Request");
CxList httpRequestFields = httpRequest.GetMembersOfTarget() - Find_Methods();	
inputs.Add(httpRequestFields);
	
outputs = Find_XSS_Outputs();	
outputs.Add(Find_Console_Outputs());

sanitize = Find_XSS_Sanitize();

result = All.FindXSS(inputs, outputs, sanitize);