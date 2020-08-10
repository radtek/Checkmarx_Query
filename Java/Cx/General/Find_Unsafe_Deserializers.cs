CxList inheritsFrom = All.InheritsFrom("ObjectInputStream");
CxList unkRefs = Find_UnknownReference();
CxList members = Find_MemberAccess();
CxList parameters = Find_Param();

CxList methods = Find_Methods();
CxList methodsDecl = Find_MethodDeclaration().FindByShortName("resolveClass");
CxList readObject = methods.FindByMemberAccess("ObjectInputStream.readObject");
readObject.Add(methods.FindByMemberAccess("Unmarshaller.readObject"));
readObject.Add(methods.FindByMemberAccess("XMLDecoder.readObject"));
readObject.Add(methods.FindByMemberAccess("HessianInput.readObject"));
readObject.Add(methods.FindByMemberAccess("Hessian2Input.readObject"));
readObject.Add(methods.FindByMemberAccess("Hessian2Input.readStreamingObject"));

CxList hessianSanitized = methods.FindByMemberAccess("HessianFactory.setWhitelist");
CxList influenced = hessianSanitized.InfluencingOn(readObject);
readObject = readObject - influenced.GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly);

CxList mappers = unkRefs.FindByType("ObjectMapper");
CxList mappersMembers = mappers.GetMembersOfTarget().FindByShortName("enableDefaultTyping");
var vulnerableParameters = new List<string>() {"OBJECT_AND_NON_CONCRETE", "NON_CONCRETE_AND_ARRAYS"};
CxList mappersMembersParameters = members.GetParameters(mappersMembers).FindByShortNames(vulnerableParameters);
CxList vulnerableMembers = mappersMembers.FindByParameters(mappersMembersParameters);

CxList allParams = parameters.GetParameters(mappersMembers);
CxList mappersMembersNoParameters = mappersMembers - mappersMembers.FindByParameters(allParams);
vulnerableMembers.Add(mappersMembersNoParameters);

CxList vulnerableMappers = vulnerableMembers.GetTargetOfMembers().FindByShortName(mappers);
CxList vulnerableMappersRefs = unkRefs.FindAllReferences(vulnerableMappers);
CxList vulnerableMappersReaders = vulnerableMappersRefs.GetMembersOfTarget().FindByShortName("readValue");

readObject.Add(vulnerableMappersReaders);

//XStream library deserialization methods
readObject.Add(Find_XStream_Deserialization_Methods());

foreach (CxList l in inheritsFrom)
{
	CxList res = methodsDecl.GetByAncs(l);
	if (res.Count > 0)
	{
		CxList temp = methods.FindByMemberAccess(l.GetName() + ".read*");
		readObject -= temp;
	}
}

result = readObject;