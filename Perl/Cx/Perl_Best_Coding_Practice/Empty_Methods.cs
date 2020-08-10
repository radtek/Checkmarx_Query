CxList methods = All.FindByType(typeof(MethodDecl));
methods -= methods.FindByShortName("Method_*");
methods -= methods.FindByRegex("="); // remove assign expressions
methods -= methods.FindByRegex("="); // remove second assign expression

result = Common_General.Find_Empty_Methods(methods);