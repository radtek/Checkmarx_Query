//Get db methods and their sanitizers
CxList methods = Find_DB_Methods();
List < string > sanitizedMethods = new List<string> {"bindParam"};
result.Add(methods.FindByShortNames(sanitizedMethods));

//Find conditions variables of if statements
CxList expressions = Find_Expressions();
CxList ifStmt = base.Find_Ifs();
CxList conditions = expressions.FindByFathers(ifStmt);
CxList conditionsVars = expressions.GetByAncs(conditions);
result.Add(All.FindAllReferences(conditionsVars));