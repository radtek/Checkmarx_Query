/*
This will point the BCryptPasswordEncoder instance that:

Use the default recommended parameters, otherwise follow the parameter selection recommendations defined by the authors of the algorithms.

When using bcrypt the following values for its parameters are recommended:
	Overall Computation cost(2 ^ c):
		greater than 12

Constructor Summary:

BCryptPasswordEncoder() 
BCryptPasswordEncoder(int strength) 
BCryptPasswordEncoder(int strength, SecureRandom random) 

*/

// Query Code
CxList objectCreate = Find_Object_Create();

CxList instancesObjects = objectCreate.FindByShortName("BCryptPasswordEncoder");

IAbstractValue valueStrength = new IntegerIntervalAbstractValue(0, 12);

CxList inSecureValues = All.NewCxList();

foreach(CxList curObj in instancesObjects)
{	
	CxList strengthParam = All.GetParameters(curObj, 0);
	if(strengthParam.Count == 0)
		continue;
	
	
	CxList rangeValue = strengthParam.FindByAbstractValue(absValue => absValue.IncludedIn(valueStrength));	
	if(rangeValue.Count == 1)
	{			
		inSecureValues.Add(curObj);	
	}			
	else
	{	
		CxList anyAbstractValue = strengthParam.FindByAbstractValue(absValue => absValue is AnyAbstractValue);	
		
		if(anyAbstractValue == null)
			continue;

		try
		{			
			IntegerLiteral integerLiteral = All.FindDefinition(strengthParam).GetAssigner()
				.TryGetCSharpGraph<IntegerLiteral>();
			
			if(integerLiteral != null && integerLiteral.Value <= 12)
			{
				inSecureValues.Add(curObj);	
			}
		}
		catch(Exception) {}
	}			
}

result = inSecureValues;