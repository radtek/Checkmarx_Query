CxList fields = Find_Field_Decl();
CxList methods = Find_MethodDeclaration();
CxList Classes = Find_Classes();

//for each class at a time
foreach(CxList singleClass in Classes){
	
	CxList fieldsVariables = fields.GetByAncs(singleClass);
	CxList methodsInClass = methods.GetByAncs(singleClass);
	
	foreach(CxList curField in fieldsVariables)
	{
		CSharpGraph graph = curField.TryGetCSharpGraph<CSharpGraph>();
		CxList methodWithSameName = methodsInClass.FindByName(graph.FullName);
		if(methodWithSameName.Count > 0)
		{
			result.Add(methodWithSameName.ConcatenateAllSources(curField));
				
		}
	}
}