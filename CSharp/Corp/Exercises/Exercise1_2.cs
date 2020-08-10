result = Find_Methods().FindByShortName("input", false);
result.Add(All.FindByType(typeof(MethodDecl)).FindByShortName("input", false));