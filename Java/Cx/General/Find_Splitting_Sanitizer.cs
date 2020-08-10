//Find_Splitting_Sanitizer//

CxList strings = Find_Strings();
CxList chars = base.Find_CharLiteral();
CxList methods = Find_Methods();
CxList integers = Find_Integers();

// All replaces that contain \r or \n as first parameter should be removed
CxList replace = methods.FindByShortName("replace*");
CxList replaceEnter = strings.GetParameters(replace, 0);
List<string> lineSeparatorsList = new List<string> {
		@"*\n*",
		@"*\r*",
		@"*%0a*",
		@"*%0d*"};
replaceEnter.FindByShortNames(lineSeparatorsList);
replace = replace.FindByParameters(replaceEnter);

// Also all places that leave only the substring before the "indexOf" \r or \n are relevant sanitizers
CxList substring = methods.FindByMemberAccess("*.substring");
CxList indexof = methods.FindByMemberAccess("*.indexOf");

CxList allIndexOfParam = chars.GetParameters(indexof);
CxList indexOfParam = allIndexOfParam.FindByRegex(@"'\\[rn]'");
indexOfParam.Add(indexOfParam.FindByShortNames(new List<string> {@"*%0a*",@"*%0d*"}));
indexof = indexof.FindByParameters(indexOfParam);

CxList substringed = substring.DataInfluencedBy(indexof).GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly);
CxList substringedEndNode = All.FindAllReferences(substringed).DataInfluencedBy(substringed).
	GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly);

CxList systemLineSeparator = methods.FindByExactMemberAccess("System", "getProperty").FindByParameters(strings.FindByShortName("line.separator"));

CxList replacesMethods = methods.FindByShortNames(new List<string> {"replace", "replaceAll"});
CxList sanitizeByLineSeparator = replacesMethods.FindByParameters(systemLineSeparator);

CxList replacersByLineSeparator = All.GetParameters(replacesMethods, 1).FindByShortNames(lineSeparatorsList);
sanitizeByLineSeparator -= replacesMethods.FindByParameters(replacersByLineSeparator);


CxList stringSplits = methods.FindByShortName("split").FindByParameters(systemLineSeparator);
stringSplits -= stringSplits.FindByParameters(All.GetParameters(stringSplits, 1));


sanitizeByLineSeparator.Add(stringSplits);


CxList encodeURL = methods.FindByExactMemberAccess("Encoder", "encodeForURL");
CxList ecodeBase64 = All.GetParameters(methods.FindByExactMemberAccess("Encoder", "encodeForBase64"), 1).FindByShortName("false").GetAncOfType(typeof(MethodInvokeExpr));

// ESAPI setHeader
CxList HTTPUtilities_s = All.FindByMemberAccess("HTTPUtilities.s*");
CxList esapi = All.NewCxList();
esapi.Add(HTTPUtilities_s.FindByMemberAccess("HTTPUtilities.setContentType"));
esapi.Add(HTTPUtilities_s.FindByMemberAccess("HTTPUtilities.safeSendRedirect"));
esapi.Add(HTTPUtilities_s.FindByMemberAccess("HTTPUtilities.safeSetHeader"));
esapi.Add(HTTPUtilities_s.FindByMemberAccess("HTTPUtilities.safeAddCookie"));
esapi.Add(encodeURL);
esapi.Add(ecodeBase64);

result.Add(integers);
result.Add(replace);
result.Add(substringed);
result.Add(sanitizeByLineSeparator);
result.Add(esapi);
result.Add(substringedEndNode);