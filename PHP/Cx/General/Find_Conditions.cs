// Find all conditions of if statements and loops

CxList ifStmt = All.FindByType(typeof(IfStmt));
CxList iteration = All.FindByType(typeof(IterationStmt));

CxList conditions = All.NewCxList();
foreach (CxList singleIf in ifStmt)
{
	try
	{
		IfStmt stmt = singleIf.TryGetCSharpGraph<IfStmt>();
		if (stmt.Condition != null)
		{
			conditions.Add(All.FindById(stmt.Condition.NodeId));
		}
	}
	catch (Exception ex)
	{
		cxLog.WriteDebugMessage(ex.ToString());
	}
}


foreach (CxList iter in iteration)
{
	try
	{
		IterationStmt stmt = iter.TryGetCSharpGraph<IterationStmt>();
		if (stmt.Test != null)
		{
			conditions.Add(All.FindById(stmt.Test.NodeId));
		}
	}
	catch (Exception ex)
	{
			cxLog.WriteDebugMessage(ex.ToString());
	}
}

result = conditions;