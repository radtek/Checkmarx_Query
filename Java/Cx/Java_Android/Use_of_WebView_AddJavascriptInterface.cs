int sdkVersion = 0;
bool isInt = false;
string pattern = @"targetSdkVersion\s+([0-9]+)";

CxList targets = All.FindByRegexExt(pattern, "build.gradle", false);
targets = targets.FindByFileName(cxEnv.Path.Combine("*", "app", "build.gradle"));

if(targets.Count > 0){
	// Find sdk version in gradle build files
	foreach(CxList comment in targets)
	{
		Comment target = comment.TryGetCSharpGraph<Comment>();
		foreach(Match m in new System.Text.RegularExpressions.Regex(pattern).Matches(target.CommentText)){
			isInt = int.TryParse(m.Groups[1].Value, out sdkVersion);
			break;
		}
		break;
	}
}
else{
	// Find sdk version in Manifest.xml files
	CxList strings = Find_Strings();
	CxList settings = Find_Android_Settings();
	
	CxList sdkVersionVar = settings.GetByAncs(All.FindByName("MANIFEST.USES_SDK.ANDROID_MINSDKVERSION"));
	CxList SdkVersionVal = strings.GetByAncs(sdkVersionVar.GetAncOfType(typeof(AssignExpr)));
	isInt = int.TryParse(SdkVersionVal.GetName(), out sdkVersion);
}

if(!isInt || sdkVersion < 17) // addJavascriptInterface vulnerability is fixed in API 17 and above
{
	result = Find_Methods().FindByShortName("addJavascriptInterface");
}