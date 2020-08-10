// Personal List
result.Add(Find_personal_information(
	General_privacy_violation_list(),
	General_privacy_violation_exclude()
	));

// Credit List
result.Add(Find_personal_information(
	Credit_privacy_violation_list(), 
	Credit_privacy_violation_exclude()
	));

// Password List
CxList passwordPrivacyViolationList = Password_privacy_violation_list();
CxList findPersonalInfo = All.NewCxList();
findPersonalInfo.Add(passwordPrivacyViolationList.FindByType("String"));
findPersonalInfo.Add(passwordPrivacyViolationList.FindByFileName("*.jsp"));
findPersonalInfo.Add(passwordPrivacyViolationList.FindByFileName("*.vm"));

result.Add(Find_personal_information(
	findPersonalInfo,
	Password_privacy_violation_exclude()
	));