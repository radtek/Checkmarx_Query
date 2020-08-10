if(cxScan.IsFrameworkActive("VueJS")){
	List < String > filemask = new List<String>(){"*.vue","*.html","*.htm"};

	//Looking for V-bind and shorthand : returning smaller list 
	CxList listofVBindshorthand = All.FindByRegexExt(@"(?<=\<[^>]+?\s+)(?<vbind>:\w+)(?=\=""\w+""[^>]*>)", filemask);
	CxList listofVBinds = All.FindByRegexExt(@"(?<=\<[^>]+?\s+)(?<vbind>v-bind:\w+)(?=\=""\w+""[^>]*>)", filemask);
	result = All.NewCxList();
	if(listofVBindshorthand.Count > 0 && listofVBinds.Count > 0){
		result.Add((listofVBindshorthand.Count < listofVBinds.Count) ? listofVBindshorthand : listofVBinds);
	}
	//Looking for V-on and shorthand @ returning smaller list 
	CxList listofVOnshorthand = All.FindByRegexExt(@"(?<=\<[^>]+?\s+)(?<vbind>@\w+)(?=\=""\w+""[^>]*>)", filemask);

	CxList listofVOn = All.FindByRegexExt(@"(?<=\<[^>]+?\s+)(?<vbind>v-on:\w+)(?=\=""\w+""[^>]*>)", filemask);
	if(listofVOnshorthand.Count > 0 && listofVOn.Count > 0){
		result.Add((listofVOnshorthand.Count() < listofVBinds.Count()) ? listofVOnshorthand : listofVOn);
	}
		
	//Looking for V-slot and shorthand # returning smaller list 
	CxList listofVSlotshorthand = All.FindByRegexExt(@"(?<=\<[^>]+?\s+)(?!"".*)(?<vbind>\#\w+)(?!.*"")(?=[^>]*>)", filemask);
	CxList listofVSlot = All.FindByRegexExt(@"(?<=\<[^>]+?\s+)(?<vbind>v-slot:\w+)(?=[^>]*>)", filemask);

	if(listofVSlotshorthand.Count > 0 && listofVSlot.Count > 0){
		result.Add((listofVSlotshorthand.Count < listofVSlot.Count) ? listofVSlotshorthand : listofVSlot);
	} 
}