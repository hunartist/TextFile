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
    <div>
    
    </div>
        <asp:SqlDataSource ID="SqlDataSourceRoomApply" runat="server" ConnectionString="<%$ ConnectionStrings:webTestConnectionString %>" SelectCommand="select distinct a.strRoom,a.intDay,a.intStartNum,a.intEndNum,a.strName,a.strClass,a.strTeacher,a.intStartWeek,a.intEndWeek,a.strApplication from RoomApply a inner join RoomApplySub s on a.Id=s.F_id"></asp:SqlDataSource>
        <asp:GridView ID="GridView10" runat="server" AutoGenerateColumns="False" DataSourceID="SqlDataSourceRoomApply" EnableModelValidation="True">
            <Columns>
                <asp:BoundField DataField="strRoom" HeaderText="strRoom" SortExpression="strRoom" />
                <asp:BoundField DataField="intDay" HeaderText="intDay" SortExpression="intDay" />
                <asp:BoundField DataField="intStartNum" HeaderText="intStartNum" SortExpression="intStartNum" />
                <asp:BoundField DataField="intEndNum" HeaderText="intEndNum" SortExpression="intEndNum" />
                <asp:BoundField DataField="strName" HeaderText="strName" SortExpression="strName" />
                <asp:BoundField DataField="strClass" HeaderText="strClass" SortExpression="strClass" />
                <asp:BoundField DataField="strTeacher" HeaderText="strTeacher" SortExpression="strTeacher" />
                <asp:BoundField DataField="intStartWeek" HeaderText="intStartWeek" SortExpression="intStartWeek" />
                <asp:BoundField DataField="intEndWeek" HeaderText="intEndWeek" SortExpression="intEndWeek" />
                <asp:BoundField DataField="strApplication" HeaderText="strApplication" SortExpression="strApplication" />
            </Columns>
        </asp:GridView>

         <div>
            <asp:GridView ID="GridView1" runat="server" 
                onrowdatabound="GridView1_RowDataBound1" BorderWidth="1">
                <HeaderStyle Wrap="False" />
                <RowStyle HorizontalAlign="Center" VerticalAlign="Middle" />
            </asp:GridView>
        </div>
    </form>
</body>
</html>
