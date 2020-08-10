CxList methods = Find_Methods();
CxList SQLiteDB = methods.FindByMemberAccess("SqLiteDataBase.*");
CxList SQLiteStmt = methods.FindByMemberAccess("SQLiteStatement.*");

List <string> sqliteMethods = new List<string>{"delete*", "query*", "execSQL*", 
		"insert*", "compileStatement*", "rawQuery*", "update*", "replace*"};

List <string> sqliteStmtMethods = new List<string>{"execute*", "simpleQueryFor*"};

result.Add(SQLiteDB.FindByShortNames(sqliteMethods));
result.Add(SQLiteStmt.FindByShortNames(sqliteStmtMethods));