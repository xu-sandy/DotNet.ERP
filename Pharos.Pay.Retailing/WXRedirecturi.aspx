<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WXRedirecturi.aspx.cs" Inherits="Pharos.Pay.Retailing.WXRedirecturi" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        hello:
        <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
    </div>
        <asp:Label ID="Label2" runat="server" Text="Label"></asp:Label>
        <p>
            <asp:LinkButton ID="LinkButton1" runat="server" OnClick="LinkButton1_Click">支付页面</asp:LinkButton>
            <asp:LinkButton ID="LinkButton2" runat="server" OnClick="LinkButton2_Click">支付页面测试</asp:LinkButton>
        </p>
    </form>
</body>
</html>
