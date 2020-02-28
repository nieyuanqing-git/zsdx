//dhKeyBoard
//author:dh20156
var dhKeyBoard = function(_instance){
	this.instance = _instance;

	//dhLayer part
	this.oPopUp = null;
	this.drag = null;
	this.ObjectId = null;

	var browser = {
		isMSIE:(navigator.appName == "Microsoft Internet Explorer"),
		isNSFF:(navigator.appName == "Netscape"),
		isOpera:(navigator.appName == "Opera")
	}

	//IE background cache
	if(browser.isMSIE) document.execCommand("BackgroundImageCache", false, true);

	this.imgKeyBoard = new Image();
	this.imgKeyBoard.src = "../../JqueryPlugin/VirtualKeyboard/keyboard.png";

	//Keyboard Row 1
	var strHtml = new Array();
	strHtml[0] = '<ul class="row1 clearfix"><li id="k_shiftState">O</li><li id="k_capsState">O</li>';
	strHtml[1] = '<li class="key" id="k_esc">&nbsp;</li>';
	strHtml[2] = '<li class="key" id="k_1">&nbsp;</li>';
	strHtml[3] = '<li class="key" id="k_2">&nbsp;</li>';
	strHtml[4] = '<li class="key" id="k_3">&nbsp;</li>';
	strHtml[5] = '<li class="key" id="k_4">&nbsp;</li>';
	strHtml[6] = '<li class="key" id="k_5">&nbsp;</li>';
	strHtml[7] = '<li class="key" id="k_6">&nbsp;</li>';
	strHtml[8] = '<li class="key" id="k_7">&nbsp;</li>';
	strHtml[9] = '<li class="key" id="k_8">&nbsp;</li>';
	strHtml[10] = '<li class="key" id="k_9">&nbsp;</li>';
	strHtml[11] = '<li class="key" id="k_0">&nbsp;</li>';
	strHtml[12] = '<li class="key" id="k_dashes">&nbsp;</li>';
	strHtml[13] = '<li class="key" id="k_plus">&nbsp;</li></ul>';
	//Keyboard Row 2
	strHtml[14] = '<ul class="row2 clearfix"><li id="k_list"><textarea id="ChiList" readonly="readonly"></textarea><div id="tempwordsshow">&nbsp;</div><input type="button" class="btn-up" value="▲" title="上一屏" onclick="'+this.instance+'.pu();"><span id="imemodulenow">English</span><input type="button" class="btn-down" value="▼" title="下一屏" onclick="'+this.instance+'.pd();"></li>';
	strHtml[15] = '<li class="key" id="k_q">&nbsp;</li>';
	strHtml[16] = '<li class="key" id="k_w">&nbsp;</li>';
	strHtml[17] = '<li class="key" id="k_e">&nbsp;</li>';
	strHtml[18] = '<li class="key" id="k_r">&nbsp;</li>';
	strHtml[19] = '<li class="key" id="k_t">&nbsp;</li>';
	strHtml[20] = '<li class="key" id="k_y">&nbsp;</li>';
	strHtml[21] = '<li class="key" id="k_u">&nbsp;</li>';
	strHtml[22] = '<li class="key" id="k_i">&nbsp;</li>';
	strHtml[23] = '<li class="key" id="k_o">&nbsp;</li>';
	strHtml[24] = '<li class="key" id="k_p">&nbsp;</li>';
	strHtml[25] = '<li class="key" id="k_backspace">&nbsp;</li></ul>';
	//Keyboard row 3
	strHtml[26] = '<ul class="row3 clearfix"><li class="key" id="k_capsLock">&nbsp;</li>';
	strHtml[27] = '<li class="key" id="k_a">&nbsp;</li>';
	strHtml[28] = '<li class="key" id="k_s">&nbsp;</li>';
	strHtml[29] = '<li class="key" id="k_d">&nbsp;</li>';
	strHtml[30] = '<li class="key" id="k_f">&nbsp;</li>';
	strHtml[31] = '<li class="key" id="k_g">&nbsp;</li>';
	strHtml[32] = '<li class="key" id="k_h">&nbsp;</li>';
	strHtml[33] = '<li class="key" id="k_j">&nbsp;</li>';
	strHtml[34] = '<li class="key" id="k_k">&nbsp;</li>';
	strHtml[35] = '<li class="key" id="k_l">&nbsp;</li>';
	strHtml[36] = '<li class="key" id="k_enter">&nbsp;</li></ul>';
	//Keyboard row 4
	strHtml[37] = '<ul class="row4 clearfix"><li class="key" id="k_shift">&nbsp;</li>';
	strHtml[38] = '<li class="key" id="k_z">&nbsp;</li>';
	strHtml[39] = '<li class="key" id="k_x">&nbsp;</li>';
	strHtml[40] = '<li class="key" id="k_c">&nbsp;</li>';
	strHtml[41] = '<li class="key" id="k_v">&nbsp;</li>';
	strHtml[42] = '<li class="key" id="k_b">&nbsp;</li>';
	strHtml[43] = '<li class="key" id="k_n">&nbsp;</li>';
	strHtml[44] = '<li class="key" id="k_m">&nbsp;</li>';
	strHtml[45] = '<li class="key" id="k_comma">&nbsp;</li>';
	strHtml[46] = '<li class="key" id="k_point">&nbsp;</li>';
	strHtml[47] = '<li class="key" id="k_question">&nbsp;</li></ul>';
	//Keyboard row 5
	strHtml[48] = '<ul class="row5 clearfix"><li class="key" id="k_en">&nbsp;</li>';
	strHtml[49] = '<li class="key" id="k_py">&nbsp;</li>';
	strHtml[50] = '<li class="key" id="k_wb">&nbsp;</li>';
	strHtml[51] = '<li class="key" id="k_space">&nbsp;</li></ul>';

	this.keyboardStr = strHtml;
	this.capslocked = false;
	this.imemodule = "en";
	this.shiftmodule = "down";
	this.tempwords = "";

	this.LastNo = 0;
	this.spaceChar = " ";
	this.OutChi = [];
	this.OutEng = [];
	this.AsciiStr = "a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z,/,',;,[,]".split(',');

	//ime py words lib
	this.CodeList = CodeList.split(',');
	//end

	//ime wb words lib
	this.imestr1 = imestr1.split(";");
	this.imestr2 = imestr2.split(";");
	//end

	this.imeindex = [0,715,927,1110,1564,1888,2345,2743,2974,3634,4037,4666,4916,5253,5636,5872,6210,6947,7554,8086,8699,9223,9516,10046,10397,10942];
	this.imeindex2 = ["1. ","2. ","3. ","4. ","5. ","6. ","7. ","8. ","9. ","0. "];
	this.ascstr = "a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z".split(",");
	this.txt1 = 0;
	this.txt2 = 0;
	this.l = 0;
	this.teststr = null;
	this.teststr2 = [];
	this.testlength = 0;
	this.init();
}

dhKeyBoard.prototype = {
    init: function () {
        if (Function.prototype.bind == undefined) {
            Function.prototype.bind = function (_obj) {
                var owner = this;
                return function () {
                    owner.apply(_obj, arguments);
                }
            }
        };
    },
    $: function (objid) {
        return document.getElementById(objid);
    },
    getElementpos: function (e) {
        var t = e.offsetTop;
        var l = e.offsetLeft;
        var w = e.offsetWidth;
        var h = e.offsetHeight;
        while (e = e.offsetParent) {
            t += e.offsetTop;
            l += e.offsetLeft;
        }
        t += h;
        return { top: t, left: l, width: w, height: h }
    },
    showKeyBoard: function (obj) {
        this.ObjectId = obj;
        var pos = this.getElementpos(obj)
        var leftpos = (document.body.offsetWidth - 865) / 2 + document.body.scrollLeft;
        var toppos = (document.body.offsetHeight - 325) / 2 + document.body.scrollTop;
        toppos = Math.max(toppos, pos.top);
        if (this.oPopUp == null) {
            this.oPopUp = new dhlayer();
            this.oPopUp.padding = 0;
            this.oPopUp.background = "#FDFDFD url(" + this.imgKeyBoard.src + ") center center no-repeat";
            this.oPopUp.border = "1px solid black";
            this.oPopUp.cursor = "move";
            this.oPopUp.content = this.keyboardStr.join("");
            this.oPopUp.body.onclick = this.pressBord.bind(this);
            this.oPopUp.init(leftpos, toppos, 865, 325);
            this.drag = new dragDrop(this.oPopUp.window);
        } else {
            this.oPopUp.show(leftpos, toppos);
        }
    },
    exitKeyBoard: function () {
        this.oPopUp.hide();
    },
    pressBord: function (e) {
        e = e || window.event;
        var obj = e.srcElement || e.target;
        if (obj.tagName == 'LI' && obj.className == 'key') {
            this.pressKey(obj);
        }
    },
    pressKey: function (obj) {
        obj.style.background = "#76B3E9";
        window.setTimeout(function () { obj.style.background = 'none' }, 100);
        switch (obj.id) {
            case "k_esc":
                this.exitKeyBoard();
                break;
            case "k_capsLock":
                this.$("k_capsState").style.color = (this.capslocked) ? "" : "#E9FE00";
                this.capslocked = (this.capslocked) ? false : true;
                break;
            case "k_en":
                this.$("imemodulenow").innerHTML = "English";
                this.imemodule = "en";
                this.tempwords = "";
                this.$("ChiList").value = "";
                this.$("tempwordsshow").innerHTML = this.tempwords;
                break;
            case "k_py":
                this.$("imemodulenow").innerHTML = "拼音";
                this.imemodule = "py";
                this.tempwords = "";
                this.$("ChiList").value = "";
                this.$("tempwordsshow").innerHTML = this.tempwords;
                break;
            case "k_wb":
                this.$("imemodulenow").innerHTML = "五笔";
                this.imemodule = "wb";
                this.tempwords = "";
                this.$("ChiList").value = "";
                this.$("tempwordsshow").innerHTML = this.tempwords;
                break;
            case "k_shift":
                this.$("k_shiftState").style.color = (this.shiftmodule == "up") ? "" : "#E9FE00";
                this.shiftmodule = (this.shiftmodule == "down") ? "up" : "down";
                break;
            case "k_backspace":
                if (this.tempwords != "") {
                    this.tempwords = this.tempwords.substring(0, this.tempwords.length - 1);
                    this.$("tempwordsshow").innerHTML = this.tempwords;
                    if (this.imemodule == "py") {
                        this.Grep(this.tempwords);
                    } else {
                        this.wbx();
                    }
                } else {
                    this.ObjectId.value = this.ObjectId.value.substring(0, this.ObjectId.value.length - 1);
                }
                break;
            case "k_enter":
                this.exitKeyBoard();
                break;
            case "k_space":
                if (this.tempwords == "") {
                    this.ObjectId.value += " ";
                } else {
                    this.SendStr(0);
                }
                break;
            default:
                this.getKey(obj.id);
                break;
        }
    },
    getKey: function (key) {
        switch (this.imemodule) {
            case "en":
                this.ime_en(key);
                break;
            default:
                this.ime_cn(key);
                break;
        }
        this.ObjectId.scrollLeft = this.ObjectId.scrollWidth;
    },
    ime_en: function (key) {
        var notShift = (this.shiftmodule == "down");
        switch (key) {
            case "k_1":
                this.ObjectId.value += (notShift) ? "1" : "!";
                break;
            case "k_2":
                this.ObjectId.value += (notShift) ? "2" : "@";
                break;
            case "k_3":
                this.ObjectId.value += (notShift) ? "3" : "#";
                break;
            case "k_4":
                this.ObjectId.value += (notShift) ? "4" : "$";
                break;
            case "k_5":
                this.ObjectId.value += (notShift) ? "5" : "%";
                break;
            case "k_6":
                this.ObjectId.value += (notShift) ? "6" : "^";
                break;
            case "k_7":
                this.ObjectId.value += (notShift) ? "7" : "&";
                break;
            case "k_8":
                this.ObjectId.value += (notShift) ? "8" : "*";
                break;
            case "k_9":
                this.ObjectId.value += (notShift) ? "9" : "(";
                break;
            case "k_0":
                this.ObjectId.value += (notShift) ? "0" : ")";
                break;
            case "k_dashes":
                this.ObjectId.value += (notShift) ? "-" : "_";
                break;
            case "k_plus":
                this.ObjectId.value += (notShift) ? "=" : "+";
                break;
            case "k_comma":
                this.ObjectId.value += (notShift) ? "," : ":";
                break;
            case "k_point":
                this.ObjectId.value += (notShift) ? "." : ";";
                break;
            case "k_question":
                this.ObjectId.value += (notShift) ? "/" : "?";
                break;
            case "k_q":
                this.ObjectId.value += (notShift && !this.capslocked) ? "q" : "Q";
                break;
            case "k_w":
                this.ObjectId.value += (notShift && !this.capslocked) ? "w" : "W";
                break;
            case "k_e":
                this.ObjectId.value += (notShift && !this.capslocked) ? "e" : "E";
                break;
            case "k_r":
                this.ObjectId.value += (notShift && !this.capslocked) ? "r" : "R";
                break;
            case "k_t":
                this.ObjectId.value += (notShift && !this.capslocked) ? "t" : "T";
                break;
            case "k_y":
                this.ObjectId.value += (notShift && !this.capslocked) ? "y" : "Y";
                break;
            case "k_u":
                this.ObjectId.value += (notShift && !this.capslocked) ? "u" : "U";
                break;
            case "k_i":
                this.ObjectId.value += (notShift && !this.capslocked) ? "i" : "I";
                break;
            case "k_o":
                this.ObjectId.value += (notShift && !this.capslocked) ? "o" : "O";
                break;
            case "k_p":
                this.ObjectId.value += (notShift && !this.capslocked) ? "p" : "P";
                break;
            case "k_a":
                this.ObjectId.value += (notShift && !this.capslocked) ? "a" : "A";
                break;
            case "k_s":
                this.ObjectId.value += (notShift && !this.capslocked) ? "s" : "S";
                break;
            case "k_d":
                this.ObjectId.value += (notShift && !this.capslocked) ? "d" : "D";
                break;
            case "k_f":
                this.ObjectId.value += (notShift && !this.capslocked) ? "f" : "F";
                break;
            case "k_g":
                this.ObjectId.value += (notShift && !this.capslocked) ? "g" : "G";
                break;
            case "k_h":
                this.ObjectId.value += (notShift && !this.capslocked) ? "h" : "H";
                break;
            case "k_j":
                this.ObjectId.value += (notShift && !this.capslocked) ? "j" : "J";
                break;
            case "k_k":
                this.ObjectId.value += (notShift && !this.capslocked) ? "k" : "K";
                break;
            case "k_l":
                this.ObjectId.value += (notShift && !this.capslocked) ? "l" : "L";
                break;
            case "k_z":
                this.ObjectId.value += (notShift && !this.capslocked) ? "z" : "Z";
                break;
            case "k_x":
                this.ObjectId.value += (notShift && !this.capslocked) ? "x" : "X";
                break;
            case "k_c":
                this.ObjectId.value += (notShift && !this.capslocked) ? "c" : "C";
                break;
            case "k_v":
                this.ObjectId.value += (notShift && !this.capslocked) ? "v" : "V";
                break;
            case "k_b":
                this.ObjectId.value += (notShift && !this.capslocked) ? "b" : "B";
                break;
            case "k_n":
                this.ObjectId.value += (notShift && !this.capslocked) ? "n" : "N";
                break;
            case "k_m":
                this.ObjectId.value += (notShift && !this.capslocked) ? "m" : "M";
                break;
            default:
                break;
        }
    },
    ime_cn: function (key) {
        var imepy = (this.imemodule == "py");
        switch (key) {
            case "k_1":
                this.SendStr(0);
                break;
            case "k_2":
                this.SendStr(1);
                break;
            case "k_3":
                this.SendStr(2);
                break;
            case "k_4":
                this.SendStr(3);
                break;
            case "k_5":
                this.SendStr(4);
                break;
            case "k_6":
                this.SendStr(5);
                break;
            case "k_7":
                this.SendStr(6);
                break;
            case "k_8":
                this.SendStr(7);
                break;
            case "k_9":
                this.SendStr(8);
                break;
            case "k_0":
                this.SendStr(9);
                break;
            case "k_dashes":
                this.pu();
                break;
            case "k_plus":
                this.pd();
                break;
            case "k_q":
                this.tempwords += "q";
                (imepy) ? this.Grep(this.tempwords) : this.wbx();
                break;
            case "k_w":
                this.tempwords += "w";
                (imepy) ? this.Grep(this.tempwords) : this.wbx();
                break;
            case "k_e":
                this.tempwords += "e";
                (imepy) ? this.Grep(this.tempwords) : this.wbx();
                break;
            case "k_r":
                this.tempwords += "r";
                (imepy) ? this.Grep(this.tempwords) : this.wbx();
                break;
            case "k_t":
                this.tempwords += "t";
                (imepy) ? this.Grep(this.tempwords) : this.wbx();
                break;
            case "k_y":
                this.tempwords += "y";
                (imepy) ? this.Grep(this.tempwords) : this.wbx();
                break;
            case "k_u":
                this.tempwords += "u";
                (imepy) ? this.Grep(this.tempwords) : this.wbx();
                break;
            case "k_i":
                this.tempwords += "i";
                (imepy) ? this.Grep(this.tempwords) : this.wbx();
                break;
            case "k_o":
                this.tempwords += "o";
                (imepy) ? this.Grep(this.tempwords) : this.wbx();
                break;
            case "k_p":
                this.tempwords += "p";
                (imepy) ? this.Grep(this.tempwords) : this.wbx();
                break;
            case "k_a":
                this.tempwords += "a";
                (imepy) ? this.Grep(this.tempwords) : this.wbx();
                break;
            case "k_s":
                this.tempwords += "s";
                (imepy) ? this.Grep(this.tempwords) : this.wbx();
                break;
            case "k_d":
                this.tempwords += "d";
                (imepy) ? this.Grep(this.tempwords) : this.wbx();
                break;
            case "k_f":
                this.tempwords += "f";
                (imepy) ? this.Grep(this.tempwords) : this.wbx();
                break;
            case "k_g":
                this.tempwords += "g";
                (imepy) ? this.Grep(this.tempwords) : this.wbx();
                break;
            case "k_h":
                this.tempwords += "h";
                (imepy) ? this.Grep(this.tempwords) : this.wbx();
                break;
            case "k_j":
                this.tempwords += "j";
                (imepy) ? this.Grep(this.tempwords) : this.wbx();
                break;
            case "k_k":
                this.tempwords += "k";
                (imepy) ? this.Grep(this.tempwords) : this.wbx();
                break;
            case "k_l":
                this.tempwords += "l";
                (imepy) ? this.Grep(this.tempwords) : this.wbx();
                break;
            case "k_z":
                this.tempwords += "z";
                (imepy) ? this.Grep(this.tempwords) : this.wbx();
                break;
            case "k_x":
                this.tempwords += "x";
                (imepy) ? this.Grep(this.tempwords) : this.wbx();
                break;
            case "k_c":
                this.tempwords += "c";
                (imepy) ? this.Grep(this.tempwords) : this.wbx();
                break;
            case "k_v":
                this.tempwords += "v";
                (imepy) ? this.Grep(this.tempwords) : this.wbx();
                break;
            case "k_b":
                this.tempwords += "b";
                (imepy) ? this.Grep(this.tempwords) : this.wbx();
                break;
            case "k_n":
                this.tempwords += "n";
                (imepy) ? this.Grep(this.tempwords) : this.wbx();
                break;
            case "k_m":
                this.tempwords += "m";
                (imepy) ? this.Grep(this.tempwords) : this.wbx();
                break;
            default:
                this.ime_en(key);
                break;
        }
        this.$("tempwordsshow").innerHTML = this.tempwords;
    },
    FindIn: function (s) {
        var find = -1, low = 0, mid = 0, high = this.CodeList.length;
        var sEng = "";
        while (low < high) {
            mid = (low + high) / 2;
            mid = Math.floor(mid);
            sEng = this.CodeList[mid];
            if (sEng.indexOf(s, 0) == 0) { find = mid; break; }
            sEng = this.CodeList[mid - 1];
            if (sEng.indexOf(s, 0) == 0) { find = mid; break; }
            if (sEng < s) { low = mid + 1; } else { high = mid - 1; }
        }
        while (find > 0) {
            sEng = this.CodeList[find - 1];
            if (sEng.indexOf(s, 0) == 0) { find = find - 1; } else { break; }
        }
        return (find);
    },
    GetStr: function (No, s) {
        this.$("ChiList").value = "";
        var sTmp = "", sChi = "";
        for (i = 0; i <= 9; i++) {
            if (No + i >= this.CodeList.length - 1) { break; }
            sTmp = this.CodeList[No + i];
            if (sTmp.indexOf(s) >= 0) {
                sChi = this.CodeList[No + i];
                this.OutEng[i] = sChi.substring(s.length, sChi.indexOf(this.spaceChar));
                this.OutChi[i] = sChi.substr(sChi.lastIndexOf(this.spaceChar) + 1);
                if (i <= 8) { this.$("ChiList").value += (i + 1) + "." + this.OutChi[i] + this.OutEng[i] + '\n'; }
                else { this.$("ChiList").value += (0) + "." + this.OutChi[i] + this.OutEng[i] + '\n'; }
                this.LastNo = No + i;
            } else { this.LastNo = -1; }
        }
    },
    Grep: function (s) {
        var No = -1;
        for (i = 0; i <= 9; i++) { this.OutChi[i] = ""; }
        if (s != "") {
            No = this.FindIn(s);
            if (No >= 0) { this.GetStr(No, s); }
        } else {
            this.$("ChiList").value = "";
        }
    },
    wbx: function () {
        if (this.tempwords != "") {
            this.txt1 = this.tempwords.substring(0, 1).charCodeAt() - 97;

            i = this.imeindex[this.txt1];
            this.txt2 = i;
            j = this.imeindex[this.txt1 + 1];
            this.l = 0;
            this.$("ChiList").value = "";
            while (i < j) {
                this.testlength = this.tempwords.length
                this.teststr = this.imestr2[i].substring(0, this.testlength);
                if (this.tempwords == this.teststr) {
                    this.teststr2[this.l] = this.imestr1[i];
                    this.$("ChiList").value += this.imeindex2[this.l] + this.imestr1[i] + "\n";
                    this.l++;
                    this.txt2++;
                    if (this.l > 9) { break; }
                }
                i++;
            }
        } else {
            this.$("ChiList").value = "";
        }
    },
    wb_pu: function () {
        this.txt2 = (this.txt2 > 0) ? (this.txt2 - 10) : this.txt2;
        i = this.txt2;
        j = this.imeindex[this.txt1];
        if (this.tempwords != "") {
            this.l = 9;
            this.$("ChiList").value = "";
            while (i > j) {
                this.testlength = this.tempwords.length
                this.teststr = this.imestr2[i].substring(0, this.testlength);
                if (this.tempwords == this.teststr) {
                    this.teststr2[this.l] = this.imestr1[i];
                    this.$("ChiList").value = this.imeindex2[this.l] + this.imestr1[i] + "\n" + this.$("ChiList").value;
                    this.l--;
                    if (this.l < 0) { break; }
                }
                i--;
            }
        }
    },
    wb_pd: function () {
        i = this.txt2;
        j = this.imeindex[this.txt1 + 1];
        if (this.tempwords != "") {
            this.l = 0;
            this.$("ChiList").value = "";
            while (i < j) {
                this.testlength = this.tempwords.length
                this.teststr = this.imestr2[i].substring(0, this.testlength);
                if (this.tempwords == this.teststr) {
                    this.teststr2[this.l] = this.imestr1[i];
                    this.$("ChiList").value += this.imeindex2[this.l] + this.imestr1[i] + "\n";
                    this.l++;
                    this.txt2++;
                    if (this.l > 9) { break; }
                }
                i++;
            }
        }
    },
    SendStr: function (n) {
        if (this.imemodule == "py") {
            if ((n >= 0) && (n <= 9) && this.OutChi.length > 0) {
                this.ObjectId.value += this.OutChi[n];
                this.tempwords = "";
                this.$("ChiList").value = "";
                this.$("tempwordsshow").innerHTML = "";
                this.OutChi.length = 0;
            } else {
                var key = (n == 9) ? 0 : ++n;
                this.ime_en('k_' + key);
            }
        } else {
            if ((n >= 0) && (n <= 9) && this.teststr2.length > 0) {
                this.ObjectId.value += this.teststr2[n];
                this.tempwords = "";
                this.$("ChiList").value = "";
                this.$("tempwordsshow").innerHTML = "";
                this.teststr2.length = 0;
            } else {
                var key = (n == 9) ? 0 : ++n;
                this.ime_en('k_' + key);
            }
        }
    },
    pu: function () {
        if (this.imemodule == "py") {
            var s = this.tempwords;
            if (s != "") {
                this.$("ChiList").value = "";
                if ((this.LastNo - 19) > this.FindIn(s)) {
                    this.LastNo = this.LastNo - 19;
                    this.GetStr(this.LastNo, s);
                } else {
                    this.GetStr(this.FindIn(s), s);
                }
            }
        } else {
            this.wb_pu();
        }
    },
    pd: function () {
        if (this.imemodule == "py") {
            s = this.tempwords;
            if ((s != "") && (this.LastNo != -1)) {
                this.$("ChiList").value = "";
                this.GetStr(this.LastNo + 1, s);
            }
        } else {
            this.wb_pd();
        }
    }
}

var keyBoard = new dhKeyBoard('keyBoard');