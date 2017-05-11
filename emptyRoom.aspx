<%@ Page Language="C#" AutoEventWireup="true" CodeFile="emptyRoom.aspx.cs" Inherits="emptyRoom" %>

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
    <asp:DropDownList ID="ddlDep" runat="server" DataSourceID="sqsDep" DataTextField="strDepart" DataValueField="depid">
        </asp:DropDownList>
    <asp:DropDownList ID="ddlWeek" runat="server" DataSourceID="sdsWeek" DataTextField="detail" DataValueField="intWeek">
        </asp:DropDownList>
        <asp:Label ID="Label1" runat="server" Text="至"></asp:Label>
        <asp:DropDownList ID="ddlWeek2" runat="server" DataSourceID="sdsWeek" DataTextField="detail" DataValueField="intWeek" >
        </asp:DropDownList>
        <asp:Label ID="Label5" runat="server" Text="星期"></asp:Label>
        <asp:DropDownList ID="ddlDay" runat="server">
            <asp:ListItem>1</asp:ListItem>
            <asp:ListItem>2</asp:ListItem>
            <asp:ListItem>3</asp:ListItem>
            <asp:ListItem>4</asp:ListItem>
            <asp:ListItem>5</asp:ListItem>
            <asp:ListItem>6</asp:ListItem>
            <asp:ListItem>7</asp:ListItem>
        </asp:DropDownList>
        <asp:Label ID="Label2" runat="server" Text="第"></asp:Label>
        <asp:DropDownList ID="ddlNum1" runat="server" OnSelectedIndexChanged="ddlNum1_SelectedIndexChanged" AutoPostBack="True">
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
            <asp:ListItem Value="11">中午</asp:ListItem>
        </asp:DropDownList>
        <asp:Label ID="Label3" runat="server" Text="节至第"></asp:Label>
        <asp:DropDownList ID="ddlNum2" runat="server" OnSelectedIndexChanged="ddlNum2_SelectedIndexChanged" AutoPostBack="True">
            <asp:ListItem>1</asp:ListItem>
            <asp:ListItem>2</asp:ListItem>
            <asp:ListItem>3</asp:ListItem>
            <asp:ListItem>4</asp:ListItem>
            <asp:ListItem>5</asp:ListItem>
            <asp:ListItem>6</asp:ListItem>
            <asp:ListItem>7</asp:ListItem>
            <asp:ListItem>8</asp:ListItem>
            <asp:ListItem>9</asp:ListItem>
            <asp:ListItem Selected="True">10</asp:ListItem>
            <asp:ListItem Value="11">中午</asp:ListItem>
        </asp:DropDownList>
        <asp:Label ID="Label4" runat="server" Text="节"></asp:Label>
        <asp:Button ID="btSearch" runat="server" Text="Search" OnClick="btSearch_Click" />
    </div>
    <div class="centertop" id="tableHeader" ></div>
        <div class="center"   >
            <table id="table" border="1">
                <thead>
                <tr id="thead" >                    
                    <th>
                        教室
                    </th>
                    <th>
                        第几周
                    </th>
                    <%--<th>
                        节次
                    </th>  --%>                  
                </tr>
                </thead>
                <tbody>
                    <asp:Repeater ID="Repeater1" runat="server">
                        <ItemTemplate>
                            <tr>                                
                                <td id="td1" runat="server" >
                                    <%#DataBinder.Eval(Container.DataItem, "strRoomName")%>
                                </td>
                                <td id="td2" runat="server">
                                    <%#DataBinder.Eval(Container.DataItem, "intWeek")%>
                                </td>                                
                                <%--<td id="td3" runat="server">
                                    <%#DataBinder.Eval(Container.DataItem, "num")%>
                                </td> --%>                               
                            </tr>
                        </ItemTemplate>
                     </asp:Repeater>
                 </tbody>
            </table>       
        </div>    
    <asp:SqlDataSource ID="sqsDep" runat="server" ConnectionString="<%$ ConnectionStrings:webTestConnectionString %>" SelectCommand="SELECT distinct depid,strDepart FROM Department order by 1"></asp:SqlDataSource>
    <asp:SqlDataSource ID="sdsWeek" runat="server" ConnectionString="<%$ ConnectionStrings:webTestConnectionString %>" SelectCommand="SELECT [intWeek],'第'+CAST(intWeek as varchar(10)) + '周 ' + datePeriod  as detail FROM [WeekStartEnd] w inner join TitleStartEnd t on w.yearID = t.yearID and t.currentFlag = 'true'"></asp:SqlDataSource>
    </form>
</body>
</html>
