CxList methods = Find_Methods();

//Hibernate SessionFactory
CxList sessionFactoryList = methods.FindByMemberAccess("SessionFactory.getCurrentSession");
sessionFactoryList.Add(methods.FindByMemberAccess("SessionFactory.openSession"));
sessionFactoryList.Add(methods.FindByMemberAccess("StatelessSession.openStatelessSession"));

CxList session = methods.FindByExactMemberAccess("Session","*");

List < string > sessionMethods = new List<string>{
		//Criteria
		"createCriteria",
		"createAlias",
		//Hibernate Query
		"createQuery",
		"createSQLQuery",
		//Hibernate Session
		"load",
		"get",
		"iterate",
		"find",
		"persist",
		"delete",
		"save",
		"update",
		"saveOrUpdate"
		};


CxList hibernateMethods = All.NewCxList();
hibernateMethods.Add(session.FindByShortNames(sessionMethods));

//Criteria
hibernateMethods.Add(methods.FindByMemberAccess("Criteria.add"));
hibernateMethods.Add(methods.FindByMemberAccess("Criteria.addOrder"));

sessionFactoryList = hibernateMethods.InfluencedBy(sessionFactoryList);

result = sessionFactoryList.GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly);

List<string> methodNames = new List<string>{"load", "find"};
result.Add(session.FindByShortNames(methodNames));