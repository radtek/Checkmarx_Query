CxList Commit = All.FindByShortName("SQL_COMMIT");
CxList Rollback = All.FindByShortName("SQL_ROLLBACK");

CxList TryBlock = Commit.GetAncOfType(typeof(TryCatchFinallyStmt));
foreach(CxList cml in TryBlock)
{
	TryCatchFinallyStmt TryGraph = cml.TryGetCSharpGraph<TryCatchFinallyStmt>();

	CxList curTry = All.FindById(TryGraph.Try.NodeId);
	
	CxList curCatch = All.NewCxList();
	if(TryGraph.CatchClauses != null && TryGraph.CatchClauses.Count > 0)
	{
		curCatch = All.FindById(TryGraph.CatchClauses[0].NodeId);
	}
	
	CxList CommitInTry = Commit.GetByAncs(curTry);
	CxList RollbackInCatch = Rollback.GetByAncs(curCatch);


	if( (RollbackInCatch.GetAncOfType(typeof(TryCatchFinallyStmt)) * 
		CommitInTry.GetAncOfType(typeof(TryCatchFinallyStmt))).Count == 0)
	{
		result.Add(cml);
	}
}