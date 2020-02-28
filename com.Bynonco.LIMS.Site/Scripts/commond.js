
String.prototype.isDefaultGuid = function () {
    if (!this) return false;
    return this == "00000000-0000-0000-0000-000000000000";
}

String.prototype.getFileName = function () {
    if (this.indexOf("\\") == -1) return this;
    return this.substring(this.lastIndexOf("\\") + 1);
}
String.prototype.toJson = function () {
    return this.replace(/\&quot;/g, "'").replace(/\&#39;/g,"\"").replace(/\&gt;/g, ">").replace(/\&lt;/g, "<");
}
String.prototype.trim = function () {
    return this.replace(/(^\s*)|(\s*$)/g, "");
}
String.prototype.lTrim = function() {
    return this.replace(/(^\s*)/g, ""); 
}
String.prototype.rTrim = function () {
    return this.replace(/(\s*$)/g, "");
}
String.prototype.isDate = function () {
    var regEx = /^((((1[6-9]|[2-9]\d)\d{2})-(0?[13578]|1[02])-(0?[1-9]|[12]\d|3[01]))|(((1[6-9]|[2-9]\d)\d{2})-(0?[13456789]|1[012])-(0?[1-9]|[12]\d|30))|(((1[6-9]|[2-9]\d)\d{2})-0?2-(0?[1-9]|1\d|2[0-8]))|(((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))-0?2-29-))$/;
    return regEx.test(this);

}
String.prototype.isInt = function () {
    var regEx = /^-?[1-9]\d*$/;
    return regEx.test(this);

}
String.prototype.isPositiveInt = function () {
    var regEx = /^[1-9]*[1-9][0-9]*$/;
    return regEx.test(this);

}
String.prototype.isPositiveFloat = function () {
    var regEx = /^[0-9]+.?[0-9]*$/;
    return regEx.test(this);

}
String.prototype.isMinusInt = function () {
    var regEx = /^-[0-9]*[1-9][0-9]*$/;
    return regEx.test(this);

}

String.prototype.isNumber = function () {
    var regEx = /^-?[1-9]\d*$/;
    return regEx.test(this);

}
String.prototype.isIdentityCardNo = function () {
    var regEx = /^([1-9]\d{7}((0\d)|(1[0-2]))(([0|1|2]\d)|3[0-1])\d{3})|([1-9]\d{5}[1-9]\d{3}((0\d)|(1[0-2]))(([0|1|2]\d)|3[0-1])\d{4})$/;
    return regEx.test(this);
}

String.prototype.isEmail = function () {
    var regEx = /^[\\w-]+(\\.[\\w-]+)*@[\\w-]+(\\.[\\w-]+)+$/;
    return regEx.test(this);
}

String.prototype.isMobile = function () {
    //var regEx = /^1[3|4|5|8][0-9]\d{4,8}$/;
    var regEx = /^1[3|4|5|7|8][0-9]\d{4,8}$/;
    return regEx.test(this);
}
String.prototype.isFixTelephoneNo = function () {
    var regEx = /^(\d{3,4}-)?\d{6,8}(-\d{2,3})?$/;
    return regEx.test(this);
}
String.prototype.isPostCode = function () {
    var regEx = /^[a-zA-Z0-9 ]{3,12}$/;
    return regEx.test(this);
}
String.prototype.isSeCurrentDate = function () {
    var month = this.substring(5, this.lastIndexOf("-"));
    var day = this.substring(this.length, this.lastIndexOf("-") + 1);
    var year = this.substring(0, this.indexOf("-"));
    return Date.parse(month + "/" + day + "/" + year) >= Date.parse(new Date().toDateString()) ? false : true;
}
String.prototype.isltCurrentDate = function () {
    return !this.isSeCurrentDate();
}

Date.prototype.toCnDate = function () {
    return this.getFullYear() + "-" + (this.getMonth() + 1) + "-" + this.getDate();
}

Date.prototype.toCnDateTime = function () {

    return this.getFullYear() + "-" + (this.getMonth() + 1) + "-" + this.getDate() + " " + this.getHours() + ":" + this.getMinutes() + ":" + this.getSeconds();
}
String.prototype.endWith = function (s) {
    if (s == null || s == "" || this.length == 0 || s.length > this.length)
        return false;
    if (this.substring(this.length - s.length) == s)
        return true;
    else
        return false;
    return true;
}

String.prototype.startWith = function (s) {
    if (s == null || s == "" || this.length == 0 || s.length > this.length)
        return false;
    if (this.substr(0, s.length) == s)
        return true;
    else
        return false;
    return true;
}