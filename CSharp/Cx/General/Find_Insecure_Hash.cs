CxList methods = Find_Methods();

CxList insecureHashes = All.NewCxList();
insecureHashes.Add(methods.FindByMemberAccess("DES.ComputeHash"));
insecureHashes.Add(methods.FindByMemberAccess("DSA.ComputeHash"));
insecureHashes.Add(methods.FindByMemberAccess("MD5.ComputeHash"));
insecureHashes.Add(methods.FindByMemberAccess("SHA1.ComputeHash"));
insecureHashes.Add(methods.FindByMemberAccess("MACTripleDES.ComputeHash"));
insecureHashes.Add(methods.FindByMemberAccess("3DES.ComputeHash"));
result = insecureHashes;