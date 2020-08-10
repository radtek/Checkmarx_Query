CxList encrypt = All.FindByShortName("*crypt*", false);
CxList encRemove = encrypt.FindByShortName("*decrypt*", false);
encRemove.Add(encrypt.FindByShortName("*unencrypt*", false));
encrypt -= encRemove;
encrypt.Add(All.FindByMemberAccess("Cipher.doFinal", false));
encrypt.Add(All.FindByMemberAccess("CipherInputStream.read", false));
encrypt.Add(All.FindByMemberAccess("CipherOutputStream.write", false));
encrypt.Add(All.FindByMemberAccess("MessageDigest.Digest", false));
// javax.crypto.spec.SecretKeySpec
encrypt.Add(All.GetParameters(All.FindByShortName("SecretKeySpec")));
// javax.crypto.KeyGenerator
encrypt.Add(All.FindByMemberAccess("KeyGenerator.init",false));
// javax.crypto.Cipher
encrypt.Add(All.FindByExactMemberAccess("Cipher.init"));
// java.security.MessageDigest
encrypt.Add(All.FindByExactMemberAccess("MessageDigest.update"));
encrypt.Add(All.FindByExactMemberAccess("MessageDigest.digest"));	
// java.security.Signature
encrypt.Add(All.FindByExactMemberAccess("Signature.initSign"));
encrypt.Add(All.FindByExactMemberAccess("Signature.initVerify"));
// java.security.KeyFactorySpi
encrypt.Add(All.FindByExactMemberAccess("KeyFactorySpi.engineGeneratePrivate"));
encrypt.Add(All.FindByExactMemberAccess("KeyFactorySpi.engineGeneratePublic"));
// java.security.KeyFactory
encrypt.Add(All.FindByExactMemberAccess("KeyFactory.KeyFactory"));
encrypt.Add(All.FindByExactMemberAccess("KeyFactory.generatePrivate"));
encrypt.Add(All.FindByExactMemberAccess("KeyFactory.generatePublic"));
// java.security.KeyPairGenerator
encrypt.Add(All.FindByExactMemberAccess("KeyPairGenerator.initialize"));

result = encrypt;