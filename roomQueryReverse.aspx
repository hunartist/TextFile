<%@ Page Language="C#" AutoEventWireup="true" CodeFile="roomQueryReverse.aspx.cs" Inherits="roomQueryReverse" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <link rel="stylesheet" type="text/css" href="StyleSheet.css" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div id ="left">
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
        <asp:SqlDataSource ID="sqsDep" runat="server" ConnectionString="<%$ ConnectionStrings:webTestConnectionString %>" SelectCommand="SELECT distinct [strDepart] FROM [RoomDetail]"></asp:SqlDataSource>
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
        <div id="right">
        <asp:placeholder id="GridViewPlaceHolder" runat="Server"/>        
            
        </div>    
        
    </form>
</body>
</html>
