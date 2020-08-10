CxList possible_db = Find_DB_Heuristic();
possible_db -= possible_db.DataInfluencedBy(possible_db);

if (possible_db.Count > 0)
{
	CxList input = Find_Interactive_Inputs();

	CxList strings = Find_Strings();
	CxList Select = strings.FindByName("*select*", false);
	CxList Where = strings.FindByName("*where*", false);
	CxList And = strings.FindByName("*And *", false) + 
		strings.FindByName("* And*", false);

	possible_db = possible_db.DataInfluencedBy(Select).DataInfluencedBy(Where);
	possible_db -= possible_db.DataInfluencedBy(And);

	result = possible_db.DataInfluencedBy(input);
}