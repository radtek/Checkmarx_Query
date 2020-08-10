List<string> annotations = new List<string> {
		"PreFilter",
		"PostFilter", 
		"PreAuthorize",
		"PostAuthorize",
		"RolesAllowed",
		"Secured" };

result = Find_CustomAttribute().FindByShortNames(annotations);