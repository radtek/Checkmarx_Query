if(Find_Import().FindByName("org.apache.ibatis.*").Count > 0){

	CxList invokes = Find_Methods();
	CxList unknowReferences = Find_UnknownReference();
	CxList strings = Find_String_Literal();

	var sql_Commands = new List<string>{"Select","Insert","Delete","Update"};
	CxList customAtts = Find_CustomAttribute().FindByShortNames(sql_Commands);
	CxList mapperMethods = customAtts.GetAncOfType(typeof(MethodDecl));

	result = All.GetParameters(invokes.FindAllReferences(mapperMethods));
}