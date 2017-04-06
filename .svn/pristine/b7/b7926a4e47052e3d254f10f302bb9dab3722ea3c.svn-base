
/*====================jquery扩展====================*/

(function ($) {
    $.fn.GetPostData = function () {
        var data = {};
        $(this).find(".datacontrol").each(function (i, value) {
            var field = $(value).attr("name");
            if (field == null) {
                field = $(value).attr("id");
            }
            if (value.tagName == "INPUT") {
                if (value.type == "checkbox") {
                    if ($(value).prop("checked") == true) {
                        if (data[field]) {
                            var a = $(value).val();
                            if (a == "") {
                                a = "1";
                            }
                            data[field] = data[field] + "," + a;
                        } else {
                            var a = $(value).val();
                            data[field] = a;
                        }
                    }
                }
                else if (value.type == "radio") {
                    if ($(value).is(':checked')) {
                        data[field] = $(value).val();
                    }
                }
                else {
                    if ($(value).val() != "") {
                        data[field] = $(value).val();
                    }
                }
            }

            else if (value.tagName == "SELECT") {
                if ($(value).val() != "") {
                    alert(data[field] + "||" + $(value).val());
                    //data[field] = $(value).val();
                    data[field] = $(value).combo("getValue");
                }
            }
            else if (value.tagName == "DIV") {
                data[field] = $(value).html();
            }
            else if (value.tagName == "IMG") {
                data[field] = $(value).attr("src");
            }
            else if (value.tagName == "SPAN") {
                data[field] = $(value).html();
            }
            else if (value.tagName == "TEXTAREA") {
                if ($(value).val() != "") {
                    data[field] = $(value).val();
                }
            }

        });
        return data;
    };
    $.fn.extend({
        json: function () {
            var array = this.serializeArray();
            var b = "";
            $.each(array, function (index, value) {
                if (b == "")
                    b = value.name + ":'" + value.value + "'";
                else
                    b += "," + value.name + ":'" + value.value + "'";
            });

            return eval("({" + b + "})");
        },
        dialogDefaults: { modal: true, closed: true, cache: false },
        openTopDialog: function (settings) {
            settings = settings || {};
            var defaults = $.dialogDefaults;
            for (var i in defaults) {
                if (settings[i] === undefined) settings[i] = defaults[i];
            }
            pharos.easyui.dialog.topOpen(settings.id,settings);
        }
    });
})(jQuery);


/*====================javascript扩展====================*/

/*Date========================================*/
// 对Date的扩展，将 Date 转化为指定格式的String   
// 月(M)、日(d)、小时(h)、分(m)、秒(s)、季度(q) 可以用 1-2 个占位符，   
// 年(y)可以用 1-4 个占位符，毫秒(S)只能用 1 个占位符(是 1-3 位的数字)   
// 例子：   
// (new Date()).Format("yyyy-MM-dd hh:mm:ss.S") ==> 2006-07-02 08:09:04.423   
// (new Date()).Format("yyyy-M-d h:m:s.S")      ==> 2006-7-2 8:9:4.18   
Date.prototype.format = function (fmt) { //author: meizz   
    var o = {
        "M+": this.getMonth() + 1,                 //月份   
        "d+": this.getDate(),                    //日   
        "h+": this.getHours(),                   //小时   
        "m+": this.getMinutes(),                 //分   
        "s+": this.getSeconds(),                 //秒   
        "q+": Math.floor((this.getMonth() + 3) / 3), //季度   
        "S": this.getMilliseconds()             //毫秒   
    };
    if (/(y+)/.test(fmt))
        fmt = fmt.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    for (var k in o)
        if (new RegExp("(" + k + ")").test(fmt))
            fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
    return fmt;
};

/*String======================================*/
/*判断txt值是否为null或空字符串
    txt => string => 源字符串
    return => bool
*/
String.isNullOrEmpty = function (txt) {
    if (txt === undefined || txt === null || txt === "") {
        return true;
    }
    return false;
}
String.isNE = String.isNullOrEmpty;
/*判断txt值是否为真
    txt => string => 字符串对象
    return => bool
*/
String.isTrue = function (txt) {
    return !!txt;
}
/*格式化字符串
    txt => string => 原始字符串
    obj => array => 替换值集合
    return => string
*/
String.format = function (txt, objs) {
    if (typeof (Ext) == "undefined" || $.isArray(objs)) {
        var t = null;
        txt = txt.replace(/\{[0-9]+\}/gi, function ($1) {
            t = objs[$1.replace(/[\{\}]+/gi, "")];
            if (t != null)
                return t;
            return $1;
        });
        return txt;
    }
    else {
        var a = Ext.toArray(arguments, 1);
        return txt.replace(/\{(\d+)\}/g, function (c, d) { return a[d]; });
    }
}
Array.prototype.indexOf = function (e) {
    for (var i = 0, j; j = this[i]; i++) {
        if (j == e) { return i; }
    }
    return -1;
}

