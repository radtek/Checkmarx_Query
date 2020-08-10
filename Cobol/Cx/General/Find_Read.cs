CxList methods = Find_Methods();
CxList cicsMethods = methods.FindByShortName("EXEC_CICS");
CxList parameters = All.GetParameters(cicsMethods).FindByType(typeof(MethodInvokeExpr));
CxList fileInvokes = parameters.FindByShortNames(new List<string>{"FILE", "READ_FILE"});
CxList fileInvokesParameters = All.GetParameters(fileInvokes);
	
result = fileInvokesParameters;
result.Add(methods.FindByShortName("READ"));