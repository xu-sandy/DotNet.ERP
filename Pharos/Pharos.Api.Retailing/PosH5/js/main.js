/**
 * Created by Administrator on 15-10-6.
 */
$(function(){
    $(document.body).css({height:$(window).height(),width:$(window).width()});
    //$('#beginTime').date();
    //$('#endTime').date({theme:"datetime"});
   // $(".cmp-date").date();

    pageManage.initLayout();
    pageManage.menuListener();
    pageManage.gridInit();
    pageManage.showSysInfo();
    pageWin.initLayout();

});
var pageManage={
    gridInit:function(){
        /**
         *  清单列表
         */
        $("#grid_orderList").grid({
            trTpl:['<tr data-barcode="<%=Barcode%>"><td class="prdNum"><%=rowIndex%></td>',
                '<td class="prdCode"><%=Barcode%></td>',
                '<td class="prdName"><%=Title%></td>',
                '<td class="prdCount"><%=Number%></td>',
                '<td class="prdUnit"><%=Unit%></td>',
                //'<td class="prdPrice"><%=ActualPrice%></td>',
                '<td class="prdPriceDiscount"><%=Price%></td>',
                '<td class="prdSubtotal"><%=Total%></td>',
                '<td class="prdOper"><i class="iconfont icon-delete"></i></td></tr>'].join(''),
            fields:{
                'Number':null,
                'ActualPrice':null,
                'Barcode':null,
                'Brand':null,
                'Category':null,
                'EnableEditNum':null,
                'EnableEditPrice':null,
                'Price':null,
                'Size':null,
                'Status':null,
                'Title':null,
                'Total':null,
                'Unit':null
            },
            listener:{
                swipeLeft:function(evt){
                    evt.stopPropagation();
                    evt.preventDefault();
                    var  $tag= $(evt.target),$bd = $(this),tr =$tag.closest('tr');
                    if(tr.length){
                        var del = tr.find('.prdOper>.icon-delete').show().removeClass("fadeInRight fadeInLeft");
                        $bd.find(".prdOper>.icon-delete").animate({
                            left:0
                        },100,function(){
                            del.animate({
                                left:-1*del.outerWidth()
                            },200,function(){
                                /**
                                 * 删除按钮出现后进行绑定 点击隐藏按钮
                                 */
                                $(document).one("tap",{scope:$bd},function(evt){
                                    var $bd = $(evt.data.scope);
                                    if($bd.length){
                                        var del = $bd.find('.prdOper>.icon-delete').removeClass("fadeInRight fadeInLeft");
                                        del.animate({
                                            left:0
                                        },200);
                                    }
                                });
                            });
                        });
                    }
                },
                swipeRight:function(evt){
                    return;
                    /*
                    var  $tag= $(evt.target),bd = $tag.closest('.p-grid-bd'),tr =$tag.closest('tr');

                    if(bd.length&&tr.length){
                        var del = tr.find('.prdOper>.icon-delete').removeClass("fadeInRight fadeInLeft");
                        del.animate({
                            left:0
                        },200);
                    }
                     */
                },
                tap:function(evt){
                    var $target = $(evt.target);
                    if($target.hasClass("icon-delete")){
                        evt.stopPropagation();
                        evt.preventDefault();
                        debugger;
                        var barcode =  $target.closest('tr').data("barcode");
                        pageManage.addOrder({
                            Barcode:barcode,
                            Number:-1
                        });
                    }
                },
                taphold:function(evt){
                    evt.stopPropagation();
                    evt.preventDefault();
                    var  $tag= $(evt.target),$bd = $(this),$tr =$tag.closest('tr');
                    if($tr.length){
                        var barcode =  $tr.data("barcode");
                        var $pop = $("#changeCountWin").show();
                        $("#popBarcode").html(barcode);
                        $("#popCount").val("");
                        $("#popPrice").val("");
                    }
                }
            }
        });


        /**
         * 促销
         */
        $("#grid_cuXiao").grid({
            trTpl:['<tr class="tr-hide"><td class="prdCode"></td>',
                '<td class="prdName"></td><td class="prdPrice"></td><td class="prdCount"></td>',
                '<td class="prdOper"></td></tr>',
                '<tr class="group"><td colspan="5"><i class="iconfont icon-arrow-down"></i><%=PromotionActivity%>赠送数量：<%=GiftNumber%> 已赠送数量：<%=ToGiftNumber%></td></tr>',
                '<tr>',
                '<%for(var i=0;i<GiftList.length;i++){%><td class="prdCode"><%=GiftList[i].Barcode%></td>',
                '<td class="prdName"><%=GiftList[i].Title%></td>',
                '<td class="prdPrice"><%=GiftList[i].Price%></td>',
                '<td class="prdCount"><%=GiftList[i].Inventory%></td>',
                '<td class="prdOper"><i class="iconfont icon-gouwuche"  data-barcode="<%=GiftList[i].Barcode%>"',
                ' data-price="<%=GiftList[i].Price%>" ',
                ' data-giftid="<%=GiftId%>" ',
                ' data-giftpromotionid="<%=GiftPromotionId%>"></i></td></tr><%}%>'].join(''),
            fields:{
                GiftId:null,
                GiftList: null,
                GiftNumber: null,
                GiftPromotionId:null,
                Mode:null,
                Price:null,
                PromotionActivity:null,
                ToGiftNumber:null
            },
            listener:{
                dataChange:function(){

                },
                tap:function(evt){
                    var $target = $(evt.target);
                    if($target.hasClass("icon-gouwuche")){
                        evt.stopPropagation();
                        evt.preventDefault();
                        var barcode = $target.data("barcode"),
                            SalePrice = $target.data("barcode"),
                            GiftId = $target.data("giftid"),
                            GiftPromotionId = $target.data("giftPromotionid");
                        pageManage.addOrder({
                            Barcode:barcode,//必填，商品条码，限长 6-30,向购物清单新增商品。如果为修改数量/售价，可为空
                            //Number:"",//可选，数量,(值范围大于 0)
                            SalePrice:SalePrice,//可选，销售价(值范围大于等于 0)
                            Status:3,//必填， 销售状态， 商品状态 （0： 正常销售商品；1：非促销赠品；2：促销赠送商品；3：促销加购赠品）
                            GiftId:GiftId,//可选，赠品 Id
                            GiftPromotionId:GiftPromotionId//可选，赠品促销活动 Id
                        });

                    }
                }
            }
        });
        /**
         * 查库存
         */
        $("#grid_kuCun").grid({
            listener:{
                tap:function(){
                    //alert(2);
                }
            }
        });
    },
    /**
     * 初始化布局
     * 订单的滚动条
     * 菜单的弹出事件、滚动条
     */
    initLayout:function(){
        /**
         * 广告信息
         */
        $("#newsDiv li:not(:first)").css("display","none");
        var B=$("#newsDiv li:last");
        var C=$("#newsDiv li:first");
        setInterval(function(){
            if(B.is(":visible")){
                C.fadeIn(500).addClass("in");B.hide()
            }else{
                $("#newsDiv li:visible").addClass("in");
                $("#newsDiv li.in").next().fadeIn(500);
                $("li.in").hide().removeClass("in")}
        },3000); //每3秒钟切换一条，你可以根据需要更改


        /*//滑动*/
        $(".p-head-menu").click(function(){
            pageManage.showMenu();
        });
        $(".p-menuToggle").click(function(){
            pageManage.hideMenu();
        });
        /**
         * 退出系统
         */
        $("#loginOutBtn").click(function(){
            Comm.delCookie("userInfo");
            location.href="./login.html";

        });

        /**
         * 右下角几个按钮事件
         */
        $("#paymentBtn").click(function(){
            pageWin.changPage("xuanZeZhiFuFangShi");
        });
        /**
         * 取消订单
         */
        $("#cancelBtn").click(function(){
            dialog({
                title: '消息',
                content:"您确定要清空当前的清单吗？",
                ok: function () {
                    var devCfg = AppJS.getDeviceInfo();
                    Comm.Action.clearOrder({
                        data:{
                            storeId:devCfg.storeId,
                            machineSN:devCfg.machineSN
                        },
                        success:function(res){
                            debugger;
                            $("#grid_orderList").grid("setData",null);
                            pageManage.renderStat(null);
                        },error:function(res){
                            debugger;
                        }
                    });
                },
                cancel:function(){},
                okValue:"确定",
                cancelValue:"取消"
            }).show();
        });
        $("#discountBtn").click(function(){
            pageWin.changPage("zhengDanZheKou");
        });
        /*
        * 促销页
        * */
        $("#cuXiaoBtn").on('click',function(){
            pageWin.changPage("cuXiao");
        });


        var pop = $("#changeCountWin");
        pop.find("input.ok").click(function(){
            var Barcode = $("#popBarcode").html(),
                devInfo = AppJS.getDeviceInfo(),
                count = parseInt($("#popCount").val()),
                price = $("#popPrice").val();
            if(count<0){
                dialog({
                    title: '提示!',
                    content: "数量必须大于0"
                }).showModal();
                return;
            }
            var args = {
                Barcode:Barcode,
                Number:count//,
                //Status:0,//必填， 销售状态， 商品状态 （0： 正常销售商品； 1：非促销赠品；2：促销赠送商品；3：促销加 购赠品）
                //SalePrice:price
            }
            pageManage.addOrder(args)
            pop.hide();
        });
        pop.find("input.cancel").click(function(){
            pop.hide();
        });

        /**
         * 时间表
         */
        window.setInterval(function(){
            $("#clockLocation").html(Comm.getLocaDate());
        },1000);

     },
    showMenu:function(){
        $("#menuScroll").iScroll("refresh");
        $(".p-menuBox").show().removeClass('fadeOutRight').addClass('fadeInRight');
    },
    hideMenu:function(){
        $(".p-menuBox").show().removeClass('fadeInRight').addClass('fadeOutRight');
    },
    /***
     * 菜单事件
     */
    menuListener:function(){
        $("#menuScroll .p-menu-item").on("tap",function(){
            var win = $("#operateWin"),$el=$(this),key=$el.data("key");
            win.show().removeClass('fadeOutRight').addClass('fadeInRight');
            pageWin.changPage(key);
        });
        $("#closeWinBtn").on("tap",function(){
            var win = $(".p-win");
            win.removeClass('fadeInRight').addClass('fadeOutLeft').one("webkitAnimationEnd",function(){
                win.hide();
                win.removeClass("fadeInRight fadeOutLeft fadeOutRight");
            });
        });
    },
    /**
     * 结算
     */
    payment:function(){

    },
    /***
     * 主页下方 条码扫描
     * @param args 见Comm.Action.fetchPrd
     */
    addOrder:function(args){
        if($.trim(args.Barcode)==""){
            return;
        }

        var deviceInfo = AppJS.getDeviceInfo(),
            data = {
                StoreId:deviceInfo.storeId,
                MachineSn:deviceInfo.machineSN,
                Status:0
            };
        $.extend(data,args)
        var option = {
            data:data,
            success:function(res){
                // {Code: null, Message: "未能解析该条码，请确认是否正确！", Result: null}
                //console.log(JSON.stringify(res));
                console.table(res);
                if(res.Code==200){
                    $("#grid_orderList").grid("setData",res.Result.BuyList);
                    $("#grid_cuXiao").grid("setData",res.Result.Gifts);
                    res.Result.Gifts.length?$("#cuXiaoBtn").show():$("#cuXiaoBtn").hide();//促销按钮
                    pageManage.renderStat(res.Result.Statistics);
                }else{
                    dialog({
                        title: '提示!',
                        content: res.Message
                    }).showModal();
                }
            },
            error:function(res){
                dialog({
                    title: '提示!',
                    content:"条码扫描,出错error!"
                }).showModal();
            }
        }
        Comm.Action.fetchPrd(option);
    },
    renderStat:function(data){
        /*{
         Num: 13
         Preferential: 0
         Receivable: 91
         Total: 91
        }*/
        var store = {
            Total:0.00,
            Preferential:0.00,
            Receivable:0.00,
            Num:0
        };
        $.extend(store,data||{});

        var tpl = [
            '<ul><li><label>应收：</label><span>￥<%=Receivable%></span></li>',
                '<li><label>优惠：</label><span >￥<%=Preferential%></span></li>',
                '<li><label>件数：</label><span><%=Num%></span></li></ul>'];
        var render = template.compile(tpl.join(''));
        $("#orderStat").html(render(store));
    },
    /**
     * 显示系统的 用户与设备信息
     */
    showSysInfo:function(){
        /*
        Comm.getCookie("userInfo");
        "{"UserCode":"1008","FullName":"温敬舟","OperateAuth":{"1":[1,3,4],"9":[1,3],"15":[1,3,4],"16":[1,3]}}"
        */
        var user = Comm.getCookie("userInfo");
        if($.trim(user)==""){
            // location.href="./login.html"; //没有登录要跳转 登录页
            return;
        }
        var cfg = AppJS.getDeviceInfo();

        var info =  JSON.parse(user),
            $box = $(".p-appInfo"),
            str = ['<label>机号：{storeId}</label>',
                '<label>状态：销售</label>',
                '<label>操作：销售扫码</label>',
                '<label>出货仓：{storeName}</label>',
                '<label>操作人：{userInfo}</label>',
                '<label>小票打印：打开</label>',
                '<div class="p-appName">道诚 · 科技　商业管理软件</div>'
            ];
        var d  = $.extend({userInfo:info.UserCode+" " + info.FullName},cfg)
        $box.html(str.join('').format(d));
    }
}


/**
 * 菜单打开子页面弹出
 * @type {{initLayout: initLayout, changPage: changPage}}
 */
var pageWin={
    initLayout:function(){
        $("#winMenuScroll li").on("tap",function(){
            var $el=$(this),key=$el.data("key");
            pageWin.changPage(key);
        });
    },
    changPage:function(key){
        var win = $("#operateWin");
        win.show().removeClass('fadeOutRight').addClass('fadeInRight');

        $("#winMenuScroll li").removeClass("seld");
        $("#winMenuScroll li[data-key="+key+"]").addClass("seld");

        var page = $("#"+key+"_page");
        $(".p-win-panel").hide();
        page.show();
        page.find(".cmp-iScroll").iScroll("refresh");
        switch (key){
            case "shouYe":
                $("#operateWin").hide();
                return;
                break;
            case "chaKuChuan"://查库存
                break;
            case "chaJiaGe"://查价格
                break;
            case "chuRuKuan"://出入款
                break;
            case "duiZhan"://对账
                break;
            case "tuiHuo"://退换货
                break;
            case "huanHuo"://换货
                break;
            case "jieBan"://班结
                break;
            case "guaDan"://挂单
                break;
            case "cuXiao"://促销
                break;
            case "zhengDanZheKou"://整单折扣
                break;
            case "duoFangShiFuKuan"://多方式付款
                break;
            case "xuanZeZhiFuFangShi"://选择支付方式
                break;
            case "xianJinZhiFu"://现金支付
                break;
            case "yinLianKaZhiFu"://银联卡支付
                break;
            case "zhiFuBaoZhiFu"://支付宝扫码支付
                break;
        }
    }
};

