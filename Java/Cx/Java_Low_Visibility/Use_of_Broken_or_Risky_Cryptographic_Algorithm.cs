CxList strings = Find_Strings();
CxList methods = Find_Methods();
CxList unknownRefs = Find_UnknownReference();

CxList des = strings.FindByShortName("*DES*")
	- strings.FindByShortName("*DESEDE*", false)
	- strings.FindByShortName("*TripleDES*");

CxList weakTypes = All.NewCxList();
weakTypes.Add(
	// CFMX
	strings.FindByName("*CFMX_COMPAT*"),
	// RCX
	strings.FindByShortNames(new List<string>(){"*RC2*", "*RC4*", "*RC5*", "*ARCFOUR*", "*Blowfish*"}, false));

// get all the weak types as unknown references
CxList weakTypeRefs = unknownRefs.FindAllReferences(weakTypes.GetAssignee());

CxList weakKeys = All.NewCxList();
weakKeys.Add(weakTypes, weakTypeRefs, des);

CxList keyPairGeneratorInitialize = methods.FindByMemberAccess("KeyPairGenerator.initialize");

CxList getInstance = methods.FindByMemberAccesses(new string[]{
	"Cipher.getInstance",
	"KeyGenerator.getInstance",
	"SecretKeyFactory.getInstance"});

IntegerIntervalAbstractValue abstractValLessThan512 = new AbstractValueTypes.IntegerIntervalAbstractValue(null, 512);

CxList absValue = All.GetParameters(keyPairGeneratorInitialize)
	.FindByAbstractValue(absVal => absVal.IncludedIn(abstractValLessThan512));
weakKeys.Add(absValue);

result.Add(getInstance.FindByParameters(weakKeys),
	keyPairGeneratorInitialize.FindByParameters(weakKeys));

//support MD5 MD2 MD4 SHA1 
CxList md5 = strings.FindByShortNames(new List<string> {
	"\"MD5\"",
	"\"MD2\"",
	"\"SHA-1\"",
	"\"MD4\""});

CxList messageDigestRet = methods.FindByMemberAccess("MessageDigest.getInstance");
result.Add(messageDigestRet.FindByParameters(md5));

//digesUtils
CxList digestUtilElements = methods.FindByMemberAccess("DigestUtils.*");
CxList digestElements = digestUtilElements.FindByShortNames(new List<string> {
	"md5*",
	"md2*",
	"sha1*"});
result.Add(digestElements);

//HMAC
CxList hmacs = methods.FindByMemberAccess("MAC.getInstance");
CxList hmacAlgorithms = strings.FindByShortNames(new List<string> {
	"\"HmacMD5\"",
	"\"HmacMD2\"",
	"\"HmacSHA-1\""});
result.Add(hmacs.FindByParameters(hmacAlgorithms));