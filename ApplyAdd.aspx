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
                <asp:Parameter Name="strYearID" Type="String" />
                <asp:Parameter Name="id" Type="String" />
            </InsertParameters>
        </asp:SqlDataSource>

        <asp:SqlDataSource ID="SqlDataSourceRoom" runat="server" ConnectionString="<%$ ConnectionStrings:webTestConnectionString %>" SelectCommand="SELECT distinct [strRoomName]+' '+[strDepart] as strRoomNameShow,[strRoomName] FROM [RoomDetail] Where ([strCDep] = @strCDep)">
            <SelectParameters>
                <asp:SessionParameter Name="strCDep" SessionField="dep" Type="String" />
            </SelectParameters>
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="SqlDataSourceWeek" runat="server" ConnectionString="<%$ ConnectionStrings:webTestConnectionString %>" SelectCommand="SELECT [id] FROM [WeekStartEnd]"></asp:SqlDataSource>
        <asp:SqlDataSource ID="SqlDataSourceYear" runat="server" ConnectionString="<%$ ConnectionStrings:webTestConnectionString %>" SelectCommand="SELECT [yearID] FROM [TitleStartEnd] WHERE [currentFlag] = 'true'"></asp:SqlDataSource>

        <asp:Label ID="lbYear" runat="server" Text="学期"></asp:Label>
        <asp:DropDownList ID="ddlYear" runat="server" DataSourceID="SqlDataSourceYear" DataTextField="yearID" DataValueField="yearID"></asp:DropDownList>

        <asp:Label ID="lbRoom" runat="server" Text="教室"></asp:Label>
        <asp:DropDownList ID="ddlRoom" runat="server" DataSourceID="SqlDataSourceRoom" DataTextField="strRoomNameShow" DataValueField="strRoomName"></asp:DropDownList>

        <asp:Label ID="lbDay" runat="server" Text="星期几"></asp:Label>
        <asp:DropDownList ID="ddlDay" runat="server">
            <asp:ListItem>1</asp:ListItem>
            <asp:ListItem>2</asp:ListItem>
            <asp:ListItem>3</asp:ListItem>
            <asp:ListItem>4</asp:ListItem>
            <asp:ListItem>5</asp:ListItem>
            <asp:ListItem>6</asp:ListItem>
            <asp:ListItem>7</asp:ListItem>
        </asp:DropDownList>

        <asp:Label ID="lbStartN" runat="server" Text="开始节次"></asp:Label>
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

        <asp:Label ID="lbEndN" runat="server" Text="结束节次"></asp:Label>
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

        <asp:Label ID="lbStartW" runat="server" Text="开始周"></asp:Label>
        <asp:DropDownList ID="ddlStartW" runat="server" DataSourceID ="SqlDataSourceWeek"  DataTextField="id" DataValueField="id" AutoPostBack="True" OnSelectedIndexChanged="ddlStartW_SelectedIndexChanged"></asp:DropDownList>

        <asp:Label ID="lbEndW" runat="server" Text="结束周"></asp:Label>
        <asp:DropDownList ID="ddlEndW" runat="server" DataSourceID ="SqlDataSourceWeek"  DataTextField="id" DataValueField="id"></asp:DropDownList>

        <asp:Label ID="lbName" runat="server" Text="课程名称"></asp:Label>
        <asp:TextBox ID="tbName" runat="server"></asp:TextBox>

        <asp:Label ID="lbClass" runat="server" Text="班级"></asp:Label>
        <asp:TextBox ID="tbClass" runat="server"></asp:TextBox>

        <asp:Label ID="lbTeacher" runat="server" Text="教师"></asp:Label>
        <asp:TextBox ID="tbTeacher" runat="server"></asp:TextBox>
    </div>
        <asp:Button ID="ButtonSave" runat="server" Text="save" OnClick="ButtonSave_Click" />
        <asp:HyperLink ID="hlEdit" runat="server" NavigateUrl="~/ApplyEdit.aspx">返回编辑</asp:HyperLink>
    </form>
</body>
</html>
