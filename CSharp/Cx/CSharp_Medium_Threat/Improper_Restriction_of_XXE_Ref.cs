//Find XXE (XML External Entity vulnerability) in C# using:
//XmlReader, XmltextReader, XmlDocument, XDocument

CxList inputs = Find_Interactive_Inputs();
CxList integers = Find_Integers();
CxList unknownList = Find_Unknown_References();

CxList XXE = Find_XXE_XmlReader(); 	//Sanitized by default
XXE.Add(Find_XXE_XmlTextReader()); 	//Sanitized by default
XXE.Add(Find_XXE_XDocument());		//Sanitized by default
XXE.Add(Find_XXE_XmlDocument());   	//NOT Sanitized by default

XXE.Add(Find_XXE_XPathDocument());

CxList xxeSanitzer = Find_XXE_Sanitize();
CxList allRefsElmentSanitized = unknownList.FindAllReferences(xxeSanitzer.GetTargetOfMembers());
XXE -= allRefsElmentSanitized.GetMembersOfTarget();

result = XXE.InfluencedByAndNotSanitized(inputs, integers);