/*
MISRA CPP RULE 6-6-2
------------------------------
This query searches for backward uses of the goto command

	The Example below shows code with vulnerability: 

			out:
			for (i=0;i< 10 ;i++ ){
				if (i == 6){
					goto out; 
				}
				...
			}


*/

//finds all declarations
CxList decl = Find_All_Declarators();
//finds goto
CxList allGoTos = All.FindByFathers(decl).FindByName("goto");
CxList labels = allGoTos.GetFathers().FindByType(typeof(Declarator));
CxList labelCalls = All.FindByType(typeof(LabeledStmt));

foreach (CxList cur in labels)
{        
	Declarator d = cur.TryGetCSharpGraph<Declarator>();
	LinePragma curLp = d.LinePragma;
	string curName = d.Name;
	
	foreach(CxList lab in labelCalls)
	{
		string labName = lab.TryGetCSharpGraph<LabeledStmt>().Label;
		LinePragma labLp = lab.GetFirstGraph().LinePragma;
		
		if(labName.Equals(curName) && labLp.Line < curLp.Line) 
		{
			result.Add(cur);
		}              
	}              
}