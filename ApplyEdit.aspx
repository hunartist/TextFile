<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ApplyEdit.aspx.cs" Inherits="NextWebF" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>edit</title>
    <link rel="stylesheet" type="text/css" href="css/StyleSheet.css" />
<%--    <script src="Scripts/jquery-3.1.1.js" type="text/javascript"></script>  
    <script type="text/javascript">   
        $(function() {   
        $("#TextBox1").keyup(function() {   
        var filterText = $(this).val();   
            $("#<%=GridView10.ClientID %> tr").not(":first").hide().filter(":contains('" + filterText + "')").show();;   
        }).keyup();   
        });   
    </script>   --%>
</head>
<body>
    <script language="C#" runat="server">
    </script>
    <form id="form1" method="post" runat="server" enableviewstate="True">
    
        <asp:SqlDataSource ID="SqlDataSourceRoomApply" runat="server" ConnectionString="<%$ ConnectionStrings:webTestConnectionString %>" SelectCommand="select aa.*,t.currentFlag from (select distinct a.id,a.strRoom,a.intDay,a.intStartNum,a.intEndNum,a.intStartWeek,a.intEndWeek,RTRIM(a.strName) as strName,RTRIM(a.strClass) as strClass,RTRIM(a.strTeacher) as strTeacher,a.yearID from RoomApply a ,RoomDetail d where a.strRoom = d.strRoomName and d.strDepart = @depN_CP ) as aa inner join TitleStartEnd t on aa.yearID = t.yearID and t.currentFlag = 'true'  order by aa.id desc"
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
                <asp:ControlParameter ControlID ="DropDownListDepart" Name ="depN_CP" PropertyName ="SelectedValue" Type ="String" />
            </SelectParameters>
        </asp:SqlDataSource>       
        <asp:SqlDataSource ID="SqlDataSourceDepartment" runat="server" ConnectionString="<%$ ConnectionStrings:webTestConnectionString %>" SelectCommand="SELECT distinct [strDepart] FROM [RoomDetail] WHERE ([strCDep] = @strCDep) order by [strDepart] desc" >
            <SelectParameters>
                <asp:SessionParameter Name="strCDep" SessionField="dep" Type="String" />
            </SelectParameters>
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="SqlDataSourceRoom" runat="server" ConnectionString="<%$ ConnectionStrings:webTestConnectionString %>" SelectCommand="SELECT [strRoomName] FROM [RoomDetail] WHERE ([strDepart] = @strDepart)">
            <SelectParameters>
                <asp:ControlParameter ControlID="DropDownListDepart" Name="strDepart" PropertyName="SelectedValue" Type="String" />
            </SelectParameters>
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="SqlDataSourceWeek" runat="server" ConnectionString="<%$ ConnectionStrings:webTestConnectionString %>" SelectCommand="SELECT [intWeek] FROM [WeekStartEnd] w inner join TitleStartEnd t on w.yearID = t.yearID and t.currentFlag = 'true'"></asp:SqlDataSource>
        <div class="left">            
            <asp:DropDownList ID="DropDownListDepart" runat="server" AutoPostBack="False" DataSourceID="SqlDataSourceDepartment" DataTextField="strDepart" DataValueField="strDepart"></asp:DropDownList>
            <asp:Button ID="btDepFlit" runat="server" Text="部门筛选" OnClick="btDepFlit_Click" />  
            <br />      
            <asp:TextBox ID="tbSearch" runat="server"></asp:TextBox>
            <asp:Button ID="btSearch" runat="server" Text="搜索（名称、教师、班级）" OnClick="btSearch_Click" />
            <br />
            <asp:DropDownList ID="ddlWeek" runat="server" DataSourceID="SqlDataSourceWeek" DataTextField="intWeek" DataValueField="intWeek"></asp:DropDownList>
            <asp:Button ID="btWeekFlit" runat="server" Text="周筛选" OnClick="btWeekFlit_Click"/>
            <br />  
            <asp:ListBox ID="liboRoom" runat="server" DataSourceID="SqlDataSourceRoom" DataTextField="strRoomName" DataValueField="strRoomName" Rows="30" SelectionMode="Multiple"></asp:ListBox>
            <br /> 
            <asp:Button ID="btRoomAll" runat="server" Text="教室全选" OnClick="btRoomAll_Click" />
            <br /> 
            <asp:Button ID="btRoomNone" runat="server" Text="教室选择清除" OnClick="btRoomNone_Click" />
            <br /> 
            <asp:Button ID="btRoomFlit" runat="server" Text="教室筛选（所有周）" OnClick="btRoomFlit_Click" />
            <%--<asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>--%>
            <div>            
                <asp:HyperLink ID="hlChangePW" runat="server" NavigateUrl="~/changePW.aspx">修改密码</asp:HyperLink>
                <asp:Button ID="btAbandon" runat="server" Text="注销" OnClick="btAbandon_Click" />
            </div>        
            <asp:Label ID="LabelID" runat="server" Text="LabelID"></asp:Label>
            <asp:Label ID="LabelMsg" runat="server" Text="LabelMsg" ForeColor="Red"></asp:Label>

            <asp:HyperLink ID="hlNew" runat="server" NavigateUrl="~/ApplyAdd.aspx">新增</asp:HyperLink>
            <asp:HyperLink ID="hlQuery" runat="server" NavigateUrl="~/RoomApply.aspx" target="_blank">查询</asp:HyperLink>
        </div>
        <div class="righttop">
        <asp:GridView ID="GridView10" runat="server" AutoGenerateColumns="False" DataSourceID="SqlDataSourceRoomApply" AllowSorting="True" HorizontalAlign="Left" AutoGenerateEditButton="True" DataKeyNames="id" OnRowUpdated="GridView10_RowUpdated" OnSelectedIndexChanging="GridView10_SelectedIndexChanging" OnRowEditing="GridView10_RowEditing" OnRowUpdating="GridView10_RowUpdating" OnRowCancelingEdit="GridView10_RowCancelingEdit" OnRowDeleted="GridView10_RowDeleted" PageSize="14">
            <Columns>
                <%--<asp:BoundField DataField="intStartWeek" HeaderText="开始周" SortExpression="intStartWeek" />--%>     
                <%--<asp:BoundField DataField="intEndWeek" HeaderText="结束周" SortExpression="intEndWeek" />--%>
                <asp:TemplateField ShowHeader="False">
                    <ItemTemplate>
                        <asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False"
                                CommandName="Delete" Text="删除" OnClientClick="return confirm('是否删除该记录？');"></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <%--<asp:TemplateField HeaderText="id" InsertVisible="False" SortExpression="id">
                    <EditItemTemplate>
                        <asp:Label ID="Label1" runat="server" Text='<%# Eval("id") %>'></asp:Label>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label2" runat="server" Text='<%# Bind("id") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>--%>

                <asp:TemplateField HeaderText="教室" SortExpression="strRoom">
                    <EditItemTemplate>
                        <asp:DropDownList ID="DropDownList1" runat="server" DataSourceID="SqlDataSourceRoom" DataTextField="strRoomName" DataValueField="strRoomName" SelectedValue='<%# Bind("strRoom") %>'>
                        </asp:DropDownList>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label1" runat="server" Text='<%# Bind("strRoom") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="intDay" HeaderText="日期（星期几）" SortExpression="intDay" />              

                <asp:BoundField DataField="intStartNum" HeaderText="开始节次" SortExpression="intStartNum" />
                <asp:BoundField DataField="intEndNum" HeaderText="结束节次" SortExpression="intEndNum" />
                <asp:TemplateField HeaderText="开始周" SortExpression="intStartWeek">
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddlStartWeek" runat="server" DataSourceID="SqlDataSourceWeek" DataTextField="intWeek" DataValueField="intWeek" SelectedValue='<%# Bind("intStartWeek") %>'>
                        </asp:DropDownList>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lbStartWeek" runat="server" Text='<%# Bind("intStartWeek") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>           
                <asp:TemplateField HeaderText="结束周" SortExpression="intEndWeek">
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddlEndWeek" runat="server" DataSourceID="SqlDataSourceWeek" DataTextField="intWeek" DataValueField="intWeek" SelectedValue='<%# Bind("intEndWeek") %>'>
                        </asp:DropDownList>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lbEndWeek" runat="server" Text='<%# Bind("intEndWeek") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>   
                <asp:BoundField DataField="strName" HeaderText="课程名称" SortExpression="strName" />
                <asp:BoundField DataField="strClass" HeaderText="班级" SortExpression="strClass" />
                <asp:BoundField DataField="strTeacher" HeaderText="教师" SortExpression="strTeacher" />
                <asp:BoundField DataField="yearID" HeaderText="学期" SortExpression="yearID" ReadOnly="True" />
            </Columns>
            <PagerSettings FirstPageText="首页" LastPageText="末页" Mode="NumericFirstLast" NextPageText="下一页" PreviousPageText="上一页" />
        </asp:GridView>   
        </div>
    </form>
</body>
</html>
