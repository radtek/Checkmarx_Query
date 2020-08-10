//查找修饰符为@safeoutputs下的方法（MethodDecl）、类（ClassDecl）、类的成员变量（FieldDecl）
CxList Calssmethods = All.FindByType(typeof(ClassDecl));
CxList FieldDecls = All.FindByType(typeof(FieldDecl));
CxList methods = All.FindByType(typeof(MethodDecl));
CxList Allmethod = Calssmethods + FieldDecls + methods;
//FindByCustomAttribute()按自定义属性查找
result = (All.FindByCustomAttribute("SafeOutputs")).GetByAncs(Allmethod);