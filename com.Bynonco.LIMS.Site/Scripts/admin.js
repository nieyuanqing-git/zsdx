$(function () {
    $.ajaxSetup({
        cache: false
    });
    if (IntervalsetSidebarHeight) {
        clearInterval(IntervalsetSidebarHeight)
    }
    IntervalsetSidebarHeight = setInterval("setSidebarHeight()", 1000);
    //顶部菜单
    $(".top-menu li a").click(function () {
        $(".top-menu li").removeClass('current');
        $(this).parent().addClass('current');
    });
    //菜单
    setLeftMenu();
    //角色
    $(".role-menu li .role-submenu").hide();
    $(".role-menu li a.nav-top-item").click(function () {
        $(this).parent().siblings().find(".role-submenu").slideUp("normal");
        $(this).next().slideToggle("normal");
        return false;
    });

    ajaxloadsuccess();
});


function changeMainMenu(menuList, menuIcon, menuType) {
    if (menuType == "no-submenu-list") {
        menuList.parent().siblings().find(".main-submenu").slideUp("normal");
        $(".main-menu-list li a.nav-top-item").removeClass('current');
        $(".main-menu-list li a.no-submenu").removeClass('current');
        menuList.addClass('current');

        $(".main-submenu-icon").css("display", "none");
        $(".main-menu-icon li a.nav-top-item").removeClass('current');
        $(".main-menu-icon li a.no-submenu").removeClass('current');
        menuIcon.addClass('current');
    }
    else if (menuType == "no-submenu-icon") {
        menuList.parent().siblings().find(".main-submenu").css("display", "none");
        $(".main-menu-list li a.nav-top-item").removeClass('current');
        $(".main-menu-list li a.no-submenu").removeClass('current');
        menuList.addClass('current');

        $(".main-submenu-icon").slideUp("normal");
        $(".main-menu-icon li a.nav-top-item").removeClass('current');
        $(".main-menu-icon li a.no-submenu").removeClass('current');
        menuIcon.addClass('current');
    }
    else if (menuType == "nav-top-item-list") {
        menuList.parent().siblings().find(".main-submenu").slideUp("normal");
        $(".main-submenu-icon").css("display", "none");
        if (menuList.next().css("display") != "block")
            menuIcon.css("display", "block"); //子菜单显示
        menuList.next().slideToggle("normal");
    }
    else if (menuType == "nav-top-item-icon") {
        menuList.parent().siblings().find(".main-submenu").css("display", "none");
        if (menuIcon.css("display") != "block") {
            $(".main-submenu-icon").slideUp("normal");
            menuList.next().css("display","block"); //子菜单显示
            menuIcon.slideToggle("normal");
        }
        else {
            menuList.next().css("display", "none"); //子菜单显示
            $(".main-submenu-icon").slideUp("normal");
        }
    }
    else if (menuType == "submenu") {
        $(".main-submenu ul.submenu li a").removeClass('current');
        menuList.parents("ul.main-menu-list").find("a").removeClass('current');
        var parentmenu = menuList.parents(".main-submenu").parent().find("a.nav-top-item");
        parentmenu.addClass('current');
        menuList.addClass('current');

        $(".main-submenu-icon ul.submenu-icon li a").removeClass('current');
        $(".main-menu-icon li a").removeClass('current');
        $("#" + parentmenu.attr("id").replace("list", "icon")).addClass('current');
        menuIcon.addClass('current');
    }
    $(".top-menu li").removeClass('current'); //顶部菜单取消高亮
}

function setLeftMenu() {
    $(".main-menu-list li .main-submenu").hide();
    $(".main-menu-list li .main-submenu a.current").parents(".main-menu-list li .main-submenu").css("display", "block");
    $(".main-submenu-icon").hide();
    $(".main-submenu-icon a.current").parents(".main-submenu-icon").css("display", "block");
    $(".main-menu-list li a.no-submenu").click(function () {
        var menuList = $(this);
        var menuIcon = $("#" + $(this).attr("id").replace("list", "icon"));
        changeMainMenu(menuList, menuIcon, "no-submenu-list");
    });
    $(".main-menu-list li a.nav-top-item").click(function () {
        var menuList = $(this);
        var menuIcon = $("#" + $(this).attr("id").replace("list", "iconsubbox"));
        changeMainMenu(menuList, menuIcon, "nav-top-item-list");
        return false;
    });
    $(".main-submenu ul.submenu li a").click(function () {
        var menuList = $(this);
        var menuIcon = $("#" + $(this).attr("id").replace("list", "icon"));
        changeMainMenu(menuList, menuIcon, "submenu");
    });

    $(".main-menu-icon li a.no-submenu").click(function () {
        var menuList = $("#" + $(this).attr("id").replace("icon", "list"));
        var menuIcon = $(this);
        changeMainMenu(menuList, menuIcon, "no-submenu-icon");
    });
    $(".main-menu-icon li a.nav-top-item").click(function () {
        var menuList = $("#" + $(this).attr("id").replace("icon", "list"));
        var menuIcon = $("#" + $(this).attr("id").replace("icon", "iconsubbox"));
        changeMainMenu(menuList, menuIcon, "nav-top-item-icon");
        return false;
    });

    $(".main-submenu-icon ul.submenu-icon li a").click(function () {
        var menuList = $("#" + $(this).attr("id").replace("icon", "list"));
        var menuIcon = $(this);
        changeMainMenu(menuList, menuIcon, "submenu");
    });
}
/* 调用ajax请求action
 * 参数：访问地址、加载时等待元素ID、更新目标ID、成功处理、失败处理、持续使用ajax
 */
function ajaxActionLink(url, loadingElementId, updateTargetId, onSuccess, onFailure, isAllwayAjax) {
    try {
        if (isAjax || (isAllwayAjax && isAllwayAjax == true)) {
            ajaxloadbegin();
            if (loadingElementId != "") $("#" + loadingElementId).css("display", "block");
            $("#" + updateTargetId).html("");
            $("#" + updateTargetId).load(url, function () {
                if (loadingElementId != "") $("#" + loadingElementId).css("display", "none");
                onSuccess();
            });
        }
        else location.href = url;
    }
    catch (err) {
        $("#" + loadingElementId).css("display", "none");
        onFailure();
    }
}
/*ajax请求加载开始*/
function ajaxloadbegin() {
    //清空标签
    if (isAjax) {
        $("html, body").animate({ scrollTop: 0 }, 120);
        $("#Loading").nextAll().each(function () {
            if ($(this).attr("class") != "t_UpdateQueue" && $(this).attr("class") != "backToTop")
                $(this).remove();
        });
    }
}

function ajaxloadsuccess() {
    //tab
    $('.main-content-box .main-title ul.content-box-tabs li').click(
	    function () {
	        $(this).siblings().removeClass('current'); 
	        $(this).addClass('current'); 
	    }
    );
	$("a").attr("hidefocus", "true");
	var browser = navigator.appName
	var b_version = navigator.appVersion
	if (b_version.indexOf(";") != -1) {
	    var version = b_version.split(";");
	    var trim_Version = version[1].replace(/[ ]/g, "");
	    if (browser == "Microsoft Internet Explorer" && trim_Version == "MSIE6.0") {
	        DD_belatedPNG.fix('.inner-right, .icon, .tree-folder');
	    }
	}
	if (viewWidth != 0 && $(".datagrid-view").length > 0) setTimeout("SetGridFix()", "2000");
	$(".fix-box").remove();
	$(".backToTop").click();
}

function setSidebarHeightTimeOut() {
    setTimeout("setSidebarHeight()","3000")
}

var viewWidth = 0;

function setSidebarHeight() {

    var myWidth = 0, myHeight = 0;
    if (typeof (window.innerWidth) == 'number') {
        //Non-IE
        myWidth = window.innerWidth;
        myHeight = window.innerHeight;
    }
    else if (document.documentElement && (document.documentElement.clientWidth || document.documentElement.clientHeight)) {
        //IE 6+ in 'standards compliant mode'
        myWidth = document.documentElement.clientWidth;
        myHeight = document.documentElement.clientHeight;
    }
    else if (document.body && (document.body.clientWidth || document.body.clientHeight)) {
        //IE 4 compatible
        myWidth = document.body.clientWidth;
        myHeight = document.body.clientHeight;
    }

    if (isIE6) {
        var myscrollTop = document.documentElement.scrollTop;
        $("#Loading").css("height", myHeight);
        $("#Loading").css("top", myscrollTop);
    }
    
    myHeight = myHeight - 52 - 3; //52:Top高度; 3:border-bottom
    var contentheight = 0;
    var centerboxheight = $("#layoutCenterBox").height();
    if ($("#layoutLeftBox").height() > $("#layoutCenterBox").height())
        contentheight = $("#layoutLeftBox").height();
    else
        contentheight = centerboxheight;
    contentheight = contentheight + 40; //  20px的padding + 10px的底部间隔
    centerboxheight = centerboxheight + 40;
    if (myHeight <= contentheight)
        $("#layoutbox").css("height", contentheight);
    else {
        $("#layoutbox").css("height", myHeight);
    }
    
    if (myHeight > centerboxheight + $(".layout-foot").outerHeight() && $(".layout-center-full").length==1) {
        var objbox = "<div class='fix-box' style='height:" + (myHeight - centerboxheight - $(".layout-foot").outerHeight()) + "px;'></div>";
        if ($(".layout-center-full .edit-box").length > 0) $(".layout-center-full .edit-box").append(objbox);
        else $(".layout-center-full").append(objbox);
    }

    if (viewWidth != myWidth) { //浏览器宽度发生变化时才执行
        SetGridFix();
        setEquimentListFix();
        viewWidth = myWidth;
        $(".fix-box").remove();
    }
    if (isIE6 || isIE7) $(".main-content-box .main-title").height($(".main-content-box .main-title ul.content-box-tabs").height());
}
function SetGridFix() {
    var myWidth = 0, myHeight = 0;
    if (typeof (window.innerWidth) == 'number') {
        //Non-IE
        myWidth = window.innerWidth;
    }
    else if (document.documentElement && (document.documentElement.clientWidth || document.documentElement.clientHeight)) {
        //IE 6+ in 'standards compliant mode'
        myWidth = document.documentElement.clientWidth;
    }
    else if (document.body && (document.body.clientWidth || document.body.clientHeight)) {
        //IE 4 compatible
        myWidth = document.body.clientWidth;
    }

    var browser = navigator.appName
    var b_version = navigator.appVersion
    var isIE6 = false;
    if (b_version.indexOf(";") != -1) {
        var version = b_version.split(";");
        var trim_Version = version[1].replace(/[ ]/g, "");
        if (browser == "Microsoft Internet Explorer" && trim_Version == "MSIE6.0") {
            isIE6 = true;
        }
    }
    if (myWidth < 1000) {
        myWidth = 1000;
        $("body").css("width", myWidth);
    }
    else
        $("body").css("width", "auto");
    var maincontent_w = $(".main-content").width();
    var layoutbody_border = 2;
    var layoutcenter_left = 240;
    var body_border = 3;
    var layoutcenter_padding = 13;
    var maincontentbox_border = 2;
    var maincontent_margin = 20;
    var maincontent_border = 0;
    var maincontent_right_left = 310;
    var maincontent_w_r = maincontent_w - maincontent_right_left;
    if (isIE6) {
        $("body").css("width", myWidth);
        $(".layout-top").css("width", myWidth);
        $(".layout-body").css("width", myWidth - layoutbody_border * 2);
        $(".layout-center").css("width", myWidth - layoutcenter_left - layoutcenter_padding - body_border * 2);
        $(".main-content-box").css("width", myWidth - layoutcenter_left - layoutcenter_padding - body_border * 2 - maincontentbox_border * 2);
        maincontent_w = myWidth - layoutcenter_left - layoutcenter_padding - body_border * 2 - maincontentbox_border * 2 - maincontent_margin - maincontent_border * 2;
        $(".main-content").css("width", maincontent_w);
        maincontent_w_r = maincontent_w - maincontent_right_left;
        $(".main-content .layout-center-right").css("width", maincontent_w_r);
    }
    var dataGrids = $("div.datagrid div.datagrid-view table[fixwidth=true]");
    if (dataGrids.length > 0) return;
    //内容部分 浏览器伸缩变化
    $(".layout-center-full .panel-header").css("width", maincontent_w - 12);
    $(".layout-center-full .panel-body").css("width", maincontent_w - 2);
    //内容right部分 浏览器伸缩变化
    $(".layout-center-right .panel-header").css("width", maincontent_w_r - 12);
    $(".layout-center-right .panel-body").css("width", maincontent_w_r - 2);
    if (($(".datagrid-view").length > 0 || $(".datagrid-view2").length > 0)) {
        $(".layout-center-full .datagrid-view").css("width", maincontent_w - 2);
        $(".layout-center-full .datagrid-view").css("overflow-x", "auto");
        $(".layout-center-right .datagrid-view").css("width", maincontent_w_r - 2);
        $(".layout-center-right .datagrid-view").css("overflow-x", "auto");
        //滚动列效果
        $(".datagrid-view2").each(function () {
            var myleftstr = $(this).css("left");
            var pwidth = $(this).parent(".datagrid-view").width();
            var myleft = 0;
            var mywidth = 300;
            if (myleftstr != "")
                myleft = parseInt(myleftstr);
            if (myleft != 0 && pwidth > myleft)
                mywidth = pwidth - myleft;
            $(this).css("width", mywidth);
            $(this).find(".datagrid-header").css("width", mywidth);
            $(this).find(".datagrid-body").css("width", mywidth);
            $(this).find(".datagrid-footer").css("width", mywidth);
        });
        $(".datagrid-view").each(function () {
            var herderheight = 0;
            var bodyheight = 0;
            var footheight = 0;
            var thisheight = 0;
            herderheight = $(this).find(".datagrid-header").height() + 1; //1:boder-bottom
            bodyheight = $(this).find(".datagrid-body").height();
            footheight = $(this).find(".datagrid-footer").height();
            thisheight = herderheight + bodyheight + footheight;
            var newbodyheight = $(this).height() - herderheight - footheight;
            if ($(this).find(".datagrid-body").css("margin-bottom")) {
                newbodyheight = newbodyheight - parseInt($(this).find(".datagrid-body").css("margin-bottom"));
            }
            $(this).find(".datagrid-body").height(newbodyheight);
        });
    }
}
function dialogOpen() {
    $.messager.alert('提示', "出错了,请刷新重试", 'warning');
}
function setEquimentListFix() {
    if ($("#EquipmentList").length > 0 || $("#NMPList").length > 0) {
        setIconEquimentListFix();
        setListEquimentListFix();
    }
}
function setIconEquimentListFix() {
    var detailpannelwidth = $(".detail-pannel").width();
    if (detailpannelwidth < 670) detailpannelwidth = 670;
    var listleftouterWidth = $(".detail-pannel .list-left").outerWidth();
    var oldlistrightwidth = $(".detail-pannel .list-right").width();
    var oldlistrightouterWidth = $(".detail-pannel .list-right").outerWidth();
    var newlistrightwidth = detailpannelwidth - listleftouterWidth - (oldlistrightouterWidth - oldlistrightwidth) - 20; //20为避免出现滚动条导致
    var newinnerleftwidth = newlistrightwidth - $(".detail-pannel .list-right .inner-right").outerWidth();
    $(".detail-pannel .list-right").width(newlistrightwidth);
    $(".detail-pannel .list-right .inner-left").width(newinnerleftwidth);
    $(".inner-right").removeClass("hide");
}
function setListEquimentListFix() {
    var detailpannellistwidth = $(".detail-pannel-list").width();
    if (detailpannellistwidth < 710) detailpannellistwidth = 710;
    var equipmentNameWidth = 200;
    var equipmentRoomNameWidth = 150;
    var perAdd = detailpannellistwidth - 710
    var picOutWidth = $(".detail-pannel-list ul.float-l li.Equipment-Pic").outerWidth();
    var picWidth = $(".detail-pannel-list ul.float-l li.Equipment-Pic").width();
    $(".detail-pannel-list ul.float-l li.Equipment-Name").width(equipmentNameWidth + perAdd);
    $(".detail-pannel-list ul.float-l li.Equipment-RoomName").width(equipmentRoomNameWidth);
    $(".detail-pannel-list ul.float-l li.Equipment-Button").width(detailpannellistwidth - picOutWidth - picWidth);
    $(".detail-pannel-list ul.float-l li").height(20);
    $(".detail-pannel-list ul.float-l li.Equipment-Button").height(24);
    $(".detail-pannel-list ul.float-l li.Equipment-Pic").height(50)
    $(".detail-pannel-list ul.header-title li").height(25);
}
$(function () {
    var $backToTopTxt = "返回顶部", $backToTopEle = $('<div class="backToTop"></div>').appendTo($("body"))
        .text($backToTopTxt).attr("title", $backToTopTxt).click(function () {
            $("html, body").animate({ scrollTop: 0 }, 120);
        }), $backToTopFun = function () {
            var st = $(document).scrollTop(), winh = $(window).height();
            (st > 0) ? $backToTopEle.show() : $backToTopEle.hide();
            //IE6下的定位

            if (!window.XMLHttpRequest) {
                $backToTopEle.css("top", st + winh - 166);
                $(".body-top").css("top", st);
            }
        };
    $(window).bind("scroll", $backToTopFun);
    $(function () { $backToTopFun(); });
});