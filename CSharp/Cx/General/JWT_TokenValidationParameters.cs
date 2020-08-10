CxList objectCreations = Find_ObjectCreations();
CxList memberAccesses = Find_MemberAccesses();
CxList methods = Find_Methods();
CxList fields = Find_FieldDecls();

CxList tokenValidationParameters = memberAccesses.FindByShortName("TokenValidationParameters")
	.GetByAncs(methods.FindByShortName("AddJwtBearer"));

CxList fieldDecls = fields
	.GetByAncs(objectCreations.FindByName("TokenValidationParameters"))
	.GetByAncs(methods.FindByShortName("AddJwtBearer"));

tokenValidationParameters.Add(fieldDecls);
	
tokenValidationParameters.Add(All.FindByMemberAccess("TokenValidationParameters.*"));

result = tokenValidationParameters;