List<string> springInputAnnotations = new List<string> {
		"RequestMapping",
		"RequestPart",
		"PostMapping",
		"PutMapping",
		"DeleteMapping",
		"PatchMapping",
		"PathVariable",
		"GetMapping"
		};


CxList customAttributes = Find_CustomAttribute().FindByShortNames(springInputAnnotations);
result.Add(customAttributes);