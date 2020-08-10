string path = cxEnv.Path.Combine("WEB-INF", "web.xml");
result = All.FindByFileName($@"*{path}");