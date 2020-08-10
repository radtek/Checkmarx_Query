result = Find_ConstructorDecl()
	.FindByFieldAttributes(Modifiers.Public | Modifiers.Protected)
	.FindByFieldAttributes(Modifiers.Static);