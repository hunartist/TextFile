﻿<%@ Page validateRequest="false" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="webText.aspx.cs" Inherits="webText_webText" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div style="text-align:center;margin:0 auto">
        <asp:TextBox ID="TextBox1" runat="server" TextMode="MultiLine" Width="100%" Height="300px"></asp:TextBox>
    </div>
        
    <br />        
    <asp:Label ID="Label1" runat="server" Text="insert"></asp:Label>
    <asp:Label ID="Label2" runat="server" Text="new"></asp:Label>
    <br />
    <asp:Button ID="Button2" runat="server" OnClick="Button2_Click" Text="new" />
    <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="save" />    
    <asp:SqlDataSource ID="SqlDataSource1" runat="server"
        ConnectionString="<%$ ConnectionStrings:webTestConnectionString %>" DeleteCommand="DELETE FROM [record] WHERE [id] = @original_id"
        InsertCommand="INSERT INTO [record] ([vc_text], [dt_date]) VALUES (@vc_text, getdate())"
        OldValuesParameterFormatString="original_{0}" SelectCommand="SELECT * FROM [record] order by [dt_date] desc"
        UpdateCommand="UPDATE [record] SET [vc_text] = @vc_text, [dt_date] = getdate() WHERE [id] = @original_id">
        <DeleteParameters>
            <asp:Parameter Name="original_id" Type="Int32" />
        </DeleteParameters>
        <UpdateParameters>
            <asp:Parameter Name="vc_text" Type="String" />
            <asp:Parameter DbType="Datetime" Name="@dt_date" />
            <asp:Parameter Name="original_id" Type="Int32" />
        </UpdateParameters>
        <InsertParameters>
            <asp:Parameter Name="vc_text" Type="String" />
            <asp:Parameter DbType="Datetime" Name="dt_date" />
        </InsertParameters>
    </asp:SqlDataSource>
    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataKeyNames="id"
        DataSourceID="SqlDataSource1" OnSelectedIndexChanging="GridView1_SelectedIndexChanging" OnRowDeleted="GridView1_RowDeleted" AutoGenerateEditButton="false">
        <Columns>
            <asp:TemplateField ShowHeader="False">
                <ItemTemplate>
                    <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Select"
                        Text="选择"></asp:LinkButton><asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False"
                            CommandName="Delete" Text="删除" OnClientClick="return confirm('是否删除该记录？');"></asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="id" HeaderText="id" InsertVisible="False" ReadOnly="True"
                SortExpression="id" />
            <asp:BoundField DataField="vc_text" HeaderText="vc_text" SortExpression="vc_text" />
            <asp:BoundField DataField="dt_date" HeaderText="dt_date" SortExpression="dt_date" />
        </Columns>
    </asp:GridView>    
    <br />

</asp:Content>
