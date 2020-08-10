if(Find_Import().FindByName("org.apache.ibatis.*").Count > 0){
	
	CxList invokes = Find_Methods();
	CxList customAtts = Find_CustomAttribute().FindByShortName("Select");

	result = invokes.FindAllReferences(customAtts.GetAncOfType(typeof(MethodDecl)));
}