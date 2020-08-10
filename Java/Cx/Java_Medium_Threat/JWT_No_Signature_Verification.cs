CxList methods = Find_Methods();
CxList jwtsParserRef = methods.FindByMemberAccess("Jwts.parser").GetTargetOfMembers();
CxList parseMethod = methods.FindByShortName("parse");
result = parseMethod.InfluencedBy(jwtsParserRef);