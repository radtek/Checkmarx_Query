CxList inputs = Find_FileStreams();
inputs.Add(Find_DB_Out());

CxList sanitize = Find_LDAP_Sanitize(); 
CxList outputs = Find_LDAP_Outputs();

result = outputs.InfluencedByAndNotSanitized(inputs, sanitize).ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow);