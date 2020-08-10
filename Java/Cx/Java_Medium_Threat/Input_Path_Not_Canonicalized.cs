/////////////////////////////////////////////////////////////////
// Query: Input_Path_Not_Canonicalized
// Purpose: Make sure that input paths are canonicalized before 
//          being used in filesystem operations.
/////////////////////////////////////////////////////////////////

CxList inputs = Find_Inputs();
CxList methods = Find_Methods();

// Find file open
CxList fileOpen = Find_Files_Open();

// Methods that return the canocial path will serve as sanitizers when used on a comparison
string[] memberAccesses = new string[]{
	"File.getCanonicalPath",
	"Path.normalize",
	"FilenameUtils.normalize",
	"FilenameUtils.normalizeNoEndSeparator",
	"URI.normalize"
	};

CxList sanitize = methods.FindByMemberAccesses(memberAccesses);
sanitize.Add(sanitize.GetTargetOfMembers());

// Find all fileOpens that have some connection with sanitizers
CxList fileOpenSanitized = All.NewCxList();
fileOpenSanitized.Add(sanitize,
	fileOpen.InfluencedBy(sanitize),
	fileOpen.InfluencingOn(sanitize));

// Delete inputs that are sanitized
inputs -= inputs.InfluencingOn(fileOpenSanitized);

// get String.replace(...), String.replaceAll(...) and String.replaceFirst(...) methods
CxList replaceMethods = methods.FindByMemberAccess("String.replace*");

inputs = inputs.InfluencingOnAndNotSanitized(fileOpen, replaceMethods);
result = inputs.ReduceFlow(CxList.ReduceFlowType.ReduceSmallFlow);