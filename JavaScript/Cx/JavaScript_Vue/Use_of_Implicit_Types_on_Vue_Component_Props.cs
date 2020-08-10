CxList vueContextVariables = Find_UnknownReference().FindByShortName("cxVueCtx").GetAssigner();
CxList propsDeclaration = Find_FieldDecls().FindByFathers(vueContextVariables).FindByShortName("props");
CxList arrayPropDeclarations = propsDeclaration.GetAssigner(Find_ArrayCreateExpr());

result = arrayPropDeclarations;