CxList readNonDB = Find_Read_NonDB();
// System.getProperties should not be considered as input
CxList systemProps = readNonDB.FindByMemberAccess("System.getProperty");
systemProps.Add(readNonDB.FindByMemberAccess("System.getProperties"));
readNonDB -= systemProps;

CxList reads = All.NewCxList();
reads.Add(readNonDB);
// Remove Properties as they are considered potential inputs
reads -= reads.FindByMemberAccess("Properties.get*");

CxList inputs = Find_DB_Out();
inputs.Add(reads);

CxList outputs = Find_XSS_Outputs();

CxList sanitize = Find_XSS_Sanitize();
sanitize.Add(Find_API_Response_Outputs());

result = inputs.InfluencingOnAndNotSanitized(outputs, sanitize, CxList.InfluenceAlgorithmCalculation.NewAlgorithm);
result = result.ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow);