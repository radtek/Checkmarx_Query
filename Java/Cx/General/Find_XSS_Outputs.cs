CxList sanitizers = All.GetByAncs(Find_XSS_Sanitize());
result = Find_Web_Outputs(); 
result -= Find_Header_Outputs();
result.Add(Find_Html_Outputs());
result -= sanitizers;

// addCookie isn't an ouput for XSS
result -= result.GetParameters(All.FindByMemberAccess("Response.addCookie"));

result -= result.GetTargetOfMembers();
result.Add(Find_AtgDspOutputs());

// Add console outputs found inside the "execute" method of classes that extend "ActionSupport".
CxList ActionSupportClasses = Find_Class_Decl().InheritsFrom("ActionSupport");
CxList ExecuteMethods = Find_MethodDeclaration().FindByShortName("execute").GetByAncs(ActionSupportClasses);
CxList ConsoleOutputs = Find_Console_Outputs().GetByAncs(ExecuteMethods);
result.Add(ConsoleOutputs);

// Cleanup ouputs from SWT, Swing and AWT because they are safe from XSS
CxList safeOutputs = All.NewCxList();
safeOutputs.Add(Find_AWT_Safe_Outputs());
safeOutputs.Add(Find_Swing_Safe_Outputs());
safeOutputs.Add(Find_SWT_Safe_Outputs());

// Remove safe uses of printStackTrace
safeOutputs.Add(Find_PrintStackTrace_Safe_Outputs());

result -= safeOutputs;