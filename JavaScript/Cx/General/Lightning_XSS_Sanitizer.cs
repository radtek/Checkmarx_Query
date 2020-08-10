CxList outputs = Lightning_Find_Outputs_XSS();
CxList potentiallySanitized = All.NewCxList();
foreach(CxList output in outputs)
{
	CxList binaryOfOutputs = output.GetFathers().FindByType(typeof(BinaryExpr));	
	CxList attr = cxXPath.GetAttributeByExpression(binaryOfOutputs);
	CxList elem = cxXPath.GetElementByExpression(binaryOfOutputs);
	if(attr.FindByShortName("href").Count == 0 || elem.FindByShortName("a").Count == 0)
	{	
		continue;
	}	
		
	CxList leftSide = All.NewCxList();	
	BinaryExpr g = binaryOfOutputs.GetFirstGraph() as BinaryExpr;

	if(g != null && g.Left != null)
	{
		leftSide.Add(g.Left.NodeId, g.Left);	
		if((leftSide.FindByShortName("/")).Count > 0){
			potentiallySanitized.Add(output);
		}
		CxList desc = cxXPath.GetAllExpressionDescendents(leftSide, 8);
		if(desc.FindByShortName("/").Count > 0)
		{
			potentiallySanitized.Add(output);
		}
	}
}
result.Add(potentiallySanitized);

//add Map addition to sanitiation
CxList Apex = Get_Apex();
CxList maps = Apex.FindByType("Map");
CxList additionOperation = maps.GetMembersOfTarget();
result.Add(additionOperation.FindByShortName("put*"));