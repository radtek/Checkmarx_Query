CxList methods = Find_Methods();
CxList returns = Find_ReturnStmt();
CxList myMethods = Find_Throws_Methods();
CxList objects = Find_Object_Create();
CxList allRefsInProj = Find_TypeRef();
CxList allConstructorDeclInProj = Find_ConstructorDecl();
CxList allStatementCollection = Find_StatementCollection();
CxList Trys = Find_TryCatchFinallyStmt();

CxList resourceReleaseSanitizers = Find_Resource_Release_Sanitizers();


List<string> relevantTypes = new List<string> {
		"AbstractInterruptibleChannel", "AbstractSelectableChannel", "AbstractSelector", "AsynchronousFileChannel", 
		"AsynchronousServerSocketChannel", "AsynchronousSocketChannel", "AudioInputStream", "AutoCloseable",
		"CharArrayReader", "CharArrayWriter", "CheckedInputStream", "CheckedOutputStream", "CipherInputStream", 
		"CipherOutputStream", "Closeable","CloseableHttpClient","CloseableHttpResponse", "DatagramChannel", "DatagramSocket", "DataInputStream", "DataOutputStream", 
		"Deflater", "DeflaterInputStream", "DeflaterOutputStream", "DigestInputStream", "DigestOutputStream", "FileCacheImageInputStream",
		"FileCacheImageOutputStream", "FileChannel", "FileImageInputStream", "FileImageOutputStream", "FileInputStream", "FileLock",
		"FileOutputStream", "FileReader", "FileSystem", "FileWriter", "FilterInputStream", "FilterOutputStream", "FilterReader",
		"FilterWriter", "Formatter", "ForwardingJavaFileManager", "GZIPInputStream", "GZIPOutputStream", "ImageInputStreamImpl",
		"ImageOutputStreamImpl", "Inflater", "InflaterInputStream", "InflaterOutputStream", "InputStream", "InputStreamLogger",
		"InputStreamReader", "JarFile", "JarInputStream", "JarOutputStream", "LineNumberInputStream", "LineNumberReader", 
		"LogStream", "MemoryCacheImageInputStream", "MemoryCacheImageOutputStream", "MLet", "MulticastSocket", "ObjectInputStream", 
		"ObjectOutputStream", "OutputStream", "OutputStream", "OutputStreamWriter", "SinkChannel", "SourceChannel", "PipedInputStream", 
		"PipedOutputStream", "PipedReader", "PipedWriter", "PrintStream", "PrintWriter", "PrivateMLet", "ProgressMonitorInputStream", "PushbackInputStream", "PushbackReader", "RandomAccessFile",
		"Reader", "RMIConnectionImpl", "RMIConnectionImpl_Stub", "RMIConnector", "RMIIIOPServerImpl", "RMIJRMPServerImpl", "RMIServerImpl", "Scanner", "SelectableChannel", "Selector", 
		"SequenceInputStream", "ServerSocket", "ServerSocketChannel", "Socket", "SocketChannel", "SSLServerSocket", "SSLSocket", 
		"StringBufferInputStream", "StringReader", "URLClassLoader", "Writer", "XMLDecoder", "XMLEncoder", 
		"ZipFile", "ZipInputStream", "ZipOutputStream", "Connection"};

// Get all classes inherits
CxList ClassInheritsFrom = All.NewCxList();
CxList ClassDeclList = Find_ClassDecl();	 

List<string> inheriting = new List<string>();
foreach( string c in relevantTypes){
	string className = ClassDeclList.InheritsFrom(c).GetName();
	if(!String.IsNullOrEmpty(className) && !relevantTypes.Contains(className)) 
		inheriting.Add(className);
}
relevantTypes.AddRange(inheriting);

CxList allResourcesInProj = (objects.FindByShortNames(relevantTypes));
CxList wrappingObjects = All.FindAllReferences(All.GetClass(allResourcesInProj.GetAncOfType(typeof(ConstructorDecl))))
	.GetFathers().FindByType(typeof(ObjectCreateExpr));


allResourcesInProj -= resourceReleaseSanitizers;

//methods where resources are opened
CxList auxOpenedMethods = All.FindAllReferences(myMethods.GetMethod(allResourcesInProj.FindByFathers(returns)))
								.FindByType(typeof(MethodInvokeExpr));
allResourcesInProj.Add(auxOpenedMethods);
allResourcesInProj.Add(allResourcesInProj.GetByAncs(myMethods.GetMethod(allResourcesInProj.FindByFathers(returns))));

CxList allOpen = (methods.FindByShortNames(new List<string> {
		"getConnection", 
		"open",
		"getOutputStream",
		"getInputStream",
		"getWriter",
		"getReader",
		"getResourceAsStream",
		"openOutputStream",
		"openFileOutput",
		"makeConnection",
		"getErrorStream",
		"streamContent",
		"createInputStream"
		}));

CxList unAccountable = allOpen.FindByMemberAccess("HttpServletRequest.*");
unAccountable.Add(allOpen.FindByMemberAccess("HttpServletResponse.*"));
allOpen -= unAccountable;

allResourcesInProj.Add(allOpen);

allResourcesInProj.Add(methods.FindByMemberAccess("MimeUtility.encode"));
allResourcesInProj.Add(methods.FindByMemberAccess("MimeUtility.decode"));

allResourcesInProj.Add(methods.FindByMemberAccess("HttpClients.create*"));
allResourcesInProj.Add(methods.FindByMemberAccess("CloseableHttpClient.execute"));

allResourcesInProj.Add(methods.FindByMemberAccess("Session.connection"));

allResourcesInProj.Add(methods.FindByMemberAccess("URL.openStream"));
allResourcesInProj.Add(methods.FindByMemberAccess("Transaction.begin"));

CxList returningConnection = All.FindAllReferences(allOpen.GetAssignee()).FindByFathers(returns);
returningConnection.Add(allOpen.FindByFathers(returns));

allResourcesInProj.Add(All.FindAllReferences(myMethods.GetMethod(returningConnection)).FindByType(typeof(MethodInvokeExpr)));
// avoid encapsulating objects (considering the resource is closed by the wrapping object)


allResourcesInProj -= allResourcesInProj.GetParameters(allResourcesInProj.FindByParameters(allResourcesInProj));

CxList ThrowingReferences = All.FindAllReferences(allResourcesInProj.GetAssignee());
ThrowingReferences.Add(All.FindAllReferences(All.FindByType(typeof(TypeRef)).FindByFathers(All.FindByType(typeof(CastExpr)))
	.FindByTypes(relevantTypes.ToArray()).GetFathers().GetAssignee()));

ThrowingReferences.Add(ThrowingReferences.GetMembersOfTarget());
	
allResourcesInProj = allResourcesInProj.InfluencingOnAndNotSanitized(ThrowingReferences, resourceReleaseSanitizers);

CxList TryEnds = allResourcesInProj.GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly);

CxList TryBlocks = All.NewCxList();
foreach(CxList tryCatch in Trys){
	try{
		TryCatchFinallyStmt tryGraph = tryCatch.TryGetCSharpGraph<TryCatchFinallyStmt>();
		if(tryGraph.Try != null){
			
			TryBlocks.Add(tryGraph.Try.NodeId, tryGraph.Try);
		}		
	}catch(Exception ex){
		
	}
}
CxList nodeObjectDeclarator = allResourcesInProj.GetStartAndEndNodes(CxList.GetStartEndNodesType.StartNodesOnly);
nodeObjectDeclarator.Add(wrappingObjects);

CxList TryBlock = nodeObjectDeclarator.GetByAncs(TryBlocks);

CxList allClose = methods.FindByShortName("close");

allClose -= Find_Dead_Code();
//search for all invocations of methods which close the resource
CxList auxClosedMethods = All.FindAllReferences(myMethods.GetMethod(allClose)).FindByType(typeof(MethodInvokeExpr));

CxList closes = TryEnds * allClose;
//methods close transaction from Apache Transaction 
closes.Add(methods.FindByMemberAccess("Transaction.safeRollback"));
closes.Add(methods.FindByMemberAccess("Transaction.rollback"));
closes.Add(methods.FindByMemberAccess("Transaction.commit"));

CxList closedResources = All.NewCxList();
foreach(CxList myTry in TryBlock){
	CxList curFinally = allStatementCollection.GetFinallyClause(myTry.GetAncOfType(typeof(TryCatchFinallyStmt)));
	try{		
		//we test if we have a try statement 
		if(curFinally.TryGetCSharpGraph<StatementCollection>().Count > 0){
			// remove resource that close on the finally block
			CxList myList = All.FindAllReferences(closes.GetByAncs(curFinally).GetTargetOfMembers()).GetAssigner();
			myList.Add(All.FindByFathers(myList.FindByType(typeof(CastExpr))));
			closedResources.Add(myList.GetByAncs(myTry));
			
			// remove resources that are wrapped and are closed along recurring to auxiliar method
			CxList openedResources = All.FindByFathers(All.FindAllReferences(auxClosedMethods.GetByAncs(curFinally).GetTargetOfMembers()).GetByAncs(TryBlocks).GetAncOfType(typeof(AssignExpr))).GetAssignee();
			CxList resourcesWrapperClass = All.FindDefinition(allRefsInProj.FindByFathers(openedResources.GetAssigner().FindByType(typeof(ObjectCreateExpr))));
			CxList ResourcesAllocatedOnWrapperConstructor = allResourcesInProj.GetByAncs(allConstructorDeclInProj.GetByAncs(resourcesWrapperClass));
			closedResources.Add(ResourcesAllocatedOnWrapperConstructor);
			CxList resource_closer = All.GetParameters(auxClosedMethods).GetByAncs(curFinally);
			closedResources.Add(All.FindAllReferences(resource_closer).FindByAssignmentSide(CxList.AssignmentSide.Left).GetAssigner());
		
			// remove resources that are wrapped, opened through a method and are close along recurring to auxiliar method 
			CxList methodsReturningResources = All.FindDefinition(openedResources.GetAssigner().FindByType(typeof(MethodInvokeExpr)));
			closedResources.Add(allResourcesInProj.GetByAncs(allResourcesInProj.GetByAncs(methodsReturningResources)));
		
		}
	}catch(Exception ex){
		
	}
}

/*
remove all resources that are closed at some "try" .
*/
CxList closableTry = methods.FindByShortName("close").GetByAncs(TryBlocks);

CxList resourceReferenceTry = closableTry.GetTargetOfMembers() - methods;

CxList resourceRefs = All.FindAllReferences(resourceReferenceTry).GetByAncs(closableTry.GetAncOfType(typeof(MethodDecl)));

CxList closeAndResourceTrys = resourceRefs.GetAncOfType(typeof(TryCatchFinallyStmt)) * 
							closableTry.GetAncOfType(typeof(TryCatchFinallyStmt));

closedResources.Add(resourceRefs - resourceRefs.GetByAncs(closeAndResourceTrys));
closedResources.Add(closableTry - closableTry.GetByAncs(closeAndResourceTrys));
/*
remove all resources that are closed at some "finally" even if not located under try.
*/
CxList allFinallies = allStatementCollection.GetFinallyClause(All.FindByType(typeof(TryCatchFinallyStmt)));
CxList closable = methods.FindByShortName("close").GetByAncs(allFinallies);
closedResources.Add(closable);
CxList resourceReference = closable.GetTargetOfMembers();
closedResources.Add(All.FindAllReferences(resourceReference));

CxList TryBlockMethods = TryBlock.FindByType(typeof(MethodInvokeExpr)) * auxOpenedMethods;

CxList mDefinitions = All.FindDefinition(TryBlockMethods);
//list with the resource open in auxiliar methods
//this methods are invoke inside a try block (mDefinitions)
CxList auxResourcesOpen = All.GetByAncs(mDefinitions).FindByType(typeof(ObjectCreateExpr));
auxResourcesOpen.Add(closedResources);

CxList tempResult = allResourcesInProj - auxResourcesOpen;

// remove methods wrapping unAccountable
CxList paramsOftempResult = All.GetParameters(tempResult);
CxList paramOfClosedStr = paramsOftempResult * unAccountable;
paramOfClosedStr.Add(unAccountable.DataInfluencingOn(paramsOftempResult));
CxList wrapperOFunAccountable = tempResult.FindByParameters(paramOfClosedStr);
tempResult -= wrapperOFunAccountable; 	
List<string> autocloseable = new List<string> {"BufferedReader","BufferedWriter", "BufferedInputStream", "BufferedOutputStream", "ResponseEntity"};
// Get all classes inherits from autocloseable
CxList classNames = ClassDeclList.InheritsFrom("AutoCloseable");
foreach(CxList c in classNames){
	string clsName = c.GetName();
	autocloseable.Add(clsName);
}

CxList paramOfBuffered = All.GetParameters(All.FindByShortNames(autocloseable));
CxList closedByBuffered = tempResult.InfluencingOn(paramOfBuffered) + paramOfBuffered; 
tempResult -= closedByBuffered;

CxList addResult = All.NewCxList();

foreach(CxList res in tempResult.GetCxListByPath()){
	CxList allNodes = res.GetStartAndEndNodes(CxList.GetStartEndNodesType.AllNodes);
	if((allNodes * closedResources).Count == 0){
		addResult.Add(res);
	}
}


Dictionary<int,Tuple<int, CxList>> validFlows = new Dictionary<int, Tuple<int, CxList>>();
foreach (CxList res in addResult.GetCxListByPath()){
	try{
		CSharpGraph init = res.GetStartAndEndNodes(CxList.GetStartEndNodesType.StartNodesOnly).GetFirstGraph();
		CxList allNodes = res.GetStartAndEndNodes(CxList.GetStartEndNodesType.AllNodes);
		int numNodes = allNodes.Count;
		if(init != null){
			if(validFlows.ContainsKey(init.NodeId)){
				Tuple<int,CxList> flowElem = validFlows[init.NodeId];
				if(numNodes > flowElem.Item1){
					validFlows[init.NodeId] = new Tuple<int,CxList>(numNodes, res.Clone());
				}
			}
			else{
				validFlows[init.NodeId] = new Tuple<int, CxList>(numNodes, res.Clone());
			}
		}
	}catch(Exception ex){
		
	}
}

foreach(KeyValuePair<int,Tuple<int,CxList>> elem in validFlows){
	result.Add(elem.Value.Item2);
}