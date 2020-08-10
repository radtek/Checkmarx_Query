// Default values

int saltLength = 16; // salt length in bytes
int hashLength = 16; // hash length in bytes
int parallelism = 1; // currently not supported by Spring Security
int memory = 1024;   // memory costs
int iterations = 3; // iterations


if (param.Length == 5)
{
	saltLength = (int)param[0];
	hashLength = (int)param[1];
	parallelism = (int) param[2];
	memory = (int) param[3];
	iterations = (int) param[4];
}


CxList objectCreate = Find_Object_Create();
CxList instancesObjects = objectCreate.FindByShortName("Argon2PasswordEncoder");

IAbstractValue range1 = new IntegerIntervalAbstractValue(0, saltLength);
IAbstractValue range2 = new IntegerIntervalAbstractValue(0, hashLength);
IAbstractValue range3 = new IntegerIntervalAbstractValue(0, parallelism);
IAbstractValue range4 = new IntegerIntervalAbstractValue(0, memory);
IAbstractValue range5 = new IntegerIntervalAbstractValue(0, iterations);

CxList inSecureValues = All.NewCxList();

foreach(CxList curObj in instancesObjects)
{	
	CxList allParams = All.GetParameters(curObj).FindByType(typeof(Param));
	if(allParams.Count == 0)
		continue;
	
	CxList param1 = All.GetParameters(curObj, 0);
	CxList param2 = All.GetParameters(curObj, 1);
	CxList param3 = All.GetParameters(curObj, 2);
	CxList param4 = All.GetParameters(curObj, 3);
	CxList param5 = All.GetParameters(curObj, 4);
	
	CxList checkValue = All.NewCxList();
	
	CxList rangeValue1 = param1.FindByAbstractValue(absValue => absValue.IncludedIn(range1));
	CxList rangeValue2 = param2.FindByAbstractValue(absValue => absValue.IncludedIn(range2));	
	CxList rangeValue3 = param3.FindByAbstractValue(absValue => absValue.IncludedIn(range3));	
	CxList rangeValue4 = param4.FindByAbstractValue(absValue => absValue.IncludedIn(range4));	
	CxList rangeValue5 = param5.FindByAbstractValue(absValue => absValue.IncludedIn(range5));
	
	checkValue.Add(rangeValue1);
	checkValue.Add(rangeValue2);
	checkValue.Add(rangeValue3);
	checkValue.Add(rangeValue4);
	checkValue.Add(rangeValue5);
	
	if(checkValue.Count > 0)
	{			
		inSecureValues.Add(curObj);	
	}			
	else
	{
		try
		{			
			IntegerLiteral param1_Literal = All.FindDefinition(param1).GetAssigner()
				.TryGetCSharpGraph<IntegerLiteral>();
			
			IntegerLiteral param2_Literal = All.FindDefinition(param2).GetAssigner()
				.TryGetCSharpGraph<IntegerLiteral>();
			
			IntegerLiteral param3_Literal = All.FindDefinition(param3).GetAssigner()
				.TryGetCSharpGraph<IntegerLiteral>();
			
			IntegerLiteral param4_Literal = All.FindDefinition(param4).GetAssigner()
				.TryGetCSharpGraph<IntegerLiteral>();
			
			IntegerLiteral param5_Literal = All.FindDefinition(param5).GetAssigner()
				.TryGetCSharpGraph<IntegerLiteral>();		

			
			if((param1_Literal != null && param1_Literal.Value < saltLength)
				|| (param2_Literal != null && param2_Literal.Value < hashLength)
				|| (param3_Literal != null && param3_Literal.Value < parallelism)
				|| (param4_Literal != null && param4_Literal.Value < memory)
				|| (param5_Literal != null && param5_Literal.Value < iterations)
				
				)
			{
				inSecureValues.Add(curObj);	
			}
		}
		catch(Exception) {}
	}			
}

result = inSecureValues;