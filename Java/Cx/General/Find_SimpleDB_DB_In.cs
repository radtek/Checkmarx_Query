CxList obj = Find_Object_Create(); 

CxList selectExp = All.FindByShortName("SelectRequest").GetByAncs(obj);
selectExp.Add(All.FindByMemberAccess("SelectRequest.setSelectExpression"));
selectExp.Add(All.FindByMemberAccess("SelectRequest.withSelectExpression"));

CxList sdbc = All.FindByMemberAccess("AmazonSimpleDBClient.select");
//SelectRequest which is inserted into select method (which executes the request)
result = selectExp.DataInfluencingOn(sdbc).GetStartAndEndNodes(CxList.GetStartEndNodesType.StartNodesOnly);