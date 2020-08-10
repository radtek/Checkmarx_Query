CxList arrays = Find_ArrayInitializer();
arrays.Add(Find_ArrayCreateExpr());
CxList publicFinalStatic = Find_Constants().FindByFieldAttributes(Modifiers.Public).FindByFieldAttributes(Modifiers.Static).FindByFieldAttributes(Modifiers.Readonly);
CxList finalArrays = arrays.GetByAncs(publicFinalStatic);
result = finalArrays.GetAssignee();