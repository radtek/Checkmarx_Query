//找到函数（方法）体内语句大于9的所以函数（方法）。
CxList newall = All.FindByFileName(@"*stmnts\src\Exc4.cs");
CxList methods = newall.FindByType(typeof(MethodDecl));
foreach(CxList method in methods){
	//ExprStmt类型为函数体的语句，占一行（特殊符合单独一行默认不计）。
	CxList exprstms = newall.GetByAncs(method).FindByType(typeof(ExprStmt));
	if(exprstms.Count >= 9){
		result.Add(method);
	}
}
//基本思路：把所有的方法找到，再遍历获取每个方法，使用ExprStmt类型获取方法体的语句数，大于9的添加输出。