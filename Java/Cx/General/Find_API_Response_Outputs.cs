CxList listReturnStmt = Find_ReturnStmt();
CxList listOfCustomAttributes = Find_CustomAttribute();
CxList listOfTypeRefs = Find_TypeRef();
CxList listOfUnknownReferences = Find_UnknownReference();
CxList listOfDecl = Find_ParamDecl();
listOfDecl.Add(Find_Declarators());
CxList listOfParams = Find_Params();
CxList listOfStrings = Find_Strings();
CxList listOfMethods = Find_Methods();
CxList listOfMemberAccess = Find_MemberAccesses();

CxList listOfStringsAndUnknownRef = All.NewCxList();
listOfStringsAndUnknownRef.Add(listOfStrings);
listOfStringsAndUnknownRef.Add(listOfUnknownReferences);
listOfStringsAndUnknownRef.Add(listOfMemberAccess);
CxList listOfParamAndUnkRefAndMethodInvok = All.NewCxList();
listOfParamAndUnkRefAndMethodInvok.Add(listOfParams);
listOfParamAndUnkRefAndMethodInvok.Add(listOfMethods);
List < String > listOfCustomAttributeNames = new List<String>{
		"RequestMapping",
		"GetMapping",
		"PostMapping",
		"DeleteMapping",
		"PatchMapping",
		"GET",
		"POST",
		"PUT",
		"DELETE",
		"HEAD",
		"PATCH",
		"Path",
		"Produces",
		"Consumes"};

CxList listOfAllCustomAttributes = listOfCustomAttributes.FindByShortNames(listOfCustomAttributeNames, false);


List < String > mimetypes = new List < String >(){
		"text/*",
		"application/*",
		"APPLICATION_*",
		"TEXT_*",
		"IMAGE_*",
		"MULTIPART_*",
		"MIME_*"};
		//TODO : Check if Assignee is a producer or if CutomeAttributes is producer.
CxList listOfMimetypes = listOfStringsAndUnknownRef.GetByAncs(listOfAllCustomAttributes).FindByShortNames(mimetypes, false);



CxList listOfMimetypesAssignees = listOfMimetypes.GetAssignee();
listOfMimetypesAssignees.Add(listOfMimetypes.GetAncOfType(typeof(CustomAttribute)));


CxList listOfProduces = listOfMimetypesAssignees.FindByShortName("Produces", false);
CxList listOfMimetypesProduces = All.NewCxList();
listOfMimetypesProduces.Add(listOfProduces.GetAssigner());
listOfMimetypesProduces.Add(listOfStringsAndUnknownRef.GetByAncs(listOfProduces));

List < String > notExcudedmimetypes = new List < String >(){
		"*/json",
		"*_JSON*"};

CxList listOfJsonMimeTypesParents = listOfMimetypesProduces.FindByShortNames(notExcudedmimetypes, false).GetAncOfType(typeof(CustomAttribute));
CxList listOfAllDeclaredMimeTypes = listOfMimetypesProduces.GetAncOfType(typeof(CustomAttribute));


CxList listOfexcludedMimeTypes = listOfAllDeclaredMimeTypes - listOfJsonMimeTypesParents;
CxList listOfExcludedMethods = listOfexcludedMimeTypes.GetAncOfType(typeof(MethodDecl));
listOfExcludedMethods.Add(listOfexcludedMimeTypes);


CxList listOfAllMethods = (listOfAllCustomAttributes ).GetAncOfType(typeof(MethodDecl));
CxList declFatherMethods = listOfAllMethods - listOfExcludedMethods;


result = listOfParamAndUnkRefAndMethodInvok.GetByAncs(listReturnStmt.GetByMethod(declFatherMethods));



List < String > listOfFilterTypeRef = new List<String>(){
		"ModelAndView",
		"Model",
		"ModelMap"};

CxList listTypeRefFatheredByParamDecl = listOfTypeRefs.FindByShortNames(listOfFilterTypeRef);

CxList listOfFilteredParams = listTypeRefFatheredByParamDecl.GetAncOfType(typeof(ParamDecl));
listOfFilteredParams.Add(listOfDecl.GetByAncs(listTypeRefFatheredByParamDecl.GetAncOfType(typeof(VariableDeclStmt))));
CxList listOfUnkRefInfluencedByTypedParams = listOfUnknownReferences.InfluencedBy(listOfFilteredParams).GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly);

List < String > listOfFilterTypes = new List<String>(){
		"HttpServletResponse",
		"HttpSession"	};



CxList listTypeRefFatheredByParamDecl1 = listOfTypeRefs.FindByShortNames(listOfFilterTypes);

CxList listOfFilteredParamsServletResponse = listTypeRefFatheredByParamDecl1.GetAncOfType(typeof(ParamDecl));
listOfFilteredParamsServletResponse.Add(listOfDecl.GetByAncs(listTypeRefFatheredByParamDecl1.GetAncOfType(typeof(VariableDeclStmt))));
CxList listOfUnkRefInfluencedByTypedParamsServlet = listOfUnknownReferences.InfluencedBy(listOfFilteredParamsServletResponse).GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly);
List < String > outputMethods = new List<String>(){
		"addAttribute",
		"addObject",
		"setStatus",
		"addAllObjects",
		"addAllAttributes",
		"mergeAttributes"};



CxList listOfOutputingMethods = listOfUnkRefInfluencedByTypedParams.GetAncOfType(typeof(MethodInvokeExpr)).FindByShortNames(outputMethods, false);



List < String > outputMethodsResponse = new List<String>(){
		"setContentType",
		"print*",
		"setStatus",
		"write",
		"append",
		"setAttribute"};
CxList listOfOutputingMethodsServletResponse = listOfUnkRefInfluencedByTypedParamsServlet.GetAncOfType(typeof(MethodInvokeExpr))
	.FindByShortNames(outputMethodsResponse, false);


CxList listOfMimeInOutputMethods = listOfStrings.GetByAncs(listOfOutputingMethodsServletResponse).FindByShortNames(mimetypes, false).GetAncOfType(typeof(MethodInvokeExpr));
CxList listOfUnExcludedMimeTypes = listOfMimeInOutputMethods.FindByShortNames(notExcudedmimetypes, false).GetAncOfType(typeof(MethodInvokeExpr));

CxList setContentType = listOfOutputingMethodsServletResponse.FindByParameters(listOfMimeInOutputMethods);
CxList NonexcludedSetContentType = listOfOutputingMethodsServletResponse.FindByParameters(listOfUnExcludedMimeTypes);

listOfOutputingMethodsServletResponse = listOfOutputingMethodsServletResponse - (setContentType - NonexcludedSetContentType);

listOfOutputingMethods.Add(listOfOutputingMethodsServletResponse);
result.Add(listOfParams.GetParameters(listOfOutputingMethods));