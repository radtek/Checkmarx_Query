CxList methods = Find_Methods();
CxList jwtsBuildRef = methods.FindByMemberAccess("Jwts.builder");
CxList sanitizedMethod = All.FindByMemberAccess("JwtBuilder.*")
	.FindByShortName("signWith").GetTargetOfMembers();
sanitizedMethod.Add(methods.FindByShortName("signWith"));

CxList lastNodeFlowJwts = methods.FindByShortName("compact");
result = lastNodeFlowJwts.InfluencedByAndNotSanitized(jwtsBuildRef, sanitizedMethod);