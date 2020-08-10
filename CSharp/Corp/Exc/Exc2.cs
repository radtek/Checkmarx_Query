//问题：查找被调用的方法定义的地方
CxList newall = All.FindByFileName(@"*Methods\src\Exc3.cs");
//被调用函数的类型为：MethodInvokeExpr
CxList method = newall.FindByType(typeof(MethodInvokeExpr));
//FindDefinition(CxList)该方法为查找CxList元素里定义的地方
result = newall.FindDefinition(method);