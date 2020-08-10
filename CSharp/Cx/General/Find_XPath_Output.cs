Dictionary < string,List < string >> potentialVulnerabilities = new Dictionary<string,List<string>>(){};
potentialVulnerabilities.Add("XmlAttribute", new List<string>(){"SelectSingleNode", "SelectNodes"});
potentialVulnerabilities.Add("XmlDocument", new List<string>(){"SelectSingleNode", "SelectNodes"});
potentialVulnerabilities.Add("XmlDocumentFragment", new List<string>(){"SelectSingleNode", "SelectNodes"});
potentialVulnerabilities.Add("XmlEntity", new List<string>(){"SelectSingleNode", "SelectNodes"});
potentialVulnerabilities.Add("XmlLinkedNode", new List<string>(){"SelectSingleNode", "SelectNodes"});
potentialVulnerabilities.Add("XmlNode", new List<string>(){"SelectSingleNode", "SelectNodes"});
potentialVulnerabilities.Add("XmlNotation", new List<string>(){"SelectSingleNode", "SelectNodes"});
potentialVulnerabilities.Add("XPathNavigator", new List<string>(){"Compile", "Evaluate", "Matches", "Select"});

/*Store Parameters,Methods and Classes*/
CxList parameters = Find_Param();
CxList methods = Find_Methods();
CxList classes = Find_ClassDecl();
CxList unknowns = Find_UnknownReference();

List<string> vulnKeys = new List<string>(potentialVulnerabilities.Keys);

List<string> refs = new List<string>();
List<string> inherits = new List<string>();

/*Get All References related to the classNames*/
foreach(CxList refe in All.FindByShortNames(vulnKeys).FindByType(typeof(TypeRef)))		
	refs.Add(refe.GetName());

/*Add Inherits to class names Because if we have X : XmlDocument, the class X wil inherit its methods*/
foreach(string name in refs){
	foreach(CxList inherit in classes.InheritsFrom(name)){
		string inheritName = inherit.GetName();
		if(!inherits.Contains(inheritName)){
			potentialVulnerabilities[inheritName] = potentialVulnerabilities[name];
			vulnKeys.Add(inheritName);
		}	
	}
}


/*Get Invocations*/
CxList invoke = All.NewCxList();

foreach(string className in vulnKeys){
	foreach(string methodName in potentialVulnerabilities[className]){
		invoke.Add(All.FindByMemberAccess(className, methodName));
	}
}	

/*Filter Invocations and get its parameters*/
foreach(CxList inv in invoke)
	result.Add(All.GetParameters(inv, 0));