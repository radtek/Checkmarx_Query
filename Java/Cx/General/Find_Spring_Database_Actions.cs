CxList methods = All.FindByType(typeof(MethodDecl));
List<string> springHibernate = new List<string>{"executeFind","executeWithNativeSession","executeWithNewSession","execute", "doExecute"};
CxList springmethods = methods.FindByShortNames(springHibernate);

List<string> modifierListforSpring = new List<string>{"save*","update*","delete*","alter*","update*", "createSessionProxy"};
CxList methodsInvokes = Find_Methods().FindByShortNames(modifierListforSpring);

result.Add(methodsInvokes.GetByAncs(springmethods));