/*
cpuCost - cpu cost of the algorithm (as defined in scrypt this is N). must be power of 2 greater than 1.
	Default is currently 16,348 or 2^14)

memoryCost - memory cost of the algorithm (as defined in scrypt this is r)
	Default is currently 8.

parallelization - the parallelization of the algorithm (as defined in scrypt this is p) 
	Default is currently 1. Note that the implementation does not currently take advantage of parallelization.

keyLength - key length for the algorithm (as defined in scrypt this is dkLen). 
	The default is currently 32.

saltLength - salt length (as defined in scrypt this is the length of S). 
	The default is currently 64.

*/

int cpuCost = (int) 2^14; // factor to increase CPU costs
int memoryCost = 8;      // increases memory usage
int parallelization = 1; // currently not supported by Spring Security
int keyLength = 32;      // key length in bytes
int saltLength = 64;     // salt length in bytes


if (param.Length == 5)
{
	cpuCost = (int) param[0];
	memoryCost = (int) param[1];
	parallelization = (int) param[2];
	keyLength = (int) param[3];
	saltLength = (int) param[4];
}

CxList objectCreate = Find_Object_Create();
CxList instancesObjects = objectCreate.FindByShortName("SCryptPasswordEncoder");

IAbstractValue range1 = new IntegerIntervalAbstractValue(0, cpuCost);
IAbstractValue range2 = new IntegerIntervalAbstractValue(0, memoryCost);
IAbstractValue range3 = new IntegerIntervalAbstractValue(0, parallelization);
IAbstractValue range4 = new IntegerIntervalAbstractValue(0, keyLength);
IAbstractValue range5 = new IntegerIntervalAbstractValue(0, saltLength);

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

			
			if((param1_Literal != null && param1_Literal.Value < cpuCost)
				|| (param2_Literal != null && param2_Literal.Value < memoryCost)
				|| (param3_Literal != null && param3_Literal.Value < parallelization)
				|| (param4_Literal != null && param4_Literal.Value < keyLength)
				|| (param5_Literal != null && param5_Literal.Value < saltLength)
				
				)
			{
				inSecureValues.Add(curObj);	
			}
		}
		catch(Exception) {}
	}			
}

result = inSecureValues;