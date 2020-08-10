CxList methods = Find_Methods();

result.Add(methods.FindByMemberAccess("HashAlgorithm.ComputeHash")); 
result.Add(methods.FindByMemberAccess("KeyedHashAlgorithm.ComputeHash")); 
result.Add(methods.FindByMemberAccess("*CryptoServiceProvider.ComputeHash"));  

result.Add(methods.FindByMemberAccess("MD5.ComputeHash"));
result.Add(methods.FindByMemberAccess("MD5Managed.ComputeHash"));
result.Add(methods.FindByMemberAccess("MD5Cng.ComputeHash"));

result.Add(methods.FindByMemberAccess("RIPEMD160.ComputeHash")); 
result.Add(methods.FindByMemberAccess("RIPEMD160Managed.ComputeHash")); 
result.Add(methods.FindByMemberAccess("RIPEMD160Cng.ComputeHash")); 

result.Add(methods.FindByMemberAccess("SHA1.ComputeHash")); 
result.Add(methods.FindByMemberAccess("SHA1Managed.ComputeHash")); 
result.Add(methods.FindByMemberAccess("SHA1Cng.ComputeHash")); 

result.Add(methods.FindByMemberAccess("SHA256.ComputeHash")); 
result.Add(methods.FindByMemberAccess("SHA256Managed.ComputeHash")); 
result.Add(methods.FindByMemberAccess("SHA256Cng.ComputeHash")); 

result.Add(methods.FindByMemberAccess("SHA384.ComputeHash")); 
result.Add(methods.FindByMemberAccess("SHA384Managed.ComputeHash")); 
result.Add(methods.FindByMemberAccess("SHA384Cng.ComputeHash")); 

result.Add(methods.FindByMemberAccess("SHA512.ComputeHash")); 
result.Add(methods.FindByMemberAccess("SHA512Managed.ComputeHash")); 
result.Add(methods.FindByMemberAccess("SHA512Cng.ComputeHash"));

// add DataContractSerializer and DataContractJsonSerializer instances as sanitizers
List<string> dataContractTypes = new List<string> {"DataContractSerializer", "DataContractJsonSerializer" };
CxList dataContract = Find_ObjectCreations().FindByShortNames(dataContractTypes);
result.Add(dataContract.GetAssignee());

// add XmlSerializer instances as sanitizers
CxList xmlSerializer = Find_ObjectCreations().FindByShortName("XmlSerializer");
xmlSerializer.Add(Find_Methods().FindByMemberAccess("XMLSerializerFactory.CreateSerializer"));
result.Add(xmlSerializer.GetAssignee());