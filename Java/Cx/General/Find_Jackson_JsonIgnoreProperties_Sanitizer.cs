CxList customAttrs = Find_CustomAttribute();
CxList strings = Find_Strings();
CxList decls = Find_Declarators();

//@JsonIgnoreProperties
CxList jsonIgnoreProperties = customAttrs.FindCustomAttributeParameterByKey("JsonIgnoreProperties", "value");

//Find string values from annotation
CxList sanitizeLiteral = strings.FindByFathers(jsonIgnoreProperties);
sanitizeLiteral.Add(jsonIgnoreProperties * strings);

//Get class with ignoreProperties and its declarators
CxList safeClass = sanitizeLiteral.GetAncOfType(typeof(ClassDecl));
CxList declsClass = decls.GetByAncs(safeClass);

//Sanitized vars
CxList sanitizedVars = All.NewCxList();
foreach(CxList s in sanitizeLiteral){
	sanitizedVars.Add(declsClass.FindByShortName(s.GetName()));
}

result = sanitizedVars.GetAncOfType(typeof(FieldDecl));