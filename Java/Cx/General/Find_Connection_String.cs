//Find StringLiteral in Connection methods

if (1 == param.Length && null != param[0] as CxList){
	
CxList pswParamInMethod = param[0] as CxList;
CxList strings = Find_Strings();
		
List<string> stringNames = new List<string>{"*key*","*pass*"};
CxList stringsPsw = strings.FindByShortNames(stringNames);
		
CxList methodsWithPsw = All.GetParameters(Find_Methods())
	.FindByShortNames(stringNames).GetAncOfType(typeof(MethodInvokeExpr));
CxList allParamzPsw = All.GetParameters(methodsWithPsw, 1);
stringsPsw.Add(allParamzPsw);

CxList paramzVars = Find_Declarators().FindByShortName(pswParamInMethod.FindByType("String"));
stringsPsw.Add(paramzVars.GetAssigner());

result = stringsPsw;			
}