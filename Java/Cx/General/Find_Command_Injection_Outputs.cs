CxList processStart = All.FindByMemberAccess("ProcessBuilder.start");
CxList processCommandWithStart = All.FindByMemberAccess("ProcessBuilder.command").GetTargetOfMembers().DataInfluencingOn(processStart).GetMembersOfTarget();
CxList exec = All.FindByMemberAccess("Runtime.exec");
exec.Add(All.FindByMemberAccess("getRuntime.exec"));
exec.Add(All.FindByMemberAccess("System.exec"));
exec.Add(processStart);
exec.Add(All.GetParameters(processCommandWithStart));

/*	
	All.FindByMemberAccess("Runtime.exec") + 
	All.FindByMemberAccess("getRuntime.exec") +
	All.FindByMemberAccess("System.exec") +
	All.FindByMemberAccess("Call.setOperationName") +
	processStart + All.GetParameters(processCommandWithStart);// ProcessBuilder
*/	

CxList outputStream = All.FindByMemberAccess("Process.getOutputStream");
CxList osWrite = All.FindByMemberAccess("OutputStream.write");
// Find only Process.OutputStream.write
CxList processOStreamWrite = osWrite.InfluencedBy(outputStream).GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly);
exec.Add(processOStreamWrite);

CxList objectCreateUnkRefs = All.NewCxList();
objectCreateUnkRefs.Add(Find_UnknownReference());
objectCreateUnkRefs.Add(Find_Object_Create());

CxList inputs = Find_Interactive_Inputs();
CxList operationNameParams = objectCreateUnkRefs.GetParameters(Find_Methods().FindByMemberAccess("Call.setOperationName"));
CxList vulnerableOperationNameMethods = operationNameParams.DataInfluencedBy(inputs).GetAncOfType(typeof(MethodInvokeExpr));
exec.Add(vulnerableOperationNameMethods);

result = exec;