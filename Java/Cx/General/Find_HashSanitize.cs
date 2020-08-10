CxList methods = base.Find_Methods();

CxList hashSanitize = methods.FindByMemberAccess("MessageDigest.digest");
hashSanitize.Add(methods.FindByMemberAccess("MessageDigest.update"));
hashSanitize.Add(methods.FindByMemberAccess("MD5.getHash*"));
hashSanitize.Add(methods.FindByMemberAccess("MD5.update*"));
hashSanitize.Add(methods.FindByShortName("md5", false));
hashSanitize.Add(methods.FindByMemberAccess("Cipher.doFinal"));

result = hashSanitize;