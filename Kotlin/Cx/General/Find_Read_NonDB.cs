CxList methods = Find_Methods();
CxList unkRefs = Find_UnknownReference();
CxList lambdaExpr = Find_LambdaExpr();

// Finds reads which write to parameter
CxList readers = methods.FindByMemberAccess("AudioInputStream.read*");
readers.Add(methods.FindByMemberAccess("BufferedInputStream.read*"));
readers.Add(methods.FindByMemberAccess("BufferedReader.read*"));
readers.Add(methods.FindByMemberAccess("ByteArrayInputStream.read*"));
readers.Add(methods.FindByMemberAccess("CharArrayReader.read*"));
readers.Add(methods.FindByMemberAccess("DataInputStream.read*"));
readers.Add(methods.FindByMemberAccess("FilterInputStream.read*"));
readers.Add(methods.FindByMemberAccess("InputStream.read*"));
readers.Add(methods.FindByMemberAccess("InputStreamReader.read*"));
readers.Add(methods.FindByMemberAccess("LineNumberReader.read*"));
readers.Add(methods.FindByMemberAccess("ObjectInputStream.read*"));
readers.Add(methods.FindByMemberAccess("PipedInputStream.read*"));
readers.Add(methods.FindByMemberAccess("PipedReader.read*"));
readers.Add(methods.FindByMemberAccess("SequenceInputStream.read*"));
readers.Add(methods.FindByMemberAccess("ServletInputStream.read*"));
readers.Add(methods.FindByMemberAccess("StringBufferInputStream.read*"));
readers.Add(methods.FindByMemberAccess("StringReader.read*"));
readers.Add(methods.FindByMemberAccess("Files.readAllBytes"));
readers.Add(methods.FindByMemberAccess("Files.readAllLines"));
readers.Add(methods.FindByMemberAccess("FileInputStream.read*"));
readers.Add(methods.FindByMemberAccess("FileReader.read*"));


CxList parameters = All.GetParameters(readers, 0);
parameters -= parameters.FindByType(typeof(Param));

result = parameters;

// Kotlin extensions for File/Reader
List<string> fileExtMethodsNames = new List<string> {
		"bufferedReader",
		"CopyRecursively",
		"copyTo",
		"forEachBlock",
		"forEachLine",
		"inputStream",
		"readBytes",
		"readLines",
		"readText",
		"reader",
		"useLines"
		};


CxList toConsider = All.NewCxList();
string[] inputsMembers = new string[]{"File.*", "InputStream.*", "Reader.*"};
toConsider.Add(methods.FindByMemberAccesses(inputsMembers));

// SomeClass.java.getResource()
CxList getResources = methods.FindByMemberAccess("java.getResource*").GetMembersOfTarget();
toConsider.Add(getResources);

// Find possible references of this (extension methods)
string[] inputsTypes = new string[]{"File", "*InputStream", "InputStream*", "*Reader"};
CxList thisFile = Find_ThisRef().FindByTypes(inputsTypes).GetMembersOfTarget();
toConsider.Add(thisFile);

result.Add(toConsider.FindByShortNames(fileExtMethodsNames));

// Ktor extensions
result.Add(methods.FindByMemberAccess("File.resolve"));
result.Add(thisFile.FindByShortName("resolve"));

// Vertx file apis
List<string> asyncApis = new List<string> {"readDir", "readFile", "readSymlink"};
List<string> blockingApis = new List<string> {"readDirBlocking", "readFileBlocking", "readSymlinkBlocking"};

CxList fsApi = methods.FindByMemberAccess("vertx.fileSystem");
CxList fsApiItems = unkRefs.FindAllReferences(fsApi.GetAssignee());
fsApiItems.Add(fsApi);
fsApiItems.Add(unkRefs.FindByType("FileSystem"));
fsApiItems = fsApiItems.GetMembersOfTarget();

result.Add(fsApiItems.FindByShortNames(blockingApis));
// For async api's, we have to look at the action result of the lambda argument
CxList lambdaParams = lambdaExpr.GetParameters(fsApiItems.FindByShortNames(asyncApis), 1);
CxList paramRefs = unkRefs.FindAllReferences(All.GetParameters(lambdaParams, 0));
result.Add(paramRefs.GetMembersOfTarget().FindByShortName("result"));

if (!All.isWebApplication)
{
	result.Add(All.FindByMemberAccess("System.getenv"));
}