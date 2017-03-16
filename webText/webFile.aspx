<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="webFile.aspx.cs" Inherits="webText_webFile" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <div>
        &nbsp;<asp:FileUpload ID="FileUpload1" runat="server" />
        <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="upload" />
        <br />
        <asp:Label ID="uploadText" runat="server" Text="uploadText"></asp:Label>
        <asp:GridView ID="GridView1" runat="server" AllowSorting="True" AutoGenerateColumns="False" >
            <Columns>
                <asp:BoundField DataField="filename" HeaderText="FILENAME" />
            </Columns>
        </asp:GridView>
        <asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl="ftp://192.168.0.11/webFile">ftp</asp:HyperLink>
        &nbsp;
    </div>
    <div>
        <asp:Button ID="Button2" runat="server" Text="刷新列表" OnClick="Button2_Click" />&nbsp;
        </div>

</asp:Content>
