CxList objCreate = Find_ObjectCreations();
CxList methods = Find_Methods();
CxList unkRefs = Find_UnknownReference();

// Common java utilities fullsanitizers

// Runtime.exec
result.Add(methods.FindByMemberAccess("Runtime.exec", true));

// new URI/URI.create
result.Add(objCreate.FindByShortName("URI"));
result.Add(methods.FindByMemberAccess("URI.create", true));
result.Add(methods.FindByMemberAccess("TimeZone.getTimeZone", true));
// Servlet request items
string[] servletMethods = new string[] {
		"HttpServletRequest.isRequestedSessionIdFromCookie",
		"HttpServletRequest.isRequestedSessionIdFromURL",
		"HttpServletRequest.isRequestedSessionIdFromUrl",
		"HttpServletRequest.isRequestedSessionIdValid",
		"HttpServletRequest.isUserInRole",
		"HttpServletRequest.authenticate"
};
result.Add(methods.FindByMemberAccesses(servletMethods));

// Servlet response
string[] servletRespMethods = new string[] {
	"HttpServletResponse.setCharacterEncoding",
	"HttpServletResponse.setBufferSize",
	"HttpServletResponse.setContentType",
	"HttpServletResponse.setLocale",
	"HttpServletResponse.setHeader"
};
result.Add(methods.FindByMemberAccesses(servletRespMethods));

// XPath api's
result.Add(methods.FindByMemberAccess("XPath.evaluate"));
result.Add(methods.FindByMemberAccess("XPath.compile"));

// LDAP
result.Add(methods.FindByMemberAccess("DirContext.search"));

// Struts ResponseUtils
result.Add(methods.FindByMemberAccess("ResponseUtils.encodeURL"));

// SafeHtmlBuilder
result.Add(methods.FindByMemberAccess("SafeHtmlBuilder.appendEscaped"));


// Add OWASP java-html-sanitizer sanitize method
CxList owaspBuilder = objCreate.FindByShortName("HtmlPolicyBuilder").GetRightmostMember().GetAssignee();
CxList allRefs = unkRefs.FindAllReferences(owaspBuilder);
CxList sanitizeMethods = allRefs.GetMembersOfTarget().FindByShortName("sanitize");
result.Add(sanitizeMethods);
result.Add(All.GetParameters(sanitizeMethods));