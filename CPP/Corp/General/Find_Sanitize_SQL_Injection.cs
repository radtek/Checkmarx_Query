result = base.Find_Sanitize_SQL_Injection();

result.Add(All.FindByName("fix") +
All.FindAllReferences(All.FindDefinition(All.FindByName("fix"))) +
All.FindAllReferences(All.FindByName("fix")));

result.Add(All.FindByName("execute") +
All.FindAllReferences(All.FindDefinition(All.FindByName("execute"))) +
All.FindAllReferences(All.FindByName("execute")));