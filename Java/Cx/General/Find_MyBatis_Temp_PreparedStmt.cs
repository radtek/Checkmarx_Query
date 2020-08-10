if(cxScan.IsFrameworkActive("MyBatis"))
{
	CxList myBatisTempPrepStmt = Find_Methods().FindByShortName("preparedStatement");
	result = myBatisTempPrepStmt.FindByFileName("*.xml");
}