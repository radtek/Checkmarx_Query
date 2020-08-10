//找到input到output数据流,且要求是在if语句里输出。
CxList newall = All.FindByFileName(@"*if\src\Exc7.cs");
CxList input = newall.FindByShortName("input");
CxList ifs = newall.GetByAncs(newall.FindByType(typeof(IfStmt))); 
CxList output = ifs.FindByShortName("output");
result = input.DataInfluencingOn(output);
