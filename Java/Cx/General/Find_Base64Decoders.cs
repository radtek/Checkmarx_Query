CxList methods = base.Find_Methods();

result.Add(methods.FindByMemberAccess("Base64.decode*", true));
result.Add(methods.FindByMemberAccess("BASE64Decoder.decode*", true));
result.Add(methods.FindByName("Base64.Decoder.decode*", true));
result.Add(methods.FindByName("Base64.getDecoder.decode*", true));