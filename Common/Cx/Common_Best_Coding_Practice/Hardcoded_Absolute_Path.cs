CxList strings = Find_Strings();
result = strings.FindByName("c:\\*", false); 
result.Add(strings.FindByName("d:\\*", false));