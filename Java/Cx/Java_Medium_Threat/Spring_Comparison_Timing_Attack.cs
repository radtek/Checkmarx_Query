CxList methods = Find_Methods();
CxList equalsMethod = methods.FindByShortName("equals");
CxList insecurePasswordEncoder = methods.FindByMemberAccess("PasswordEncoder.encode");
result = insecurePasswordEncoder.DataInfluencingOn(equalsMethod);