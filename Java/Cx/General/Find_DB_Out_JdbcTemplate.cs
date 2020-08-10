List <string> JdbcTemplateMethodsNames = new List<string>() {"query*", "execute*"};
CxList methods = Find_Methods();

CxList queryMethods = All.NewCxList();

// JdbcTemplate with Inheritance
CxList JdbcClasses = All.InheritsFrom("JdbcTemplate");
CxList JdbcTemplateMethods = methods.FindByShortNames(JdbcTemplateMethodsNames);
queryMethods.Add(JdbcTemplateMethods.GetByAncs(JdbcClasses));

//JdbcTemplate without Inheritance
CxList jdbc = methods.FindByMemberAccess("JdbcTemplate.*", false, StringComparison.Ordinal);
queryMethods.Add(jdbc.FindByShortNames(JdbcTemplateMethodsNames));

CxList assignees = queryMethods.GetAssignee();
CxList methodsWithoutAssignee = queryMethods - assignees.GetAssigner();

result.Add(assignees);
result.Add(methodsWithoutAssignee);