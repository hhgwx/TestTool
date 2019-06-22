var butFlg1_B = "false"; //button1 clicked flag
var startFlg_B = "stop";//start stop flag
var BCanStart_B = "false";

function startSub() {
	chrome.runtime.sendMessage({greeting: "check Status"}, function(response) { //get Background start status
		startFlg_B = response;
	//	console.log('recive' + response);
	});
	
	chrome.runtime.sendMessage({greeting: "check b Start Status"}, function(response) { //get Background B can start status
		BCanStart_B = response;
	});
	
	if (startFlg_B == "start" && butFlg1_B=="false" && BCanStart_B=="true") {
		butFlg1_B = "true";
		setTimeout(function(){
			if($("button.yt-uix-button&.yt-uix-button-size-default&.yt-uix-button-subscribe-branded&.yt-uix-button-has-icon&.no-icon-markup&.yt-uix-subscription-button&.yt-can-buffer&.yt-uix-servicelink").length > 0){
				$("button.yt-uix-button&.yt-uix-button-size-default&.yt-uix-button-subscribe-branded&.yt-uix-button-has-icon&.no-icon-markup&.yt-uix-subscription-button&.yt-can-buffer&.yt-uix-servicelink")[0].click();
			}
		}, 10000);

		setTimeout(function(){
			if($("button.yt-uix-button&.yt-uix-button-size-default&.yt-uix-button-opacity&.yt-uix-button-has-icon&.no-icon-markup&.like-button-renderer-like-button&.like-button-renderer-like-button-unclicked&.&.yt-uix-post-anchor&.yt-uix-tooltip").length > 0){
				$("button.yt-uix-button&.yt-uix-button-size-default&.yt-uix-button-opacity&.yt-uix-button-has-icon&.no-icon-markup&.like-button-renderer-like-button&.like-button-renderer-like-button-unclicked&.&.yt-uix-post-anchor&.yt-uix-tooltip")[0].click();
			}
		}, 11000);

		setTimeout(function(){
			if($("button.yt-uix-button&.yt-uix-button-size-default&.yt-uix-button-opacity&.yt-uix-button-empty&.yt-uix-button-has-icon&.no-icon-markup&.like-button-renderer-like-button&.like-button-renderer-like-button-unclicked&.&.yt-uix-post-anchor&.yt-uix-tooltip").length > 0){
				$("button.yt-uix-button&.yt-uix-button-size-default&.yt-uix-button-opacity&.yt-uix-button-empty&.yt-uix-button-has-icon&.no-icon-markup&.like-button-renderer-like-button&.like-button-renderer-like-button-unclicked&.&.yt-uix-post-anchor&.yt-uix-tooltip")[0].click();
			}
		}, 12000);

		setTimeout(function(){
			window.close();
		}, 14000);
	}
}

setInterval(startSub,2000);