var packageBoxSize = 0; //每个格子大小
var packageBoxNumX = 8; //wupin部 横格子数
var packageBoxNumY = 4; //wupin部 竖格子数
var packageBoxSpaceTop = 0; //wupin部 上距离
var packageBoxSpaceButtom = 6; //wupin部 下距离
var packageBoxSpaceLeftRight = 20; //wupin部 左右距离
var packageChangePageWidth = 0; //改page按钮宽度
var packagePageNow = 0; //当前Page页

var packageBoxMoveSprite;

var packageBoxArea;//1页重复利用
var packageTopSprite;

var soldDelClicked = 0; //1:sold 2:delete
var waitToSoldDel = [];
var waitToSoldDelSpr = [];


var packageUserCarryWuqiSprite = [];

var nowUserWuqiPage = 0; //0 ,1 
var packageUserSprite; //user 面板

var packageModoDetailSprite; //物品的详细

var footerSprite; // 最底下的部分

var menuNowTab = 0;
var menuChangeTabSpriteBG;
var menuBodySpriteBG;
var menuInfoSprite;
var menuTabWidth = 0;
var menuChangeTabArr = [];

openPackage = function () {// 打开xiangzi
	if (!spritePackage) { //打开
		userNowAct = "package";
		userDirect = "";

		//cc.eventManager.removeListener(listener1);
		if (nowPageName.indexOf("map") == 0) {
			cc.eventManager.pauseTarget(nowLayer.spriteMap, true);
		} else if (nowPageName.indexOf("home") == 0) {
			cc.eventManager.pauseTarget(nowLayer.spriteHome, true);
		}
		spritePackage = new cc.Sprite(img_package_backGround);
		spritePackage.setAnchorPoint(0, 1);
		spritePackage.setPosition(0, size.height - mapSpaceTop);
		spritePackage.setScale(size.width / spritePackage.width, (size.height - mapSpaceTop - mapSpaceButtom) / spritePackage.height);

		nowLayer.addChild(spritePackage, 8);

		//描绘格子changePage
		initButtomPackageChangePage(spritePackage);

		//描绘格子
		initButtomPackageBox();

		//top
		initPackageTop();

		//7件wuqi box
		initMiddleLeft();

	} else { //关闭
		userNowAct = "";
		userDirect = "";
		spritePackage.removeAllChildren(true); //todo del?
		nowLayer.removeChild(spritePackage, true);
		
		spritePackage = null;

		if (nowPageName.indexOf("map") == 0) {
			cc.eventManager.resumeTarget(nowLayer.spriteMap, true);
		} else if (nowPageName.indexOf("home_person4") == 0 || nowPageName.indexOf("home_person5") == 0) {
			nowPageName = "home";
			cc.eventManager.resumeTarget(nowLayer.spriteHome, true);
			nowLayer.plusWuqi = [];
			plusWaitToDel = -1;
		} else if (nowPageName.indexOf("home") == 0) {
			nowPageName = "home";
			cc.eventManager.resumeTarget(nowLayer.spriteHome, true);
		}
	}
}

openMenu = function () {// 打开Menu
	if (!spritePackage) { //打开
		userNowAct = "menu";
		userDirect = "";

		if (nowPageName.indexOf("map") == 0) {
			cc.eventManager.pauseTarget(nowLayer.spriteMap, true);
		} else if (nowPageName.indexOf("home") == 0) {
			cc.eventManager.pauseTarget(nowLayer.spriteHome, true);
		}
		spritePackage = new cc.Sprite(img_package_backGround);
		spritePackage.setAnchorPoint(0, 1);
		spritePackage.setPosition(0, size.height - mapSpaceTop);
		spritePackage.setScale(size.width / spritePackage.width, (size.height - mapSpaceTop - mapSpaceButtom) / spritePackage.height);

		nowLayer.addChild(spritePackage, 8);

		//top
		initPackageTop();

		//changeMeneTab
		initMenuChangeTab();

		initMenuBody();

//		initMenuInfo();

	} else { //关闭
		userNowAct = "";
		userDirect = "";
		spritePackage.removeAllChildren(true); //todo del?
		nowLayer.removeChild(spritePackage, true);

		spritePackage = null;

		if (nowPageName.indexOf("map") == 0) {
			cc.eventManager.resumeTarget(nowLayer.spriteMap, true);
		} else if (nowPageName.indexOf("home_person4") == 0 || nowPageName.indexOf("home_person5") == 0) {
			nowPageName = "home";
			cc.eventManager.resumeTarget(nowLayer.spriteHome, true);
			nowLayer.plusWuqi = [];
			plusWaitToDel = -1;
		} else if (nowPageName.indexOf("home") == 0) {
			nowPageName = "home";
			cc.eventManager.resumeTarget(nowLayer.spriteHome, true);
		}
	}
}

function initMenuChangeTab() {

	//换页用背景
	menuChangeTabSpriteBG = new cc.Sprite(img_package_changePage_BG);
	menuChangeTabSpriteBG.setAnchorPoint(0, 1);
	menuChangeTabSpriteBG.setPosition(0, spritePackage.height - packageTopSprite.height);
	menuChangeTabSpriteBG.setScale(spritePackage.width / menuChangeTabSpriteBG.width);
	spritePackage.addChild(menuChangeTabSpriteBG, 1);

	menuChangeTabArr = [];

	var nowImgTemp = "";
	for (var i = 0; i < menuTypeNum; i++) {

		if (i == menuNowTab) {
		    nowImgTemp = img_package_changePageActive;//new cc.Sprite(img_package_changePageActive); //TODO change img
		} else {
		    nowImgTemp = img_package_changePage;//new cc.Sprite(img_package_changePage); //TODO change img
		}
		if (menuTabWidth == 0) {
			menuTabWidth = menuChangeTabSpriteBG.width / menuTypeNum;
		}

		menuChangeTabArr[i] = new cc.MenuItemImage(nowImgTemp, img_package_changePageActive, function (para1) {
		    if (menuNowTab != parseInt(para1.getTag())) {
		        menuChangeTabArr[menuNowTab].normalImage.setTexture(img_package_changePage);
				menuNowTab = parseInt(para1.getTag());
				menuChangeTabArr[menuNowTab].normalImage.setTexture(img_package_changePageActive);

				initMenuBody();
			}
		}, this);
		menuChangeTabArr[i].setAnchorPoint(0, 0);
		menuChangeTabArr[i].setPosition(menuTabWidth * i, 0);
		menuChangeTabArr[i].setScale(menuTabWidth / menuChangeTabArr[i].width, menuChangeTabSpriteBG.height / menuChangeTabArr[i].height);
		menuChangeTabArr[i].setTag(i);
	}
	var menu = new cc.Menu(menuChangeTabArr);
	menu.setPosition(0, 0);
	menuChangeTabSpriteBG.addChild(menu, 1);
}



function initMenuBody() {

	if (menuBodySpriteBG) {
		menuBodySpriteBG.removeAllChildren(true);
	}

	//body
	menuBodySpriteBG = new cc.Sprite(img_menu_body_BG);
	menuBodySpriteBG.setAnchorPoint(0, 1);
	menuBodySpriteBG.setPosition(0, spritePackage.height - packageTopSprite.height - menuChangeTabSpriteBG.height);
	menuBodySpriteBG.setScale(spritePackage.width / menuBodySpriteBG.width);
	spritePackage.addChild(menuBodySpriteBG, 1);
	var ngSpr;
	var okSpr;

	var nowNumber = [];
	if (menuNowTab == 0) {// 技能
		var oneLineHeight = Math.floor(menuBodySpriteBG.height / (user.property.length + 3));
		var oneLineHeightTrue = Math.floor(oneLineHeight * 0.8);
		var bodyLeftMargin = 50;
		var perWidth = (menuBodySpriteBG.width - bodyLeftMargin * 2) / 4;
		var nowYPoint = 0;
		var propertyList = [];
		var leftPointSpr;

		var leftPointNow = user.leftPoint;
		for (var i = -1; i < user.addPoint.length; i++) {
			nowYPoint = menuBodySpriteBG.height - oneLineHeight * (i + 2);
			if (i == -1) {
				leftPointSpr = new cc.LabelTTF("剩余点数：" + leftPointNow, "Impact", 20);
				leftPointSpr.setAnchorPoint(0, 0);
				leftPointSpr.setPosition(bodyLeftMargin, nowYPoint);
				leftPointSpr.setColor(cc.color(0, 0, 0, 255));
				leftPointSpr.setScale(oneLineHeightTrue / leftPointSpr.height);
				menuBodySpriteBG.addChild(leftPointSpr, 1);

				//NG
				ngSpr = new cc.MenuItemImage(img_ngBtn, img_ngBtn, function () {
					for (var j = 0; j < user.property.length;j++){
						nowNumber[j].setString(user.addPoint[j] + "");
					}
					leftPointNow = user.leftPoint;
					leftPointSpr.setString("剩余点数：" + leftPointNow);
					ngSpr.setVisible(false);
					okSpr.setVisible(false);
				}, this);
				ngSpr.setAnchorPoint(0, 0);
				ngSpr.setPosition(bodyLeftMargin + leftPointSpr.width + perWidth, nowYPoint);
				ngSpr.setScale(oneLineHeightTrue / ngSpr.height);
				propertyList.push(ngSpr);
				//OK
				okSpr = new cc.MenuItemImage(img_okBtn, img_okBtn, function () {
					user.activeJineng = [];
					for (var j = 0; j < user.property.length; j++) {
						user.addPoint[j] = parseInt(nowNumber[j].getString());
						if (user.addPoint[j] >= 10) {
							user.activeJineng.push(j + "_2");
						} else if (user.addPoint[j] >= 5) {
							user.activeJineng.push(j + "_1");
						}
					}
					user.leftPoint = leftPointNow;
					renewUser();
					initMenuBody();
				}, this);
				okSpr.setAnchorPoint(0, 0);
				okSpr.setPosition(bodyLeftMargin + leftPointSpr.width + perWidth + ngSpr.width, nowYPoint);
				okSpr.setScale(oneLineHeightTrue / okSpr.height);
				propertyList.push(okSpr);

				ngSpr.setVisible(false);
				okSpr.setVisible(false);
			} else if (i < user.property.length) {
				
				var leftWenziSpr = new cc.MenuItemFont(getPropertyName[i], function (para1) {
					cc.log("TODO show + " + para1.getTag());

					var index = parseInt(para1.getTag());
					if (getPropertyName[index] < 1) {
					    menuInfoSprite.setString("每点[" + getPropertyName[index] + "]属性点，增加[" + getPlusPropertyInfo[index] * 100 + "%]·" + getPropertyName[index]);
					} else {
					    menuInfoSprite.setString("每点[" + getPropertyName[index] + "]属性点，增加[" + getPlusPropertyInfo[index] + "]点·" + getPropertyName[index]);
					}
				}, this);
				leftWenziSpr.setAnchorPoint(0, 0);
				leftWenziSpr.setPosition(bodyLeftMargin, nowYPoint);
				leftWenziSpr.setColor(cc.color(0, 0, 0, 255));
				leftWenziSpr.setScale(oneLineHeightTrue / leftWenziSpr.height);
				leftWenziSpr.setTag(i);
				propertyList.push(leftWenziSpr);

				//less 按钮
				var lessSpr = new cc.MenuItemImage(img_lesBtn, img_lesBtn, function (para1) {
					// todo
					cc.log("TODO lessSpr + " + para1.getTag());
					var index = parseInt(para1.getTag());
					if (parseInt(nowNumber[index].getString()) <= user.addPoint[index]) {
						// TODO
						cc.log("cant less");
					} else {
						leftPointNow++;
						leftPointSpr.setString("剩余点数：" + leftPointNow);
						nowNumber[index].setString(parseInt((nowNumber[index].getString()) - 1) + "");

						ngSpr.setVisible(true);
						okSpr.setVisible(true);
					}
				}, this);
				lessSpr.setAnchorPoint(0, 0);
				lessSpr.setPosition(bodyLeftMargin + perWidth, nowYPoint);
				lessSpr.setScale(oneLineHeightTrue / lessSpr.height);
				lessSpr.setTag(i);
				propertyList.push(lessSpr);

				//nowNumber
				nowNumber[i] = new cc.LabelTTF(user.addPoint[i] + "", "Impact", 20);
				nowNumber[i].setAnchorPoint(0.5, 0);
				nowNumber[i].setPosition(bodyLeftMargin + perWidth * 1.4, nowYPoint);
				nowNumber[i].setColor(cc.color(0, 0, 0, 255));
				nowNumber[i].setScale(oneLineHeightTrue / nowNumber[i].height);
				menuBodySpriteBG.addChild(nowNumber[i], 1);

				//add 按钮
				var addSpr = new cc.MenuItemImage(img_addBtn, img_addBtn, function (para1) {
					// todo
					cc.log("TODO addSpr + " + para1.getTag());
					var index = parseInt(para1.getTag());
					cc.log("now:" + parseInt(nowNumber[index].getString()));
					if (parseInt(nowNumber[index].getString()) >= 10 || leftPointNow == 0) {
						// TODO
						cc.log("bu zu or max");
					} else {
						leftPointNow--;
						leftPointSpr.setString("剩余点数：" + leftPointNow);
						nowNumber[index].setString((parseInt(nowNumber[index].getString()) + 1) + "");

						ngSpr.setVisible(true);
						okSpr.setVisible(true);
					}
				}, this);
				addSpr.setAnchorPoint(0.5, 0);
				addSpr.setPosition(bodyLeftMargin + perWidth * 1.7, nowYPoint);
				addSpr.setScale(oneLineHeightTrue / addSpr.height);
				addSpr.setTag(i);
				propertyList.push(addSpr);

				//jineng
				var activeJinengColor = cc.color(0, 0, 100, 255);
				var noActiveJinengColor = cc.color(100, 100, 100, 255);
				var colorTemp;
				if (jinengName[i + "_" + 1]) {
					//jineng 1
					if (user.activeJineng.indexOf(i + "_" + 1) >= 0) {
						colorTemp = activeJinengColor;
					} else {
						colorTemp = noActiveJinengColor;
					}
					var jineng1Spr = new cc.MenuItemFont(jinengName[i + "_" + 1], function (para1) {
						// todo
						cc.log("TODO jineng1Spr + " + para1.getTag());
						menuInfoSprite.setString(jinengInfo[para1.getTag()]);
					}, this);
					jineng1Spr.setAnchorPoint(0, 0);
					jineng1Spr.setPosition(bodyLeftMargin + perWidth * 2, nowYPoint);
					jineng1Spr.setColor(colorTemp);
					jineng1Spr.setScale(oneLineHeightTrue / jineng1Spr.height);
					jineng1Spr.setTag(i + "_" + 1);
					propertyList.push(jineng1Spr);

/*					var nameOfJineng1 = new cc.LabelTTF(jinengName[i + "_" + 1], "Impact", 20);
					nameOfJineng1.setAnchorPoint(0.5, 0.5);
					nameOfJineng1.setPosition(jineng1Spr.width / 2, jineng1Spr.height / 2);
					nameOfJineng1.setColor(cc.color(0, 0, 0, 255));
					jineng1Spr.addChild(nameOfJineng1, 1);*/

					//jineng 2
					if (user.activeJineng.indexOf(i + "_" + 2) >= 0) {
						colorTemp = activeJinengColor;
					} else {
						colorTemp = noActiveJinengColor;
					}
					var jineng2Spr = new cc.MenuItemFont(jinengName[i + "_" + 2], function (para1) {
						// todo
						cc.log("TODO jineng2Spr + " + para1.getTag());
						menuInfoSprite.setString(jinengInfo[para1.getTag()]);
					}, this);
					jineng2Spr.setAnchorPoint(0, 0);
					jineng2Spr.setPosition(bodyLeftMargin + perWidth * 3, nowYPoint);
					jineng2Spr.setColor(colorTemp);
					jineng2Spr.setScale(oneLineHeightTrue / jineng2Spr.height);
					jineng2Spr.setTag(i + "_" + 2);
					propertyList.push(jineng2Spr);

					/*var nameOfJineng2 = new cc.LabelTTF(jinengName[i + "_" + 2], "Impact", 20);
					nameOfJineng2.setAnchorPoint(0.5, 0.5);
					nameOfJineng2.setPosition(nameOfJineng2.width / 2, nameOfJineng2.height / 2);
					nameOfJineng2.setColor(cc.color(0, 0, 0, 255));
					jineng2Spr.addChild(nameOfJineng2, 1);*/
				}
			} else {
			}
		}
		var menu = new cc.Menu(propertyList);
		menu.setPosition(0, 0);
		menuBodySpriteBG.addChild(menu, 1);

		menuInfoSprite = new cc.LabelTTF("", "Impact", 20);
		menuInfoSprite.setAnchorPoint(0, 0.5);
		menuInfoSprite.setPosition(bodyLeftMargin, oneLineHeight);
		menuInfoSprite.setColor(cc.color(0, 0, 0, 255));
		//menuInfoSprite.setScale(oneLineHeightTrue / menuInfoSprite.height);
		menuBodySpriteBG.addChild(menuInfoSprite, 1);
	} else if (menuNowTab == 1) { //帮助？

	} else { //系统菜单
		var setList = [];

		var setSpr1 = new cc.MenuItemFont("退出这次探险", function () {
			var spriteTemp = new cc.Sprite(img_dialog_center);
			spriteTemp.setAnchorPoint(0.5, 0.5);
			spriteTemp.setPosition(size.width / 2, size.height / 2);
			spriteTemp.setScale(size.width / spriteTemp.width);
			spritePackage.addChild(spriteTemp, 10);

			//message
			var message = new cc.LabelTTF("这次探险所得都将失去。\n确定退出这次探险吗？", "Impact", 40);
			message.setAnchorPoint(0.5, 0.5);
			message.setPosition(spriteTemp.width / 2, spriteTemp.height * 0.75);
			message.setColor(cc.color(0, 0, 0, 255));
			spriteTemp.addChild(message, 1);

			var buttonList = [];
			//ng
			var ngSpr = new cc.MenuItemImage(img_dialog_ng_btn, img_dialog_ng_btn, function () {
				spritePackage.removeChild(spriteTemp);
			}, this);
			ngSpr.setAnchorPoint(0.5, 0.5);
			ngSpr.setPosition(spriteTemp.width * 0.25, spriteTemp.height * 0.25);
			buttonList.push(ngSpr);
			//ok
			var okSpr = new cc.MenuItemImage(img_dialog_ok_btn, img_dialog_ok_btn, function () {
				loadOldUser();
				gotoHome();
			}, this);
			okSpr.setAnchorPoint(0.5, 0.5);
			okSpr.setPosition(spriteTemp.width * 0.75, spriteTemp.height * 0.25);
			buttonList.push(okSpr);

			var menu = new cc.Menu(buttonList);
			menu.setPosition(0, 0);
			spriteTemp.addChild(menu, 1);
		}, this);
		setSpr1.setAnchorPoint(0.5, 0.5);
		setSpr1.setPosition(menuBodySpriteBG.width * 0.5, menuBodySpriteBG.height * 0.8);
		setSpr1.setColor(cc.color(0, 0, 0, 255));
		setSpr1.setScale(menuBodySpriteBG.height/10/setSpr1.height);
		setList.push(setSpr1);

		var setSpr2 = new cc.MenuItemFont("联系作者", function () {
			var spriteTemp = new cc.Sprite(img_dialog_center);
			spriteTemp.setAnchorPoint(0.5, 0.5);
			spriteTemp.setPosition(size.width / 2, size.height / 2);
			spriteTemp.setScale(size.width / spriteTemp.width);
			spritePackage.addChild(spriteTemp, 10);

			//message
			var message = new cc.LabelTTF("****@163.com", "Impact", 40);
			message.setAnchorPoint(0.5, 0.5);
			message.setPosition(spriteTemp.width / 2, spriteTemp.height * 0.75);
			message.setColor(cc.color(0, 0, 0, 255));
			spriteTemp.addChild(message, 1);

			var buttonList = [];
			//ng
			var ngSpr = new cc.MenuItemImage(img_dialog_ng_btn, img_dialog_ng_btn, function () {
				spritePackage.removeChild(spriteTemp);
			}, this);
			ngSpr.setAnchorPoint(0.5, 0.5);
			ngSpr.setPosition(spriteTemp.width * 0.5, spriteTemp.height * 0.25);
			buttonList.push(ngSpr);

			var menu = new cc.Menu(buttonList);
			menu.setPosition(0, 0);
			spriteTemp.addChild(menu, 1);
		}, this);
		setSpr2.setAnchorPoint(0.5, 0.5);
		setSpr2.setPosition(menuBodySpriteBG.width * 0.5, menuBodySpriteBG.height * 0.6);
		setSpr2.setColor(cc.color(0, 0, 0, 255));
		setSpr2.setScale(menuBodySpriteBG.height / 10 / setSpr2.height);
		setList.push(setSpr2);

		var menuList = new cc.Menu(setList);
		menuList.setPosition(0, 0);
		menuBodySpriteBG.addChild(menuList, 1);
	}

}

function initXuemo() {
	var xueLeftSpace = 50;

	if (!topSprite) {
		//background
		topSprite = new cc.Sprite(img_top_BG);
		topSprite.setAnchorPoint(0, 1);
		topSprite.setPosition(0, nowLayer.height);
		topSprite.setScale(nowLayer.width / topSprite.width);
		nowLayer.addChild(topSprite, 1);

		//xue
		topSprite_xue = new cc.Sprite(img_top_xue);
		topSprite_xue.setAnchorPoint(0, 1);
		topSprite_xue.setPosition(xueLeftSpace, topSprite.height);
		if (user.xueNow>0) {
			topSprite_xue.setScale((topSprite.width/2-xueLeftSpace)/topSprite_xue.width * (user.xueNow/user.property[xueNo]),(topSprite.width/2-xueLeftSpace)/topSprite_xue.width);
		} else {
			topSprite_xue.setScale(0,(topSprite.width/2-xueLeftSpace)/topSprite_xue.width);
		}
		topSprite.addChild(topSprite_xue, 1);
		//xue shuzi
		topLabel_xue = new cc.LabelTTF(user.xueNow + "/" + user.property[xueNo], "Impact", 20);
		topLabel_xue.setAnchorPoint(0.5,0.5);
		topLabel_xue.setPosition(xueLeftSpace + topSprite_xue.width/2 , topSprite.height - topSprite_xue.height/2);
		topLabel_xue.setColor(cc.color(0, 0, 0, 255));
		topSprite.addChild(topLabel_xue, 5);

		//mo
		topSprite_mo = new cc.Sprite(img_top_mo);
		topSprite_mo.setAnchorPoint(0, 1);
		topSprite_mo.setPosition(xueLeftSpace, topSprite.height - topSprite_xue.height);
		if (user.moNow>0) {
			topSprite_mo.setScale((topSprite.width/2-xueLeftSpace)/topSprite_mo.width * (user.moNow/user.property[moNo]),(topSprite.width/2-xueLeftSpace)/topSprite_mo.width);
		} else {
			topSprite_mo.setScale(0,(topSprite.width/2-xueLeftSpace)/topSprite_mo.width);
		}
		topSprite.addChild(topSprite_mo, 1);
		//mo shuzi
		topLabel_mo = new cc.LabelTTF(user.moNow + "/" + user.property[moNo], "Impact", 20);
		topLabel_mo.setAnchorPoint(0.5,0.5);
		topLabel_mo.setPosition(xueLeftSpace + topSprite_mo.width/2 , topSprite.height - topSprite_mo.height - topSprite_mo.height/2);
		topLabel_mo.setColor(cc.color(0, 0, 0, 255));
		topSprite.addChild(topLabel_mo, 5);

		//jingyan
		topSprite_jingyan = new cc.Sprite(img_top_jingyan);
		topSprite_jingyan.setAnchorPoint(0, 1);
		topSprite_jingyan.setPosition(xueLeftSpace, topSprite.height - topSprite_xue.height - topSprite_mo.height);
		if (user.jingyanNow>0) {
			topSprite_jingyan.setScale((topSprite.width/2-xueLeftSpace)/topSprite_jingyan.width * (user.jingyanNow/user.jingyanAll),(topSprite.width/2-xueLeftSpace)/topSprite_jingyan.width);
		} else {
			topSprite_jingyan.setScale(0,(topSprite.width/2-xueLeftSpace)/topSprite_jingyan.width);
		}
		topSprite.addChild(topSprite_jingyan, 1);
		//jingyan shuzi
		topLabel_jingyan = new cc.LabelTTF(user.jingyanNow + "/" + user.jingyanAll, "Impact", 20);
		topLabel_jingyan.setAnchorPoint(0.5,0.5);
		topLabel_jingyan.setPosition(xueLeftSpace + topSprite_jingyan.width/2 , topSprite.height - topSprite_mo.height - topLabel_mo.height - topSprite_jingyan.height/2 - 3);
		topLabel_jingyan.setColor(cc.color(0, 0, 0, 255));
		topSprite.addChild(topLabel_jingyan, 5);

		//jinbi
		topLabel_jinbi = new cc.LabelTTF(user.jinbi + "", "Impact", 20);
		topLabel_jinbi.setAnchorPoint(0,1);
		topLabel_jinbi.setPosition(topSprite.width/2 + xueLeftSpace*1.5, topSprite.height);
		topLabel_jinbi.setColor(cc.color(0, 0, 0, 255));
		topSprite.addChild(topLabel_jinbi, 5);

		//zuanshi
		topLabel_zuanshi = new cc.LabelTTF(user.zuanshi + "", "Impact", 20);
		topLabel_zuanshi.setAnchorPoint(0,1);
		topLabel_zuanshi.setPosition(topSprite.width/2 + xueLeftSpace*1.5, topSprite.height - topLabel_jinbi.height);
		topLabel_zuanshi.setColor(cc.color(0, 0, 0, 255));
		topSprite.addChild(topLabel_zuanshi, 5);


		//退出按钮
		var closeSpr = new cc.MenuItemImage(s_CloseNormal, s_CloseSelected, function () {
			if (nowPageName.indexOf("home") == 0) {
				// todo
				if (userNowAct == "package") {
					spritePackage.removeAllChildren(true);
					nowLayer.removeChild(spritePackage, true);
					spritePackage = null;

					cc.eventManager.resumeTarget(nowLayer.spriteHome, true);
				}
			} else if (nowPageName.indexOf("map") == 0) {
				if (userNowAct == "package") {
					spritePackage.removeAllChildren(true);
					nowLayer.removeChild(spritePackage, true);
					spritePackage = null;

					if (nowLayer.boxSelectedSpr) {
						nowLayer.boxSelectedSpr.setVisible(false);
					}
				} else {
					gotoHome();
				}
			}
			userNowAct = "";
		}, this);
		closeSpr.setAnchorPoint(1, 1);
		closeSpr.setPosition(topSprite.width, topSprite.height);

		var menu = new cc.Menu(closeSpr);
		menu.setPosition(0, 0);
		topSprite.addChild(menu, 1);
		
	} else {
		if (user.xueNow>0) {
			topSprite_xue.setScale((topSprite.width/2-xueLeftSpace)/topSprite_xue.width * (user.xueNow/user.property[xueNo]),(topSprite.width/2-xueLeftSpace)/topSprite_xue.width);
		} else {
			user.xueNow = 0;
			topSprite_xue.setScale(0,(topSprite.width/2-xueLeftSpace)/topSprite_xue.width);
		}
		topLabel_xue.setString(user.xueNow + "/" + user.property[xueNo]);

		if (user.moNow>0) {
			topSprite_mo.setScale((topSprite.width/2-xueLeftSpace)/topSprite_mo.width * (user.moNow/user.property[moNo]),(topSprite.width/2-xueLeftSpace)/topSprite_mo.width);
		} else {
			topSprite_mo.setScale(0,(topSprite.width/2-xueLeftSpace)/topSprite_mo.width);
		}
		topLabel_mo.setString(user.moNow + "/" + user.property[moNo]);

		if (user.jingyanNow>0) {
			topSprite_jingyan.setScale((topSprite.width/2-xueLeftSpace)/topSprite_jingyan.width * (user.jingyanNow/user.jingyanAll),(topSprite.width/2-xueLeftSpace)/topSprite_jingyan.width);
		} else {
			topSprite_jingyan.setScale(0,(topSprite.width/2-xueLeftSpace)/topSprite_jingyan.width);
		}
		topLabel_jingyan.setString(user.jingyanNow + "/" + user.jingyanAll);

		topLabel_jinbi.setString(user.jinbi);
		topLabel_zuanshi.setString(user.zuanshi);
	}
}

function initPackageTop() {
	
	if (packageTopSprite) {
		packageTopSprite.removeAllChildren(true);
		spritePackage.removeChild(packageTopSprite, true);
	}

	packageTopSprite = new cc.Sprite(img_package_top_BG);
	packageTopSprite.setAnchorPoint(0, 1);
	packageTopSprite.setPosition(0, spritePackage.height);
	packageTopSprite.setScale(spritePackage.width / packageTopSprite.width);
	spritePackage.addChild(packageTopSprite, 1);

	return;//TODO

	var message1 = "";
	var message2 = "";
	var message3 = "";
	var message4 = "";
	for (var i = 0; i < user.property.length; i++) {
		if (i < 4) {
			message1 += getPropertyName[i] + " : " + user.property[i] + "\n";
		} else if (4 <= i && i < 7) {
			message2 += getPropertyName[i] + " : " + user.property[i] + "\n";
		} else if (7 <= i && i < 11) {
			message3 += getPropertyName[i] + " : " + user.property[i] + "\n";
		} else if (11 <= i) {
			message4 += getPropertyName[i] + " : " + user.property[i] + "\n";
		}
	}

	var propertyLabel1 = new cc.LabelTTF(message1, "Impact", 30);
	propertyLabel1.setAnchorPoint(0, 1);
	propertyLabel1.setPosition(20, packageTopSprite.height - 40);
	propertyLabel1.setColor(cc.color(0, 0, 255, 255));
	packageTopSprite.addChild(propertyLabel1, 1);

	var propertyLabel2 = new cc.LabelTTF(message2, "Impact", 30);
	propertyLabel2.setAnchorPoint(0, 1);
	propertyLabel2.setPosition(190, packageTopSprite.height - 40);
	propertyLabel2.setColor(cc.color(0, 0, 255, 255));
	packageTopSprite.addChild(propertyLabel2, 1);

	var propertyLabel3 = new cc.LabelTTF(message3, "Impact", 30);
	propertyLabel3.setAnchorPoint(0, 1);
	propertyLabel3.setPosition(360, packageTopSprite.height - 40);
	propertyLabel3.setColor(cc.color(0, 0, 255, 255));
	packageTopSprite.addChild(propertyLabel3, 1);

	var propertyLabel4 = new cc.LabelTTF(message4, "Impact", 30);
	propertyLabel4.setAnchorPoint(0, 1);
	propertyLabel4.setPosition(530, packageTopSprite.height - 40);
	propertyLabel4.setColor(cc.color(0, 0, 255, 255));
	packageTopSprite.addChild(propertyLabel4, 1);

}

//user area
function initMiddleLeft() {

	renewUser();
	initPackageTop();

	if (packageUserSprite) {
		spritePackage.removeChild(packageUserSprite,true);
	}

	//8件wuqi box
	//background
	packageUserSprite = new cc.Sprite(img_homeLeft_BG);
	packageUserSprite.setAnchorPoint(0, 1);
	packageUserSprite.setPosition(0, spritePackage.height - packageTopSprite.height);
	packageUserSprite.setScale(spritePackage.width / 2 / packageUserSprite.width);
	spritePackage.addChild(packageUserSprite, 1);

	//第1，2重身
	var numTemp = "";
	if (nowUserWuqiPage == 0) {
		numTemp = "第\n一\n重\n身\n主";
	} else {
		numTemp = "第\n二\n重\n身\n附";
	}
	var message = new cc.LabelTTF(numTemp, "Impact", 25);
	message.setAnchorPoint(0.5, 0.5);
	message.setPosition(packageUserSprite.width * 0.45, packageUserSprite.height * 0.45);
	message.setColor(cc.color(180, 180, 180, 255));
	packageUserSprite.addChild(message, 2);

	//wuqi
	packageUserCarryWuqiSprite = [];
	for (var i=0;i<8;i++) {
		packageUserCarryWuqiSprite[i] = addMonoSprite(
			packageUserSprite,
			img_package_boxBG_empty,
			userCarryPosX[i],
			userCarryPosY[i],
			1,
			"",
			0
		);

		//wuqiLeibie
		var message = new cc.LabelTTF(getWuqiType[i], "Impact", 15);
		message.setAnchorPoint(0.5, 0.5);
		message.setPosition(packageUserCarryWuqiSprite[i].width / 2, packageUserCarryWuqiSprite[i].height / 2);
		message.setColor(cc.color(180, 180, 180, 255));
		packageUserCarryWuqiSprite[i].addChild(message, 2);

		if (user.wuqiCarry[nowUserWuqiPage][i] != null && user.wuqiCarry[nowUserWuqiPage][i] !="") {
			packageUserCarryWuqiSprite[i] = addMonoSprite(
				packageUserSprite,
				wuqiFolder + user.wuqiCarry[nowUserWuqiPage][i].id + ".png",
				userCarryPosX[i],
				userCarryPosY[i],
				1,
				"wuqiCarry_" + i + "_" + nowUserWuqiPage,
				user.wuqiCarry[nowUserWuqiPage][i].special_type.length
			);

			cc.eventManager.addListener(listener_package_boxMove.clone(), packageUserCarryWuqiSprite[i]);

			if (user.wuqiCarry[nowUserWuqiPage][i].plusLevel > 0) {
				showWuqiPlusNum(packageUserCarryWuqiSprite[i], user.wuqiCarry[nowUserWuqiPage][i].plusLevel);
			}
		}
	}

	if (user.activeJineng.indexOf("2_2") >= 0) {
		//change user page
		var changeUserPageButton = new cc.MenuItemImage(img_package_boxBG_locked, img_package_boxBG_locked, 
			function () {
				nowUserWuqiPage = (nowUserWuqiPage + 1) % 2;
				initMiddleLeft();
			}, this);
		changeUserPageButton.setAnchorPoint(0, 0);
		changeUserPageButton.setScale(packageBoxSize / changeUserPageButton.width, packageBoxSize / changeUserPageButton.height/2);
		changeUserPageButton.setPosition(0, 0);
		var menuChagenUser = new cc.Menu(changeUserPageButton);
		menuChagenUser.setPosition(packageUserSprite.width-100, spritePackage.height - packageTopSprite.height-50);
		spritePackage.addChild(menuChagenUser, 5);
	}
}

//mono detail area
function initMiddleRight(monoType, key, node) {
	if (monoType==null || monoType.length ==0) {
		return;
	}

	if (packageModoDetailSprite) {
		packageModoDetailSprite.removeAllChildren(true);
		spritePackage.removeChild(packageModoDetailSprite, true);
	}
	packageModoDetailSprite = new cc.Sprite(img_package_user_BG_right);
	packageModoDetailSprite.setAnchorPoint(1, 0);
	packageModoDetailSprite.setPosition(spritePackage.width, packageBoxArea.height + packageChangeBK.height);
	packageModoDetailSprite.setScale(spritePackage.width / 2 / packageModoDetailSprite.width);
	spritePackage.addChild(packageModoDetailSprite, 1);

	var monoDetail;
	if (monoType == "wuqi") {
		monoDetail = new cc.Sprite(wuqiFolder + key.id + ".png");

		if (key.plusLevel > 0) {
			showWuqiPlusNum(monoDetail, key.plusLevel);
		}
	} else {
		monoDetail = new cc.Sprite(yaoshuiFolder + key + ".png");
	}
	monoDetail.setAnchorPoint(0.5, 1);
	monoDetail.setPosition(10+monoDetail.width/2, packageModoDetailSprite.height-10);
	monoDetail.setScale(packageBoxSize / monoDetail.width,packageBoxSize / monoDetail.height);
	packageModoDetailSprite.addChild(monoDetail, 1);

	var message1 = "";
	var message2 = "";
	var message3 = "";
	var message4 = "";
	var message5 = "";
	var message6 = "";

	if (monoType == "wuqi") {
		var wuqiInfo = getWuqiInfo(key.id);
		message1 += "" + wuqiInfo.name + "(" + getWuqiType[wuqiInfo.type] + ")" + "\n";
		for (var i = 0; i < key.trueProperty.length; i++) {
			if (key.trueProperty[i] > 0) {
				message1 += getPropertyName[i] + "：+" + key.trueProperty[i] + "\n";
			}
		}

		for (var i=0;i<key.special_type.length;i++) {
			if (key.special_type[i] >= 0) {
				message2 += getPropertyName[key.special_type[i]] + "：+" + key.special_value[i] + "\n";
			}
		}

		for (var i=0;i<key.add_type.length;i++) {
			if (key.add_type[i] >= 0) {
				message3 += getPropertyName[key.add_type[i]] + "：+" + key.add_value[i] + "\n";
			}
		}

		if (wuqiInfo.groupNum > 0) {
			var nowGroupWuqiNum = 0;
			for (var i=0;i<user.wuqiCarry[nowUserWuqiPage].length;i++) { //看看当前穿戴了几件套装
				if (user.wuqiCarry[nowUserWuqiPage][i] != null && user.wuqiCarry[nowUserWuqiPage][i] != "" && user.wuqiCarry[nowUserWuqiPage][i].id.length > 0) {
					var wuqiInfo2 = getWuqiInfo(user.wuqiCarry[nowUserWuqiPage][i].id);
					if (wuqiInfo2.groupNum == wuqiInfo.groupNum) {
						nowGroupWuqiNum++;
					}
				}
			}

			var wuqiGroupInfo = getWuqiGroupInfo(wuqiInfo.groupNum);
			message4 += "套装属性\n";
			for (var i=0;i<3;i++) {
				switch(i){
					case 0:
						if(nowGroupWuqiNum>=2){
							message4 += "    穿戴2件\n";
							message4 += "          " + getPropertyName[wuqiGroupInfo.have246_type[i]] + "：+" + wuqiGroupInfo.have246_value[i] + "\n";
						}else{
							message5 += "    穿戴2件\n";
							message5 += "          " + getPropertyName[wuqiGroupInfo.have246_type[i]] + "：+" + wuqiGroupInfo.have246_value[i] + "\n";
						}
						break;
					case 1: 
						if(nowGroupWuqiNum>=4){
							message4 += "    穿戴4件\n";
							message4 += "          " + getPropertyName[wuqiGroupInfo.have246_type[i]] + "：+" + wuqiGroupInfo.have246_value[i] + "\n";
						}else{
							message5 += "    穿戴4件\n";
							message5 += "          " + getPropertyName[wuqiGroupInfo.have246_type[i]] + "：+" + wuqiGroupInfo.have246_value[i] + "\n";
						}
						break;
					case 2:
						if(nowGroupWuqiNum>=6){
							message4 += "    穿戴6件\n";
							message4 += "          " + getPropertyName[wuqiGroupInfo.have246_type[i]] + "：+" + wuqiGroupInfo.have246_value[i] + "\n";
						}else{
							message5 += "    穿戴6件\n";
							message5 += "          " + getPropertyName[wuqiGroupInfo.have246_type[i]] + "：+" + wuqiGroupInfo.have246_value[i] + "\n";
						}
						break;
				}
			}
		}
		var numLabel2 = new cc.LabelTTF(message2, "Impact", 20);
		numLabel2.setAnchorPoint(0,1);
		numLabel2.setPosition(10, packageModoDetailSprite.height-110);
		numLabel2.setColor(cc.color(100, 100, 100, 255));
		packageModoDetailSprite.addChild(numLabel2, 5);

		var numLabel3 = new cc.LabelTTF(message3, "Impact", 20);
		numLabel3.setAnchorPoint(0,1);
		numLabel3.setPosition(10, packageModoDetailSprite.height-80-numLabel2.height);
		numLabel3.setColor(cc.color(0, 200, 255, 255));
		packageModoDetailSprite.addChild(numLabel3, 5);

		var numLabel4 = new cc.LabelTTF(message4, "Impact", 20);
		numLabel4.setAnchorPoint(0,1);
		numLabel4.setPosition(10, packageModoDetailSprite.height-70-numLabel2.height-numLabel3.height);
		numLabel4.setColor(cc.color(255, 0, 0, 255));
		packageModoDetailSprite.addChild(numLabel4, 5);

		var numLabel5 = new cc.LabelTTF(message5, "Impact", 20);
		numLabel5.setAnchorPoint(0,1);
		numLabel5.setPosition(10, packageModoDetailSprite.height-45-numLabel2.height-numLabel3.height-numLabel4.height);
		numLabel5.setColor(cc.color(170, 170, 170, 255));
		packageModoDetailSprite.addChild(numLabel5, 5);

		message6 = "SOLD : " + wuqiInfo.soldGold;
	} else {
		var yaoshuiInfo = getYaoshuiInfo(key);
		message1 += "" + yaoshuiInfo.name + "\n\n";
		message1 += "类别：" + getYaoshuiType[yaoshuiInfo.type] + "\n";
		message6 = "SOLD : " + yaoshuiInfo.soldGold;
	}
	var numLabel1 = new cc.LabelTTF(message1, "Impact", 20);
	numLabel1.setAnchorPoint(0,1);
	numLabel1.setPosition(15 + monoDetail.width, packageModoDetailSprite.height-10);
	numLabel1.setColor(cc.color(0, 0, 0, 255));
	packageModoDetailSprite.addChild(numLabel1, 5);

	var numLabel6 = new cc.LabelTTF(message6, "Impact", 25);
	numLabel6.setAnchorPoint(0, 0);
	numLabel6.setPosition(15, 15);
	numLabel6.setColor(cc.color(255, 150, 0, 255));
	packageModoDetailSprite.addChild(numLabel6, 5);

	if (nowLayer.boxSelectedSpr && nowLayer.boxSelectedSpr.parent) {
		nowLayer.boxSelectedSpr.parent.removeChild(nowLayer.boxSelectedSpr, false);
		nowLayer.boxSelectedSpr = null;
	}
	nowLayer.boxSelectedSpr = new cc.Sprite(img_package_box_selected);
	nowLayer.boxSelectedSpr.setScale(node.width / nowLayer.boxSelectedSpr.width, node.height / nowLayer.boxSelectedSpr.height);
	nowLayer.boxSelectedSpr.setAnchorPoint(0.5, 0.5);
	nowLayer.boxSelectedSpr.setPosition(node.getPosition());
	node.parent.addChild(nowLayer.boxSelectedSpr, 9);
}

//package area
var listener_package_changePage = null;
var listener_sold = null;
function initButtomPackageChangePage(layerSprite) {

    //非强化页面，最后一个按钮不显示
	if (nowPageName != "home_person4" && nowPageName != "home_person5" && packagePageNow == img_package_num - 1) {
        packagePageNow = 0;
    }

    //页格子面板
    packageBoxArea = new cc.Sprite(img_package_areaBackGround);
    packageBoxArea.setAnchorPoint(0, 0);
    packageBoxArea.setPosition(0, 0);
    packageBoxArea.setScale(layerSprite.width / packageBoxArea.width);
    layerSprite.addChild(packageBoxArea, 1);

    //换页用背景
    packageChangeBK = new cc.Sprite(img_package_changePage_BG);
    packageChangeBK.setAnchorPoint(0, 0);
    packageChangeBK.setPosition(0, packageBoxArea.height);
    packageChangeBK.setScale(layerSprite.width / packageChangeBK.width);
    layerSprite.addChild(packageChangeBK, 1);
    //换页用按钮
    if (listener_package_changePage == null) { 
        listener_package_changePage = cc.EventListener.create({
            event: cc.EventListener.TOUCH_ONE_BY_ONE,
            swallowTouches: true,						// 设置是否吞没事件，在 onTouchBegan 方法返回 true 时吞掉事件，不再向下传递。
            onTouchBegan: function (touch, event) {		//实现 onTouchBegan 事件处理回调函数
                cc.log("onTouchBegan_changePage1");

                //判断点击区域
                var pos = this._node.convertToNodeSpace(touch.getLocation());
                var target = event.getCurrentTarget();
                if (!cc.rectContainsPoint(target.getTextureRect(), pos)) return false;
                cc.log("onTouchBegan_changePage2");

                if (packagePageNow != parseInt(this._node.getTag())) {
                    packageChangeArr[packagePageNow].setTexture(img_package_changePage);
                    packagePageNow = parseInt(this._node.getTag());
                    packageChangeArr[packagePageNow].setTexture(img_package_changePageActive);

                    initButtomPackageBox();
                }

                return true;
            }
        });
    }

    packageChangeArr = [];
    for (var i = 0; i < img_package_num; i++) {
        if (i == packagePageNow) {
            packageChangeArr[i] = new cc.Sprite(img_package_changePageActive);
        } else {
            packageChangeArr[i] = new cc.Sprite(img_package_changePage);
        }
        if (packageChangePageWidth == 0) {
            packageChangePageWidth = (packageChangeBK.width - packageBoxSpaceLeftRight * 2) / (img_package_num+1);
        }
        packageChangeArr[i].setAnchorPoint(0, 0);
        packageChangeArr[i].setPosition(packageChangePageWidth * i + packageBoxSpaceLeftRight, 0);
        packageChangeArr[i].setScale(packageChangePageWidth / packageChangeArr[i].width,packageChangeBK.height / packageChangeArr[i].height);
        packageChangeBK.addChild(packageChangeArr[i], 1);
        packageChangeArr[i].setTag(i);

        cc.eventManager.addListener(listener_package_changePage.clone(), packageChangeArr[i]);
    }

    //计算每个格子大小
    if (packageBoxSize == 0) {
        packageBoxSize = Math.floor(Math.min((packageBoxArea.height - packageBoxSpaceButtom) / packageBoxNumY, (packageBoxArea.width - 2 * packageBoxSpaceLeftRight) / packageBoxNumX));
        packageBoxSpaceTop = packageBoxArea.height - packageBoxNumY * packageBoxSize - packageBoxSpaceButtom;
        packageBoxSpaceLeftRight = (packageBoxArea.width - packageBoxNumX * packageBoxSize) / 2;
    }

    //非强化页面，最后一个按钮不显示
    if (nowPageName != "home_person4" && nowPageName != "home_person5") {
        packageChangeArr[img_package_num-1].setVisible(false);
    }
    /*
        if (listener_sold == null) {
            listener_sold = cc.EventListener.create({
                event: cc.EventListener.TOUCH_ONE_BY_ONE,
                swallowTouches: true,
                onTouchBegan: function (touch, event) {
    
                    //判断点击区域
                    var pos = this._node.convertToNodeSpace(touch.getLocation());
                    var target = event.getCurrentTarget();
                    if (!cc.rectContainsPoint(target.getTextureRect(), pos)) return false;
                    cc.log("onTouchBegan_changePage2");
    
                    if (packagePageNow != parseInt(this._node.getTag())) {
                        packageChangeArr[packagePageNow].setTexture(img_package_changePage);
                        packagePageNow = parseInt(this._node.getTag());
                        packageChangeArr[packagePageNow].setTexture(img_package_changePageActive);
    
                        initButtomPackageBox();
                    }
    
                    return true;
                }
            });
        }*/


    //sold buttom  ||  delete buttom
    var soldDelImg = null;
    if (nowPageName != "home_person1" && nowPageName != "home_person3" && nowPageName != "home_person4" && nowPageName != "home_person5") {
        if (nowPageName == "home_person0" || nowPageName == "home_person2") { //sold
            soldDelImg = img_package_sold_btn;
        } else {
            soldDelImg = img_package_del_btn;
        }

        soldDelClicked = 0;
        waitToSoldDel = [];
        waitToSoldDelSpr = [];
        var soldDelSpr = new cc.MenuItemImage(soldDelImg, img_package_soldDel_btn_clicked, function () {
            if (soldDelClicked == 0) {
                soldDelSpr.normalImage.setTexture(img_package_soldDel_btn_clicked);
                if (nowPageName == "home_person0" || nowPageName == "home_person2") { //sold
                    soldDelClicked = 1;
                } else {
                    soldDelClicked = 2;
                }
            } else {
                if (soldDelClicked == 1) { //sold
                    soldDelSpr.normalImage.setTexture(img_package_sold_btn);
                    //to sold
                    var tagTemp="";
                    var monoInfo;
                    for (var i = 0; i < waitToSoldDel.length;i++) {
                        tagTemp = waitToSoldDel[i].split("_");
                        if (tagTemp[0] == "wuqiPackage") {
                            monoInfo = getWuqiInfo(user.wuqiPackage[tagTemp[1]].id);
                            user.jinbi += monoInfo.soldGold;
                            user.wuqiPackage[tagTemp[1]] = "";
                        } else if (tagTemp[0] == "yaoshuiPackage") {
                            monoInfo = getYaoshuiInfo(user.yaoshuiPackage[tagTemp[1]]);
                            user.jinbi += monoInfo.soldGold * user.yaoshuiNum[tagTemp[1]];
                            user.yaoshuiPackage[tagTemp[1]] = "";
                            user.yaoshuiNum[tagTemp[1]] = 0;

                            if (user.yaoshuiCarry.indexOf(parseInt(tagTemp[1])) >= 0) {
                                user.yaoshuiCarry[user.yaoshuiCarry.indexOf(parseInt(tagTemp[1]))] = -1;
                            }
                        }
                    }
                    initButtomPackageBox();
                    initXuemo();
                    initFooter();
                } else {
                    soldDelSpr.normalImage.setTexture(img_package_del_btn);
                    //to del
                    for (var i = 0; i < waitToSoldDel.length; i++) {
                        tagTemp = waitToSoldDel[i].split("_");
                        if (tagTemp[0] == "wuqiPackage") {
                            user.wuqiPackage[tagTemp[1]] = "";
                        } else if (tagTemp[0] == "yaoshuiPackage") {
                            user.yaoshuiPackage[tagTemp[1]] = "";
                            user.yaoshuiNum[tagTemp[1]] = 0;
                        }

                        if (user.yaoshuiCarry.indexOf(parseInt(tagTemp[1])) >= 0) {
                            user.yaoshuiCarry[user.yaoshuiCarry.indexOf(parseInt(tagTemp[1]))] = -1;
                        }
                    }
				
                    initButtomPackageBox();
                    initFooter();
                }
                waitToSoldDel = [];
                waitToSoldDelSpr = [];
                soldDelClicked = 0
            }
        }, this);
        soldDelSpr.setAnchorPoint(1, 0);
        soldDelSpr.setPosition(packageChangeBK.width-15, 0);
        soldDelSpr.setScale(packageChangeBK.height / soldDelSpr.height);

        var menu = new cc.Menu(soldDelSpr);
        menu.setPosition(0, 0);
        packageChangeBK.addChild(menu, 1);
    }
}


function compare(property){
    return function(a,b){
        var value1 = a[property];
        var value2 = b[property];
        return value1>value2;
    }
}
//package area
function initButtomPackageBox() {
	//描绘格子
	for (var i = 0; i < user.wuqiPackage.length;i++){
		if (user.wuqiPackage[i] == "") {
		    user.wuqiPackage.splice(i, 1);
		    i--;
		}
	}
	for (var i = 0; i < user.yaoshuiPackage.length; i++) {
		if (user.yaoshuiPackage[i] == "") {
			user.yaoshuiPackage.splice(i, 1);
			user.yaoshuiNum.splice(i, 1);
			i--;
		}
	}

	user.wuqiPackage.sort(compare("id"));
//	user.yaoshuiPackage.sort();
	var monoIndex = 0;
	var boxIndex = 0;
	var lockBoxIndex = 0;
	packageBoxArea.removeAllChildren(true);

	boxIndex = packagePageNow * (packageBoxNumY * packageBoxNumX);
	if (boxIndex > user.wuqiPackage.length + user.yaoshuiPackage.length) {
		monoIndex = user.wuqiPackage.length + user.yaoshuiPackage.length;
	} else {
		monoIndex = boxIndex;
	}
	if (boxIndex > user.openedPackageNum) {
		lockBoxIndex = boxIndex - user.openedPackageNum;
	}
	for (var y = 0; y < packageBoxNumY; y++) {
		for (var x = 0; x < packageBoxNumX; x++) {
			addMonoSprite(
				packageBoxArea,
				img_package_boxBG_empty,
				packageBoxSize * x + packageBoxSpaceLeftRight + packageBoxSize / 2,
				packageBoxArea.height - packageBoxSize * y - packageBoxSize / 2,
				5,
				""
			);

			var packageBoxTemp = null;
			if (boxIndex < user.openedPackageNum && packagePageNow < img_package_num - 1) { //当前盒子index 小于 解锁盒子数 and 不是carry用
				if (monoIndex < user.wuqiPackage.length) { //当前盒子index小于wuqi盒子数
					var soldDelFlg = false;
					if (soldDelFlg != 0 && waitToSoldDel.indexOf("wuqiPackage_" + monoIndex) >= 0) {
						soldDelFlg = true;
					}

					packageBoxTemp = addMonoSprite(
						packageBoxArea,
						wuqiFolder + user.wuqiPackage[monoIndex].id + ".png",
						packageBoxSize * x + packageBoxSpaceLeftRight + packageBoxSize / 2,
						packageBoxArea.height - packageBoxSize * y - packageBoxSize / 2,
						5,
						"wuqiPackage_" + monoIndex,
						user.wuqiPackage[monoIndex].special_type.length,
						soldDelFlg
					);
					cc.eventManager.addListener(listener_package_boxMove.clone(), packageBoxTemp);

					monoIndex++;
				} else if (monoIndex < (user.wuqiPackage.length + user.yaoshuiPackage.length)) { //当前盒子index 小于yaoshui加wuqi数
					var soldDelFlg = false;
					if (soldDelFlg != 0 && waitToSoldDel.indexOf("wuqiPackage_" + monoIndex) >= 0) {
						soldDelFlg = true;
					}
					packageBoxTemp = addMonoSprite(
						packageBoxArea,
						yaoshuiFolder + user.yaoshuiPackage[monoIndex - user.wuqiPackage.length] + ".png",
						packageBoxSize * x + packageBoxSpaceLeftRight + packageBoxSize / 2,
						packageBoxArea.height - packageBoxSize * y - packageBoxSize / 2,
						5,
						"yaoshuiPackage_" + (monoIndex - user.wuqiPackage.length),
						0,
						soldDelFlg
					);
					cc.eventManager.addListener(listener_package_boxMove.clone(), packageBoxTemp);

					showYaoshuiNum(packageBoxTemp, user.yaoshuiNum[monoIndex - user.wuqiPackage.length]);
					monoIndex++;
				}
			} else if (packagePageNow == img_package_num - 1) { //carry 用
				if (y < user.wuqiCarry.length && x <user.wuqiCarry[0].length && user.wuqiCarry[y][x] && user.wuqiCarry[y][x] != "") {
					packageBoxTemp = addMonoSprite(
							packageBoxArea,
							wuqiFolder + user.wuqiCarry[y][x].id + ".png",
							packageBoxSize * x + packageBoxSpaceLeftRight + packageBoxSize / 2,
							packageBoxArea.height - packageBoxSize * y - packageBoxSize / 2,
							5,
							"wuqiPackageCarry_" + x + "_" + y,
							user.wuqiCarry[y][x].special_type.length
						);
					cc.eventManager.addListener(listener_package_boxMove.clone(), packageBoxTemp);

					if (user.wuqiCarry[y][x].plusLevel > 0) {
						showWuqiPlusNum(packageBoxTemp, user.wuqiCarry[y][x].plusLevel);
					}
				}
			} else { //被锁的
				lockBoxIndex++;
				packageBoxTemp = addMonoSprite(
					packageBoxArea,
					img_package_boxBG_locked,
					packageBoxSize * x + packageBoxSpaceLeftRight + packageBoxSize / 2,
					packageBoxArea.height - packageBoxSize * y - packageBoxSize / 2,
					5,
					"lockPackage_" + lockBoxIndex,
					0
				);
				cc.eventManager.addListener(listener_unlockBox.clone(), packageBoxTemp);
			}
			boxIndex++;

		}
	}
}
//lockBox 解锁
var listener_unlockBox = cc.EventListener.create({
	event: cc.EventListener.TOUCH_ONE_BY_ONE,
	swallowTouches: true,
	onTouchBegan: function (touch, event) {
		//判断点击区域
		var target = event.getCurrentTarget();
		var pos = target.convertToNodeSpace(touch.getLocation());
		if (!cc.rectContainsPoint(target.getTextureRect(), pos)) return false;

		//show unlock message
		var nodeTempTag = this._node.getTag().split("_");
		if (nodeTempTag == null || nodeTempTag.length == 0) { //空box判断
			return false;
		}

		if (nodeTempTag[0] == "lockPackage" || nodeTempTag[0] == "lockPackageHome") {
			//click jinzhi
			cc.eventManager.pauseTarget(nowLayer, true);

			var buttonList = [];
			//background
/*			var unlockBoxSpriteBG = new cc.Sprite(img_map_backGround);
			unlockBoxSpriteBG.setAnchorPoint(0, 0);
			unlockBoxSpriteBG.setPosition(0, 0);
			unlockBoxSpriteBG.setScale(size.width / unlockBoxSpriteBG.width, size.height / unlockBoxSpriteBG.height);
			nowLayer.addChild(unlockBoxSpriteBG, 8);*/

			var spriteTemp = new cc.Sprite(img_dialog_center);
			spriteTemp.setAnchorPoint(0.5, 0.5);
			spriteTemp.setPosition(size.width / 2, size.height / 2);
			spriteTemp.setScale(size.width / spriteTemp.width);
			nowLayer.addChild(spriteTemp, 10);

			//message
			var message = new cc.LabelTTF("要解锁 " + nodeTempTag[1] + " 个格子吗？", "Impact", 40);
			message.setAnchorPoint(0.5, 0.5);
			message.setPosition(spriteTemp.width / 2, spriteTemp.height * 0.75);
			message.setColor(cc.color(0, 0, 0, 255));
			spriteTemp.addChild(message, 1);


			var ngSpr = new cc.MenuItemImage(img_dialog_ng_btn, img_dialog_ng_btn, function () {
			    spriteTemp.removeAllChildren();
			    nowLayer.removeChild(spriteTemp, true);
				cc.eventManager.resumeTarget(nowLayer, true);
			}, this);
			ngSpr.setAnchorPoint(0.5, 0.5);
			ngSpr.setPosition(spriteTemp.width / 4, spriteTemp.height / 4);
			buttonList.push(ngSpr);

			var okSpr = new cc.MenuItemImage(img_dialog_ok_btn, img_dialog_ok_btn, function () {
			    if (user.zuanshi >= parseInt(nodeTempTag[1]) * unlockBoxPrice) {
			        spriteTemp.removeAllChildren();
			        nowLayer.removeChild(spriteTemp, true);
					user.zuanshi = user.zuanshi - parseInt(nodeTempTag[1]) * unlockBoxPrice;
					if (nodeTempTag[0] == "lockPackage") {
						user.openedPackageNum = user.openedPackageNum + parseInt(nodeTempTag[1]);
					} else {
					    user.openedPackageHomeNum = user.openedPackageHomeNum + parseInt(nodeTempTag[1]);
					    initHomeLeft(3);
					}
					cc.eventManager.resumeTarget(nowLayer, true);
					initButtomPackageBox();
					initXuemo();
					saveUser();
				} else {
				    //TODO zuanshi buzu
				    cc.log("zan shi bu zu");
				}
			}, this);
			okSpr.setAnchorPoint(0.5, 0.5);
			okSpr.setPosition(spriteTemp.width * 0.75, spriteTemp.height / 4);
			buttonList.push(okSpr);

			//message
			var price = new cc.LabelTTF(parseInt(nodeTempTag[1]) * unlockBoxPrice + "" , "Impact", 35);
			price.setAnchorPoint(0.5, 0.5);
			price.setPosition(okSpr.width * 0.7, okSpr.height/2);
			price.setColor(cc.color(0, 255, 0, 255));
			okSpr.addChild(price, 1);

			var menu = new cc.Menu(buttonList);
			menu.setPosition(0, 0);
			spriteTemp.addChild(menu, 1);
		}
	},
	onTouchMoved: function (touch, event) {

	},
	onTouchEnded: function (touch, event) {

	}
});
//格子拖动listener
var listener_package_boxMove = cc.EventListener.create({
	event: cc.EventListener.TOUCH_ONE_BY_ONE,
	swallowTouches: true,						// 设置是否吞没事件，在 onTouchBegan 方法返回 true 时吞掉事件，不再向下传递。
	onTouchBegan: function (touch, event) {		//实现 onTouchBegan 事件处理回调函数

		//判断点击区域
//		var pos = this._node.convertToNodeSpace(touch.getLocation());
		var target = event.getCurrentTarget();
		var pos = target.convertToNodeSpace(touch.getLocation());
		if(!cc.rectContainsPoint(target.getTextureRect(), pos))return false;
		cc.log("onTouchBegan_boxMove2");

		var nodeTempTag =this._node.getTag().split("_");
		if (nodeTempTag == null || nodeTempTag.length == 0) { //空box判断
			return false;
		}

		nowBuyIndex = -1;

		if (nodeTempTag[0] == "yaoshuiCarry" && nodeTempTag[1]=="5") {
			openPackage(); //箱子按钮被点击
		} else if (nodeTempTag[0] == "yaoshuiCarry" && nodeTempTag[1] == "6") {
			openMenu(); //菜单按钮被点击
		} else if (nowPageName.indexOf("map") == 0 && userNowAct != "package") {
			// 药水使用
			cc.log("药水使用");
			if (user.yaoshuiNum[user.yaoshuiCarry[parseInt(nodeTempTag[1])]] > 0) {
				var yaoshuiInfo = getYaoshuiInfo(user.yaoshuiPackage[user.yaoshuiCarry[parseInt(nodeTempTag[1])]]);
				if (yaoshuiInfo.addXue > 0 && user.xueNow<user.property[xueNo]) {
					user.yaoshuiNum[user.yaoshuiCarry[parseInt(nodeTempTag[1])]]--;
					user.xueNow += yaoshuiInfo.addXue;
					if (user.xueNow > user.property[xueNo]) {
						user.xueNow = user.property[xueNo];
						showLabelInfo(0, 20, "+" + yaoshuiInfo.addXue, nowLayer.userSprite._position);
					}
				}
				if (yaoshuiInfo.addMo > 0 && user.moNow<user.property[moNo]) {
					user.yaoshuiNum[user.yaoshuiCarry[parseInt(nodeTempTag[1])]]--;
					user.moNow += yaoshuiInfo.addMo;
					if (user.moNow > user.property[moNo]) {
						user.moNow = user.property[moNo];
						showLabelInfo(1, 20, "+" + yaoshuiInfo.addMo, nowLayer.userSprite._position);
					}
				}
				initXuemo();
				initFooter();
			}
			return true;
		} else if (userNowAct == "package") {
			if (nodeTempTag[0] == "wuqiPackage") {
				initMiddleRight("wuqi", user.wuqiPackage[parseInt(nodeTempTag[1])], this._node);
			} else if (nodeTempTag[0] == "wuqiCarry" || nodeTempTag[0] == "wuqiPackageCarry") {
				initMiddleRight("wuqi", user.wuqiCarry[nodeTempTag[2]][parseInt(nodeTempTag[1])], this._node);
			} else if (nodeTempTag[0] == "yaoshuiPackage") {
				initMiddleRight("yaoshui", user.yaoshuiPackage[parseInt(nodeTempTag[1])], this._node);
			} else if (nodeTempTag[0] == "yaoshuiCarry") {
				initMiddleRight("yaoshui", user.yaoshuiPackage[user.yaoshuiCarry[parseInt(nodeTempTag[1])]], this._node);
			} else if (nodeTempTag[0] == "wuqiPackageHome") {
				initMiddleRight("wuqi", user.wuqiPackageHome[parseInt(nodeTempTag[1])], this._node);
			} else if (nodeTempTag[0] == "yaoshuiPackageHome") {
				initMiddleRight("yaoshui", user.yaoshuiPackageHome[parseInt(nodeTempTag[1])], this._node);
			} else if (nodeTempTag[0] == "plus") {
				initMiddleRight("wuqi", nowLayer.plusWuqi[parseInt(nodeTempTag[1])], this._node);
			}

			if (soldDelClicked != 0) {
				if ((nodeTempTag[0] == "wuqiPackage" || nodeTempTag[0] == "yaoshuiPackage")) {
				    if (waitToSoldDel.indexOf(this._node.getTag()) >= 0) {
				        var indexSoldDel = waitToSoldDel.indexOf(this._node.getTag());
						packageBoxArea.removeChild(waitToSoldDelSpr[indexSoldDel], true);
						waitToSoldDel.splice(indexSoldDel, 1);
						waitToSoldDelSpr.splice(indexSoldDel, 1);
					} else {
					    var soldDelSelectedSpr = new cc.Sprite(img_package_mono_selected); //TODO
						soldDelSelectedSpr.setAnchorPoint(0.5, 0.5);
						soldDelSelectedSpr.setPosition(this._node._position);
						soldDelSelectedSpr.setScale(packageBoxSize / soldDelSelectedSpr.width, packageBoxSize / soldDelSelectedSpr.height);
						packageBoxArea.addChild(soldDelSelectedSpr, 11);
						waitToSoldDel.push(this._node.getTag());
						waitToSoldDelSpr.push(soldDelSelectedSpr);
					}
				}
			} else {
				packageBoxMoveSprite = new cc.Sprite(this._node.getSpriteFrame());
				packageBoxMoveSprite.setAnchorPoint(0.5, 0.5);
				packageBoxMoveSprite.setPosition(touch.getLocation());
				packageBoxMoveSprite.setScale(packageBoxSize / packageBoxMoveSprite.width, packageBoxSize / packageBoxMoveSprite.height);
				nowLayer.addChild(packageBoxMoveSprite, 10);
				this._node.setVisible(false);
			}

			return true;
		}
	},
	onTouchMoved: function (touch, event) {			//实现onTouchMoved事件处理回调函数, 触摸移动时触发
		if (nowPageName.indexOf("map")==0 && userNowAct != "package") {
			// 药水使用
		} else if (userNowAct == "package") {
			if (soldDelClicked != 0) {
			} else {
				packageBoxMoveSprite.setPosition(touch.getLocation());
			}
		}
	},
	onTouchEnded: function (touch, event) {			// 实现onTouchEnded事件处理回调函数
		cc.log("onTouchEnded_boxMove");

		if (nowPageName.indexOf("map")==0 && userNowAct != "package") {
			// 药水使用

			initXuemo();
			return;
		}

		if (userNowAct != "package"){
			return;
		}

		if (soldDelClicked != 0) {
			return;
		}

		var nodeTempTag =this._node.getTag().split("_");
		if (nodeTempTag != null && nodeTempTag.length>=2) {
			if (nodeTempTag[0] == "wuqiPackage" || nodeTempTag[0] == "wuqiPackageCarry") { //从箱子拖wuqi
				//this._node.convertToNodeSpace(touch.getLocation());
				if (nowPageName == "home_person4" || nowPageName == "home_person5") { //plus
					for (var i = 0; nowLayer.plusWuqiSprite.length > 0 && i < nowLayer.plusWuqiSprite.length; i++) {
						var pos = nowLayer.plusWuqiSprite[i].convertToNodeSpace(touch.getLocation());
						if (!cc.rectContainsPoint(nowLayer.plusWuqiSprite[i].getTextureRect(), pos)) {
							continue;
						} else {
							if (nodeTempTag[0] == "wuqiPackageCarry") {
								if (i == 0) { //身上东西只可以被强化
									nowLayer.plusWuqi[i] = user.wuqiCarry[parseInt(nodeTempTag[2])][parseInt(nodeTempTag[1])];
								} else {
									nowLayer.removeChild(packageBoxMoveSprite, true);
									this._node.setVisible(true);

									return;
								}
							} else{
								nowLayer.plusWuqi[i] = user.wuqiPackage[parseInt(nodeTempTag[1])];
							} 
							
							if (nowLayer.plusWuqi[0] === nowLayer.plusWuqi[1]) {
								if (i==0) {
									nowLayer.plusWuqi[1] = null;
								} else {
									nowLayer.plusWuqi[0] = null;
								}
							}
							initHomeLeft(4);
							if (i == 1) { 
								plusWaitToDel = parseInt(nodeTempTag[1]);
							}
							nowLayer.removeChild(packageBoxMoveSprite, true);
							this._node.setVisible(true);

							return;
						}
						break;
					}
				} else { //tocarry
					for (var i = 0; packageUserCarryWuqiSprite.length > 0 && i < packageUserCarryWuqiSprite.length; i++) {
						var pos = packageUserCarryWuqiSprite[i].convertToNodeSpace(touch.getLocation());
						if (!cc.rectContainsPoint(packageUserCarryWuqiSprite[i].getTextureRect(), pos)) {
							continue;
						} else {
							var wuqiInfo = getWuqiInfo(user.wuqiPackage[parseInt(nodeTempTag[1])].id);
							if (wuqiInfo.type == i) { //位置正确
								var wuqiTemp = user.wuqiCarry[nowUserWuqiPage][i];
								user.wuqiCarry[nowUserWuqiPage][i] = user.wuqiPackage[parseInt(nodeTempTag[1])];
								if (wuqiTemp != null && wuqiTemp.length != 0) {
									user.wuqiPackage[parseInt(nodeTempTag[1])] = wuqiTemp;
								} else {
									user.wuqiPackage.splice(parseInt(nodeTempTag[1]), 1);
								}
								nowLayer.removeChild(packageBoxMoveSprite, true);
								initMiddleLeft();
								initButtomPackageBox();
								initXuemo();
								return;
							}
						}
						break;
					}
				}
			} else if (nodeTempTag[0] == "yaoshuiPackage") { //从箱子拖yaoshui
				for (var i = 0; i < img_menuNumber - img_menuNumber_notMono; i++) { //-1是把背包按钮除去
					var pos = buttomMenuSpr[i].convertToNodeSpace(touch.getLocation());
					if(!cc.rectContainsPoint(buttomMenuSpr[i].getTextureRect(), pos)) {//cc.rect(0, 0, buttomMenuSpr[i].width, buttomMenuSpr[i].height), pos)) {
						continue;
					} else {
						var yaoshuiInfo = getYaoshuiInfo(user.yaoshuiPackage[parseInt(nodeTempTag[1])]);
						if (yaoshuiInfo.type == 0 || yaoshuiInfo.type == 1) { //使用可
							if (user.yaoshuiCarry.indexOf(parseInt(nodeTempTag[1])) >= 0) {
								user.yaoshuiCarry[user.yaoshuiCarry.indexOf(parseInt(nodeTempTag[1]))] = -1;
							}
							user.yaoshuiCarry[i] = parseInt(nodeTempTag[1]);
							nowLayer.removeChild(packageBoxMoveSprite, true);
							initFooter();
							initButtomPackageBox();
							return;
						}
					}
					break;
				}
			} else if (nodeTempTag[0] == "wuqiCarry") { //从身上拖wuqi
				var pos = packageBoxArea.convertToNodeSpace(touch.getLocation());
				if(!cc.rectContainsPoint(packageBoxArea.getTextureRect(), pos)) {
					;
				} else {
					if (user.wuqiPackage.length + user.yaoshuiPackage.length + 1 <= user.openedPackageNum) { //有空
						user.wuqiPackage.push(user.wuqiCarry[nowUserWuqiPage][parseInt(nodeTempTag[1])]);
						user.wuqiCarry[nowUserWuqiPage][parseInt(nodeTempTag[1])]="";
						nowLayer.removeChild(packageBoxMoveSprite, true);
						initMiddleLeft();
						initButtomPackageBox();
						initXuemo();
						return;
					} else {
						showLabelInfo(0, 30, "背包满了", cc.p(nowLayer.width / 2, nowLayer.height / 2));
					}
				}
			} else if (nodeTempTag[0] == "yaoshuiCarry") { //从身上拖yaoshui
				var pos = packageBoxArea.convertToNodeSpace(touch.getLocation());
				if(cc.rectContainsPoint(packageBoxArea.getTextureRect(), pos)) { //拖到背包
					if (user.wuqiPackage.length + user.yaoshuiPackage.length + 1 <= user.openedPackageNum) { //有空
						user.yaoshuiCarry[nodeTempTag[1]] = -1;
						nowLayer.removeChild(packageBoxMoveSprite, true);
						initFooter();
						initButtomPackageBox();
						return;
					} else {
						showLabelInfo(0, 30, "仓库满了", cc.p(nowLayer.width / 2, nowLayer.height / 2));
					}
				} else {
					for (var i = 0; i < img_menuNumber - img_menuNumber_notMono; i++) { //-1是把背包按钮除去
						var pos = buttomMenuSpr[i].convertToNodeSpace(touch.getLocation());
						if(!cc.rectContainsPoint(buttomMenuSpr[i].getTextureRect(), pos)) {//cc.rect(0, 0, buttomMenuSpr[i].width, buttomMenuSpr[i].height), pos)) {
							continue;
						} else { //拖到menu
							var indexFrom = user.yaoshuiCarry[parseInt(nodeTempTag[1])];
							user.yaoshuiCarry[parseInt(nodeTempTag[1])] =user.yaoshuiCarry[i];
							user.yaoshuiCarry[i] = indexFrom;
							nowLayer.removeChild(packageBoxMoveSprite, true);
							initFooter();
							return;
						}
						break;
					}
				}
			} else if (nodeTempTag[0] == "yaoshuiPackageHome") { //从家里的仓库拖yaoshui
				var pos = packageBoxArea.convertToNodeSpace(touch.getLocation());
				if(!cc.rectContainsPoint(packageBoxArea.getTextureRect(), pos)) {
					;
				} else {
					if (userGetItem("yaoshui",user.yaoshuiPackageHome[nodeTempTag[1]],user.yaoshuiNumHome[nodeTempTag[1]])) {
						user.yaoshuiPackageHome.splice(parseInt(nodeTempTag[1]), 1);
						user.yaoshuiNumHome.splice(parseInt(nodeTempTag[1]), 1);
						initHomeLeft(3);
						initButtomPackageBox();
						initFooter();
						nowLayer.removeChild(packageBoxMoveSprite, true);
						return;
					}
				}
			} else if (nodeTempTag[0] == "wuqiPackageHome") { //从家里的仓库拖wuqi
				var pos = packageBoxArea.convertToNodeSpace(touch.getLocation());
				if(!cc.rectContainsPoint(packageBoxArea.getTextureRect(), pos)) {
					;
				} else {
					if (userGetItem("wuqi",user.wuqiPackageHome[nodeTempTag[1]],1)) {
						user.wuqiPackageHome.splice(parseInt(nodeTempTag[1]), 1);
						initHomeLeft(3);
						initButtomPackageBox();
						initFooter();
						nowLayer.removeChild(packageBoxMoveSprite, true);
						return;
					}
				}
			} else if (nodeTempTag[0] == "plus") { //从plus拖wuqi
				var pos = packageBoxArea.convertToNodeSpace(touch.getLocation());
				if (!cc.rectContainsPoint(packageBoxArea.getTextureRect(), pos)) {
					;
				} else {
					nowLayer.plusWuqi[parseInt(nodeTempTag[1])] = null;
					initHomeLeft(4);
					nowLayer.removeChild(packageBoxMoveSprite, true);
					initButtomPackageBox();
					return;
				}
			}
		}

//		var target = event.getCurrentTarget();
//		if(!cc.rectContainsPoint(target.getTextureRect(), pos))return false;
		nowLayer.removeChild(packageBoxMoveSprite, true);
		this._node.setVisible(true);
	}
});



//menu area
function initFooter() {
	if (footerSprite) {
		footerSprite.removeAllChildren(true);
		nowLayer.removeChild(footerSprite, true);
	}

	footerSprite = new cc.Sprite(img_menu_BG);
	footerSprite.setAnchorPoint(0, 0);
	footerSprite.setScale(mapSpaceButtom / footerSprite.height);
	footerSprite.setPosition(0,0);
	nowLayer.addChild(footerSprite, 1);

	//下部menu
	var minTemp = Math.min(mapSpaceButtom, size.width / img_menuNumber);
	var imgTemp = "";
	for (var i = 0; i < img_menuNumber; i++) {
		if (i < (img_menuNumber - img_menuNumber_notMono) && user.yaoshuiCarry[i] != -1) { //0-4
			addMonoSprite(
				footerSprite,
				img_package_boxBG_empty,
				nowLayer.width / img_menuNumber * i + nowLayer.width / img_menuNumber / 2,
				mapSpaceButtom / 2,
				1,
				"",
				0
			);

			imgTemp = yaoshuiFolder + user.yaoshuiPackage[user.yaoshuiCarry[i]] + ".png";
		} else if (i < (img_menuNumber - img_menuNumber_notMono)) {
			imgTemp = img_package_boxBG_empty;
		} else if (i == (img_menuNumber - img_menuNumber_notMono)) {
			imgTemp = img_menuPackage;
		} else {
			imgTemp = img_menuButton; //TODO
		}


		buttomMenuSpr[i] = new cc.Sprite(imgTemp);
		buttomMenuSpr[i].setAnchorPoint(0.5, 0.5);
		buttomMenuSpr[i].setTag("yaoshuiCarry_" + i);
		buttomMenuSpr[i].setScale(mapSpaceButtom / buttomMenuSpr[i].height);
		buttomMenuSpr[i].setPosition(nowLayer.width / img_menuNumber * i + nowLayer.width / img_menuNumber / 2, mapSpaceButtom / 2);

		if (i < (img_menuNumber - img_menuNumber_notMono) && user.yaoshuiCarry[i] != -1) { //0-4
			showYaoshuiNum(buttomMenuSpr[i], user.yaoshuiNum[user.yaoshuiCarry[i]]);
		}

		if (i >= (img_menuNumber - img_menuNumber_notMono) || user.yaoshuiCarry[i] != -1) {
			cc.eventManager.addListener(listener_package_boxMove.clone(), buttomMenuSpr[i]);
		}
		footerSprite.addChild(buttomMenuSpr[i], 1);
	}
}

function showYaoshuiNum(toSpr, num){
	var numLabel = new cc.LabelTTF(num + "", "Impact", 20);
	numLabel.setAnchorPoint(1,0);
	numLabel.setPosition(toSpr.width-5, 5);
	numLabel.setColor(cc.color(0, 0, 255, 255));
	toSpr.addChild(numLabel, 5);
}

function addMonoSprite(toSpr, ImgPath, posX, posY, pri,tag,colorType,soldDelFlg){
	var packageBoxTemp = new cc.Sprite(ImgPath);
	packageBoxTemp.setAnchorPoint(0.5, 0.5);
	packageBoxTemp.setPosition(posX, posY);
	packageBoxTemp.setScale(packageBoxSize / packageBoxTemp.width,packageBoxSize / packageBoxTemp.height);
	packageBoxTemp.setTag(tag);
	toSpr.addChild(packageBoxTemp, pri);

	if (colorType==1) { //蓝色
		var colorSqu = new cc.Sprite(img_package_boxBG_blue);
		colorSqu.setAnchorPoint(0, 0);
		colorSqu.setPosition(0, 0);
		colorSqu.setScale(packageBoxTemp.width / colorSqu.width,packageBoxTemp.height / colorSqu.height);
		colorSqu.setTag(tag);
		packageBoxTemp.addChild(colorSqu, 1);
	} else if (colorType==2) { //红色
		var colorSqu = new cc.Sprite(img_package_boxBG_red);
		colorSqu.setAnchorPoint(0, 0);
		colorSqu.setPosition(0, 0);
		colorSqu.setScale(packageBoxTemp.width / colorSqu.width,packageBoxTemp.height / colorSqu.height);
		colorSqu.setTag(tag);
		packageBoxTemp.addChild(colorSqu, 1);
	}

	if (soldDelFlg == true) {
		var soldDelSelectedSpr = new cc.Sprite(img_package_box_selected); //TODO
		soldDelSelectedSpr.setAnchorPoint(0.5, 0.5);
		soldDelSelectedSpr.setPosition(posX, posY);
		soldDelSelectedSpr.setScale(packageBoxSize / soldDelSelectedSpr.width, packageBoxSize / soldDelSelectedSpr.height);
		toSpr.addChild(soldDelSelectedSpr, pri+1);
	}

	return packageBoxTemp;
}

function showWuqiPlusNum(toSpr, plusNum){
	var plusLabel = new cc.LabelTTF("+" + plusNum, "Arial Bold", 25);
	plusLabel.setAnchorPoint(1, 1);
	plusLabel.setPosition(toSpr.width, toSpr.height);
	plusLabel.setColor(cc.color(255, 0, 0, 255));
	toSpr.addChild(plusLabel, 5);
}