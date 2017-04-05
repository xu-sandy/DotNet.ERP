jQuery.validator.addMethod("rule_username", function (value, element) {
  return this.optional(element) || /^\w(\w|\d|_){1,20}$/i.test(value);
}, "It requires 2-10 characters.It can only contain letters, numbers, underscores,and must start with a letter .");//2至20位字母、数字或下划线，首字母必须是字母

jQuery.validator.addMethod("rule_password", function (value, element) {
  return this.optional(element) || /^[^\s]{6,20}$/i.test(value);
}, "It requires 6-20 characters,but it can not contain whitespace,like Spaces,Tab,End-of-line character,Chinese full-width character and so on. ");//6至20位任意字符，不能包含任意的空白符，包括空格，制表符(Tab)，换行符，中文全角空格等

jQuery.validator.addMethod("rule_cn_phone", function (value, element) {
  return this.optional(element) || /^(\d{4}-|\d{3}-)?(\d{8}|\d{7})$/i.test(value);
}, "Invalid telephone number.You can fill in like this:0123-12345678.");//无效的固定电话号码，示例：0123-12345678

jQuery.validator.addMethod("rule_cn_mobile", function (value, element) {
  return this.optional(element) || /^1[3-8]\d{9}$/i.test(value);
}, "Invalid mobile number.");//无效的手机号码

jQuery.validator.addMethod("rule_qq", function (value, element) {
  return this.optional(element) || /^[1-9]\d{4,}$/i.test(value);
}, "Invalid QQ number.");//无效的QQ号码

jQuery.validator.addMethod("rule_cn_idno", function (value, element) {
  return this.optional(element) || /^\d{15}(\d\d[0-9xX])?$/i.test(value);
}, "Invalid ID number.");//无效的身份证号码
// 邮政编码验证 
jQuery.validator.addMethod("rule_cn_iszipcode", function (value, element) {
  var tel = /^[0-9]{6}$/;
  return this.optional(element) || (tel.test(value));
}, "Please fill in your postal code correctly.");//请正确填写您的邮政编码
// 金额验证
jQuery.validator.addMethod("rule_cn_decimal", function (value, element) {
    return this.optional(element) || /^(?!0+(?:\.0+)?$)(?:[1-9]\d*|0)(?:\.\d{1,2})?$/.test(value);
}, "The money must be greater than 0,and it can only be accurate to 2 decimal places.");//金额必须大于0并且只能精确到分
