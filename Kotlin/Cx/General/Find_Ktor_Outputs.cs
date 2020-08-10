CxList methods = Find_Methods();

// default respond text  methods
string[] respDefaultNames = new string[] {"call.respond", "call.respondText*", "call.respondOutputStream"};
CxList respText = methods.FindByMemberAccesses(respDefaultNames);
result.Add(respText);

// kotlinx.html outputs 
CxList respHtmlMethods = methods.FindByMemberAccess("call.respondHtml*");

// Add unsafe{} and script{} blocks, as those dont escape the output
CxList unsafeAndScriptMethods = methods.FindByShortNames(new List<string>{"unsafe", "script"}).GetByAncs(respHtmlMethods);
CxList unsafeAncs = All.GetByAncs(unsafeAndScriptMethods);

result.Add(unsafeAncs);
result.Add(unsafeAncs.GetAssigner());

// Add assignments, as those are not escaped by default
CxList assigns = Find_AssignExpr().GetByAncs(respHtmlMethods);
result.Add(All.GetByAncs(assigns).FindByAssignmentSide(CxList.AssignmentSide.Right));
// Add assignments to arrays a[b] = c => a.set(b, c)
result.Add(methods.FindByShortName("set"));

// Add views outputs from templates languages
result.Add(methods.FindByMemberAccess("call.respondTemplate"));
result.Add(Find_ViewOutputs());