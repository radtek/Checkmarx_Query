result = Common_Best_Coding_Practice.Detection_of_Error_Condition_Without_Action();

CxList mkdirs = All.FindByMemberAccess("File.mkdirs");
CxList not = mkdirs.GetAncOfType(typeof(UnaryExpr)).FindByShortName("Not");
CxList If = not.GetFathers().FindByType(typeof(IfStmt));
foreach(CxList curIf in If)
{
	IfStmt ifStmt = curIf.TryGetCSharpGraph<IfStmt>();
	if(ifStmt.TrueStatements.Count == 0)
	{
		result.Add(ifStmt.NodeId, ifStmt);
	}
}