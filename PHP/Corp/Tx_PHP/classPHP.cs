CxList CxAll = All.FindByFileName(@"*test\classPHP.php");
CxList input = CxAll.FindByShortName("_GET");
CxList output = CxAll.FindByShortName("eval");
result = input.InfluencingOn(output);