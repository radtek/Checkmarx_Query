result = All.FindByMemberAccess("String.toByteArray");
CxList methods = Find_Methods();

// Sanitizer for JSF: All convertors (including custom-made) and validators
CxList JSFSanitizers = methods.FindByShortNames(new List<string> {
	"convertDateTimeJSF",
	"convertNumberJSF",
	"converterGenericJSF",
	"validateDoubleRangeJSF",
	"validateLongRangeJSF",
	"validateRegexJSF",
	"validatorGenericJSF"
});

result.Add(Find_General_Sanitize());
result.Add(methods.FindByMemberAccess("DriverManager.getConnection"));
result.Add(methods.FindByShortName("getUserPrincipal"));
result.Add(All.FindByType("Connection"));

result.Add(JSFSanitizers);


// If it is an Android project - Add Android DB
if(Find_Android_Settings().Count > 0)
{
	result.Add(Find_Android_Sanitize());
}