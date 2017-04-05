/**
 * Created by Administrator on 15-10-29.
 */

/**
 * Keyboard 插件
 * 软件盘插件
 * */
;(function($){
    var methods = {
        init:function(){
            var me = this;
            if(!$.fn.keyboard.$el){
                <!-- 软键盘 -->
                var str = ['<ul class="p-keyboard animated">',
                    '<li class="text"><input readonly /></li>',
                    '<li style="width:33.33%" data-key="del"><i class="iconfont icon-backspace color-red"></i></li>',
                    '<li>1</li><li>2</li><li>3</li>',
                    '<li>4</li><li>5</li><li>6</li>',
                    '<li>7</li><li>8</li><li>9</li>',
                    '<li>0</li><li>.</li>',
                    '<li data-key="ok"><i class="iconfont icon-right color-green1"></i></li>',
                    '</ul>'];
                $.fn.keyboard.$el = $(str.join('')).appendTo(document.body);

                var $keyBtn = $.fn.keyboard.$el.find("li");
                $keyBtn.on("vmousedown",function(evt){
                    evt.stopPropagation();
                    evt.preventDefault();
                    var $el = $(this),
                        key = $el.data("key"),
                        $p = $el.parent(),
                        $text = $("#"+$p.data("text")),
                        hasFormat = $text.data("format"),
                        val=$el.text(),
                        txtVal = $.trim($text.val());
                    if(val=="."){
                        if(txtVal.length==0){
                            return;
                        }
                        if(txtVal.indexOf('.')>-1){
                            return;
                        }
                    }
                    //$text.toggleClass("p-input-focus");
                    if(key=="del"){
                        var v = $text.val(),l = v.length;
                        if(l>0){
                            $text.val(v.substr(0, v.length-1));
                        }
                    }else if(key=="ok"){
                        $text.removeClass("p-input-focus");
                        $p.removeClass("fadeIn").hide();
                        //通过输入的二维码取数
                        var cb = $text.data("callback");
                        try{
                            eval("var callBcakFn = "+cb);
                            typeof callBcakFn === "function" && callBcakFn({
                                Barcode:$text.val()
                            });
                        }catch (e){

                        }
                    }else{
                        var result = $.trim($text.val()+$el.text());
                        if(hasFormat=="code"){
                            result=result.replace(/\s/g,"");
                            result = result.replace(/(\d|\.)(?=(?:(\d|\.){4})+$)/g, '$1 ');
                        }
                        $text.val(result);
                    }
                    $($p.find(".text>input")).val($text.val());
                });

                /**
                 * 处理长按删除
                 */
                $keyBtn.on("vmousedown",function(evt){
                    var $el = $(this),
                        key = $el.data("key"),
                        $p = $el.parent(),
                        $key_text =$($p.find(".text>input")),
                        $text = $("#"+$p.data("text"));
                    var keyTimer;
                    if(keyTimer){
                        window.clearInterval(keyTimer);
                    }
                    keyTimer = window.setInterval(function(){
                        if(key=="del"){
                            var v = $text.val(),l = v.length;
                            if(l>0){
                                $text.val(v.substr(0, v.length-1));
                                $key_text.val($text.val());
                            }else{
                                window.clearInterval(keyTimer);
                            }
                        }
                    },100);
                    $el.one('vmouseup',function(){
                        window.clearInterval(keyTimer);
                    });
                });
                $(document).on("vmousedown",function(){
                    var id = $.fn.keyboard.$el.data("text"),
                        $text = $("#"+id);
                    $text.removeClass("p-input-focus");
                    $.fn.keyboard.$el.hide();
                });
            };
            $.each(this,function(i,el){
                var $el = $(this),$text = $("#"+$el.data("text"));
                if($.trim($el.attr("placeholder"))==""){
                    $el.attr("placeholder","点击输入");
                }
                $text.attr("readonly",true);
                $el.off("click.keyboard").on('click.keyboard',function(evt){
                    evt.stopPropagation();
                    var $el = $(this),$text = $("#"+$el.data("text"));
                    $(".p-input-focus").removeClass("p-input-focus");
                    $text.addClass("p-input-focus");
                    $.fn.keyboard.$el.data("text",$el.data("text"));
                    $.fn.keyboard.$el.hide();
                    $.fn.keyboard.$el.show();
                    var p = $text.offset(), w =  $text.width();
                    $.fn.keyboard.$el.find(".text>input").val($text.val()).attr("type",$text.attr("type"));
                });
            });
        }
    };
    $.fn.keyboard = function() {
        var method = arguments[0];
        if(methods[method]) {
            method = methods[method];
            arguments = Array.prototype.slice.call(arguments, 1);
        } else if( typeof(method) == 'object' || !method ) {
            method = methods.init;
        } else {
            $.error( 'Method ' +  method + ' does not exist on jQuery.pluginName' );
            return this;
        }
        return method.apply(this, arguments);
    };
    $(function(){
        $(".cmp-keyboard").keyboard();
    });
})(jQuery);
/**
 * 初始化扫描器
 * 会员
 * 二维码商品
 */
(function($){
    var methods = {
        init:function(){
            $.each(this,function(i,el){
                var $el = $(el);
                if($.trim($el.attr("placeholder"))==""){
                    $el.attr("placeholder","点击扫码");
                }
                if($el.attr("id")==""){
                    $el.attr("id","cmpScanner"+$.guid+i);
                }
                $(el).off('click.scanner').on("click.scanner",function(evt){
                    AppJS.ScanningCode(this.id);
                });
            });
        }
    };
    $.fn.scanner = function() {
        var method = arguments[0];
        if(methods[method]) {
            method = methods[method];
            arguments = Array.prototype.slice.call(arguments, 1);
        } else if( typeof(method) == 'object' || !method ) {
            method = methods.init;
        } else {
            $.error( 'Method ' +  method + ' does not exist on jQuery.pluginName' );
            return this;
        }
        return method.apply(this, arguments);
    };
    $(function(){
        $(".cmp-scanner").scanner();
    });
})(jQuery);
/**
 *
 * 初始网格
 *
 */
(function($){
    var bindEvt = function($el){
        $el.on({
            'swipeleft':function(evt){
                var ops = $(this).data("option");
                var _events = ops["listener"];
                _events["swipeLeft"]&&_events["swipeLeft"].apply(this,arguments);
            },
            'swiperight':function(evt){
                var ops = $(this).data("option");
                var _events = ops["listener"];
                _events["swipeRight"]&&_events["swipeRight"].apply(this,arguments);
            },
            'tap':function(evt){
                var ops = $(this).data("option");
                var _events = ops["listener"];

                var $tag = $(evt.target),$tr = $tag.closest('tr');
                $(this).find('tr').removeClass("selected");
                $tr.addClass("selected");
                if($tr.length){
                    _events["rowClick"]&&_events["rowClick"].apply(this,arguments);
                }
                _events["tap"]&&_events["tap"].apply(this,arguments);
            },
            'taphold':function(){
                var ops = $(this).data("option");
                var _events = ops["listener"];
                _events["taphold"]&&_events["taphold"].apply(this,arguments);
            }
        });
    };
    var methods = {
        init:function(config){
            var option = {
                url:null,
                params:null,
                store:null,
                trTpl:"",
                fields:null,//{age:'',name:''}
                listener:{
                    loadBefore:null,
                    loadAfter:null,
                    addBefore:null,
                    addAfter:null,
                    delBefore:null,
                    delAfter:null,
                    swipeLeft:null,
                    swipeRight:null,
                    rowClick:null
                }
            }
            $.extend(option,config);
            /**
             * 数据库
             * @type {*|void|m.extend|e.Model.extend|e.Collection.extend|e.Router.extend}
             */
            var model =  Backbone.Model.extend({
                defaults: $.extend({},option.fields),
                initialize: function () {},
                validate: function (attrs, options) {
                    for(var i in attrs){
                        if(this.defaults[i]===undefined){
                            return "不存在字段:"+i;
                        }
                    }
                }
            });
            var Store =  Backbone.Collection.extend({
                model: model
            });
            var store = new Store();
            store.on("invalid",function(model,msg,options){
                alert(msg);
            });
            store.on("remove",function(){
                debugger;
            });
            store.on("add",function(model,store,options){
                debugger;
            });
            store.on("change",function(){
                debugger;
            });
            store.on("reset",function(){
                debugger;
            });
            option.store = store;
            $.each(this,function(i,el){
                var $el = $(el),$bd = $el.find('.p-grid-bd');
                $el.data('option',option);
                $bd.iScroll({hideScrollbar:false});
                bindEvt($el);
            });
        },
        render:function(){
            var $el = $(this),option = $el.data("option"),store = option.store,str=[],render;
            for(var i=0;i<store.models.length;i++){
                render = template.compile(option.trTpl)
                str.push(render($.extend({rowIndex:i+1},store.models[i].attributes)));
            }
            $el.find(".p-grid-bd>table").html(str.join(''));
            $el.grid("scrollRefresh");
        },
        renderRow:function(){

        },
        scrollRefresh:function(){
            var iscroll = $(this).find('.p-grid-bd').data("iScroll");
            iscroll&&iscroll.refresh();
        },
        addRecord:function(data){
            var $el = $(this),option = $el.data("option");
            option.store.add(data,{validate: true});
        },
        existsRecord:function(where){
            var $el = $(this),option = $el.data("option");
            var d = option.store.where(where);
            return d.length>0?!0:!1;
        },
        /**
         * 载入数据
         */
        setData:function(data){
            var $el = $(this),option = $el.data("option");
            option.store.remove(option.store.models);
            option.store.add(data,{validate: true});
            $el.grid("render");
        },
        delRecord:function(guid){

        },
        updateRecord:function(data){

        },
        selected:function(){

        }
    };
    $.fn.grid = function(){
        var method = arguments[0];
        if(methods[method]) {
            method = methods[method];
            arguments = Array.prototype.slice.call(arguments, 1);
        } else if( typeof(method) == 'object' || !method ) {
            method = methods.init;
        } else {
            $.error( 'Method ' +  method + ' does not exist on jQuery.pluginName' );
            return this;
        }
        return method.apply(this, arguments);
    };
    $(function(){
        //$(".p-grid").grid();
    });
})(jQuery);

/**
 * iScroll统一初始化
 */
(function($){
    var methods = {
        init:function(option){
            $.each(this,function(i,el){
                var $el = $(el);
                !$el.hasClass("cmp-iScroll")&&$el.addClass("cmp-iScroll");
                $el.data("iScroll",new iScroll($el[0],option));//{hideScrollbar:false}
            });
        },
        refresh:function(){
            $.each(this,function(i,el){
                var iScroll= $(el).data("iScroll");
                iScroll&&iScroll.refresh();
            });
        }
    };
    $.fn.iScroll = function() {
        var method = arguments[0];
        if(methods[method]) {
            method = methods[method];
            arguments = Array.prototype.slice.call(arguments, 1);
        } else if( typeof(method) == 'object' || !method ) {
            method = methods.init;
        } else {
            $.error( 'Method ' +  method + ' does not exist on jQuery.pluginName' );
            return this;
        }
        return method.apply(this, arguments);
    };
    $(function(){
        $(".cmp-iScroll").iScroll({hideScrollbar:false});
    });
})(jQuery);