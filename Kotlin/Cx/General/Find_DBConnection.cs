CxList methods = base.Find_Methods();
CxList objCreate = Find_ObjectCreations();
CxList connections = All.NewCxList();
CxList imports =  All.FindByType(typeof(Import));
CxList importNames = All.NewCxList();

foreach(CxList import in imports)
{
	var importObject = import.TryGetCSharpGraph<Import>() as Import;
	
	if(importObject != null)
	{
		if(importObject.Namespace == "kotliquery")
		{
			connections.Add(methods.FindByShortName("sessionOf"));
		}
		
		if(importObject.Namespace == "com.github.andrewoma")
		{
			connections.Add(methods.FindByShortName("DefaultSession"));
		}
		
		cxLog.WriteDebugMessage(importObject.NamespaceAlias);
	}	
}


connections.Add(methods.FindByMemberAccess("DriverManager.getConnection"));
connections.Add(methods.FindByMemberAccess("DataSource.getConnection"));
connections.Add(methods.FindByMemberAccess("DataSourceBuilder.url"));
connections.Add(methods.FindByExactMemberAccess("Configuration.setProperty"));
connections.Add(objCreate.FindByShortName("MongoClientURI"));

result = connections;