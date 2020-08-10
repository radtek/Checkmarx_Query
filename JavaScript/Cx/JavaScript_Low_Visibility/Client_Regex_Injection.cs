CxList inputs = Find_Inputs();
CxList sanitize = Sanitize();
CxList methods = Find_Methods();


//1. regex declaration of type ==> var patt=new RegExp(pattern,modifiers);
CxList newReg = Find_ObjectCreations().FindByShortName("RegExp");
CxList theReg = All.GetParameters(newReg, 0);
CxList regFromInput = theReg.InfluencedByAndNotSanitized(inputs, sanitize);
result.Add(regFromInput);


//2. RegExp methods exec + test
CxList execOrTest = methods.FindByShortNames(new List<string>{"exec","test"}).GetTargetOfMembers();
CxList blackList = methods.FindByShortNames(new List<string>{"limit","populate","skip","nSQL","query"});
execOrTest -= blackList;

result.Add(execOrTest.InfluencedByAndNotSanitized(inputs, sanitize).IntersectWithNodes(theReg));
result = result.ReduceFlow(CxList.ReduceFlowType.ReduceSmallFlow);