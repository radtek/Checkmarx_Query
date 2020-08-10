CxList methods = Find_Methods();
CxList objectCreate = Find_Object_Create();

CxList inSecureTypes = All.NewCxList();

List<string> deprecatedClassInSecure = new List<string>(){
		"StandardPasswordEncoder", 
		"MessageDigestPasswordEncoder",
		"NoOpPasswordEncoder"};

CxList deprecatedInstances = objectCreate.FindByShortNames(deprecatedClassInSecure);

inSecureTypes.Add(deprecatedInstances);

string[] encryptorsClassInSecure = new string[]{	
	"Encryptors.noOpText"};

CxList encryptorsInstances = methods.FindByMemberAccesses(encryptorsClassInSecure);

inSecureTypes.Add(encryptorsInstances);

result = inSecureTypes;