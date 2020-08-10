//This query intends to find permissions declared in the android manifest that are not used in the project
//Find permissions declared in the AndroidManifest.xml
CxList AndroidManifest = All.FindByFileName("*AndroidManifest.xml");
CxList permissions = AndroidManifest.FindByType(typeof(StringLiteral)).FindByShortName("android.permission.*");
permissions -= permissions.FindByShortName("android.permission.INTERNET");

//Find permissions used in the project 
CxList permissionsUsed = Find_MemberAccesses().FindByName("Manifest.permission").GetFathers();

//If the permissions declared are not used they're added to the results
foreach(CxList permission in permissions){
	string permissionString = permissions.GetName();
	string[] words = permissionString.Split('.');
	if (permissionsUsed.FindByShortName(words[words.Length - 1]).Count == 0){
		result.Add(permission);
	}
}