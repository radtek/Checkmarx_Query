CxList inputs = Find_Inputs();
inputs.Add(Find_DB_Out());

CxList ModelAndView = Find_Object_Create().FindByShortName("ModelAndView");
ModelAndView = All.GetParameters(ModelAndView, 0);
CxList sanitize = Find_General_Sanitize();
sanitize.Add(Find_General_Sanitize_Injection());
result = ModelAndView.InfluencedByAndNotSanitized(inputs, sanitize);