/* This query finds all Kony's local store values retrival */

if(cxScan.IsFrameworkActive("KonyInFF"))
{
	CxList konyStore = Kony_All().FindByMemberAccess("kony.store");
	result = konyStore.GetMembersOfTarget().FindByShortName("getItem");
}