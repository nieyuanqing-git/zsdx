function SetHomeHeight() {
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
    myHeight = myHeight - $(".body-top").height();
    if ($(".banner-box")) myHeight = myHeight - $(".banner-box").height();
    if ($(".menu-box")) myHeight = myHeight - $(".menu-box").height();
    if ($(".footer")) myHeight = myHeight - $(".footer").outerHeight(true);
    var myLeftContentHeight = myHeight;
    var myRightContentHeight = myHeight;
    var myFullContentHeight = myHeight;
    if ($(".main-box-left h2")) myLeftContentHeight = myLeftContentHeight - $(".main-box-left h2").height();
    if ($(".main-box-right h2")) myRightContentHeight = myRightContentHeight - $(".main-box-right h2").height();
    if ($(".main-box-full h2")) myFullContentHeight = myFullContentHeight - $(".main-box-full h2").height();
    myLeftContentHeight = myLeftContentHeight - 40;
    myRightContentHeight = myRightContentHeight - 40;
    myFullContentHeight = myFullContentHeight - 40;
    if ($(".main-box-left .content-box") && $(".main-box-left .content-box").height() < myLeftContentHeight && $(".main-box-left .content-box").length == 1)
        $(".main-box-left .content-box").css("height", myLeftContentHeight);
    if ($(".main-box-right .content-box") && $(".main-box-right .content-box").height() < myRightContentHeight && $(".main-box-right .content-box").length == 1) 
        $(".main-box-right .content-box").css("height", myRightContentHeight);
    if ($(".main-box-full .content-box") && $(".main-box-full .content-box").height() < myFullContentHeight && $(".main-box-full .content-box").length == 1)
        $(".main-box-full .content-box").css("height", myFullContentHeight);
}

function setListEquimentListFix() {
    var detailpannellistwidth = $(".detail-pannel-list").width();
    var picOutWidth = $(".detail-pannel-list ul.float-l li.Equipment-Pic").outerWidth();
    var picWidth = $(".detail-pannel-list ul.float-l li.Equipment-Pic").width();
    $(".detail-pannel-list ul.float-l li.Equipment-Name").width(equipmentNameWidth);
    $(".detail-pannel-list ul.float-l li.Equipment-RoomName").width(equipmentRoomNameWidth);
    $(".detail-pannel-list ul.float-l li.Equipment-Button").width(detailpannellistwidth - picOutWidth - picWidth);
    $(".detail-pannel-list ul.float-l li").height(20);
    $(".detail-pannel-list ul.float-l li.Equipment-Button").height(24);
    $(".detail-pannel-list ul.float-l li.Equipment-Pic").height(50)
    $(".detail-pannel-list ul.header-title li").height(25);
}

$(function () {
    $("a").attr("hidefocus", "true");
    $("#sethomepage").click(function () {
        var url = location.href;
        try {
            this.style.behavior = "url(#default#homepage)";
            this.setHomePage(url);
        } catch (e) {
            if (window.netscape) {
                try {
                    netscape.security.PrivilegeManager.enablePrivilege("UniversalXPConnect");
                } catch (e) {
                    alert("此操作被浏览器拒绝！\n请在浏览器地址栏输入“about:config”并回车\n然后将 [signed.applets.codebase_principal_support]的值设置为'true',双击即可。");
                    return false;
                }
                var prefs = Components.classes["@mozilla.org/preferences-service;1"].getService(Components.interfaces.nsIPrefBranch);
                prefs.setCharPref('browser.startup.homepage', url);
            }
        }
        return false;
    });
    $("#setfavorite").click(function () {
        var title = document.title;
        var url = location.href;
        if (window.sidebar) {
            window.sidebar.addPanel(title, url, "");
        } else if (document.all) {
            window.external.AddFavorite(url, title);
        } else {
            return true;
        }
    });
    $("#setdesktop").click(function () {
        var title = document.title;
        var url = location.href;
        try {
            var WshShell = new ActiveXObject("WScript.Shell");
            var oUrlLink = WshShell.CreateShortcut(WshShell.SpecialFolders("Desktop") + "\\" + title + ".url");
            oUrlLink.TargetPath = url;
            oUrlLink.Save();
        } 
        catch (e) {
            alert("当前安全级别不允许操作！");
        }
    });


});
function setlihover() {
    $(".news-list li").mouseover(function () {
        $(this).addClass("hover");
    });
    $(".news-list li").mouseout(function () {
        $(this).removeClass("hover");
    });
}

function ajaxloadsuccess() {
    $("a").attr("hidefocus", "true");
    var browser = navigator.appName
    var b_version = navigator.appVersion
    if (b_version.indexOf(";") != -1) {
        var version = b_version.split(";");
        var trim_Version = version[1].replace(/[ ]/g, "");
        if (browser == "Microsoft Internet Explorer" && trim_Version == "MSIE6.0") {
            DD_belatedPNG.fix('.icon');
        }
    }
    $(".inner-right").removeClass("hide");
}
function dialogOpen() {
    $.messager.alert('提示', "出错了,请刷新重试", 'warning');
}
function ajaxActionLink(url, loadingElementId, updateTargetId, onSuccess, onFailure) {
    if (isAjax) {
        if (updateTargetId == "layoutCenterBox")
            location.href = '/Admin' + "?CenterBoxUrl=" + escape(url);
    }
    else
        location.href = url;
}

