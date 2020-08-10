CxList methods = base.Find_Methods();
CxList encodeHex = All.NewCxList();

encodeHex.Add(methods.FindByMemberAccess("Hex.encode*", true));
encodeHex.Add(methods.FindByMemberAccess("HexBin.encode"));

result = encodeHex;