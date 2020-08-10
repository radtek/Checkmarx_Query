//This query looking for places where software specifies permissions for a security-critical resource (files and directories) 
// in a way that allows that resource to be read or modified by unintended actors.

// first part -> handle the old way of file creation
// objects that create file or directory on HD. Access permit can be changed later 
//Find only Files&Paths Influenced By Assignment (new File("file"), Paths.get(args[argi]), Files.createDirectories...)
CxList fileFolderDecl = All.FindByTypes(new string [] {"File", "Path"});
fileFolderDecl -= Find_Properties_Files();

CxList PathFiles = fileFolderDecl.FindByType(typeof(UnknownReference)) + fileFolderDecl.FindByType(typeof(Declarator));
CxList leftSide = PathFiles.FindByAssignmentSide(CxList.AssignmentSide.Left);
CxList nullLiterals = Find_Null_String_Name();
//References assigned null are not vulnerable - there is no file creation
CxList assignedNull = nullLiterals.GetFathers() * leftSide;
assignedNull.Add(leftSide.FindByFathers(nullLiterals.GetFathers()));
leftSide -= assignedNull;
CxList allFiles = leftSide + PathFiles.DataInfluencedBy(leftSide);

//Find all Writers
CxList writersDecl = All.FindByTypes(new string [] {"FileSystems", "FileWriter", "FileOutputStream", "PrintWriter", "Writer"}, true);
allFiles.Add(writersDecl.FindByType(typeof(UnknownReference)));
allFiles.Add(writersDecl.FindByType(typeof(Declarator)));

CxList param_ = Find_Params();
CxList metods = Find_Methods();

// We can change permissions by "Files.setPosixFilePermissions" and (heuristicly) "file" is OK:
// Files.setPosixFilePermissions(file, changer.change(perms));
CxList Sanitizers = metods.FindByShortName("setPosixFilePermissions");
CxList paramOfSanitizers =  Sanitizers.DataInfluencedBy(allFiles).GetStartAndEndNodes(CxList.GetStartEndNodesType.StartNodesOnly);

CxList tempRes = All.NewCxList();
foreach( CxList af in allFiles)
{
	CxList allReffAf = allFiles.FindAllReferences(af);
                
	//on each one look for access to permissions by setExecutable,setReadable and setWritable
	CxList myrefWithMember = allReffAf.GetMembersOfTarget();
	CxList atr = myrefWithMember.FindByShortName("setExecutable");
	atr.Add(myrefWithMember.FindByShortName("setReadable"));
	atr.Add(myrefWithMember.FindByShortName("setWritable"));
	// if none of them, this is default permissions and can be unsafe
                
	// if allReffAf is the param of paramOfSanitizers (sanitizer), then it is OK, so we add it and Count >0
	atr.Add(allReffAf * paramOfSanitizers);
	if(atr.Count == 0){
		tempRes.Add(af);
	}
		// if such with params (true, false) - set file access for everybody = not good
	else foreach( CxList oneatr in atr)
		{
			if( param_.GetParameters(oneatr, 0).GetName() == "true" &&
				param_.GetParameters(oneatr, 1).GetName() == "false")
			{
				tempRes.Add(af);
				break;
			}
		}
}
               
// second part -> handle the SDK7 way of file creation

// in following code creating file with default permission without accessing to attrib.
// Files.write(Paths.get("file6.txt"), lines, utf8, StandardOpenOption.CREATE, StandardOpenOption.APPEND);
CxList pathsGet = All.FindByMemberAccess("Paths.get");

CxList fileMemberAccess = metods.FindByMemberAccess("Files.*");
fileMemberAccess.Add(metods.FindByMemberAccess("Paths.*"));
fileMemberAccess.Add(metods.FindByMemberAccess("FileChannel.*"));
fileMemberAccess.Add(metods.FindByMemberAccess("BufferedWriter.*"));

List < string > vulnerableMethodNames = new List<string>
	{"newBufferedWriter", "newOutputStream", "newByteChannel",
		"createFile", "createTempFile", "createTempDirectory", "readSymbolicLink",
		"write", "move", "copy", 
		"createDirectory", "createDirectories", "newDirectoryStream"}; 
CxList vulnerableMethod = fileMemberAccess.FindByShortNames(vulnerableMethodNames);
//"onflight" creation of file: Files.write(Paths.get("file6.txt"), lines
result.Add(pathsGet.GetParameters(vulnerableMethod));

// objects that create file or directory on HD directly with writing. Access permit can be given by attribute
//file that we access them in following way is (heuristicly) safe:
//Files.newByteChannel(file, options, attr)) Because FileAttribute<Set<PosixFilePermission>> attr = PosixFilePermissions.asFileAttribute(perms);
//Files.createFile(file, attr); - OK

CxList SuspParams = All.GetParameters(vulnerableMethod);
CxList BadParams = SuspParams.DataInfluencedBy(tempRes);
CxList badMetodsWBadParams = vulnerableMethod.FindByParameters(BadParams);
CxList attributes = All.FindAllReferences(All.FindByType("*FileAttribute*"));
CxList goodMethods = badMetodsWBadParams.FindByParameters(attributes);
CxList toRemove1 = tempRes.GetParameters(goodMethods);
CxList toRemove2 = tempRes.DataInfluencingOn(toRemove1);
result.Add(tempRes - toRemove1 - toRemove2);

result -= result.DataInfluencedBy(result);