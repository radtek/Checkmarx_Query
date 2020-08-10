CxList Inputs = Find_Interactive_Inputs();
CxList methods = Find_Methods();
CxList parameters = Find_Params();
CxList sleep = methods.FindByName("*Thread.sleep");
CxList timeunitSleep = methods.FindByMemberAccess("SECONDS.sleep");
timeunitSleep.Add(methods.FindByMemberAccess("MINUTES.sleep"));
timeunitSleep.Add(methods.FindByMemberAccess("HOURS.sleep"));
timeunitSleep.Add(methods.FindByMemberAccess("DAYS.sleep"));
sleep.Add(timeunitSleep);

CxList tooltipDelay = Find_Jsp_Tags().GetMembersOfTarget().FindByMemberAccess("tooltipDelay.*"); 
tooltipDelay = methods.GetParameters(tooltipDelay);
CxList delay = All.NewCxList();
delay.Add(sleep);
delay.Add(tooltipDelay);

CxList scheduleExecutor = methods.FindByMemberAccess("ScheduledExecutorService.schedule");
CxList scheduleExecutorParams = All.GetParameters(scheduleExecutor, 1) - parameters;  

//Sleep sanitization
CxList sleepMethods = All.NewCxList();
sleepMethods.Add(delay);
CxList sleepParameters = All.GetParameters(sleepMethods) - parameters;
sleepParameters.Add(scheduleExecutorParams);

CxList integersAbstractValues = sleepParameters.FindByAbstractValue(abstractValue => abstractValue is IntegerIntervalAbstractValue);
IAbstractValue intervalOneToMax = new IntegerIntervalAbstractValue(1, Int32.MaxValue);
CxList validIntervals = integersAbstractValues.FindByAbstractValue(abstractValue => abstractValue.IncludedIn(intervalOneToMax));
CxList vulnerableSleepInvokes = sleepMethods.FindByParameters(sleepParameters - validIntervals); 

IAbstractValue negativeInterval = new IntegerIntervalAbstractValue(Int32.MinValue, 0); 

// 1- Sanitize the negative call of ScheduleExecutorService.schedule()
CxList integersAbstractValuesSchedParam = scheduleExecutorParams.FindByAbstractValue(abstractValue => abstractValue is IntegerIntervalAbstractValue);
CxList invalidIntervalSchedule = integersAbstractValuesSchedParam.FindByAbstractValue(abstractValue => abstractValue.IncludedIn(negativeInterval));
CxList sanitizedSchedules = scheduleExecutor.FindByParameters(invalidIntervalSchedule);

// 2- Sanitize the negative call of TimeUnit.sleep()
CxList timeUnitSleepParams = All.GetParameters(timeunitSleep, 0) - parameters;
CxList integersAbstractValuesTimeUnitSleepParams = timeUnitSleepParams.FindByAbstractValue(abstractValue => abstractValue is IntegerIntervalAbstractValue);
CxList invalidIntervalTimeUnitSleep = integersAbstractValuesTimeUnitSleepParams.FindByAbstractValue(abstractValue => abstractValue.IncludedIn(negativeInterval));
CxList sanitizedTimeUnitSleep = timeunitSleep.FindByParameters(invalidIntervalTimeUnitSleep);

CxList typeRefs = Find_TypeRef();
CxList catcheClauses = base.Find_Catch();
CxList catches = typeRefs.FindByShortName("IllegalArgumentException").FindByFathers(catcheClauses);

CxList sanitizers = Find_CollectionAccesses();
CxList getMethod = methods.FindByMemberAccess(".get");
sanitizers.Add(getMethod.FindByMemberAccess("SystemProperties.get"));
sanitizers.Add(catches);
sanitizers.Add(sanitizedSchedules);
sanitizers.Add(sanitizedTimeUnitSleep);
sanitizers.Add(Find_BinaryExpr().FindByName("=="));
sanitizers.Add(All.FindAllReferences(Find_FieldDecls().FindByFieldAttributes(Modifiers.Sealed)));

result = vulnerableSleepInvokes.InfluencedByAndNotSanitized(Inputs, sanitizers);