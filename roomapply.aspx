<%@ Page Language="C#" AutoEventWireup="true" CodeFile="roomapply.aspx.cs" Inherits="roomapply" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
<link rel="stylesheet" type="text/css" href="StyleSheet.css" />
    <title>roomapply</title>
</head>
<body>
    <form id="form1" runat="server">

         <div align="center">
             <asp:Label ID="lbTitle" runat="server" Text="lbTitle"></asp:Label>
         </div>
        <div>
             <asp:DropDownList ID="DropDownListWeek" runat="server" AutoPostBack="False" DataSourceID="SqlDataSourceRoomDetail" DataTextField="detail" DataValueField="id"></asp:DropDownList>
             <asp:DropDownList ID="DropDownListDepart" runat="server" AutoPostBack="False" DataSourceID="SqlDataSourceDepartment" DataTextField="strDepart" DataValueField="strDepart"></asp:DropDownList>
             <asp:Button ID="Button1" runat="server" Text="SearchButton" OnClick="Button1_Click" />
             <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
             <asp:SqlDataSource ID="SqlDataSourceRoomDetail" runat="server" ConnectionString="<%$ ConnectionStrings:webTestConnectionString %>" SelectCommand="SELECT [id],'第'+CAST(id as varchar(10)) + '周 ' + datePeriod  as detail FROM [WeekStartEnd]"></asp:SqlDataSource>
             <asp:SqlDataSource ID="SqlDataSourceDepartment" runat="server" ConnectionString="<%$ ConnectionStrings:webTestConnectionString %>" SelectCommand="SELECT distinct RTRIM(strDepart) strDepart FROM [RoomDetail]"></asp:SqlDataSource>
             <asp:HyperLink ID="hlLogin" runat="server" NavigateUrl="~/templogin.aspx">登录</asp:HyperLink>


             
                    

             <%-- 各GridView数据在PrintTab中动态生成，去掉单独GridView的绑定 --%>
            <%--<asp:GridView ID="GridView2" runat="server" onrowdatabound="GridView1_RowDataBound1" BorderWidth="1">--%>             
             <asp:placeholder id="GridViewPlaceHolder" runat="Server"/>

                       
        </div>
    </form>
</body>
</html>
