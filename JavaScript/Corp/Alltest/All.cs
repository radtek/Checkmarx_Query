//CxList CxAll = All;
//result = CxAll;
CxList userinfo = All.FindByShortName("userinfo");
CxList cookieinfo = All.FindByParameters(userinfo).FindByShortName("cookie");
//CxList cookieMethod = Find_Methods("$.cookie");
//CxList co = Find_Members("$.cookie");
result = cookieinfo;
