
function RegisterArea(url, element, veiwmodel) {
    if (!element) element = "body";
    if (!veiwmodel) veiwmodel = {};
    $.ajax({
        url: url,
        dataType: 'text',
        success: function (data) {
            var html = "<script type='text/x-jquery-tmpl'></script>"
            var tmp = $(html).append($(data).not('script'));
            tmp.tmpl(veiwmodel).appendTo(element);
            $(data).filter('script').appendTo(element);
        }
    });
}