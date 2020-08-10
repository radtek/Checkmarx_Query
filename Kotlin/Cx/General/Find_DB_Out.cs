CxList exposedModels = Find_Classes().InheritsFrom("Table");
CxList modelsInstances = Find_UnknownReference().FindAllReferences(exposedModels);
CxList exposedOuts = modelsInstances.GetMembersOfTarget().FindByShortNames(new List<string>{"select", "selectAll"});
result.Add(exposedOuts);

// Android specific db items
if (Find_Android_Settings().Count > 0)
{
	result.Add(Find_Dao_DB_Out());
	result.Add(Find_SQLiteDatabase_Out());
	result.Add(Find_RoomDatabase_Out());
	result.Add(Find_SupportSQLiteStatement_In());
}