if (param.Length == 1)
{

	CxList inputs = param[0] as CxList;

	CxList requests = All.NewCxList();
	requests.Add(inputs);
	requests -= requests.GetParameters(Find_MethodDeclaration().FindByName("*.main")
		.FindByFieldAttributes(Modifiers.Public | Modifiers.Static));
	requests.Add(Find_XSRF_Requests());

	CxList java_xsrf_sanitize = Find_XSRF_Sanitize();
	requests -= requests.GetByAncs(java_xsrf_sanitize);

	CxList db = Find_DB_In();

	CxList write = Find_XSRF_Write();

	CxList dbWrite = db.DataInfluencedBy(write);
	dbWrite.Add(Find_XSRF_Additional_DB_Write(db));
	CxList dbwriteInfluencedByRequests = dbWrite.DataInfluencedBy(requests);

	CxList csrf = All.FindByMemberAccess("csrf.disable");
	csrf.Add(All.FindByMemberAccess("CsrfConfigurer.disable"));

	dbwriteInfluencedByRequests.Add(csrf);

	result = dbwriteInfluencedByRequests;

}