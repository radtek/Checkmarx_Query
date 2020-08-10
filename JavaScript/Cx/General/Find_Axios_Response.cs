CxList unknwnRef = Find_UnknownReference();
CxList methods = Find_Methods();
CxList lambdas = Find_LambdaExpr();
CxList paramDecls = Find_ParamDecl();

//axios(options)
CxList axiosCalls = methods.FindByShortName("axios");

//require('axios') instances and axios Unknown References
CxList axios = Find_Require("axios");
axios.Add(unknwnRef.FindByShortName("axios"));

//axios instances from axios.create()
CxList axiosInstances = (axios.GetMembersOfTarget() * methods.FindByShortName("create"));
axiosInstances.Add(axiosInstances.GetAssignee());
axiosInstances.Add(unknwnRef.FindAllReferences(axiosInstances));
axios.Add(axiosInstances);

//get methods axios.interceptors.response.use(func)
CxList interceptorsRespUse = methods.FindByName("*.interceptors.response.use");
interceptorsRespUse =
	(interceptorsRespUse.GetLeftmostTarget() * axios).GetRightmostMember() * interceptorsRespUse;

//get axios methods
CxList axiosMethodNames = methods.FindByShortNames(new List<string>() {
		"request",
		"get",
		"delete",
		"head",
		"options",
		"post",
		"put",
		"patch",
		"getUri",
		"all"
		});

CxList axiosMethodsCalls = axios.GetMembersOfTarget() * axiosMethodNames;
axiosMethodsCalls.Add(axiosCalls);

CxList thenPromise = methods.FindByShortName("then");

CxList axiosThenPromise = 
	thenPromise.DataInfluencedBy(axiosMethodsCalls).GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly);
CxList axiosThenLambdas = lambdas.GetParameters(axiosThenPromise);
//Add lambdas from first parameter of interceptors.response.use
axiosThenLambdas.Add(lambdas.GetParameters(interceptorsRespUse, 0));
//Get lambdas from axios.spread method invokes
CxList axiosSpreadThenInvokes = methods.GetParameters(axiosThenPromise).FindByName("*.spread");
axiosThenLambdas.Add(lambdas.GetParameters(axiosSpreadThenInvokes));
CxList axiosResponsesRefs = unknwnRef.FindAllReferences(paramDecls.GetParameters(axiosThenLambdas));
CxList axiosResponsesData = axiosResponsesRefs.GetMembersOfTarget().FindByShortName("data");

result = axiosResponsesData;