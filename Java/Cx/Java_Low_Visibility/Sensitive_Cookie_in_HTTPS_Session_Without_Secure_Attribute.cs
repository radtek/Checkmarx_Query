// Find the setSecured(true)
CxList setSecure = All.FindByMemberAccess("Cookie.setSecure");
CxList securedParams = All.FindByShortName("true");
CxList secured = setSecure.FindByParameters(securedParams);

CxList webFiles = Find_Web_Files();
// Verify if in the webFiles (*.xml) the fields in the session-config more precisely  
//the http-only and the secure fields are setted true 
if ( webFiles.FindByName("WEB_APP.SESSION_CONFIG.COOKIE_CONFIG.HTTP_ONLY.TEXT").GetAssigner().FindByShortName("true").Count == 0 || 
	webFiles.FindByName("WEB_APP.SESSION_CONFIG.COOKIE_CONFIG.SECURE.TEXT").GetAssigner().FindByShortName("true").Count == 0)
{
	// Find the added cookies 
	CxList addCookie = All.FindByMemberAccess("response.addCookie");
	addCookie.Add(All.FindByName("*response.addCookie"));
	addCookie.Add(All.FindByName("*Response.addCookie"));

	CxList cookies = All.GetParameters(addCookie).FindByTypes(new String[]{"*.Cookie","Cookie"});
	
	// Return the added cookies that are not secured
	result = cookies - cookies.DataInfluencedBy(secured);
}