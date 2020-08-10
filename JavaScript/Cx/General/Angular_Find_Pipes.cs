if(cxScan.IsFrameworkActive("Angular")) {
	CxList methods = Find_Methods();
	CxList customAttribute = Find_CustomAttribute();

	CxList pipesAttribute = customAttribute.FindByShortName("Pipe");
	result.Add(pipesAttribute.GetAncOfType(typeof(MethodInvokeExpr)));
}