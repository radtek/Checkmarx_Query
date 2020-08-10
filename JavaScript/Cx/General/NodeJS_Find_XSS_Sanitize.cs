CxList methodInvoke = Find_Methods();
CxList allParams = Find_Parameters();
CxList uRef = Find_UnknownReference();
CxList mA = Find_MemberAccesses();

CxList urMA = All.NewCxList();
urMA.Add(uRef);
urMA.Add(mA);

CxList sanitize = methodInvoke.FindByShortNames(
	new List<string>{
		"escape",
		"encodeURIComponent",
		"*.escapeId"
		}
	);

//Add all parameters of sanitation methods to sanitizers
CxList _params = All.NewCxList();
_params.Add(allParams);
_params.Add(uRef);

CxList sanitizeParams = _params.GetParameters(sanitize);

sanitize.Add(urMA.GetByAncs(sanitizeParams));

// Add ESAPI encodeForHTML
CxList ESAPIencoders = All.FindByShortName("ESAPI").GetMembersOfTarget().FindByShortName("encoder");
sanitize.Add(ESAPIencoders.GetMembersOfTarget().FindByShortName("encodeForHTML"));

// Add xss-filters
CxList xssFilters = Find_Require("xss-filters").GetMembersOfTarget().FindByShortNames(
	new List<string>{
		"inHTMLData",
		"inHTMLComment",
		"inSingleQuotedAttr",
		"inDoubleQuotedAttr",
		"inUnQuotedAttr"
		}
	);
sanitize.Add(xssFilters);

result.Add(sanitize);
result.Add(NodeJS_Find_General_Sanitize());