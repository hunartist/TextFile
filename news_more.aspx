<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="news_more.aspx.cs" Inherits="news_more" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <div>
        say news more
        <br />
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:webTestConnectionString %>"
            SelectCommand="SELECT [n_title] FROM [news]"></asp:SqlDataSource>
    </div>
        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataSourceID="SqlDataSource1">
            <Columns>
                <asp:BoundField DataField="n_title" HeaderText="n_title" SortExpression="n_title" />
            </Columns>
        </asp:GridView>

</asp:Content>
