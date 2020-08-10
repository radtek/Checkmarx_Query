if(cxScan.IsFrameworkActive("MyBatis"))
{
	CxList methods = Find_MethodDeclaration();
	CxList allMethods = Find_Methods();
	CxList allStrings = Find_Strings();
	
	CxList sessionMethods = allMethods.FindByMemberAccesses(new string[]{
		"SqlSession.insert","SqlSession.update","SqlSession.delete","SqlSession.select",
		"SqlSession.selectCursor","SqlSession.selectList","SqlSession.selectMap","SqlSession.selectOne"});

	foreach (CxList insert in sessionMethods)
	{
		CxList firstParam = allStrings.GetParameters(insert, 0);
		if(firstParam.Count == 0)
			continue;
	
		CxList secondParam = All.GetParameters(insert, 1);
		if(secondParam.Count == 0)
			continue;
	
		var methodName = firstParam.TryGetCSharpGraph<StringLiteral>().FullName.Trim(new char[]{'\"'});

		CxList insertDeclaration = methods.FindByName(methodName);
		CxList methodParam = All.GetParameters(insertDeclaration, 0);
		if(methodParam.Count == 0)	
			continue;
	
		CustomFlows.AddFlow(secondParam, methodParam);
	}
}