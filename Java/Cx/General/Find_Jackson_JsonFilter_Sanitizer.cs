CxList customAttrs = Find_CustomAttribute();
CxList strings = Find_Strings();
CxList methods = Find_Methods();
CxList decls = Find_Declarators();

//@JsonFilter
CxList jsonFilter = customAttrs.FindCustomAttributeParameterByKey("JsonFilter", "value");

//Serialize all properites except ones includes in set
CxList serializeExcept = methods.FindByShortName("serializeAllExcept");	
CxList sanitizeLiteral = strings.GetByAncs(serializeExcept);

//Find classes with filters 
CxList addFilters = strings.FindByShortName(jsonFilter).GetAncOfType(typeof(MethodInvokeExpr));
CxList classes = jsonFilter.FindByShortName(All.GetParameters(addFilters, 0)).GetAncOfType(typeof(ClassDecl));

//Sanitized vars
CxList sanitizedVars = All.NewCxList();
foreach(CxList s in sanitizeLiteral){
	CxList sanDecl = decls.FindByShortName(s.GetName()).GetByAncs(classes);
	sanitizedVars.Add(sanDecl);
}

result = sanitizedVars.GetAncOfType(typeof(FieldDecl));