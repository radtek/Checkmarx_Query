CxList input = All.FindByShortName("GetName");
CxList dbIn = All.GetParameters(All.FindByMemberAccess("Database.execute"), 0);
result = dbIn.InfluencedBy(input);