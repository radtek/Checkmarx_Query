// Query Client_Side_Injection
// ---------------------------
// This query finds possible SQL injections flows in the client side 
CxList methods = Find_Methods();
CxList methodDeclaration = Find_MethodDeclaration();
CxList classes = Find_Class_Decl();

CxList db = Find_Android_DB_In();

CxList ContentProvider = All.FindByType(All.InheritsFrom("ContentProvider"));
ContentProvider.Add(methods.FindByShortName("getContentResolver"));

ContentProvider = ContentProvider.GetMembersOfTarget();

CxList contentProviderMethods = methodDeclaration.GetByAncs(classes.InheritsFrom("ContentProvider"));
CxList contentProviderFirstParameter = All.GetParameters(contentProviderMethods, 0);

CxList temp = All.FindByMemberAccess("ContentResolver.*");

CxList contentQuery = All.NewCxList();
contentQuery.Add(temp.FindByMemberAccess("ContentResolver.query"));
contentQuery.Add(temp.FindByMemberAccess("ContentResolver.update"));
contentQuery.Add(temp.FindByMemberAccess("ContentProvider.query"));
contentQuery.Add(temp.FindByMemberAccess("ContentProvider.update"));

contentQuery.Add(ContentProvider.FindByShortNames(new List<string> {"query","update"})); 
db.Add(All.GetParameters(contentQuery, 2));

CxList contentDelete = All.FindByMemberAccess("ContentResolver.delete");
contentDelete.Add(All.FindByMemberAccess("ContentProvider.delete"));

contentDelete.Add(ContentProvider.FindByShortName("delete"));
db.Add(All.GetParameters(contentDelete, 1));

CxList getSharedPref = All.FindByMemberAccess("Context.getSharedPreferences").GetMembersOfTarget();
CxList sharedPrefString = All.NewCxList();
sharedPrefString.Add(All.FindByMemberAccess("SharedPreferences.getString"));
sharedPrefString.Add(All.FindByMemberAccess("SharedPreferences.getStringSet"));
sharedPrefString.Add(getSharedPref.FindByShortNames(new List<string> {"getString","getStringSet"}));

CxList inputs = All.NewCxList();
inputs.Add(Find_Read());
inputs.Add(sharedPrefString);
inputs.Add(methods.FindByMemberAccess("SmsMessage.get*"));
inputs.Add(methods.FindByMemberAccess("Folder.get*"));
inputs.Add(methods.FindByMemberAccess("EditText.getText"));
inputs.Add(methods.FindByMemberAccess("TextView.getText"));
inputs.Add(contentProviderFirstParameter);

CxList create = Find_Object_Create();
CxList textWatcher = create.GetParameters(create.FindByShortName("TextWatcher"));

CxList ss = classes.FindByShortName(textWatcher);
CxList textChanged = methodDeclaration.GetByAncs(ss).FindByShortName("*TextChanged");
inputs.Add(All.GetParameters(textChanged, 0));

//Add possible inputs from other applications that are allowed using intent filters.
CxList getIntents = methods.FindByShortName("getIntent");
CxList classDecls = Find_ClassDecl();
CxList stringLiterals = Find_Strings();
CxList sendIntentFilters = stringLiterals.FindByName(@"""android.intent.action.SEND"""); 

//Find the activities containing the filters
CxList filterFathers = sendIntentFilters.GetAncOfType(typeof(IfStmt));
filterFathers = filterFathers.GetFathers().GetAncOfType(typeof(IfStmt));
filterFathers = filterFathers.GetFathers().GetAncOfType(typeof(IfStmt));

CxList settings = Find_Android_Settings();
CxList activityNames = settings.FindByName(@"*ACTIVITY.ANDROID_NAME");
activityNames = activityNames.GetByAncs(filterFathers);
activityNames = stringLiterals.GetByAncs(activityNames.GetFathers());

foreach (CxList activityFather in filterFathers) 
{
	//Get the relevant activity name
	CxList activity = activityNames.GetByAncs(activityFather);
	CSharpGraph g = activity.TryGetCSharpGraph<CSharpGraph>();
	if (g.ShortName != null) 
	{
		string curName = g.ShortName.Trim('"');
		string[] curNameSplit = curName.Split('.');
		curName = curNameSplit[curNameSplit.Length - 1];
		//Find the class matching the activity name
		CxList curClass = classDecls.FindByShortName(curName);
		//Find the intents in curClass
		CxList curIntents = getIntents.GetByAncs(curClass);
		inputs.Add(curIntents);
	}
}

CxList exportedBroadcastReceiver = Find_Android_Exported_BroadcastReceiver();
CxList recieveMethod = methodDeclaration.GetByAncs(exportedBroadcastReceiver).FindByShortName("onRecieve");
inputs.Add(All.GetParameters(recieveMethod, 1));

CxList sanitized = Find_Sanitize();
result = db.InfluencedByAndNotSanitized(inputs, sanitized);