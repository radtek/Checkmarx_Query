CxList strings = Find_Strings();
CxList methods = Find_Methods();

CxList jwtsRef = methods.FindByMemberAccess("Jwts.builder").GetTargetOfMembers();
CxList signWithMethod = methods.FindByShortName("signWith");
CxList refSignWithMethod = signWithMethod.InfluencedBy(jwtsRef);

CxList signWithMet = refSignWithMethod.FindByShortName("signWith");
CxList parameterInMethod = All.GetParameters(signWithMet).FindByType("Key");

CxList flows = parameterInMethod.DataInfluencedBy(strings)
	.ReduceFlow(CxList.ReduceFlowType.ReduceSmallFlow);

CxList attributesValue = Create_Flow_Spring_CustomAttribute();
flows.Add(attributesValue);

result = flows;