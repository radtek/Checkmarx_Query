CxList methods = Find_Methods();
CxList ofTypeString = Find_Strings();


// JDBCTemplate
CxList jdbcQuery = methods.FindByMemberAccess("JDBCTemplate.query");
result.Add(ofTypeString.GetByAncs(All.GetParameters(jdbcQuery)));

// Heuristic: queries starting with SELECT
CxList sqlSelect = ofTypeString.FindByShortName("SELECT*");
result.Add(sqlSelect);