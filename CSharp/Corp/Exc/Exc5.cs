//找到自己调用自己的方法定义的地方
CxList newAll = All.FindByFileName(@"*recursive\src\Exc6.cs");
CxList methods = newAll.FindByType(typeof(MethodDecl));
foreach(CxList method in methods){
	CxList Expr = newAll.FindDefinition(newAll.GetByAncs(method).FindByType(typeof(MethodInvokeExpr)));
	//*号表示获取交集的部分
	if((method * Expr).Count > 0){
		result.Add(method);
	}
}
//基本思路：1、获取所以方法；2、遍历方法，3、获取方法体内的调用方法找到定义的位置，4、获取2步骤和3步骤结果的交集大于0表示自己调用了自己。