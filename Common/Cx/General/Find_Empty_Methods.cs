CxList methods = param.Length == 1 ?
	param[0] as CxList :
	Find_MethodDecls();

CxList stmtCollect = Find_StatementCollection().FindByFathers(methods);
CxList retStmt = All.NewCxList();
foreach(CxList method in stmtCollect)
{
    try
	{
		StatementCollection col = method.TryGetCSharpGraph<StatementCollection>();	
		if(col.Count <= 1)
		{
			result.Add(method.GetFathers());
			foreach(Statement s in col)
			{
				retStmt.Add(s.NodeId, s);
			}
		}
	}
	catch (Exception ex)
	{
		cxLog.WriteDebugMessage(ex);
	}
	
}
result -= retStmt.GetAncOfType(typeof(MethodDecl));
result -= retStmt.GetAncOfType(typeof(LambdaExpr));