//	OffByOne - loops
//  ////////////////-
//  Find all the loops that have "<=" instead of "<", thus creating an
//  off by one problem.
///////////////////////////////////////////////////////////////////////

// Find all iterations/loops
CxList iterations = All.FindByType(typeof(IterationStmt));
// Keep all iterations members (for performance, not to use All anymore)
CxList allIterationsMembers = All.GetByAncs(iterations);
CxList arraysAsParam = All.FindByType(typeof(RankSpecifier)).GetAncOfType(typeof(ParamDecl));
CxList arrayCreate = All.FindByType(typeof(ArrayCreateExpr));
CxList arraySize = All.FindByType(typeof(IntegerLiteral)).GetByAncs(arrayCreate);

// Run on every loop and look for the vulnerability
foreach (CxList iteration in iterations)
{
	try{
		// Check if we are dealing with a potential problem ("<=")
		IterationStmt iter = iteration.TryGetCSharpGraph<IterationStmt>();
		if ((iter.Test != null) && (iter.Test.ShortName.Equals("<=")))
		{
			// Keep all the members of the current iteration (performance)
			CxList iterationMembers = allIterationsMembers.GetByAncs(iteration);

			// Find all initializing expression of the iteration
			CxList initExpr = All.NewCxList();
			foreach(Statement stmt in iter.Init)
			{
				initExpr.Add(stmt.NodeId, stmt);
			}

			CxList condition = All.FindById(iter.Test.NodeId);
			CxList indexUses = iterationMembers.FindAllReferences(iterationMembers.GetByAncs(initExpr).FindByAssignmentSide(CxList.AssignmentSide.Left));

			// arrays into loop / iteration
			CxList arrays = iterationMembers.FindByType(typeof(IndexerRef));
			arrays -= arrays.FindAllReferences(arraysAsParam);

			CxList sizes = arraySize.GetByAncs(All.FindDefinition(arrays));

			// Right side, like count <= 99, the 99 will be returned
			CxList conditionParts = All.GetByAncs(condition) - condition - indexUses;

			indexUses = indexUses.FindByFathers(arrays);

			bool needToCheck = false;
			foreach (CxList array in arrays){
				CxList arrDef = All.FindDefinition(array);
				CxList curArraySize = All.FindByType(typeof(IntegerLiteral)).GetByAncs(arrDef);
				IntegerLiteral curArraySizeAsIntegerLiteral = curArraySize.TryGetCSharpGraph<IntegerLiteral>();
				if ((curArraySizeAsIntegerLiteral != null) &&
					(conditionParts.FindByShortName(curArraySizeAsIntegerLiteral.ShortName).Count > 0))
				{
					needToCheck = true;
				}
				// Add a check if "array.length" is used
				if (conditionParts.FindByShortName("length").Count > 0 &&
					conditionParts.FindByType(typeof(IntegerLiteral)).Count == 0 &&
					conditionParts.FindByShortName(array).Count > 0)
				{
					result.data.AddRange(array.data);
				}
			}

			// Find the values of initialization
			CxList val = iterationMembers.GetByAncs(initExpr).FindByAssignmentSide(CxList.AssignmentSide.Right);

			// If exists a value 0, then it's probably a problematic loop.
			if (needToCheck){
				if (val.FindByShortName("0").Count > 0){
					CxList iterationValue = iterationMembers.FindById(iter.Test.NodeId);
					CxList sizeMethod = conditionParts.FindByType(typeof(MethodInvokeExpr)).FindByShortName("size");

					if ((sizeMethod - iterationMembers.FindById(iter.Test.NodeId)).Count > 0)
					{
						CxList binary = conditionParts.FindByType(typeof(BinaryExpr));
						if (iterationMembers.GetByAncs(binary).Count > 0)
						{
							iterationValue = All.NewCxList();
						}
					}
					result.data.AddRange(iterationValue.data);

				}
			}

			// Sanitize if minus in interation test
			CxList TestNode = iterationMembers.FindById(iter.Test.NodeId);
			BinaryExpr b = conditionParts.TryGetCSharpGraph<BinaryExpr>();
			if (b != null && b.Operator == BinaryOperator.Subtract)
			{
				result -= TestNode;
			}
		}
	}
	catch(Exception ex)
	{
		cxLog.WriteDebugMessage(ex);
	}
}