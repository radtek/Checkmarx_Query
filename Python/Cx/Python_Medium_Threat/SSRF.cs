CxList inputs = Find_Interactive_Inputs();
CxList requests = Find_Remote_Requests();
CxList sanitize = Find_Remote_Requests_Sanitize();
result = requests.InfluencedByAndNotSanitized(inputs, sanitize);