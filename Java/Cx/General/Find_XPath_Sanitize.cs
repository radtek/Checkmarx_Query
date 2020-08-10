result.Add(Find_CollectionAccesses());
result.Add(Get_ESAPI().FindByMemberAccess("Encoder.encodeForXPath"));
result.Add(Find_Integers());

//XQuery sanitize
CxList methods = Find_Methods();
CxList xQueryExecute = methods.FindByMemberAccess("XQuery.execute");
CxList sanitizerParam = All.GetParameters(xQueryExecute, 2);
CxList refsOfSanitizer = All.FindAllReferences(sanitizerParam);
CxList sanitizerMethod = refsOfSanitizer.GetMembersOfTarget().FindByShortName("put");
result.Add(All.GetParameters(sanitizerMethod, 1));

//Sanitized with AbsInt
CxList unknRefs = Find_UnknownReference();
CxList stringAbsValue = unknRefs.FindByAbstractValue(abs => abs is StringAbstractValue);
stringAbsValue -= All.GetParameters(methods.FindByMemberAccess("request.getParameter"), 0);
result.Add(stringAbsValue);

//base64 encode
CxList base64Encode = Find_Base64Encoders();
CxList base64Decode = Find_Base64Decoders();
CxList reversedOperations = base64Encode.DataInfluencingOn(base64Decode);
result.Add(base64Encode - reversedOperations);