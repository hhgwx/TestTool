var s_HelloWorld = "Normal/HelloWorld.jpg";
var s_CloseNormal = "Normal/CloseNormal.png";
var s_CloseSelected = "Normal/CloseSelected.png";


var img_package_changePage_BG = "package/package_area_backGround_changePage.png";
var img_package_changePage = "package/change_page.png";
var img_package_changePageActive = "package/change_page_active.png";

var img_package_backGround = "package/back_ground.png";
var img_package_areaBackGround = "package/package_area_backGround.png";
var img_package_boxBG_empty = "package/box_bg.png";
var img_package_boxBG_red = "package/box_bg_red.png";
var img_package_boxBG_blue = "package/box_bg_blue.png";
var img_package_boxBG_locked = "package/box_bg_locked.png";
var img_package_box_selected = "package/box_selected.png";

var img_package_sold_btn = "package/sold_btn.png";
var img_package_del_btn = "package/del_btn.png";
var img_package_soldDel_btn_clicked = "package/soldDel_btn_clicked.png";

var img_package_mono_selected = "package/mono_selected.png";

var img_package_top_BG = "package/package_top_backGround.png";

var img_dialog_center = "package/dialog_center.png";
var img_dialog_ng_btn = "package/dialog_ng_btn.png";
var img_dialog_ok_btn = "package/dialog_ok_btn.png";

var img_top_BG="xuemo/back_ground.png";
var img_top_xue="xuemo/xue.png";
var img_top_mo="xuemo/mo.png";
var img_top_jingyan="xuemo/jingyan.png";


var img_package_user_BG_left = "package/package_user_backGround_left.jpg";
var img_package_user_BG_right = "package/package_user_backGround_right.png";

var img_menu_BG = "menu/back_ground.png";
var img_menu_body_BG = "menu/menu_body_BG.png";

var img_map_backGround = "map/back_ground.png";
var img_map_infoBG = "map/info_bg.png";

var img_menuPackage = "menu/package.png";
var img_menuNumber = 7; //下部按钮总数
var img_menuNumber_notMono = 2; //非物品格子数
var img_menuButton = "menu/menu.png";

var img_addBtn = "menu/add_btn.png";
var img_lesBtn = "menu/les_btn.png";

var img_okBtn = "menu/ok_btn.png";
var img_ngBtn = "menu/ng_btn.png";

var img_home_backGround = "home/back_ground.png";
var img_homeMapEntrance = "home/map_entrance.png";
var img_homeSelectDiffcult = "home/select_level.png";
var img_homeSelectDiffcultPage = "home/select_diffcult_page.png";
var img_homeSelectLevelBtn = "home/select_level_btn.png";
var img_homeLeft_BG = "home/home_left_bg.png";
var img_buy = "home/buy.png";
var img_buy_clicked = "home/buy_clicked.png";
var img_plus = "home/plus.png";
var img_plus_clicked = "home/plus_clicked.png";
var img_addMagic = "home/addMagic.png";
var img_addMagic_clicked = "home/addMagic_clicked.png";


var img_home_package_bg = "home/home_package_bg.png";
var img_homePerson = [];
img_homePerson[0] = "home/0.png";
img_homePerson[1] = "home/1.png";
img_homePerson[2] = "home/2.png";
img_homePerson[3] = "home/3.png";
img_homePerson[4] = "home/4.png";
img_homePerson[5] = "home/5.png";

var img_direct_4 = [];
img_direct_4[0] = "direct/left.png";
img_direct_4[1] = "direct/up.png";
img_direct_4[2] = "direct/right.png";
img_direct_4[3] = "direct/down.png";
var img_direct_center = "direct/center.png";
var img_direct_bg = "direct/direct_bg.png";
var img_direct_bg_move = "direct/direct_bg_move.png";

var img_userMove = [];
var img_userAttack = [];
for (var i=0;i<4;i++) {
	img_userMove[i] = [];
	img_userAttack[i] = [];
	for (var j=0;j<4;j++) {
		img_userMove[i][j] = "user/move/user_" + i + "_" + j + ".png";
		img_userAttack[i][j] = "user/attack/user_" + i + "_" + j + ".png";
	}
}


var nowLayer;

var monsterFolder = "monster/";
var roadFolder = "road/";
var wallFolder = "wall/";
var wuqiFolder = "mono_wuqi/";
var wuqiGroupFolder = "mono_wuqi/group/";
var yaoshuiFolder = "mono_yaoshui/";

var direct = {"left":0,"up":1, "right":2, "down":3}; // 方向

var diffcultNumber = 4; // level类别
var getDiffcultType = { 0: "普通", 1: "噩梦", 2: "炼狱", 3: "超神" }; // level类别
var getMonsterType = { 7: "动物系", 8: "植物系", 9: "其他系"};

var getWuqiType = { 0: "项链", 1: "戒指", 2: "头盔", 3: "武器", 4: "衣服", 5: "护手", 6: "鞋子", 7: "裤子" }; // 武器类别
var getYaoshuiType = {0:"血药",1:"蓝药", 2:"强化用"}; // 药水类别
var getPropertyName = { 0: "攻击", 1: "防御", 2: "血量", 3: "魔量", 4: "爆击", 5: "闪避", 6: "穿透", 7: "动物系", 8: "植物系", 9: "其他系", 10: "全     系", 11: "幸运", 12: "经验", 13: "金币" };
var gongNo = 0;
var fangNo = 1;
var xueNo = 2;
var moNo = 3;
var baoNo = 4;
var shanNo = 5;
var chuanNo = 6;
var dongNo = 7;
var zhiNo = 8;
var otherNo = 9;
var allNo = 10;
var luckNo = 11;
var jingNo = 12;
var jinbiNo = 13;

var baoMaxValue = 60; //baoji max value
var shanMaxValue = 60; //baoji max value
var baoPlus = 2.5; //baoji *2.5
var nowPageName = "";

var unlockBoxPrice = 30;

//var goldBoxPer = 0.3;

var menuType = { 0: "技能", 1: "帮助", 2: "系统" };
var menuTypeNum = 3;

var jinengName = {
	"0_1": "jineng0_1",
	"0_2": "jineng0_2",
	"1_1": "jineng1_1",
	"1_2": "jineng1_2",
	"2_1": "jineng2_1",
	"2_2": "jineng2_2",
	"3_1": "jineng3_1",
	"3_2": "jineng3_2",
	"4_1": "jineng4_1",
	"4_2": "jineng4_2",
	"7_1": "jineng7_1",
	"7_2": "jineng7_2",
	"11_1": "jineng11_1",
	"11_2": "jineng11_2",
	"12_1": "jineng12_1",
	"12_2": "jineng12_2",
	"13_1": "jineng13_1",
	"13_2": "jineng13_2"
};
var jinengInfo = {
	"0_1": "jineng0_1_INFO",
	"0_2": "jineng0_2_INFO",
	"1_1": "jineng1_1_INFO",
	"1_2": "jineng1_2_INFO",
	"2_1": "jineng2_1_INFO",
	"2_2": "jineng2_2_INFO",
	"3_1": "jineng3_1_INFO",
	"3_2": "jineng3_2_INFO",
	"4_1": "jineng4_1_INFO",
	"4_2": "jineng4_2_INFO",
	"7_1": "jineng7_1_INFO",
	"7_2": "jineng7_2_INFO",
	"11_1": "jineng11_1_INFO",
	"11_2": "jineng11_2_INFO",
	"12_1": "jineng12_1_INFO",
	"12_2": "jineng12_2_INFO",
	"13_1": "jineng13_1_INFO",
	"13_2": "jineng13_2_INFO"
};
var getPlusPropertyInfo = [0.01,0.01,0.01,0.01,0.01,0.01,0.01,0.03,0.03,0.03,0.01,5,5,5];


var g_resources = [
    //image
    s_HelloWorld,
    s_CloseNormal,
    s_CloseSelected,

//    img_user,
//    img_user_walk,
//    img_user_att,

	img_package_backGround,
	img_package_areaBackGround,
	img_package_boxBG_empty,
	img_package_boxBG_red,
	img_package_boxBG_blue,
	img_package_boxBG_locked,
	img_package_box_selected,
	img_package_sold_btn,
	img_package_del_btn,
	img_package_soldDel_btn_clicked,
    img_package_mono_selected,
	img_package_changePage_BG,
	img_package_changePage,
	img_package_changePageActive,
	img_package_user_BG_left,
	img_package_user_BG_right,
	img_package_top_BG,
	img_dialog_center,
    img_dialog_ng_btn,
    img_dialog_ok_btn,

	img_top_BG,
	img_top_xue,
	img_top_mo,
	img_top_jingyan,

	img_home_backGround,
	img_homeMapEntrance,
	img_homeSelectDiffcult,
	img_homeSelectDiffcultPage,
	img_homeSelectLevelBtn,
	img_homeLeft_BG,
	img_buy,
	img_buy_clicked,
	img_plus,
	img_plus_clicked,
	img_addMagic,
	img_addMagic_clicked,
	img_home_package_bg,
	img_homePerson[0],
	img_homePerson[1],
	img_homePerson[2],
	img_homePerson[3],
	img_homePerson[4],
	img_homePerson[5],

	img_map_backGround,
	img_map_infoBG,
	img_menu_BG,
	img_menu_body_BG,

	img_menuPackage,
	img_menuButton,

	img_addBtn,
	img_lesBtn,

	img_okBtn,
	img_ngBtn,

	img_direct_4[0],
	img_direct_4[1],
	img_direct_4[2],
	img_direct_4[3],
	img_direct_center,
	img_direct_bg,
	img_direct_bg_move,

	//plist
	img_userMove[0][0],
	img_userMove[0][1],
	img_userMove[0][2],
	img_userMove[0][3],
	img_userMove[1][0],
	img_userMove[1][1],
	img_userMove[1][2],
	img_userMove[1][3],
	img_userMove[2][0],
	img_userMove[2][1],
	img_userMove[2][2],
	img_userMove[2][3],
	img_userMove[3][0],
	img_userMove[3][1],
	img_userMove[3][2],
	img_userMove[3][3],
	img_userAttack[0][0],
	img_userAttack[0][1],
	img_userAttack[0][2],
	img_userAttack[0][3],
	img_userAttack[1][0],
	img_userAttack[1][1],
	img_userAttack[1][2],
	img_userAttack[1][3],
	img_userAttack[2][0],
	img_userAttack[2][1],
	img_userAttack[2][2],
	img_userAttack[2][3],
	img_userAttack[3][0],
	img_userAttack[3][1],
	img_userAttack[3][2],
	img_userAttack[3][3]
    //fnt

    //tmx

    //bgm

    //effect
];

var userCarryPosX=[60,260,160,60,260,60,160,260];
var userCarryPosY=[330,330,330,200,200,60,60,60];

var bossArea = [-1]; //-1:boss
var moveArea = [0]; //0:road
var monsterArea = [1,2];
var supriseAreaClosed = [3,4,5]; //yin jin bossbox
var supriseAreaOpened = [6,7,8];
var cantMoveArea = [9, 10]; //wall
var outDoorArea = [11]; //door
var bossAllBoxNum = 16;
var bossStartY = 0;
var bossStartX = 3;

var mapArrWidth = 10; //0-9
var mapArrHeight = 16; //0-15

var packageWidth = 10;
var packageHeight = 10;

var img_package_num = 5; // 0-3是正常package 4是carry


function _tagWuqi() {
	this.id = "001_001";		//jsonID
	this.trueProperty = [];		//zhenshi pro
	this.plusLevel = 0;			//plus levle
	this.special_type = [];
	this.special_value = [];
	this.add_type = [];			//fumo
	this.add_value = [];		//fumo
}


function _tagMonster(isBoss){
	this.sprite;
	this.name = "";
	this.type = 0;
	this.pic = "";
	this.pic_att = "";
	this.property=[0,0,0,0];
	this.allXue=0;
	this.jinbi=0;
	this.jingyan=0;

	this.setValue = function (monValue, spriteValue) {
		this.sprite = spriteValue;
		this.name = monValue.name;
		this.type = monValue.type;
		this.pic = monValue.pic;
		this.pic_att = monValue.pic_att;
		this.jinbi = monValue.jinbi;
		this.allXue = monValue.allXue;
		this.jingyan = monValue.jingyan;

		var randomTemp= Math.random();
		if (!isBoss && randomTemp < 0.05) { //经验怪
			this.property=[monValue.property[0]*1.5,monValue.property[1]*1.5,monValue.property[2]*1.5,monValue.property[3]*1.5];
			this.allXue = monValue.allXue*1.5;
			this.jingyan = monValue.jingyan * 5;
			showWuqiPlusNum(this.sprite, "XP");
		} else if (!isBoss && randomTemp < 0.1) { //金币怪
			this.property=[monValue.property[0]*1.5,monValue.property[1]*1.5,monValue.property[2]*1.5,monValue.property[3]*1.5];
			this.allXue = monValue.allXue*1.5;
			this.jinbi = monValue.jinbi * 5;
			showWuqiPlusNum(this.sprite, "Gold");
		} else {
			this.property=monValue.property;
			this.allXue = monValue.allXue;
		}
	}
}
var user;
var monster;

///USER
function _tagUser()
{
 this.UserId = 0; //0-4
 this.UserName = ""; //5-

 this.Parse = function(VarTypeUser)
 {
  for (var i=0;i<VarTypeUser.length;i++)
   VarTypeUser[i] = String.fromCharCode(VarTypeUser.charCodeAt(i)^0xFFFF);
  this.UserId = VarTypeUser.substr(0,4); //0-4
  this.UserName = VarTypeUser.substr(5); //4--
 }
 this.ToString = function()
 {
  var str = this.UserId.toString();
  while (str.length<5)
  {
   str += "\u0000";
  }
  str += this.UserName;
  for (var i=0;i<str.length;i++)
  {
   str[i] = String.fromCharCode(str.charCodeAt(i)^0xFFFF);
  }
  return str;
 }
}

function getWuqiInfo(id) {
	var wuqiInfo = JSON.parse(cc.loader._loadTxtSync("res/" + wuqiFolder + id + ".json"));
	return wuqiInfo;
}
function getWuqiGroupInfo(id) {
	var wuqiGroupInfo = JSON.parse(cc.loader._loadTxtSync("res/" + wuqiGroupFolder + id + ".json"));
	return wuqiGroupInfo;
}
function getYaoshuiInfo(id) {
	var wuqiInfo = JSON.parse(cc.loader._loadTxtSync("res/" + yaoshuiFolder + id + ".json"));
	return wuqiInfo;
}