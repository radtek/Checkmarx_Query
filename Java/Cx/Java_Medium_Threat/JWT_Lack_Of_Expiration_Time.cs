CxList methods = Find_Methods();
CxList jwtsBuildRef = methods.FindByMemberAccess("Jwts.builder");
CxList sanitizedMethod = All.FindByMemberAccess("JwtBuilder.*")
	.FindByShortName("setExpiration").GetTargetOfMembers();
sanitizedMethod.Add(methods.FindByShortName("setExpiration"));

CxList lastNodeFlowJwts = methods.FindByShortName("compact");
result = lastNodeFlowJwts.InfluencedByAndNotSanitized(jwtsBuildRef, sanitizedMethod);