CxList methods = Find_Methods();

CxList processStart = methods.FindByMemberAccess("ProcessBuilder.start");
CxList processCommandWithStart = All.FindByMemberAccess("ProcessBuilder.command").GetTargetOfMembers().DataInfluencingOn(processStart).GetMembersOfTarget();
CxList exec = methods.FindByMemberAccess("Runtime.exec");
exec.Add(methods.FindByMemberAccess("getRuntime.exec"));
exec.Add(methods.FindByMemberAccess("System.exec"));
exec.Add(methods.FindByMemberAccess("Call.setOperationName"));
exec.Add(processStart);
exec.Add(All.GetParameters(processCommandWithStart));

CxList outputStream = methods.FindByMemberAccess("Process.getOutputStream");
CxList osWrite = methods.FindByMemberAccess("OutputStream.write");

// Find only Process.OutputStream.write
CxList processOStreamWrite = osWrite.InfluencedBy(outputStream).GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly);
exec.Add(processOStreamWrite);

result = exec;