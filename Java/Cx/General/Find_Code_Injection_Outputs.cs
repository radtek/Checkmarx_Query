CxList refl = All.FindByName("System.loadLibrary");
refl.Add(All.FindByName("System.load"));

CxList loadClass = All.FindByMemberAccess("ClassLoader.loadClass");
loadClass.Add(All.FindByMemberAccess("DexClassLoader.loadClass"));
loadClass.Add(All.FindByMemberAccess("BaseClassLoader.loadClass"));

CxList forNames = All.FindByName("Class.forName");
forNames.Add(All.FindByName("*.Class.forName"));

CxList classLoaders = All.GetParameters(forNames, 2);
classLoaders.Add(loadClass.GetTargetOfMembers());

CxList script = All.FindByMemberAccess("ScriptEngine.eval");
script.Add(All.FindByMemberAccess("getEngineByMimeType.eval"));
script.Add(All.FindByMemberAccess("getEngineByName.eval"));
script.Add(All.FindByMemberAccess("getEngineByExtension.eval"));

result.Add(refl);
result.Add(script);
result.Add(classLoaders);