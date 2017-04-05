/*
 * Translated default messages for the jQuery validation plugin.
 * Locale: ZH (Chinese, 中文 (Zhōngwén), 汉语, 漢語)
 */
(function ($) {
	$.extend($.validator.messages, {
	    required: "Required field.",//必选字段
	    remote: "Please fix this field.",//请修正该字段
	    email: "Please enter a valid Email adddress.",//请输入正确格式的电子邮件
	    url: "Please enter a valid URL.",//请输入合法的网址
	    date: "Please enter a valid date.",//请输入合法的日期
	    dateISO: "Please enter a valid date(ISO).",//请输入合法的日期 (ISO).
	    number: "Please enter a valid number.",//请输入合法的数字
	    digits: "Enter only integers.",//只能输入整数
	    creditcard: "Please enter a valid credit card number.",//请输入合法的信用卡号
	    equalTo: "Please re-enter the same value.",//请再次输入相同的值
	    accept: "Please enter a string with a valid suffix.",//请输入拥有合法后缀名的字符串
	    maxlength: $.validator.format("The maximum length of the string is {0}."),//请输入一个长度最多是 {0} 的字符串
	    minlength: $.validator.format("The minimum length of the string is {0}."),//请输入一个长度最少是 {0} 的字符串
	    rangelength: $.validator.format("The length of the string is between {0} and {1}."),//请输入一个长度介于 {0} 和 {1} 之间的字符串
	    range: $.validator.format("The length of the value is between {0} and {1}."),//请输入一个介于 {0} 和 {1} 之间的值
	    max: $.validator.format("The maximum length of the value is {0}."),//请输入一个最大为 {0} 的值
	    min: $.validator.format("The minimum length of the value is {0}.")//请输入一个最小为 {0} 的值
	});
}(jQuery));