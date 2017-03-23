<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ApplyAdd.aspx.cs" Inherits="ApplyAdd" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>

        <asp:SqlDataSource ID="SqlDataSourceRoomApply" runat="server" ConnectionString="<%$ ConnectionStrings:webTestConnectionString %>" InsertCommandType ="StoredProcedure" InsertCommand="RoomApplyAction">
            <InsertParameters>
                <asp:Parameter Name="Action" Type="String" />
                <asp:Parameter Name="strRoom" Type="String" />
                <asp:Parameter Name="intDay" Type="int16" />
                <asp:Parameter Name="intStartNum" Type="int16" />
                <asp:Parameter Name="intEndNum" Type="int16" />
                <asp:Parameter Name="intStartWeek" Type="int16" />
                <asp:Parameter Name="intEndWeek" Type="int16" />
                <asp:Parameter Name="strName" Type="String" />
                <asp:Parameter Name="strClass" Type="String" />
                <asp:Parameter Name="strTeacher" Type="String" />
                <asp:Parameter Name="id" Type="String" />
            </InsertParameters>
        </asp:SqlDataSource>

        <asp:SqlDataSource ID="SqlDataSourceRoom" runat="server" ConnectionString="<%$ ConnectionStrings:webTestConnectionString %>" SelectCommand="SELECT distinct [strRoomName]+' '+[strDepart] as strRoomNameShow,[strRoomName] FROM [RoomDetail]"></asp:SqlDataSource>
        <asp:SqlDataSource ID="SqlDataSourceWeek" runat="server" ConnectionString="<%$ ConnectionStrings:webTestConnectionString %>" SelectCommand="SELECT [id] FROM [WeekStartEnd]"></asp:SqlDataSource>

        <asp:Label ID="lbRoom" runat="server" Text="Room"></asp:Label>
        <asp:DropDownList ID="ddlRoom" runat="server" DataSourceID="SqlDataSourceRoom" DataTextField="strRoomNameShow" DataValueField="strRoomName"></asp:DropDownList>

        <asp:Label ID="lbDay" runat="server" Text="Day"></asp:Label>
        <asp:DropDownList ID="ddlDay" runat="server">
            <asp:ListItem>1</asp:ListItem>
            <asp:ListItem>2</asp:ListItem>
            <asp:ListItem>3</asp:ListItem>
            <asp:ListItem>4</asp:ListItem>
            <asp:ListItem>5</asp:ListItem>
            <asp:ListItem>6</asp:ListItem>
            <asp:ListItem>7</asp:ListItem>
        </asp:DropDownList>

        <asp:Label ID="lbStartN" runat="server" Text="StartN"></asp:Label>
        <asp:DropDownList ID="ddlStartN" runat="server">
            <asp:ListItem>1</asp:ListItem>
            <asp:ListItem>2</asp:ListItem>
            <asp:ListItem>3</asp:ListItem>
            <asp:ListItem>4</asp:ListItem>
            <asp:ListItem>5</asp:ListItem>
            <asp:ListItem>6</asp:ListItem>
            <asp:ListItem>7</asp:ListItem>
            <asp:ListItem>8</asp:ListItem>
            <asp:ListItem>9</asp:ListItem>
            <asp:ListItem>10</asp:ListItem>
        </asp:DropDownList>

        <asp:Label ID="lbEndN" runat="server" Text="EndN"></asp:Label>
        <asp:DropDownList ID="ddlEndN" runat="server">
            <asp:ListItem>1</asp:ListItem>
            <asp:ListItem>2</asp:ListItem>
            <asp:ListItem>3</asp:ListItem>
            <asp:ListItem>4</asp:ListItem>
            <asp:ListItem>5</asp:ListItem>
            <asp:ListItem>6</asp:ListItem>
            <asp:ListItem>7</asp:ListItem>
            <asp:ListItem>8</asp:ListItem>
            <asp:ListItem>9</asp:ListItem>
            <asp:ListItem>10</asp:ListItem>
        </asp:DropDownList>

        <asp:Label ID="lbStartW" runat="server" Text="StartW"></asp:Label>
        <asp:DropDownList ID="ddlStartW" runat="server" DataSourceID ="SqlDataSourceWeek"  DataTextField="id" DataValueField="id"></asp:DropDownList>

        <asp:Label ID="lbEndW" runat="server" Text="EndW"></asp:Label>
        <asp:DropDownList ID="ddlEndW" runat="server" DataSourceID ="SqlDataSourceWeek"  DataTextField="id" DataValueField="id"></asp:DropDownList>

        <asp:Label ID="lbName" runat="server" Text="Name"></asp:Label>
        <asp:TextBox ID="tbName" runat="server"></asp:TextBox>

        <asp:Label ID="lbClass" runat="server" Text="Class"></asp:Label>
        <asp:TextBox ID="tbClass" runat="server"></asp:TextBox>

        <asp:Label ID="lbTeacher" runat="server" Text="Teacher"></asp:Label>
        <asp:TextBox ID="tbTeacher" runat="server"></asp:TextBox>
    </div>
        <asp:Button ID="ButtonSave" runat="server" Text="save" OnClick="ButtonSave_Click" />
    </form>
</body>
</html>
