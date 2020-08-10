CxList con = Find_DBConnection();

CxList inputs = Find_Interactive_Inputs();
CxList sanitize = Find_CollectionAccesses();
sanitize.Add(Find_Integers());
sanitize.Add(Find_Methods().FindByMemberAccess("Properties.getProperty"));

result = con.InfluencedByAndNotSanitized(inputs, sanitize);