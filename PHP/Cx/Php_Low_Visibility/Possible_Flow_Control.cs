CxList inputs = Find_Inputs();
inputs.Add(Find_Read());
inputs.Add(Find_File_Read());

CxList sanitized = Find_Flow_Control_Sanitize();

CxList flowClauses = All.NewCxList();
try 
{
	foreach( CxList statement in All.FindByType(typeof(SwitchStmt)) )
	{
		SwitchStmt g = statement.TryGetCSharpGraph<SwitchStmt>();
		flowClauses.Add(All.FindById(g.Condition.NodeId));
	}
}
catch (Exception ex)
{
	cxLog.WriteDebugMessage(ex);
}
try 
{
	foreach( CxList statement in All.FindByType(typeof(IterationStmt)) )
	{
		IterationStmt g = statement.TryGetCSharpGraph<IterationStmt>();
		if (g.Test != null)
			flowClauses.Add(All.FindById(g.Test.NodeId));	
	}
}
catch (Exception ex)
{
	cxLog.WriteDebugMessage(ex);
}
try 
{
	foreach( CxList statement in All.FindByType(typeof(IfStmt)) )
	{
		IfStmt g = statement.TryGetCSharpGraph<IfStmt>();
		flowClauses.Add(All.FindById(g.Condition.NodeId));

	}
}
catch (Exception ex)
{
	cxLog.WriteDebugMessage(ex);
}
try 
{
	
	foreach( CxList statement in All.FindByType(typeof(TernaryExpr)) )
	{
		TernaryExpr g = statement.TryGetCSharpGraph<TernaryExpr>();
		flowClauses.Add(All.FindById(g.Test.NodeId));
	}
}
catch (Exception ex)
{
	cxLog.WriteDebugMessage(ex);
}

result = flowClauses.InfluencedByAndNotSanitized(inputs, sanitized);