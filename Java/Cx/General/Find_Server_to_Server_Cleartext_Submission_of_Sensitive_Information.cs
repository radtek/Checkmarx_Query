if (param.Length == 1)
{
	// Inputs
	CxList personalInfo = param[0] as CxList;
	CxList sanitizerTestCode = Find_UnitTest_Code();
	personalInfo -= sanitizerTestCode;
	
	// Remove static values 
	CxList statics = personalInfo.FindByName("org.opends.server.*");
	statics = statics.FindByFieldAttributes(Modifiers.Sealed).FindByFieldAttributes(Modifiers.Static);
	personalInfo -= All.FindAllReferences(statics);

	CxList pureHTTP = Find_Pure_http();
	pureHTTP.Add(All.FindByType("HttpURLConnection"));
	
	// Pure HTTP as outputs	
	CxList write = Find_Write();
	write.Add(Find_Request());
	CxList sanitizerWrite = All.FindByMemberAccess("FileWriter.write*");
	sanitizerWrite.Add(All.FindByMemberAccess("PrintWriter.*"));
	sanitizerWrite.Add(All.FindByMemberAccess("FileOutputStream.*"));
	write -= sanitizerWrite;
	CxList outputs = write * write.DataInfluencedBy(pureHTTP);

	// Remote requests as outputs
	CxList remoteRequests = Find_Remote_Requests();
	remoteRequests -= Find_XXE_Requests();

	// Sanitizers
	CxList sanitizers = All.NewCxList();
	sanitizers.Add(All.FindByShortName("ssl*", false));
	sanitizers.Add(Find_Strings().FindByShortName("https://*", false));	
	sanitizers.Add(Find_DB_In());
	sanitizers.Add(Find_Dead_Code_Contents());
	sanitizers.Add(Find_Encrypt());
	sanitizers.Add(All.FindByType("HttpsURLConnection"));
	sanitizers.Add(sanitizerTestCode);
	sanitizers.Add(Find_CollectionAccesses());

	CxList sanitized = remoteRequests.DataInfluencedBy(sanitizers);	
	CxList nonSanitizedOutputs = remoteRequests - sanitized;
	outputs.Add(nonSanitizedOutputs);
	
	// Sanitize some parameters
	CxList vars = Find_UnknownReference().FindByShortNames(new List<string> {"size","len","*length", "intValue","Index*", "*offset"}, false);
	CxList sanitizedParams = vars.GetByAncs(Find_Params().GetParameters(outputs));
	sanitizedParams += Find_UnknownReference().GetByAncs(sanitizerTestCode);

	result = outputs.InfluencedByAndNotSanitized(personalInfo, sanitizedParams);
	result = result.ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow);
}