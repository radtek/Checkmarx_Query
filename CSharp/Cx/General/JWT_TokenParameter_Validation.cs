if(param.Length == 1)
{	
	CxList vulnerable = All.NewCxList();
	
	CxList token = (CxList) param[0];
	
	
	// Member Access, Ex: TokenValidationParameters.RequireExpirationTime
	
	CxList disableValidation = token.FindByAbstractValue(abstractValue => abstractValue is FalseAbstractValue);

	vulnerable.Add(disableValidation);

	//x.TokenValidationParameters = new TokenValidationParameters
	//{
	//	ValidateIssuerSigningKey = true,
	//  ...
	//	
	CxList booleanFalse = All.FindByAssignmentSide(CxList.AssignmentSide.Right).FindByShortName("false");
	disableValidation = (token.GetAssigner() * booleanFalse).GetAncOfType(typeof(FieldDecl));
	
	vulnerable.Add(disableValidation);
	
	result = vulnerable;
	
}