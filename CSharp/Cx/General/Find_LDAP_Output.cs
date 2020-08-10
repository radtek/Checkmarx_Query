List<string> LDAPConstructors = new List<string> {"*DirectoryEntry","LdapConnection"};
CxList de = All.FindByShortNames(LDAPConstructors).FindByType(typeof(ObjectCreateExpr));
CxList ds = All.FindByType("*DirectorySearcher");
CxList ldap = All.GetParameters(de, 0);
ldap.Add(ds);
ldap -= Find_Methods().FindByShortName("Dispose").GetTargetOfMembers();


// Handles special case :
//    new LdapConnection(args[0])
// where the IndexerRef was returned instead of UnknownReference
ldap.Add(All.FindByFathers(ldap.FindByType(typeof(Param))));

result = ldap;