<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ipmsg.aspx.cs" Inherits="ipmsg" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<link rel="stylesheet" type="text/css" href="css/StyleSheet.css" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div id ="seatmsg">

    </div>
    <div>
    <script type="text/javascript">
	function getQueryString(name) {
		var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
		var r = document.location.search.substr(1).match(reg);
		if (r != null) return unescape(r[2]); return null;
	}
	if(getQueryString("seatnum") != null) {
	    //document.getElementById("seatmsg").style.display = "none";
	    document.getElementById("seatmsg").innerText = getQueryString("seatnum");//ie8 不支持textContent
	    document.getElementById("seatmsg").textContent = getQueryString("seatnum");
	}
	else {
	    document.getElementById("seatmsg").innerText = "nulpar";
	    document.getElementById("seatmsg").textContent = "nulpar";
	}
	</script>
    </div>
        <asp:SqlDataSource ID="SqlDataSourceIPSeat" runat="server" ConnectionString="<%$ ConnectionStrings:webTestConnectionString %>" SelectCommand="SELECT * FROM [ipseat] WHERE ([ip] = @ip)">
            <SelectParameters>
                <asp:QueryStringParameter Name="ip" QueryStringField="ipadd" Type="String" />
            </SelectParameters>
        </asp:SqlDataSource>
        <asp:DetailsView ID="DetailsViewIPSeat" runat="server" AutoGenerateRows="False" DataKeyNames="id" DataSourceID="SqlDataSourceIPSeat" EnableModelValidation="True" Height="50px" Width="125px" BorderStyle="None" EmptyDataText="not found" GridLines="None" HorizontalAlign="Center">
            <Fields>
                <asp:BoundField DataField="id" HeaderText="id" InsertVisible="False" ReadOnly="True" SortExpression="id" Visible="False" />
                <asp:BoundField DataField="ip" HeaderText="ip" SortExpression="ip" Visible="False" />
                <asp:BoundField DataField="seatnum" HeaderText="seatnum" SortExpression="seatnum" ShowHeader="False" />
            </Fields>
        </asp:DetailsView>
    </form>
    </body>
</html>
