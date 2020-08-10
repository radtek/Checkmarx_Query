Func<string, bool> isComponentNameInvalid = (s) =>
	{
	int pascalCaseWordCount = s.Count(c => char.IsUpper(c));
	int kebabCaseWordCount = s.Split('-').Count();
	int kebabTransformedWordCount = s.Split('_').Count();
		
	return(pascalCaseWordCount < 2 && kebabCaseWordCount < 2 && kebabTransformedWordCount < 2);
	};

if(cxScan.IsFrameworkActive("VueJS")){
	CxList vueComponents = All.NewCxList();
	CxList vueSingleComponents = All.NewCxList();
	CxList vueGlobalComponents = All.NewCxList();
	CxList vueModelComponents = All.NewCxList();
	CxList oneWordComponents = All.NewCxList();
	
	// get all components
	vueComponents = Find_UnknownReference().FindByShortName("cxVueCtx").GetAssigner(); 
	
	// get global components (childs of ViewModelComponent)
	vueGlobalComponents = vueComponents.GetByAncs(Find_ViewModelComponent()); 
	
	// get single file components
	vueSingleComponents = vueComponents - vueGlobalComponents; 
	vueModelComponents = vueComponents.GetAncOfType(typeof(ViewModelComponent));

	// -------------- single file components name check -----------------------
	CxList nameDeclarations = Find_FieldDecls().FindByFathers(vueSingleComponents).FindByShortName("name");
	
	// check file name on components without name
	CxList componentsWithoutName = vueSingleComponents - nameDeclarations.GetFathers(); 
	CxList nameValues = Find_String_Literal().GetByAncs(nameDeclarations);

	foreach(CxList component in nameValues){
		if(isComponentNameInvalid(component.GetName())){
			oneWordComponents.Add(component);
		}
	}

	foreach(CxList component in componentsWithoutName){
		try
		{
			CSharpGraph componentGraph = component.GetFirstGraph();
	
			string [] fileNamePath = componentGraph.LinePragma.FileName.Split(cxEnv.Path.DirectorySeparatorChar);
			string fileName = fileNamePath[fileNamePath.Length - 1];
			
			if(isComponentNameInvalid(fileName)){
				oneWordComponents.Add(component);
			}
		}
		catch
		{
		}
	}

	// ---------------- global components name check ----------------------------
	foreach(CxList component in vueModelComponents){
		if(isComponentNameInvalid(component.GetName())){
			oneWordComponents.Add(component);
		}
	}
	
	result = oneWordComponents;
}