
var NowStatus;


//listen from content-script
chrome.runtime.onMessage.addListener(function(request, sender, sendResponse)
{
    sendResponse(NowStatus);
});
