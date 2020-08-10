result.Add(All.FindByName("execute") +
All.FindAllReferences(All.FindDefinition(All.FindByName("execute"))) +
All.FindAllReferences(All.FindByName("execute")));