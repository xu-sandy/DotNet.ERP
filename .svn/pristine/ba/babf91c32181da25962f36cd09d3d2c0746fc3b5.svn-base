<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PayFor.aspx.cs" Inherits="Pharos.Pay.Retailing.PayFor" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div align="center">
		    <br/><br/><br/>
            <asp:Button ID="submit" runat="server" Text="立即支付" OnClientClick="return callpay()" style="width:210px; height:50px; border-radius: 15px;background-color:#00CD00; border:0px #FE6714 solid; cursor: pointer;  color:white;  font-size:16px;" />
	    </div>
    </form>
</body>
</html>
<script type="text/javascript">
    //调用微信JS api 支付
    function jsApiCall()
    {
        WeixinJSBridge.invoke(
        'getBrandWCPayRequest',
        <%=wxJsApiParam%>,//josn串
        function (res)
        {
            alert(res.err_code + res.err_desc + res.err_msg);
            WeixinJSBridge.log(res.err_msg);
        }
        );
    }

    function callpay()
    {
        if (typeof WeixinJSBridge == "undefined")
        {
            if (document.addEventListener)
            {
                document.addEventListener('WeixinJSBridgeReady', jsApiCall, false);
            }
            else if (document.attachEvent)
            {
                document.attachEvent('WeixinJSBridgeReady', jsApiCall);
                document.attachEvent('onWeixinJSBridgeReady', jsApiCall);
            }
        }
        else
        {
            jsApiCall();
        }
        return false;
    }
               
</script>