if(cxScan.IsFrameworkActive("MyBatis"))
{
	CxList sqlDB = Find_SQL_DB_In();
	CxList javaCode = All - Find_Jsp_Code();
	CxList sqlDbMethods = javaCode.GetMethod(sqlDB);
	CxList methods = Find_MethodDeclaration();
	CxList paramDecl = Find_ParamDeclaration();
	
	//Look for _parameter, present in select, update, insert and delete operations
	//ex. private select(...) {Object _parameter; ...}
	//Look for st, present in prepared statements
	//ex. private insert(...){ PreparedStatement st; }
	List<string> myBatisTempParams = new List<string> {"_parameter","st"};
	CxList parametersVars = Find_Declarators().FindByShortNames(myBatisTempParams); 
	CxList parametersMethods = parametersVars.GetMethod(sqlDbMethods);
	
	//Look for the parameters of the sql elements translation
	//ex. private string <name> (Statement st, Object ctx) { ... }
	CxList methodsReturningString = methods.FindByMethodReturnType("string");
	List<string> sqlMethodTempParams = new List<string> {"st","ctx"};
	CxList relevantSqlMethodsParams = paramDecl.FindByShortNames(sqlMethodTempParams);
	CxList relevantSqlMethods = methodsReturningString.FindByParameters(relevantSqlMethodsParams);
	
	CxList methodsReturningObj = methods.FindByMethodReturnType("Object");
	CxList relevantSqlParam = paramDecl.FindByShortName("_parameter");
	CxList relevantSqlMethod = methodsReturningObj.FindByParameters(relevantSqlParam);
	parametersVars.Add(relevantSqlParam);
	relevantSqlMethods.Add(relevantSqlMethod);
	
	CxList mybatisTempVar = parametersVars.GetByAncs(parametersMethods);
	mybatisTempVar.Add(relevantSqlMethodsParams.GetByAncs(relevantSqlMethods));
	
	result = mybatisTempVar.FindByFileName("*.xml");
}