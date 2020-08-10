// Spring Query Annotation
CxList customAttributes = Find_CustomAttribute();
CxList queryCustomAttr = customAttributes.FindByName("Query");
CxList annotatedMethods = queryCustomAttr.GetAncOfType(typeof(MethodDecl));

CxList unkRefs = Find_UnknownReference();
CxList methods = Find_Methods();
CxList refs = All.NewCxList();
refs.Add(unkRefs);
refs.Add(methods);

CxList queryMethods = refs.FindAllReferences(annotatedMethods);

CxList assignees = queryMethods.GetAssignee();
CxList methodsWithoutAssignee = queryMethods - assignees.GetAssigner();

result.Add(assignees);
result.Add(methodsWithoutAssignee);