CxList classesAndInterfaces = Find_Class_Decl();
classesAndInterfaces.Add(Find_InterfaceDecl());

CxList actionClass = Find_Action_Classes();
 
CxList newSetMethods = Find_MethodDeclaration().FindByShortName("set*");
CxList setMethods = All.NewCxList();
setMethods.Add(newSetMethods);
setMethods = setMethods.FindByFieldAttributes(Modifiers.Public);
CxList secondParameter = All.GetParameters(setMethods, 1);
setMethods -= secondParameter.GetFathers().GetFathers();

CxList mySetMethods = setMethods.GetByAncs(actionClass);
CxList struts2Inputs = All.GetParameters(mySetMethods, 0);
CxList inputsToSearch = All.NewCxList();
inputsToSearch.Add(struts2Inputs);

CxList classList = All.NewCxList();
CxList inputTypes;
int counter = 0;
do {
	inputTypes = All.NewCxList();
	foreach(CxList myInput in inputsToSearch)
	{
		CSharpGraph gr = myInput.TryGetCSharpGraph<CSharpGraph>();
		CxList myClassList = classesAndInterfaces.FindByShortName(gr.TypeName);
		inputTypes = myClassList - classList;
		classList.Add(inputTypes);
	}
	if (inputTypes.Count > 0)
	{
		mySetMethods = setMethods.GetByAncs(inputTypes);
		inputsToSearch = All.GetParameters(mySetMethods, 0);
		struts2Inputs.Add(inputsToSearch);
	}
}
while (inputTypes.Count > 0 && counter++ < 100);

CxList inActionClass = All.GetByAncs(actionClass);
CxList session = Find_Methods().FindByShortName("getSession");
CxList map = inActionClass.FindByType("Map");
session = map.DataInfluencedBy(session);
CxList sessionAware = inActionClass.InheritsFrom("SessionAware");
session.Add(inActionClass.FindByShortName("session").GetByAncs(sessionAware));

CxList sessionTarget = session.GetMembersOfTarget();
CxList sessionGet = sessionTarget.FindByShortName("get");
sessionGet -= sessionGet.FindByShortName("getAttribute");  // This is treated as real input/DB when applicable
CxList sessionGetParams = inActionClass.GetByAncs(All.GetParameters(sessionGet, 0));

CxList potentialInputs = All.NewCxList();
potentialInputs.Add(struts2Inputs);
potentialInputs.Add(sessionGetParams);
CxList potentialInputsDescsDefinitions = All.FindDefinition(All.GetByAncs(potentialInputs) - potentialInputs);
result = potentialInputs;
result.Add(All.GetParameters(newSetMethods.GetByAncs(potentialInputsDescsDefinitions)));

if(!All.isWebApplication)
{
	result.Add(All.FindByMemberAccess("Properties.getProperty"));
}

result -= Find_Interactive_Inputs();