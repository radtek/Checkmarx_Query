if(cxScan.IsFrameworkActive("Angular")) {
	CxList methods = Find_Methods();
	
	CxList inputs = Find_Inputs();
	CxList outputs = Angular_Find_Outputs_XSS();
	CxList desanitizer = methods.FindByShortName("bypassSecurityTrust*");

	CxList possibleXSS = outputs.DataInfluencedBy(inputs);
	result = possibleXSS.IntersectWithNodes(desanitizer);
}