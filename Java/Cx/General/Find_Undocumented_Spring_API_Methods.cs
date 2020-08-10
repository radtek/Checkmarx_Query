// Support for official Swagger Annotations Api - io.swagger - (Swagger JAXRS 1.5, 2.x)
// https://github.com/swagger-api/swagger-core/wiki/Swagger-2.X---Annotations
// For these cases, the annotations are associated with the paths definitions used by other frameworks/libraries 
// (e.g., spring, spark, jax-rs, ...)

CxList methodDecls = Find_MethodDeclaration();
CxList customAttributes = Find_CustomAttribute();

CxList endpointAnnotations = Find_Frameworks_Inputs_Annotations();
CxList endpointHandlerMethods = endpointAnnotations.GetFathers() * methodDecls;
	
var apiDocAnnotationNames = new List<string>() {
		//Swagger 2
		"Operation", "Parameter", "RequestBody", "ResponseBody", "ApiResponse", 
		"Tag", "Callback", "Link", "Hidden", "ExternalDocumentation",
		//Swagger 1.x
		"Api", "ApiImplicitParam", "ApiImplicitParams", "ApiModel",
		"ApiModelProperty", "ApiOperation", "ApiParam", "ApiResponse", "ApiResponses"
		};
	
CxList apiDocAnnotations = customAttributes.FindByShortNames(apiDocAnnotationNames);
CxList documentedEndpointHandlers = endpointHandlerMethods * apiDocAnnotations.GetAncOfType(typeof(MethodDecl));
result = endpointHandlerMethods - documentedEndpointHandlers;