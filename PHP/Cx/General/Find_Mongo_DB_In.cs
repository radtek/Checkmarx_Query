if (base.Find_ObjectCreations().FindByType("MongoClient").Count > 0)
{
	CxList methods = Find_Methods();

	List<String> methodsMongo = new List<String> {"find","insertOne"};

	CxList directDbMethods = methods.FindByShortNames(methodsMongo);
	result.Add(directDbMethods);
}