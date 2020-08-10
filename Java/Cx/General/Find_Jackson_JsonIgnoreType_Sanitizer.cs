CxList customAttrs = Find_CustomAttribute();
CxList unks = Find_UnknownReference();
CxList classes = Find_Class_Decl();
CxList fields = Find_Field_Decl().GetByAncs(classes);

//@JsonIgnoreType
CxList jsonIgnoreType = customAttrs.FindByShortName("JsonIgnoreType");
jsonIgnoreType = jsonIgnoreType.GetAncOfType(typeof(ClassDecl));

//Find addMixIns
CxList addMixIn = unks.FindAllReferences(jsonIgnoreType).GetAncOfType(typeof(MethodInvokeExpr));

//Find types to ignore
CxList sanitizeTypes = All.GetParameters(addMixIn, 0);
sanitizeTypes = Find_TypeRef().GetByAncs(sanitizeTypes.GetTargetOfMembers());

//Find declarators from the sanitized types
CxList decls = Find_Declarators().GetByAncs(fields).FindByType(sanitizeTypes);

//Get classes influencedBy addMixIn
CxList mapper = addMixIn.GetTargetOfMembers();
mapper = unks.FindAllReferences(mapper);
mapper = mapper.GetMembersOfTarget();

result = mapper.DataInfluencedBy(decls);