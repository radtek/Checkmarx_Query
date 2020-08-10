result = base.Find_SQL_Sanitize();

result.Add(All.FindByName("ViewState_SortColumn") +
All.FindAllReferences(All.FindDefinition(All.FindByName("ViewState_SortColumn"))) +
All.FindAllReferences(All.FindByName("ViewState_SortColumn")));