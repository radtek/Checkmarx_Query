CxList methods = base.Find_Methods();
CxList objCreate = Find_Object_Create();
CxList connections = All.NewCxList();

connections.Add(methods.FindByMemberAccess("DriverManager.getConnection"));
connections.Add(methods.FindByMemberAccess("DataSource.getConnection"));
connections.Add(methods.FindByMemberAccess("DataSourceBuilder.url"));
connections.Add(methods.FindByExactMemberAccess("Configuration.setProperty"));
connections.Add(objCreate.FindByShortName("MongoClientURI"));

result = connections;