﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ApplyEdit.aspx.cs" Inherits="NextWebF" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>edit</title>
</head>
<body>
    <form id="form1" runat="server">
    
        <asp:SqlDataSource ID="SqlDataSourceRoomApply" runat="server" ConnectionString="<%$ ConnectionStrings:webTestConnectionString %>" SelectCommand=" select * from (select distinct s.intWeek, a.strRoom,a.intDay,a.intStartNum,a.intEndNum,a.strName,a.strClass,a.strTeacher from RoomApply a inner join RoomApplySub s on a.Id=s.F_id) as aa right join RoomDetail d on aa.strRoom = d.strRoomName where aa.intWeek is not null and d.strDepart = @strDepart">
            <SelectParameters>
                <asp:ControlParameter ControlID ="DropDownListDepart" Name ="strDepart" PropertyName ="SelectedValue" Type ="String" />
            </SelectParameters>
        </asp:SqlDataSource>       
        <asp:SqlDataSource ID="SqlDataSourceDepartment" runat="server" ConnectionString="<%$ ConnectionStrings:webTestConnectionString %>" SelectCommand="SELECT distinct RTRIM(strDepart) strDepart FROM [RoomDetail]"></asp:SqlDataSource>
        <asp:DropDownList ID="DropDownListDepart" runat="server" AutoPostBack="False" DataSourceID="SqlDataSourceDepartment" DataTextField="strDepart" DataValueField="strDepart"></asp:DropDownList>
           

        <asp:GridView ID="GridView10" runat="server" AutoGenerateColumns="False" DataSourceID="SqlDataSourceRoomApply" EnableModelValidation="True" AutoGenerateDeleteButton="True" AutoGenerateEditButton="True" AllowPaging="True" AllowSorting="True" HorizontalAlign="Left" DataKeyNames="Id">
            <Columns>
                <asp:BoundField DataField="intWeek" HeaderText="intWeek" SortExpression="intWeek" />
                <asp:BoundField DataField="strRoom" HeaderText="strRoom" SortExpression="strRoom" />
                <asp:BoundField DataField="intDay" HeaderText="intDay" SortExpression="intDay" />
                <asp:BoundField DataField="intStartNum" HeaderText="intStartNum" SortExpression="intStartNum" />
                <asp:BoundField DataField="intEndNum" HeaderText="intEndNum" SortExpression="intEndNum" />
                <asp:BoundField DataField="strName" HeaderText="strName" SortExpression="strName" />
                <asp:BoundField DataField="strClass" HeaderText="strClass" SortExpression="strClass" />
                <asp:BoundField DataField="strTeacher" HeaderText="strTeacher" SortExpression="strTeacher" />
                <asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" ReadOnly="True" />
                <asp:BoundField DataField="strRoomName" HeaderText="strRoomName" SortExpression="strRoomName" />
                <asp:BoundField DataField="strDepart" HeaderText="strDepart" SortExpression="strDepart" />
            </Columns>
        </asp:GridView>

        

    </form>
</body>
</html>