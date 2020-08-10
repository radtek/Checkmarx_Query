CxList customAttrs = Find_CustomAttribute();

//@JsonIgnore
CxList jsonIgnoreField = customAttrs.FindByShortName("JsonIgnore");

result = jsonIgnoreField.GetAncOfType(typeof(FieldDecl));