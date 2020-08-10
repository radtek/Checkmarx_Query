//Get fields with sensitive information
CxList sensitive = Find_Personal_Info().FindByType(typeof(FieldDecl));
CxList classSens = sensitive.GetAncOfType(typeof(ClassDecl));

//Get classes with unknown references
classSens = Find_TypeRef().FindAllReferences(classSens).GetAncOfType(typeof(VariableDeclStmt));
classSens = Find_Declarators().GetByAncs(classSens);
classSens = Find_UnknownReference().FindAllReferences(classSens);

//Get all data from sensitive information
sensitive = All.FindAllReferences(sensitive);
sensitive.Add(classSens);

//Remove invoke methods (gets\sets) 
sensitive -= sensitive.GetMembersOfTarget().GetTargetOfMembers();

//Get every method output with an "mapping" annotation
CxList methodsWithAnnotation = Find_Spring_Inputs_Annotations().GetAncOfType(typeof(MethodDecl));
CxList outputs = All.FindByFathers(Find_ReturnStmt().GetByAncs(methodsWithAnnotation));
//Get other outputs
outputs.Add(Find_API_Response_Outputs());
outputs.Add(Find_Html_Outputs());
outputs.Add(Find_Write());
outputs -= Find_Web_Outputs();

//Sanitized references (DTO)
outputs -= Find_UnknownReference().FindByShortName("*DTO*");

//All outputs
outputs.Add(All.FindAllReferences(outputs));

CxList outputStmts = outputs.GetAncOfType(typeof(ExprStmt));
outputStmts.Add(outputs.GetAncOfType(typeof(ReturnStmt)));

//Get methods that are sanitized by rules\filters\authorizations annotations
CxList sanitizers = Find_Spring_Security_Annotations();
sanitizers = All.FindAllReferences(sanitizers.GetAncOfType(typeof(MethodDecl)));

//Sanitized outputs
outputStmts -= outputStmts.GetByAncs(sanitizers);

//Sensitive Data Exposure
result = sensitive.GetByAncs(outputStmts);
//Projects that use Jackson
result.Add(Find_Jackson_Serialization());

result = result.ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow);