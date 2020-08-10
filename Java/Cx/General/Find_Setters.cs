CxList methods = Find_Methods();

CxList setters = methods.FindByMemberAccesses(new string[] {
	"System.setProperty",
	"System.setProperties",
	"Properties.setProperty",
	"Properties.setProperties",
	"Connection.setCatalog"
});

if (Find_Citrus_Framework().Count > 0)
{
	CxList calls = Find_MemberAccess();
	calls.Add(methods);
	// Citrus Webx: exclude Group.setProperties()
	setters -= calls.FindByMemberAccess("Group.setProperties");
}

result = setters;