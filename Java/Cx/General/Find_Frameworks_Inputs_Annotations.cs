List <string> requestAnnotations = new List<string>() {
		"GET", "POST", "PUT", "DELETE", "PATCH", "HEAD"
		};

CxList customAttributes = Find_CustomAttribute().FindByShortNames(requestAnnotations);
result.Add(customAttributes);

CxList springAnnotations = Find_Spring_Inputs_Annotations();
result.Add(springAnnotations);