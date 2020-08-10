CxList view = All.FindByFileName(cxEnv.Path.Combine("*views", "*"));
view.Add(All.FindByFileName(cxEnv.Path.Combine("*view", "*")));
view.Add(All.FindByFileName("*.phtml")); 
view.Add(All.FindByFileName("*.phtm"));
view.Add(Find_Ctp_Files());
view.Add(Find_Twig());
result.Add(view);