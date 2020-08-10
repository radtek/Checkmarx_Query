/****************************************************************************
* Find_Partial_XSS_Sanitize: only contain partial sanitizers that do not pass 
*						     the global XSS sanitization test (`,',",<,>,\,:)
*****************************************************************************/
CxList strings = Find_Strings();
CxList methods = Find_Methods();
methods.Add(Find_MethodRef(),
	base.Find_MethodRef());
CxList customAttributes = Find_CustomAttribute();

CxList xssPartialSanitizerMethods = methods.FindByMemberAccesses(new string[]{
	// add ResponseUtils methods
	"ResponseUtils.filter",
	
	// add OWASP Java Enconder methods
	"Encode.forJavaScript",
	"Encode.encodeForVBScript",
	"Encode.encodeForJavaScript",
	
	// add HtmlUtils methods
	"HtmlUtils.htmlEscape",
	"HtmlUtils.htmlEscapeHex",
	"HtmlUtils.htmlEscapeDecimal",
	
	// add StringEscapeUtils methods
	"StringEscapeUtils.escapeXml",
	"StringEscapeUtils.escapeHtml",
	"StringEscapeUtils.escapeHtml3",
	"StringEscapeUtils.escapeHtml4"
}, false);

// add all replace methods that contain \r or \n as first parameter
CxList replaceMethods = methods.FindByShortName("replace*");
CxList replaceParam = strings.GetParameters(replaceMethods, 0);
replaceParam = replaceParam.FindByShortName(@"*[^a-zA-Z]*");
replaceMethods = replaceMethods.FindByParameters(replaceParam);

// add xss replace methods
replaceMethods.Add(Find_XSS_Replace(),
	Find_Replace_Param());

// add ibatis execute methods
CxList ibatis = Ibatis();
CxList ibatisSanitizers = ibatis - ibatis.FindByShortName("execute*");

// add general encoders
CxList encoders = Find_Encode();
// remove sanitation if in a JS event, unless encoding for JS
encoders -= encoders.FindByRegex(
	@"<[^>]+(onclick|ondblclick|onmousedown|onmousemove
	|onmouseover|onmouseout|onmouseup|onchange|oncontextmenu
	|oncopy|oncut|onerror|onfocus|onkeydown|onkeypress|onkeyup
	|onload|onpaste|onreset|onresize|onscroll|onsubmit)
	\s*=[^>]*for(?!JavaScript)[^>]*>");

/************ JSF sanitizers *******************/
CxList jsfCode = Find_JSF_Code();
CxList jsfSanitizers = jsfCode.FindByMemberAccess("response.write"); // ny default in JSF it is sanitized

// remove the ones that were set escape="false"
CxList jsfNotEscaped = jsfCode.FindByMemberAccess("cx_escFalseJSF.*");
CxList jsfNotSanitizers = jsfCode.GetByAncs(jsfNotEscaped.GetFathers().GetAncOfType(typeof(MethodInvokeExpr)));
jsfSanitizers -= jsfNotSanitizers;

// add all JSF framework ouptuts (remove the ones that were set escape="false")
CxList jsfFrameworkOutputs = methods.FindByShortName("CxJsfOutput");
CxList jsfVulnerable = methods.FindByShortName("CxJsfEscapeFalse");
jsfVulnerable = jsfFrameworkOutputs.GetByAncs(jsfVulnerable);
jsfFrameworkOutputs -= jsfVulnerable;
/************ END JSF sanitizers ***************/

/****************** RESTful ********************/
// this part looking for classes with Custom Atribute "@Produces(MediaType.APPLICATION_JSON)".
// all Methods of such class is protected from XSS 
CxList allMethodsDecls = Find_MethodDeclaration();
CxList allParamDecl = Find_ParamDeclaration();
CxList allClasses = Find_Class_Decl();

CxList customAttributeProduces = customAttributes.FindByCustomAttribute("Produces");
CxList producesJSON = customAttributeProduces.FindByRegex(@"MediaType\.APPLICATION_JSON");
producesJSON.Add(customAttributeProduces.FindByRegex(@"application/json"));
CxList fatherOfCs = producesJSON.GetFathers();

CxList goodMethods = allMethodsDecls * fatherOfCs;
CxList goodClasses = allClasses * fatherOfCs;
goodMethods.Add(allMethodsDecls.GetByAncs(goodClasses));
CxList methodOfClsWithAPPLICATION_JSON = allParamDecl.GetParameters(goodMethods);
/************** END RESTful ********************/   

result.Add(xssPartialSanitizerMethods,
	replaceMethods,
	ibatisSanitizers,
	encoders,
	jsfSanitizers,
	jsfFrameworkOutputs,
	methodOfClsWithAPPLICATION_JSON,
	
	// add general queries
	Find_Parameters(), // add all DB parametrized query methods
	Find_Whitelisting(),
	Find_JSF_Sanitize(),
	Find_AtgDspSanitize(),
	Find_Aliyun_securityUtil_Escapers());