
var butFlg1 = "false"; //button1 clicked flag
var startFlg = "stop";//start stop flag

function autoClick(){
	if ($("#mo2g") && butFlg1=="false") {
	
//		chrome.extension.sendMessage({cmd: "Call background"}, function(response) {  console.log(response); } );//send message to Background
		chrome.runtime.sendMessage({greeting: 'check Status'}, function(response) { //get Background start status
			startFlg = response;
		    console.log('recive' + response);
		});
	
		if(startFlg == "start"){
		     $("#mo2g").click();
	     	butFlg1 = "true";
	     	
	     	setTimeout(function(){
						$("#reflush").click();
						}, 5000);
			}
     }
}

setInterval(autoClick,2000);

