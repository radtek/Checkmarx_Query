CxList customAttributes = Find_CustomAttribute();
CxList objectCreations = Find_Object_Create();
CxList methodDecls = Find_MethodDeclaration();

// support for springfox (Swagger/OpenApi 1.2, 2.0 -- aka swagger-springmvc)
// http://springfox.github.io/springfox/docs/current/

// When springfox is used, the documentation is automatic and global on the presence of
// 1. a swagger configuration class where annotations @EnableSwagger, @EnableSwagger2 
//	or @EnableSwaggerWebMvc are added to the config class 
// 2. a Docket object is created in a method in that class (with DocumentationType.SWAGGER/SWAGGER2 as parameter)

var springFoxAnnotationNames = new List<string>() {"EnableSwagger", "EnableSwagger2", "EnableSwagger2WebMvc"};
CxList springFoxConfigAnnotations = customAttributes.FindByShortNames(springFoxAnnotationNames);
CxList springFoxAnnotatedClasses = springFoxConfigAnnotations.GetAncOfType(typeof(ClassDecl));
CxList methodsOfAnnotatedClasses = methodDecls.GetByAncs(springFoxAnnotatedClasses);

CxList springFoxDocketObjects = objectCreations.FindByShortName("Docket").GetByAncs(methodsOfAnnotatedClasses);
CxList springFoxDocumentationTypes = All.FindByMemberAccesses(new string[] {"DocumentationType.SWAGGER_12", "DocumentationType.SWAGGER_2", "DocumentationTye.SPRING_WEB"}, false);
CxList relevantSpringFoxDocketObjects = springFoxDocketObjects.FindByParameters(springFoxDocumentationTypes);

// If there are no such annotations and object creations, then search for other api documentation approaches 
// that are not global or automatic

if(relevantSpringFoxDocketObjects.Count() == 0) 
{	
	result.Add(Find_Undocumented_Spring_API_Methods());
	result.Add(Find_Manually_Undocumented_Swagger_API());	
}