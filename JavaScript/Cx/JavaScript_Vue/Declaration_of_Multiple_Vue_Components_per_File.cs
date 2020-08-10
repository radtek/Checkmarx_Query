CxList multiComponentFiles = All.NewCxList();

if(cxScan.IsFrameworkActive("VueJS")){
	CxList vueContextVariables = Find_UnknownReference().FindByShortName("cxVueCtx").GetAssigner();
	CxList vueNewInstances = All.FindByCustomAttribute("VueInstance").GetAncOfType(typeof(ViewModelComponent));
	CxList vueContextVariablesInsideNewInstances = vueContextVariables.GetByAncs(vueNewInstances);
	
	vueContextVariables -= vueContextVariablesInsideNewInstances;
	
	List<int> fileIds = new List<int>();
	CxList vueContextModels = All.FindByCustomAttribute("VueGlobalComponent").GetAncOfType(typeof(ViewModelComponent));

	CxList componentDefinitions = All.NewCxList();
	componentDefinitions.Add(vueContextVariables, vueContextModels);
	
	foreach(CxList component in componentDefinitions){
		try 
		{
			CSharpGraph comp = component.GetFirstGraph();
			if(fileIds.Contains(comp.LinePragma.GetFileId())) {
				multiComponentFiles.Add(component);
			} 
			else
			{
				fileIds.Add(comp.LinePragma.GetFileId());
			}
		}
		catch
		{
		}
	}
}

result = multiComponentFiles;