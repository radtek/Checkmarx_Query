if(cxScan.IsFrameworkActive("VueJS"))
{
	CxList inputs = Find_Inputs();
	CxList viewInputs = Find_ViewInputs();
	inputs.Add(viewInputs);
	// Add VueJs Router API inputs
	inputs.Add(Find_VueJs_Inputs());
		
	CxList outputs = All.NewCxList();
	
	// Add v-html
	outputs.Add(Find_ViewOutputs() - Find_ViewEscapedOutputs());
	
	// Add v-bind	
	CxList lefts = All.FindByAssignmentSide(CxList.AssignmentSide.Left);
	CxList atttrs = All.FindByMemberAccesses(new string[]{"*.href", "*.src","*.on*"}, false) * lefts;
	outputs.Add(atttrs.GetByAncs(Find_ViewDecls()));
    		
	// Add innerHTML inside domProps
	CxList decls = Find_Declarators(); 
	outputs.Add(decls.FindByShortName("innerHTML").GetByAncs(decls.FindByShortName("domProps")));

	// Add vue compile
	outputs.Add(Find_Methods().FindByMemberAccess("Vue.compile"));
	
	CxList sanitize = Find_XSS_Sanitize();
      
	result.Add(outputs.InfluencedByAndNotSanitized(inputs, sanitize));
	
	//Remove flows that goes from ViewInputs to v-once outputs
	CxList cxOnceIfs = Find_UnknownReference().FindByShortName("CxOnce").GetAncOfType(typeof(IfStmt));
	CxList onceOutputs = outputs.GetByAncs(cxOnceIfs);
	result -= onceOutputs.InfluencedByAndNotSanitized(viewInputs, sanitize);
}