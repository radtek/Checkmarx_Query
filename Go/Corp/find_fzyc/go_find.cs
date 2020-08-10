CxList input = All.FindByType(typeof(ParamDecl));
CxList output = Find_Methods().FindByShortName("make");
//return All.GetBlocksOfIfStatements(false);
CxList sanitize = All.GetByAncs(All.GetBlocksOfIfStatements(true));
result = output.InfluencedByAndNotSanitized(input, sanitize);