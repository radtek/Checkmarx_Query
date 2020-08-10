if(Find_Import().FindByName("org.apache.ibatis.*").Count > 0){
	
	CxList invokes = Find_Methods();
	CxList unknowReferences = Find_UnknownReference();
	CxList strings = Find_String_Literal();

	var sql_Commands = new List<string>{"Select","Insert","Delete","Update"};
	CxList customAtts = Find_CustomAttribute().FindByShortNames(sql_Commands);

	CxList stringsInOperation = strings.GetByAncs(customAtts);
	stringsInOperation.Add(strings.GetByAncs(All.FindDefinition(unknowReferences.GetByAncs(customAtts))));

	stringsInOperation = stringsInOperation.FindByShortName(@"*#{*");

	CxList paramsOfCommands = Find_ParamDecl().GetParameters(customAtts.GetAncOfType(typeof(MethodDecl)));

	CxList sanitizedParameters = All.NewCxList();
	CxList unsafeParameters = All.NewCxList();
	
	foreach(CxList parameter in paramsOfCommands){
	    
		int position = parameter.GetIndexOfParameter(); 
		var safePatterns = new List<string>{"*#{" + parameter.GetName() + "}*","*#{" + position + "}*"};
		var unsafePattern = "*${*";

		CxList method = parameter.GetAncOfType(typeof(MethodDecl));
		CxList stringCommand = stringsInOperation.GetByAncs(method);
		stringCommand.Add(stringsInOperation.GetByAncs(All.FindDefinition(unknowReferences.GetByAncs(customAtts.FindByFathers(method)))));
		
		CxList safeReplacements = stringCommand.FindByShortNames(safePatterns);
		CxList unsafeReplacements = stringCommand.FindByShortName(unsafePattern);
		
		if( safeReplacements.Count > 0){
			sanitizedParameters.Add(All.GetParameters(invokes.FindAllReferences(method), position));
		}
		
		if( unsafeReplacements.Count > 0){
			unsafeParameters.Add(All.GetParameters(invokes.FindAllReferences(method), position));
		}
		
		if( safeReplacements.Count == 0 && unsafeReplacements.Count == 0 && stringCommand.Count > 0){
			sanitizedParameters.Add(All.GetParameters(invokes.FindAllReferences(method), position));
		}
				
	}

	result = sanitizedParameters - unsafeParameters;
}