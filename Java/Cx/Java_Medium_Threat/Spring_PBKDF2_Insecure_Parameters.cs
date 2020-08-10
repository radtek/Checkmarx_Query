/*
This query will point the Pbkdf2PasswordEncoder instance that:

Use the default recommended parameters, otherwise follow the parameter selection recommendations defined by the authors of the algorithms.

PBKDF2 is recommended only for legacy uses, consider updating to a more secure solution.

When using PBKDF2 the following values for its parameters are recommended:

Iteration Count
	- 10,000

Length of salt used
	- At least 32bits

Constructor Summary:

Pbkdf2PasswordEncoder()
Pbkdf2PasswordEncoder(CharSequence secret)
Pbkdf2PasswordEncoder(CharSequence secret, int iterations, int hashWidth)

*/

// Query Code
CxList objectCreate = Find_Object_Create();

CxList deprecatedInstances = objectCreate.FindByShortName("Pbkdf2PasswordEncoder");

IAbstractValue valueForIterations = new IntegerIntervalAbstractValue(0, 9999);
IAbstractValue valueForHashWidth = new IntegerIntervalAbstractValue(0, 31);

CxList inSecureValues = All.NewCxList();

foreach(CxList curObj in deprecatedInstances)
{	
	CxList iterationsParam = All.GetParameters(curObj, 1);
	CxList hashWidthParam = All.GetParameters(curObj, 2);		

	if(iterationsParam.Count == 0 && hashWidthParam.Count == 0)
		continue;
	
	CxList param1 = iterationsParam.FindByAbstractValue(absValue => absValue.IncludedIn(valueForIterations));
	CxList param2 = hashWidthParam.FindByAbstractValue(absValue => absValue.IncludedIn(valueForHashWidth));	
	
	if((param1.Count == 1 || param2.Count == 1))
	{			
		inSecureValues.Add(curObj);	
	}
	else
	{			
		try
		{			
			IntegerLiteral iterationsValue = All.FindDefinition(iterationsParam).GetAssigner().TryGetCSharpGraph<IntegerLiteral>();
			IntegerLiteral hashWidthValue = All.FindDefinition(hashWidthParam).GetAssigner().TryGetCSharpGraph<IntegerLiteral>();			
			
			if((iterationsValue != null && iterationsValue.Value <= 9999)
				||
				(hashWidthValue != null && hashWidthValue.Value <= 31))
			{
				inSecureValues.Add(curObj);	
			}
		}
		catch(Exception) {}
			
	}			
}

result = inSecureValues;