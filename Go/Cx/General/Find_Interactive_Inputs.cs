// from https://golang.org/pkg/bufio/ package
// Package bufio implements buffered I/O.

CxList bufioInputs = All.NewCxList();
List<string> bufioReaders = new List<string> {"NewScanner","NewReader","NewReaderSize"};
CxList bufioInputVariables = All.FindByMemberAccess("bufio.*").FindByShortNames(bufioReaders).GetAssignee();
CxList variablesOcurrences = All.FindAllReferences(bufioInputVariables);

String[] methods = new string[] {"Bytes", "Text","ReadString","ReadRune"}; 
foreach(String m in methods){
	bufioInputs.Add(variablesOcurrences.GetMembersOfTarget().FindByShortName(m));
}

// from https://golang.org/pkg/bytes/ package
// Package bytes implements functions for the manipulation of byte slices.
CxList bytesInputs = All.NewCxList();
List<string> bytesBuffers = new List<string> {"NewBuffer","NewBufferString"};
CxList bytesInputVariables = All.FindByMemberAccess("bytes.*").FindByShortNames(bytesBuffers).GetAssignee();
variablesOcurrences = All.FindAllReferences(bytesInputVariables);

methods = new string[] {"ReadString", "ReadRune"};
foreach(String m in methods){
	bytesInputs.Add(variablesOcurrences.GetMembersOfTarget().FindByShortName(m));
}

// from https://golang.org/pkg/fmt/ package
// This package implements formatted I/O with functions analogous to C
CxList fmtMemberAccess = All.FindByMemberAccess("fmt.*");
CxList scanAllParams = fmtMemberAccess.FindByShortNames(new List<string> {
		"Scan", "Scanln" });
CxList scanFirstParam = fmtMemberAccess.FindByShortNames(new List<string> {
		"Scanf", "Fscan", "Fscanln", "Sscan", "Sscanln" });
CxList scanSecondParam = fmtMemberAccess.FindByShortNames(new List<string> {
		"Fscanf", "Sscanf" });
CxList fmtMethods = All.NewCxList();
fmtMethods.Add(scanAllParams, scanFirstParam, scanSecondParam);
CxList fmtInputs = All.GetParameters(fmtMethods);

// remove the non interactive inputs / parameters
CxList toRemove = All.NewCxList();
toRemove.Add(All.GetParameters(scanFirstParam, 1),
	All.GetParameters(scanSecondParam, 1),
	All.GetParameters(scanSecondParam, 2),
	Find_Param());
fmtInputs -= toRemove;

// add interactive inputs to result
result.Add(bufioInputs,
	bytesInputs,
	fmtInputs);