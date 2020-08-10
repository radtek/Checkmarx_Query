// This query finds printStackTrace methods that are safe outputs from XSS
CxList unknownRefs = Find_UnknownReference();
CxList methods = Find_Methods();
CxList systemUnkRefs = unknownRefs.FindByType("System");
CxList pst = methods.FindByShortName("printStackTrace");
CxList pstWithParams = pst.FindByParameters(Find_Param().GetParameters(pst));

// Cases where printStackTrace is redirected to the response writer are unsafe
// Example: PrintWriter out = response.getWriter(); e.printStackTrace(out);
CxList httpServletResponses = unknownRefs.FindByType("HttpServletResponse");
result = pstWithParams - pst.InfluencedBy(httpServletResponses);

// Cases where the System.setOut is redirected to the response writer are also unsafe
// Example: System.setOut(new PrintStream(resp.getOutputStream())); e.printStackTrace();
CxList systemSetOut = systemUnkRefs.GetMembersOfTarget().FindByShortName("setOut");
CxList systemSetOutRedirectedToResponse = systemSetOut.InfluencedBy(httpServletResponses);
if(systemSetOutRedirectedToResponse.Count == 0)
{
	CxList pstWithoutParams = pst - pstWithParams;
	result.Add(pstWithoutParams);
}