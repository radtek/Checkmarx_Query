bool partialQuery = false;
if (param.Length == 1)
{
	partialQuery = true;
}

CxList apex = Find_Apex_Files();
CxList test = Find_Test_Code();
CxList vfPages = Find_VF_Pages();
CxList apexFiles = apex - Find_Triggers_Code() - test;
CxList methodInvoke = apexFiles.FindByType(typeof(MethodInvokeExpr));
CxList objects = Find_sObjects();
CxList sObjectsInApex = objects * apexFiles;
CxList objectPages = objects * vfPages;
CxList objectPagesMembers = objectPages.GetMembersOfTarget();
sObjectsInApex.Add(sObjectsInApex.GetFathers().FindByType(typeof(ObjectCreateExpr)));
CxList methods = Find_Methods() * apexFiles;
CxList strings = Find_Strings();
CxList ifNotStmt = Find_If_Not_Stmt();
CxList ifYesStmt = Find_If_Yes_Stmt();

CxList create = apexFiles.FindByShortName("insert")
	+ apexFiles.FindByShortName("convertlead")
	+ apexFiles.FindByShortName("undelete")
	+ apexFiles.FindByShortName("upsert");

CxList createMembers = sObjectsInApex.GetParameters(create);

CxList isCreateableShort = methods.FindByShortName("iscreateable");
CxList isCreateable = isCreateableShort.FindByMemberAccess("getdescribe.iscreateable");
CxList getDescribe = apexFiles.FindByMemberAccess("get.getdescribe") +
	apexFiles.FindByMemberAccess("sobjecttype.getdescribe");

isCreateable.Add(isCreateableShort * isCreateableShort.DataInfluencedBy(getDescribe));
isCreateable.Add(apexFiles.FindAllReferences(apexFiles.GetMethod(isCreateable)));

// Remove the case where we only check CRUD (not FLS)
if (isCreateable.Count > 0)
{
	CxList nonSchemaDescribe = apexFiles.FindByName("*sobjecttype.getdescribe").GetMembersOfTarget();
	nonSchemaDescribe -= apexFiles.FindByName("*schema.sobjecttype.getdescribe").GetMembersOfTarget();
	isCreateable -= nonSchemaDescribe;
}
// Less constraint createable
CxList isSomewhatCreateable = isCreateable + methods.FindByShortName("getDescribe", false).GetMembersOfTarget().FindByShortName("isCreateable", false);
isCreateable.Add(isSomewhatCreateable);

// Methods that call isCreateable
CxList isCreateable2 = isCreateable - isCreateable.FindByShortName("isCreateable", false);

CxList ifStmt = isCreateable.GetAncOfType(typeof(IfStmt));
CxList returnValues = createMembers;

returnValues *= returnValues.DataInfluencedBy(apexFiles);

CxList assignTo = vfPages.FindByMemberAccess("apex.param").GetMembersOfTarget().FindByMemberAccess("param.assignto");
CxList sObjects = sObjectsInApex * sObjectsInApex.DataInfluencingOn(assignTo);
returnValues.Add(returnValues.FindAllReferences(sObjects));

// Heuristically removing all methods in which there is a negative validation
CxList ifNot = Find_If_Not_Stmt() * ifStmt;

// Heuristically remove check in constructor
CxList exception = Find_Create_In_Constructor().GetByAncs(ifNot);
CxList exceptionClass = apexFiles.GetClass(exception);
returnValues -= apexFiles.GetByAncs(exceptionClass);

// Remove field-level check

// Find assign
createMembers = returnValues;
CxList assign = apexFiles.GetAncOfType(typeof(AssignExpr));
assign = apexFiles.GetByAncs(assign);
assign = assign.FindByAssignmentSide(CxList.AssignmentSide.Left);
assign = assign.GetByAncs(assign) - methods;

CxList originalAdditionals = assign.GetByAncs(apexFiles.GetParameters(apexFiles.FindByType(typeof(ObjectCreateExpr))));
CxList assign2 = originalAdditionals.GetAncOfType(typeof(AssignExpr));
assign2 = apexFiles.FindByFathers(assign2);
CxList additionalMembers = assign2.DataInfluencingOn(originalAdditionals);

additionalMembers = additionalMembers.DataInfluencingOn(createMembers);
additionalMembers -= additionalMembers.GetTargetOfMembers();
additionalMembers = assign2 * additionalMembers;

System.Collections.SortedList permissionsList = new System.Collections.SortedList();
System.Collections.SortedList nameCxList = new System.Collections.SortedList();
System.Collections.SortedList typeCxList = new System.Collections.SortedList();

CxList assignTarget = assign.GetTargetOfMembers();
CxList apexAssignTarget = apexFiles * assignTarget;
CxList apexNotAssignTarget = apexFiles - apexAssignTarget;
CxList badFields = All.NewCxList();
CxList relevantMembers = All.NewCxList();
foreach (CxList member in createMembers)
{
	CSharpGraph g1 = member.GetFirstGraph();
	string type = g1.TypeName;
	if (!typeCxList.GetKeyList().Contains(type))
	{
		typeCxList.Add(type, apexFiles.FindByShortName(type, false));
	}

	CxList objectReferences = apexFiles.FindAllReferences(member);
	CxList orTypes = apexAssignTarget.FindByType(type);
	objectReferences.Add(orTypes.DataInfluencingOn(objectReferences));
	CxList fields = (objectReferences * assignTarget).GetMembersOfTarget();
	fields.Add(additionalMembers);
	
	CxList relevantFields = fields * fields.InfluencingOn(member);
	string name1 = g1.ShortName;
	CxList setters = All.NewCxList();
	if (member.FindAllReferences(sObjects).Count > 0)
	{
		setters = vfPages.FindByShortName("get" + name1, false).DataInfluencingOn(assignTo).GetMembersOfTarget().FindByShortName("set*");
		setters -= objectPagesMembers;
		relevantFields.Add(setters);
	}
	int numOfFields = relevantFields.Count;
	if (numOfFields > 0)
	{
		int fieldsOK = 0;
		bool found = false;
		System.Collections.ArrayList goodFields = new System.Collections.ArrayList();
		System.Collections.ArrayList membersList = new System.Collections.ArrayList();
		CxList assign0 = relevantFields.GetAncOfType(typeof(AssignExpr)); /*********/
		CxList field0 = originalAdditionals.GetByAncs(assign0); /*********/
		foreach (CxList field in relevantFields)
		{
			if (!partialQuery || !found)
			{
				bool isInitializer = (additionalMembers * field).Count > 0;
				bool fieldOK = false;
				string name = "";
				CSharpGraph g2 = null;
				if (isInitializer)
				{
					CxList assign1 = field.GetAncOfType(typeof(AssignExpr));
					CxList field1 = field0.GetByAncs(assign1); /*******************/
					g2 = field1.GetFirstGraph();
				}
				else
				{
					g2 = field.GetFirstGraph();
				}
				name = g2.ShortName;
				if ((field * setters).Count > 0)
				{
					name = name.Substring(3);
				}
				if (!membersList.Contains(name))
				{
					membersList.Add(name);
					if (!nameCxList.GetKeyList().Contains(name))
					{
						nameCxList.Add(name, apex.FindByShortName(name, false));
					}
					if (goodFields.Contains(name))
					{
						fieldsOK++;
					}
					else
					{
						CxList createable = All.NewCxList();
						string key = type + "." + name;
						if (permissionsList.ContainsKey(key))
						{
							createable = permissionsList[key] as CxList;
						}
						else
						{
							createable = Get_Permission("isCreateable", type, name, nameCxList[name], typeCxList[type]);
							createable.Add(isCreateable2);
							if (createable.Count > 0)
							{
								createable.Add(isCreateable * isCreateable.DataInfluencedBy(strings.FindByShortName("'" + name + "'")));
							}
							permissionsList[key] = createable;
						}
						if (createable.Count == 0)
						{
							found = true;
							badFields.Add(field);
							relevantMembers.Add(member);
							result.Add(field.DataInfluencingOn(member));
						}
						else
						{
							ifStmt = createable.GetAncOfType(typeof(IfStmt));
							ifNot = ifNotStmt * ifStmt;
							CxList ifToIgnore = Find_If_With_Return_or_Create() * ifNot;
							CxList ifNotMethod = apexFiles.GetMethod(ifToIgnore);
							fieldOK = member.GetByAncs(ifNotMethod).Count > 0;

							if (isInitializer &&
								field.GetAncOfType(typeof(ObjectCreateExpr)).FindByShortName(type).Count == 0)
							{
								fieldOK = true;
							}

							if (!fieldOK)
							{
								// Clean check inside if statement
								CxList ifYes = ifYesStmt * ifStmt;
								foreach (CxList stmt in ifYes)
								{
									IfStmt ifGrph = stmt.TryGetCSharpGraph<IfStmt>();
									StatementCollection trueS = ifGrph.TrueStatements;
									if (trueS != null)
									{
										if ((member + field).GetByAncs(apexFiles.FindById(trueS.NodeId)).Count > 0)
										{
											fieldOK = true;
										}
									}
								}
							}
			
							if (!fieldOK)
							{
								// Clean when the return is in the "else" of the if statement
								foreach (CxList stmt in ifNot)
								{
									IfStmt ifGrph = stmt.TryGetCSharpGraph<IfStmt>();
									StatementCollection falseS = ifGrph.FalseStatements;
									if (falseS != null)
									{
										if ((member + field).GetByAncs(apexFiles.FindById(falseS.NodeId)).Count > 0)
										{
											fieldOK = true;
										}
									}
								}
							}

							if (!fieldOK)
							{
								CxList methodMember = apexFiles.GetByAncs(apexFiles.GetMethod(member) + apexFiles.GetMethod(relevantFields));
								CxList methodsDecl = apexFiles.FindAllReferences(methodMember.FindByType(typeof(MethodDecl)));
								CxList invoke2 = methodInvoke.GetByAncs(apexFiles.GetMethod(methodsDecl));
								CxList localMethodInvoke = apexFiles.FindDefinition(invoke2);
								methodMember.Add(apexFiles.GetByAncs(localMethodInvoke));
								CxList members = methodMember.FindByMemberAccess(type + "." + name, false);
								CxList influenced = isSomewhatCreateable.DataInfluencedBy(members);
								if (influenced.Count > 0)
								{
									fieldOK = true;
								}
							}

							if (fieldOK)
							{
								fieldsOK++;
								goodFields.Add(name);
							}
							else
							{
								found = true;
								badFields.Add(field);
								relevantMembers.Add(member);
								result = field.Concatenate(member, true) + result;
							}
						}
					}
				}
			}
		}

		if (fieldsOK == numOfFields)
		{
			returnValues -= member;
		}
	}
}