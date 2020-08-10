//org.owasp.esapi.Encoder
	List<string > owaspSanitizers = new List<string> {
		"forUriComponent",
		"encodeForBase64",
		"encodeForCSS",
		"encodeForHTML",
		"encodeForHTMLAttribute",
		"encodeForJavaScript",
		"encodeForURL",
		"encodeForXML",
		"encodeForXMLAttribute",
		"encodeForDN",
		"encodeForLDAP"};
CxList owaspEncoder = Get_ESAPI().FindByMemberAccess("Encoder.encode*").FindByShortNames(owaspSanitizers);
result.Add(owaspEncoder);

List<string> reformSanitizers = new List<string> {
		"HtmlAttributeEncode",
		"HtmlEncode",
		"JsString",
		"VbsString",
		"XmlAttributeEncode",
		"XmlEncode"};

CxList reformEncoder = All.FindByType("Reform").GetMembersOfTarget().FindByShortNames(reformSanitizers);
result.Add(reformEncoder);

//Exclude 'false' encoders 
CxList base64Encoders = Find_Base64Encoders();
result.Add(base64Encoders - base64Encoders.DataInfluencingOn(Find_Base64Decoders()));

result.Add(Find_HexEncoders());