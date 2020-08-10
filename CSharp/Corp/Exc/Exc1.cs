//找到以b开头的所有方法
CxList newall = All.FindByFileName(@"*B\src\T1.cs");
CxList methods = newall.FindByType(typeof(MethodDecl));
result = methods.FindByShortName(@"b*");