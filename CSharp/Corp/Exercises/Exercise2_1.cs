CxList input = Find_Methods().FindByShortName("GetSpeed");
CxList sink = All.FindByMemberAccess("Car.Drive");
//CxList sink = All.GetParameters(All.FindByMemberAccess("Car.Drive"),0);
result = input.InfluencingOn(sink);
//result = sink.InfluencedBy(input);