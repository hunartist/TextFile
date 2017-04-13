<%@ Page Language="C#" AutoEventWireup="true" CodeFile="roomQueryReverse.aspx.cs" Inherits="roomQueryReverse" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <link rel="stylesheet" type="text/css" href="css/StyleSheet.css" />
    
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
<%--<script src="Scripts/jquery-3.1.1.min.js" type="text/javascript"></script>
    <script src="Scripts/ScrollableGridPlugin.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#<%=gvTest.ClientID %>').Scrollable({
                ScrollHeight: 700,
                Width: 1425
            });
        });
    </script>--%>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div id ="noleft">
        <asp:DropDownList ID="ddlDep" runat="server" DataSourceID="sqsDep" DataTextField="strDepart" DataValueField="strDepart">
        </asp:DropDownList>
        <asp:DropDownList ID="ddlWeek" runat="server" DataSourceID="sdsWeek" DataTextField="detail" DataValueField="intWeek">
        </asp:DropDownList>
        <asp:Button ID="btSearch" runat="server" Text="查询" OnClick="btSearch_Click" />
        <asp:SqlDataSource ID="sdsRoom" runat="server" ConnectionString="<%$ ConnectionStrings:webTestConnectionString %>" SelectCommand="SELECT [strRoomName] FROM [RoomDetail] WHERE ([strDepart] = @strDepart)">
            <SelectParameters>
                <asp:ControlParameter ControlID="ddlDep" Name="strDepart" PropertyName="SelectedValue" Type="String" />
            </SelectParameters>
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="sdsWeek" runat="server" ConnectionString="<%$ ConnectionStrings:webTestConnectionString %>" SelectCommand="SELECT [intWeek],'第'+CAST(intWeek as varchar(10)) + '周 ' + datePeriod  as detail FROM [WeekStartEnd] w inner join TitleStartEnd t on w.yearID = t.yearID and t.currentFlag = 'true'"></asp:SqlDataSource>
        <asp:SqlDataSource ID="sqsDep" runat="server" ConnectionString="<%$ ConnectionStrings:webTestConnectionString %>" SelectCommand="SELECT distinct RTRIM(strDepart) strDepart FROM [RoomDetail] order by 1 desc"></asp:SqlDataSource>
        </div>
        <div class="centertop" id="tableHeader" ></div>
        <div class="center"   >
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
