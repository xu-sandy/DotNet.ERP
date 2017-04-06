(function () {
    window.cascade = {};
    cascade.callback = function (data) { }
    $("[cascade]").change(function () {
        var key = $(this).attr("cascade");
        var value = $(this).val();
        $.getJSON("/Cascade?key=" + key + "&targetValue=" + value, function (result) {
            window.cascade.callback(key,result);
        });
    })
})();