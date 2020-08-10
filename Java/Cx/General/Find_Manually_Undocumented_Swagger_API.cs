CxList customAttributes = Find_CustomAttribute();
CxList methodDecls = Find_MethodDeclaration();
CxList classDecls = Find_Class_Decl();

//	 Find if manual specification exists (Swagger or OpenAPI)

CxList swaggerSpec = cxJson.FindJsonPropertyByName("*.json", 2, "swagger", false, true);
CxList openApiSpec = cxJson.FindJsonPropertyByName("*.json", 2, "openapi", false, true);

if (swaggerSpec.Count > 0 || openApiSpec.Count > 0)
{
	//	 Build API paths from specification
		
	List<string> documentedEndpoints = new List<string>();
	
	CxList endpoints = cxJson.FindChildPropertiesByJsonPath("*.json", 2, "paths");
				
	foreach (CxList endpoint in endpoints)
	{
		string jsonPath = "paths['" + endpoint.GetName() + "']";
		CxList endpointRequests = cxJson.FindChildPropertiesByJsonPath("*.json", 2, jsonPath);
		
		foreach (CxList endpointRequest in endpointRequests)
		{
			string docEndpoint = endpointRequest.GetName().ToUpper() + endpoint.GetName();
			documentedEndpoints.Add(docEndpoint);
		}
	}
	
	//	 Build API paths from annotations (@Path and @RequestMapping annotations)
		
	var endpointsPaths = new Dictionary<string, CxList>();
	
	var pathAnnotationsNames = new List<string>() {"RequestMapping", "Path"};
	CxList pathAnnotations = customAttributes.FindByShortNames(pathAnnotationsNames);
	CxList classesWithPath = pathAnnotations.GetFathers() * classDecls;
	
	CxList requestAnnotations = Find_Frameworks_Inputs_Annotations();
	
	// get the path from the class
	
	foreach (CxList classDecl in classesWithPath)
	{
		CxList classPathAnnotation = pathAnnotations.GetByAncs(classDecl);

		string classPath = classPathAnnotation.FindCustomAttributeParameters("RequestMapping").Count > 0 ?
			classPathAnnotation.FindCustomAttributeParameters("RequestMapping").GetName() :
			classPathAnnotation.FindCustomAttributeParameters("Path").GetName();
		
		CxList classMethods = methodDecls.GetByAncs(classDecl);
		
		// get the path for each method and its request type (GET, POST, etc..)
		
		foreach (CxList method in classMethods)
		{
			CxList methodPathAnnotation = customAttributes.GetByAncs(method);
			
			string requestType = requestAnnotations.GetByAncs(method).GetName();
			string pathParam = string.Empty;
			string request = string.Empty;
				
			if (requestType.Equals("GetMapping"))
			{
				pathParam = methodPathAnnotation.FindCustomAttributeParameters(requestType).GetName();
				request = "GET";
			}
			else if (requestType.Equals("PutMapping"))
			{
				pathParam = methodPathAnnotation.FindCustomAttributeParameters(requestType).GetName();
				request = "PUT";
			}
			else if (requestType.Equals("PostMapping"))
			{
				pathParam = methodPathAnnotation.FindCustomAttributeParameters(requestType).GetName();
				request = "POST";
			}
			else if (requestType.Equals("DeleteMapping"))
			{
				pathParam = methodPathAnnotation.FindCustomAttributeParameters(requestType).GetName();
				request = "DELETE";
			}
			else if (requestType.Equals("RequestMapping"))
			{
				pathParam = methodPathAnnotation.FindCustomAttributeParameters(requestType).GetName();
				request = methodPathAnnotation.FindCustomAttributeParameterByKey(requestType, "method").GetName();
			}
			else if (requestType.Equals("GET") || requestType.Equals("PUT") ||
				requestType.Equals("POST") || requestType.Equals("DELETE"))
			{
				pathParam = methodPathAnnotation.FindCustomAttributeParameters("Path").GetName();
				request = requestType;
			}
				
			if (!string.IsNullOrEmpty(request))
			{
				string path = request + classPath + pathParam;
				endpointsPaths[path] = method.Clone();
			}
		}
	}
		
	// Compare documented and actual paths
		
	foreach (var docEndpoint in documentedEndpoints)
	{	
		if (endpointsPaths.ContainsKey(docEndpoint))
			endpointsPaths.Remove(docEndpoint);
	}
		
	result.AddRange(endpointsPaths.Values);
}