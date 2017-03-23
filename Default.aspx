<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" Title="yeswecan" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div>
        <asp:LoginView ID="LoginView1" runat="server" Visible="False">
            <LoggedInTemplate>
                logged in&nbsp;
                <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/admin.aspx">admin</asp:HyperLink>
                &nbsp;
            </LoggedInTemplate>
            <AnonymousTemplate>
                not logged in
            </AnonymousTemplate>
        </asp:LoginView>
    
    </div>
        <asp:LoginStatus ID="LoginStatus1" runat="server" Visible="False" />
        <br />
        <br />
        <br />
        <br />
        <br />
        <asp:Menu ID="Menu1" runat="server">
            <Items>
                <asp:MenuItem NavigateUrl="~/news_more.aspx" Text="新建项1" Value="新建项1"></asp:MenuItem>
                <asp:MenuItem Text="新建项2" Value="新建项2" NavigateUrl="~/video.aspx"></asp:MenuItem>
            </Items>
        </asp:Menu>
        <br />
</asp:Content>

