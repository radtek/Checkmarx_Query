CxList allGlobals = Get_All_Global_Variables();

// Get only variables with short name
foreach (CxList gl in allGlobals)
{
	CSharpGraph gr = gl.GetFirstGraph();
	if (gr.ShortName.Length < 6)
	{
		result.Add(gl);
	}
}