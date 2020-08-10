CxList passwords = Find_All_Passwords();

//configuration
CxList customAttributes = Find_CustomAttribute();
CxList configurationClasses = customAttributes.FindByShortName("Configuration").GetFathers();
configurationClasses = configurationClasses.InheritsFrom("WebSecurityConfigurerAdapter");

CxList configurationOverridenMethods = customAttributes.FindByShortName("Override").GetFathers().FindByShortName("configure");
CxList configurationPasswords = Find_Methods().GetByAncs(configurationOverridenMethods).FindByShortName("password");

//bean
CxList beanUserDetailsService = customAttributes.FindByShortName("Bean").GetFathers().FindByShortName("userDetailsService");
CxList userDetailsPasswords = Find_Methods().GetByAncs(beanUserDetailsService).FindByShortName("password");

//values
CxList defaultValuePasswords = customAttributes.FindByShortName("Value").FindByFathers(passwords).GetFathers();

result.Add(configurationPasswords);
result.Add(userDetailsPasswords);
result.Add(defaultValuePasswords);