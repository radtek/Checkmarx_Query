if(cxScan.IsFrameworkActive("Angular")) {
	CxList imports = Find_Import();

	CxList relevant = Find_UnknownReference();
	relevant.Add(Find_FieldDecls());
	relevant.Add(Find_MemberAccesses());
	relevant.Add(Find_Methods());
	relevant.Add(Find_ObjectCreations());
	relevant.Add(Find_TypeRef());

	Func<string, string, CxList> findDeprecatedImport = (deprecatedSymbol, deprecatedPackage) =>
	{
		return imports.FilterByDomProperty<Import>(i => 
			i.Symbols != null && i.Symbols.Contains(deprecatedSymbol) && i.ImportedFilename.StartsWith(deprecatedPackage)
		);
	};

	Func<string, string, CxList> findUsageOfDeprecatedApis = (deprecatedSymbol, deprecatedPackage) =>
	{
		CxList foundDeprecatedImport = findDeprecatedImport(deprecatedSymbol, deprecatedPackage);
		return (relevant - foundDeprecatedImport).FindByShortName(deprecatedSymbol).FindByFiles(foundDeprecatedImport);
	};

	Func<string, string, string, CxList> findDeprecatedMemberOfApi = (deprecatedMember, deprecatedSymbol, deprecatedPackage) =>
	{
		CxList foundDeprecatedApis = findUsageOfDeprecatedApis(deprecatedSymbol, deprecatedPackage);
		return foundDeprecatedApis.GetMembersOfTarget().FindByShortName(deprecatedMember);
	};

	Func<string, string, string, CxList> findDeprecatedFieldOfApi = (deprecatedField, deprecatedSymbol, deprecatedPackage) =>
	{
		CxList foundDeprecatedImport = findDeprecatedImport(deprecatedSymbol, deprecatedPackage);
		return (relevant - foundDeprecatedImport).FindByShortName(deprecatedField).FindByFiles(foundDeprecatedImport);
	};

	Func<string, CxList> findDeprecatedPackage = (deprecatedPackage) =>
	{
		HashSet<string> importedSymbols = new HashSet<string>();
		CxList deprecatedPackages = imports.FilterByDomProperty<Import>(i => i.ImportedFilename.StartsWith(deprecatedPackage));
		foreach(CxList dp in deprecatedPackages)
		{
			Import i = dp.TryGetCSharpGraph<Import>();
			if (i != null && i.Symbols != null)
			{
				importedSymbols.UnionWith(i.Symbols);
			}
		}
		CxList deprecatedSymbols = All.NewCxList();
		foreach(string impSymbol in importedSymbols)
		{
			deprecatedSymbols.Add(findUsageOfDeprecatedApis(impSymbol, deprecatedPackage));
		}
		return deprecatedSymbols;
	};
	
	CxList deprecatedApis = All.NewCxList();

	// @angular/common
	deprecatedApis.Add(findUsageOfDeprecatedApis("DeprecatedI18NPipesModule", "@angular/common"));
	deprecatedApis.Add(findUsageOfDeprecatedApis("DeprecatedCurrencyPipe", "@angular/common"));
	deprecatedApis.Add(findUsageOfDeprecatedApis("DeprecatedDatePipe", "@angular/common"));
	deprecatedApis.Add(findUsageOfDeprecatedApis("DeprecatedDecimalPipe", "@angular/common"));
	deprecatedApis.Add(findUsageOfDeprecatedApis("DeprecatedPercentPipe", "@angular/common"));

	// @angular/core
	deprecatedApis.Add(findUsageOfDeprecatedApis("CollectionChangeRecord", "@angular/core"));
	deprecatedApis.Add(findUsageOfDeprecatedApis("DefaultIterableDiffer", "@angular/core"));
	deprecatedApis.Add(findUsageOfDeprecatedApis("ReflectiveInjector", "@angular/core"));
	deprecatedApis.Add(findUsageOfDeprecatedApis("ReflectiveKey", "@angular/core"));
	deprecatedApis.Add(findUsageOfDeprecatedApis("RenderComponentType", "@angular/core"));
	deprecatedApis.Add(findUsageOfDeprecatedApis("Renderer", "@angular/core"));
	deprecatedApis.Add(findUsageOfDeprecatedApis("RootRenderer", "@angular/core"));
	deprecatedApis.Add(findDeprecatedMemberOfApi("Native", "ViewEncapsulation", "@angular/core"));
	deprecatedApis.Add(findUsageOfDeprecatedApis("state", "@angular/core"));
	deprecatedApis.Add(findUsageOfDeprecatedApis("style", "@angular/core"));
	deprecatedApis.Add(findUsageOfDeprecatedApis("animate", "@angular/core"));
	deprecatedApis.Add(findUsageOfDeprecatedApis("AnimationEntryMetadata", "@angular/core"));
	deprecatedApis.Add(findUsageOfDeprecatedApis("AnimationPlayer", "@angular/core"));
	deprecatedApis.Add(findUsageOfDeprecatedApis("AnimationTransitionEvent", "@angular/core"));
	deprecatedApis.Add(findUsageOfDeprecatedApis("AUTO_STYLE", "@angular/core"));

	// @angular/forms
	deprecatedApis.Add(findUsageOfDeprecatedApis("NgFormSelectorWarning", "@angular/forms"));

	// @angular/router
	deprecatedApis.Add(findDeprecatedFieldOfApi("preserveQueryParams", "NavigationExtras", "@angular/router"));

	// @angular/platform-webworker
	deprecatedApis.Add(findUsageOfDeprecatedApis("WorkerAppModule", "@angular/platform-webworker"));

	// @angular/platform-webworker-dynamic
	deprecatedApis.Add(findUsageOfDeprecatedApis("platformWorkerAppDynamic", "@angular/platform-webworker-dynamic"));

	// @angular/upgrade
	deprecatedApis.Add(findUsageOfDeprecatedApis("UpgradeAdapter", "@angular/upgrade"));

	// @angular/upgrade/static
	deprecatedApis.Add(findUsageOfDeprecatedApis("getAngularLib", "@angular/upgrade/static"));
	deprecatedApis.Add(findUsageOfDeprecatedApis("setAngularLib", "@angular/upgrade/static"));

	// @angular/http
	deprecatedApis.Add(findDeprecatedPackage("@angular/http"));

	// @angular/http/testing
	deprecatedApis.Add(findDeprecatedPackage("@angular/http/testing"));

	// @angular/platform-browser
	deprecatedApis.Add(findUsageOfDeprecatedApis("DOCUMENT", "@angular/platform-browser"));

	// @angular/core/testing
	deprecatedApis.Add(findDeprecatedMemberOfApi("deprecatedOverrideProvider", "TestBed", "@angular/core/testing"));
	deprecatedApis.Add(findDeprecatedMemberOfApi("deprecatedOverrideProvider", "TestBedStatic", "@angular/core/testing"));

	result = deprecatedApis;
}