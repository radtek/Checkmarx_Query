try{
	if(param.Length == 1 ){
		CxList securityConfigClass = All.InheritsFrom("WebSecurityConfigurerAdapter");
		if(cxXPath.GetXmlFiles("spring-security.xml", true).ToList().Count > 0 || securityConfigClass.Count > 0){ // If there is Spring-Security Configuration
			CxList customAttributes = Find_CustomAttribute();
			Dictionary <string,CxList> CAsByRoute = new Dictionary<string,CxList>();

			// Auxiliar variables
			List < string > controllerMappings = param[0] as List < string >;
			CxList controllerAnnotations = customAttributes.FindByShortNames(controllerMappings, false);
			CxList strings = Find_Strings();
			CxList paramsOfCA = strings.GetByAncs(controllerAnnotations);
			CxList memberAccesses = Find_MemberAccesses();
			CxList methodDecls = Find_MethodDeclaration();
			CxList methods = Find_Methods();
            
			foreach(CxList annot in paramsOfCA)
			{
				string route = annot.GetName();
				string finalRoute = Regex.Replace(route, @"\/\{\w+\}", "/**");
				CxList ca = annot.GetAncOfType(typeof(CustomAttribute));
				if(!(CAsByRoute.ContainsKey(finalRoute)))
				{
					CAsByRoute.Add(finalRoute, ca);
				}
				else{
					CxList existingList = CAsByRoute[finalRoute];
					existingList.Add(ca);
					CAsByRoute[finalRoute] = existingList;
				}
			}

			// Get routes that are Http Security controlled
			CxList methodsConfigClass = methodDecls.GetByAncs(securityConfigClass);
			CxList httpSecurity = All.GetParameters(methodsConfigClass).FindByType("HttpSecurity");
			CxList configureMethod = httpSecurity.GetAncOfType(typeof(MethodDecl)).FindByShortName("configure");
			CxList methodsInvokesInConfigure = methods.GetByAncs(configureMethod);
			CxList mvcAntMatchers = methodsInvokesInConfigure.FindByShortNames(new List < string > {"mvcMatchers","antMatchers"});

			// Mark Safe Controllers mvcAntMatchers 
			CxList safeControllers = All.NewCxList();

			// Collect Routes that are "protected with mvcMatchers/antMatchers hasAuthority, permitAll, hasRole
			CxList mvcAntMatchersHttpMeths = memberAccesses.GetParameters(mvcAntMatchers, 0);
			string [] memberAcesses = new string[]{"mvcMatchers.hasAuthority","mvcMatchers.permitAll","mvcMatchers.hasRole",
				"antMatchers.hasAuthority", "antMatchers.permitAll","antMatchers.hasRole"};
			CxList hasAuthAndPermitAll = methods.FindByMemberAccesses(memberAcesses, true);
			CxList targets = hasAuthAndPermitAll.GetTargetOfMembers();
			CxList safeRoutesHasAuthorityOrPermitAll = strings.GetParameters(targets, 0);

			// Mark every Controller associated to these safeRoutesHasAuthorityOrPermitAll as safe
			foreach(CxList safeRoute in safeRoutesHasAuthorityOrPermitAll){
				string sroute = safeRoute.GetName();
				if(CAsByRoute.ContainsKey(sroute)){
					CxList CAs = CAsByRoute[sroute];
					foreach (CxList ca in CAs)
					{
						safeControllers.Add(ca);
					}
				}
			}

			// Mark Controller regarding the HTTP Method associated to mvcMatchers or antMatchers methods as safe
			foreach(CxList mvcAntMatchersHttpMet in mvcAntMatchersHttpMeths){
				string httpM = mvcAntMatchersHttpMet.GetName();
				string routeParam = All.GetParameters(mvcAntMatchersHttpMet.GetAncOfType(typeof(MethodInvokeExpr)), 1).GetName();
				if(CAsByRoute.ContainsKey(routeParam)){ 
					CxList CAsAssociatedToRoute = CAsByRoute[routeParam];
					foreach (CxList ca in CAsAssociatedToRoute)
					{
						string httpMet = ca.GetName();
						if(httpMet.StartsWith(httpM, StringComparison.InvariantCultureIgnoreCase))
						{
							safeControllers.Add(ca);
						}
					}
				}
			}

			// Get safe controllers from spring-security.xml
			foreach(string route in CAsByRoute.Keys){
				string trimmedRoute = route.Trim('"');
				if (cxXPath.FindXmlAttributesByNameAndValue("spring-security.xml", 2, "pattern", trimmedRoute).Count > 0){
					CxList CAs = CAsByRoute[route];
					foreach(CxList ca in CAs){
						safeControllers.Add(ca);
					}
				}
			}

			// Find @EnableWebSecurity @EnableGlobalMethodSecurity and.. 
			CxList enableMethodSec = customAttributes.GetByAncs(securityConfigClass).FindByShortName("EnableGlobalMethodSecurity");
			CxList spels = All.GetByAncs(enableMethodSec).FindByType(typeof(BooleanLiteral)).FindByShortName("true").GetAssignee();

			List<string> safeAnnotations = new List<string>();   
			// if (prePostEnabled = true) @PreAuthorize @PostAuthorize 
			if (spels.FindByShortName("prePostEnabled").Count > 0 || cxXPath.FindXmlAttributesByNameAndValue("spring-security.xml", 2, "pre-post-annotations", "enabled").Count > 0){
				safeAnnotations.AddRange(new string[]{"PreAuthorize","PostAuthorize"});
			}
			// if (securedEnabled = true) @Secured
			if (spels.FindByShortName("securedEnabled").Count > 0 || cxXPath.FindXmlAttributesByNameAndValue("spring-security.xml", 2, "secured-annotations-annotations", "enabled").Count > 0){
				safeAnnotations.Add("Secured");
			}
                
			// if (jsr250Enabled = true) @RoleAllowed 
			if (spels.FindByShortName("jsr250Enabled").Count > 0 || cxXPath.FindXmlAttributesByNameAndValue("spring-security.xml", 2, "jsr250-annotations-annotations", "enabled").Count > 0){
				safeAnnotations.Add("RoleAllowed");
			}                    

			CxList methodsWithControllers = controllerAnnotations.GetAncOfType(typeof(MethodDecl));
            
			// Services and Repositories that are protected with "Spring Security Method Authorization" annotations
			CxList servicesAndRepositories = methods.GetByAncs(methodsWithControllers).FindByMemberAccess("*Service.*");
			servicesAndRepositories.Add(methods.GetByAncs(methodsWithControllers).FindByMemberAccess("*Repository.*"));
			CxList serviceCallers = methodDecls.FindDefinition(servicesAndRepositories);
			CxList annotations = All.GetByAncs(serviceCallers).FindByType(typeof(CustomAttribute)); 
			CxList safeAnnotationsInDOM = annotations.FindByShortNames(safeAnnotations);
			
			CxList methodAuthControllerAnnotations = customAttributes.GetByAncs(All.FindAllReferences(serviceCallers).GetAncOfType(typeof(MethodDecl))) * controllerAnnotations;
			controllerAnnotations.Add(methodAuthControllerAnnotations);

			CxList sanitizers = safeAnnotationsInDOM.GetAncOfType(typeof(MethodDecl));
			sanitizers.Add(customAttributes.GetByAncs(All.FindAllReferences(sanitizers).GetAncOfType(typeof(MethodDecl))) * controllerAnnotations);
			sanitizers.Add(safeControllers);
            
			result = controllerAnnotations - sanitizers;
		}
		else{
			result = All.NewCxList();
		}
	}
	else {
		result = All.NewCxList();
	}

}catch(Exception e){
	cxLog.WriteDebugMessage(e);
}