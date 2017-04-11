<%@ Page Language="C#" AutoEventWireup="true" CodeFile="roomapply.aspx.cs" Inherits="roomapply" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
<link rel="stylesheet" type="text/css" href="css/StyleSheet.css" media="screen" />
<link rel="stylesheet" type="text/css" href="css/PrintCss.css" media="print" />
    <script src="Scripts/jquery-3.1.1.js" type="text/javascript"></script>    
    <script type="text/javascript" >
        function ExportDivDataToExcel()
        {
            var html = $("#divExport").html();
            html = $.trim(html);
            html = html.replace(/>/g,'&gt;');
            html = html.replace(/</g,'&lt;');
            $("input[id$='HdnValue']").val(html);
        }
        </script>
    <title>roomapply</title>
</head>
<body>
    <form id="form1" runat="server">        
        <div align="center">
             <asp:Label ID="lbTitle" runat="server" Text="lbTitle"></asp:Label>
        </div>
        <div class="noprint">
             <asp:DropDownList ID="DropDownListWeek" runat="server" AutoPostBack="False" DataSourceID="SqlDataSourceRoomDetail" DataTextField="detail" DataValueField="id"></asp:DropDownList>
             <asp:DropDownList ID="DropDownListDepart" runat="server" AutoPostBack="False" DataSourceID="SqlDataSourceDepartment" DataTextField="strDepart" DataValueField="strDepart"></asp:DropDownList>
             <asp:Button ID="Button1" runat="server" Text="查询" OnClick="Button1_Click" />
             <asp:HyperLink ID="hlRoomQuery" runat="server" NavigateUrl="~/roomQuery.aspx" target="_blank">整体查询</asp:HyperLink>
             <asp:HyperLink ID="hlRoomQueryR" runat="server" NavigateUrl="~/roomQueryReverse.aspx" target="_blank">查询空教室</asp:HyperLink>
             <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
             <asp:SqlDataSource ID="SqlDataSourceRoomDetail" runat="server" ConnectionString="<%$ ConnectionStrings:webTestConnectionString %>" SelectCommand="SELECT [id],'第'+CAST(id as varchar(10)) + '周 ' + datePeriod  as detail FROM [WeekStartEnd]"></asp:SqlDataSource>
             <asp:SqlDataSource ID="SqlDataSourceDepartment" runat="server" ConnectionString="<%$ ConnectionStrings:webTestConnectionString %>" SelectCommand="SELECT distinct RTRIM(strDepart) strDepart FROM [RoomDetail] order by 1 desc"></asp:SqlDataSource>
             <asp:HyperLink ID="hlLogin" runat="server" NavigateUrl="~/templogin.aspx" target="_blank">登录</asp:HyperLink>
             <asp:Button ID="btExcel" runat="server" onclick="btExcel_Click" Text="导出" OnClientClick="ExportDivDataToExcel()"/>
             <asp:Label ID="lbPrint" runat="server" Text="打印请查询后按ctrl+p" ToolTip="建议使用Chrome浏览器打印，可选择是否显示页眉页脚、缩放页面范围大小等"/>
            </div>
        <div id="divExport">        
             <asp:placeholder id="GridViewPlaceHolder" runat="Server"/>                       
        </div>
        <asp:HiddenField ID="HdnValue" runat="server" />
    </form>
</body>
</html>
