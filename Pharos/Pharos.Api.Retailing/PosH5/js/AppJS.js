/**
 * Created by Administrator on 15-9-25.
 */
var AppJS={callback:{}};
/**
 * 返回设备信息
 * return string
 */
AppJS.getDeviceInfo = function(){
    /*测试1009 张三
     123456
     门店标识03，是汇怡8店的
    */
    var cfg = {
        name:"东本",//软件名
        machineSN:"1601",// 必填，Pos 设备编号
        storeId:"16",// 必填，门店 ID，限长 2-3
        storeName:"汇怡16店",//门店名称
        storePhone:"0592-1234567",//门店电话
        url:"http://27.154.234.10:8012/PosH5/login.html",//系统入口地址
        entryPoint:0// 必填，入口点（0：PosApp 1:Mobile APP）
    }
    var strCfg = window.Device&&window.Device.getDeviceInfo()||"{}";
    var config = JSON.parse(strCfg);
    //config.url = "http://27.154.234.10:8012/PosH5/login.html";
    config.entryPoint = 0;
    return Comm.isMb()?config:cfg;
};
/**
 * 设置设备信息
 * @param name  软件名
 * @param machineSN  必填，Pos 设备编号
 * @param storeId 必填，门店 ID，限长 2-3
 * @param storeName 门店名称
 * @param storePhone 门店电话
 * @param url 系统入口地址
 */
AppJS.setDeviceInfo = function(name,machineSN,storeId,storeName,storePhone,url){
    window.Device&&window.Device.setDeviceInfo(name,machineSN,storeId,storeName,storePhone,url);
}
/**
 * 扫描二维码
 * @param textId
 * @constructor
 */
AppJS.ScanningCode=function(textId){
    window.Device&&window.Device.ScanningCode(textId);
};
/**
 * 打印
 * @param data {json}
 */
AppJS.printData = function(data){
    window.Device&&window.Device.printData(data);

}
/*=======以下是设备回调============================================================*/

/**
 * 扫描回调  AppJS.callback.ScanningCode(textId)
 * @param textId
 * @constructor
 */
AppJS.callback.ScanningCode=function(num,textId){
    var result = num,$el=$("#"+textId);
    if($el.data("format")=="code"){
        result=result.replace(/\s/g,"");
        result = result.replace(/(\d)(?=(?:\d{4})+$)/g, '$1 ');
    }
    $el.val("");
    var cb = $el.data("callback");
    try{
        eval("var callBcakFn = "+cb);
        typeof callBcakFn === "function" && callBcakFn({
            Barcode:barcode,
            Status:0
        });
    }catch (e){
        alert(e.message);
    }
};