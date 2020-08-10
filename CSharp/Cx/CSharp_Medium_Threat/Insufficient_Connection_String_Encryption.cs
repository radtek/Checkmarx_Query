/// <summary>
/// Checks for insufficient connection string encryption 
/// </summary>

CxList connectionStringsValues = Find_Connection_String_Value();
CxList connectionStringsWithConcatValues = Find_Connection_String_Concat_Value();

// Builds the regex filters
var matchesEncryptRegex = new Regex(@"(Encrypt)=(?<val>True)", RegexOptions.IgnoreCase);
var matchesTCRegex = new Regex(@"(Trusted_Connection)=(?<val>False)", RegexOptions.IgnoreCase);
var matchesTCExistRegex = new Regex(@"Trusted_Connection=", RegexOptions.IgnoreCase);
var matchesTCPSRegex = new Regex(@"PROTOCOL\s*=\s*TCPS", RegexOptions.IgnoreCase);
var matchesSsl = new Regex(@"SslMode=(?<val>(require|verify-ca|verify-full))", RegexOptions.IgnoreCase);

CxList safeConnectionStrings = All.NewCxList();

CxList strings = Find_Strings().GetByAncs(connectionStringsWithConcatValues);

foreach(CxList concatValue in connectionStringsWithConcatValues)
{
	bool matchesEncryptTrue = false;
	bool matchesTCFalse = false;
	bool matchesTCExist = false;
	bool hasUserId = false;
	bool hasPassword = false;
	bool matchesTCPS = false;	
	bool hasSslRequire = false;

	foreach(CxList str in strings.GetByAncs(concatValue))
	{
		var connection = str.GetName();
		matchesEncryptTrue |= matchesEncryptRegex.IsMatch(connection);
		matchesTCFalse |= matchesTCRegex.IsMatch(connection);
		matchesTCExist |= matchesTCExistRegex.IsMatch(connection);
		matchesTCPS |= matchesTCPSRegex.IsMatch(connection);
		hasUserId |= connection.Contains("User ID=");
		hasPassword |= connection.Contains("Password=");
		hasSslRequire |= matchesSsl.IsMatch(connection);
	}
	
	bool hasCredentials = hasUserId || hasPassword;
	
	if((matchesEncryptTrue && matchesTCFalse) ||
		(matchesEncryptTrue && !matchesTCExist) ||
		hasCredentials == false || matchesTCPS || hasSslRequire)
	{
		safeConnectionStrings.Add(concatValue);
	}
}

result.Add(connectionStringsWithConcatValues);

// Checks if the connection strings are safe.
foreach(CxList element in connectionStringsValues) 
{
	string connection = element.GetName();
	bool matchesEncryptTrue = matchesEncryptRegex.IsMatch(connection);
	bool matchesTCFalse = matchesTCRegex.IsMatch(connection);
	bool matchesTCExist = matchesTCExistRegex.IsMatch(connection);
	bool matchesTCPS = matchesTCPSRegex.IsMatch(connection);
	bool hasUserId = connection.Contains("User ID=");
	bool hasPassword = connection.Contains("Password=");
	bool hasCredentials = hasUserId || hasPassword;
	bool hasSslRequire = matchesSsl.IsMatch(connection);
	
	if((matchesEncryptTrue && matchesTCFalse) ||
		(matchesEncryptTrue && !matchesTCExist) ||
		hasCredentials == false || matchesTCPS || hasSslRequire)
	{
		safeConnectionStrings.Add(element);
	}
}


result.Add(connectionStringsValues);
result -= safeConnectionStrings;