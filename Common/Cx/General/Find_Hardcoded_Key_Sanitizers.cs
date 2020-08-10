// All parameters of the following methods are safe
CxList sanitizers = All.NewCxList();
CxList strLiterals = Find_PrimitiveExpr();
CxList binaryExpr = Find_BinaryExpr();
CxList createExpressions = Find_ObjectCreations();
CxList variables = Find_UnknownReference();
CxList indexes = Find_IndexerRefs();

foreach(CxList idx in indexes){
	try {
		IndexerRef index = idx.TryGetCSharpGraph<IndexerRef>();
		if(index.Indices.Count > 0 && index.Indices[0] != null){
			var indexValue = index.Indices[0];
			if(All.FindById(indexValue.NodeId).FindByAbstractValue(x => x is AnyAbstractValue).Count > 0){
				sanitizers.Add(All.FindById(index.Target.NodeId));
			}
		}
	}catch(Exception){}
}

// All parameters of the following methods are safe
CxList otherSafeMethods = All.NewCxList();
CxList methods = Find_Methods();
otherSafeMethods.Add(methods.FindByMemberAccess("String.split"));
otherSafeMethods.Add(methods.FindByMemberAccess("String.substring"));
otherSafeMethods.Add(methods.FindByMemberAccess("HttpServletRequest.get*"));
otherSafeMethods.Add(methods.FindByMemberAccess("Class.getResource"));
sanitizers.Add(All.GetParameters(otherSafeMethods));


sanitizers.Add(All.GetParameters(methods.FindByMemberAccess("string.getBytes", false)));
sanitizers.Add(All.GetParameters(methods.FindByShortName("copyOf"), 1));
// Add the first parameter of replaceAll to sanitizers
sanitizers.Add(All.GetParameters(methods.FindByShortName("replaceAll"), 0));
CxList randomValues = Find_Random();
randomValues.Add(createExpressions.FindByShortName("SecureRandom"));
randomValues.Add(All.GetParameters(randomValues));
CxList secureRandom = methods.FindByMemberAccess("SecureRandom.*");
randomValues.Add(secureRandom);


List<string> logicalBinaryExpr = new List<string> {"==","!=",">",">=","<","<=","&&","||","<<",">>",">>>","&","^","|" }; 
CxList binaryExprSanitized = binaryExpr.FindByShortNames(logicalBinaryExpr);

sanitizers.Add(randomValues);
sanitizers.Add(methods.FindByMemberAccess("Properties.get*"));
sanitizers.Add(methods.FindByMemberAccess("request.get*"));
sanitizers.Add(methods.FindByMemberAccess("InputStream.read"));
sanitizers.Add(methods.FindByMemberAccess("*.getInstance"));
sanitizers.Add(methods.FindByMemberAccess("Paths.get"));
sanitizers.Add(methods.FindByMemberAccess("JSONObject.getJSONObject"));
sanitizers.Add(methods.FindByMemberAccess("JSONObject.getJSONArray"));
sanitizers.Add(methods.FindByMemberAccess("JsonValue.get"));
sanitizers.Add(methods.FindByMemberAccess("FileSystem.getPath"));
sanitizers.Add(methods.FindByMemberAccess("FileSystem.provider").GetMembersOfTarget().FindByShortName("getPath"));
sanitizers.Add(Find_CollectionAccesses());
sanitizers.Add(binaryExprSanitized);

CxList sanitizedMembers = methods.FindByMemberAccess("*.replace");
sanitizedMembers.Add(methods.FindByMemberAccess("*.getBytes"));
sanitizedMembers.Add(methods.FindByMemberAccess("*.readAllBytes"));
CxList sanitizedParameters = strLiterals.GetParameters(sanitizedMembers);
sanitizedMembers = sanitizedMembers.GetTargetOfMembers();
sanitizedMembers = sanitizedMembers.NotInfluencedBy(strLiterals - sanitizedParameters);
sanitizedMembers = sanitizedMembers.GetMembersOfTarget();
sanitizers.Add(sanitizedMembers);


sanitizers.Add(methods.FindByMemberAccess("Charset.forName"));
sanitizers.Add(createExpressions.FindByShortName("File"));
sanitizers.Add(createExpressions.FindByShortName("SimpleDateFormat"));
sanitizers.Add(methods.FindByMemberAccess("Boolean.booleanValue"));
sanitizers.Add(variables.FindByType("boolean"));
sanitizers.Add(strLiterals.FindByShortNames(new List<String>{
		":","\\","/",".","+","-","%","#","!","?","_",".//"
		}));

result = sanitizers;