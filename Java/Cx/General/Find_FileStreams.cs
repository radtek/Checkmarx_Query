CxList fileStreams = All.NewCxList();
CxList methods = base.Find_Methods();

fileStreams.Add(methods.FindByMemberAccess("Files.readAllBytes"));
fileStreams.Add(methods.FindByMemberAccess("Files.readAllLines"));
fileStreams.Add(methods.FindByMemberAccess("FileInputStream.read*"));
fileStreams.Add(methods.FindByMemberAccess("FileReader.read*"));

result = fileStreams;