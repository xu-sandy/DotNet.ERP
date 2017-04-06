/**
 * Created by Administrator on 15-10-19.
 */
var Comm={
    rootPath:"http://27.154.234.10:8012/Api",
    isMb:function(){
        var sUserAgent = navigator.userAgent.toLowerCase();
        var bIsIpad = sUserAgent.match(/ipad/i) == 'ipad';
        var bIsIphone = sUserAgent.match(/iphone os/i) == 'iphone os';
        var bIsMidp = sUserAgent.match(/midp/i) == 'midp';
        var bIsUc7 = sUserAgent.match(/rv:1.2.3.4/i) == 'rv:1.2.3.4';
        var bIsUc = sUserAgent.match(/ucweb/i) == 'web';
        var bIsCE = sUserAgent.match(/windows ce/i) == 'windows ce';
        var bIsWM = sUserAgent.match(/windows mobile/i) == 'windows mobile';

        if(bIsIpad || bIsIphone || bIsMidp || bIsUc7 || bIsUc || bIsCE || bIsWM){
            return !0;
        }else{
            return !1;
        }
    },
    setCookie:function (name,value)
    {
        var Days = 30;
        var exp = new Date();
        exp.setTime(exp.getTime() + Days*24*60*60*1000);
        document.cookie = name + "="+ escape (value) + ";expires=" + exp.toGMTString();
    },
    getCookie:function (name)
    {
        var arr,reg=new RegExp("(^| )"+name+"=([^;]*)(;|$)");
        if(arr=document.cookie.match(reg))
            return unescape(arr[2]);
        else
            return null;
    },
    delCookie:function (name)
    {
        var exp = new Date();
        exp.setTime(exp.getTime() - 1);
        var cval=Comm.getCookie(name);
        if(cval!=null)
            document.cookie= name + "="+cval+";expires="+exp.toGMTString();
    },
    getLocaDate:function(){
        var now=new Date();
        var year=now.getYear();
        var month=now.getMonth();
        var day=now.getDate();
        var hours=now.getHours();
        var minutes=now.getMinutes();
        var seconds=now.getSeconds();
        return hours+":"+minutes+":"+seconds+"";
    }
}
Comm.Uri={
    /**
     * 登录
     */
    login:Comm.rootPath+"/Login",
    /**
     * 取销售商品
     */
    fetchPrd:Comm.rootPath+"/Sale",
    /**
     * 取消、清空销售清单
     */
    clearOrder:Comm.rootPath+"/ClearOrder"
}
Comm.Action =  {
    request:function(config){
        var cfg={
            type:"post",
            url:"",
            data:null,
            dataType:"json",
            success:function(){},
            error:function(){}
        };
        $.extend(cfg,config);
        $.ajax(cfg);
    },
    /**
     * 参数：
     * @param option
     *  param:{ },
        success:function ,
        error:function
    */
    login:function(config){
        /**
         * 登录Aip说明
         {
            storeId  必填，门店 ID，限长 2-3
            machineSN  必填，Pos 设备编号
            account  必填，登录账号，限长 3-16
            pwd  必填，登录密码（MD5）
         }
         * @returns object
         *范例:
         {
         "Uid":"02",
         "FullName":"蔡少发",
         "OperateAuth":[{门店 ID:角色 ID}, {门店 ID:角色 ID}]
         }
         */
        var cfg=$.extend({
            url:Comm.Uri.login
        },config);
        Comm.Action.request(cfg);
    },
    fetchPrd:function(config){
        /** 扫描商品编号
        data = {
            Barcode:"4719858573920",必填，商品条码，限长 6-30,向购物清单新增商品。如果为修改数量/售价，可为空
            StoreId:1,必填，商店 Id
            MachineSn:"001",必填，POS 设备编号
            Number:"",可选，数量,(值范围大于 0)
            SalePrice:"",可选，销售价(值范围大于等于 0)
            Status:1,必填， 销售状态， 商品状态 （0： 正常销售商品；1：非促销赠品；2：促销赠送商品；3：促销加购赠品）
            GiftId:"",可选，赠品 Id
            GiftPromotionId:""可选，赠品促销活动 Id
        }
        */

        var cfg=$.extend({
            url:Comm.Uri.fetchPrd
        },config);
        Comm.Action.request(cfg);
    },
    clearOrder:function(config){
        var cfg=$.extend({
            url:Comm.Uri.clearOrder
        },config);
        Comm.Action.request(cfg);
    }
}

String.prototype.format = function(args) {
    var result = this;
    if (arguments.length > 0) {
        if (arguments.length == 1 && typeof (args) == "object") {
            for (var key in args) {
                if(args[key]!=undefined){
                    var reg = new RegExp("({" + key + "})", "g");
                    result = result.replace(reg, args[key]);
                }
            }
        }
        else {
            for (var i = 0; i < arguments.length; i++) {
                if (arguments[i] != undefined) {
                    var reg = new RegExp("({[" + i + "]})", "g");
                    result = result.replace(reg, arguments[i]);
                }
            }
        }
    }
    return result;
}