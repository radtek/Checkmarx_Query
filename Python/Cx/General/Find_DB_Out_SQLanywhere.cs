CxList allDataInfluencedByConn = Find_DB_Conn_SQLanywhere();

//Explicit fetch functions 
result.Add(allDataInfluencedByConn.GetMembersOfTarget().FindByShortName("fetch*"));

//Implicit fecth
result.Add(allDataInfluencedByConn.GetFathers().FindByType(typeof(ForEachStmt)));