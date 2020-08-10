CxList methods = base.Find_Methods();

result.Add(methods.FindByMemberAccess("Base64.encode*", true));
result.Add(methods.FindByMemberAccess("BASE64Encoder.encode*", true));
result.Add(methods.FindByName("Base64.Encoder.encode*", true));
result.Add(methods.FindByName("Base64.getEncoder.encode*", true));