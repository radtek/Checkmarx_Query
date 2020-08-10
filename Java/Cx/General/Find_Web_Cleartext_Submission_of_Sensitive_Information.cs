if (param.Length == 1)
{
	CxList personalInfo = param[0] as CxList;
	CxList allMethods = Find_Methods();
	
	// Remove user inputs
	personalInfo -= personalInfo.DataInfluencedBy(Find_Interactive_Inputs());

	// 1. HTTP connections
	CxList outputs = Find_Web_Outputs();
	CxList responses = Find_HTTP_Responses();
	CxList webOutputsParams = outputs.FindByType(typeof(Param));   
	CxList webOutputsMethods = allMethods.FindByParameters(webOutputsParams);
	
	CxList outputsResponse = All.NewCxList();
	outputsResponse.Add(outputs);
	outputsResponse.Add(webOutputsMethods);
	outputsResponse = outputsResponse.DataInfluencedBy(responses);

	// Check if it's in a secure channel
	CxList conditions = Find_Conditions();
	CxList isSecure = conditions.FindByMemberAccess("HttpServletRequest.isSecure");
	isSecure.Add(conditions.FindByMemberAccess("HTTPUtilities.isSecureChannel"));
	CxList secureIf = isSecure.GetFathers();
	outputsResponse -= outputsResponse.GetByAncs(secureIf);

	// Parameters of HttpResponse objects
	CxList allParams = All.GetParameters(webOutputsMethods);  
	CxList methods = outputsResponse.GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly); 
	CxList outputsResponseParams = allParams.GetParameters(methods);  

	// Relevant methods
	CxList outputRelevantMethods = methods - webOutputsMethods;
	CxList writeMethods = outputRelevantMethods.FindByShortNames(new List<string> {"write*", "print*", "append*"});
	outputRelevantMethods -= writeMethods;
	outputRelevantMethods.Add(All.GetParameters(writeMethods));

	// 2. Sockets
	CxList sockets = All.FindByTypes(new String[] {"Socket", "ServerSocket"});

	// Secure 
	CxList wrapSSL = All.FindByMemberAccess("SSLEngine.wrap");
	CxList wrap_param = All.FindAllReferences(All.GetParameters(wrapSSL, 1)); //Get output from wrap(passed by reference)

	// Outputs that use secure parameters
	CxList sanitized_outputs = wrap_param.DataInfluencingOn(outputs).GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly);

	// Sockets that influence outputs that don't have secure parameters
	CxList outputsNotSanitized = outputs - sanitized_outputs;
	CxList outputsSocket = sockets.DataInfluencingOn(outputsNotSanitized).GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly);

	// Define outputs
	outputs = outputRelevantMethods;
	outputs.Add(outputsResponseParams);
	outputs.Add(outputsSocket);

	// Anything that passes through the DB now has info from the DB and not the sensitive data
	CxList sanitize = Find_DB_In();
	sanitize.Add(Find_Dead_Code_Contents());
	sanitize.Add(Find_Encrypt());
	sanitize.Add(Find_UnitTest_Code());
	
	result = outputs.InfluencedByAndNotSanitized(personalInfo, sanitize);
	
	// Relevant parameters from HTTP methods 
	CxList jspResponseParams = outputsResponseParams * Find_Jsp_Code();
	CxList relevantParameters = (outputs - jspResponseParams) * personalInfo;
	CxList relevantJspParameters = jspResponseParams * personalInfo;
	relevantJspParameters = relevantJspParameters.ReduceFlowByPragma();
	relevantParameters.Add(relevantJspParameters);

	// Remove nodes that are part of outputs, but of type Param
	CxList realOutputs = result.GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly);
	CxList realOutputsAsParam = realOutputs.GetAncOfType(typeof(Param));
	result.Add(relevantParameters - sanitize - realOutputsAsParam);

	result = result.ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow);
}