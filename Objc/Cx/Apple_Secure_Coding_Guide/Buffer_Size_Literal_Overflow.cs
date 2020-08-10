CxList arrays = Find_ArrayCreateExpr();
CxList num = Find_IntegerLiterals();
CxList unknownList = Find_UnknownReference();
CxList declaratorList = Find_Declarators();
CxList arraysDeclarators = arrays.GetFathers() * declaratorList;
CxList indexerList = Find_IndexerRefs();
num.Add(unknownList.FindAllReferences(num.GetAssignee()).GetByAncs(arraysDeclarators));
num.Add(unknownList.FindByAbstractValue(x => x is IntegerIntervalAbstractValue).FindByFathers(indexerList));

foreach(CxList aux in arraysDeclarators){
	ArrayCreateExpr myArray = arrays.FindByFathers(aux).TryGetCSharpGraph<ArrayCreateExpr>();
	CxList numOfArrays = All.NewCxList();
	foreach (Expression expr in myArray.Sizes){
		numOfArrays.Add(expr.NodeId, expr);
	}
	CxList refArrays = unknownList.FindAllReferences(aux);	
	CxList indexer = refArrays.GetFathers().FindByType(typeof(IndexerRef));
	
	foreach(CxList numAux in numOfArrays){
		CxList varTowork = numAux;
		IntegerLiteral myInt = varTowork.TryGetCSharpGraph<IntegerLiteral>();
		if(myInt == null){
			varTowork = declaratorList.FindDefinition(varTowork).GetAssigner();
			myInt = varTowork.TryGetCSharpGraph<IntegerLiteral>();
		}
		if(myInt != null){
			CxList upperNumber = num.FindByFathers(indexer).FindByAbstractValue(x => x is IntegerIntervalAbstractValue isInt && isInt.UpperIntervalBound >= myInt.Value);
			result.Add(aux.ConcatenateAllPaths(upperNumber));
			indexer = indexer.GetFathers().FindByType(typeof(IndexerRef));
		}
	}
}