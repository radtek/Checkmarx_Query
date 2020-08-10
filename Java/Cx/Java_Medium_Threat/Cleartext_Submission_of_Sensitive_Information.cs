// Get all sensitive data. 
CxList personalInfo = Find_Personal_Info();

// Remove strings, since they might contain: "Enter password".
// A potential problem is that it might also contain: "password is...", but then it's hardcoded, 
// and not really sensitive information.
personalInfo -= Find_Strings();
personalInfo -= Find_Integers();
personalInfo -= personalInfo.FindByShortNames(new List<string> {"*regex*", "*pattern*"}, false);

// Remove declarators that are null or have an empty string assigned to it from personalInfo
CxList nullOrEmpty = Find_Null_String_Name();
nullOrEmpty.Add(Find_Empty_Strings());
CxList assignedNull = nullOrEmpty.GetFathers() * personalInfo;
assignedNull.Add(personalInfo.FindByFathers(nullOrEmpty.GetFathers()));

personalInfo -= assignedNull;

bool isUsingSSL = Framework_Is_Using_SSL().Count > 0;

if (!isUsingSSL)
{
	result = Find_Web_Cleartext_Submission_of_Sensitive_Information(personalInfo);
}

result.Add(Find_Server_to_Server_Cleartext_Submission_of_Sensitive_Information(personalInfo));