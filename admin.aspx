﻿<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="admin.aspx.cs" Inherits="admin" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <div>
        admin<br />
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:webTestConnectionString %>"
            SelectCommand="SELECT * FROM [news]" OldValuesParameterFormatString="original_{0}" ConflictDetection="CompareAllValues" DeleteCommand="DELETE FROM [news] WHERE [n_id] = @original_n_id AND [n_title] = @original_n_title" InsertCommand="INSERT INTO [news] ([n_id], [n_title]) VALUES (@n_id, @n_title)" UpdateCommand="UPDATE [news] SET [n_title] = @n_title WHERE [n_id] = @original_n_id AND [n_title] = @original_n_title">
            <DeleteParameters>
                <asp:Parameter Name="original_n_id" Type="String" />
                <asp:Parameter Name="original_n_title" Type="String" />
            </DeleteParameters>
            <UpdateParameters>
                <asp:Parameter Name="n_title" Type="String" />
                <asp:Parameter Name="original_n_id" Type="String" />
                <asp:Parameter Name="original_n_title" Type="String" />
            </UpdateParameters>
            <InsertParameters>
                <asp:Parameter Name="n_id" Type="String" />
                <asp:Parameter Name="n_title" Type="String" />
            </InsertParameters>
        </asp:SqlDataSource>
        
    </div>
        <asp:GridView ID="GridView1" runat="server" AllowSorting="True" AutoGenerateColumns="False"
            DataKeyNames="n_id" DataSourceID="SqlDataSource1" OnSelectedIndexChanged="GridView1_SelectedIndexChanged" AutoGenerateDeleteButton="True" AutoGenerateEditButton="True" AutoGenerateSelectButton="True">
            <Columns>
                <asp:BoundField DataField="n_id" HeaderText="n_id" ReadOnly="True" SortExpression="n_id" />
                <asp:BoundField DataField="n_title" HeaderText="n_title" SortExpression="n_title" />
            </Columns>
        </asp:GridView>
        <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>&nbsp;&nbsp;&nbsp;&nbsp;
    &nbsp;
        <asp:SqlDataSource ID="SqlDataSourceDetail" runat="server" ConnectionString="<%$ ConnectionStrings:webTestConnectionString %>"
            SelectCommand="SELECT * FROM [news] WHERE ([n_id] = @n_id)" ConflictDetection="CompareAllValues" DeleteCommand="DELETE FROM [news] WHERE [n_id] = @original_n_id AND [n_title] = @original_n_title" InsertCommand="INSERT INTO [news] ([n_id], [n_title]) VALUES (@n_id, @n_title)" OldValuesParameterFormatString="original_{0}" UpdateCommand="UPDATE [news] SET [n_title] = @n_title WHERE [n_id] = @original_n_id AND [n_title] = @original_n_title">
            <SelectParameters>
                <asp:ControlParameter ControlID="GridView1" DefaultValue="0" Name="n_id" PropertyName="SelectedValue"
                    Type="String" />
            </SelectParameters>
            <DeleteParameters>
                <asp:Parameter Name="original_n_id" Type="String" />
                <asp:Parameter Name="original_n_title" Type="String" />
            </DeleteParameters>
            <UpdateParameters>
                <asp:Parameter Name="n_title" Type="String" />
                <asp:Parameter Name="original_n_id" Type="String" />
                <asp:Parameter Name="original_n_title" Type="String" />
            </UpdateParameters>
            <InsertParameters>
                <asp:Parameter Name="n_id" Type="String" />
                <asp:Parameter Name="n_title" Type="String" />
            </InsertParameters>
        </asp:SqlDataSource>
        <asp:DetailsView ID="DetailsView1" runat="server" DataKeyNames="n_id"
            DataSourceID="SqlDataSourceDetail" Height="50px" Width="125px" HorizontalAlign="Left" AutoGenerateDeleteButton="True" AutoGenerateEditButton="True" AutoGenerateInsertButton="True">
        </asp:DetailsView>


</asp:Content>
