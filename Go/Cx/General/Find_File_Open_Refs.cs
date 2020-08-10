// from https://golang.org/pkg/io/ioutil/ package
// Package that provides basic interfaces to I/O primitives.

CxList declaratorsAndUnkRefs = Find_Declarators();
declaratorsAndUnkRefs.Add(Find_UnknownReference());
	
CxList methods = Find_Methods();

CxList ioutilInputs = methods.FindByMemberAccess("io/ioutil.*").FindByShortNames(new List<string>{"ReadAll", "ReadFile"});
CxList ioInputs = methods.FindByMemberAccess("io.*").FindByShortNames(new List<string>{"ReadAtLeast", "ReadFull"});

// from https://golang.org/pkg/os/ package
// Package that provides basic interfaces to I/O primitives.
CxList readInputs = All.NewCxList();
CxList openCalls = methods.FindByMemberAccess("os.*").FindByShortNames(new List<string>{"Open", "OpenFile"});
CxList fileVariables = declaratorsAndUnkRefs.DataInfluencedBy(openCalls);

CxList fileVariablesOcurrences = All.FindAllReferences(fileVariables);
result = fileVariablesOcurrences;