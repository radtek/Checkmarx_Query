CxList tokenValidation = JWT_TokenValidationParameters();

CxList listToValidate = All.NewCxList();
CxList memberAccess = tokenValidation.FindByType(typeof(MemberAccess)).FindByShortName("RequireSignedTokens");
CxList fieldDecl = tokenValidation.FindByType(typeof(FieldDecl)).FindByShortName("RequireSignedTokens");

listToValidate.Add(memberAccess);
listToValidate.Add(fieldDecl);

CxList vulnerable = JWT_TokenParameter_Validation(listToValidate);

// Set the colletion to be validate
CxList fieldDecls = Find_FieldDecls();
CxList objectCreate = Find_ObjectCreations();

CxList toValidate = fieldDecls
	.GetByAncs(objectCreate.FindByName("TokenValidationParameters"));
CxList disableValidation = toValidate - tokenValidation;

CxList node = disableValidation.FindByType(typeof(FieldDecl)).FindByShortName("RequireSignedTokens");

// Get the field set to false
disableValidation = JWT_TokenParameter_Validation(node);

// Create flow from FieldDecl to x.TokenValidationParameters
CxList flows = JWT_FlowFromFliedToTokenValidationParameters(disableValidation);
vulnerable.Add(flows);

result = vulnerable;