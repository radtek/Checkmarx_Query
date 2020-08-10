//*********************************************************************
// A potentially tainted value passed as an argument to an external program,
// which is executed by code, may allow for command argument injection.
//*********************************************************************

// inputs
CxList inputs = Find_DB_Out();
inputs.Add(Find_Read_NonDB(), 
	Find_FileStreams(), 
	Find_FileSystem_Read());

// sinks
CxList commandArg = Find_Command_Arguments();

// sanitize
CxList sanitize = Find_Command_Injection_Sanitize();

// add String.replace() and String.replaceAll as sanitizers
CxList methods = Find_Methods();
sanitize.Add(methods.FindByShortName("replace*", false));

// result
result = commandArg.InfluencedByAndNotSanitized(inputs, sanitize, CxList.InfluenceAlgorithmCalculation.NewAlgorithm);
result = result.ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow);