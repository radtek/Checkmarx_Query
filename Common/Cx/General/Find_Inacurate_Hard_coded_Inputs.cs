CxList stringLiterals = Find_Strings();

CxList stringLiteralsLowLength = All.NewCxList();
foreach (CxList str in stringLiterals){
	if(str.TryGetCSharpGraph<StringLiteral>().Value.Length <= 6){
		stringLiteralsLowLength.Add(str);
	}
}
result = stringLiteralsLowLength + Find_Hard_coded_Objects();