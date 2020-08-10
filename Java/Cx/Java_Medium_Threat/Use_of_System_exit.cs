CxList relevantExit = Find_Use_Of_System_Exit();
CxList intCast = Find_TypeRef().FindByShortName("int").GetAncOfType(typeof(CastExpr));

foreach(CxList curExit in relevantExit)
{
	CxList paramInExist = All.GetParameters(curExit);	
	
	CxList prms = paramInExist.FindByType(typeof(IntegerLiteral));
	if(prms.Count == 1)
	{
		result.Add(curExit);
	}
	
	CxList prmsUnknownRef = paramInExist.FindByType(typeof(UnknownReference));
	if(prmsUnknownRef.Count == 1)
	{
		result.Add(curExit);
	}
	
	CxList castExpr = paramInExist.FindByType(typeof(CastExpr)) * intCast;
	if(castExpr.Count == 1)
	{
		result.Add(curExit);
	}
}