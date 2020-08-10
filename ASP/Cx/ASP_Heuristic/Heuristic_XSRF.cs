CxList possible_db = Find_DB_Heuristic();

if (possible_db.Count > 0)
{
	CxList requests = Find_Interactive_Inputs();
	CxList strings = Find_Strings();
	CxList write = strings.FindByName("*update*", StringComparison.OrdinalIgnoreCase) +
		strings.FindByName("*delete*", StringComparison.OrdinalIgnoreCase) +
		strings.FindByName("*insert*", StringComparison.OrdinalIgnoreCase);

	result = possible_db.DataInfluencedBy(write).DataInfluencedBy(requests);

	if (result.Count > 0)
	{
		CxList db = Find_DB();
		result -= db.DataInfluencedBy(write).DataInfluencedBy(requests);
	}
}