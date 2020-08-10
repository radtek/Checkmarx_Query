// This query expects to parameters: the first is the equalsMethodName, and the second is the hashCodeMethodName

if (param.Length == 2)
{
	string equalsMethodName = (string) param[0];
	string hashCodeMethodName = (string) param[1];
	
	CxList methodDecl = Find_MethodDecls();
	
	CxList Equals = methodDecl.FindByShortName(equalsMethodName);
	Equals = Equals.FindByFieldAttributes(Modifiers.Override);
	
	CxList getHash = methodDecl.FindByShortName(hashCodeMethodName);
	getHash = getHash.FindByFieldAttributes(Modifiers.Override);
	
	CxList equalsClass = All.GetClass(Equals);
	CxList getHashClass = All.GetClass(getHash);
	
	result.Add(equalsClass);
	result.Add(getHashClass);
	result -= equalsClass * getHashClass;
}
else
	cxLog.WriteDebugMessage("common.Just_One_of_Equals_and_Hash_code_Defined must be called with 2 arguments. It was actually called with " + param.Length);