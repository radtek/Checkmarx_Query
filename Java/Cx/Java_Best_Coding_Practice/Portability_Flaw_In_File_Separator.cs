//Common-Lists
CxList listOfStrings = Find_Strings();  
CxList listOfMethods = Find_Methods();
CxList listOfMemberAccess = Find_MemberAccesses();
CxList htmlCreationMethodInvokes = Find_Html_Outputs(); 
CxList listOfParams = Find_Params();
CxList sanitize = Find_Integers();
CxList listOfConstantsDecl = Find_ConstantDecl();
CxList listOfConstants = Find_Constants();
CxList listOfUnkRef = Find_UnknownReference();
CxList listOfVarDecl = Find_VariableDeclStmt();
CxList listOfDec = Find_Declarators();
CxList listOfAssignExpr = Find_AssignExpr();
CxList listOfClasses = Find_Classes();
CxList listOfTypeRefs = Find_TypeRef();

CxList allNeededTypes = All.NewCxList();
allNeededTypes.Add(listOfParams);
allNeededTypes.Add(listOfUnkRef);
allNeededTypes.Add(listOfStrings);

CxList allTypesInFile = All.NewCxList();
allTypesInFile.Add(listOfUnkRef);
allTypesInFile.Add(listOfMemberAccess);
allTypesInFile.Add(listOfAssignExpr);

CxList listOfDeclAndMemberAndUnknown = All.NewCxList();
listOfDeclAndMemberAndUnknown.Add(listOfUnkRef);
listOfDeclAndMemberAndUnknown.Add(listOfMemberAccess);
listOfDeclAndMemberAndUnknown.Add(listOfVarDecl);

// java.io.File*
List<string> objNames = new List<string> { "File", "FileInputStream", "FileOutputStream", "FileReader", "FileWriter" };
CxList obj = Find_Object_Create().FindByShortNames(objNames);

// remove the local classes with File prefix to reduce FPs
CxList localClassRefs = listOfTypeRefs.FindByFathers(obj).FindAllReferences(listOfClasses);
obj -= localClassRefs.GetFathers();

CxList allElementsPropertiesFile = allTypesInFile.FindByFileName("*.properties");
CxList assignPropsAssignee = allElementsPropertiesFile * allTypesInFile.FindByAssignmentSide(CxList.AssignmentSide.Left);

obj.Add(listOfMethods.FindByMemberAccess("ReadCert.getKeyInfo"));

//Collection of string with separator

CxList linuxStrings = listOfStrings.FindByShortName("*/*"); 
CxList windowsStrings = listOfStrings.FindByShortName("*\\\\*"); 
CxList strings_with_separator = All.NewCxList();

//## remove invalid string separators, Illegal Characters in file/folderNames:
List < string > invalidLinux = new List<string> {"*<*","*>*","*;*","*\\n","*=*","*http:*","*https:","*?*"};
List < string > invalidWindows = new List<string> {"*<*","*>*","*=*","*\\n","*\\r","*http:*","*https:*","*?*","***","*/*","*|*"};

CxList invalidLinuxStrings = linuxStrings.FindByShortNames(invalidLinux);
CxList invalidWindowsStrings = windowsStrings.FindByShortNames(invalidWindows);

strings_with_separator.Add(linuxStrings - invalidLinuxStrings);
strings_with_separator.Add(windowsStrings - invalidWindowsStrings);


// Remove HTML Creations and Src Params
CxList htmlCreationParamsAndetSrcParams = All.NewCxList();
htmlCreationParamsAndetSrcParams.Add(allNeededTypes.GetParameters(htmlCreationMethodInvokes));
htmlCreationParamsAndetSrcParams.Add(allNeededTypes.GetParameters(htmlCreationMethodInvokes.GetMembersOfTarget().FindByShortName("setSrc")));

strings_with_separator -= htmlCreationParamsAndetSrcParams;

//Remove Replaces 
CxList replace = All.NewCxList();
replace.Add(listOfMethods.FindByShortName("replace*", false));
replace.Add(listOfMemberAccess.FindByShortName("replace*", false));


CxList replace_string = allNeededTypes.GetParameters(replace, 0).FindByType(typeof(StringLiteral));
CxList replace_with_separator = strings_with_separator * replace_string;

strings_with_separator -= strings_with_separator.GetByAncs(replace_with_separator);


CxList variablesOfStringWithSeparator = strings_with_separator.GetAssignee();
//find all references of string with separator(it will find references inside of a setter)
CxList referencesOfVariables = listOfUnkRef.FindAllReferences(variablesOfStringWithSeparator);
referencesOfVariables -= listOfDec.FindDefinition(referencesOfVariables);
strings_with_separator.Add(referencesOfVariables);
//it will be only used to find setters after we should remove them in order to have
//more readable flows
List<String> sanitizers = new List<String> {
		"getClass",
		"getClassLoader",
		"setContentType",
		"setTimezone",
		"setMimeType",
		"getServlet*",
		"createTempFileFromMime",
		"getRealPath",
		"split"
		};
String[] listOfClassTypes = new String[]{"Class","ClassLoader"};


CxList sanitizingMethods = listOfMethods.FindByShortNames(sanitizers, false); 
sanitizingMethods.Add(listOfDeclAndMemberAndUnknown.FindByTypes(listOfClassTypes));
CxList sanitizingParam = allNeededTypes.GetParameters(sanitizingMethods);
sanitize.Add(sanitizingMethods);
sanitize.Add(sanitizingParam);
sanitize.Add(sanitizingMethods.GetMembersOfTarget().FindByShortName("getResource*"));

//find setters that has some param related with separators
CxList methods_with_strings = strings_with_separator.GetAncOfType(typeof(MethodInvokeExpr));
strings_with_separator -= referencesOfVariables;

CxList strings_with_separator_untouched = strings_with_separator.Clone();
result = strings_with_separator.InfluencingOnAndNotSanitized(obj, sanitize).ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow);

//### End of First Result ###
List <string> setterMethodsNames = new List<string>(){"setParameter", "setProperty", "setAttribute"};
List <string> getterMethodsNames = new List<string>(){"getParameter", "getProperty", "getAttribute"};

CxList setterMethods = listOfMethods.FindByShortNames(setterMethodsNames);


// Catching variable references from properties files and setters;
CxList getterMethods = listOfMethods.FindByShortNames(getterMethodsNames);

CxList settersFirstParam = allNeededTypes.GetParameters(setterMethods);

//String literal params
CxList setterStringParam = settersFirstParam.FindByType(typeof(StringLiteral));
CxList unknownReferenceSetterParam = settersFirstParam.Clone();
unknownReferenceSetterParam -= setterStringParam;

CxList definitionSetterParam = listOfDec.FindDefinition(unknownReferenceSetterParam).GetByAncs(listOfConstants);

CxList setterFirstParamByType = setterStringParam.Clone();
setterFirstParamByType.Add(definitionSetterParam);

//getters that has influence on outputs
CxList gettersInfluencingOnOutputs = getterMethods.InfluencingOn(obj);

gettersInfluencingOnOutputs = gettersInfluencingOnOutputs.GetStartAndEndNodes(CxList.GetStartEndNodesType.StartNodesOnly);

//the first parameter of setter should influence the getter
CxList gettersInfluencedBySetterParameter = gettersInfluencingOnOutputs.InfluencedBy(setterFirstParamByType);

gettersInfluencedBySetterParameter.Add(gettersInfluencingOnOutputs);

List<Tuple<CxList,CxList>> interestingSetters_Getters = new List<Tuple<CxList,CxList>>();

foreach(CxList setterToGetter in gettersInfluencedBySetterParameter.GetCxListByPath()){
	
	CxList getter = setterToGetter.GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly);
	CxList getterFirstParam = allNeededTypes.GetParameters(getter, 0).FindByType(typeof(StringLiteral));
	
	CxList setterFirstParam = setterToGetter.GetStartAndEndNodes(CxList.GetStartEndNodesType.StartNodesOnly);

	
	CxList setter = setterFirstParam.GetAncOfType(typeof(MethodInvokeExpr));

	CSharpGraph setterParam = setterFirstParam.TryGetCSharpGraph<CSharpGraph>();
	CSharpGraph getterParam = getterFirstParam.TryGetCSharpGraph<CSharpGraph>();
	
	//Calculate if first param of setter is the same as getter
	if(setterParam != null && getterParam != null ){
	
		CxList setterFirstParam1 = assignPropsAssignee.FindByMemberAccess(getterParam.ShortName.Trim('"').Replace("get", ""));
		setterFirstParam1 = (setterFirstParam1.Count <= 0) ? assignPropsAssignee.FindByShortName(getterParam.ShortName.Trim('"').Replace("get", "")) : setterFirstParam1;
		CSharpGraph setterFirst1 = setterFirstParam1.TryGetCSharpGraph<CSharpGraph>();
		
		if(setterParam.ShortName.Trim('"').Equals(getterParam.ShortName.Trim('"')))
		{
			Tuple<CxList,CxList> t = Tuple.Create(setter, getter);
			interestingSetters_Getters.Add(t);
		
		}else if(setterFirst1 != null){
			Tuple<CxList,CxList> t = Tuple.Create(setterFirstParam1, getter);
			interestingSetters_Getters.Add(t);
		}
		else{
			Tuple<CxList,CxList> t = Tuple.Create(setter, All.NewCxList());
			interestingSetters_Getters.Add(t);	
			
		}
		
		
	}
}

CxList toRemove = All.NewCxList();
CxList notRemove = All.NewCxList();
CxList toAdd = All.NewCxList();
foreach(Tuple<CxList,CxList> pair in interestingSetters_Getters){
	CxList setter = pair.Item1;
	CxList getter = pair.Item2;
	CSharpGraph setterGraph = setter.TryGetCSharpGraph<CSharpGraph>();
	string setterName = setterGraph != null ? setterGraph.ShortName : null;
	CSharpGraph getterGraph = getter.TryGetCSharpGraph<CSharpGraph>();
	string getterName = getterGraph != null ? getterGraph.ShortName : null;
	//	
	CxList l1 = result.IntersectWithNodes(setterFirstParamByType.GetParameters(setter, 1));
	CxList l2 = result.IntersectWithNodes(getter);

	//if we have a setter and getter and is the same fuction eg.(setParameter == getParameter)
	//then we will check if both are contained in the previous result calculation then
	//we should not remove them
	//else if we only have the setter the result should be removed
	if(setterName != null && getterName != null && setterName.EndsWith(getterName.Replace("get", ""))){
		if(l1.Count > 0 && l2.Count > 0){
			
			notRemove.Add(l1);	
		}
	}
	else{
		if(l1.Count > 0 && l2.Count == 0){
			toRemove.Add(l1);
		
		}else if(setterName != null && getterName != null ){
			CxList assignMatch = strings_with_separator_untouched * setter.GetAssigner();
			if(assignMatch.Count > 0){
				CxList gettterPath = getter.InfluencingOn(obj);
				if(gettterPath.Count > 0){
					toAdd.Add(assignMatch.ConcatenateAllPaths(gettterPath));
			
				}
			}
		
		}
	}
}

toRemove = toRemove - (toRemove * notRemove);
//remove flows that only have the setter
result -= toRemove;
result += toAdd - (toAdd * result);