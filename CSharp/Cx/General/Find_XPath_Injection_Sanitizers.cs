CxList sanitizers = Find_Sanitize();
CxList integers = Find_Integers();
CxList hashFunctions = Find_Secure_Hash();
hashFunctions.Add(Find_Insecure_Hash());
CxList methodsBase64 = Find_Methods().FindByShortName("*Base64*");
CxList encoding = Find_URL_Encode();
encoding.Add(Find_HTML_Encode());
CxList deadcode = Find_Dead_Code_AbsInt();

sanitizers.Add(integers);
sanitizers.Add(hashFunctions);
sanitizers.Add(methodsBase64);
sanitizers.Add(encoding);
sanitizers.Add(All.GetByAncs(deadcode));

result = sanitizers;