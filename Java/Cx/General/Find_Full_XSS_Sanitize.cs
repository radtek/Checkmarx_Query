/****************************************************************************
* Find_Full_XSS_Sanitize: only contain strong sanitizers that remove all 
*						  meta-characters that can break out of some context:
*						  - must encode, remove or reject `,', ", <, >, \,:
*						  - encode/encrypt/hash/map the entire string (General Sanitize)
*****************************************************************************/
CxList methods = Find_Methods();
CxList jspTags = Find_Output_Tags();
CxList objectCreate = Find_Object_Create();

CxList xssSanitizerMethods = methods.FindByMemberAccesses(new string[]{
	// add addEntities methods
	"Utils.addEntities",
	
	// add DirContext methods
	"DirContext.search",
	
	// add ResponseUtils methods
	"ResponseUtils.encodeURL",
	
	// add Runtime methods
	"Runtime.exec",
	"getRuntime.exec",
	
	// add XPATH methods
	"XPATH.compile",
	"XPATH.evaluate",
	
	// add GWT methods
	"SafeHtml.asString", 
	"sanitizeHtml.asString",
	
	// add SafeHtmlBuilder methods
	"SafeHtmlBuilder.appendEscaped",
	"SafeHtmlBuilder.appendEscapedLines",
	
	// add time manipulation methods
	"TimeZone.getTimeZone",
	"SimpleDateFormat.parse",
	"GregorianCalendar.setTime",
	
	// add OWASP Java Enconder methods
	"Encode.encodeForURL",
	"Encode.encodeForXML",
	"Encode.encodeForHTML",
	"Encode.encodeForXPath",
	
	// add HttpServletResponse methods
	"HttpServletResponse.setHeader",
	"HttpServletResponse.setLocale",
	"HttpServletResponse.setBufferSize",
	"HttpServletResponse.setContentType",
	"HttpServletResponse.setCharacterEncoding",
	
	// add HttpServletRequest methods that are not outputs for XSS
	"HttpServletRequest.isUserInRole",
	"HttpServletRequest.authenticate",
	"HttpServletRequest.getIntHeader",
	"HttpServletRequest.getDateHeader",
	"HttpServletRequest.isRequestedSessionIdValid",
	"HttpServletRequest.isRequestedSessionIdFromUrl",
	"HttpServletRequest.isRequestedSessionIdFromURL",
	"HttpServletRequest.isRequestedSessionIdFromCookie",	
});

// add all the methods that could encode/encrypt/hash/map the entire string (General Sanitize) 
CxList generalSanitize = Find_General_Sanitize();

// add new URI(...) and URI.create(...)
CxList uriObjCreate = objectCreate.FindByShortName("URI", true);
uriObjCreate.Add(methods.FindByMemberAccess("URI.create", true));

// add spring getMessage(...) methods
CxList springGetMessage = methods.FindByMemberAccesses(new string[] {
	"*MessageSource.getMessage",
	"*ApplicationContext.getMessage"
});
CxList springGetMessageParams = All.GetParameters(springGetMessage, 0);
springGetMessageParams -= Find_Param();

// add GWT SimpleHtmlSanitizer.getInstance().sanitize(...).asString() methods
CxList simpleHtmlSanitizerMethods = methods.FindByMemberAccess("SimpleHtmlSanitizer.getInstance");
CxList gwtSanitizeMethods = methods.FindByMemberAccess("sanitize.asString");
CxList gwtSanitizer = simpleHtmlSanitizerMethods.GetRightmostMember() * gwtSanitizeMethods;

/************ JSP sanitizers *******************/
CxList jspCode = Find_Jsp_Code();
CxList jspSanitizers = jspCode.GetByAncs(jspTags);
CxList mvcSanitizers = jspCode.FindByMemberAccesses(new string[]{
	"c_out.value*",
	"c_param.value"
});

CxList mvcSanitizersGetMembersOfTarget = mvcSanitizers.GetMembersOfTarget();

mvcSanitizers = mvcSanitizers.FindByMemberAccess("response.write");
mvcSanitizers.Add(mvcSanitizersGetMembersOfTarget.FindByMemberAccess("response.write"),
	mvcSanitizersGetMembersOfTarget.GetMembersOfTarget().FindByMemberAccess("response.write"));

CxList beanWriteSanitizers = jspCode.FindByMemberAccess("bean_write.response")
	.GetMembersOfTarget().FindByMemberAccess("response.write");
CxList auxSanitizers = All.NewCxList();
auxSanitizers.Add(mvcSanitizers, beanWriteSanitizers);
jspSanitizers.Add(jspCode.GetByAncs(auxSanitizers));

CxList jspEscaped = jspCode.FindByMemberAccess("cx_escFalse.*");
jspSanitizers -= jspCode.GetByAncs(jspEscaped.GetFathers().GetAncOfType(typeof(MethodInvokeExpr)) * beanWriteSanitizers);
jspSanitizers -= jspCode.GetByAncs(jspEscaped.GetAncOfType(typeof(MethodInvokeExpr)));

CxList methodsInJspSanitizers = All.NewCxList();
methodsInJspSanitizers.Add(jspSanitizers.FindByType(typeof(MethodRef)),
	jspSanitizers.FindByType(typeof(MethodInvokeExpr)));
CxList gettersInJspSanitizers = methodsInJspSanitizers.FindByShortName("get*");
jspSanitizers -= gettersInJspSanitizers.GetByAncs(jspSanitizers.FindByAssignmentSide(CxList.AssignmentSide.Right));
/************ END JSP sanitizers ***************/

result.Add(xssSanitizerMethods,
	generalSanitize,
	uriObjCreate,
	springGetMessageParams,
	gwtSanitizer,
	jspSanitizers,
	jspCode.GetParameters(jspSanitizers));