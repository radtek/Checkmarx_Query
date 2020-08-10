if(cxScan.IsFrameworkActive("Angular")) {
	CxList paramType = Find_Parameters();
	CxList pipes = Angular_Find_Pipes();
	
	//remove pipes that we don't know the definition of first param
	CxList toRemove = All.NewCxList();
	foreach(CxList pipe in pipes){
		CxList firstParam = All.GetParameters(pipe, 0) - paramType;
		CxList def = All.FindDefinition(firstParam);
		if(def.Count == 0){
			toRemove.Add(pipe);
		}
	}
	pipes -= toRemove;
	
	Func < CxList, string[], CxList > filterByDeclaredTypes = (searchSpace, types) => {
		CxList filtered = All.NewCxList();
		CxList definitions = All.FindDefinition(searchSpace);
		
		foreach( string t in types) {
			filtered.Add(definitions.FindByRegex(@"((\s*:\s*)|\s*=\s*new\s*)" + t));
		}
		return searchSpace.FindAllReferences(filtered);
		};

	Func<string[], string[], CxList> improperTypeFinder = (pipeClassNames, expectedTypeNames) =>
		{
		CxList pipeCalls = All.NewCxList();
		foreach (var pipeClassName in pipeClassNames)
		{
			pipeCalls.Add(pipes.FindByShortName(pipeClassName));
		}
		
		CxList firstParam = All.GetParameters(pipeCalls, 0) - paramType;	
		CxList isExpectedType = filterByDeclaredTypes(firstParam, expectedTypeNames);
		CxList isNotExpectedType = firstParam - isExpectedType;
	
		return isNotExpectedType;
		};

	string notArray = @"[^\[\n]*$";
	//The DecimalPipe is used with identifier "number"
	result.Add(improperTypeFinder(new string[] { "currency", "number", "percent", "i18nPlural" },
		new string[] { "number" + notArray}));

	result.Add(improperTypeFinder(new string[] { "i18nSelect", "lowercase", "uppercase", "titlecase" },
		new string[] { "string" + notArray }));

	result.Add(improperTypeFinder(new string[] { "async" },
		new string[] { "Promise", "Observable" }));

	result.Add(improperTypeFinder(new string[] { "date" },
		new string[] { "Date" + notArray, "string" + notArray, "number" + notArray }));

	result.Add(improperTypeFinder(new string[] { "slice" },
		new string[] { "string", "Array", @"\w+\[" }));
}