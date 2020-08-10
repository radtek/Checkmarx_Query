CxList springInputsAnnotations = general.Find_Frameworks_Inputs_Annotations();

CxList controllers = general.Find_Controllers();

CxList customAttributes = springInputsAnnotations.GetByAncs(controllers);

List<string> suspiciousEndpoints = new List<string> {
		"*debug*",
		"*dev*",
		"*test*",
		"old_*",		
		"*secret*"
		};

CxList specificStringLiterals = general.Find_Strings().GetByAncs(customAttributes).FindByShortNames(suspiciousEndpoints);

result = specificStringLiterals.GetAncOfType(typeof(MethodDecl));