<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RoomDetail.aspx.cs" Inherits="RoomDetail" %>

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
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <%--<asp:SqlDataSource ID="sdsRoomDetail" runat="server" ConnectionString="<%$ ConnectionStrings:webTestConnectionString %>" SelectCommand="SELECT * FROM [RoomDetail]">--%>
        <asp:SqlDataSource ID="sdsRoomDetail" runat="server" ConnectionString="<%$ ConnectionStrings:webTestConnectionString %>" SelectCommand="SELECT strRoomName,strCDep,strDepart,strCompConf,strRoomCourse,strCourseObject FROM [RoomDetail] WHERE ([strRoomName] = @strRoomName)">
            <SelectParameters>
                <asp:QueryStringParameter Name="strRoomName" QueryStringField="qsRoomName" Type="String" />
            </SelectParameters>
        </asp:SqlDataSource>
        <div class="centertop" id="tableHeader" ></div>
        <div class="center"   >
            <table id="table" border="1">
                <thead>
                <tr id="thead" >
                    <th>
                        名称
                    </th>
                    <th>
                        所属学院
                    </th>
                    <th>
                        二级部门
                    </th>
                    <th>
                        电脑配置
                    </th>
                    <th>
                        可开设课程
                    </th>
                    <th>
                        可开设实验项目
                    </th>                    
                </tr>
            </thead>
            <tbody>
                <asp:Repeater ID="rpRoomDetail" runat="server" DataSourceID="sdsRoomDetail">
                    <ItemTemplate>
                                <tr>                                    
                                    <td id="td1" runat="server">
                                        <%#DataBinder.Eval(Container.DataItem, "strRoomName")%>
                                    </td>
                                    <td id="td2" runat="server">
                                        <%#DataBinder.Eval(Container.DataItem, "strCDep")%>
                                    </td>
                                    <td id="td3" runat="server">
                                        <%#DataBinder.Eval(Container.DataItem, "strDepart")%>
                                    </td>
                                    <td id="td4" runat="server">
                                        <%#DataBinder.Eval(Container.DataItem, "strCompConf")%>
                                    </td>
                                    <td id="td5" runat="server">
                                        <%#DataBinder.Eval(Container.DataItem, "strRoomCourse")%>
                                    </td>
                                    <td id="td6" runat="server">
                                        <%#DataBinder.Eval(Container.DataItem, "strCourseObject")%>
                                    </td>                            
                                </tr>
                            </ItemTemplate>
                </asp:Repeater>
            </tbody>
        </table>    
    </div>
    </div>
    </form>
</body>
</html>
