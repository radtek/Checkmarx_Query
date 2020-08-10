//SQL注入案例
CxList newall = All.FindByFileName(@"*SQLi\src\T1.cs");
//CxList output = newall.FindByShortName("execute");
CxList input = newall.FindByShortNames(new List<string> {"getParameterValues","input"});
CxList output = newall.FindByShortName("execute");
CxList sanitize = newall.FindByShortName("sanitize*");
//CxList input = output.InfluencedBy(newall);
//CxList input = newall.InfluencingOn(output);
result = output.InfluencedByAndNotSanitized(input, sanitize);