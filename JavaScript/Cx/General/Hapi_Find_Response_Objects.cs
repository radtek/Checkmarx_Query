/* This query returns all of the response objects generated by the request object */
CxList responseInstance = All.NewCxList();

CxList assignLefts = Find_Assign_Lefts();
CxList declarators = Find_Declarators();

CxList requestObject = Hapi_Find_Request_Objects();
CxList responseObject = requestObject.GetMembersOfTarget().FindByShortName("response");

//Case : reply()
CxList replyInterfaceInvokes = Hapi_Find_Reply_Interface_Invokes();

// Case: const response = reply(`You sent: ${body} to Hapi`)
responseObject.Add(replyInterfaceInvokes.GetFathers());

/* Case: for h param 

 handler: function (request, h) {
            const response = h.entity({ etag: 'abc' });
...
*/
CxList replyInterface = Hapi_Find_Reply_Interface();
responseObject.Add(replyInterface);

/* Case : 
	handler: (request, h) => {  
		const response1 = request.response;
    	const response = h.response('Hi, YLD!');

*/

// Case: const response = reply('success').hold();
responseObject.Add(replyInterfaceInvokes.GetMembersOfTarget().FindByShortName("hold"));

CxList leftAssigns = assignLefts.GetByAncs(responseObject.GetFathers());
leftAssigns.Add(declarators * responseInstance.GetFathers());

responseObject.Add(All.FindAllReferences(leftAssigns));

result = responseObject;