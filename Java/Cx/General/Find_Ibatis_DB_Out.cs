CxList methods = Find_Methods();
CxList fields = Find_Field_Decl();
CxList ibatisSrc = All.NewCxList();
ibatisSrc.Add(All.InheritsFrom("SqlMapClientOperations"));
ibatisSrc.Add(All.InheritsFrom("SqlMapClientTemplate"));
ibatisSrc.Add(All.InheritsFrom("SqlMapClientImpl"));
ibatisSrc.Add(All.InheritsFrom("SqlMapClient"));

CxList ibatisRef = All.FindAllReferences(ibatisSrc) - ibatisSrc;
CxList ibatisReff = ibatisRef.GetFathers();
CxList ibatis = ibatisReff.FindByType(typeof(MethodDecl));
ibatis.Add(ibatisReff * fields);
ibatis = All.FindAllReferences(ibatis);

CxList ibatisTypes = All.FindByTypes(new string [] {
	"SqlMapClientOperations", "SqlMapClientTemplate",
	"SqlMapClient",	"SqlMapClientImpl"});
ibatisTypes.Add(methods.FindByShortNames(new List<string> {"getSqlMap", "getSqlMapClientTemplate"}));

CxList allIbatis = All.NewCxList();
allIbatis.Add(ibatisRef);
allIbatis.Add(ibatis);
allIbatis.Add(ibatisTypes);

CxList ibatisTarget = allIbatis.GetMembersOfTarget();

List<string> queryMethodsList = new List<string> {
		"queryForObject",
		"queryForList", 
		"queryForPaginatedList",
		"queryForMap",
		"insert",
		"execute",
		"executeWithListResult",
		"executeWithMapResult"};

CxList sqlmapClientMembers = All.NewCxList();
sqlmapClientMembers.Add(methods.FindByMemberAccess("SqlMapClientOperations.*"));
sqlmapClientMembers.Add(methods.FindByMemberAccess("SqlMapClientImpl.*"));
sqlmapClientMembers.Add(methods.FindByMemberAccess("SqlMapClient.*"));
sqlmapClientMembers.Add(methods.FindByMemberAccess("SqlMapClientTemplate.*"));

CxList searchSpace = All.NewCxList();
searchSpace.Add(ibatisTarget);
searchSpace.Add(sqlmapClientMembers);

CxList queryMethods = searchSpace.FindByShortNames(queryMethodsList);

CxList assignees = queryMethods.GetAssignee();
CxList methodsWithoutAssignee = queryMethods - assignees.GetAssigner();

result.Add(assignees);
result.Add(methodsWithoutAssignee);