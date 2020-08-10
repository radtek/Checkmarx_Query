// Find all instances of JQuery objects use.
CxList JQueryObj = All.FindByShortNames(new List<string>{"jquery","$"}, false);
CxList jqMethods = Find_JQuery_Methods();
CxList Methods = Find_All_JQuery_Methods_Including_Aliases(jqMethods, jqMethods, 0);
CxList selectors = All.GetParameters(JQueryObj, 0);

// List of deprecated methods 
// https://api.jquery.com/category/deprecated/
List<string> deprecatedMethodNames = new List<string> {
	// Deprecated in 1.3
	"boxModel",
	"browser",	
	// Deprecated in 1.7
	"die",
	"sub",
	"live",
	"selector",
	// Deprecated in 1.8
	"andSelf",
	"error",
	"load",
	"size",
	"unload",
	// Deprecated in 1.9
	"support",
	// Deprecated in 1.10
	"context"
};

CxList deprecatedMethods = Methods.FindByShortNames(deprecatedMethodNames);

// List of deprecated selectors
// https://bugs.jquery.com/ticket/9400
List<string> deprecatedSelectorNames = new List<string> {
	":button",
	":checkbox",
	":file",
	":image",
	":input",
	":password",
	":radio",
	":submit",
	":text",
	":reset"		
};

CxList deprecatedSelectors = selectors.FindByShortNames(deprecatedSelectorNames);

// With regards to the toggle method (deprecated as of 1.8), we only 
// care if a function pointer is passed to it as 1st parameter
CxList toggleMethod = Methods.FindByShortName("toggle");
CxList toggleParameter = All.GetParameters(toggleMethod, 0);
CxList toggleLambdaParameter = toggleParameter.FindByType(typeof(LambdaExpr));
CxList toggleMethodDeclParameter = toggleParameter.FindByType(typeof(UnknownReference)).FindAllReferences(Find_MethodDecls());

CxList toggleLambdaParameterAll = All.NewCxList();
toggleLambdaParameterAll.Add(toggleLambdaParameter);
toggleLambdaParameterAll.Add(toggleMethodDeclParameter);

CxList toggleDeprecatedUsage = toggleMethod.FindByParameters(toggleLambdaParameterAll);

// The https://api.jquery.com/load-event/ is deprecated but http://api.jquery.com/load/ is not.
// Both have the same name (load) but a different number (and type) of arguments.
CxList loadMethods = deprecatedMethods.FindByShortName("load");
CxList argumentChildren = All.FindByFathers(Find_Param());
CxList firstArguments = argumentChildren.GetParameters(loadMethods, 0).FindByAbstractValue(a => a is StringAbstractValue);
CxList secondArguments = argumentChildren.GetParameters(loadMethods, 1);
CxList nonDeprecatedLoadArgs = All.NewCxList();
nonDeprecatedLoadArgs.Add(firstArguments, secondArguments);
CxList nonDeprecatedLoadMethod = nonDeprecatedLoadArgs.GetFathers().GetAncOfType(typeof(MethodInvokeExpr));
deprecatedMethods -= nonDeprecatedLoadMethod;

result.Add(deprecatedMethods);
result.Add(deprecatedSelectors);
result.Add(toggleDeprecatedUsage);