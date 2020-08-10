//*********************************************************************
// Add vulnerable JsonConvert.DeserializeObject(...) methods
//*********************************************************************
CxList methods = Find_Methods();
CxList memberAccesses = Find_MemberAccesses();
CxList typeRefCollection = All.FindByType(typeof(TypeRefCollection));

// get JsonConvert.DeserializeObject methods
CxList unsafeDeserializeObjects = methods.FindByMemberAccess("JsonConvert.DeserializeObject");

// remove JsonConvert.DeserializeObject(...) where method only has 1 parameter since its safe by default
CxList possibleUnsafeParam = All.GetParameters(unsafeDeserializeObjects, 1);
CxList possibleUnsafeDeserialize = unsafeDeserializeObjects.FindByParameters(possibleUnsafeParam);
unsafeDeserializeObjects -= (unsafeDeserializeObjects - possibleUnsafeDeserialize);

// remove safe settings (TypeNameHandling.None) direct from DeserializeObject(...) parameter
CxList safeTypeNameHandling = memberAccesses.FindByName("*TypeNameHandling.None");
unsafeDeserializeObjects -= safeTypeNameHandling.GetAncOfType(typeof(MethodInvokeExpr)) * unsafeDeserializeObjects;

// get the JsonConvert.DeserializeObject(...) settings (JsonSerializerSettings references)
CxList jsonSerializerSettingsTypes = All.FindByTypes(new String[]{ "JsonSerializerSettings" });
CxList jsonSerializerSettingsRefs = All.FindAllReferences(jsonSerializerSettingsTypes);

// remove safe settings (TypeNameHandling.None)
CxList safeSettingsTypes = safeTypeNameHandling.GetByAncs(jsonSerializerSettingsTypes);
CxList safeSettings = safeSettingsTypes.GetAncOfType(typeof(ObjectCreateExpr)).GetFathers(); // object creations
safeSettings.Add(jsonSerializerSettingsTypes.DataInfluencedBy(safeTypeNameHandling));
CxList safeSettingsRefs = jsonSerializerSettingsRefs.FindAllReferences(safeSettings);
CxList safeDeserializeObjects = safeSettingsRefs.GetAncOfType(typeof(MethodInvokeExpr)) * unsafeDeserializeObjects;
unsafeDeserializeObjects -= safeDeserializeObjects;

// unsafe deserializing types (object, dynamic or no defined type)
CxList unsafeTypeRefs = All.FindByTypes(new String[]{ "object", "dynamic" });
CxList unsafeDeserializeObjectTypes = All.NewCxList();
foreach(CxList deserializeObj in unsafeDeserializeObjects)
{
	MethodInvokeExpr expr = deserializeObj.TryGetCSharpGraph<MethodInvokeExpr>();
	TypeRefCollection typeArgs = expr.TypeArguments;
	CxList typeArgsNodeId = typeRefCollection.FindById(typeArgs.NodeId);
	
	if(typeArgs.Count == 0 || unsafeTypeRefs.GetByAncs(typeArgsNodeId).Count > 0)
	{
		unsafeDeserializeObjectTypes.Add(deserializeObj);
	}
}
result = unsafeDeserializeObjects * unsafeDeserializeObjectTypes;