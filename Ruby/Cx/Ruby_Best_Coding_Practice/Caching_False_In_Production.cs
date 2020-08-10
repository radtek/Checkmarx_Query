CxList production = All.FindByFileName(cxEnv.Path.Combine("*", "environments", "production.rb"));

CxList consider = production.FindByShortName("perform_caching");

result = production.DataInfluencingOn(consider).FindByShortName("false");