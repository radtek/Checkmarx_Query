// Find spring file upload

CxList customAttributes = Find_CustomAttribute();
CxList multipartFileParam = Find_ParamDecl().FindByType("MultipartFile");
multipartFileParam = multipartFileParam * customAttributes.FindByShortName("RequestParam").GetAncOfType(typeof(ParamDecl));

CxList sanitizers = All.FindByName("*spring.servlet.multipart.max*");
if(sanitizers.Count == 0){
	result.Add(multipartFileParam);
}