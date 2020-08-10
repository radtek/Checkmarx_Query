CxList inputs = Find_Inputs();
CxList methods = Find_Methods();
CxList methodDecls = Find_MethodDecls();
CxList objectCreations = Find_ObjectCreations();
CxList unknownReferences = Find_UnknownReference();

// deserialize methods
CxList deserialize = methods.FindByMemberAccess("BinaryFormatter.Deserialize");
deserialize.Add(methods.FindByMemberAccess("BinaryFormatter.DeserializeMethodResponse"));
deserialize.Add(methods.FindByMemberAccess("BinaryFormatter.UnsafeDeserialize"));
deserialize.Add(methods.FindByMemberAccess("BinaryFormatter.UnsafeDeserializeMethodResponse"));
deserialize.Add(methods.FindByMemberAccess("SoapFormatter.Deserialize"));
deserialize.Add(methods.FindByMemberAccess("IFormatter.Deserialize"));
deserialize.Add(methods.FindByMemberAccess("XmlObjectSerializer.ReadObject"));
deserialize.Add(methods.FindByMemberAccess("Marshal.Read*"));
deserialize.Add(methods.FindByMemberAccess("Marshal.PtrToStructure"));
deserialize.Add(methods.FindByMemberAccess("JSON.ToObject"));
deserialize.Add(methods.FindByMemberAccess("LosFormatter.Deserialize"));
deserialize.Add(methods.FindByMemberAccess("NetDataContractSerializer.Deserialize"));
deserialize.Add(methods.FindByMemberAccess("ObjectStateFormatter.Deserialize"));

//*********************************************************************
// Add vulnerable JsonConvert.DeserializeObject(...) methods
//*********************************************************************
deserialize.Add(Find_Unsafe_DeserializeObject());

//*********************************************************************
// Add Deserialize(...) methods that inherits from IFormatter interface
//*********************************************************************
CxList iformatter = All.InheritsFrom("IFormatter");
CxList iformatterMethods = methodDecls.GetByAncs(iformatter);
CxList iformatterDeserialize = iformatterMethods.FindByShortName("Deserialize");
deserialize.Add(methods.FindAllReferences(iformatterDeserialize));

//*********************************************************************
// Add GetObjectData(...) methods that inherits from ISerializable
//*********************************************************************
CxList iserializable = All.InheritsFrom("ISerializable");
CxList iserializableMethods = methodDecls.GetByAncs(iserializable);
CxList iiserializableGetObject = iserializableMethods.FindByShortName("GetObjectData");
deserialize.Add(methods.FindAllReferences(iiserializableGetObject));

//*********************************************************************
// Add Deserialize(...) methods from JavaScriptSerializer (the param from instantiation)
//*********************************************************************
CxList javaScriptSerializerCreations = All.FindByShortName("JavaScriptSerializer").FindByType(typeof(ObjectCreateExpr));
CxList javaScriptSerializer = All.NewCxList();
foreach (CxList javaScriptSerializerCreation in javaScriptSerializerCreations)
{
	CxList parameters = All.GetParameters(javaScriptSerializerCreation);
	if (parameters.Count > 0)
	{
		javaScriptSerializer.Add(javaScriptSerializerCreation);
	}
}

CxList javaScriptSerializerDeserialize = methods.FindByMemberAccess("JavaScriptSerializer.Deserialize");
CxList javaScriptSerializerDeserializeFlow = javaScriptSerializerDeserialize.InfluencedBy(javaScriptSerializer.GetFathers());
deserialize.Add(javaScriptSerializerDeserializeFlow.GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly));

//*********************************************************************
// Add ReadObject(...) methods where attackers control the type to be deserialized 
// in DataContractSerializer and DataContractJsonSerializer (the param from instantiation)
//*********************************************************************
List<string> dataContractTypes = new List<string> { "DataContractSerializer" , "DataContractJsonSerializer" };
CxList dataContract = objectCreations.FindByShortNames(dataContractTypes);
CxList dataContractParams = All.GetParameters(dataContract);

// deserialize types that are influenced by user input
CxList vulnerableFlow = dataContractParams.DataInfluencedBy(inputs);
CxList vulnerableDataContract = vulnerableFlow.GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly);
CxList unsafeDataContract = vulnerableDataContract.GetAncOfType(typeof(ObjectCreateExpr));

// add vulnerable ReadObject(...) methods
CxList unsafeDeclarator = unsafeDataContract.GetAssignee();
CxList unsafeDeclaratorRefs = unknownReferences.FindAllReferences(unsafeDeclarator);
CxList unsafeDeserialize = unsafeDeclaratorRefs.GetMembersOfTarget().FindByShortName("ReadObject");
deserialize.Add(unsafeDeserialize);

//*********************************************************************
// Add Deserialize(...) methods where attackers control the type to be deserialized 
// in XmlSerializer (the param from instantiation)
//*********************************************************************
CxList xmlSerializer = objectCreations.FindByShortName("XmlSerializer");
xmlSerializer.Add(methods.FindByMemberAccess("XMLSerializerFactory.CreateSerializer"));
CxList xmlSerializerParams = All.GetParameters(xmlSerializer);

// deserialize types (from XmlSerializer objects) that are influenced by user input
vulnerableFlow = xmlSerializerParams.DataInfluencedBy(inputs);
CxList vulnerableXmlSerializer = vulnerableFlow.GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly);
CxList unsafeXmlSerializer = All.FindByParameters(vulnerableXmlSerializer);

// add vulnerable Deserialize(...) methods
CxList vulnerableDeclaration = unsafeXmlSerializer.GetAssignee();
CxList vulnerableDeclarationRefs = unknownReferences.FindAllReferences(vulnerableDeclaration);
unsafeDeserialize = vulnerableDeclarationRefs.GetMembersOfTarget().FindByShortName("Deserialize");
deserialize.Add(unsafeDeserialize);

result = deserialize;