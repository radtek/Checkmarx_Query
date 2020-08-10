if(param.Length == 1)
{
	
	CxList disableValidation = (CxList) param[0];	
	
	// Create a flow from flied to declarator
	Dictionary<int, CxList> lastElAndFlow = new Dictionary<int, CxList>();
	CxList listToValidate = All.NewCxList();
	foreach (CxList cml in disableValidation)
	{
		CxList parent = cml.GetFathers();	
		CxList flow = cml.Concatenate(parent);	
		CxList decl = parent.GetFathers();
		if(decl.FindByType(typeof(AssignExpr)).Count == 1)
		{
			decl = decl.GetAncOfType(typeof(ObjectCreateExpr));
		}	
		CxList flow2 = parent.Concatenate(decl);
		flow = flow.ConcatenatePath(flow2);	

		CxList lastElemt = flow.GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly); 	
		lastElAndFlow.Add(lastElemt.GetFirstGraph().DomId, flow);	
		listToValidate.Add(flow);

	}

	// #### Second part of build flow

	CxList vulnerableResults = All.NewCxList();

	CxList tokenValidationParameters = All.FindByType(typeof(MemberAccess)).FindByShortName("TokenValidationParameters")
		.GetByAncs(All.FindByType(typeof(MethodInvokeExpr)).FindByShortName("AddJwtBearer"));	

	CxList vulnerable = All.NewCxList();
	vulnerable = tokenValidationParameters.GetAssigner();
	vulnerable = vulnerable - vulnerable.FindByType(typeof(ObjectCreateExpr));

	CxList finalFlow = All.NewCxList();

	foreach (CxList v in vulnerable)
	{
		CxList el = v.GetAssigner();
		if(el.Count == 1)
		{	
			if(el.FindByType(typeof(UnknownReference)).Count == 1)
			{
				CxList definition = All.FindDefinition(el);
				if(definition.Count == 1)
				{
					CxList flowToDefinition = definition.Concatenate(el);				
					CxList firtElemt = flowToDefinition.GetStartAndEndNodes(CxList.GetStartEndNodesType.StartNodesOnly); 				
					int key = firtElemt.GetFirstGraph().DomId;
					if(lastElAndFlow.ContainsKey(key))
					{						
						CxList existFlow = lastElAndFlow[key];
						existFlow = existFlow.ConcatenatePath(flowToDefinition); 					
						finalFlow.Add(existFlow);
					}else
					{
						CxList haveFlow = All.InfluencingOn(firtElemt);
						if(haveFlow.Count > 0)
						{
							haveFlow = haveFlow.ReduceFlow(CxList.ReduceFlowType.ReduceSmallFlow);				
							haveFlow = haveFlow.ConcatenatePath(flowToDefinition);				
			
							firtElemt = haveFlow.GetStartAndEndNodes(CxList.GetStartEndNodesType.StartNodesOnly); 
							key = firtElemt.GetFirstGraph().DomId;
							if(lastElAndFlow.ContainsKey(key))
							{
								CxList existFlow = lastElAndFlow[key];
								haveFlow = existFlow.ConcatenatePath(haveFlow); 					
								finalFlow.Add(haveFlow);
							}
						}
					}
				}
			}
			else if(el.FindByType(typeof(MethodInvokeExpr)).Count == 1)
			{
				CxList flow = All.InfluencingOn(v);
				flow = flow.ReduceFlow(CxList.ReduceFlowType.ReduceSmallFlow);
				CxList firtElemt = flow.GetStartAndEndNodes(CxList.GetStartEndNodesType.StartNodesOnly); 
				int key = firtElemt.GetFirstGraph().DomId;
				if(lastElAndFlow.ContainsKey(key))
				{
					CxList existFlow = lastElAndFlow[key];
					flow = existFlow.ConcatenatePath(flow); 
					finalFlow.Add(flow);
				}
				
			}
		}
	}

	vulnerableResults.Add(finalFlow);
	result = vulnerableResults;
}