/////////////////////////////////////////////////////
// Find all elements in file AndroidManifest.xml
/////////////////////////////////////////////////////

result = All.FindByName("MANIFEST.*");
result.Add(Find_Strings());
result = result.FindByFileName("*AndroidManifest.xml");
result -= result.FindByFileName(cxEnv.Path.Combine("*", "bin", "AndroidManifest.xml"));