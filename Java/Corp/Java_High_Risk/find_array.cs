// Find all declarators
CxList allDeclarators = All.FindByType(typeof(Declarator));
// Find all generic type refs
CxList genericTypeRefs = All.FindByType(typeof(GenericTypeRef));
// Transverse all the declarators
CxList mylist = All.NewCxList();
result = allDeclarators;