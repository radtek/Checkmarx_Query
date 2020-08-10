// If it is an Android project - Add Android DB
if(Find_Android_Settings().Count > 0)
{
	result.Add(Find_Android_DB_Out());
}

result.Add(Find_Oracle_DB_Out());
result.Add(Find_SoapBindingStub_DB_Out());
result.Add(Find_qSQL_DB_Out());
result.Add(Find_SalesForce_DB_Out());
result.Add(Find_DAL_DB());
result.Add(Find_DB_Out_JdbcTemplate());
result.Add(Find_Ibatis_DB_Out());
result.Add(Find_MyBatis_DB_Out());
result.Add(Find_JDO_DB_Out());
result.Add(Find_SimpleDB_DB_Out());
result.Add(Find_SpringQuery_DB_Out());

result.Add(Find_Query_DB_In());
result.Add(Find_SpringHibernate_DB_In());
result.Add(Find_HibernateQuery_DB_In());
result.Add(Find_Hibernate_DB_In());
result.Add(Find_PostgreSQL_DB_In());