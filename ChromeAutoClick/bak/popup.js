document.addEventListener('DOMContentLoaded', function () {
    var dom = document.getElementById('Start');

    var bg = chrome.extension.getBackgroundPage();
    

    if(bg.NowStatus!=null && bg.NowStatus != ""){
    	if (bg.NowStatus == "start") {
    		dom.value = "stop";
    	} else {
    		dom.value = "start";
    	}
    }

    dom.addEventListener('click', function () {
       if (dom.value == 'start') {
         dom.value = "stop";
         bg.NowStatus = "start";
       } else {
         dom.value = "start";
         bg.NowStatus = "stop";
       }
    });
});