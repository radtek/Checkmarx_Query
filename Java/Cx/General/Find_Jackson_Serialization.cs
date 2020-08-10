if(Find_Pom_Config().FindByShortName("com.fasterxml.jackson.dataformat").Count > 0){

	CxList sanitizers = All.NewCxList();
	
	//Sensitive data
	CxList sensitive = Find_Personal_Info().FindByType(typeof(FieldDecl));
	
	if(sensitive.Count > 0){
		//@JsonIgnore
		sensitive -= Find_Jackson_JsonIgnore_Sanitizer();
		//@JsonIgnoreProperties
		sensitive -= Find_Jackson_JsonIgnoreProperties_Sanitizer();
		//@JsonFilter
		sensitive -= Find_Jackson_JsonFilter_Sanitizer();
		//@JsonIgnoreType
		sanitizers.Add(Find_Jackson_JsonIgnoreType_Sanitizer());
	}
	
	//Get all vars of sensitive data
	sensitive = All.FindAllReferences(sensitive);

	//Get output
	CxList outputs = All.FindByShortName("writeValueAsString");
	
	result = outputs.InfluencedByAndNotSanitized(sensitive, sanitizers);
}