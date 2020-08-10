result = base.Find_Interactive_Inputs();

//result.Add(All.FindByName("ViewState_SortDir") +
//All.FindAllReferences(All.FindDefinition(All.FindByName("ViewState_SortDir"))) +
//All.FindAllReferences(All.FindByName("ViewState_SortDir")));

result.Add(All.FindByName("ns.User.Input") +
All.FindAllReferences(All.FindDefinition(All.FindByName("ns.User.Input"))) +
All.FindAllReferences(All.FindByName("ns.User.Input"))+ 
All.FindByMemberAccess("User.Input"));