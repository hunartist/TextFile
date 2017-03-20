<%@ Page Language="C#" AutoEventWireup="true"  CodeFile="templogin.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>登录页面</title>
</head>
<body>
    <form id="form1" runat="server" style="text-align:center;"><br />
    <div style="width:200px; height:200px; vertical-align:middle; text-align:center;">
    
        <br />
        <br />
        <br />
        <table style="width:100%;">
            <tr>
                <td>
    
        <asp:Label ID="Label1" runat="server" Text="UserName:"></asp:Label>
                </td>
                <td colspan="2">
        <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
        <asp:Label ID="Label2" runat="server" Text="Pwd:"></asp:Label>
                </td>
                <td colspan="2">
        <asp:TextBox ID="TextBox2" runat="server" TextMode="Password"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
        <asp:Button ID="Button1" runat="server" Text="登录" onclick="Button1_Click" />
    
                </td>
                <td>
        <asp:Button ID="Button2" runat="server" Text="取消" onclick="Button2_Click" />
    
                </td>
                <td>
        <asp:Button ID="Button3" runat="server" Text="注销" onclick="Button3_Click" 
                        style="height: 26px" />
    
                </td>
            </tr>
        </table>
    
    </div>
    </form>
</body>
</html>
