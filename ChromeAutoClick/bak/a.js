var butFlg1_A = "false"; //button1 clicked flag
var startFlg_A = "stop";//start stop flag

function autoClick(){
	if(butFlg1_A=="false" && $("i.fa&.fa-fw&.fa-youtube-play").length > 0){
		chrome.runtime.sendMessage({greeting: 'check Status'}, function(response) { //get Background start status
			startFlg_A = response;
		//	console.log('recive' + response);
		});
	
		if(startFlg_A == "start"){
			$("i.fa&.fa-fw&.fa-youtube-play")[0].click();
			butFlg1_A = "true";
			
			chrome.runtime.sendMessage({greeting: 'set b Start Status'}, function(response) { //set b can start status
			});
			
			setTimeout(function(){
			 $("button#btnSubVerify")[0].click();
			}, 25000);
		}
	}
	
	//最後のエンド
	if(startFlg_A == "start"){
		setTimeout(function(){
		 $("button#close&orange&button&planButton&closeWithDialog")[0].click();
		}, 6000);
	}
}

setInterval(autoClick,2000);



