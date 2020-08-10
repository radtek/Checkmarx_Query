CxList test = Find_Test_Code();
CxList strings = Find_Strings();
CxList findIfNot = Find_If_Not_Stmt();
CxList apexFiles = Find_Apex_Files() - Find_Triggers_Code() - test;
CxList sObjectsInApex = Find_sObjects() * apexFiles;
CxList methods = Find_Methods() * apexFiles;
sObjectsInApex.Add(methods.FindByShortName("select", false));
CxList deleteMembers = sObjectsInApex.GetParameters(methods.FindByShortName("delete"))
	+ sObjectsInApex.GetParameters(methods.FindByShortName("merge"));
CxList isDeletableShort = methods.FindByShortName("isdeletable");

CxList isDeletable = isDeletableShort.FindByMemberAccess("getdescribe.isdeletable");

CxList getDescribe = apexFiles.FindByMemberAccess("get.getdescribe") +
	apexFiles.FindByMemberAccess("sobjecttype.getdescribe");
isDeletable.Add(isDeletableShort * isDeletableShort.DataInfluencedBy(getDescribe));
isDeletable.Add(apexFiles.FindAllReferences(apexFiles.GetMethod(isDeletable)));

// Less constraint createable
CxList isSomewhatDeletable = isDeletable + methods.FindByShortName("getDescribe", false).GetMembersOfTarget().FindByShortName("isDeletable", false);
isDeletable.Add(isSomewhatDeletable);

CxList returnValues = deleteMembers;

if ((isDeletable + Find_General_Permissions()).Count > 0)
{
	// Methods that call isCreateable
	CxList isDeletable2 = isDeletable - isDeletable.FindByShortName("isDeletable", false);
	

	CxList ifStmt = isDeletable.GetAncOfType(typeof(IfStmt));
	
	foreach (CxList stmt in ifStmt)
	{
		IfStmt ifGrph = stmt.TryGetCSharpGraph<IfStmt>();
		StatementCollection trueS = ifGrph.TrueStatements;
		if (trueS != null)
		{
			CxList potentialRemove = deleteMembers.GetByAncs(apexFiles.FindById(trueS.NodeId));
			
			CxList condition = apexFiles.GetByAncs(All.FindById(ifGrph.Condition.NodeId));
			condition = condition.GetTargetOfMembers().GetTargetOfMembers();
			condition.Add(condition.GetTargetOfMembers());
			condition.Add(condition.GetTargetOfMembers());
			condition.Add(condition.GetTargetOfMembers());
			

			CxList apexConditions = apexFiles.GetByAncs(condition);
			string[] names = new string[apexConditions.Count];
			int count = 0;
			foreach (CxList obj in apexConditions)
			{
				string name1 = (obj.GetFirstGraph()).ShortName.Trim(new char[]{'\'','"'});
				names[count++] = name1;
			}
			returnValues -= potentialRemove.FindByTypes(names);
			returnValues -= returnValues.FindAllReferences(apexConditions);
			foreach (CxList p in potentialRemove)
			{
				CxList parameters = strings.GetByAncs(apexFiles.GetParameters(p));
				if (parameters.Count > 0)
				{
					CSharpGraph gg = parameters.GetFirstGraph();
					string name = gg.ShortName.ToLower();
					if (name.IndexOf("from") >= 0)
					{
						name = name.Substring(name.IndexOf("from") + 4).Trim();
						if (name.IndexOf(" ") >= 0)
						{
							name = name.Substring(0, name.IndexOf(" ")).Trim();
						}
					}
					else
					{
						name = "";
					}
					if (condition.FindByShortName(name).Count > 0)
					{
						returnValues -= p;
					}
				}		 
			}
		}
	}
	// Removing all methods in which there is a relevant negative validation
	CxList ifNot = findIfNot * ifStmt;
	CxList ifToIgnore = Find_If_With_Return_or_Create() * ifNot;

	foreach (CxList i in ifToIgnore)
	{
		CxList ifNotMethod = apexFiles.GetMethod(i);
		CxList potentialRemove = returnValues.GetByAncs(ifNotMethod);

		
		
		CxList apexI = apexFiles.GetByAncs(i);
		string[] names = new string[apexI.Count];
		int count = 0;
		foreach (CxList obj in apexI)
		{
			string name1 = (obj.GetFirstGraph()).ShortName.Trim(new char[]{'\'','"'});
			names[count++] = name1;
		}

		returnValues -= potentialRemove.FindByTypes(names);
		returnValues -= returnValues.FindAllReferences(apexI);
	}

	// Heuristically remove check in constructor
	CxList exception = Find_Create_In_Constructor().GetByAncs(ifNot);
	CxList exceptionClass = apexFiles.GetClass(exception);
	returnValues -= returnValues.GetByAncs(exceptionClass);

	// Remove fields with security infrastructure
	CxList isDeletable3 = (isDeletable2 + isDeletable).GetByAncs(findIfNot);
	isDeletable3 -= isDeletable3
		.GetTargetOfMembers().GetTargetOfMembers().GetTargetOfMembers()
		.GetMembersOfTarget().GetMembersOfTarget().GetMembersOfTarget();
	

	deleteMembers = returnValues;
	foreach (CxList member in deleteMembers)
	{
		CSharpGraph g = member.GetFirstGraph();
		string typeName = g.TypeName;
		if (typeName == "")
		{
			CxList member1 = strings.GetByAncs(apexFiles.GetParameters(member));
			CSharpGraph g1 = member1.GetFirstGraph();
			typeName = g1.ShortName.ToLower();
			if (typeName.IndexOf("from") >= 0)
			{
				typeName = typeName.Substring(typeName.IndexOf("from") + 4).Trim();
				if (typeName.IndexOf(" ") >= 0)
				{
					typeName = typeName.Substring(0, typeName.IndexOf(" ")).Trim();
				}
			}
			else
			{
				typeName = "";
			}
		}
		if (typeName != "")
		{
			CxList methodMember = apexFiles.GetByAncs(apexFiles.GetMethod(member));
			CxList members = methodMember.FindByMemberAccess(typeName + ".sObjectType", false);
			members = members.GetByAncs(findIfNot);
			CxList methodMembers = members;
			methodMembers.Add(methodMember);
			CxList influenced = methodMembers.DataInfluencingOn(isDeletable) + isDeletable3 * methodMember;
			if (influenced.Count > 0)
			{
				returnValues -= member;
			}
		}
	}
}

result = returnValues;