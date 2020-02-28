
function confirmBox(title, content,url) {
    $.messager.confirm(title, content, function (r) {
        if (r) {
            location.href = url;
        }
    });
}

function judgeDateText(dateFromTextName, dateToTextName) {
    if ($.trim($("input[name='" + dateFromTextName+"']").val()) != "" && $.trim($("input[name='" + dateToTextName+"']").val()) == "") {
        $.messager.alert('提示', '请输入结束日期!', 'warning');
        return false;
    }
    if ($.trim($("input[name='" + dateFromTextName + "']").val()) == "" && $.trim($("input[name='" + dateToTextName + "']").val()) != "") {
        $.messager.alert('提示', '请输入开始日期!', 'warning');
        return false;
    }
    if ($("input[name='" + dateFromTextName + "']").val() > $("input[name='" + dateToTextName + "']").val()) {
        $.messager.alert('提示', '开始日期不能大于结束日期！', 'warning');
        return false;
    }
}