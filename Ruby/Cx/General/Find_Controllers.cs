CxList baseClass = All.FindByType(typeof(TypeRef)).FindByShortName("Base");
CxList classes = All.FindByType(typeof(ClassDecl));

CxList classDecl = All.NewCxList();
foreach(CxList a in baseClass)
{
	TypeRef t = a.TryGetCSharpGraph<TypeRef>();
	string type = t.TypeName;
	if(type.Equals("ActionController.Base"))
	{
		classDecl.Add(a.GetFathers().GetFathers().FindByType(typeof(ClassDecl)));
	}
}

CxList controllers = All.NewCxList();
controllers.Add(classDecl);
foreach(CxList cla in classDecl)
{
	CSharpGraph c = cla.GetFirstGraph();
	controllers.Add(classes.InheritsFrom(c.ShortName));
}

result = controllers;