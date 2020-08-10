/*	Find classes that implement MessageListener
*	From those classes, get the declaration of the method onMessage(Message m)
*	Inside the onMessage method, it is unsafe to access some members of the 
*	message given as parameter unless it has been already cast to a trusted type object
*/

CxList unkRefs = Find_UnknownReference();
CxList paramDecls = Find_ParamDecl();
CxList methodDecls = Find_MethodDecls();
CxList methods = Find_Methods();
CxList castExprs = Find_CastExpr();

CxList outputs = methods.FindByMemberAccess("Message.*");
outputs.Add(Find_Interactive_Outputs());
outputs.Add(Find_Log_Outputs());

CxList onMessageMethods = methodDecls.FindByType(typeof(MethodDecl)).FindByShortName("onMessage");
CxList methodsFromMessageListener = onMessageMethods.GetByAncs(All.InheritsFrom("MessageListener"));
CxList inputs = paramDecls.GetByAncs(methodsFromMessageListener).FindByType("Message");

CxList objectMessages = All.FindByType("ObjectMessage");

CxList castsToObjectMessage = objectMessages.GetAncOfType(typeof(CastExpr));

CxList sanitizers = castExprs.GetAncOfType(typeof(CastExpr));
sanitizers -= castsToObjectMessage;

/*
*	Add unknown references used inside a try-catch after beeing casted to safe types 
*	Ex.: 
*	// "message" is sanitized in the try-catch scope after the cast to TextMessage but not before
*		try {
*			System.out.println(message.toString());
*			TextMessage textMessage = (TextMessage) message; // will fail if message is an ObjectMessage
*			saveMessage(message.toString()); // Processing will only occur if message is a safe type
*		} catch (ClassCastException e) {
*			...
*		}
*/
CxList tryCatchFinally = Find_TryCatchFinallyStmt();
CxList catches = Find_Catch();

// Get all sanitizers inside try-catch
CxList sanitizersInsideTryCatch = sanitizers.GetByAncs(tryCatchFinally);
// Get the definitions of those sanitizers
CxList definitionsOnTryCatch = paramDecls.FindDefinition(unkRefs.FindByFathers(sanitizersInsideTryCatch));
// Get all references of those sanitizers
CxList potentiallySanitizedRefs = unkRefs.FindAllReferences(definitionsOnTryCatch);
// Get all try-catchs with sanitizers inside
CxList tryCatchsWithSanitizers = sanitizers.GetAncOfType(typeof(TryCatchFinallyStmt));

// Get all casts except casts to ObjectMessage
CxList casts = All.InfluencedBy(inputs).GetAncOfType(typeof(CastExpr));
casts -= castsToObjectMessage;

// Check witch references are actually sanitized 
CxList betweenCastAndCatch = All.NewCxList();
foreach(CxList tcf in tryCatchsWithSanitizers)
{
	CxList cat = catches.GetByAncs(tcf);
	CxList cast = casts.GetByAncs(tcf);
	betweenCastAndCatch.Add(potentiallySanitizedRefs.FindInScope(cast, cat));
}

sanitizers.Add(betweenCastAndCatch);

/*	Find cases where conditions use instanceof (two cases)
*	1. x instanceof y, where y is not of type ObjectMessage
*	2. !(x instanceof ObjectMessage)
*/
CxList instanceOfs = All.GetByBinaryOperator(BinaryOperator.InstanceOf);
CxList nots = Find_Unarys().FindByShortName("Not");

CxList instanceOfObjectMessage =
	objectMessages.GetByAncs(instanceOfs).GetAncOfType(typeof(BinaryExpr)).GetByBinaryOperator(BinaryOperator.InstanceOf);

CxList ifSanitizers = instanceOfObjectMessage.GetByAncs(nots).GetAncOfType(typeof(IfStmt));

CxList instanceOfsAll = All.NewCxList();
instanceOfsAll.Add(instanceOfs);
instanceOfsAll -= instanceOfObjectMessage;

ifSanitizers.Add(instanceOfsAll.GetAncOfType(typeof(IfStmt)));

sanitizers.Add(All.GetByAncs(ifSanitizers));
sanitizers.Add(ifSanitizers);

/*
*	Add safe ObjectMessage/Message methods to sanitizers (methods to be ignored)
*/
CxList messageReferences = unkRefs.FindByType("Message");
messageReferences.Add(unkRefs.FindByType("ObjectMessage"));
CxList messageMembers = messageReferences.GetMembersOfTarget();
CxList safeMessageMembers = messageMembers - messageMembers.FindByShortNames(new List<string> {"toString", "getObject"});
sanitizers.Add(safeMessageMembers);

result = inputs.InfluencingOnAndNotSanitized(outputs, sanitizers).ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow);