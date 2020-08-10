CxList newAll = All.FindByFileName(@"*stmnts\src\Exc4.cs");
CxList AllMethodDecl = newAll.FindByType(typeof(MethodDecl));
foreach(CxList MethodDecl in AllMethodDecl){
	CxList Ex = newAll.GetByAncs(MethodDecl).FindByType(typeof(ExprStmt));
	if (Ex.Count >= 9 ){
		result.Add(MethodDecl);
	}
}