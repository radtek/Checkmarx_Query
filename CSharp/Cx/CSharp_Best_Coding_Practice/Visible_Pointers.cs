CxList allFields = All.FindByType(typeof(FieldDecl));

CxList allPublicFields = allFields.FindByFieldAttributes(Modifiers.Public);
CxList allProtectedFields = allFields.FindByFieldAttributes(Modifiers.Protected);
CxList allInternalFields = allFields.FindByFieldAttributes(Modifiers.Internal);
CxList allReadonlyFields = allFields.FindByFieldAttributes(Modifiers.Readonly);
CxList allExposedFields = ((allPublicFields + allProtectedFields) - 
						   allInternalFields) - allReadonlyFields;

CxList allIntPtr = All.FindByType("IntPtr").GetAncOfType(typeof(FieldDecl));
CxList allUIntPtr = All.FindByType("UIntPtr").GetAncOfType(typeof(FieldDecl));

result = (allIntPtr + allUIntPtr) * allExposedFields;