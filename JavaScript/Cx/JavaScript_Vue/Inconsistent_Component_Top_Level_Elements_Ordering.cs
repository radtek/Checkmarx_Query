if(cxScan.IsFrameworkActive("VueJS")){
	// this structure maps the fileId of the elements to its corresponding nodeId's using an array of:
	// [template element nodeId (0), script element nodeId (1), style element nodeId (2)] 
	Dictionary<int, int[]> fileToNodes = new Dictionary<int, int[]>();
	// this structure maps the fileId of the elements to its corresponding line using an array of: 
	// [template element line occurrence (0), script element line occurrence (1), style element line occurrence (2)]
	Dictionary<int, int[]> fileToLines = new Dictionary<int, int[]>();
	
	// the arrays used on the dictionaries start with -1 value by default indicating 
	// no occurence of that component part in the file.
	
	CxList orderErrorComponents = All.NewCxList();
	CxList scriptFirstComponents = All.NewCxList();
	CxList templateFirstComponents = All.NewCxList();
	CxList vueStyleDeclarations = All.NewCxList();
	CxList vueTemplateDeclarations = All.NewCxList();
	CxList vueScriptDeclarations = All.NewCxList();

	Func<CxList, int, bool> retrieveComponentInfo = (components, type) =>
		{
		foreach(CxList component in components){
			try 
			{
				CSharpGraph comp = component.GetFirstGraph();
				int fileId = comp.LinePragma.GetFileId();
				
				if(!fileToNodes.ContainsKey(fileId))
				{
					fileToNodes.Add(fileId, new int[3]{ -1, -1, -1});
					fileToLines.Add(fileId, new int[3]{ -1, -1, -1});
				}
				
				fileToNodes[fileId][type] = comp.NodeId;
				fileToLines[fileId][type] = comp.LinePragma.Line;
			}
			catch
			{
			}
		}		
		return true;
		};
	
	// Method to classify component ordering as error in case style is not the last occurrence and template or script first occurrence
	Func<bool> classifyComponents = () =>
		{
		foreach (var occurrence in fileToLines) 
		{   
			if((occurrence.Value[2] >= 0) && (occurrence.Value[2] < occurrence.Value[0] || occurrence.Value[2] < occurrence.Value[1]))
			{
				orderErrorComponents.Add(vueStyleDeclarations.FindById(fileToNodes[occurrence.Key][2]));
			}
			else 
			{
				if(occurrence.Value[0] >= 0 && occurrence.Value[1] >= 0) 
				{
					if(occurrence.Value[0] < occurrence.Value[1])
					{
						templateFirstComponents.Add(vueTemplateDeclarations.FindById(fileToNodes[occurrence.Key][0]));
					}
					else 
					{
						scriptFirstComponents.Add(vueScriptDeclarations.FindById(fileToNodes[occurrence.Key][1]));
					}
				}
			}
		}
		
		return true;
		};

	CxList vueComponentContext = Find_UnknownReference().FindByShortName("cxVueCtx").GetAssigner();
	CxList vueNewInstances = All.FindByCustomAttribute("VueInstance").GetAncOfType(typeof(ViewModelComponent));
	CxList vueContextVariablesInsideNewInstances = vueComponentContext.GetByAncs(vueNewInstances);
	
	vueScriptDeclarations = vueComponentContext - vueContextVariablesInsideNewInstances;
	vueTemplateDeclarations = Find_ViewDecls();
	vueStyleDeclarations = All.FindByRegexExt(@"<style[^>]*>(.|\n)*?<\/style>");
	
	// associate template occurence with position 0 (template position on the array) of line occurences array
	retrieveComponentInfo(vueTemplateDeclarations, 0);
	// associate script occurence with position 1 (script position on the array) of line occurences array
	retrieveComponentInfo(vueScriptDeclarations, 1);
	// associate style occurence with position 2 (style position on the array) of line occurences array
	retrieveComponentInfo(vueStyleDeclarations, 2);
	
	// classify components into 3 categories:
	// 1 - orderErrorComponents (components with style not as the last element)
	// 2 - templateFirstComponents (components where template element occurs first)
	// 3 - scriptFirstComponents (components where script element occurs first)
	classifyComponents();
	
	// check if there are multiple definitions and if so the one with least elements is considered the wrong one
	if(templateFirstComponents.Count > 0 && scriptFirstComponents.Count > 0) {
		if(templateFirstComponents.Count > scriptFirstComponents.Count)
		{
			orderErrorComponents.Add(scriptFirstComponents);
		}
		else
		{
			orderErrorComponents.Add(templateFirstComponents);
		}
	}
	result = orderErrorComponents;
}