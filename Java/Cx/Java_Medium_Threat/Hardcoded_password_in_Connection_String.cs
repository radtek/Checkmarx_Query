// Find the string literals containing "password"
CxList psw = Find_Password_Strings();
CxList methods = Find_Methods();


// Sanitizers
CxList sanitizers = Find_ESAPI_Sanitizer();
sanitizers.Add(Find_CollectionAccesses());

CxList hardcodedStringSanitizers = methods.FindByMemberAccess("ResultSet.*");
List<string> safeMethods = new List<string>{ "getNString", "getString" };
sanitizers.Add(hardcodedStringSanitizers.FindByShortNames(safeMethods));

// Only first parameters of get/setProperty methods are safe
CxList propertyMethods = All.FindByMemberAccess("Properties.*");
List<string> safePropertyParams = new List<string>{ "getProperty", "setProperty" };
sanitizers.Add(All.GetParameters(propertyMethods.FindByShortNames(safePropertyParams), 0));

// Find creation of connections or connection strings, influenced by password
CxList createExpressions = Find_Object_Create();
CxList openConnection = createExpressions.FindByShortName("*Connection");
result = openConnection.InfluencedByAndNotSanitized(psw, sanitizers);


// Find password in relevant DB connection class initialization. There are three cases: 
// 1. DataSource.getConnection(String user, String password)
CxList dataSourceConn = All.FindByMemberAccess("DataSource.getConnection");
// 2. DriverManager.getConnection(String url, String user, String password)
CxList driverManagerConn = All.FindByMemberAccess("DriverManager.getConnection");
// 3.1 DriverManagerDataSource.setPassword(String password)
CxList dmdsSetPassword = All.FindByMemberAccess("DriverManagerDataSource.setPassword");
// 3.2 new DriverManagerDataSource(String url, String username, String password)
CxList driverManagerDataSource = createExpressions.FindByShortName("DriverManagerDataSource");
// 4. new SimpleDriverDataSource(Driver instance, String url, String user, String password)
CxList simpleDriverDataSource = createExpressions.FindByShortName("SimpleDriverDataSource");

// Some connection initialization methods
CxList pswInitMethods = All.NewCxList();
pswInitMethods.Add(dataSourceConn);
pswInitMethods.Add(driverManagerConn);
pswInitMethods.Add(dmdsSetPassword);
pswInitMethods.Add(driverManagerDataSource);
pswInitMethods.Add(simpleDriverDataSource);

// Password parameters in connection initialization methods
CxList pswParamInMethod = All.GetParameters(dataSourceConn, 1);
CxList connectionWith3Params = driverManagerConn
	.FindByParameters(All.GetParameters(driverManagerConn, 2));
CxList connectionWith2Params = driverManagerConn
	.FindByParameters(All.GetParameters(driverManagerConn, 1)) - connectionWith3Params;
pswParamInMethod.Add(All.GetParameters(connectionWith2Params, 1));
pswParamInMethod.Add(All.GetParameters(connectionWith3Params, 2));

pswParamInMethod.Add(All.GetParameters(dmdsSetPassword, 0));
pswParamInMethod.Add(All.GetParameters(driverManagerDataSource, 2));
pswParamInMethod.Add(All.GetParameters(simpleDriverDataSource, 3));

//Get Strings influencing Connections
CxList stringsPsw = Find_Connection_String(pswParamInMethod);
CxList influencedParam = pswParamInMethod.DataInfluencedBy(stringsPsw.FindByType(typeof(StringLiteral)));
influencedParam.Add(pswParamInMethod.FindByType(typeof(StringLiteral)));
	
CxList pswStrings = influencedParam.GetStartAndEndNodes(CxList.GetStartEndNodesType.StartNodesOnly);

// More sanitizers
// Sanitize params that are not passwords in DB connection methods
CxList notPswInMethod = All.NewCxList();
notPswInMethod.Add(dataSourceConn, driverManagerConn, dmdsSetPassword,
	driverManagerDataSource, simpleDriverDataSource);
CxList notPswParamInMethod = All.GetParameters(notPswInMethod);
notPswParamInMethod -= pswParamInMethod;
sanitizers.Add(notPswParamInMethod);

sanitizers.Add(Find_Hardcoded_Key_Sanitizers());

// Add the path from the string/parameter to its method
result.Add(pswInitMethods.InfluencedByAndNotSanitized(pswStrings, sanitizers));