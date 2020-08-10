CxList methods = Find_Methods();
CxList unkRefs = Find_UnknownReference();
CxList unkRefsAndMethods = methods + unkRefs;

// Known types that extend SQLOperations
string[] jdbcTypes = new string[] {
	"SQLOperations", "SQLClient", "JDBCClient", "SQLConnection",
	"AsyncSQLClient", "MySQLClient", "PgConnection"
};

List<string> jdbcMethodsNames = new List<string> {
	"query*", "execute*", "update*", "call*", "batch*"
};

// Find Pool objects (can be used to get SQLConnection or execute methods)
CxList poolMethods = methods.FindByMemberAccesses(new string[] {
		"PgPool.pool", "MySQLPool.pool", "MSSQLPool.pool", "DB2Pool.pool"});

CxList poolRefs = All.FindByTypes(new string[]{"Pool", "PgPool", "MySQLPool"});
poolRefs.Add(poolMethods.GetAssignee());

CxList typesRefs = unkRefs.FindByTypes(jdbcTypes);
typesRefs.Add(unkRefs.FindAllReferences(poolRefs));

CxList getConnection = typesRefs.GetMembersOfTarget().FindByShortName("getConnection*");
// await version
typesRefs.Add(unkRefs.FindAllReferences(getConnection.GetAssignee()));
// add refs and method inside lambda callback
typesRefs.Add(unkRefsAndMethods.GetByAncs(getConnection));

CxList jdbcMethods = typesRefs.GetMembersOfTarget().FindByShortNames(jdbcMethodsNames);
jdbcMethods.Add(methods.FindByMemberAccesses(new string[] {
		"MySQLPool.query", "Pool.query", "PgPool.query", "MSSQLPool.query", "DB2Pool.query"}));
result.Add(jdbcMethods);