CxList allMethods = Find_Methods();
CxList integerLiterals = Find_IntegerLiterals();
CxList exprStmts = Find_ExprStmt();
CxList returnStmts = Find_ReturnStmt();
CxList conditions = Find_Conditions();

CxList errorVariables = All.FindByType("error", true);
CxList methodDeclsWithErrorReturn = All.FindByMethodReturnType("error");

//Filter methodDeclsWithErrorReturn if they are testing methods
CxList testingMethods = Find_MethodDecls_Testing();
methodDeclsWithErrorReturn = methodDeclsWithErrorReturn - testingMethods;

CxList errorHandlers = Get_Error_Handlers();
errorHandlers.Add(allMethods.FindByShortName("panic"));
errorHandlers.Add(conditions);


CxList unassignedErrors = All.NewCxList();
CxList methodsReturningOneSingleError = All.NewCxList();

Dictionary<string, int> methodErrorIndexPositions = new Dictionary<string, int>();
methodErrorIndexPositions.Add("os.Open", 1);
methodErrorIndexPositions.Add("os.Close", 0);
methodErrorIndexPositions.Add("os.OpenFile", 1);
methodErrorIndexPositions.Add("os.Stat", 1);
methodErrorIndexPositions.Add("io/ioutil.ReadFile", 1);
methodErrorIndexPositions.Add("io/ioutil.WriteFile", 0);
methodErrorIndexPositions.Add("io/ioutil.ReadAll", 1);
methodErrorIndexPositions.Add("io/ioutil.ReadDir", 1);
methodErrorIndexPositions.Add("io/ioutil.TempDir", 1);
methodErrorIndexPositions.Add("io/ioutil.TempFile", 1);
methodErrorIndexPositions.Add("net/http.Get", 1);
methodErrorIndexPositions.Add("net/http.Post", 1);
methodErrorIndexPositions.Add("net/http.NewRequest", 1);


CxList multipleReturnMethods = All.NewCxList();

foreach(var methodName in methodErrorIndexPositions)
{
	CxList methods = allMethods.FindByMemberAccess(methodName.Key);
	if(methodName.Value == 0)
	{
		methodsReturningOneSingleError.Add(methods);
	}
	else 
	{
		multipleReturnMethods.Add(methods);
	}
}

foreach (CxList methodStmt in methodDeclsWithErrorReturn)
{
	MethodDecl st = methodStmt.TryGetCSharpGraph<MethodDecl>();	
	CxList methodReferences = allMethods.FindAllReferences(methodStmt);
	if(st.ReturnTypes.Count > 0)
	{
		multipleReturnMethods.Add(methodReferences);
		//Adding to the dictionary just to know the error index in the code that follows
		methodErrorIndexPositions[st.FullName] = st.ReturnTypes.Count - 1;
	}
	else if(st.ReturnType != null)
	{
		methodsReturningOneSingleError.Add(methodReferences);
	}
}

unassignedErrors.Add(methodsReturningOneSingleError.GetFathers() * exprStmts);	
errorVariables.Add(methodsReturningOneSingleError.GetAssignee());

unassignedErrors -= unassignedErrors.GetByAncs(testingMethods);
errorVariables -= errorVariables.GetByAncs(testingMethods);

//Filter if defined inside a testing method
CxList unwantedMultReturnMethods = multipleReturnMethods.GetByAncs(testingMethods);
// Filter if called in return statement
unwantedMultReturnMethods.Add(multipleReturnMethods.GetByAncs(returnStmts));

multipleReturnMethods -= unwantedMultReturnMethods;

foreach(CxList methodCall in multipleReturnMethods)
{
	CxList declarators = methodCall.GetAssignee();
	if (declarators.Count == 0)
	{
		unassignedErrors.Add(methodCall);
	}
	else
	{
		MethodInvokeExpr mie = methodCall.TryGetCSharpGraph<MethodInvokeExpr>();
		if (mie != null && methodErrorIndexPositions.ContainsKey(mie.FullName))
		{
			int errorVarPosition = methodErrorIndexPositions[mie.FullName];
			CxList errorVar = declarators.GetCxListByPath().ElementAtOrDefault(errorVarPosition);
			if (errorVar != null)
			{
				errorVariables.Add(errorVar);	
			}
		}
	}
}
//-----  All errorVariables were discovered ------------//


//Remove unused variables
CxList rightSides = All.FindByAssignmentSide(CxList.AssignmentSide.Right);
errorVariables *= rightSides.GetAssignee();


//Remove member access
errorVariables -= errorVariables.FindByType(typeof(MemberAccess));

// Remove returned error variables
CxList errorReferences = All.FindAllReferences(errorVariables);
CxList errorReferencesInReturn = errorReferences.GetByAncs(returnStmts);
errorReferences -= errorReferencesInReturn;


CxList errorVariablesHandled = All.NewCxList();
errorVariablesHandled.Add(errorVariables.GetByAncs(Find_FieldDecls()));
errorVariablesHandled.Add(errorVariables.FindDefinition(errorReferencesInReturn));
errorVariablesHandled.Add(errorVariables.FindAllReferences(errorReferencesInReturn));
errorVariablesHandled.Add(errorReferences.DataInfluencingOn(errorHandlers).GetStartAndEndNodes(CxList.GetStartEndNodesType.StartNodesOnly));

errorVariables = errorVariables - errorVariablesHandled;


result.Add(unassignedErrors);
result.Add(errorVariables);