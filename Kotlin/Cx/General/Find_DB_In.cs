// Android specific db items
if (Find_Android_Settings().Count > 0)
{
	result.Add(Find_Dao_DB_In());
	result.Add(Find_SQLiteDatabase_In());
	result.Add(Find_RoomDatabase_In());
	result.Add(Find_SupportSQLiteStatement_In());
}

result.Add(Find_Oracle_DB_In());
result.Add(Find_DAL_DB());
result.Add(Find_Exposed_In());
result.Add(Find_Vertx_DB_In());