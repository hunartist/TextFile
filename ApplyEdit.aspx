﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ApplyEdit.aspx.cs" Inherits="NextWebF" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>edit</title>
</head>
<body>
    <script language="C#" runat="server">
    </script>
    <form id="form1" method="post" runat="server">
    
        <asp:SqlDataSource ID="SqlDataSourceRoomApply" runat="server" ConnectionString="<%$ ConnectionStrings:webTestConnectionString %>" SelectCommand="select distinct a.id,a.strRoom,a.intDay,a.intStartNum,a.intEndNum,a.intStartWeek,a.intEndWeek,a.strName,a.strClass,a.strTeacher from RoomApply a ,RoomDetail d where a.strRoom = d.strRoomName and d.strDepart = @strDepart"
            UpdateCommand="UPDATE [RoomApply] SET [strName] = @strName  WHERE [id] = @original_id"
            >
            <UpdateParameters>
                <asp:Parameter Name="strRoom" Type="String" />
                <asp:Parameter Name="intDay" Type="String" />
                <asp:Parameter Name="intStartNum" Type="String" />
                <asp:Parameter Name="intEndNum" Type="String" />
                <asp:Parameter Name="intStartWeek" Type="String" />
                <asp:Parameter Name="intEndWeek" Type="String" />
                <asp:Parameter Name="strName" Type="String" />
                <asp:Parameter Name="strClass" Type="String" />
                <asp:Parameter Name="strTeacher" Type="String" />
                <asp:Parameter Name="original_id" Type="String" />
            </UpdateParameters>
            <SelectParameters>
                <asp:ControlParameter ControlID ="DropDownListDepart" Name ="strDepart" PropertyName ="SelectedValue" Type ="String" />
            </SelectParameters>
        </asp:SqlDataSource>       
        <asp:SqlDataSource ID="SqlDataSourceDepartment" runat="server" ConnectionString="<%$ ConnectionStrings:webTestConnectionString %>" SelectCommand="SELECT distinct RTRIM(strDepart) strDepart FROM [RoomDetail]"></asp:SqlDataSource>
        <asp:DropDownList ID="DropDownListDepart" runat="server" AutoPostBack="True" DataSourceID="SqlDataSourceDepartment" DataTextField="strDepart" DataValueField="strDepart"></asp:DropDownList>
        <asp:Label ID="LabelID" runat="server" Text="LabelID"></asp:Label>
           
        <asp:Button ID="ButtonNew" runat="server" Text="New" OnClick="ButtonNew_Click" />

        <asp:GridView ID="GridView10" runat="server" AutoGenerateColumns="False" DataSourceID="SqlDataSourceRoomApply" EnableModelValidation="True" AutoGenerateDeleteButton="True" AllowPaging="True" AllowSorting="True" HorizontalAlign="Left" AutoGenerateSelectButton="True" AutoGenerateEditButton="True" DataKeyNames="id" OnRowUpdated="GridView10_RowUpdated" OnSelectedIndexChanging="GridView10_SelectedIndexChanging" OnRowEditing="GridView10_RowEditing">
            <Columns>
                <asp:BoundField DataField="id" HeaderText="id" SortExpression="id" InsertVisible="False" ReadOnly="True" />
                <asp:BoundField DataField="strRoom" HeaderText="strRoom" SortExpression="strRoom" />
                <asp:BoundField DataField="intDay" HeaderText="intDay" SortExpression="intDay" />
                <asp:BoundField DataField="intStartNum" HeaderText="intStartNum" SortExpression="intStartNum" />
                <asp:BoundField DataField="intEndNum" HeaderText="intEndNum" SortExpression="intEndNum" />
                <asp:BoundField DataField="intStartWeek" HeaderText="intStartWeek" SortExpression="intStartWeek" />                
                <asp:BoundField DataField="intEndWeek" HeaderText="intEndWeek" SortExpression="intEndWeek" />
                <asp:BoundField DataField="strName" HeaderText="strName" SortExpression="strName" />
                <asp:BoundField DataField="strClass" HeaderText="strClass" SortExpression="strClass" />
                <asp:BoundField DataField="strTeacher" HeaderText="strTeacher" SortExpression="strTeacher" />
            </Columns>
        </asp:GridView>        

        

        

        

    </form>
</body>
</html>
