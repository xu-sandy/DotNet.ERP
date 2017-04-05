
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

