/* 
	It dynamically creates flows from the annotation (Value - base64Secret) to the corresponding param in signWith method (key).

	Example:

	class TokenProvider {

 		private final String base64Secret;
		private Key key;

		public TokenProvider(
      		// if jwt.base64-secret is not set the default value will be used
      		// which is hardcoded with @Value annotation
      		@Value("${jwt.base64-secret:NWViZTIyOTRlY2QwZTBmMDhlYWI3NjkwZDJhNmVlNjk=}") String base64Secret,
			...
			) {
				this.base64Secret = base64Secret;
			}

		// Method not called
 		public void afterPropertiesSet() {
 			byte[] keyBytes = Decoders.BASE64.decode(base64Secret);
      		this.key = Keys.hmacShaKeyFor(keyBytes);
			...

		public String createToken(Authentication authentication, boolean rememberMe) {
	      return Jwts.builder()
	         .setSubject(authentication.getName())
	         .claim(AUTHORITIES_KEY, authorities)
	         .signWith(key)
	         .setExpiration(validity)
	         .compact();
   	}
*/

CxList strings = Find_Strings();
CxList methods = Find_Methods();

CxList jwtsParserRef = methods.FindByMemberAccess("Jwts.builder").GetTargetOfMembers();
CxList signWithMethod = methods.FindByShortName("signWith");
CxList paramMethod = All.GetParameters(signWithMethod);

CxList typeKey = All.FindAllReferences(All.FindDefinition(paramMethod));

CxList parameterInMethod = typeKey.FindByType(typeof(UnknownReference))
	.GetByAncs(signWithMethod) * paramMethod;

CxList unkRefAndMemberAccess = typeKey.FindByType(typeof(MemberAccess));
unkRefAndMemberAccess.Add(typeKey.FindByType(typeof(UnknownReference)));


CxList unkAndAccessMembrer = Find_MemberAccess();
unkAndAccessMembrer.Add(Find_UnknownReference());

CxList flows = typeKey.DataInfluencedBy(strings);

CxList springValue = All.FindByCustomAttribute("Value");
CxList stringValues = strings.GetByAncs(springValue);
foreach(CxList strValue in stringValues) 
{	
	var name = strValue.GetName();
	if (name.StartsWith("${") && name.Contains(":")) {
		var item1 = strValue.GetFathers().GetFathers();		
		var item2 = unkAndAccessMembrer.DataInfluencedBy(item1);						
		flows.Add(item2);
	}
}
//Reduce the flow
flows = flows.ReduceFlow(CxList.ReduceFlowType.ReduceSmallFlow);

Func<CxList,CxList,CxList,CxList> CreateFlow = (CxList flow, CxList parameterInMethod, CxList lastElem) => 
	{ 
	CxList dataStoreAux = All.NewCxList();
	
	CxList definition = All.FindDefinition(lastElem);
	CxList newFlow = flow.ConcatenatePath(definition, false);
			
	CxList lastElemNewFlow = newFlow.GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly);
			
	CxList definitionUnitParam = parameterInMethod.DataInfluencedBy(lastElemNewFlow)
		.ReduceFlow(Checkmarx.DataCollections.CxQueryProvidersInterface.CxList.ReduceFlowType.ReduceBigFlow);
			
	CxList fullFlow = newFlow.ConcatenatePath(definitionUnitParam, false)
		.ReduceFlow(Checkmarx.DataCollections.CxQueryProvidersInterface.CxList.ReduceFlowType.ReduceSmallFlow);				
	
	dataStoreAux.Add(fullFlow);
	
	return dataStoreAux;	
	};


CxList dataStore = All.NewCxList();
foreach (CxList flow in flows.GetCxListByPath())
{
	CxList lastElem = flow.GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly);	
	CxList isEqual = parameterInMethod * lastElem.FindByType(typeof(UnknownReference));
	if(isEqual.Count() == 1)
	{	
		dataStore.Add(flow);		
	}
	else
	{
		CxList isUnkRefAndMemberAccess = (lastElem * unkRefAndMemberAccess);
		isUnkRefAndMemberAccess.Add((lastElem * unkRefAndMemberAccess));				
		if(isUnkRefAndMemberAccess.Count() == 1)
		{
			CxList tResult = CreateFlow(flow, parameterInMethod, lastElem);
			dataStore.Add(tResult);
			
		}else
		{				
			CxList isAccessMember = (All.FindDefinition(lastElem).FindByType("string"));
			if(isAccessMember.Count() == 1)
			{			
				CxList tResult = CreateFlow(flow, parameterInMethod, lastElem);				
				CxList lastElemResult = tResult.GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly);
				
				//Flow  definition (base64Secret) to this.key
				CxList flowUnitkey = typeKey.DataInfluencedBy(lastElemResult);		
				
				// Value String base64Secret to  this.key
				CxList customAttributeValueToKey = tResult.ConcatenatePath(flowUnitkey, false);					
				
				// Flow  definition (base64Secret) to key param
				CxList lastElemFlowUnitkey = customAttributeValueToKey.GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly);
				CxList tResultFlowFinal = CreateFlow(customAttributeValueToKey, parameterInMethod, lastElemFlowUnitkey);				
				dataStore.Add(tResultFlowFinal);			
			}
		}	
	}	
}

result = dataStore;