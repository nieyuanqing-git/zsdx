$.extend($.fn.validatebox.defaults.rules, {
    minLength: {
        validator: function (value, param) {
            if (param.length != 3) {
                param[2] = "参数格式错误";
                return false;
            }
            if (param[0]) {
                var regEx = /^\s*$/;
                if (regEx.test(value)) {
                    param[2] = "该输入项为必填项";
                    return false;
                }
            }
            return value.length >= param[1];
        },
        message: '{2}'
    }
});
$.extend($.fn.validatebox.defaults.rules, {
    notEmpty: {
        validator: function (value, param) {
            if (param.length != 1) {
                param[1] = "参数格式错误";
                return false;
            }
            var regEx = /^\s*$/;
            return !regEx.test(value);
        },
        message: '{0}'
    }
});
$.extend($.fn.validatebox.defaults.rules, {
    range: {
        validator: function (value, param) {
            if (param.length != 4) {
                param[3] = "参数格式错误";
                return false;
            }
            if (param[0]) {
                var regEx = /^\s*$/;
                if (regEx.test(value)) {
                    param[3] = "该输入项为必填项";
                    return false;
                }
            }
            if (!Number(value)) {
                param[3] = "请输入数值数据";
                return false;
            }
            var beginNumber = Number(param[1]);
            var endNumber = Number(param[2]);
            if (!beginNumber) {
                param[3] = "第一个参数不是数字";
                return false;
            }
            if (!Number(param[2])) {
                param[3] = "第二个参数不是数字";
                return false;
            }
            return Number(value) >= beginNumber && Number(value) <= endNumber;
        },
        message: '{3}'
    }
});
$.extend($.fn.validatebox.defaults.rules, {
    regExpression: {
        validator: function (value, param) {
            if (param.length != 3) {
                param[2] = "参数格式错误";
                return false;
            }
            if (param[0]) {
                var regEx = /^\s*$/;
                if (regEx.test(value)) {
                    param[2] = "该输入项为必填项";
                    return false;
                }
            }
            var regEx = new RegExp(param[1]);
            return regEx.test(value);
        },
        message: '{2}'
    }
});  