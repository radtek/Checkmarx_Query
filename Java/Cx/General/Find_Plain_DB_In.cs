// If it is an Android project - Add Android DB
if(Find_Android_Settings().Count > 0)
{
	result.Add(Find_Android_DB_In());
}

result.Add(Find_Oracle_DB_In());
result.Add(Find_SoapBindingStub_DB_In());
result.Add(Find_qSQL_DB_In());
result.Add(Find_SalesForce_DB_In());
result.Add(Find_DAL_DB());
result.Add(Find_DB_In_JdbcTemplate());
result.Add(Find_Ibatis_DB_In());
result.Add(Find_MyBatis_DB_In());
result.Add(Find_JDO_DB_In());
result.Add(Find_SimpleDB_DB_In());
result.Add(Find_SpringQuery_DB_In());

result.Add(Find_Query_DB_In());
result.Add(Find_SpringHibernate_DB_In());
result.Add(Find_HibernateQuery_DB_In());
result.Add(Find_PostgreSQL_DB_In());

CxList Hibernate_DB_In = Find_Hibernate_DB_In();
result.Add(Hibernate_DB_In);

CxList PossibleHibernateTargets = result.GetTargetOfMembers();
CxList RedundantHibernateDBIns = Hibernate_DB_In * PossibleHibernateTargets;
RedundantHibernateDBIns.Add(All.FindAllReferences(Hibernate_DB_In.GetAssignee()) * PossibleHibernateTargets);

result.Add(Find_Methods().FindByShortName("executeStatement"));

result -= RedundantHibernateDBIns.GetMembersOfTarget();