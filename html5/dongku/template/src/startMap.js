var userDirect = "";
var userNowX = 0;  //列 0から
var userNowY = 0;  //行 0から
var userToX = 0;  //goto列 0から
var userToY = 0;  //goto行 0から
var userNowAct = "";  //"move" "attack" "package"
var userMoveSpeed;  //move speed

var mapSpaceWidth = 0;
var mapSpaceTop = 80;//top space
var mapSpaceButtom = 80;//top space
var mapBoxSize = 0;

var topSprite;
var topSprite_Left_BG;
var topSprite_xue;
var topSprite_mo;
var topSprite_jingyan;
var topLabel_xue;
var topLabel_mo;
var topLabel_jingyan;
var topLabel_jinbi;
var topLabel_zuanshi;
var spritePackage;

var buttomMenuSpr = [];

var MapSceneLayer = cc.Layer.extend({
	helloLabel:null,
	monsterSpriteArr: [],
	monsterInfoSprite: null,

	schedule1:null,
	userSprite: null,
	
	spriteMap: null,
	boxSelectedSpr: null,
	directBGSpr: null,
	directSpr: [], //0:left 1:top 2:right 3:down
	moveAnimate: [], //0:left 1:top 2:right 3:down
	attAnimate: [], //0:left 1:top 2:right 3:down

	attackDirect : null,
	init:function () {
//		cc._addEventListener("mousemove","");
//		this._touchEnabled = true;  // 设置触摸模式为:可用 
//		this._keyboardEnabled = true;  // 设置键盘为:可用 
//		this._mouseEnabled = true;  // 设置鼠标为:可用 
		//////////////////////////////
		// 1. super init first
		this._super();
		nowPageName = "map";
		topSprite = null;

		nowLayer = this;

		userDirect = "";
		userNowAct = "";  //"move" "attack"
		userMoveSpeed = 0.3;  //move speed


		mapSpaceWidth = 0;
		mapSpaceTop = 80;//top space
		mapSpaceButtom = 80;//buttom space
		mapBoxSize = 0;

		/////////////////////////////
		// 2. add a menu item with "X" image, which is clicked to quit the program
		//	you may modify it.
		// ask director the window size
//		var size = cc.director.getWinSize();
		
		// add a "close" icon to exit the progress. it's an autorelease object
/*		var closeItem = new cc.MenuItemImage(
			s_CloseNormal,
			s_CloseSelected,
			function () {
				cc.log("toStartG");
				toStartG();
			},this);
		closeItem.setAnchorPoint(1, 0);
//		closeItem.setScale(size.width / 6 / menuImg0.getContentSize().width);
		closeItem.setPosition(size.width, 0);*/


		
/*
		//add menu 0
		var menuImg0 = new cc.MenuItemImage(
			menu_select,
			menu_select,
			function () {
				//cc.log("toStartG");
				//toStartG();
			}, this);
		menuImg0.setAnchorPoint(0, 0);
		menuImg0.setScale(size.width / 6 / menuImg0.getContentSize().width);

		var menu0 = new cc.Menu(menuImg0);
		menu0.setPosition(0, 0);
		nowLayer.addChild(menu0, 5);
		closeItem.setPosition(size.width, 0);*/
		/////////////////////////////
		// 3. add your codes below...
		// add a label shows "Hello World"
		// create and initialize a label
/*		nowLayer.helloLabel = new cc.LabelTTF("MAPMAPMAPMAPMAP", "Impact", 38);
		// position the label on the center of the screen
		nowLayer.helloLabel.setPosition(size.width / 2, size.height - 40);
		// add the label as a child to this layer
		nowLayer.addChild(nowLayer.helloLabel, 10);*/

		//load move animate
		for (var i=0;i<4;i++){
			var moveAnimationTemp = new cc.Animation();
			var attAnimationTemp = new cc.Animation();
			for (var j = 1; j < 4; j++)
			{
				moveAnimationTemp.addSpriteFrameWithFile(img_userMove[i][j]);
				attAnimationTemp.addSpriteFrameWithFile(img_userAttack[i][j]);
			}
			//设置帧动画属性
			moveAnimationTemp.setDelayPerUnit(0.05);				//每一帧停留的时间
			moveAnimationTemp.setRestoreOriginalFrame(true);	//播放完后回到第一帧
			nowLayer.moveAnimate[i] = new cc.Animate(moveAnimationTemp);

			attAnimationTemp.setDelayPerUnit(0.05);				//每一帧停留的时间
			attAnimationTemp.setRestoreOriginalFrame(true);	//播放完后回到第一帧
			nowLayer.attAnimate[i] = new cc.Animate(attAnimationTemp);
		}

		//map
		//background
		nowLayer.spriteMap = new cc.Sprite(img_package_backGround);
		nowLayer.spriteMap.setAnchorPoint(0, 0);
		nowLayer.spriteMap.setPosition(0, mapSpaceButtom);
		nowLayer.spriteMap.setScale(size.width / nowLayer.spriteMap.width, (size.height-mapSpaceButtom-mapSpaceTop) / nowLayer.spriteMap.height);
		nowLayer.addChild(nowLayer.spriteMap, 1);

		mapBoxSize = Math.floor(Math.min(nowLayer.spriteMap.height / mapArrHeight, (nowLayer.spriteMap.width - 8) / mapArrWidth));
		mapSpaceWidth = Math.floor((nowLayer.spriteMap.width - mapArrWidth * mapBoxSize) / 2);

		//上部xue
		initXuemo();
		//下部menu
		initFooter();
		//中间地图部
		nowLayer.initMap();

		//执行一次，无次数限制
//		cc.director.getScheduler().schedule(nowLayer.userWalk, this, 0.2, false);

		//direct
		//Listen
		var timeOutEvent;
		var directMoveFlg = false;
		var listener_center = cc.EventListener.create({
			event: cc.EventListener.TOUCH_ONE_BY_ONE,
			swallowTouches: true,						// 设置是否吞没事件，在 onTouchBegan 方法返回 true 时吞掉事件，不再向下传递。
			onTouchBegan: function (touch, event) {		//实现 onTouchBegan 事件处理回调函数
				//判断点击区域
				var target = event.getCurrentTarget();
				var pos = target.convertToNodeSpace(touch.getLocation());
				if(!cc.rectContainsPoint(target.getTextureRect(), pos))return false;

				userDirect = "";
				timeOutEvent = setTimeout(function () {
					var pos2 = target.convertToNodeSpace(touch.getLocation());
					if(!cc.rectContainsPoint(target.getTextureRect(), pos2))return false;
					//长按事件
					timeOutEvent = 0;
					cc.log("长按");
					nowLayer.directBGSpr.setTexture(img_direct_bg_move);
					directMoveFlg = true;
				},1000);//长按ms，可根据需求自行更改
				return true;
			},
			onTouchMoved: function (touch, event) {			//实现onTouchMoved事件处理回调函数, 触摸移动时触发
				if (directMoveFlg) {
					nowLayer.directBGSpr.setPosition(nowLayer.convertToNodeSpace(touch.getLocation()));
				} else {
					var target = event.getCurrentTarget();
					var pos = target.convertToNodeSpace(touch.getLocation());
					if(!cc.rectContainsPoint(target.getTextureRect(), pos)) {
						clearTimeout(timeOutEvent);
						timeOutEvent = 0;
					}
				}
			},
			onTouchEnded: function (touch, event) {			// 实现onTouchEnded事件处理回调函数
				clearTimeout(timeOutEvent);
				timeOutEvent = 0;
				nowLayer.directBGSpr.setTexture(img_direct_bg);
				directMoveFlg = false;
			}
		});

		var listener_direct = cc.EventListener.create({
			event: cc.EventListener.TOUCH_ONE_BY_ONE,
			swallowTouches: true,
			onTouchBegan: function (touch, event) {
				//判断点击区域
				var target = event.getCurrentTarget();
				var pos = target.convertToNodeSpace(touch.getLocation());
				if(!cc.rectContainsPoint(target.getTextureRect(), pos))return false;

				userDirect = target.getTag();
				cc.log("start+" + userDirect);
				nowLayer.userWalk();
				return true;
			},
			onTouchMoved: function (touch, event) {			//实现onTouchMoved事件处理回调函数, 触摸移动时触发
				var target = event.getCurrentTarget();
				for (var i=0; i<nowLayer.directSpr.length;i++) {
					var pos = nowLayer.directSpr[i].convertToNodeSpace(touch.getLocation());
					if (!cc.rectContainsPoint(nowLayer.directSpr[i].getTextureRect(), pos)) {
						continue;
					} else {
						userDirect = nowLayer.directSpr[i].getTag();
						cc.log("move+" + userDirect);
						return;
					}
				}
				userDirect = "";
				cc.log("move+" + userDirect);
				return;
			},
			onTouchEnded: function (touch, event) {			// 实现onTouchEnded事件处理回调函数
				userDirect = "";
				cc.log("end+" + userDirect);
			}
		});

		//BG
		nowLayer.directBGSpr = new cc.Sprite(img_direct_bg);
		nowLayer.directBGSpr.setAnchorPoint(0.5, 0.5);
//		nowLayer.directBGSpr.setPosition(nowLayer.directBGSpr.width/2+mapSpaceWidth*2,nowLayer.directBGSpr.height/2+mapSpaceButtom);
		nowLayer.directBGSpr.setPosition(nowLayer.spriteMap.width-nowLayer.directBGSpr.width/2,nowLayer.directBGSpr.height/2+mapSpaceButtom);
		nowLayer.addChild(nowLayer.directBGSpr,7);

		var directCenter = new cc.Sprite(img_direct_center);
		directCenter.setAnchorPoint(0.5, 0.5);
		directCenter.setPosition(nowLayer.directBGSpr.width/2,nowLayer.directBGSpr.height/2);
		nowLayer.directBGSpr.addChild(directCenter,1);
		cc.eventManager.addListener(listener_center, directCenter);

		for (var i=0;i<img_direct_4.length;i++) {
			nowLayer.directSpr[i] = new cc.Sprite(img_direct_4[i]);
			if (i==0) {
				nowLayer.directSpr[i].setAnchorPoint(0, 0);
				nowLayer.directSpr[i].setPosition(0,nowLayer.directBGSpr.height/3);
				nowLayer.directSpr[i].setTag("left");
			} else if (i==1) { //left
				nowLayer.directSpr[i].setAnchorPoint(0, 1);
				nowLayer.directSpr[i].setPosition(nowLayer.directBGSpr.width/3,nowLayer.directBGSpr.height);
				nowLayer.directSpr[i].setTag("up");
			} else if (i==2) { //left
				nowLayer.directSpr[i].setAnchorPoint(1, 0);
				nowLayer.directSpr[i].setPosition(nowLayer.directBGSpr.width,nowLayer.directBGSpr.height/3);
				nowLayer.directSpr[i].setTag("right");
			} else if (i==3) { //left
				nowLayer.directSpr[i].setAnchorPoint(0, 0);
				nowLayer.directSpr[i].setPosition(nowLayer.directBGSpr.width/3,0);
				nowLayer.directSpr[i].setTag("down");
			}
			nowLayer.directBGSpr.addChild(nowLayer.directSpr[i],1);
			cc.eventManager.addListener(listener_direct.clone(), nowLayer.directSpr[i]);
		}

		//ゴミとなる
		var listener1 = cc.EventListener.create({
			event: cc.EventListener.TOUCH_ONE_BY_ONE,
			swallowTouches: true,						// 设置是否吞没事件，在 onTouchBegan 方法返回 true 时吞掉事件，不再向下传递。
			onTouchBegan: function (touch, event) {		//实现 onTouchBegan 事件处理回调函数
				cc.log("onTouchBegan");
				userDirect = "";
				return true;
			},
			onTouchMoved: function (touch, event) {			//实现onTouchMoved事件处理回调函数, 触摸移动时触发
				cc.log("onTouchMoved");
				// 移动当前按钮精灵的坐标位置
				var delta = touch.getDelta();			  //获取事件数据: delta
				if (Math.abs(delta.x) > Math.abs(delta.y) && Math.abs(delta.x) > 1) { //2是移动距离
					if (delta.x > 0) {
						userDirect = "right";
					} else {
						userDirect = "left";
					}
				} else if (Math.abs(delta.y) > Math.abs(delta.x) && Math.abs(delta.y) > 1) {
					if (delta.y > 0) {
						userDirect = "up";
					} else {
						userDirect = "down";
					}
				} else {
					userDirect = "";
				}
				return true;
			},
			onTouchEnded: function (touch, event) {			// 实现onTouchEnded事件处理回调函数
				cc.log("onTouchEnded");
				userDirect = "";
			}
		});
//		cc.eventManager.addListener(listener1, nowLayer.spriteMap);
	},

	initMap: function () {
		nowLayer.spriteMap.removeAllChildren(true);

		//中间地图部
		nowLayer.monsterSpriteArr = [];
		var boxFlg = 0;
		var spriteBackground = null;
		var spriteMonster = null;
		var spriteSuprise = null;
		var spriteWall = null;
		var spriteDoor = null;
		var bossBoxNumNow = 0;
		var bossTag = ",";
		for (var y = 0; y < mapArrHeight; y++) {
			for (var x = 0; x < mapArrWidth; x++) {
				spriteBackground = null;
				spriteMonster = null;
				spriteSuprise = null;
				spriteWall = null;
				spriteDoor = null;
				boxFlg = mapData.map[y][x];
				if (bossArea.indexOf(boxFlg) >= 0) { //boss
					bossBoxNumNow++;
					bossTag += y + "_" + x + ",";
					if (bossBoxNumNow == bossAllBoxNum) { //最后一个boss格子到了
						var _tagMonsterTemp = new _tagMonster();
						var monsterInfo = getJson(mapData.map_boss_json[bossArea.indexOf(boxFlg)]);

						spriteMonster = new cc.Sprite(monsterInfo.pic);
						spriteMonster.setAnchorPoint(1, 0);
						spriteMonster.setPosition(cc.p(mapBoxSize * x + mapSpaceWidth + mapBoxSize, nowLayer.spriteMap.height - mapBoxSize * (y+1)));
						spriteMonster.setScale(mapBoxSize * Math.sqrt(bossAllBoxNum) / spriteMonster.getContentSize().height);
						spriteMonster.setTag(bossTag);
						nowLayer.spriteMap.addChild(spriteMonster, 5);
						_tagMonsterTemp.setValue(monsterInfo,spriteMonster,true);
						nowLayer.monsterSpriteArr.push(_tagMonsterTemp);
						cc.eventManager.addListener(nowLayer.listener_monster.clone(), spriteMonster);
					}
					spriteBackground = new cc.Sprite(mapData.map_road_png[0]);
					
				} else if (monsterArea.indexOf(boxFlg) >= 0) { //monster
					var _tagMonsterTemp = new _tagMonster();
					var monsterInfo = getJson(mapData.map_monster_json[monsterArea.indexOf(boxFlg)]);

					spriteMonster = new cc.Sprite(monsterInfo.pic);
					spriteMonster.setAnchorPoint(0, 1);
					spriteMonster.setPosition(nowLayer.getMapPosition(x, y));
					spriteMonster.setScale(mapBoxSize / spriteMonster.getContentSize().height);
					spriteMonster.setTag(y + "_" + x);
					nowLayer.spriteMap.addChild(spriteMonster, 5);
					_tagMonsterTemp.setValue(monsterInfo,spriteMonster,false);
					nowLayer.monsterSpriteArr.push(_tagMonsterTemp);
					cc.eventManager.addListener(nowLayer.listener_monster.clone(), spriteMonster);

					spriteBackground = new cc.Sprite(mapData.map_road_png[0]);
				} else if (supriseAreaClosed.indexOf(boxFlg) >= 0) { //suprise closedBox
					spriteBackground = new cc.Sprite(mapData.map_road_png[0]);
					spriteSuprise = new cc.Sprite(mapData.map_supriseBox_closed_png[supriseAreaClosed.indexOf(boxFlg)]);
				} else if (supriseAreaOpened.indexOf(boxFlg) >= 0) { //suprise opendBox
					spriteBackground = new cc.Sprite(mapData.map_road_png[0]);
					spriteSuprise = new cc.Sprite(mapData.map_supriseBox_opened_png[supriseAreaOpened.indexOf(boxFlg)]);
				} else if (cantMoveArea.indexOf(boxFlg) >= 0) { //wall
					spriteWall = new cc.Sprite(mapData.map_wall_png[cantMoveArea.indexOf(boxFlg)]);
					spriteWall.setAnchorPoint(0, 1);
					spriteWall.setPosition(nowLayer.getMapPosition(x, y));
					spriteWall.setScale(mapBoxSize / spriteWall.getContentSize().height);
					nowLayer.spriteMap.addChild(spriteWall, 5);

					spriteBackground = new cc.Sprite(mapData.map_road_png[0]);
				} else if (outDoorArea.indexOf(boxFlg) >= 0) { //door
					spriteDoor = new cc.Sprite(mapData.map_outDoor_png[outDoorArea.indexOf(boxFlg)]);
					spriteDoor.setAnchorPoint(0, 1);
					spriteDoor.setPosition(nowLayer.getMapPosition(x, y));
					spriteDoor.setScale(mapBoxSize / spriteDoor.getContentSize().width,mapBoxSize / spriteDoor.getContentSize().height);
					nowLayer.spriteMap.addChild(spriteDoor, 5);

					spriteBackground = new cc.Sprite(mapData.map_road_png[0]);
				} else {
					spriteBackground = new cc.Sprite(mapData.map_road_png[0]);
				}
				spriteBackground.setAnchorPoint(0, 1);
				spriteBackground.setPosition(nowLayer.getMapPosition(x, y));
				spriteBackground.setScale(mapBoxSize / spriteBackground.getContentSize().height);
				nowLayer.spriteMap.addChild(spriteBackground, 1);

				if (spriteSuprise != null) {
					spriteSuprise.setAnchorPoint(0, 1);
					spriteSuprise.setPosition(nowLayer.getMapPosition(x, y));
					spriteSuprise.setScale(mapBoxSize / spriteSuprise.getContentSize().height);
					nowLayer.spriteMap.addChild(spriteSuprise, 5);
				}
			}
		}

		//add user
		nowLayer.userSprite = new cc.Sprite(img_userMove[2][0]);
		nowLayer.userSprite.setAnchorPoint(0, 1);
		nowLayer.userSprite.setPosition(nowLayer.getMapPosition(userNowX, userNowY));
		nowLayer.userSprite.setScale(mapBoxSize / nowLayer.userSprite.getContentSize().height);
		nowLayer.spriteMap.addChild(nowLayer.userSprite, 5);
	},

	listener_monster:cc.EventListener.create({
		event: cc.EventListener.TOUCH_ONE_BY_ONE,
		swallowTouches: true,						// 设置是否吞没事件，在 onTouchBegan 方法返回 true 时吞掉事件，不再向下传递。
		timeOutEvent : null,
		onTouchBegan: function (touch, event) {		//实现 onTouchBegan 事件处理回调函数
			//判断点击区域
			var target = event.getCurrentTarget();
			var pos = target.convertToNodeSpace(touch.getLocation());
			if(!cc.rectContainsPoint(target.getTextureRect(), pos))return false;

			timeOutEvent = setTimeout(function () {
				var pos2 = target.convertToNodeSpace(touch.getLocation());
				if(!cc.rectContainsPoint(target.getTextureRect(), pos2))return false;
				//长按事件
				timeOutEvent = 0;
				cc.log("长按monster");

				//get monster info
				for (var index in nowLayer.monsterSpriteArr) {
					if (nowLayer.monsterSpriteArr[index].sprite === target) {
						nowLayer.showMonsterInfo(nowLayer.monsterSpriteArr[index]);
						break;
					}
				}


			},1000);//长按ms，可根据需求自行更改
			return true;
		},
		onTouchMoved: function (touch, event) {			//实现onTouchMoved事件处理回调函数, 触摸移动时触发
			var target = event.getCurrentTarget();
			var pos = target.convertToNodeSpace(touch.getLocation());
			if (!cc.rectContainsPoint(target.getTextureRect(), pos)) {
				clearTimeout(timeOutEvent);
				timeOutEvent = 0;
			}
		},
		onTouchEnded: function (touch, event) {			// 实现onTouchEnded事件处理回调函数
			clearTimeout(timeOutEvent);
			timeOutEvent = 0;
			nowLayer.removeMonsterInfo();
		}
	}),

	showMonsterInfo:function(monsTemp){
		var message = "名字:" + monsTemp.name + "\n" +
			  "性质:" + getMonsterType[monsTemp.type] + "\n" +
			  "攻击:" + monsTemp.property[0] + "\n" +
			  "防御:" + monsTemp.property[1] + "\n" +
			  "血量:" + monsTemp.property[2] + "\n";

		if (nowLayer.monsterInfoSprite) {
			nowLayer.monsterInfoSprite.children[0].setString(message);
		} else {
			//showInfo
			nowLayer.monsterInfoSprite = new cc.Sprite(img_map_infoBG);
			var tagInfo = monsTemp.sprite.getTag().split("_");
			if (parseInt(tagInfo[1]) < mapArrWidth / 2) {
				nowLayer.monsterInfoSprite.setAnchorPoint(1, 1);
				nowLayer.monsterInfoSprite.setPosition(nowLayer.spriteMap.width, nowLayer.spriteMap.height);
			} else {
				nowLayer.monsterInfoSprite.setAnchorPoint(0, 1);
				nowLayer.monsterInfoSprite.setPosition(0, nowLayer.spriteMap.height);
			}
			nowLayer.monsterInfoSprite.setScale(nowLayer.spriteMap.width / 3 / nowLayer.monsterInfoSprite.width);
			nowLayer.spriteMap.addChild(nowLayer.monsterInfoSprite, 9);

			var infoLabel = new cc.LabelTTF(message, "Impact", 25);
			infoLabel.setAnchorPoint(0, 1);
			infoLabel.setPosition(20, nowLayer.monsterInfoSprite.height - 20);
			infoLabel.setColor(cc.color(0, 0, 0, 255));
			nowLayer.monsterInfoSprite.addChild(infoLabel, 1);
		}
	},
	removeMonsterInfo:function(){
		if (nowLayer.monsterInfoSprite) {
			nowLayer.spriteMap.removeChild(nowLayer.monsterInfoSprite, true);
			nowLayer.monsterInfoSprite = null;
		}
	},

	userWalk: function() {//移动
		if (nowLayer.userSprite != null && userNowAct == "" && userDirect != "") {
			if (userDirect == "up") {
				cc.log("up");
				if (userNowY-1 >= 0) { //边界以内
					userToY = userNowY-1;
					userToX = userNowX;
				} else {
					userDirect = "";
				}
			} else if (userDirect == "down") {
				cc.log("down");
				if (userNowY + 1 < mapArrHeight) {
					userToY = userNowY+1;
					userToX = userNowX;
				} else {
					userDirect = "";
				}
			} else if (userDirect == "right") {
				cc.log("right");
				if (userNowX + 1 < mapArrWidth) {
					userToY = userNowY;
					userToX = userNowX+1;
				} else {
					userDirect = "";
				}
			} else if (userDirect == "left") {
				cc.log("left");
				if (userNowX-1 >= 0) {
					userToY = userNowY;
					userToX = userNowX-1;
				} else {
					userDirect = "";
				}
			}

			if (userToY != userNowY || userToX != userNowX) {
				var boxFlg = mapData.map[userToY][userToX];
				if (cantMoveArea.indexOf(boxFlg) >= 0 || supriseAreaOpened.indexOf(boxFlg) >= 0) { //不可移动
					cc.log("cant move");
					nowLayer.userSprite.setTexture(img_userMove[direct[userDirect]][0]);
					return;
				} else if (supriseAreaClosed.indexOf(boxFlg) >= 0) { //suprise
					cc.log("suprise");
					nowLayer.userSprite.setTexture(img_userMove[direct[userDirect]][0]);
					//get suprise
					if (user.wuqiPackage.length + user.yaoshuiPackage.length < user.openedPackageNum) { //Package not full
						mapData.map[userToY][userToX] = supriseAreaOpened[supriseAreaOpened.indexOf(boxFlg)]; //mapData.map[userToY][userToX] + supriseAreaClosed.length;
						var spriteSupriseOpened = new cc.Sprite(mapData.map_supriseBox_opened_png[supriseAreaClosed.indexOf(boxFlg)]);
						spriteSupriseOpened.setAnchorPoint(0, 1);
						spriteSupriseOpened.setPosition(nowLayer.getMapPosition(userToX, userToY));
						spriteSupriseOpened.setScale(mapBoxSize / spriteSupriseOpened.getContentSize().height);
						nowLayer.spriteMap.addChild(spriteSupriseOpened, 7);

						var randomTemp = Math.random();
						if (supriseAreaClosed.indexOf(boxFlg) == 2) { //boss box
							randomTemp = randomTemp + (mapData.dropWuqiPer[mapData.dropWuqiPer.length-1] - 1);//1以上的boss专用掉落
						}
						for (var i=0;i<mapData.dropWuqiPer.length;i++) {
							if (randomTemp < mapData.dropWuqiPer[i]) {
								var wuqiInfo = getWuqiInfo(mapData.dropWuqi[i]);
								var tagWuqiTemp = new _tagWuqi();
								tagWuqiTemp.id = mapData.dropWuqi[i];
								tagWuqiTemp.trueProperty = wuqiInfo.property;
								if (wuqiInfo.special_type && wuqiInfo.special_type.length>0) { //武器自带特殊属性
									tagWuqiTemp.special_type = wuqiInfo.special_type;
									tagWuqiTemp.special_value = wuqiInfo.special_value;
								} else {
									var randomTemp2 = Math.random();
									if (supriseAreaClosed.indexOf(boxFlg) == 1 || supriseAreaClosed.indexOf(boxFlg) == 2) { //gold box  or  boss box
										randomTemp2 = randomTemp2 * mapData.special_per[mapData.special_per.length - 1];
									}
									for (var j=0;j<mapData.special_per.length;j++) {
										if (randomTemp2 < mapData.special_per[j]) { //特殊个数
											for(var x=0;x<j+1;x++){
												var randomTemp3 = Math.floor(Math.random() * 14);
												tagWuqiTemp.special_type.push(randomTemp3);
												tagWuqiTemp.special_value.push(mapData.special_value[randomTemp3]);
											}
											break;
										}
									}
								}
								user.wuqiPackage.push(tagWuqiTemp);
								showGetMono(wuqiFolder + mapData.dropWuqi[i] + ".png",spriteSupriseOpened._position);
								break;
							}
						}

						saveUser();
						cc.log(user.wuqiPackage);
					} else { //package full
						showLabelInfo(0, 30, "背包满了", nowLayer.userSprite._position);
					}

					return;
				} else if (moveArea.indexOf(boxFlg) >= 0 || outDoorArea.indexOf(boxFlg) >= 0) {  //can move  or  //door
					nowLayer.moveAnimateFun(userDirect);
					var actionMove = cc.MoveTo.create(userMoveSpeed,nowLayer.getMapPosition(userToX,userToY));
					var actionMoveDone = cc.CallFunc.create(nowLayer.spriteMoveFinished,this);
					nowLayer.userSprite.runAction(cc.Sequence.create(actionMove,actionMoveDone));
					
					userNowAct = "move";
					userNowX = userToX;
					userNowY = userToY;

					if (outDoorArea.indexOf(boxFlg) >= 0) { //door
						if (user.selectLevel < 100) {
							var spriteTemp = new cc.Sprite(img_dialog_center);
							spriteTemp.setAnchorPoint(0.5, 0.5);
							spriteTemp.setPosition(size.width / 2, size.height / 2);
							spriteTemp.setScale(size.width / spriteTemp.width);
							nowLayer.addChild(spriteTemp, 10);

							//message
							var message = new cc.LabelTTF("要进入下一区域吗？", "Impact", 40);
							message.setAnchorPoint(0.5, 0.5);
							message.setPosition(spriteTemp.width / 2, spriteTemp.height * 0.75);
							message.setColor(cc.color(0, 0, 0, 255));
							spriteTemp.addChild(message, 1);

							var buttonList = [];
							//ng
							var ngSpr = new cc.MenuItemImage(img_dialog_ng_btn, img_dialog_ng_btn, function () {
								nowLayer.removeChild(spriteTemp);
							}, this);
							ngSpr.setAnchorPoint(0.5, 0.5);
							ngSpr.setPosition(spriteTemp.width * 0.25, spriteTemp.height *0.25);
							buttonList.push(ngSpr);
							//ok
							var okSpr = new cc.MenuItemImage(img_dialog_ok_btn, img_dialog_ok_btn, function () {
								user.selectLevel++;
								if (user.maxDiffcute == user.selectDiffcute && user.selectLevel > user.maxLevel) {
									user.maxLevel = user.selectLevel;
								}
								mapData = getJson("res/map/" + user.selectLevel + "_" + user.selectDiffcute + ".json");

								autoMapCreat();

								nowLayer.initMap();
								saveUser();
								userNowAct = "";
							}, this);
							okSpr.setAnchorPoint(0.5, 0.5);
							okSpr.setPosition(spriteTemp.width * 0.75, spriteTemp.height * 0.25);
							buttonList.push(okSpr);

							var menu = new cc.Menu(buttonList);
							menu.setPosition(0, 0);
							spriteTemp.addChild(menu, 1);
						} else {
							var spriteTemp = new cc.Sprite(img_dialog_center);
							spriteTemp.setAnchorPoint(0.5, 0.5);
							spriteTemp.setPosition(size.width / 2, size.height / 2);
							spriteTemp.setScale(size.width / spriteTemp.width);
							nowLayer.addChild(spriteTemp, 10);

							//message
							var message = new cc.LabelTTF("恭喜你通关。更强的挑战向你敞开了大门。", "Impact", 40);
							message.setAnchorPoint(0.5, 0.5);
							message.setPosition(spriteTemp.width / 2, spriteTemp.height * 0.75);
							message.setColor(cc.color(0, 0, 0, 255));
							spriteTemp.addChild(message, 1);

							var buttonList = [];
							//ng
							var ngSpr = new cc.MenuItemImage(img_dialog_ng_btn, img_dialog_ng_btn, function () {
								nowLayer.removeChild(spriteTemp);
							}, this);
							ngSpr.setAnchorPoint(0.5, 0.5);
							ngSpr.setPosition(spriteTemp.width * 0.25, spriteTemp.height * 0.25);
							buttonList.push(ngSpr);
							//ok
							var okSpr = new cc.MenuItemImage(img_dialog_ok_btn, img_dialog_ok_btn, function () {
								if (user.maxDiffcute == user.selectDiffcute) {
									user.maxDiffcute++;
									user.selectDiffcute = user.maxDiffcute;
									user.maxLevel = 1;
								}
								userNowAct = "";
								saveUser();
								gotoHome();
							}, this);
							okSpr.setAnchorPoint(0.5, 0.5);
							okSpr.setPosition(spriteTemp.width * 0.75, spriteTemp.height * 0.25);
							buttonList.push(okSpr);

							var menu = new cc.Menu(buttonList);
							menu.setPosition(0, 0);
							spriteTemp.addChild(menu, 1);
						}
					}
				} else if (monsterArea.indexOf(boxFlg) >= 0 || bossArea.indexOf(boxFlg) >= 0) { //monster
					cc.log("monster");
					//attack TODO
					userNowAct = "attack";
//					nowLayer.userSprite.setTexture(img_user_att);
					
					//get monster info
					for (var index in nowLayer.monsterSpriteArr) {
						if (nowLayer.monsterSpriteArr[index].sprite.getTag() == userToY + "_" + userToX
							|| nowLayer.monsterSpriteArr[index].sprite.getTag().indexOf("," + userToY + "_" + userToX + ",") >=0) { //TODO BOSS check
							monster = nowLayer.monsterSpriteArr[index];
							break;
						}
					}

					nowLayer.attackDirect = userDirect;
					nowLayer.showMonsterInfo(monster);
					nowLayer.attackMethod();
					//nowLayer.userSprite.schedule(nowLayer.attackMethod,0.5);
				}
			}
		}
	},
	spriteMoveFinished: function() {//移动ok
		userNowAct = "";
		nowLayer.userWalk();
	},
	getMapPosition: function(x,y) {//mapBoxのposition  x:列 y:行
		var position = cc.p(mapBoxSize*x + mapSpaceWidth, nowLayer.spriteMap.height - mapBoxSize*y);
		return position;
	},
	attackMethod: function() {
		cc.log("start att");

		var monsterAtt = getMonsterTrueAttack();
		var userAtt = getUserTrueAttack();

		//user att
//		nowLayer.attAnimateFun(nowLayer.attackDirect);

		monster.property[xueNo] = monster.property[xueNo] - userAtt;
		//掉血模拟
		showLabelInfo(0, 20, "-" + userAtt, monster.sprite._position);
		nowLayer.showMonsterInfo(monster);

		if (monster.property[xueNo] <= 0) {
			nowLayer.userSprite.unscheduleAllCallbacks();
			cc.log("success");

			//add jingyan
			user.jingyanNow += monster.jingyan + user.property[jingNo];
			while (user.jingyanNow >= user.jingyanAll) {
				user.level++;
				user.jingyanNow = user.jingyanNow - user.jingyanAll;
				levelUpUser();
				renewUser();
			}
			//add jinbi
			user.jinbi += monster.jinbi + user.property[jinbiNo];
			initXuemo();

			//diaoluo
			var randomPer = Math.random();
			for (var i=0;i<mapData.dropYaoshuiPer.length;i++) {
				if (randomPer < mapData.dropYaoshuiPer[i]) {
					userGetItem("yaoshui",mapData.dropYaoshui[i] ,1);
					cc.log(user.yaoshuiPackage);
					showGetMono(yaoshuiFolder + mapData.dropYaoshui[i] + ".png", monster.sprite._position);
					saveUser();
					break;
				}
			}

			//remove guaiwu 
			nowLayer.spriteMap.removeChild(monster.sprite, true);
			nowLayer.monsterSpriteArr.splice(nowLayer.monsterSpriteArr.indexOf(monster), 1);

			//ditubiao zhi biangeng
			if (bossArea.indexOf(mapData.map[userToY][userToX]) >= 0) { //boss
				var bossBoxNumberTemp = 0;
				for(var i=0;i<mapArrHeight;i++){
					for(var j=0;j<mapArrWidth;j++){
						if (bossArea.indexOf(mapData.map[i][j]) >= 0) {
							bossBoxNumberTemp++;
							if (bossBoxNumberTemp == 3) { //door
								mapData.map[i][j] = outDoorArea[0];

								var spriteDoor = new cc.Sprite(mapData.map_outDoor_png[0]);
								spriteDoor.setAnchorPoint(0, 1);
								spriteDoor.setPosition(nowLayer.getMapPosition(j, i));
								spriteDoor.setScale(mapBoxSize / spriteDoor.getContentSize().width,mapBoxSize / spriteDoor.getContentSize().height);
								nowLayer.spriteMap.addChild(spriteDoor, 5);
							} else if (bossBoxNumberTemp == 1) { //bossBox
								mapData.map[i][j] = supriseAreaClosed[2];

								var spriteSuprise = new cc.Sprite(mapData.map_supriseBox_closed_png[2]);
								spriteSuprise.setAnchorPoint(0, 1);
								spriteSuprise.setPosition(nowLayer.getMapPosition(j, i));
								spriteSuprise.setScale(mapBoxSize / spriteSuprise.getContentSize().width, mapBoxSize / spriteSuprise.getContentSize().height);
								nowLayer.spriteMap.addChild(spriteSuprise, 5);
							} else {
								mapData.map[i][j] = moveArea[0];
							}
						}
					}
				}
			} else {
				if (Math.random() < 1/(nowLayer.monsterSpriteArr.length+1) && mapData.has_boss_flag == "0") {
					mapData.map[userToY][userToX] = outDoorArea[0];

					var spriteDoor = new cc.Sprite(mapData.map_outDoor_png[0]);
					spriteDoor.setAnchorPoint(0, 1);
					spriteDoor.setPosition(nowLayer.getMapPosition(userToX, userToY));
					spriteDoor.setScale(mapBoxSize / spriteDoor.getContentSize().width, mapBoxSize / spriteDoor.getContentSize().height);
					nowLayer.spriteMap.addChild(spriteDoor, 5);

					userToX = userNowX;
					userToY = userNowY;
				} else {
					mapData.map[userToY][userToX] = moveArea[0];
				}
			}

			nowLayer.moveAnimateFun(nowLayer.attackDirect);
			var actionMove = cc.MoveTo.create(userMoveSpeed,nowLayer.getMapPosition(userToX,userToY));
			var actionMoveDone = cc.CallFunc.create(nowLayer.spriteMoveFinished,this);
			nowLayer.userSprite.runAction(cc.Sequence.create(actionMove,actionMoveDone));
			userNowAct = "move";
			userNowX = userToX;
			userNowY = userToY;

			//save user info
			saveUser();
			nowLayer.removeMonsterInfo();
			initFooter();
			return;
		}

		user.xueNow = user.xueNow - monsterAtt;
		//掉血模拟
		if (monsterAtt == 0) {
			showLabelInfo(0, 20, "MISS", nowLayer.userSprite._position);
		} else {
			showLabelInfo(0, 20, "-" + monsterAtt, nowLayer.userSprite._position);
		}
		initXuemo();
		if (user.xueNow <= 0) {
			cc.log("failed");
//TODO			nowLayer.userSprite.setTexture(img_userMove[3][0]);

			cc.eventManager.pauseTarget(nowLayer, true);

			nowLayer.userSprite.unscheduleAllCallbacks();
			reloadUser();
			userNowAct = "";
			userDirect = "";
			nowLayer.removeMonsterInfo();

			var spriteTemp = new cc.Sprite(img_dialog_center);
			spriteTemp.setAnchorPoint(0.5, 0.5);
			spriteTemp.setPosition(size.width / 2, size.height / 2);
			spriteTemp.setScale(size.width / spriteTemp.width);
			nowLayer.addChild(spriteTemp, 10);

			//message
			var message = new cc.LabelTTF("很遗憾，你变成了这片森林的肥料。\n退出你将失去这次获得的一切。", "Impact", 40);
			message.setAnchorPoint(0.5, 0.5);
			message.setPosition(spriteTemp.width / 2, spriteTemp.height * 0.75);
			message.setColor(cc.color(0, 0, 0, 255));
			spriteTemp.addChild(message, 1);

			var buttonList = [];
			//out
			var outSpr = new cc.MenuItemImage(img_dialog_ng_btn, img_dialog_ng_btn, function () {
				loadOldUser();
				saveUser();
				gotoHome();
			}, this);
			outSpr.setAnchorPoint(0.5, 0.5);
			outSpr.setPosition(spriteTemp.width / 6, spriteTemp.height / 6);
			buttonList.push(outSpr);

			//video
			var videoSpr = new cc.MenuItemImage(img_dialog_ng_btn, img_dialog_ng_btn, function () {
				//TODO
				spriteTemp.removeAllChildren();

				user.xueNow = user.property[xueNo];
				user.moNow = user.property[moNo];

				cc.eventManager.resumeTarget(nowLayer, true);
				initXuemo();
				saveUser();
			}, this);
			videoSpr.setAnchorPoint(0.5, 0.5);
			videoSpr.setPosition(spriteTemp.width / 6, spriteTemp.height / 6);
			buttonList.push(videoSpr);

			//zuanshi
			var zuanshiNum = 300;
			var zuanshiSpr = new cc.MenuItemImage(img_dialog_ok_btn, img_dialog_ok_btn, function () {
				if (user.zuanshi >= zuanshiNum) {
					spriteTemp.removeAllChildren();
					nowLayer.removeChild(spriteTemp, true);
					user.zuanshi = user.zuanshi - zuanshiNum;
					user.xueNow = user.property[xueNo];
					user.moNow = user.property[moNo];

					cc.eventManager.resumeTarget(nowLayer, true);
					initXuemo();
					saveUser();
				} else {
					//TODO zuanshi buzu
					cc.log("zan shi bu zu");
				}
			}, this);
			zuanshiSpr.setAnchorPoint(0.5, 0.5);
			zuanshiSpr.setPosition(spriteTemp.width * 0.75, spriteTemp.height / 4);
			buttonList.push(zuanshiSpr);

			//message
			var price = new cc.LabelTTF(zuanshiNum + "", "Impact", 35);
			price.setAnchorPoint(0.5, 0.5);
			price.setPosition(zuanshiSpr.width * 0.7, zuanshiSpr.height / 2);
			price.setColor(cc.color(0, 255, 0, 255));
			zuanshiSpr.addChild(price, 1);

			var menu = new cc.Menu(buttonList);
			menu.setPosition(0, 0);
			spriteTemp.addChild(menu, 1);
			return;
		}
		nowLayer.userSprite.schedule(nowLayer.attackMethod,0.5);
	},
	moveAnimateFun: function (directTo) {
		nowLayer.userSprite.setTexture(img_userMove[direct[directTo]][0]);
		nowLayer.userSprite.runAction(nowLayer.moveAnimate[direct[directTo]]);
	},
	attAnimateFun: function (directTo) {
		nowLayer.userSprite.setTexture(img_userMove[direct[directTo]][0]);
		nowLayer.userSprite.runAction(nowLayer.attAnimate[direct[directTo]]);
	}
});

toStartG=function () {
	var MyScene = cc.Scene.create();
	var layer = new MenuLayer();
	MyScene.addChild(layer);
	layer.init();

	var tranScene=cc.TransitionMoveInR.create(0.5,MyScene);
	cc.director.runScene(tranScene);
}


var mapData;

var userGetItem = function(type,mono,num) {
	if (type == "yaoshui") {
		var yaoshuiIndex = user.yaoshuiPackage.indexOf(mono);
		if (yaoshuiIndex >= 0) { //已有同样的
			user.yaoshuiNum[yaoshuiIndex] = user.yaoshuiNum[yaoshuiIndex] + num;
			return true;
		} else if (user.wuqiPackage.length + user.yaoshuiPackage.length < user.openedPackageNum) {//Package not full
			user.yaoshuiPackage.push(mono);
			user.yaoshuiNum.push(1);
			return true;
		} else {
			showLabelInfo(0, 30, "背包满了", cc.p(nowLayer.width / 2, nowLayer.height / 2));
			return false;
		}
	} else if (type == "wuqi") {
		if (user.wuqiPackage.length + user.yaoshuiPackage.length < user.openedPackageNum) { //有空
			user.wuqiPackage.push(mono);
			return true;
		} else {
			return false;
		}
	}
	return false;
	cc.log(user.yaoshuiPackage);
}

function showLabelInfo(colorNum, labelSize, message, position){ //colorNum 0:diaoxue 1:jiaxue 3:
	var info = new cc.LabelTTF(message, "Impact", labelSize);
	info.setAnchorPoint(0.5, 0.5);
	info.setPosition(position);
	if (colorNum == 0) {
		info.setColor(cc.color(255, 0, 0, 255));
	} else if (colorNum == 1) {
		info.setColor(cc.color(0, 0, 255, 255));
	}
	
	if (userNowAct != "package") {
		nowLayer.spriteMap.addChild(info, 10);
	} else {
		nowLayer.addChild(info, 10);
	}

	var actionMove = cc.MoveTo.create(0.5, cc.p(info.x, info.y + 50));
	var actionMoveDone = cc.CallFunc.create(function () {
		if (userNowAct != "package") {
			nowLayer.spriteMap.removeChild(info, true)
		} else {
			nowLayer.removeChild(info, true)
		}
	}, this);
	info.runAction(cc.Sequence.create(actionMove, actionMoveDone));
}
function showGetMono(imgPath, position){
	var spriteTemp = new cc.Sprite(imgPath);
	spriteTemp.setAnchorPoint(0, 1);
	spriteTemp.setPosition(position);
	spriteTemp.setScale(mapBoxSize / spriteTemp.height/2);
	nowLayer.spriteMap.addChild(spriteTemp, 8);

	var actionMove = cc.MoveTo.create(0.5, cc.p(spriteTemp.x, spriteTemp.y + 50));
	var actionMoveDone = cc.CallFunc.create(function () { nowLayer.spriteMap.removeChild(spriteTemp,true)}, this);
	spriteTemp.runAction(cc.Sequence.create(actionMove, actionMoveDone));
}

function autoMapCreat() {
	//init Map
	if (mapData.map.length == 0) {
		for (var y = 0; y < mapArrHeight;y++) {
			mapData.map[y] = [];
			for (var x = 0; x < mapArrWidth; x++) {
				mapData.map[y][x] = cantMoveArea[Math.floor(Math.random() * cantMoveArea.length)];
			}
		}

		var tempY = mapArrHeight-1;
		var tempX = Math.floor(mapArrWidth / 2);

		mapData.map[tempY][tempX] = moveArea[Math.floor(Math.random() * moveArea.length)];

		var randomWalkLen; //1-20
		var randomTempDirect; //1:left 2:up 3:right 4:down

		var flag = 20; //map walk times
		//set road
		while (flag > 0) {
			randomWalkLen = Math.floor(Math.random() * mapData.autoMapWalkLen + 1);
			randomTempDirect = Math.floor(Math.random() * 4 + 1);

			for (var i = 0; i < randomWalkLen; i++) {
				if (randomTempDirect == 1) {
					tempX = tempX - 1;
					if (tempX < 0) {
						tempX = tempX + 2;
						randomTempDirect = 3;
					}
				} else if (randomTempDirect == 3) {
					tempX = tempX + 1;
					if (tempX > mapArrWidth-1) {
						tempX = tempX - 2;
						randomTempDirect = 1;
					}
				} else if (randomTempDirect == 2) {
					tempY = tempY - 1;
					if (tempY < 0) {
						tempY = tempY + 2;
						randomTempDirect = 4;
					}
				} else if (randomTempDirect == 4) {
					tempY = tempY + 1;
					if (tempY > mapArrHeight -1) {
						tempY = tempY - 2;
						randomTempDirect = 2;
					}
				}
				mapData.map[tempY][tempX] = moveArea[Math.floor(Math.random() * moveArea.length)];
			}
			flag--;

			if (flag == 0 && mapData.has_boss_flag == 1) {
				//check has roads to boss
				var bossOkFlg = false;
				for (var bossY = bossStartY; bossY < bossStartY + Math.sqrt(bossAllBoxNum) ; bossY++) {
					for (var bossX = bossStartX; bossX < bossStartX + Math.sqrt(bossAllBoxNum) ; bossX++) {
						if (moveArea.indexOf(mapData.map[bossY][bossX]) >= 0) {
							//ok
							bossY = bossStartY + Math.sqrt(bossAllBoxNum);
							bossOkFlg = true;
							break;
						}
					}
				}

				if (bossOkFlg) {
					for (var bossY = bossStartY; bossY < bossStartY + Math.sqrt(bossAllBoxNum) ; bossY++) {
						for (var bossX = bossStartX; bossX < bossStartX + Math.sqrt(bossAllBoxNum) ; bossX++) {
							mapData.map[bossY][bossX] = bossArea[0];
						}
					}
				} else { // not ok
					flag++;
				}
			}
		}

		//set box monster
		var randomMonNum = Math.floor(Math.random() * (mapData.autoMapMaxMonsterNum - mapData.autoMapMinMonsterNum) + mapData.autoMapMinMonsterNum); //10-25
		var randomBoxNum = Math.floor(Math.random() * (mapData.autoMapMaxBoxNum - mapData.autoMapMinBoxNum) + mapData.autoMapMinBoxNum); //5-10

		var okFlg = false;
		var maxTime = 200;
		while ((randomMonNum > 0 || randomBoxNum > 0) && maxTime > 0) {
			maxTime--;
			okFlg = false;
			tempY = Math.floor(Math.random() * mapArrHeight); //0-15
			tempX = Math.floor(Math.random() * mapArrWidth); //0-9


			if (randomBoxNum > 0) {
				if (cantMoveArea.indexOf(mapData.map[tempY][tempX]) >= 0) {
					if (tempX > 0 && moveArea.indexOf(mapData.map[tempY][tempX - 1]) >= 0) {
						mapData.map[tempY][tempX - 1] = monsterArea[Math.floor(Math.random() * monsterArea.length)];
						randomMonNum--;
						okFlg = true;
					}
					if (tempX < mapArrWidth - 1 && moveArea.indexOf(mapData.map[tempY][tempX + 1]) >= 0) {
						mapData.map[tempY][tempX + 1] = monsterArea[Math.floor(Math.random() * monsterArea.length)];
						randomMonNum--;
						okFlg = true;
					}
					if (tempY > 0 && moveArea.indexOf(mapData.map[tempY - 1][tempX]) >= 0) {
						mapData.map[tempY - 1][tempX] = monsterArea[Math.floor(Math.random() * monsterArea.length)];
						randomMonNum--;
						okFlg = true;
					}
					if (tempY < mapArrHeight - 1 && moveArea.indexOf(mapData.map[tempY + 1][tempX]) >= 0) {
						mapData.map[tempY + 1][tempX] = monsterArea[Math.floor(Math.random() * monsterArea.length)];
						randomMonNum--;
						okFlg = true;
					}
					if (okFlg) {
						if (Math.random() < mapData.map_goldBox_per) {
							mapData.map[tempY][tempX] = supriseAreaClosed[1];
						} else {
							mapData.map[tempY][tempX] = supriseAreaClosed[0];
						}
						randomBoxNum--;
					}
				}
			} else {
				if (moveArea.indexOf(mapData.map[tempY][tempX]) >= 0) {
					mapData.map[tempY][tempX] = monsterArea[Math.floor(Math.random() * monsterArea.length)];
					randomMonNum--;
				}
			}

			if (maxTime <= 0 && randomBoxNum > 0) { //奇怪地图防止（没有地方放box）
				randomBoxNum == 0;
				maxTime = 100;
			}
		}
	}

	//set user start position
	while (true) {
		tempY = Math.floor(Math.random() * mapArrHeight); //0-15
		tempX = Math.floor(Math.random() * mapArrWidth); //0-9

		if (moveArea.indexOf(mapData.map[tempY][tempX]) >= 0) {
			userNowY = tempY;
			userNowX = tempX;
			userToY = userNowY;
			userToX = userNowX;

			break;
		}
	}
}

var MyMapScene = cc.Scene.extend({
	onEnter: function () {
		this._super();
		var layer = new MapSceneLayer();
		this.addChild(layer);
		layer.init();
	}
});