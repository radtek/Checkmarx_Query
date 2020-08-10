CxList replaced = All.FindByShortName("replace*", false);

CxList replace = replaced.FindByType(typeof(MemberAccess));
replace.Add(replaced.FindByType(typeof(MethodInvokeExpr)));

result = replace.FindByParameterValue(0, "'", BinaryOperator.IdentityEquality)
	.FindByParameterValue(1, "'", BinaryOperator.IdentityInequality);
	
result.Add(replace.FindByParameterValue(1, "'", BinaryOperator.IdentityEquality)
	.FindByParameterValue(0, "'", BinaryOperator.IdentityInequality)
	.FindByParameterValue(2, "'", BinaryOperator.IdentityInequality));

result = All.GetByAncs(result).FindByShortName("replace*", false);