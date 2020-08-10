//Finds report metadata with no filters
 foreach (CxXmlDoc doc in cxXPath.GetXmlFiles("*.report"))
{
	XPathNavigator navigator = doc.CreateNavigator();
	XPathNodeIterator nodeIterator = navigator.Select("/*[local-name() = 'Report'][not(*[local-name()='filter'])]");
	cxLog.WriteDebugMessage(navigator.NameTable);
   
	while (nodeIterator.MoveNext())
	{
		XPathNavigator currentNodeNavigator = nodeIterator.Current;
		result.Add(cxXPath.CreateXmlNode(currentNodeNavigator, doc, 16, false));
	}
}