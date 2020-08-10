CxList vueContextVariables = Find_UnknownReference().FindByShortName("cxVueCtx").GetAssigner();
CxList vueNewInstances = All.FindByCustomAttribute("VueInstance").GetAncOfType(typeof(ViewModelComponent));
CxList vueContextVariablesInsideNewInstances = vueContextVariables.GetByAncs(vueNewInstances);
	
vueContextVariables -= vueContextVariablesInsideNewInstances;

CxList dataDeclarations = Find_FieldDecls()
	.FindByFathers(vueContextVariables).FindByShortName("data");


CxList dataDeclarators = Find_Declarators().FindByFathers(dataDeclarations);
CxList dataFunctionDeclarations = Find_LambdaExpr().GetAssignee(dataDeclarators);

result = dataDeclarators - dataFunctionDeclarations;