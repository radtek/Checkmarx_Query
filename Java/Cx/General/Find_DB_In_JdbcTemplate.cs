// JdbcTemplate methods
CxList methods = Find_Methods();
List <string> JdbcTemplateMethodsNames = new List<string>() {"query*", "update*", "execute*", "batchUpdate*"};
CxList JdbcTemplateMethods = methods.FindByShortNames(JdbcTemplateMethodsNames);

// JdbcTemplate with Inheritance
CxList JdbcClasses = All.InheritsFrom("JdbcTemplate");
result.Add(JdbcTemplateMethods.GetByAncs(JdbcClasses));

// JdbcTemplate without Inheritance
CxList jdbc = methods.FindByMemberAccess("JdbcTemplate.*", false, StringComparison.Ordinal);
result.Add(jdbc.FindByShortNames(JdbcTemplateMethodsNames));

// Support getJdbcTemplate method
List<string> jdbcMethods = new List<string>{"getJdbcTemplate", "getNamedParameterJdbcTemplate"};
CxList getJdbcTemplateMembers = methods.FindByShortNames(jdbcMethods).GetMembersOfTarget();
result.Add(getJdbcTemplateMembers * JdbcTemplateMethods);