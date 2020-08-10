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
CxList unknownRefs = Find_UnknownReference();
result.Add(All.FindByMemberAccess("ResultSetMetaData.*")); 
result.Add(methods.FindByShortName("getClass*"));
result.Add(All.GetParameters(methods.FindByMemberAccess("Hashtable.get")));
result.Add(methods.FindByMemberAccess("URL.getProtocol"));
result.Add(methods.FindByMemberAccess("URL.getPort"));
result.Add(methods.FindByMemberAccess("FileInputStream.markSupported"));
result.Add(methods.FindByMemberAccess("*.setContentLength"));
result.Add(methods.FindByMemberAccess("Cookie.setMaxAge"));
result.Add(All.GetParameters(methods.FindByMemberAccess("EntityManager.find"), 1));

CxList ESAPI = Find_ESAPI_Sanitizer();

// getAttribute
CxList getAttr = Get_Session_Attribute();
getAttr.Add(Get_Context_Attribute());
getAttr = All.GetParameters(getAttr);
CxList constants = Find_Constants();
constants.Add(getAttr.FindAllReferences(constants));
CxList strings = Find_Strings();
CxList allString = All.NewCxList();
allString.Add(strings);
allString.Add(strings.GetFathers());
allString.Add(constants);
allString.Add(constants.GetFathers());
getAttr -= allString;

result.Add(getAttr);
result.Add(Find_HashSanitize());
result.Add(ESAPI);
result.Add(Find_Dead_Code_Contents());
result.Add(Find_Dead_Code_AbsInt());
result.Add(Find_CollectionAccesses());
result.Add(Set_Context_Attribute());
result.Add(Set_Session_Attribute());

CxList propertiesMethods = methods.FindByMemberAccess("Properties.getProperty");
CxList getParams = All.GetParameters(propertiesMethods, 0);
result.Add(methods.GetByAncs(getParams));