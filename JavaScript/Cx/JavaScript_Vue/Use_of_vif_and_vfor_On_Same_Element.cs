/*******************************************************************
* Never use v-if on the same element as v-for.
* There are two common cases where this can be tempting:
* - To filter items in a list (e.g. v-for="user in users" v-if="user.isActive"). 
*	In these cases, replace the list with a new computed property that returns your filtered list (e.g. activeUsers).
* - To avoid rendering a list if it should be hidden (e.g. v-for="user in users" v-if="shouldShowUsers").
*	In these cases, move the v-if to a container element (e.g. ul, ol).
* (https://vuejs.org/v2/style-guide/#Avoid-v-if-with-v-for-essential)
********************************************************************/
if(cxScan.IsFrameworkActive("VueJS")){
		
	// the cxVFor flag only exists when there is
	// a v-if on the same element as v-for	
	result = Find_UnknownReference()
		.FindByShortName("cxVFor")
		.GetAncOfType(typeof(IfStmt))
		.GetByAncs(Find_ForEachStmt());
}