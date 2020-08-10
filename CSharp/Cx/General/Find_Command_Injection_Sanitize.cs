CxList methods = Find_Methods();

// Add Hashes and base64 encoding to sanitizers
result = Find_Secure_Hash() + Find_Insecure_Hash() + Find_Integers() + methods.FindByMemberAccess("Convert.ToBase64*");