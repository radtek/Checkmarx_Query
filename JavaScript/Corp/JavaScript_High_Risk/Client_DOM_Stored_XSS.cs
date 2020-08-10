//web storage stored xss
CxList outputs = Find_Outputs_XSS();

CxList sanitize = Sanitize();
sanitize.Add(Find_XSS_Sanitize());

CxList sources = Find_Storage_Outputs();
sources.Add(Find_Cookie());
sources.Add(Find_DB_Out());
//gzcb自定义部分
CxList userinfo = All.FindByShortName("user*");
CxList cookieinfo = All.FindByParameters(userinfo).FindByShortName("cookie");
sources -= cookieinfo;

//All sources and elements influenced by them, that have a sanitizer in their ancestors, are also sanitizers
CxList sourcesAndInfluencedBy = All.NewCxList();
sourcesAndInfluencedBy.Add(sources);
sourcesAndInfluencedBy.Add(All.InfluencedBy(sources, CxList.InfluenceAlgorithmCalculation.NewAlgorithm));
sanitize.Add(sourcesAndInfluencedBy.GetByAncs(sanitize));

result = outputs.InfluencedByAndNotSanitized(sources, sanitize, CxList.InfluenceAlgorithmCalculation.NewAlgorithm);

result.Add(outputs.FindByType(typeof(Param)) * (sources) -sanitize);
result.Add(Find_Source_Equals_Sink(sources, outputs));
result = result.ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow);