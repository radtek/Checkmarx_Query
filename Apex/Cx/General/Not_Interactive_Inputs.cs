CxList list = All.FindByShortName("fields", false)
	+ All.FindByShortName("getChildRelationships", false)
	+ All.FindByShortName("getChildSObject", false)
	+ All.FindByShortName("getDescribe", false)
	+ All.FindByShortName("getField", false)
	+ All.FindByShortName("getKeyPrefix", false)
	+ All.FindByShortName("getLabel", false)
	+ All.FindByShortName("getLabelPlural", false)
	+ All.FindByShortName("getLocalName", false)
	+ All.FindByShortName("getName", false)
	+ All.FindByShortName("getRecordTypeId", false)
	+ All.FindByShortName("getRecordTypeInfos", false)
	+ All.FindByShortName("getRecordTypeInfosByID", false)
	+ All.FindByShortName("getRecordTypeInfosByName", false)
	+ All.FindByShortName("getRelationshipName", false)
	+ All.FindByShortName("getSObjectType", false)
	+ All.FindByShortName("isAccessible", false)
	+ All.FindByShortName("isAvailable", false)
	+ All.FindByShortName("isCascadeDelete", false)
	+ All.FindByShortName("isCreateable", false)
	+ All.FindByShortName("isCustom", false)
	+ All.FindByShortName("isCustomSetting", false)
	+ All.FindByShortName("isDefaultRecordTypeMapping", false)
	+ All.FindByShortName("isDeletable", false)
	+ All.FindByShortName("isDeprecatedAndHidden", false)
	+ All.FindByShortName("isMergeable", false)
	+ All.FindByShortName("isQueryable", false)
	+ All.FindByShortName("isSearchable", false)
	+ All.FindByShortName("isUndeletable", false)
	+ All.FindByShortName("isUpdateable", false);

result = list + list.GetTargetOfMembers();