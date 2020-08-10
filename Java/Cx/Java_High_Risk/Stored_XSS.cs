CxList readNonDB = Find_Read_NonDB();
//System.getProperties should not be considered as input
CxList listSystemGetPropertiesInInputs = readNonDB.FindByMemberAccess("System.getProperty");
listSystemGetPropertiesInInputs.Add(readNonDB.FindByMemberAccess("System.getProperties"));
readNonDB -= listSystemGetPropertiesInInputs;


CxList read = All.NewCxList();
read.Add(readNonDB);
// Remove Properties as they are considered potential inputs and are handled by the Potential_Stored_XSS query
read -= read.FindByMemberAccess("Properties.getProperty");
read.Add(Find_FileSystem_Read());

CxList inputs = Find_DB_Out();
inputs.Add(read);

CxList outputs = Find_XSS_Outputs();

CxList sanitize = Find_XSS_Sanitize() + Find_API_Response_Outputs();

result = inputs.InfluencingOnAndNotSanitized(outputs, sanitize, CxList.InfluenceAlgorithmCalculation.NewAlgorithm);
result = result.ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow);