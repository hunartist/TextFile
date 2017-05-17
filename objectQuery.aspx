<%@ Page Language="C#" AutoEventWireup="true" CodeFile="objectQuery.aspx.cs" Inherits="objectQuery" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
<link rel="stylesheet" type="text/css" href="css/StyleSheet.css" media="screen" />
<link rel="stylesheet" type="text/css" href="css/PrintCss.css" media="print" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div class="noprint">
            <asp:DropDownList ID="ddlWeek1" runat="server" AutoPostBack="False" DataSourceID="sqsWeek" DataTextField="detail" DataValueField="intWeek"></asp:DropDownList>
            <asp:DropDownList ID="ddlWeek2" runat="server" AutoPostBack="False" DataSourceID="sqsWeek" DataTextField="detail" DataValueField="intWeek"></asp:DropDownList>
            <asp:DropDownList ID="ddlDepart" runat="server" AutoPostBack="true" DataSourceID="sqsDep" DataTextField="strDepart" DataValueField="depid"></asp:DropDownList>
            <asp:DropDownList ID="ddlRoom" runat="server" AutoPostBack="False" DataSourceID="sqsRoom" DataTextField="strRoomName" DataValueField="roomid"></asp:DropDownList>
            <asp:Button ID="btSearch" runat="server" Text="查询" OnClick="btSearch_Click" />
            <asp:SqlDataSource ID="sqsWeek" runat="server" ConnectionString="<%$ ConnectionStrings:webTestConnectionString %>" SelectCommand="SELECT [intWeek],'第'+CAST(w.intWeek as varchar(10)) + '周 ' + w.datePeriod  as detail FROM [WeekStartEnd] w inner join TitleStartEnd t on w.yearID = t.yearID and t.currentFlag = 'true'"></asp:SqlDataSource>
            <asp:SqlDataSource ID="sqsDep" runat="server" ConnectionString="<%$ ConnectionStrings:webTestConnectionString %>" SelectCommand="SELECT distinct depid,strDepart FROM [Department] order by 2 desc"></asp:SqlDataSource>
            <asp:SqlDataSource ID="sqsRoom" runat="server" ConnectionString="<%$ ConnectionStrings:webTestConnectionString %>" SelectCommand="SELECT [roomid], [strRoomName] FROM [RoomDetail] WHERE ([depid] = @depid)">
                <SelectParameters>
                    <asp:ControlParameter ControlID="ddlDepart" PropertyName="SelectedValue" Name="depid" Type="Int32"></asp:ControlParameter>
                </SelectParameters>
            </asp:SqlDataSource>            
        </div>
        <div id="divExport" style="text-align:center;margin:0 auto">        
                 <asp:placeholder id="GridViewPlaceHolder" runat="Server"/>                       
        </div>
    </form>
</body>
</html>
