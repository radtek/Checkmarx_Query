if (Find_Device_Root_Verification().Count == 0)
{
	CxList methodDecls = Find_MethodDecls();
	CxList mainMethods = methodDecls.FindByShortName("main");
	CxList methods = All.NewCxList();
	
	if(mainMethods.Count > 0)
	{
		// Try to place the item in a main method
		methods.Add(mainMethods);
	}
	else
	{
		// Try to place it in an activity
		CxList classes = Find_Classes();
		CxList activities = methodDecls.GetByAncs(classes.FindByShortName("*activity", false));
		methods.Add(activities);
	}
	
	if(methods.Count == 0)
	{
		methods = methodDecls;
	}
	
	MethodDecl firstMethod = methods.TryGetCSharpGraph<MethodDecl>();
	result.Add(firstMethod.DomId, firstMethod);
	
}