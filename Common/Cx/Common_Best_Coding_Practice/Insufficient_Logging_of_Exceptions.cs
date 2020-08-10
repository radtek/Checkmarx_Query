CxList logs = general.Find_Log_Outputs();
CxList frameworksCatchs = general.Find_Frameworks_Catch();
CxList catchs = Find_Catch();
catchs.Add(frameworksCatchs);
CxList declarators = Find_Declarators();
CxList methods = Find_Methods();
CxList declaratorsAndMethods = All.NewCxList();
declaratorsAndMethods.Add(methods);
declaratorsAndMethods.Add(declarators);
CxList catchScope = declaratorsAndMethods.GetByAncs(catchs);
CxList references = Find_UnknownReference();


//Get methods's parameters inside a catch that influences on the logs functions
CxList catchMethods = catchScope.FindByType(typeof(MethodInvokeExpr));
CxList catchMethodsParameters = All.GetParameters(catchMethods);
CxList parametersInfluence = logs.DataInfluencedBy(catchMethodsParameters);
CxList startNodes = parametersInfluence.GetStartAndEndNodes(CxList.GetStartEndNodesType.StartNodesOnly).FindByType(typeof(UnknownReference));
//Logs Incompleted

CxList catchDeclarators = declarators.GetByAncs(catchs);
CxList types = Find_TypeRef();
CxList exceptionTypes = types.FindByFathers(catchs);
CxList declaratorsOfExceptionType = All.NewCxList();
foreach(CxList exceptionType in exceptionTypes){
	declaratorsOfExceptionType.Add(declarators.FindByType(exceptionType.GetName()));	
}
CxList exceptionReferences = references.FindAllReferences(declaratorsOfExceptionType);
CxList logsValid = logs.FindByParameters(exceptionReferences * All.GetParameters(logs));
CxList logsInCatch = catchScope * logsValid;
CxList stackTrace_ = methods.FindByShortName("printStackTrace").GetTargetOfMembers();

CxList sanitizedScopes = All.NewCxList();
sanitizedScopes.Add(logsInCatch);
sanitizedScopes.Add(startNodes);
sanitizedScopes.Add(stackTrace_);
sanitizedScopes.Add(Find_ThrowStmt());

CxList catchsToRemove = sanitizedScopes.GetAncOfType(typeof(Catch));
catchsToRemove.Add(sanitizedScopes.GetAncOfType(typeof(MethodDecl)) * frameworksCatchs);

CxList catchsWithNoLogs = catchs - catchsToRemove;
result = catchsWithNoLogs;