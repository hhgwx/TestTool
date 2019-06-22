var NowStatus;
var BCanStart;

//listen from content-script
chrome.runtime.onMessage.addListener(function(request, sender, sendResponse)
{
	if (request.greeting== "check Status")
	{
		sendResponse(NowStatus);
	}
	if (request.greeting== "set b Start Status")
	{
		BCanStart="true";
		sendResponse(NowStatus);
	}
	if (request.greeting== "check b Start Status")
	{
		sendResponse(BCanStart);
	}
});