result = Find_General_Sanitize();
CxList methods = base.Find_Methods();

string[] regexMemberAccesses = new string[]{
		// regex to replace
		"Matcher.replaceAll",
		"String.replaceAll"	,
		"Matcher.replaceFirst",
		"Matcher.appendReplacement",
		
		// regex filter
		"Matcher.group",
		"Pattern.split",
		"*.encodeForLDAP",
		"LdapEncoder.nameEncode",
		"LdapEncoder.filterEncode"
};
result.Add(methods.FindByMemberAccesses(regexMemberAccesses));

CxList parameters = Find_Params();
CxList searchMethods = Find_LDAP_Outputs().FindByShortName("search");
CxList sanitizedMethods = searchMethods.FindByParameters(parameters.GetParameters(searchMethods, 3));
sanitizedMethods = All.GetParameters(sanitizedMethods, 2) - parameters;
result.Add(sanitizedMethods);