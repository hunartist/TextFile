<%@ Page Language="C#" AutoEventWireup="true" CodeFile="roomQuery.aspx.cs" Inherits="roomQuery" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <link rel="stylesheet" type="text/css" href="css/StyleSheet.css" />
    <title></title>
    <script type="text/javascript" >
        function FixTableHeader() {
            var t = document.getElementById("table");
            var thead = t.getElementsByTagName("thead")[0];
            var t1 = t.cloneNode(false);
            t1.appendChild(thead);
            document.getElementById("tableHeader").appendChild(t1)
        }
        window.onload = FixTableHeader;
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="left">
        <asp:DropDownList ID="ddlDep" runat="server" DataSourceID="sqsDep" DataTextField="strDepart" DataValueField="strDepart">
        </asp:DropDownList>
        <asp:DropDownList ID="ddlWeek" runat="server" DataSourceID="sdsWeek" DataTextField="detail" DataValueField="id">
        </asp:DropDownList>
        <asp:Button ID="btSearch" runat="server" Text="查询" OnClick="btSearch_Click" />
        <asp:SqlDataSource ID="sdsRoom" runat="server" ConnectionString="<%$ ConnectionStrings:webTestConnectionString %>" SelectCommand="SELECT [strRoomName] FROM [RoomDetail] WHERE ([strDepart] = @strDepart)">
            <SelectParameters>
                <asp:ControlParameter ControlID="ddlDep" Name="strDepart" PropertyName="SelectedValue" Type="String" />
            </SelectParameters>
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="sdsWeek" runat="server" ConnectionString="<%$ ConnectionStrings:webTestConnectionString %>" SelectCommand="SELECT [id],'第'+CAST(id as varchar(10)) + '周 ' + datePeriod  as detail FROM [WeekStartEnd]"></asp:SqlDataSource>
        <asp:SqlDataSource ID="sqsDep" runat="server" ConnectionString="<%$ ConnectionStrings:webTestConnectionString %>" SelectCommand="SELECT distinct RTRIM(strDepart) strDepart FROM [RoomDetail] order by 1 desc"></asp:SqlDataSource>
            <asp:ListView ID="ListView1" runat="server" DataSourceID="sdsRoom" EnableModelValidation="True">
                <AlternatingItemTemplate>
                    <tr style="">
                        <td>
                            <asp:Label ID="strRoomNameLabel" runat="server" Text='<%# Eval("strRoomName") %>' />
                        </td>
                    </tr>
                </AlternatingItemTemplate>
                <EditItemTemplate>
                    <tr style="">
                        <td>
                            <asp:Button ID="UpdateButton" runat="server" CommandName="Update" Text="更新" />
                            <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" Text="取消" />
                        </td>
                        <td>
                            <asp:TextBox ID="strRoomNameTextBox" runat="server" Text='<%# Bind("strRoomName") %>' />
                        </td>
                    </tr>
                </EditItemTemplate>
                <EmptyDataTemplate>
                    <table runat="server" style="">
                        <tr>
                            <td>未返回数据。</td>
                        </tr>
                    </table>
                </EmptyDataTemplate>
                <InsertItemTemplate>
                    <tr style="">
                        <td>
                            <asp:Button ID="InsertButton" runat="server" CommandName="Insert" Text="插入" />
                            <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" Text="清除" />
                        </td>
                        <td>
                            <asp:TextBox ID="strRoomNameTextBox" runat="server" Text='<%# Bind("strRoomName") %>' />
                        </td>
                    </tr>
                </InsertItemTemplate>
                <ItemTemplate>
                    <tr style="">
                        <td>
                            <asp:Label ID="strRoomNameLabel" runat="server" Text='<%# Eval("strRoomName") %>' />
                        </td>
                    </tr>
                </ItemTemplate>
                <LayoutTemplate>
                    <table runat="server">
                        <tr runat="server">
                            <td runat="server">
                                <table id="itemPlaceholderContainer" runat="server" border="0" style="">
                                    <tr runat="server" style="">
                                        <th runat="server">教室列表</th>
                                    </tr>
                                    <tr id="itemPlaceholder" runat="server">
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr runat="server">
                            <td runat="server" style=""></td>
                        </tr>
                    </table>
                </LayoutTemplate>
                <SelectedItemTemplate>
                    <tr style="">
                        <td>
                            <asp:Label ID="strRoomNameLabel" runat="server" Text='<%# Eval("strRoomName") %>' />
                        </td>
                    </tr>
                </SelectedItemTemplate>
            </asp:ListView>        
        </div>
        <div class="righttop" id="tableHeader" ></div>
        <div class="right"   >
            <table id="table" border="1">
                <thead>
                <tr id="thead" >
                    <th>
                        查询
                    </th>
                    <th>
                        星期一
                    </th>
                    <th>
                        星期二
                    </th>
                    <th>
                        星期三
                    </th>
                    <th>
                        星期四
                    </th>
                    <th>
                        星期五
                    </th>
                    <th>
                        星期六
                    </th>
                    <th>
                        星期日
                    </th>
                </tr>
            </thead>
            <tbody>
                <asp:Repeater ID="Repeater1" runat="server">
                    <ItemTemplate>
                        <tr>
                            <td id="td0" runat="server">
                                <%#DataBinder.Eval(Container.DataItem, "查询")%>
                            </td>
                            <td id="td1" runat="server">
                                <%#DataBinder.Eval(Container.DataItem, "星期一")%>
                            </td>
                            <td id="td2" runat="server">
                                <%#DataBinder.Eval(Container.DataItem, "星期二")%>
                            </td>
                            <td id="td3" runat="server">
                                <%#DataBinder.Eval(Container.DataItem, "星期三")%>
                            </td>
                            <td id="td4" runat="server">
                                <%#DataBinder.Eval(Container.DataItem, "星期四")%>
                            </td>
                            <td id="td5" runat="server">
                                <%#DataBinder.Eval(Container.DataItem, "星期五")%>
                            </td>
                            <td id="td6" runat="server">
                                <%#DataBinder.Eval(Container.DataItem, "星期六")%>
                            </td>
                            <td id="td7" runat="server">
                                <%#DataBinder.Eval(Container.DataItem, "星期日")%>
                            </td>
                        </tr>
                    </ItemTemplate>
                 </asp:Repeater>
             </tbody>
        </table>       
        </div>
    </form>
</body>
</html>
