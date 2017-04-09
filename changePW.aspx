<%@ Page Language="C#" AutoEventWireup="true" CodeFile="changePW.aspx.cs" Inherits="changePW" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Label ID="lbName" runat="server" Text="用户名" AssociatedControlID ="tbName"></asp:Label>
        <asp:TextBox ID="tbName" runat="server" ReadOnly="true"></asp:TextBox>
        <br />
        <asp:Label ID="lbOldPW" runat="server" Text="旧密码" AssociatedControlID ="tbOldPW"></asp:Label>
        <asp:TextBox ID="tbOldPW" runat="server"></asp:TextBox>
        <br />
        <asp:Label ID="lbNewPW1" runat="server" Text="新密码" AssociatedControlID ="tbNewPW1"></asp:Label>
        <asp:TextBox ID="tbNewPW1" runat="server"></asp:TextBox>
        <br />
        <asp:Label ID="lbNewPW2" runat="server" Text="确认新密码" AssociatedControlID ="tbNewPW2"></asp:Label>
        <asp:TextBox ID="tbNewPW2" runat="server"></asp:TextBox>
        <br />
        <asp:Button ID="btOK" runat="server" Text="确定" OnClick="btOK_Click" />
    </div>
    </form>
</body>
</html>
