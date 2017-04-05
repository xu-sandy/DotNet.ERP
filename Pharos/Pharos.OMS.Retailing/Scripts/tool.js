var tool = tool || {};
(function (para) {
    para.tol = {
        //建立一个可存取到该file的url
        getObjectURL: function (file) {
            var url = null;
            if (window.createObjectURL != undefined) { // basic
                url = window.createObjectURL(file);
            } else if (window.URL != undefined) { // mozilla(firefox)
                url = window.URL.createObjectURL(file);
            } else if (window.webkitURL != undefined) { // webkit or chrome
                url = window.webkitURL.createObjectURL(file);
            }
            return url;
        }
    };
})(tool);

/**
 * 给时间框控件扩展一个清除的按钮
 */
$.fn.datebox.defaults.cleanText = '清空';
(function ($) {
    var buttons = $.extend([], $.fn.datebox.defaults.buttons);
    buttons.splice(1, 0, {
        text: function (target) {
            return $(target).datebox("options").cleanText
        },
        handler: function (target) {
            $(target).datebox("setValue", "");
            $(target).datebox("hidePanel");
        }
    });
    $.extend($.fn.datebox.defaults, {
        buttons: buttons
    });

})(jQuery)

//获取url的参数
function getUrlParam(name) {

    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)"); //构造一个含有目标参数的正则表达式对象

    var r = window.location.search.substr(1).match(reg);  //匹配目标参数

    if (r != null) return unescape(r[2]); return null; //返回参数值

}

//限制textarea输入字符数量
function textareaNum(textarea, textNum, num) {
    var curLength = $(textarea).val().length;
    if (curLength > num) {
        var n = $(textarea).val().substr(0, num);
        $(textarea).val(n);
        $(textNum).text(0)
    }
    else {
        $(textNum).text(num - $(textarea).val().length)
    }
}

// 粘贴事件监控
// 使用
//$("input[type='text']").on("postpaste", function () {
//    code...
//}).pasteEvents();
$.fn.pasteEvents = function (delay) {
    if (delay == undefined) delay = 10;
    return $(this).each(function () {
        var $el = $(this);
        $el.on("paste", function () {
            $el.trigger("prepaste");
            setTimeout(function () { $el.trigger("postpaste"); }, delay);
        });
    });
};