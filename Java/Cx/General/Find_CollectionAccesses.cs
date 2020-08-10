CxList methods = base.Find_Methods();
CxList collectionAccess = All.NewCxList();

//.get
CxList getMethod = methods.FindByMemberAccess(".get");
string[] getMethodStr = new string[]{
	"Attributes.get",
	"Collection.get",
	"List.get",
	"Map.get",
	"Table.get",
	"Vector.get",
	"Properties.get"
};
collectionAccess.Add(getMethod.FindByMemberAccesses(getMethodStr));

//.remove
CxList removeMethod = methods.FindByMemberAccess(".remove");
string[] removeMethodStr = new string[]{
	"Attributes.remove",
	"Collection.remove",
	"List.remove",
	"Map.remove",
	"Table.remove",
	"Vector.remove",
	"Properties.remove"
};
collectionAccess.Add(removeMethod.FindByMemberAccesses(removeMethodStr));

//.elementAt
CxList elementAtMethod = methods.FindByMemberAccess(".elementAt");
string[] elementAtMethodStr = new string[]{
	"Collection.elementAt",
	"Vector.elementAt"
};
collectionAccess.Add(elementAtMethod.FindByMemberAccesses(elementAtMethodStr));
	
result.Add(All.GetParameters(collectionAccess));

//.put
CxList safePutParams = All.NewCxList();
CxList putMethod = methods.FindByMemberAccess(".put");
string[] putMethodStr = new string[]{
	"Attributes.put",
	"Collection.put",
	"List.put",
	"Map.put",
	"Table.put",
	"Vector.put",
	"Properties.put"
};
safePutParams.Add(putMethod.FindByMemberAccesses(putMethodStr));

result.Add(All.GetParameters(safePutParams, 0));