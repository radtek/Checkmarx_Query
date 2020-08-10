//找到input到output所有数据流,且要求是在if判断语句里输出。
CxList newall = All.FindByFileName(@"*condition\src\Exc8.cs");
CxList input = newall.FindByShortName("input");
CxList ifmethods = newall.FindByType(typeof(IfStmt));
foreach(CxList ifmethod in ifmethods){
	CxList ifcond = newall.GetByAncs(ifmethod);
	if(ifcond.FindByShortName("==").Count>0){
		cxLog.WriteDebugMessage("ifcond=" + ifcond);
		result.Add(ifcond);
		}
	}
//result = ifmethods;
//CxList output = newall.FindByShortName("output");
//result = input.DataInfluencingOn(output);

//该方法不知道是否正确
//CxList CxAll = All.FindByFileName(@"*condition\src\Exc8.cs");
//CxList input = CxAll.FindByShortName("input");
//CxList output = CxAll.GetByAncs(CxAll.FindByType(typeof(IfStmt))).FindByShortName("output");
//result = output.DataInfluencedBy(input);