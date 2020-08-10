if(React_Find_References().Count > 0) {
	result.Add(React_DeprecatedMethods());
	result.Add(React_ChildrenOfVoidElement());
	result.Add(React_UseOfPropsOfRefs());
}