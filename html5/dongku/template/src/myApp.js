/////////////////////////////
// 2. add a menu item with "X" image, which is clicked to quit the program
//	you may modify it.
// ask director the window size
var size;


var MenuLayer = cc.Layer.extend({
	helloLabel:null,
	sprite:null,

	init:function () {

		//////////////////////////////
		// 1. super init first
		this._super();

		nowLayer = this;

		/////////////////////////////
		// 2. add a menu item with "X" image, which is clicked to quit the program
		//	you may modify it.
		// ask director the window size
		//		var size = cc.director.getWinSize();
		size = cc.director.getWinSize();

		// add a "close" icon to exit the progress. it's an autorelease object
		var closeItem = new cc.MenuItemImage(
			s_CloseNormal,
			s_CloseSelected,
			function () {
				gotoHome();
			},this);
		closeItem.setAnchorPoint(0.5, 0.5);

		var menu = new cc.Menu(closeItem);
		menu.setPosition(0, 0);
		this.addChild(menu, 1);
		closeItem.setPosition(size.width - 20, 20);

		/////////////////////////////
		// 3. add your codes below...
		this.helloLabel = new cc.LabelTTF("いい部屋", "Impact", 38);
		this.helloLabel.setPosition(size.width / 2, size.height - 40);
		this.addChild(this.helloLabel, 5);

		// add "Helloworld" splash screen"
		this.sprite = new cc.Sprite(s_HelloWorld);
		this.sprite.setAnchorPoint(0.5, 0.5);
		this.sprite.setPosition(size.width / 2, size.height / 2);
		this.sprite.setScale(size.height / this.sprite.getContentSize().height);
		this.addChild(this.sprite, 0);
	}
});

gotoHome=function () {
//	if (JSON.parse(localStorage.getItem('userNowStorage'))) {
//		reloadUser();
//	} else {
		user = JSON.parse(cc.loader._loadTxtSync("res/user/user_bak.json"));
//	}
	user.xueNow = user.property[xueNo];
	user.moNow = user.property[moNo];
	saveUser();

	var loadResource = resourceArrSet_user();

	var myHomeScene = new MyHomeScene();
	var tranScene = cc.TransitionMoveInT.create(0.5, myHomeScene);
	cc.LoaderScene.preload(loadResource, function () {
		cc.director.runScene(tranScene);
	}, this);
}


var MyScene = cc.Scene.extend({
	onEnter:function () {
		this._super();
		var layer = new MenuLayer();
		this.addChild(layer);
		layer.init();
	}
});

