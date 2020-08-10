CxList methods = Find_DB_In_Django();
methods.Add(Find_DB_Out_Django());

CxList allParams = Find_Param();

CxList relevantMethods = methods.FindByName("*.execute");
relevantMethods.Add(methods.FindByName("*.raw"));

CxList unknownRefs = Find_UnknownReference();
CxList decls = Find_Declarators();
CxList paramDecls = Find_ParamDecl();
CxList methodsIvk = Find_Methods(); 
CxList methosDecls = Find_MethodDecls();
CxList memberAccesses = Find_MemberAccesses();

CxList relevantSourceTypes = All.NewCxList();
relevantSourceTypes.Add(unknownRefs, decls, paramDecls, methodsIvk, methosDecls, memberAccesses);

CxList managerRaw = Find_Django_Model_Components(); 
CxList managerObject = All.FindAllReferences(relevantSourceTypes.InfluencedBy(managerRaw)
	.FindByAssignmentSide(CxList.AssignmentSide.Left)); 

managerObject = managerObject.GetByAncs(managerRaw.GetAncOfType(typeof(AssignExpr)));
managerObject.Add(managerRaw.GetAncOfType(typeof(Declarator)));
 
CxList modelInstancesMembers = All.FindAllReferences(managerObject)
	.FindByType(typeof(UnknownReference)).GetMembersOfTarget();

result.Add(modelInstancesMembers.FindByShortName("save"));
result.Add(All.GetByAncs(allParams.GetParameters(relevantMethods, 1)));