if(cxScan.IsFrameworkActive("Angular")) {
	CxList methods = Find_Methods();

	result.Add(methods.FindByMemberAccess("HttpClient.*"));
	result.Add(methods.FindByMemberAccess("HttpHandler.handle"));
	
	CxList viewOutputStmt = Find_ViewOutputStmt();
	viewOutputStmt.Add(Find_ViewEscapedOutputStmt());
	result.Add(All.GetByAncs(viewOutputStmt));
}