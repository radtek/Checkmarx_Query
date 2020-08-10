CxList CxAll = All.FindByFileName(@"*Clients\getContentType.jsp");
CxList CxgetContentType = CxAll.FindByShortName("getContentType").FindByType(typeof(MethodInvokeExpr));
result = CxgetContentType;