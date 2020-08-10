CxList strings = Find_Strings();
CxList unkRefs = Find_UnknownReference();

CxList newQuery = All.FindByMemberAccess("PersistenceManager.newQuery");
CxList sqlType = strings.FindByName("*javax.jdo.query.SQL*");
sqlType.Add(unkRefs.FindByShortName("Query").GetMembersOfTarget().FindByShortName("SQL"));
newQuery = newQuery.FindByParameters(sqlType);
CxList sqlParam = All.GetParameters(newQuery, 1);
CxList queryExecute = All.FindByMemberAccess("Query.execute*");
//sql expression parameter which is inserted into query and executed later
result = sqlParam.DataInfluencingOn(queryExecute).GetStartAndEndNodes(CxList.GetStartEndNodesType.StartNodesOnly);