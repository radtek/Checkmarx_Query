CxList objCreations = Find_ObjectCreations();
CxList methods = Find_Methods();
CxList nullLiteral = All.FindByName("null");
CxList booleans = Find_BooleanLiteral();
booleans.Add(All.FindByType("bool"));

CxList nullsAndBooleans = All.NewCxList();
nullsAndBooleans.Add(nullLiteral);
nullsAndBooleans.Add(booleans);

CxList strLiterals = Find_PrimitiveExpr();
strLiterals -= nullsAndBooleans;

CxList irrelevantStrings = strLiterals.FindByRegex(@"(BEGIN|END).*KEY", false, true, true);
strLiterals -= irrelevantStrings;

strLiterals -= strLiterals.FindByShortName("sun.*");

CxList integerLiterals = strLiterals.FindByType(typeof(IntegerLiteral));
CxList methodsList = All.NewCxList();
methodsList.Add(methods.FindByMemberAccess("*.split"));
methodsList.Add(methods.FindByMemberAccess("*.copyOfRange"));

CxList irrelevantLiterals = integerLiterals.GetParameters(methodsList);
strLiterals -= irrelevantLiterals;

CxList ivParameterSpec = All.GetParameters(objCreations.FindByShortName("ivParameterSpec", false));

CxList putsAndSets = All.NewCxList();
CxList collections = Find_Collections();
putsAndSets.Add(collections.FindByMemberAccess("*.put"));
putsAndSets.Add(collections.FindByMemberAccess("*.set"));

CxList firstParametersOfCollections = integerLiterals.GetParameters(putsAndSets, 0);
strLiterals -= firstParametersOfCollections;

CxList sanitizers = Find_Hardcoded_Key_Sanitizers();
sanitizers.Add(ivParameterSpec);

CxList javaxCryptoSinks = All.NewCxList();
List<string> secretKeys = new List<string> {"SecretKeySpec","KeySpec", "DESKeySpec","DESedeKeySpec","EncodedKeySpec",
		"PBEKeySpec","PKCS8EncodedKeySpec","X509EncodedKeySpec"}; 
// javax.crypto.spec.SecretKeySpec
javaxCryptoSinks.Add(All.GetParameters(objCreations.FindByShortNames(secretKeys), 0));
CxList otherParameters = All.GetParameters(objCreations.FindByShortNames(secretKeys));
CxList sanitizedParameters = otherParameters - javaxCryptoSinks;
sanitizers.Add(sanitizedParameters);

// javax.crypto.KeyGenerator
CxList keyGenerators = methods.FindByExactMemberAccess("KeyGenerator.init");
javaxCryptoSinks.Add(keyGenerators);
CxList secureKeyGenerator = All.GetParameters(keyGenerators, 1).GetAncOfType(typeof(MethodInvokeExpr));
javaxCryptoSinks -= secureKeyGenerator;

// javax.crypto.Cipher
javaxCryptoSinks.Add(All.GetParameters(methods.FindByExactMemberAccess("Cipher.init"), 1));
CxList javaSecuritySinks = All.NewCxList();

// java.security.Signature
javaSecuritySinks.Add(methods.FindByExactMemberAccess("Signature.initSign"));
javaSecuritySinks.Add(methods.FindByExactMemberAccess("Signature.initVerify"));

// java.security.KeyFactorySpi
javaSecuritySinks.Add(methods.FindByExactMemberAccess("KeyFactorySpi.engineGeneratePrivate"));
javaSecuritySinks.Add(methods.FindByExactMemberAccess("KeyFactorySpi.engineGeneratePublic"));

// java.security.KeyFactory
javaSecuritySinks.Add(methods.FindByExactMemberAccess("KeyFactory.KeyFactory"));
javaSecuritySinks.Add(methods.FindByExactMemberAccess("KeyFactory.generatePrivate"));
javaSecuritySinks.Add(methods.FindByExactMemberAccess("KeyFactory.generatePublic"));

CxList sinks = All.NewCxList();
sinks.Add(javaxCryptoSinks);
sinks.Add(javaSecuritySinks);

CxList inputs = Find_Inacurate_Hard_coded_Inputs();
sanitizers.Add(strLiterals - inputs);

result = sinks.InfluencedByAndNotSanitized(inputs, sanitizers).ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow);