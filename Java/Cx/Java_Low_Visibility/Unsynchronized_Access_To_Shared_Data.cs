CxList logs = All.FindByTypes(new string [] {"Log", "Logger"});
// Remove the ThreadLocal because it's a thread safe object
CxList localThread = All.FindByType("ThreadLocal");
// Remove Location.getLocation static field because it is never changed after its initialization
CxList locations = All.FindByMemberAccess("Location.getLocation").GetAncOfType(typeof(FieldDecl));

CxList toRemove = All.NewCxList();
toRemove.Add(logs, localThread, locations);

// Remove false sinks such asTypeRef and GenericTypeRef
CxList noLogs = All - logs;
noLogs -= noLogs.FindByType(typeof(ThisRef));

CxList statics = noLogs.FindByFieldAttributes(Modifiers.Static);
CxList unwanted = noLogs.FindByFieldAttributes(Modifiers.Sealed | Modifiers.Readonly);
unwanted.Add(locations);
statics -= unwanted;

CxList staticsRefs = noLogs.FindAllReferences(statics);
staticsRefs -= Find_Methods();

CxList staticDecl = staticsRefs.FindByType(typeof(Declarator));
staticsRefs -= staticDecl;
 
CxList inputs = Find_Interactive_Inputs();
inputs.Add(All.FindByMemberAccess("ServerRequest.getAttribute"));

CxList threadSafetyIssue = inputs.InfluencingOnAndNotSanitized(staticsRefs, staticDecl);

CxList inputInitializers = inputs.GetByAncs(staticDecl);
foreach (CxList initializedInput in inputInitializers)
{
	CxList decl = initializedInput.GetAncOfType(typeof(Declarator));
	threadSafetyIssue.Add(initializedInput.ConcatenateAllTargets(decl));
}

CxList Servlet = All.FindByShortName("HttpServletRequest");
CxList ServletClasses = All.GetClass(Servlet);

CxList StaticFields = Find_Field_Decl().FindByFieldAttributes(Checkmarx.Dom.Modifiers.Static);
StaticFields -= toRemove;
StaticFields = All.FindAllReferences(StaticFields) - StaticFields;
StaticFields = StaticFields.GetByAncs(ServletClasses);

// remove init() method of HttpServlet because it's only run once
// in one thread during the application life cycle
CxList initMethods = Find_MethodDecls().FindByShortName("init").GetByAncs(ServletClasses);
StaticFields -= StaticFields.GetByAncs(initMethods);

CxList Locks = All.FindByType(typeof(LockStmt));
StaticFields = StaticFields - StaticFields.GetByAncs(Locks);

CxList nulls = Find_NullLiteral();
nulls -= nulls.FindByRegex("null");
nulls.Add(StaticFields, StaticFields.GetMembersOfTarget());
CxList nonNull = All - nulls;
StaticFields = StaticFields.DataInfluencingOn(nonNull).DataInfluencedBy(nonNull)
				.GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly)
			+ StaticFields.GetByAncs(All.FindByType(typeof(PostfixExpr)).GetFathers());

CxList StaticFieldsDef = All.FindDefinition(StaticFields);
StaticFields -= StaticFieldsDef;

result = threadSafetyIssue;
result.Add(StaticFields);