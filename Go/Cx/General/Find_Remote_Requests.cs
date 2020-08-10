// from https://golang.org/pkg/net/http/ package
// Package url parses URLs and implements query escaping.

CxList unkRefs = Find_UnknownReference();
CxList httpImport = All.FindByMemberAccess("net/http.*");
CxList httpRequests = (All.FindByType("http.request", false));
List<string> requestMethods = new List<string>(){"Get","Head","Post","PostForm"};
CxList netHttpInputs = httpImport.FindByShortNames(requestMethods);

//http.Request.URL.Query().Get
CxList urls = httpRequests.GetMembersOfTarget().FindByShortName("URL", false);
urls.Add(unkRefs.FindAllReferences(urls.GetAssignee()));
CxList queries = urls.GetMembersOfTarget().FindByShortName("Query", false);
queries.Add(unkRefs.FindAllReferences(queries.GetAssignee()));
CxList requestMethodsNames = queries.GetMembersOfTarget().FindByShortNames(requestMethods);

netHttpInputs.Add(requestMethodsNames);

CxList httpRequester = httpImport.FindByShortNames(new List<string>{"Client","Response"});
// eg: catch also &http.Client
httpRequester.Add(httpRequester.GetFathers());

CxList requestOcurrences = All.FindAllReferences(httpRequester.GetAssignee());
List<string> clientMemberNames = new List<string>(){"Do", "Get", "Head", "Post", "PostForm"};
CxList clientMembers = requestOcurrences.GetMembersOfTarget().FindByShortNames(clientMemberNames);
netHttpInputs.Add(clientMembers.ReduceFlowByPragma());

// Grab http.Client and tls.Conn pointers (usually function arguments)
CxList variables = Find_UnknownReferences();
CxList clientType = variables.FindByPointerTypes(new string[] {"http.Client", "tls.Conn"});
CxList allClientTypeMembers = clientType.GetMembersOfTarget();
netHttpInputs.Add(allClientTypeMembers);

// from https://golang.org/pkg/net/url/ package
// Package url parses URLs and implements query escaping.
CxList netUrlInputs = All.FindByMemberAccess("net/url.*").FindByShortNames(new List<string>{"Parse","ParseRequestURI"});

// from https://golang.org/pkg/net/ package
// Package url parses URLs and implements query escaping.
CxList netInputs = All.NewCxList();

List<string> netMembers = new List<string> {
		"SplitHostPort","Lookup*",
		"Dial","DialIP","DialTCP","DialUDP"
		};

netInputs.Add(All.FindByMemberAccess("net.*").FindByShortNames(netMembers));

CxList muxLibraryInputs = All.FindByMemberAccess("github.com/gorilla/mux.*").FindByShortName("Vars");

result.Add(netHttpInputs);
result.Add(netUrlInputs);
result.Add(netInputs);
result.Add(muxLibraryInputs);