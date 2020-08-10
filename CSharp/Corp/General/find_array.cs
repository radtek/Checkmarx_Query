// Find all declarators
CxList allDeclarators = All.FindByType(typeof(Declarator));
// Find all generic type refs
CxList genericTypeRefs = All.FindByType(typeof(GenericTypeRef));
// Transverse all the declarators
CxList mylist = All.NewCxList();
foreach(CxList declarator in allDeclarators){
	// Find variable decl stmt
	CxList tmp = declarator.GetAncOfType(typeof(VariableDeclStmt));
	// Find type reference
	CxList typeRef = All.GetByAncs(tmp).FindByType(typeof(TypeRef));
	typeRef -= genericTypeRefs;
	if(typeRef != null){
		cxLog.WriteDebugMessage("found type reference");
		TypeRef type = typeRef.TryGetCSharpGraph<TypeRef>();
		if(type != null){
			Checkmarx.Dom.RankSpecifierCollection ranks = type.ArrayRanks;
			if(ranks.Count != 0){
				mylist.Add(declarator);
			}
		}
	}
}
result = All.FindAllReferences(mylist);