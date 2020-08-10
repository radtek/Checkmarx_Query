CxList integers = Find_Integers();

//Remove integers inside casts to chars
string[] castTypes = new string[]{"char", "Character"};
CxList typeRefs = Find_TypeRef();
CxList integersCast = integers.GetFathers().FindByType(typeof(CastExpr));
CxList castToChar = typeRefs.GetByAncs(integersCast).FindByTypes(castTypes).GetFathers();
CxList integersCharCast = integers.GetByAncs(castToChar);
integers -= integersCharCast;

result = integers;

CxList methods = Find_Methods();
result.Add(All.FindByMemberAccess("ResultSetMetaData.*")); 
result.Add(methods.FindByShortName("getClass*"));
result.Add(All.GetParameters(methods.FindByMemberAccess("Hashtable.get")));
result.Add(methods.FindByMemberAccess("FileInputStream.markSupported"));
result.Add(methods.FindByMemberAccess("*.setContentLength"));
result.Add(methods.FindByMemberAccesses(new string[] {
	"Cookie.setMaxAge", "Cookie.maxAge",
	"URL.getProtocol", "URL.protocol",
	"URL.getPort", "URL.port"
}));
result.Add(All.GetParameters(methods.FindByMemberAccess("EntityManager.find"), 1));

CxList esapi = Find_ESAPI_Sanitizer();

result.Add(Find_HashSanitize());
result.Add(esapi);
result.Add(Find_Dead_Code_AbsInt());
result.Add(Find_CollectionAccesses());

CxList propertiesMethods = methods.FindByMemberAccess("Properties.getProperty");
CxList getParams = All.GetParameters(propertiesMethods, 0);
result.Add(methods.GetByAncs(getParams));