CxList methods = Find_Methods();

CxList secureHashes = All.NewCxList();
secureHashes.Add(methods.FindByMemberAccess("AES.ComputeHash"));
secureHashes.Add(methods.FindByMemberAccess("ECDH.ComputeHash"));
secureHashes.Add(methods.FindByMemberAccess("ECDSA.ComputeHash"));
secureHashes.Add(methods.FindByMemberAccess("RIPEMD160.ComputeHash"));
secureHashes.Add(methods.FindByMemberAccess("SHA256.ComputeHash"));
secureHashes.Add(methods.FindByMemberAccess("SHA384.ComputeHash"));
secureHashes.Add(methods.FindByMemberAccess("SHA512.ComputeHash"));
secureHashes.Add(methods.FindByMemberAccess("RC2.ComputeHash"));
secureHashes.Add(methods.FindByMemberAccess("Rijndael.ComputeHash"));
secureHashes.Add(methods.FindByMemberAccess("RSA.ComputeHash"));
result = secureHashes;