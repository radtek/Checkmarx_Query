string[] relevantTypes = new string[] {
	"*.Context",
	"*.DirContext",
	"*.InitialContext",
	"*.InitialDirContext",
	"*.InitialLdapContext",
	"*.LdapContext",
	"Context",
	"DirContext",
	"InitialContext",
	"InitialDirContext",
	"InitialLdapContext",
	"LdapContext"
	};

List<string> relevantMethods = new List<string> {
		"getAttributes",
		"list",
		"listBindings",
		"lookup",
		"lookupLink",
		"search"
		};

CxList outputs = Find_UnknownReference().FindByTypes(relevantTypes).GetMembersOfTarget().FindByShortNames(relevantMethods);

result = outputs;