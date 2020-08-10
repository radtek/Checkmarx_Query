// Query Failure_to_Implement_Least_Privilege 
// ------------------------------------------

CxList strings = Find_Strings();
CxList methods = Find_Methods();
CxList settings = Find_Android_Settings();
CxList androidPermission = settings.FindByShortName("*android.permission.*", false);
// Application Requiered Network access but not uses it
CxList permissionInternet = androidPermission.FindByShortName("*android.permission.INTERNET*", false);
CxList usingNetwork = All.NewCxList();
usingNetwork.Add(
	Find_TypeRef().FindByTypes(new string[]{ "HttpClient", "OkHttpClient" }),
	methods.FindByMemberAccess("URL.openConnection", false),
	methods.FindByMemberAccesses(new string[]{ "Connector.open", "Transport.send", "Retrofit.create" }),
	All.FindByShortNames(new List<string>{ "HTTPConnection", "HttpGet"}),
	All.FindByType("Socket"));

if ((permissionInternet.Count > 0) && (usingNetwork.Count == 0)){
	result.Add(permissionInternet);
}
 
// Application Requiered SMS access but not uses it
CxList permissionSMS = androidPermission.FindByShortNames(new List<string>{
		"*android.permission.SEND_SMS*", 
		"*android.permission.RECEIVE_SMS*" }, false);

CxList usingSMS = All.FindByShortName("*SmsManager*");

if ((permissionSMS.Count > 0) && (usingSMS.Count == 0)){
	result.Add(permissionSMS);
}

// Read SMS content
CxList permissionSmsContect = androidPermission.FindByShortName("*android.permission.READ_SMS*");

// Find SMS content string
if(permissionSmsContect.Count > 1)
{
	CxList smsContent = All.NewCxList();
	foreach(CxList str in strings)
	{
		if(str.GetName().StartsWith("content://sms"))
		{
			smsContent.Add(str);
			result.Add(permissionSmsContect);
			break; // one is enough
		}
	}
}
// Application Requiered Telephony access but not uses it
CxList createIntent = All.FindByType("Intent").FindByType(typeof(ObjectCreateExpr));
CxList actionCall = All.FindByMemberAccess("Intent.ACTION_CALL");
CxList intentToCall = actionCall.GetParameters(createIntent);

CxList permissionPhone = androidPermission.FindByShortNames(new List<string> {
		"*android.permission.READ_PHONE_STATE*",
		"*android.permission.MODIFY_PHONE_STATE*",
		"*android.permission.PROCESS_OUTGOING_CALLS*",
		"*android.permission.CALL_PHONE*"}, false);

CxList usingPhone = All.FindByShortName("*TelephonyManager*");
usingPhone.Add(intentToCall);

if ((permissionPhone.Count > 0) && (usingPhone.Count == 0)){
	result.Add(permissionPhone);
}

// Application Requiered GPS access but does not use it
CxList permissionGPS = androidPermission.FindByShortNames(new List<string> {
		"*android.permission.ACCESS_FINE_LOCATION*",
		"*android.permission.ACCESS_COARSE_LOCATION*"}, false);	

CxList usingGPS = All.NewCxList();
usingGPS.Add(
	All.FindByShortNames(new List<string>{ "*LocationManager*", "*LocationListener*" }, false),
	All.FindByMemberAccesses(new string[]{ "*MapView.*", "*MapActivity*.*", "*GeoPoint.*", "*MyLocationOverlay.*" }, false));

if ((permissionGPS.Count > 0) && (usingGPS.Count == 0)){
	result.Add(permissionGPS);
}

// Application Requiered Contacts access but not uses it

CxList permissionContacts = All.NewCxList();
permissionContacts.Add(androidPermission.FindByShortNames(new List<string>{ 
		"*android.permission.READ_CONTACTS*",
		"*android.permission.WRITE_CONTACTS*" }, false));	

CxList usingContacts = All.NewCxList();
usingContacts.Add(All.FindByShortName("*ContactsContract*"),
	All.FindByName("*android.provider.CallLog*"),
	All.FindByName("*Contacts.People*"),
	All.FindByName("*Contacts.Phones*"),
	All.FindByName("*Contacts.Photos*"),
	All.FindByName("*Contacts.Organizations*"));


if ((permissionContacts.Count > 0) && (usingContacts.Count == 0))
{
	result.Add(permissionContacts);
}

// Application Required to be able to disable the device (very dangerous!).
CxList permissionBrick = androidPermission.FindByShortName("*android.permission.BRICK*", false);

if (permissionBrick.Count > 0){
	result.Add(permissionBrick);
}

// Application Requiered access to write on external storage but not uses it
CxList permissionExternalStorage = 
	androidPermission.FindByShortName("*android.permission.WRITE_EXTERNAL_STORAGE*", false);

CxList usingExternalStorage = All.FindByShortName(@"*/sdcard/*");
usingExternalStorage.Add(All.FindByMemberAccess("Environment.getExternalStorageDirectory"));

if ((permissionExternalStorage.Count > 0) && (usingExternalStorage.Count == 0)){
	result.Add(permissionExternalStorage);
}
// Application Requiered access to use camera but not uses it
CxList permissionCamera = androidPermission.FindByShortName("*android.permission.CAMERA*", false);
CxList usingCamera = All.FindByMemberAccess("*Camera.open*");
if ((permissionCamera.Count > 0) && (usingCamera.Count == 0))
{
	result.Add(permissionCamera);
}

// Application Requiered access to Record Audio
CxList permissionRecordAudio = androidPermission.FindByShortName("*android.permission.RECORD_AUDIO*", false);
CxList usingMicrophone = All.GetParameters(All.FindByName("*MediaRecorder.AudioSource.MIC*"), 0);

if ((permissionRecordAudio.Count > 0) && (usingMicrophone.Count == 0)){
	result.Add(permissionRecordAudio);
}

// Application Requiered ACCESS_NETWORK_STATE access but not uses it
CxList networkState = androidPermission.FindByShortName("*android.permission.ACCESS_NETWORK_STATE*", false);
CxList connectivityManager = All.FindByType("ConnectivityManager").FindByType(typeof(TypeRef));

if ((networkState.Count > 0) && (connectivityManager.Count == 0)){
	result.Add(networkState);
}

// NFC
CxList permissionNfc = androidPermission.FindByName("\"android.permission.NFC\"");

CxList ndefMsgCallback = All.FindByTypes(new string[] { "CreateNdefMessageCallback", "NfcAdapter" });

if (permissionNfc.Count > 0 && ndefMsgCallback.Count == 0)
	result.Add(permissionNfc);

// Manage accounts (add / remove / change credentials)
CxList permissionManageAccounts = androidPermission.FindByName("\"android.permission.MANAGE_ACCOUNTS\"");
CxList accountManager = All.FindByMemberAccess("AccountManager.*");

CxList manageAccounts = All.NewCxList();
manageAccounts.Add(accountManager.FindByMemberAccesses(new string[]{
	"AccountManager.addAccount",
	"AccountManager.removeAccount",
	"AccountManager.clearPassword",
	"AccountManager.confirmCredentials",
	"AccountManager.editProperties",
	"AccountManager.getAuthTokenByFeatures",
	"AccountManager.updateCredentials"}));

if(permissionManageAccounts.Count > 0 && manageAccounts.Count == 0)
	result.Add(permissionManageAccounts);

// Change configuration
CxList permissionChangeConfig = androidPermission.FindByName("\"android.permission.CHANGE_CONFIGURATION\"");
CxList changesConfig = All.FindByMemberAccess("Configuration.set*");
if(permissionChangeConfig.Count > 0 && changesConfig.Count == 0)
{
	result.Add(permissionChangeConfig);
}