CxList methods = Find_Methods();

CxList nextRandom = methods.FindByMemberAccess("*Random.next*");
nextRandom.Add(methods.FindByMemberAccess("ThreadLocalRandom.current").GetMembersOfTarget().FindByShortName("next*"));

nextRandom = nextRandom.FindByShortNames(new List<string> {
		"next",
		"nextBoolean",
		"nextBytes",
		"nextDouble",
		"nextFloat",
		"nextGaussian",
		"nextInt",
		"nextLong"});

CxList mathRandom = methods.FindByMemberAccess("Math.random");

CxList secureRandom = methods.FindByMemberAccess("SecureRandom.*");
secureRandom.Add(Find_Object_Create().FindByShortName("SecureRandom"));
secureRandom.Add(nextRandom.DataInfluencedBy(secureRandom));

result = nextRandom - secureRandom;
result.Add(mathRandom);