// Find ouputs from AWT because they are safe from XSS 
CxList unkRefs = Find_UnknownReference();
CxList imports = Find_Import();

CxList awtImportsToIgnore = imports.FindByName("java.awt*");

List < string > awtSafeList = new List<string>{"Font", "Label", "TextArea", "TextComponent", "TextField", 
		"Button", "Checkbox", "Choice", "Dialog", "Frame", "Graphics", 
		"Graphics2D", "Menu", "Panel", "PopupMenu"};


// Get all awt outputs from files with the "java.awt" import
CxList awtSafeOutputs = unkRefs.FindByFiles(awtImportsToIgnore).FindByTypes(awtSafeList.ToArray());

result = awtSafeOutputs;
result.Add(awtSafeOutputs.GetMembersOfTarget());