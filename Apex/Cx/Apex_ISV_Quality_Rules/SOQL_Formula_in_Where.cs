//Find SOQL WHERE clauses with formula fields or criteria that span objects
CxList formulaFields = All.FindByCustomAttribute("formula").GetFathers();
foreach (CxList field in formulaFields)
{
	try 
	{
		string fieldName = field.GetFirstGraph().ShortName;
		CxList relevantWhere = Extract_From_SOQL("where", fieldName);//Find the fieldName in SOQL statements
		if (relevantWhere.Count > 0) 
		{
			foreach (CxList curWhere in relevantWhere)
			{
				result.Add(curWhere.Concatenate(field, true));
			}
		}
	}
	catch (Exception e) 
	{
		cxLog.WriteDebugMessage(e);
	}
}