CxList stringImport = All.FindByMemberAccess("strings.*");
List<string> stringMethods = new List<string>(){"Builder","HasPrefix","HasSuffix","Replace", "ReplaceAll", "Split", "SplitAfter", "SplitAfterN", "SplitN"};
CxList sanitizers = stringImport.FindByShortNames(stringMethods);
result = sanitizers;