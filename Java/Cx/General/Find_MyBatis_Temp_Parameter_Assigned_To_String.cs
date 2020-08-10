if(cxScan.IsFrameworkActive("MyBatis"))
{
	CxList _parameter = Find_MyBatis_Temp_Variables().FindByShortName("_parameter");
	CxList _parameterRefs = Find_UnknownReference().FindAllReferences(_parameter);
	
	CxList assignedToStr = Find_String_Types().GetAssigner();
	
	CxList _parameterAssignedToStr = assignedToStr;
	_parameterAssignedToStr.Add(assignedToStr.GetLeftmostTarget()); 

	result = _parameterAssignedToStr * _parameterRefs;
}