// Improper Clearing of Heap Memory Before Release ('Heap Inspection')
// this query is looking for storage of password(s) that are stored unencrypted in the memory,
// and so visible to an attacker invoking a memory dump.
// In general, one should not store passwords in an unencrypted form.
// This query finds variables that store passwords in an unencrypted form.
CxList passwords = Find_All_Passwords();
passwords -= passwords.FindByFileName("*.aspx");
passwords -= passwords.FindByFileName("*.aspx.designer.cs");
CxList objectTest = Find_ObjectCreations();
CxList encryptTest = Find_Encrypt();
CxList declaratorsList = Find_Declarators();
CxList methodsList = Find_Methods();
CxList unknownList = Find_Unknown_References();
CxList intList = Find_IntegerLiterals();
CxList fieldList = base.Find_FieldDecls();
CxList propertyList = base.Find_PropertyDecl();
CxList arrayCreateList = Find_ArrayCreateExpr();

// 1)exclude variables that are all uppercase - usually describes the pattern of the data, such as PASSWORDPATTERN, PASSORDTYPE...
CxList upperCase = All.NewCxList();
foreach (CxList res in passwords)
{
	string name = res.GetName();
	if (name.ToUpper().Equals(name))
	{
		upperCase.Add(res);
	}
}
passwords -= upperCase;

//2) define sanitizers = encryption
CxList sanitizeMethods = objectTest.FindByShortName("SecureString");
sanitizeMethods.Add(All.FindByMemberAccess("CryptoStream.Write*"));
CxList sanitize = All.FindByFathers(sanitizeMethods);
sanitize.Add(encryptTest);

//3) define safe types
string[] safeTypes = {"*.securestring", "securestring", "*.cryptostream", "cryptoatream", "dbset"};
passwords -= passwords.FindByTypes(safeTypes);
passwords -= passwords.FindByType(typeof(ConstantDecl));
passwords -= passwords.FindByShortName("*Encrypt*");

//4) find assignments of literals
CxList literals = Find_Strings();
literals.Add(intList);
CxList assignLiteral = literals.GetFathers();

//find the declaration statements of passwords
CxList declarationOfPasswords = passwords.FindByType(typeof(Declarator)); 
declarationOfPasswords.Add(passwords.FindByType(typeof(FieldDecl))); // property of classes are of FieldDecl type
declarationOfPasswords.Add(passwords.FindByType(typeof(PropertyDecl))); // or PropertyDecl

// find password that is assigned a literal
CxList passAssigned = passwords.FindByAssignmentSide(CxList.AssignmentSide.Left);
CxList passAssignLiteral = passAssigned.FindByFathers(assignLiteral);
passAssignLiteral.Add(assignLiteral);
passAssigned -= passAssignLiteral; //remove password that is assigned a literal

//remove password that is assigned a safe object or an encrypted object
passAssigned -= passAssigned.InfluencedBy(sanitize);  
	
result = All.FindDefinition(passAssigned);
result.Add(declarationOfPasswords);

//net core
CxList sanitizerProtector = encryptTest.FindByType(typeof(MethodInvokeExpr)).FindByShortName("Protect");
CxList sanitizeObj = sanitizerProtector.GetAssignee();
sanitizeObj.Add(All.FindAllReferences(sanitizeObj));
result -= sanitizeObj;

//take the field related with properties 
CxList declaratorsOnField = declaratorsList.GetByAncs(fieldList);
CxList declaratorsFieldOnProperty = unknownList.FindAllReferences(declaratorsOnField).GetByAncs(propertyList);
result -= declaratorsList.FindDefinition(declaratorsFieldOnProperty);

// zeroization
//first case - System.Array.Clear
CxList arrayClear = methodsList.FindByName("*Array.Clear");
CxList firstParamClear = unknownList.GetParameters(arrayClear, 0);
result -= declaratorsList.FindDefinition(firstParamClear);

//second case - array create expr
CxList assignVar = arrayCreateList.GetAssignee();
result -= declaratorsList.FindDefinition(assignVar);