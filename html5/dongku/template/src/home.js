var nowBuyIndex = -1;

var HomeSceneLayer = cc.Layer.extend({
	spriteHome: null,
	diffcult_message: null,
	boxSelectedSpr: null,
	homeLeftSprite: null,
	plusWuqi: [],
	plusWuqiSprite: [],
	plusWaitToDel: -1,

	init:function () {
		//////////////////////////////
		// 1. super init first
		this._super();
		nowPageName = "home_top";
		userNowAct = "";
		nowLayer = this;
		topSprite = null;

		//home
		//background
		this.spriteHome = new cc.Sprite(img_home_backGround);
		this.spriteHome.setAnchorPoint(0, 0);
		this.spriteHome.setPosition(0, mapSpaceButtom);
		this.spriteHome.setScale(size.width / this.spriteHome.width, (size.height-mapSpaceTop-mapSpaceButtom) / this.spriteHome.height);
		this.addChild(this.spriteHome, 1);

		//上部xue
		initXuemo();
		//下部menu
		initFooter();

		var homeItem = [];
		//map entrance
		var mapEntranceSprite = new cc.MenuItemImage(img_homeMapEntrance, img_homeMapEntrance, function () { cc.log("mapEntranceSprite"); this.showSelectLevel(); }, this);//gotoMap();},this);
		mapEntranceSprite.setAnchorPoint(0, 1);
		mapEntranceSprite.setPosition(0, this.spriteHome.height);
		mapEntranceSprite.setScale(this.spriteHome.width / mapEntranceSprite.width, this.spriteHome.height/4/mapEntranceSprite.height);
		homeItem.push(mapEntranceSprite);

		var diffcultSelectedSprite = new cc.MenuItemImage(img_homeSelectDiffcult, img_homeSelectDiffcult, function () { this.showSelectDiffcult(); }, this);
		diffcultSelectedSprite.setAnchorPoint(0, 0);
		diffcultSelectedSprite.setPosition(0, this.spriteHome.height - mapEntranceSprite.height);
		diffcultSelectedSprite.setScale(mapEntranceSprite.height / 3 / diffcultSelectedSprite.height);
		//	
		this.diffcult_message = new cc.LabelTTF(getDiffcultType[user.selectDiffcute], "Impact", 25);
		this.diffcult_message.setAnchorPoint(0.5, 0.5);
		this.diffcult_message.setPosition(diffcultSelectedSprite.width / 2, diffcultSelectedSprite.height / 2);
		this.diffcult_message.setColor(cc.color(255, 255, 0, 255));
		diffcultSelectedSprite.addChild(this.diffcult_message, 1);
		homeItem.push(diffcultSelectedSprite);

		//person部
		for (var i=0;i<img_homePerson.length;i++) {
			var spriteTemp = new cc.MenuItemImage(img_homePerson[i], img_homePerson[i],
				function (para1) {
					cc.eventManager.pauseTarget(nowLayer.spriteHome, true);
					switch (para1.getTag()) {
						case 0: this.showPerson0(); break;
						case 1: this.showPerson1(); break;
						case 2: this.showPerson2(); break;
						case 3: this.showPerson3(); break;
						case 4: this.showPerson4(); break;
						case 5: this.showPerson5(); break;
					}
				}, this);
			spriteTemp.setAnchorPoint(0, 1);
			switch(i){
				case 0:spriteTemp.setPosition(0, this.spriteHome.height/4*3);break;
				case 1:spriteTemp.setPosition(this.spriteHome.width/3*2, this.spriteHome.height/4*3);break;
				case 2:spriteTemp.setPosition(0, this.spriteHome.height/4*2);break;
				case 3:spriteTemp.setPosition(this.spriteHome.width/3*2, this.spriteHome.height/4*2);break;
				case 4:spriteTemp.setPosition(0, this.spriteHome.height/4*1);break;
				case 5:spriteTemp.setPosition(this.spriteHome.width/3*2, this.spriteHome.height/4*1);break;
			}
			spriteTemp.setScale(this.spriteHome.width / 3 / spriteTemp.width, this.spriteHome.height/4 / spriteTemp.height);
			spriteTemp.setTag(i);
			homeItem.push(spriteTemp);
		}
		
		var menu = new cc.Menu(homeItem);
		menu.setPosition(0, 0);
		this.spriteHome.addChild(menu, 1);
	},
	showSelectDiffcult: function () {
		var selectDiffcultItem = [];

		//background
		var selectDiffcultSpriteBG = new cc.Sprite(img_map_backGround);
		selectDiffcultSpriteBG.setAnchorPoint(0, 0);
		selectDiffcultSpriteBG.setPosition(0, 0);
		selectDiffcultSpriteBG.setScale(size.width / selectDiffcultSpriteBG.width, size.height / selectDiffcultSpriteBG.height);
		this.addChild(selectDiffcultSpriteBG, 8);

		var closeSpr = new cc.MenuItemImage(s_CloseNormal, s_CloseSelected, function () {
			selectDiffcultSpriteBG.removeAllChildren();
			this.removeChild(selectDiffcultSpriteBG, true);
		}, this);
		closeSpr.setAnchorPoint(1, 1);
		closeSpr.setPosition(selectDiffcultSpriteBG.width, selectDiffcultSpriteBG.height);
		selectDiffcultItem.push(closeSpr);

		//list部
		var colorTemp = cc.color(0, 0, 255, 255);
		for (var i = 0; i < diffcultNumber; i++) {
			var spriteTemp = new cc.MenuItemImage(img_homeSelectDiffcultPage, img_homeSelectDiffcultPage, function (para1) {
				cc.log("select");
				if (para1.getTag() <= user.maxDiffcute) {
					user.selectDiffcute = para1.getTag();
					this.diffcult_message.setString(getDiffcultType[user.selectDiffcute]);
					selectDiffcultSpriteBG.removeAllChildren();
					this.removeChild(selectDiffcultSpriteBG, true);
				}
			}, this);
			spriteTemp.setAnchorPoint(0, 1);
			spriteTemp.setPosition(0, selectDiffcultSpriteBG.height - spriteTemp.height * i - closeSpr.height);
			spriteTemp.setScale(selectDiffcultSpriteBG.width / spriteTemp.width, (selectDiffcultSpriteBG.height-80)/ 4 / spriteTemp.height);
			spriteTemp.setTag(i);
			if (i > user.maxDiffcute) {
				colorTemp = cc.color(100, 100, 100, 255);
			}
			//wenzi
			var diffcult_message_title = new cc.LabelTTF(getDiffcultType[i], "Impact", 40);
			diffcult_message_title.setAnchorPoint(0.5, 0.5);
			diffcult_message_title.setPosition(spriteTemp.width / 2, spriteTemp.height - diffcult_message_title.height);
			diffcult_message_title.setColor(colorTemp);
			spriteTemp.addChild(diffcult_message_title, 1);
			//wenzi detail
			var diffcult_message_detail = new cc.LabelTTF("sfa\naf\na", "Impact", 30);
			diffcult_message_detail.setAnchorPoint(0, 1);
			diffcult_message_detail.setPosition(80, spriteTemp.height - diffcult_message_title.height * 2);
			diffcult_message_detail.setColor(colorTemp);
			spriteTemp.addChild(diffcult_message_detail, 1);

			selectDiffcultItem.push(spriteTemp);
		}

		var menu = new cc.Menu(selectDiffcultItem);
		menu.setPosition(0, 0);
		selectDiffcultSpriteBG.addChild(menu, 1);
	},
	showSelectLevel: function () {
		var selectLevelItem = [];

		//background
		var selectLevelSpriteBG = new cc.Sprite(img_map_backGround);
		selectLevelSpriteBG.setAnchorPoint(0, 0);
		selectLevelSpriteBG.setPosition(0, 0);
		selectLevelSpriteBG.setScale(size.width / selectLevelSpriteBG.width, size.height / selectLevelSpriteBG.height);
		this.addChild(selectLevelSpriteBG, 8);

		var closeSpr = new cc.MenuItemImage(s_CloseNormal, s_CloseSelected, function () {
			selectLevelSpriteBG.removeAllChildren();
			this.removeChild(selectLevelSpriteBG, true);
		}, this);
		closeSpr.setAnchorPoint(1, 1);
		closeSpr.setPosition(selectLevelSpriteBG.width, selectLevelSpriteBG.height);
		selectLevelItem.push(closeSpr);

		//level wenzi
		var titleMsg = new cc.LabelTTF("选择开始区数", "Impact", 50);
		titleMsg.setAnchorPoint(0.5, 0.5);
		titleMsg.setPosition(selectLevelSpriteBG.width / 2, selectLevelSpriteBG.height/12*11);
		selectLevelSpriteBG.addChild(titleMsg, 1);

		//list部
		var colorTemp = cc.color(0, 0, 0, 255);
		for (var i = 0; i < 10; i++) {
			var spriteTemp = new cc.MenuItemImage(img_homeSelectLevelBtn, img_homeSelectLevelBtn, function (para1) {
				if (user.selectDiffcute < user.maxDiffcute || (user.selectDiffcute == user.maxDiffcute && para1.getTag() <= user.maxLevel)) {
					user.selectLevel = para1.getTag();
					gotoMap();
				}
			}, this);
			spriteTemp.setAnchorPoint(0.5, 0.5);
			spriteTemp.setPosition(selectLevelSpriteBG.width / 2, selectLevelSpriteBG.height/12*(10 - i));
			spriteTemp.setScale(selectLevelSpriteBG.width / 3 / spriteTemp.width);
			spriteTemp.setTag(i*10 + 1);
			if (user.selectDiffcute < user.maxDiffcute || (user.selectDiffcute == user.maxDiffcute && (i*10+1) <= user.maxLevel)) {
				colorTemp = cc.color(0, 0, 0, 255);
			} else {
				colorTemp = cc.color(100, 100, 100, 255);
			}
			//level wenzi
			var level_number = new cc.LabelTTF("第" + (i * 10 + 1) + "区", "Impact", 30);
			level_number.setAnchorPoint(0.5, 0.5);
			level_number.setPosition(spriteTemp.width / 2, spriteTemp.height / 2);
			level_number.setColor(colorTemp);
			spriteTemp.addChild(level_number, 1);

			selectLevelItem.push(spriteTemp);
		}

		var menu = new cc.Menu(selectLevelItem);
		menu.setPosition(0, 0);
		selectLevelSpriteBG.addChild(menu, 1);
	},
	showPerson0: function () { //yaoshui
	    userNowAct = "package";
	    nowPageName = "home_person0";
		//background
		spritePackage = new cc.Sprite(img_package_backGround);
		spritePackage.setAnchorPoint(0, 1);
		spritePackage.setPosition(0, size.height - mapSpaceTop);
		spritePackage.setScale(size.width / spritePackage.width, (size.height - mapSpaceTop - mapSpaceButtom) / spritePackage.height);
		this.addChild(spritePackage, 8);

/*		var closeSpr = new cc.MenuItemImage(s_CloseNormal, s_CloseSelected, function () {
			userNowAct = "";
			spritePackage.removeAllChildren();
			nowLayer.removeChild(spritePackage, true);
		}, this);
		closeSpr.setAnchorPoint(1, 1);
		closeSpr.setPosition(spritePackage.width, spritePackage.height);
		var menu = new cc.Menu(closeSpr);
		spritePackage.addChild(menu, 1);*/

		//list部
		initButtomPackageChangePage(spritePackage);
		initButtomPackageBox();
		
		initHomeLeft(0);


	},
	showPerson1: function () { //renwu
	    nowPageName = "home_person1";
	},
	showPerson2: function () { //wuqi
	    userNowAct = "package";
	    nowPageName = "home_person2";
		//background
		spritePackage = new cc.Sprite(img_package_backGround);
		spritePackage.setAnchorPoint(0, 1);
		spritePackage.setPosition(0, size.height - mapSpaceTop);
		spritePackage.setScale(size.width / spritePackage.width, (size.height - mapSpaceTop - mapSpaceButtom) / spritePackage.height);
		this.addChild(spritePackage, 8);

/*		var closeSpr = new cc.MenuItemImage(s_CloseNormal, s_CloseSelected, function () {
			userNowAct = "";
			spritePackage.removeAllChildren(true);
			nowLayer.removeChild(spritePackage, true);
			spritePackage = null;
		}, this);
		closeSpr.setAnchorPoint(1, 1);
		closeSpr.setPosition(spritePackage.width, spritePackage.height);
		var menu = new cc.Menu(closeSpr);
		spritePackage.addChild(menu, 1);*/

		//list部
		initButtomPackageChangePage(spritePackage);
		initButtomPackageBox();
		
		initHomeLeft(2);

	},
	showPerson3: function () { //cangku
		userNowAct = "package";
		nowPageName = "home_person3";
		//background
		spritePackage = new cc.Sprite(img_package_backGround);
		spritePackage.setAnchorPoint(0, 1);
		spritePackage.setPosition(0, size.height - mapSpaceTop);
		spritePackage.setScale(size.width / spritePackage.width, (size.height - mapSpaceTop - mapSpaceButtom) / spritePackage.height);
		this.addChild(spritePackage, 8);

		//list部
		initButtomPackageChangePage(spritePackage);
		initButtomPackageBox();

		initHomeLeft(3);
	},
	showPerson4: function () { //wuqiduanzao
		userNowAct = "package";
		nowPageName = "home_person4";
		//background
		spritePackage = new cc.Sprite(img_package_backGround);
		spritePackage.setAnchorPoint(0, 1);
		spritePackage.setPosition(0, size.height - mapSpaceTop);
		spritePackage.setScale(size.width / spritePackage.width, (size.height - mapSpaceTop - mapSpaceButtom) / spritePackage.height);
		this.addChild(spritePackage, 8);

		//list部
		packagePageNow = img_package_num - 1;
		initButtomPackageChangePage(spritePackage);
		initButtomPackageBox();

		initHomeLeft(4);
	},
	showPerson5: function () { //xingyun
		userNowAct = "package";
		nowPageName = "home_person5";
		//background
		spritePackage = new cc.Sprite(img_package_backGround);
		spritePackage.setAnchorPoint(0, 1);
		spritePackage.setPosition(0, size.height - mapSpaceTop);
		spritePackage.setScale(size.width / spritePackage.width, (size.height - mapSpaceTop - mapSpaceButtom) / spritePackage.height);
		this.addChild(spritePackage, 8);

		//list部
		packagePageNow = img_package_num - 1;
		initButtomPackageChangePage(spritePackage);
		initButtomPackageBox();

		initHomeLeft(5);
	}
});

function saveUser() {
	localStorage.setItem('userNowStorage', JSON.stringify(user));
}

function reloadUser() {
	user = JSON.parse(localStorage.getItem('userNowStorage'));
}

function saveOldUser() {
	localStorage.setItem('userOldStorage', JSON.stringify(user));
}
function loadOldUser() {
	user = JSON.parse(localStorage.getItem('userOldStorage'));
}

function levelUpUser() {
	//TODO 等级的对应
	user.baseAttack = user.baseAttack * 1.03;
	user.baseDefect = user.baseDefect * 1.01;
	user.baseXue = user.baseXue * 1.03;
	user.baseMo = user.baseMo * 1.01;

	user.leftPoint += 2;

	if (user.level <= 10) {
		user.jingyanAll = user.jingyanAll * 1.5;
	} else if (user.level <= 20) {
		user.jingyanAll = user.jingyanAll * 1.3;
	} else if (user.level <= 30) {
		user.jingyanAll = user.jingyanAll * 1.2;
	} else if (user.level <= 40) {
		user.jingyanAll = user.jingyanAll * 1.1;
	} else if (user.level <= 50) {
		user.jingyanAll = user.jingyanAll * 1.05;
	} else {
		user.jingyanAll = user.jingyanAll * 1.2;
	}
}

function renewUser(){
	user.property = [user.baseAttack,user.baseDefect,user.baseXue,user.baseMo,0,0,0,0,0,0,0,0,0,0];

	var map = new Map();
	for (var i=0;i<user.wuqiCarry[nowUserWuqiPage].length;i++){
		if (user.wuqiCarry[nowUserWuqiPage][i] != "") {
			var wuqiInfo = getWuqiInfo(user.wuqiCarry[nowUserWuqiPage][i].id);
			for (var j = 0; j < user.wuqiCarry[nowUserWuqiPage][i].trueProperty.length; j++) {
				if (user.wuqiCarry[nowUserWuqiPage][i].trueProperty[j] > 0) {
					user.property[j] += user.wuqiCarry[nowUserWuqiPage][i].trueProperty[j];
				}
			}

			for (var j=0;j<user.wuqiCarry[nowUserWuqiPage][i].special_type.length;j++) {
				if (user.wuqiCarry[nowUserWuqiPage][i].special_type[j] > -1) {
					user.property[user.wuqiCarry[nowUserWuqiPage][i].special_type[j]]+=user.wuqiCarry[nowUserWuqiPage][i].special_value[j];
				}
			}
			for (var j=0;j<user.wuqiCarry[nowUserWuqiPage][i].add_type.length;j++) {
				if (user.wuqiCarry[nowUserWuqiPage][i].add_type[j] > -1) {
					user.property[user.wuqiCarry[nowUserWuqiPage][i].add_type[j]]+=user.wuqiCarry[nowUserWuqiPage][i].add_value[j];
				}
			}
			if (wuqiInfo.groupNum >= 0) {
				if (map.has(wuqiInfo.groupNum)) {
					map.set(wuqiInfo.groupNum,map.get(wuqiInfo.groupNum)+1);
				} else {
					map.set(wuqiInfo.groupNum,1);
				}
//				groupCount[nowUserWuqiPage][wuqiInfo.type] = wuqiInfo.groupNum;
//				groupCountStr += wuqiInfo.groupNum + ",";
			}
		}
	}

	for (var mapElement of map) {
		if (mapElement[1] >= 6) {
			var wuqiGroupInfo = getWuqiGroupInfo(mapElement[0]);
			for (var i=0;i<3;i++) {
				user.property[wuqiGroupInfo.have246_type[i]] += wuqiGroupInfo.have246_value[i];
			}
			break;
		} else if (mapElement[1] >= 4) {
			var wuqiGroupInfo = getWuqiGroupInfo(mapElement[0]);
			for (var i=0;i<2;i++) {
				user.property[wuqiGroupInfo.have246_type[i]] += wuqiGroupInfo.have246_value[i];
			}
			break;
		} else if (mapElement[1] >= 2) {
			var wuqiGroupInfo = getWuqiGroupInfo(mapElement[0]);
			var wuqiGroupInfo = getWuqiGroupInfo(mapElement[0]);
			for (var i=0;i<1;i++) {
				user.property[wuqiGroupInfo.have246_type[i]] += wuqiGroupInfo.have246_value[i];
			}
			break;
		}
	}

	//addPoint
	for (var i = 0; i < user.addPoint.length; i++) {
		if (getPlusPropertyInfo[i] < 1) {
			user.property[i] = Math.round(user.property[i] * (1 + getPlusPropertyInfo[i] * user.addPoint[i]));
		} else {
			user.property[i] = user.property[i] + getPlusPropertyInfo[i] * user.addPoint[i];
		}
	}

	//baoji shanbi max value
	if (user.property[baoNo] > baoMaxValue) {
		user.property[baoNo] = baoMaxValue;
	}
	if (user.property[shanNo] > shanMaxValue) {
		user.property[shanNo] = shanMaxValue;
	}

	saveUser();
}

function getUserTrueAttack(){
	var monTrueFang = monster.property[fangNo] - user.property[chuanNo];
	if (monTrueFang < 0) {
		monTrueFang = 0;
	}
	var attack=user.property[gongNo] - monTrueFang;
	if (attack < 1) {
		attack = user.baseAttack;
	}

	attack = attack * (1 + user.property[monster.type] / 100 + user.property[allNo] / 100);

	//baoji
	if (Math.random() < user.property[baoNo]/100) {
		attack = attack * baoPlus;
	}
	attack = Math.ceil(attack);
	return attack;
}
function getMonsterTrueAttack(){
	var attack = monster.property[gongNo] - user.property[fangNo];
	if (attack < 1) {
		attack = 1;
	}
	//shanbi
	if (Math.random() < user.property[shanNo] / 100) {
		attack = 0;
	}
	attack = Math.ceil(attack);
	return attack;
}
//const countArr = (arr, value) => arr.reduce((a, v) => v === value ? a + 1 : a + 0, 0);

//user area
function initHomeLeft(type) {

	if (nowLayer.homeLeftSprite) {
		nowLayer.homeLeftSprite.removeAllChildren(true);
	} else {
		nowBuyIndex = -1;
	}


	if (type == 0 || type == 2) { //yaoshui
		nowLayer.homeLeftSprite = new cc.Sprite(img_homeLeft_BG);
		nowLayer.homeLeftSprite.setAnchorPoint(0, 0);
		nowLayer.homeLeftSprite.setPosition(0, packageBoxArea.height + packageChangeBK.height);
		nowLayer.homeLeftSprite.setScale(spritePackage.width / 2 / nowLayer.homeLeftSprite.width);
		spritePackage.addChild(nowLayer.homeLeftSprite, 1);

		var boxIndex = 0;
		for (var y = 0; y < 4; y++) {
			for (var x = 0; x < 4; x++) {
				var packageBoxTemp;
				var packageBoxBGTemp;
				
				addMonoSprite(nowLayer.homeLeftSprite,
					img_package_boxBG_empty,
					packageBoxSize * x + packageBoxSpaceLeftRight/2 + packageBoxSize / 2,
					nowLayer.homeLeftSprite.height - packageBoxSize * y - packageBoxSpaceTop - packageBoxSize / 2,
					1,
					"",
					0
				);

				if (boxIndex < user.person_items[type].length) {
					var imgPath = "";
					var colorType=0;
					if (type == 0) {
						imgPath = yaoshuiFolder + user.person_items[type][boxIndex] + ".png";
					} else if (type == 2) {
						imgPath = wuqiFolder + user.person_items[type][boxIndex].id + ".png";
						colorType = user.person_items[type][boxIndex].special_type.length;
					}

					packageBoxTemp = addMonoSprite(
						nowLayer.homeLeftSprite,
						imgPath,
						packageBoxSize * x + packageBoxSpaceLeftRight/2 + packageBoxSize / 2,
						nowLayer.homeLeftSprite.height - packageBoxSize * y - packageBoxSpaceTop - packageBoxSize / 2,
						2,
						"person" + type+ "_" + boxIndex,
						colorType
					);
					cc.eventManager.addListener(listener_person_buy.clone(), packageBoxTemp);

					//个数
					showYaoshuiNum(packageBoxTemp, user.person_items_number[type][boxIndex]);
				}
				boxIndex++;
			}
		}

		//显示 购买 按钮
		var buySpr = new cc.MenuItemImage(img_buy, img_buy_clicked, function () {
			if (nowBuyIndex != -1 && user.person_items_number[type][nowBuyIndex] > 0) {
				
				if (type == 0) {
					if (userGetItem("yaoshui", user.person_items[type][nowBuyIndex], 1)) {
						user.person_items_number[type][nowBuyIndex]--;
						initHomeLeft(type);
						initButtomPackageBox();
						initFooter();
						saveUser();
					}
				} else if (type == 2) {
					if (userGetItem("wuqi", user.person_items[type][nowBuyIndex], 1)) {
						user.person_items_number[type][nowBuyIndex]--;
						initHomeLeft(type);
						initButtomPackageBox();
						initFooter();
						saveUser();
					}
				}
			}
		}, this);
		buySpr.setAnchorPoint(1, 0);
		buySpr.setPosition(0, 0);
		buySpr.setScale((nowLayer.homeLeftSprite.height - packageBoxSpaceTop - packageBoxSize*4) / buySpr.height);
		var menu = new cc.Menu(buySpr);
		menu.setPosition(nowLayer.homeLeftSprite.width, 0);
		nowLayer.homeLeftSprite.addChild(menu, 5);
	} else if (type == 3) { //cangku
		nowLayer.homeLeftSprite = new cc.Sprite(img_home_package_bg);
		nowLayer.homeLeftSprite.setAnchorPoint(0, 0);
		nowLayer.homeLeftSprite.setPosition(0, packageBoxArea.height + packageChangeBK.height);
		nowLayer.homeLeftSprite.setScale(spritePackage.width / 2 / nowLayer.homeLeftSprite.width);
		spritePackage.addChild(nowLayer.homeLeftSprite, 1);

		
		var boxIndex = 0;
		var lockBoxIndex = 0;
		for (var y = 0; y < 8; y++) {
			for (var x = 0; x < 4; x++) {
				addMonoSprite(nowLayer.homeLeftSprite,
					img_package_boxBG_empty,
					packageBoxSize * x + packageBoxSpaceLeftRight/2 + packageBoxSize / 2,
					nowLayer.homeLeftSprite.height - packageBoxSize * y - packageBoxSpaceTop/2 - packageBoxSize / 2,
					5,
					0
				);

				var packageBoxTemp = null;
				if (boxIndex < user.openedPackageHomeNum) { //当前盒子index 小于 解锁盒子数
					if (boxIndex < user.wuqiPackageHome.length) { //当前盒子index小于wuqi盒子数
						packageBoxTemp = addMonoSprite(
							nowLayer.homeLeftSprite,
							wuqiFolder + user.wuqiPackageHome[boxIndex].id + ".png",
							packageBoxSize * x + packageBoxSpaceLeftRight/2 + packageBoxSize / 2,
							nowLayer.homeLeftSprite.height - packageBoxSize * y - packageBoxSpaceTop/2 - packageBoxSize / 2,
							5,
							"wuqiPackageHome_" + boxIndex,
							user.wuqiPackageHome[boxIndex].special_type.length
						);
						cc.eventManager.addListener(listener_package_boxMove.clone(), packageBoxTemp);

						if (user.wuqiPackageHome[boxIndex].plusLevel > 0) {
							showWuqiPlusNum(packageBoxTemp,user.wuqiPackageHome[boxIndex].plusLevel);
						}

					} else if (boxIndex < (user.wuqiPackageHome.length + user.yaoshuiPackageHome.length)) { //当前盒子index 小于yaoshui加wuqi数
						packageBoxTemp = addMonoSprite(
							nowLayer.homeLeftSprite,
							yaoshuiFolder + user.yaoshuiPackageHome[boxIndex - user.wuqiPackageHome.length] + ".png",
							packageBoxSize * x + packageBoxSpaceLeftRight/2 + packageBoxSize / 2,
							nowLayer.homeLeftSprite.height - packageBoxSize * y - packageBoxSpaceTop/2 - packageBoxSize / 2,
							5,
							"yaoshuiPackageHome_" + (boxIndex - user.wuqiPackageHome.length),
							0
						);
						cc.eventManager.addListener(listener_package_boxMove.clone(), packageBoxTemp);

						showYaoshuiNum(packageBoxTemp, user.yaoshuiNumHome[boxIndex - user.wuqiPackageHome.length]);
					}
				} else { //被锁的
					lockBoxIndex++;
					packageBoxTemp = addMonoSprite(
						nowLayer.homeLeftSprite,
						img_package_boxBG_locked,
						packageBoxSize * x + packageBoxSpaceLeftRight/2 + packageBoxSize / 2,
						nowLayer.homeLeftSprite.height - packageBoxSize * y - packageBoxSpaceTop/2 - packageBoxSize / 2,
						5,
						"lockPackageHome_" + lockBoxIndex,
						0
					);
					cc.eventManager.addListener(listener_unlockBox.clone(), packageBoxTemp);
				}

				boxIndex++;
			}
		}
	} else if (type == 4 || type == 5) { //plus or addSpecial(shuxinghebin)
		nowLayer.homeLeftSprite = new cc.Sprite(img_homeLeft_BG);
		nowLayer.homeLeftSprite.setAnchorPoint(0, 0);
		nowLayer.homeLeftSprite.setPosition(0, packageBoxArea.height + packageChangeBK.height);
		nowLayer.homeLeftSprite.setScale(spritePackage.width / 2 / nowLayer.homeLeftSprite.width);
		spritePackage.addChild(nowLayer.homeLeftSprite, 1);

		//main
		var setOkFlg = [];
		for (var i=0;i<2;i++) {
			nowLayer.plusWuqiSprite[i] = addMonoSprite(
				nowLayer.homeLeftSprite,
				img_package_boxBG_empty,
				nowLayer.homeLeftSprite.width / 3 *(i+1),
				nowLayer.homeLeftSprite.height - 80,
				1,
				"",
				0
			);

			if (nowLayer.plusWuqi != null && nowLayer.plusWuqi[i]) {
				nowLayer.plusWuqiSprite[i] = addMonoSprite(
					nowLayer.homeLeftSprite,
					wuqiFolder + nowLayer.plusWuqi[i].id + ".png",
					nowLayer.homeLeftSprite.width / 3 *(i+1),
					nowLayer.homeLeftSprite.height - 80,
					1,
					"plus_" + i,
					nowLayer.plusWuqi[i].special_type.length
				);
				showWuqiPlusNum(nowLayer.plusWuqiSprite[i], nowLayer.plusWuqi[i].plusLevel);

				setOkFlg[i] = true;
				cc.eventManager.addListener(listener_package_boxMove.clone(), nowLayer.plusWuqiSprite[i]);
			}
		}

		if (setOkFlg[0] && setOkFlg[1]) {
			var toWuqiInfo = getWuqiInfo(nowLayer.plusWuqi[0].id);
			var fromWuqiInfo = getWuqiInfo(nowLayer.plusWuqi[1].id);

			var per = 0;
			var price = 0;
			var perLabel;
			if (type == 4) {
				per = Math.round((1 - (nowLayer.plusWuqi[0].plusLevel - (fromWuqiInfo.mapLevel - toWuqiInfo.mapLevel)) * 0.1) * 100) / 100;
				if (per > 1) {
					per = 1;
				}

				price = Math.round(1000 * (1 + (toWuqiInfo.mapLevel - 1) / 10 + nowLayer.plusWuqi[0].plusLevel * 0.1));
				perLabel = new cc.LabelTTF("chenggonglv:" + per * 100 + "%\njiage:" + price, "Impact", 20);
				perLabel.setAnchorPoint(0, 0);
				perLabel.setPosition(50, nowLayer.homeLeftSprite.height - 300);
				perLabel.setColor(cc.color(100, 100, 100, 255));
				nowLayer.homeLeftSprite.addChild(perLabel, 2);

				//显示 plus 按钮
				var plusSpr = new cc.MenuItemImage(img_plus, img_plus_clicked, function () {
					//todo
					if (user.jinbi >= price && plusWaitToDel >= 0 && plusWaitToDel < user.wuqiPackage.length) { //TODO per price
						if (Math.random() < per) {
							nowLayer.plusWuqi[0].plusLevel++;
							var value = 0;
							for (var i = 0; i < nowLayer.plusWuqi[0].trueProperty.length; i++) {
								value = Math.floor(nowLayer.plusWuqi[0].trueProperty[i] * 1.05);
								if (value == nowLayer.plusWuqi[0].trueProperty[i]) {
									value = nowLayer.plusWuqi[0].trueProperty[i] + 1;
								}
								nowLayer.plusWuqi[0].trueProperty[i] = value;
							}
							for (var i = 0; i < nowLayer.plusWuqi[0].special_value.length; i++) {
								value = Math.floor(nowLayer.plusWuqi[0].special_value[i] * 1.05);
								if (value == nowLayer.plusWuqi[0].special_value[i]) {
									value = nowLayer.plusWuqi[0].special_value[i] + 1;
								}
								nowLayer.plusWuqi[0].special_value[i] = value;
							}
							for (var i = 0; i < nowLayer.plusWuqi[0].add_value.length; i++) {
								value = Math.floor(nowLayer.plusWuqi[0].add_value[i] * 1.05);
								if (value == nowLayer.plusWuqi[0].add_value[i]) {
									value = nowLayer.plusWuqi[0].add_value[i] + 1;
								}
								nowLayer.plusWuqi[0].add_value[i] = value;
							}
						} else {
							//TODO  show shibai
						}
						user.jinbi = user.jinbi - price;
						nowLayer.plusWuqi[1] = null;
						user.wuqiPackage.splice(plusWaitToDel, 1);
						plusWaitToDel = -1;
						initHomeLeft(4);
						initButtomPackageBox();
						initXuemo();
						renewUser();
					} else {
						//TODO  show jinbi buzu
					}
				}, this);
				plusSpr.setAnchorPoint(0.5, 0.5);
				plusSpr.setPosition(nowLayer.homeLeftSprite.width / 2, nowLayer.homeLeftSprite.height - 370);
				plusSpr.setScale(packageBoxSize / plusSpr.height);
				var menu = new cc.Menu(plusSpr);
				menu.setPosition(0, 0);
				nowLayer.homeLeftSprite.addChild(menu, 2);
			} else if (type == 5) {
				per = 0.2 + 0.1 * (fromWuqiInfo.mapLevel - toWuqiInfo.mapLevel);
				if (nowLayer.plusWuqi[1].special_type.length ==0) {
					per = 0;
				} else if (per > (1.0 / nowLayer.plusWuqi[1].special_type.length)) {
					per = 1.0 / nowLayer.plusWuqi[1].special_type.length;
				} else if (per <= 0.0) {
					per = 0;
				}
				per = Math.round(per * 100) / 100;

				price = Math.round(1000 * (1 + (toWuqiInfo.mapLevel - 1) / 10 + nowLayer.plusWuqi[0].special_type.length * 0.2));
				perLabel = new cc.LabelTTF("每项属性转移几率:" + per * 100 + "%\njiage:" + price, "Impact", 20);
				perLabel.setAnchorPoint(0, 0);
				perLabel.setPosition(50, nowLayer.homeLeftSprite.height - 300);
				perLabel.setColor(cc.color(100, 100, 100, 255));
				nowLayer.homeLeftSprite.addChild(perLabel, 2);

				//显示 hebin 按钮
				var addProSpr = new cc.MenuItemImage(img_addMagic, img_addMagic_clicked, function () {
					//todo
					if (user.jinbi >= price && plusWaitToDel >= 0 && plusWaitToDel < user.wuqiPackage.length) { //TODO per price
						var fromProIndex = -1; //被转移的属性index
						if (per > 0) {
							fromProIndex = Math.floor(Math.random() / per);
							if (fromProIndex >= nowLayer.plusWuqi[1].special_type.length) {
								fromProIndex = -1; //lose
							}
						}

						if (fromProIndex >= 0) {
							nowLayer.plusWuqi[0].add_type[0] = nowLayer.plusWuqi[1].special_type[fromProIndex];
							nowLayer.plusWuqi[0].add_value[0] = nowLayer.plusWuqi[1].special_value[fromProIndex];
						} else {
							//TODO  show shibai
						}
						user.jinbi = user.jinbi - price;
						nowLayer.plusWuqi[1] = null;
						user.wuqiPackage.splice(plusWaitToDel, 1);
						plusWaitToDel = -1;
						initHomeLeft(4);
						initButtomPackageBox();
						initXuemo();
						renewUser();
					} else {
						//TODO  show jinbi buzu
					}
				}, this);
				addProSpr.setAnchorPoint(0.5, 0.5);
				addProSpr.setPosition(nowLayer.homeLeftSprite.width / 2, nowLayer.homeLeftSprite.height - 370);
				addProSpr.setScale(packageBoxSize / addProSpr.height);
				var menu = new cc.Menu(addProSpr);
				menu.setPosition(0, 0);
				nowLayer.homeLeftSprite.addChild(menu, 2);
			}
		}
	} else {

	}
}

//buy_listener
var listener_person_buy = cc.EventListener.create({
	event: cc.EventListener.TOUCH_ONE_BY_ONE,
	swallowTouches: true,						// 设置是否吞没事件，在 onTouchBegan 方法返回 true 时吞掉事件，不再向下传递。
	onTouchBegan: function (touch, event) {		//实现 onTouchBegan 事件处理回调函数
		cc.log("onTouchBegan_buy");

		//判断点击区域
		var target = event.getCurrentTarget();
		var pos = target.convertToNodeSpace(touch.getLocation());
		if(!cc.rectContainsPoint(target.getTextureRect(), pos))return false;

		var nodeTempTag =this._node.getTag().split("_");
		if (nodeTempTag == null || nodeTempTag.length == 0) { //空box判断
			return false;
		}

		if (userNowAct == "package") {
			if (nodeTempTag[0] == "person0") {
				nowBuyIndex = parseInt(nodeTempTag[1]);
				initMiddleRight("yaoshui", user.person_items[0][nodeTempTag[1]], this._node);
			} else if (nodeTempTag[0] == "person2") {
				nowBuyIndex = parseInt(nodeTempTag[1]);
				initMiddleRight("wuqi", user.person_items[2][nodeTempTag[1]], this._node);
			}

			return true;
		}
	},
	onTouchMoved: function (touch, event) {			//实现onTouchMoved事件处理回调函数, 触摸移动时触发
	},
	onTouchEnded: function (touch, event) {			// 实现onTouchEnded事件处理回调函数
	}
});


gotoMap = function () {
	saveUser();
	saveOldUser();

	mapData = getJson("res/map/" + user.selectLevel + "_" + user.selectDiffcute + ".json");

	autoMapCreat();

	var loadResource = resourceArrSet_map();
	var myMapScene = new MyMapScene();
	var tranScene = cc.TransitionMoveInL.create(0.5, myMapScene);
	cc.LoaderScene.preload(loadResource, function () {
		cc.director.runScene(tranScene);
	}, this);
}

getJson=function (path) {
	var txt = cc.loader._loadTxtSync(path);
	return JSON.parse(txt);
}


function resourceArrSet_map() {
	var loadResource = [];
	var dataTemp;
	for (var i = 0; i < mapData.map_monster_json.length; i++) {
		dataTemp = JSON.parse(cc.loader._loadTxtSync(mapData.map_monster_json[i]));
		loadResource.push(dataTemp.pic);
		loadResource.push(dataTemp.pic_att);
	}
	for (var i = 0; i < mapData.map_boss_json.length; i++) {
		dataTemp = JSON.parse(cc.loader._loadTxtSync(mapData.map_boss_json[i]));
		loadResource.push(dataTemp.pic);
		loadResource.push(dataTemp.pic_att);
	}
	for (var i = 0; i < mapData.map_road_png.length; i++) {
		if (loadResource.indexOf(mapData.map_road_png[i]) < 0) {
			loadResource.push(mapData.map_road_png[i]);
		}
	}
	for (var i = 0; i < mapData.map_wall_png.length; i++) {
		if (loadResource.indexOf(mapData.map_wall_png[i]) < 0) {
			loadResource.push(mapData.map_wall_png[i]);
		}
	}
	for (var i = 0; i < mapData.map_outDoor_png.length; i++) {
		if (loadResource.indexOf(mapData.map_outDoor_png[i]) < 0) {
			loadResource.push(mapData.map_outDoor_png[i]);
		}
	}

	for (var i = 0; i < mapData.map_supriseBox_closed_png.length; i++) {
		if (loadResource.indexOf(mapData.map_supriseBox_closed_png[i]) < 0) {
			loadResource.push(mapData.map_supriseBox_closed_png[i]);
		}
	}
	for (var i = 0; i < mapData.map_supriseBox_opened_png.length; i++) {
		if (loadResource.indexOf(mapData.map_supriseBox_opened_png[i]) < 0) {
			loadResource.push(mapData.map_supriseBox_opened_png[i]);
		}
	}
	for (var i = 0; i < mapData.dropYaoshui.length; i++) {
		if (loadResource.indexOf(yaoshuiFolder + mapData.dropYaoshui[i] + ".png") < 0) {
			loadResource.push(yaoshuiFolder + mapData.dropYaoshui[i] + ".png");
		}
	}
	for (var i = 0; i < mapData.dropWuqi.length; i++) {
		if (loadResource.indexOf(wuqiFolder + mapData.dropWuqi[i] + ".png") < 0) {
			loadResource.push(wuqiFolder + mapData.dropWuqi[i] + ".png");
		}
	}
/*	for (var i = 0; i < user.wuqiPackage.length; i++) {
		if (loadResource.indexOf(wuqiFolder + user.wuqiPackage[i].id + ".png") < 0) {
			loadResource.push(wuqiFolder + user.wuqiPackage[i].id + ".png");
		}
	}
	for (var i = 0; i < user.yaoshuiPackage.length; i++) {
		if (loadResource.indexOf(yaoshuiFolder + user.yaoshuiPackage[i].id + ".png") < 0) {
			loadResource.push(yaoshuiFolder + user.yaoshuiPackage[i].id + ".png");
		}
	}*/
	return loadResource;
}

function resourceArrSet_user() {
	var loadResource = [];
	var dataTemp;
	for (var i = 0; i < user.wuqiPackage.length; i++) {
		if (loadResource.indexOf(wuqiFolder + user.wuqiPackage[i].id + ".png") < 0) {
			loadResource.push(wuqiFolder + user.wuqiPackage[i].id + ".png");
		}
	}
	for (var page = 0; page < user.wuqiCarry.length; page++) {
		for (var i = 0; i < user.wuqiCarry[page].length; i++) {
			if (user.wuqiCarry[page][i] != ""){
				if (loadResource.indexOf(wuqiFolder + user.wuqiCarry[page][i].id + ".png") < 0) {
					loadResource.push(wuqiFolder + user.wuqiCarry[page][i].id + ".png");
				}
			}
		}
	}
	for (var i = 0; i < user.yaoshuiPackage.length; i++) {
		if (loadResource.indexOf(yaoshuiFolder + user.yaoshuiPackage[i] + ".png") < 0) {
			loadResource.push(yaoshuiFolder + user.yaoshuiPackage[i] + ".png");
		}
	}
	for (var i = 0; i < user.person_items.length; i++) {
		for (var j = 0; j < user.person_items[i].length; j++) {
			if (i == 0) {
				if (loadResource.indexOf(yaoshuiFolder + user.person_items[i][j] + ".png") < 0) {
					loadResource.push(yaoshuiFolder + user.person_items[i][j] + ".png");
				}
			} else if(i == 2){
				if (loadResource.indexOf(wuqiFolder + user.person_items[i][j].id + ".png") < 0) {
					loadResource.push(wuqiFolder + user.person_items[i][j].id + ".png");
				}
			}
		}
	}
	//todo cangku maiyaoshui maiwuqi
	return loadResource;
}


var MyHomeScene = cc.Scene.extend({
	onEnter: function () {
		this._super();
		var layer = new HomeSceneLayer();
		this.addChild(layer);
		layer.init();
	}
});