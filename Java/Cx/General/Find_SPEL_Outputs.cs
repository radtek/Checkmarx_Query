CxList springImports = Find_Spring_Imports();

if(springImports.Count > 0) {
	CxList methods = Find_Methods();

	//SPEL 
	CxList parseMethods = methods.FindByExactMemberAccess("Expression.getValue");
	parseMethods.Add(methods.FindByMemberAccess("SpelExpression.getValue"));
	parseMethods.Add(methods.FindByMemberAccess("CompositeStringExpression.getValue"));
	parseMethods.Add(methods.FindByMemberAccess("CompiledExpression.getValue"));
	parseMethods.Add(methods.FindByMemberAccess("SpelNode.getValue"));
	parseMethods.Add(methods.FindByMemberAccess("SpelNode.getTypedValue"));

	//Spring eval tag
	CxList evalJspOutput = methods.FindByMemberAccess("spring.eval");

	result.Add(parseMethods);
	result.Add(evalJspOutput);
}