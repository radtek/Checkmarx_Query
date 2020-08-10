CxList Catch = Find_Catch();
foreach(CxList curCatch in Catch)
{
	try
	{
		Catch ch = curCatch.TryGetCSharpGraph<Catch>();
		if(ch != null && ch.Statements.Count == 0)
		{
			result.Add(curCatch);
		}
	}
	catch (Exception ex){
		cxLog.WriteDebugMessage(ex);
	}
}