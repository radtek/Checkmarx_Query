CxList ifStmts = Find_Ifs();
CxList conditions = All.FindByFathers(ifStmts);
conditions = conditions.FindByType(typeof(Expression));
CxList binaryExprs = conditions.FindByShortNames(new List<string> {"==", "!="});
foreach(CxList binaryExpr in binaryExprs){
	CxList expr = All.GetByAncs(binaryExpr);
	CxList user = expr.FindByShortName("*user*", false);
	CxList stringLiteral = expr.FindByType(typeof(StringLiteral));
	if(user.Count > 0 && stringLiteral.Count > 0){
		result.Add(binaryExpr);
	}
}
