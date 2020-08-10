// SSL use in Frameworks

try 
{
	// Struts
	foreach (CxXmlDoc doc in cxXPath.GetXmlFiles("struts-config.xml"))
	{
		XPathNavigator navigator = doc.CreateNavigator();

		XPathNodeIterator nodeIter = navigator.Select("//set-property[@property='secure' and @value='true']");
		if (nodeIter.Count > 0)
		{
			result.Add(cxXPath.FindXmlNodesByLocalName("struts-config.xml", 2, "set-property", true, "property", "secure"));
		}
	}
}
catch (System.Xml.XmlException){}

try 
{
	// JSF
	result.Add(cxXPath.FindXmlNodesByLocalNameAndValue("web.xml", 2, "transport-guarantee", "CONFIDENTIAL"));
}
catch (System.Xml.XmlException){}
	
try 
{
	// Spring MVC
	result.Add(cxXPath.FindXmlNodesByLocalName("server.xml", 2, "Connector", true, "SSLEnabled", "true"));
	result.Add(cxXPath.FindXmlNodesByLocalName("server.xml", 2, "Connector", true, "scheme", "https"));	
}
catch (System.Xml.XmlException){}