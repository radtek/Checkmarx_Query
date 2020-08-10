CxList inputs = Find_Remote_Requests(); 
CxList unkRefs = Find_UnknownReference();
CxList httpImport = All.FindByMemberAccess("net/http.*");
CxList outputs = httpImport.FindByShortNames(new List<string>{"Redirect", "RedirectHandler"});
CxList sanitizers = Find_String_Sanitizer();
sanitizers.Add(Find_General_Sanitize());

CxList relevantArguments = unkRefs.GetParameters(outputs.FindByShortName("Redirect"), 2);
relevantArguments.Add(unkRefs.GetParameters(outputs.FindByShortName("RedirectHandler"), 0));
CxList relevantArgumentsReferences = unkRefs.FindAllReferences(relevantArguments);
CxList relevantArgumentsReferencesSanitized = relevantArgumentsReferences.GetParameters(sanitizers);
CxList sanitizedRefs = unkRefs.FindAllReferences(relevantArgumentsReferencesSanitized);
sanitizers.Add(sanitizedRefs);

result = outputs.InfluencedByAndNotSanitized(inputs, sanitizers);