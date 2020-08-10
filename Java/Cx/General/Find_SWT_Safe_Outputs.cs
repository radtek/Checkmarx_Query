// Find ouputs from SWT because they are safe from XSS 
CxList unkRefs = Find_UnknownReference();
CxList imports = Find_Import();

CxList swtImportsToIgnore = imports.FindByName("org.eclipse.swt*");

List < string > swtSafeList = new List<string>{"Font", "TextLayout", "Button", "Combo", "Decorations", "Dialog", 
		"DirectoryDialog", "ExpandItem", "FileDialog", "FontDialog", "Group",
		"Item", "Label", "Link", "MenuItem", "MessageBox", "Shell", "TabItem",
		"TableItem", "Text", "ToolItem", "ToolTip", "TrayItem", "TreeColumn",
		"TreeItem"};

// Get all swt outputs from files with the "org.eclipse.swt" import
CxList swtSafeOutputs = unkRefs.FindByFiles(swtImportsToIgnore).FindByTypes(swtSafeList.ToArray());

result = swtSafeOutputs;
result.Add(swtSafeOutputs.GetMembersOfTarget());