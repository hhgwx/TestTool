cc.game.onStart = function(){
    if(!cc.sys.isNative && document.getElementById("cocosLoading")) //If referenced loading.js, please remove it
        document.body.removeChild(document.getElementById("cocosLoading"));

    var designSize = cc.size(720, 1280);
    var screenSize = cc.view.getFrameSize();

//    if(!cc.sys.isNative && screenSize.height < 800){
//        designSize = cc.size(320, 480);
//			cc.loader.resPath = "res/Normal";
		 cc.loader.resPath = "res";
//    }else{
//        cc.loader.resPath = "res/HD";
//    }
    
//    document.getElementById("gameCanvas").width = screenSize.width;
//    document.getElementById("gameCanvas").height = screenSize.height;
    cc.view.setDesignResolutionSize(designSize.width, designSize.height, cc.ResolutionPolicy.SHOW_ALL);

    //load resources
    cc.LoaderScene.preload(g_resources, function () {
        cc.director.runScene(new MyScene());
    }, this);
};
cc.game.run();