CxList inputs = Find_Interactive_Inputs();

string[] apis = new string[]
{
		"com.sun.identity.shared.configuration.ISystemProperties.*",
		"com.sun.identity.shared.configuration.SystemPropertiesManager.*",
		"com.iplanet.am.util.SystemProperties.*"
};

foreach (string name in apis)
{
	result.Add(inputs.FindByName(name));
}