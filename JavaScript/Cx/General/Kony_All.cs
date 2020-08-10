if(cxScan.IsFrameworkActive("KonyInFF"))
{
	result.Add(All.FindByFileName(cxEnv.Path.Combine("*", "modules", "*")).FindByFileName(@"*.js"));
	result.Add(All.FindByFileName(cxEnv.Path.Combine("*", "forms", "*")).FindByFileName(@"*.json"));
}