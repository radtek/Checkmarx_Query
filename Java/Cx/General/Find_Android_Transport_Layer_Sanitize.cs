CxList androidSettings = Find_Android_Settings();
CxList falseLiteralsManifest = androidSettings.FindByShortName("\"false\"");
CxList clearTextTraffic = androidSettings.FindByMemberAccess("*.ANDROID_USESCLEARTEXTTRAFFIC");
result.Add(clearTextTraffic.GetAssigner() * falseLiteralsManifest);

//Network config
CxList networkConfigFiles = androidSettings.FindByShortName("ANDROID_NETWORKSECURITYCONFIG").GetAssigner();
foreach(CxList networkConfig in networkConfigFiles){
	StringLiteral networkConfigString = networkConfig.TryGetCSharpGraph<StringLiteral>();
	string networkConfigValue = networkConfigString.Value;
	char[] charsToTrim = {'"', '@'};
	networkConfigValue = networkConfigValue.Trim(charsToTrim);
	char[] separators = {'\\', '/'};
	string[] splited = networkConfigValue.Split(separators);
	string fileName = splited[splited.Length - 1];

	string filter = string.Format("*{0}.xml", fileName);
	result.Add(cxXPath.FindXmlAttributesByNameAndValue(filter, 2, "cleartextTrafficPermitted", "false"));
}