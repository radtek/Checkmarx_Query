// A general query for finding all JS-CodeInjection sanitizers
CxList generalSanitize = Sanitize();
result.Add(generalSanitize);
result.Add(Find_SAPUI5_Sanitize().FindByShortNames(new List<string> {"escapeJS", "encodeJS"}));

//add component getEvent to sanitizers:
if(Lightning_Find_Aura_Cmp_And_App().Count > 0)
{
	CxList component = Lightning_Find_Controller_Component_Object();
	component.Add(Lightning_Find_Helper_Component_Object());
	result.Add(component.GetMembersOfTarget().FindByShortName("getEvent"));		

	
}