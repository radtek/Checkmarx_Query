CxList methods = Find_Methods();
CxList unkRefs = Find_UnknownReference();
CxList paramDecls = Find_ParamDecl();
CxList objCreate = Find_ObjectCreations();

// We will sanitize tokens with expiration values smaller than 24 hours (in ms)
int maxValue = 86400 * 1000;
IAbstractValue acceptableRange = new IntegerIntervalAbstractValue(maxValue + 1, null);

CxList setExp = methods.FindByShortName("setExpiration");
CxList expirationParams = All.GetParameters(setExp, 0);

CxList builders = methods.FindByMemberAccess("Jwts.builder");
// JwtBuilder as parameter/definition
builders.Add(unkRefs.FindByType("JwtBuilder"));
// Add assignments references
builders.Add(unkRefs.FindAllReferences(builders.GetAssignee()));

CxList generators = methods.FindByShortName("compact");
generators = generators * builders.GetRightmostMember();
expirationParams = expirationParams.FindByAbstractValue(absValue => acceptableRange.Contains(absValue));

// Try to heuristically find Date usages, of the pattern new Date(current + expiration)
CxList possibleParams = Find_BinaryExpr();
possibleParams.Add(unkRefs);
possibleParams.Add(Find_IntegerLiterals());

CxList dateParams = All.GetParameters(objCreate.FindByType("Date"));
CxList excessiveValues = possibleParams.GetByAncs(dateParams)
	.FindByAbstractValue(absValue => acceptableRange.Contains(absValue)).GetFathers() * dateParams;

result = generators.DataInfluencedBy(expirationParams + excessiveValues);