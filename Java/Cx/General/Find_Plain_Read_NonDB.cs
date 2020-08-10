CxList streams = All.NewCxList();
CxList methods = base.Find_Methods();
streams.Add(methods.FindByMemberAccess("AudioInputStream.read*"));
streams.Add(methods.FindByMemberAccess("BufferedInputStream.read*"));
streams.Add(methods.FindByMemberAccess("BufferedReader.read*"));
streams.Add(methods.FindByMemberAccess("ByteArrayInputStream.read*"));
streams.Add(methods.FindByMemberAccess("CharArrayReader.read*"));
streams.Add(methods.FindByMemberAccess("DataInputStream.read*"));
streams.Add(methods.FindByMemberAccess("FilterInputStream.read*"));
streams.Add(methods.FindByMemberAccess("InputStream.read*"));
streams.Add(methods.FindByMemberAccess("InputStreamReader.read*"));
streams.Add(methods.FindByMemberAccess("LineNumberReader.read*"));
streams.Add(methods.FindByMemberAccess("ObjectInputStream.read*"));
streams.Add(methods.FindByMemberAccess("PipedInputStream.read*"));
streams.Add(methods.FindByMemberAccess("PipedReader.read*"));
streams.Add(methods.FindByMemberAccess("SequenceInputStream.read*"));
streams.Add(methods.FindByMemberAccess("ServletInputStream.read*"));
streams.Add(methods.FindByMemberAccess("StringBufferInputStream.read*"));
streams.Add(methods.FindByMemberAccess("StringReader.read*"));
streams.Add(Find_FileStreams());

////////////////////////////-
//deal differently with Call.invoke (from org.apache.axis.client.Call)
//since the first argument may be an array of objects without relevance to reading purposes.
CxList callInvoke = All.FindByMemberAccess("Call.invoke");
CxList callInvokeParams = All.GetParameters(callInvoke, 0);
CxList findArrayParams = callInvokeParams.FindByType(typeof(ArrayCreateExpr)); 
findArrayParams.Add(callInvokeParams.InfluencedBy(Find_ArrayCreateExpr()));
callInvokeParams -= findArrayParams;
////////////////////////////-

CxList parameters = All.GetParameters(streams, 0);
parameters.Add(callInvokeParams);
CxList findIntegers = Find_Integers();
parameters -= findIntegers;
parameters -= findIntegers.GetFathers();
parameters -= parameters.FindByType(typeof(Param));

// remove the methods that have parameters, because we are using the parameters
// (if there was no parameter for the method, then it stays)
//streams -= streams.FindByParameters(parameters); 

streams.Add(callInvoke); 
streams.Add(// add methods that their parameters are not inputs
	All.FindByMemberAccess("System.getProperty"));
streams.Add(All.FindByMemberAccess("System.getProperties"));
streams.Add(All.FindByMemberAccess("SAXReader.read*"));

if(!All.isWebApplication)
{
	streams.Add(All.FindByName("System.getenv"));
	streams.Add(All.FindByMemberAccess("Properties.getProperty"));
}

result = streams;
result.Add(parameters);

// Addition of reads from Cloud services
result.Add(Find_Cloud_Storage_In());


result -= Find_Plain_Interactive_Inputs();