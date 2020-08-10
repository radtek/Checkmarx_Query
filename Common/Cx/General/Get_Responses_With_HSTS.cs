if(param.Length == 2){
	CxList responses = (CxList) param[0];
	CxList hstsHeaders = (CxList) param[1];
	CxList influenced = responses.InfluencedBy(hstsHeaders);
	
	if(influenced.Count() > 0){	
		result.Add(influenced);
	}
		CxList responseHeaderChanges = Find_Change_Response_Header(); 
		CxList headers = Get_HSTS_Headers(responseHeaderChanges);

		CxList respWithOutputStream = responses.InfluencedBy(All.FindAllReferences(responseHeaderChanges.GetTargetOfMembers()));
		CxList firstNode = respWithOutputStream.GetStartAndEndNodes(CxList.GetStartEndNodesType.StartNodesOnly);
	
		if(((responseHeaderChanges.GetTargetOfMembers() * All.FindAllReferences(firstNode)).Count > 0) && respWithOutputStream.Count()>0 )
		{
			CxList noBadFindings = All.NewCxList();
			CxList maxAgeValue = headers.FindByRegex(@"(?<=max-age\s*=\s*)(.*[1-9]\d{8,}|[3-9][1-9][5-9][3-9][6-9][0-9][0-9][0-9])");
	
			CxList includeSubDomains = headers.FindByRegex(@"(?:^|\W)includeSub(d|D)omains(?:$|\W)");
			CxList validParameters = includeSubDomains * maxAgeValue;

			if(validParameters.Count() == 0){
				noBadFindings.Add(headers);
			}
			
			if(noBadFindings.Count == 0 && result.Count() == 0)
			{
				result = noBadFindings;				
			}
		}
}