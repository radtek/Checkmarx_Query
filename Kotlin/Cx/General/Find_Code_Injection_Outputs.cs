CxList unkRefs = Find_UnknownReference();
CxList methods = Find_Methods();
methods.Add(Find_ObjectCreations());

// Reflection apis
CxList reflection = methods.FindByMemberAccesses(new string[]{
	"System.loadLibrary", "System.load",
	"Class.getMethod", "Class.forName",
	// Accesses from ::class.java
	"java.getMethod", "java.forName"
});

// Add loadClass usages by types
string[] loaders = new string[] {"ClassLoader.loadClass", "DexClassLoader.loadClass", "BaseClassLoader.loadClass"};
reflection.Add(methods.FindByMemberAccesses(loaders));

// Add loadClass usages from references
CxList assigners = methods.FindByMemberAccess("ClassLoader.getPlatformClassLoader");
assigners.Add(methods.FindByShortNames(new List<string>{"DexClassLoader", "BaseClassLoader"}));
assigners.Add(unkRefs.FindAllReferences(assigners.GetAssignee()));
reflection.Add(assigners.GetMembersOfTarget().FindByShortName("loadClass"));


// ScriptEngine.eval
List<string> names = new List<string> {"getEngineByName", "getEngineByMimeType", "getEngineByExtension"}; 
CxList scriptEngine = methods.FindByShortNames(names);
scriptEngine.Add(unkRefs.FindAllReferences(scriptEngine.GetAssignee()));

result.Add(reflection);
result.Add(scriptEngine.GetMembersOfTarget().FindByShortName("eval"));