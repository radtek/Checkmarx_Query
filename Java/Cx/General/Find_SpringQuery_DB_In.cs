// Spring Query Annotation
CxList customAttributes = Find_CustomAttribute();
CxList queryCustomAttr = customAttributes.FindByName("Query");
CxList annotatedMethods = queryCustomAttr.GetAncOfType(typeof(MethodDecl));
CxList paramDecl = Find_ParamDeclaration();
result.Add(annotatedMethods);
result.Add(paramDecl.GetParameters(annotatedMethods));