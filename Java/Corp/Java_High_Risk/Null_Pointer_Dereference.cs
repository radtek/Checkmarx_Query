// Query Null_Pointer_Dereference
// ==============================

/*

      // Gets the system property indicated by the specified key
      String cmd = System.getProperty( "java.class.path" );
      
      // BUG
      // Returns the string, with leading and trailing whitespace omitted
      cmd = cmd.trim();
      
      // If java.class.path is undefined, then return value of
      // systemProperty is undefined. So cmd is not defined.
      // Thus, calling cmd.trim() causes a NULL dereference exception.

*/
/*
CxList infByGetProp = All.DataInfluencedBy(All.FindByName("*System.getProperty*"));
CxList methodInvokeExpr = infByGetProp.FindByType(typeof(MethodInvokeExpr));
CxList nullLiteral =  All.FindByType(typeof(NullLiteral));
CxList nullIfStmt = nullLiteral.GetAncOfType(typeof(IfStmt));
CxList methodInvokeExprUnderIf = methodInvokeExpr.GetByAncs(nullIfStmt);

result =  methodInvokeExpr - methodInvokeExprUnderIf;
*/


// General members
CxList memberAccess = All.FindByType(typeof(MemberAccess));
CxList methods = Find_Methods();

// Add the FindByRegex, because the default initialization of an object is to null
//添加FindByRegex，因为对象的默认初始化为null
CxList nullLiteral = All.FindByType(typeof(NullLiteral)).FindByRegex("null");

// All "null" that are set as an initialvalue of an array member
//所有设置为数组成员初始值的“null”
CxList nullInArray = nullLiteral.GetByAncs(All.FindByType(typeof(ArrayInitializer)));

// Remove the cases where there are "null in member" from the "set as null" cases
nullLiteral -= nullInArray;

// All if-statements that check "null"
CxList nullIfStmt = nullLiteral.GetByAncs(Find_Conditions()).GetAncOfType(typeof(IfStmt));

// Unknown references, that are not under the "null" if-statement (otherwise - irrelevant
CxList unknown = All.FindByType(typeof(UnknownReference));
unknown -= unknown.GetByAncs(nullIfStmt + Find_Dead_Code());

// All "unknown" methods (no definition) are null-potential
CxList undeclaredMethods = methods - methods.FindAllReferences(All.FindDefinition(methods));
// ...except for length, index and alike
undeclaredMethods -= Find_Integers().GetTargetOfMembers().FindAllReferences(nullInArray);
// ...except for write or print
undeclaredMethods -= undeclaredMethods.FindByShortName("*write*", false);
undeclaredMethods -= undeclaredMethods.FindByShortName("*print*", false);
undeclaredMethods -= undeclaredMethods.FindByShortName("*toString*", false);
undeclaredMethods -= undeclaredMethods.FindByShortName("substring");
undeclaredMethods -= undeclaredMethods.FindByShortName("append*");
undeclaredMethods -= undeclaredMethods.FindByShortName("getClass");
undeclaredMethods -= undeclaredMethods.FindByShortName("replace");
undeclaredMethods -= undeclaredMethods.FindByShortName("getRequest");
undeclaredMethods -= undeclaredMethods.FindByShortName("getPackage");
undeclaredMethods -= undeclaredMethods.FindByShortName("getSession");
undeclaredMethods -= undeclaredMethods.FindByShortName("getUserPrincipal");
undeclaredMethods -= undeclaredMethods.FindByShortName("getCurrentUser");
undeclaredMethods -= undeclaredMethods.FindByMemberAccess("Class.*");

// More likely that we are looking for methods that are actually properties of some object
undeclaredMethods = undeclaredMethods.GetTargetOfMembers().GetMembersOfTarget();

///Case 1 - Members of an unknown reference target that is influenced by null literal

// Find membes of all unknown references that are influenced by a null literal
CxList unknownMembers = unknown.GetMembersOfTarget().GetTargetOfMembers();
// Remove the cases where the last influence was by an assign operation
// (we need things that were set as null before this instance)
unknownMembers -= All.FindByType(typeof(MemberAccess))
	.GetByAncs(All.FindByAssignmentSide(CxList.AssignmentSide.Left))
	.GetTargetOfMembers();

CxList returnStmt = All.GetByAncs(All.FindByType(typeof(ReturnStmt)));
CxList parameters = All.GetByAncs(undeclaredMethods) - undeclaredMethods;
CxList bin = All.FindByType(typeof(BinaryExpr));
CxList sanitizer = returnStmt + parameters + bin + Find_Dead_Code_Contents();

// Find all members that were influenced by null
CxList memberByNull = nullLiteral.InfluencingOnAndNotSanitized(unknownMembers, sanitizer);

///Case 2 - Relevant methods that are directly null literals

CxList membersNull = undeclaredMethods.GetMembersOfTarget();
membersNull -= membersNull.GetByAncs(nullIfStmt);

///Case 3 - Getting a member of an array that was initialized with at least one member which is null

CxList nonIntegers = All - Find_Integers();
// Arrays that are initialized with at least one member that is null
CxList arrayWithNull = nullInArray.GetAncOfType(typeof(Declarator));
// Find all references of these arrays, that that do not have - length, index and alike - member
CxList arrayWithNullRef = nonIntegers.GetTargetOfMembers().FindAllReferences(arrayWithNull);
// Get the relevant member
CxList nullInArrayInit = arrayWithNullRef.GetMembersOfTarget();
// and leave only non-(null-if-statemtnt) results
nullInArrayInit -= nullInArrayInit.GetByAncs(nullIfStmt);

/// Result - all together
result = membersNull + memberByNull + nullInArrayInit;

result -= Find_Dead_Code_Contents();
result -= result.GetMembersOfTarget();
