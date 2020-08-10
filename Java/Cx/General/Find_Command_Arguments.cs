//*********************************************************************
// Get all the command execution arguments
//*********************************************************************
CxList strings = Find_Strings();
CxList unknownRefs = Find_UnknownReference();
CxList decls = Find_Declarators(); 
CxList binaryExpr = Find_BinaryExpr();
CxList indexerRefs = Find_IndexerRefs();
CxList integerLiterals = Find_IntegerLiterals();
CxList argumentType = All.NewCxList();
argumentType.Add(strings, unknownRefs);

// command exec methods
CxList exec = Find_Command_Injection_Outputs();

// command exec parameters
CxList variables = All.NewCxList();
variables.Add(unknownRefs, decls);
CxList execParams = All.GetParameters(exec);
CxList execParamsDecls = variables.FindAllReferences(execParams).GetAssigner();
CxList execParamsRef = All.NewCxList();
execParamsRef.Add(execParams, execParamsDecls);

// command arguments from binary expressions (exclude left side)
// sample: "cmd.exe" + " " + arg
CxList arguments = All.NewCxList();
CxList commandArgBinaryExpr = execParamsRef * binaryExpr;
commandArgBinaryExpr.Add(binaryExpr.GetByAncs(execParamsRef));
arguments = commandArgBinaryExpr.CxSelectDomProperty<BinaryExpr>(x => x.Right);  

// command arguments from array initializers (exclude first element)
// sample: String[] programArgs = { "cmd.exe", arg };
CxList arrayInitializers = execParamsDecls.FindByType(typeof(ArrayInitializer));
foreach(CxList arrayInitializer in arrayInitializers)
{
	CxList initValues = arrayInitializer.CxSelectElements<ArrayInitializer>(x => x.InitialValues);	
	CxList initValuesExceptFirst = initValues.FindSubList(initValues.Count - 1, false);
	arguments.Add(initValuesExceptFirst);
}

// command arguments from binary expression (exclude first element)
// sample: 	String[] cmd = new String[3]; cmd[0] = "cmd.exe"; cmd[1] = arg;
arguments.Add(indexerRefs.CxSelectDomProperty<IndexerRef>(x => x.Indices[0])
	.FilterByDomProperty<IntegerLiteral>(x => x.Value > 0)
	.GetFathers()
	.GetAssigner());

result = execParams.InfluencedBy(arguments).GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly);