//CxList arrayDeclarator = All.FindByType(typeof(Declarator)).FindByRegex(@"(?<=\w+\[.*\]\s).*");
//result = All.FindAllReferences(arrayDeclarator);
CxList arrayDeclarator = All.FindByType(typeof(Declarator)).FindByRegex(@"(?<=\w+\[.*\]\s).*");
result = All.FindAllReferences(arrayDeclarator);
//CxList genericTypeRefs = All.FindAllReferences(arrayDeclarator);
//result = arrayDeclarator.Concatenate(genericTypeRefs);