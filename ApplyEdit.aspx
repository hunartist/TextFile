<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ApplyEdit.aspx.cs" Inherits="NextWebF" %>

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
            UpdateCommandType ="StoredProcedure" UpdateCommand="RoomApplyAction" 
            DeleteCommandType ="StoredProcedure" DeleteCommand="RoomApplyAction"
            >
            <UpdateParameters>
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
            </UpdateParameters>
            <DeleteParameters>
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
            </DeleteParameters>
            <SelectParameters>
                <asp:ControlParameter ControlID ="DropDownListDepart" Name ="strDepart" PropertyName ="SelectedValue" Type ="String" />
            </SelectParameters>
        </asp:SqlDataSource>       
        <asp:SqlDataSource ID="SqlDataSourceDepartment" runat="server" ConnectionString="<%$ ConnectionStrings:webTestConnectionString %>" SelectCommand="SELECT distinct RTRIM(strDepart) strDepart FROM [RoomDetail]"></asp:SqlDataSource>
        <asp:SqlDataSource ID="SqlDataSourceRoom" runat="server" ConnectionString="<%$ ConnectionStrings:webTestConnectionString %>" SelectCommand="SELECT [strRoomName] FROM [RoomDetail] WHERE ([strDepart] = @strDepart)
select d.strRoomName from RoomApply a right join RoomDetail d on a.strRoom = d.strRoomName where d.strDepart = @strDepart">
            <SelectParameters>
                <asp:ControlParameter ControlID="DropDownListDepart" Name="strDepart" PropertyName="SelectedValue" Type="String" />
            </SelectParameters>
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="SqlDataSourceWeek" runat="server" ConnectionString="<%$ ConnectionStrings:webTestConnectionString %>" SelectCommand="SELECT [id] FROM [WeekStartEnd]"></asp:SqlDataSource>
        <asp:DropDownList ID="DropDownListDepart" runat="server" AutoPostBack="True" DataSourceID="SqlDataSourceDepartment" DataTextField="strDepart" DataValueField="strDepart"></asp:DropDownList>
        <asp:Label ID="LabelID" runat="server" Text="LabelID"></asp:Label>
        <asp:Label ID="LabelMsg" runat="server" Text="LabelMsg" ForeColor="Red"></asp:Label>

        <asp:GridView ID="GridView10" runat="server" AutoGenerateColumns="False" DataSourceID="SqlDataSourceRoomApply" EnableModelValidation="True" AllowPaging="True" AllowSorting="True" HorizontalAlign="Left" AutoGenerateEditButton="True" DataKeyNames="id" OnRowUpdated="GridView10_RowUpdated" OnSelectedIndexChanging="GridView10_SelectedIndexChanging" OnRowEditing="GridView10_RowEditing" OnRowUpdating="GridView10_RowUpdating" OnRowCancelingEdit="GridView10_RowCancelingEdit" OnRowDeleted="GridView10_RowDeleted">
            <Columns>
                <%--<asp:BoundField DataField="intStartWeek" HeaderText="开始周" SortExpression="intStartWeek" />--%>     
                <%--<asp:BoundField DataField="intEndWeek" HeaderText="结束周" SortExpression="intEndWeek" />--%>
                <asp:TemplateField ShowHeader="False">
                    <ItemTemplate>
                        <asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False"
                                CommandName="Delete" Text="删除" OnClientClick="return confirm('是否删除该记录？');"></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="id" InsertVisible="False" SortExpression="id">
                    <EditItemTemplate>
                        <asp:Label ID="Label1" runat="server" Text='<%# Eval("id") %>'></asp:Label>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label2" runat="server" Text='<%# Bind("id") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="教室" SortExpression="strRoom">
                    <EditItemTemplate>
                        <asp:DropDownList ID="DropDownList1" runat="server" DataSourceID="SqlDataSourceRoom" DataTextField="strRoomName" DataValueField="strRoomName" SelectedValue='<%# Bind("strRoom") %>'>
                        </asp:DropDownList>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label1" runat="server" Text='<%# Bind("strRoom") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="intDay" HeaderText="日期" SortExpression="intDay" />              

                <asp:BoundField DataField="intStartNum" HeaderText="开始节次" SortExpression="intStartNum" />
                <asp:BoundField DataField="intEndNum" HeaderText="结束节次" SortExpression="intEndNum" />
                <asp:TemplateField HeaderText="开始周" SortExpression="intStartWeek">
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddlStartWeek" runat="server" DataSourceID="SqlDataSourceWeek" DataTextField="id" DataValueField="id" SelectedValue='<%# Bind("intStartWeek") %>'>
                        </asp:DropDownList>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lbStartWeek" runat="server" Text='<%# Bind("intStartWeek") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>           
                <asp:TemplateField HeaderText="结束周" SortExpression="intEndWeek">
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddlEndWeek" runat="server" DataSourceID="SqlDataSourceWeek" DataTextField="id" DataValueField="id" SelectedValue='<%# Bind("intEndWeek") %>'>
                        </asp:DropDownList>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lbEndWeek" runat="server" Text='<%# Bind("intEndWeek") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>   
                <asp:BoundField DataField="strName" HeaderText="课程名称" SortExpression="strName" />
                <asp:BoundField DataField="strClass" HeaderText="班级" SortExpression="strClass" />
                <asp:BoundField DataField="strTeacher" HeaderText="教师" SortExpression="strTeacher" />
            </Columns>
        </asp:GridView>        

        

        

        

        <asp:HyperLink ID="hlNew" runat="server" NavigateUrl="~/ApplyAdd.aspx">新增</asp:HyperLink>
        <asp:HyperLink ID="hlQuery" runat="server" NavigateUrl="~/RoomApply.aspx">查询</asp:HyperLink>
        

        

        

    </form>
</body>
</html>
