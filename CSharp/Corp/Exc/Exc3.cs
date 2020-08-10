//查找死代码：意思是找出未调用的函数或方法。
CxList newall = All.FindByFileName(@"*Code\src\Exc3.cs");
//MethodDecl类型为定义方法。
CxList methods = newall.FindByType(typeof(MethodDecl));
//MethodInvokeExpr类型为被调用的方法。
CxList methodExpr = newall.FindByType(typeof(MethodInvokeExpr));
//FindDefinition()需要找到被调用方法的定义位置，才能进行运算。
CxList methodExpr_location = newall.FindDefinition(methodExpr);
//使用所以定义的方法减去被调用的方法即可得出没有被调用的方法，即死代码。
result = methods - methodExpr_location;