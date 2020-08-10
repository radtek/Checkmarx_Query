if (ReactNative_Find_Presence().Count > 0) { 
	
	CxList unkRefs = Find_UnknownReference();
	CxList fieldDecls = Find_FieldDecls();
	CxList stringLiterals = Find_String_Literal();
	CxList outputs = All.NewCxList();
	
	CxList insecureStrings = stringLiterals.FilterByDomProperty<StringLiteral>(x => x.Value.StartsWith("http://"));
	
	// Find all Axios references
	CxList axiosRequire = Find_Require("axios");
	CxList axiosInstances = axiosRequire.GetMembersOfTarget().FindByShortName("create");
	CxList axiosUnkRefs = unkRefs.FindAllReferences(axiosInstances.GetAssignee());
	axiosRequire.Add(axiosUnkRefs);
	
	// Find all Axios method uses
	var axiosRequestMethods = new List<string>{"get", "put", "post", "delete", "head", "options", "patch"};
	CxList axiosMethods = axiosRequire.GetMembersOfTarget().FindByShortNames(axiosRequestMethods);
	outputs.Add(axiosMethods);
	
	// Find all fetch() and XMLHttpRequest.Open methods
	outputs.Add(Find_Methods().FindByShortName("fetch"));
	outputs.Add(Find_XmlHttp_Open());
	
	// Get the insecure URLs
	CxList unkRefsParams = unkRefs.GetParameters(outputs);
	CxList insecureURLs = insecureStrings.DataInfluencingOn(unkRefsParams);
	insecureURLs.Add(insecureStrings.GetByAncs(All.GetParameters(outputs)));
	
	result.Add(insecureURLs);

	// Find URI fields in WebViews and Images
	var components = new List<string>{"WebView", "Image"};
	CxList componentsObjects = Find_ObjectCreations().FindByShortNames(components);
	CxList sourceFields = fieldDecls.GetByAncs(componentsObjects).FindByShortName("source");
	CxList uriFields = fieldDecls.GetByAncs(sourceFields).FindByShortName("uri");
	CxList insecureURI = insecureStrings.DataInfluencingOn(All.FindAllReferences(uriFields));
	
	result.Add(insecureURI);
}