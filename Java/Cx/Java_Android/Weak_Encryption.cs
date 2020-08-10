//////////////////////////////////////////////////////////////
// Query Weak_Encryption
// DRD17-J Do not use the Android cryptographic security 
// provider encryption default for AES
// The query looks for use of DES or AES with ECB block encryption
//////////////////////////////////////////////////////////////
CxList methods = Find_Methods();
CxList strings = Find_Strings();
CxList declarators = Find_Declarators();
CxList unknowRef = Find_UnknownReference();

CxList parameters = All.NewCxList();
parameters.Add(strings, unknowRef);
CxList cipherGetInstance = methods.FindByMemberAccess("Cipher.getInstance");
CxList encryptionAlgorithm = parameters.GetParameters(cipherGetInstance, 0);

CxList assigner = All.NewCxList();
assigner.Add(declarators, unknowRef);
encryptionAlgorithm.Add(assigner.FindAllReferences(encryptionAlgorithm).GetAssigner());
CxList encryptionStrings = encryptionAlgorithm.FindByType(typeof(StringLiteral));

result = encryptionStrings.FindByShortNames(new List<string>{ 
	"AES", 
	"AES/ECB*", 
	"DES*"
});