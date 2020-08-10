if(cxScan.IsFrameworkActive("Angular")) {
	CxList methods = Find_Methods().FindByFileName("*.html");
	result.Add(methods.FindByShortName("date"));
	result.Add(methods.FindByShortName("currency"));
	result.Add(methods.FindByShortName("number"));
	result.Add(methods.FindByShortName("percent"));
	result.Add(methods.FindByShortName("i18nPlural"));
	result.Add(methods.FindByShortName("i18nSelect"));
	result.Add(Find_ViewEscapedOutputStmt());
}