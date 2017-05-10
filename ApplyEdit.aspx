<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ApplyEdit.aspx.cs" Inherits="NextWebF" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>edit</title>
    <script type="text/javascript">
    function expandcollapse(obj, row) {
        var div = document.getElementById(obj);
        var img = document.getElementById('img' + obj);

        if (div.style.display == "none") {
            div.style.display = "block";
            if (row == 'alt') {
                img.src = "img/minus.gif";
            }
            else {
                img.src = "img/minus.gif";
            }
            img.alt = "Close";
        }
        else {
            div.style.display = "none";
            if (row == 'alt') {
                img.src = "img/plus.gif";
            }
            else {
                img.src = "img/plus.gif";
            }
            img.alt = "Expand";
        }
    }
    </script>
    <link rel="stylesheet" type="text/css" href="css/StyleSheet.css" />
</head>
<body>
    <script language="C#" runat="server">
    </script>
    <form id="form1" method="post" runat="server" enableviewstate="True">
        <asp:SqlDataSource ID="sqsApplyList" runat="server" ConnectionString='<%$ ConnectionStrings:webTestConnectionString %>' 
            SelectCommand="SELECT [applyid],RTRIM([strName]) as strName,[yearID],[cdepid],[strRemark] FROM [ApplyList] WHERE [cdepid] = @cdepid  order by applyid desc " 
            UpdateCommandType ="StoredProcedure" UpdateCommand="ApplyListAction" 
            DeleteCommandType ="StoredProcedure" DeleteCommand="ApplyListAction"
            InsertCommandType ="StoredProcedure" InsertCommand="ApplyListAction">
            <SelectParameters>
                <asp:SessionParameter Name="cdepid" SessionField="cdep" Type="String" />
            </SelectParameters>
            <UpdateParameters>
                <asp:Parameter Name="Action" Type="String" />                
                <asp:Parameter Name="strName" Type="String" />
                <asp:Parameter Name="strYearID" Type="String" />
                <asp:Parameter Name="cdepid" Type="String" />  
                <asp:Parameter Name="strRemark" Type="String" />                
                <asp:Parameter Name="applyid" Type="String" />
            </UpdateParameters>
            <DeleteParameters>
                <asp:Parameter Name="Action" Type="String" />
                <asp:Parameter Name="applyid" Type="String" />
            </DeleteParameters>
            <InsertParameters>
                <asp:Parameter Name="Action" Type="String" />                
                <asp:Parameter Name="strName" Type="String" />
                <asp:Parameter Name="strYearID" Type="String" />
                <asp:Parameter Name="cdepid" Type="String" />  
                <asp:Parameter Name="strRemark" Type="String" />                
                <asp:Parameter Name="applyid" Type="String" />
            </InsertParameters>
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="SqlDataSourceDepartment" runat="server" ConnectionString="<%$ ConnectionStrings:webTestConnectionString %>" SelectCommand="SELECT distinct roomid,[strDepart] FROM [RoomDetail] WHERE ([depid] = @depid) order by [strDepart] desc" >
            <SelectParameters>
                <asp:SessionParameter Name="depid" SessionField="cdep" Type="String" />
            </SelectParameters>
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="SqlDataSourceRoom" runat="server" ConnectionString="<%$ ConnectionStrings:webTestConnectionString %>" SelectCommand="SELECT rd.roomid,rd.strRoomName FROM RoomDetail rd,Department d WHERE rd.depid = d.depid and d.cdepid = @cdepid">
            <SelectParameters>
                <asp:SessionParameter Name="cdepid" SessionField="cdep" Type="String" />
            </SelectParameters>
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="SqlDataSourceWeek" runat="server" ConnectionString="<%$ ConnectionStrings:webTestConnectionString %>" SelectCommand="SELECT [intWeek] FROM [WeekStartEnd] w inner join TitleStartEnd t on w.yearID = t.yearID and t.currentFlag = 'true'"></asp:SqlDataSource>
        <div class="left" runat="Server" id="leftTool">                    
            <asp:Button ID="btTotalSearchUp" runat="server" Text="筛选" OnClick="btTotalSearch_Click" />
            <br />
            <asp:TextBox ID="tbSearch" runat="server"></asp:TextBox>            
            <br />
            <asp:ListBox ID="liboDay" runat="server" SelectionMode="Multiple" Rows="7">
                <asp:ListItem>1</asp:ListItem>
                <asp:ListItem>2</asp:ListItem>
                <asp:ListItem>3</asp:ListItem>
                <asp:ListItem>4</asp:ListItem>
                <asp:ListItem>5</asp:ListItem>
                <asp:ListItem>6</asp:ListItem>
                <asp:ListItem>7</asp:ListItem>
            </asp:ListBox>                        
            <asp:ListBox ID="liboWeek" runat="server" DataSourceID="SqlDataSourceWeek" DataTextField="intWeek" DataValueField="intWeek" Rows="30" SelectionMode="Multiple"></asp:ListBox>            
            <asp:ListBox ID="liboRoom" runat="server" DataSourceID="SqlDataSourceRoom" DataTextField="strRoomName" DataValueField="roomid" Rows="30" SelectionMode="Multiple"></asp:ListBox>
            <br /> 
            <asp:Button ID="btliboAll" runat="server" Text="全选" OnClick="btliboAll_Click" />
            <br /> 
            <asp:Button ID="btliboNone" runat="server" Text="选择清除" OnClick="btliboNone_Click" />
            <br />               
            <asp:Button ID="btTotalSearch" runat="server" Text="筛选" OnClick="btTotalSearch_Click" /> 
            <div>            
                <asp:HyperLink ID="hlChangePW" runat="server" NavigateUrl="~/changePW.aspx">修改密码</asp:HyperLink>
                <asp:Button ID="btAbandon" runat="server" Text="注销" OnClick="btAbandon_Click" />
            </div>          
            <asp:HyperLink ID="hlNew" runat="server" NavigateUrl="~/ApplyAdd.aspx">新增</asp:HyperLink>
            <asp:HyperLink ID="hlQuery" runat="server" NavigateUrl="~/RoomApply.aspx" target="_blank">查询</asp:HyperLink>           
        </div>
        <div class="righttop">         
            <asp:GridView ID="GVApplyList" runat="server" AutoGenerateColumns="False" DataKeyNames="applyid" DataSourceID="sqsApplyList" 
                EnableModelValidation="True" AllowPaging="True" AllowSorting="True" OnRowDataBound="GVApplyList_RowDataBound" 
                AutoGenerateEditButton="True" OnRowDeleted="GVApplyList_RowDeleted" OnRowUpdated="GVApplyList_RowUpdated" 
                OnRowDeleting="GVApplyList_RowDeleting" OnRowEditing="GVApplyList_RowEditing" ShowFooter="True" 
                OnRowUpdating="GVApplyList_RowUpdating" OnRowCancelingEdit="GVApplyList_RowCancelingEdit"
                OnRowCommand="GVApplyList_RowCommand" OnPreRender="GVApplyList_PreRender" PageSize="30" Width="95%">
                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <a href="javascript:expandcollapse('div<%# Eval("applyid") %>', 'one');">
                                <img id="imgdiv<%# Eval("applyid") %>" alt="Click to show/hide detail <%# Eval("applyid") %>"  width="9px" border="0" src="img/plus.gif"/>
                            </a>
                        </ItemTemplate>
                    </asp:TemplateField>                    
                    <asp:TemplateField HeaderText="申请单号" SortExpression="applyid">
                        <EditItemTemplate>
                            <asp:Label ID="lbApplyid" runat="server" Text='<%# Eval("applyid") %>'></asp:Label>                      
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lbApplyid" runat="server" Text='<%# Eval("applyid") %>'></asp:Label>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:Label ID="lbApplyid" runat="server" Text='<%# Eval("applyid") %>'></asp:Label>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="课程名称" SortExpression="strName">
                        <EditItemTemplate>
                            <asp:TextBox ID="tbStrNameE" Text='<%# Eval("strName") %>' runat="server"></asp:TextBox>                     
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lbStrName" runat="server" Text='<%# Eval("strName") %>'></asp:Label>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="tbStrNameA" Text='' runat="server"></asp:TextBox>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="学期" SortExpression="yearID">
                        <EditItemTemplate>
                            <asp:Label ID="lbYearID" runat="server" Text='<%# Eval("yearID") %>'></asp:Label>                      
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lbYearID" runat="server" Text='<%# Eval("yearID") %>'></asp:Label>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:Label ID="lbYearID" runat="server" Text='<%# Eval("yearID") %>'></asp:Label>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="部门" SortExpression="cdepid">
                        <EditItemTemplate>
                            <asp:Label ID="lbStrCDep" runat="server" Text='<%# Eval("cdepid") %>'></asp:Label>                      
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lbStrCDep" runat="server" Text='<%# Eval("cdepid") %>'></asp:Label>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:Label ID="lbStrCDep" runat="server" Text='<%# Eval("cdepid") %>'></asp:Label>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="备注" SortExpression="strRemark">
                        <EditItemTemplate>
                            <asp:TextBox ID="tbStrRemarkE" Text='<%# Eval("strRemark") %>' runat="server"></asp:TextBox>                     
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lbStrRemark" runat="server" Text='<%# Eval("strRemark") %>'></asp:Label>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="tbStrRemarkA" Text='' runat="server"></asp:TextBox>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--<asp:TemplateField ShowHeader="False">
                        <ItemTemplate>
                            <asp:LinkButton ID="lbDelete" runat="server" CausesValidation="False"
                                    CommandName="Delete" Text="删除" OnClientClick="return confirm('是否删除该记录？');"></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>--%>
                    <asp:TemplateField >
                        <ItemTemplate>
                            <asp:LinkButton ID="lbDelete" runat="server" CausesValidation="False"
                                    CommandName="Delete" Text="删除" OnClientClick="return confirm('是否删除该记录？');"></asp:LinkButton>
                            <tr>
                            <td colspan="100%">
                                <div id="div<%# Eval("applyid") %>" style="display:none;position:relative;left:15px;OVERFLOW: auto;WIDTH:97%" >
                                <asp:SqlDataSource ID="sqsRoomApply" runat="server" ConnectionString="<%$ ConnectionStrings:webTestConnectionString %>" 
                                    SelectCommand="SELECT a.id, a.applyid, d.strRoomName,a.roomid, a.intDay, a.intStartNum, a.intEndNum, RTRIM(a.strClass) as strClass, RTRIM(a.strTeacher) as strTeacher, a.strWeekReg, a.strWeekData,a.strRemark FROM RoomApply a,RoomDetail d WHERE a.applyid = @applyid and a.roomid = d.roomid order by d.strRoomName"
                                    UpdateCommandType ="StoredProcedure" UpdateCommand="RoomApplyAction" 
                                    DeleteCommandType ="StoredProcedure" DeleteCommand="RoomApplyAction"
                                    InsertCommandType ="StoredProcedure" InsertCommand="RoomApplyAction">
                                    <SelectParameters>
                                        <asp:Parameter Name="applyid" Type="String" />
                                    </SelectParameters>
                                    <UpdateParameters>
                                        <asp:Parameter Name="Action" Type="String" />
                                        <asp:Parameter Name="roomid" Type="String" />
                                        <asp:Parameter Name="intDay" Type="int16" />
                                        <asp:Parameter Name="intStartNum" Type="int16" />
                                        <asp:Parameter Name="intEndNum" Type="int16" />
                                        <asp:Parameter Name="strWeekReg" Type="String" />
                                        <asp:Parameter Name="strWeekData" Type="String" />                                    
                                        <asp:Parameter Name="strClass" Type="String" />
                                        <asp:Parameter Name="strTeacher" Type="String" />
                                        <asp:Parameter Name="strRemark" Type="String" />                
                                        <asp:Parameter Name="id" Type="String" />
                                    </UpdateParameters>
                                    <DeleteParameters>
                                        <asp:Parameter Name="Action" Type="String" />                                                    
                                        <asp:Parameter Name="id" Type="String" />
                                    </DeleteParameters>
                                    <InsertParameters>
                                        <asp:Parameter Name="Action" Type="String" />
                                        <asp:Parameter Name="roomid" Type="String" />
                                        <asp:Parameter Name="intDay" Type="int16" />
                                        <asp:Parameter Name="intStartNum" Type="int16" />
                                        <asp:Parameter Name="intEndNum" Type="int16" />
                                        <asp:Parameter Name="strWeekReg" Type="String" />
                                        <asp:Parameter Name="strWeekData" Type="String" />                                    
                                        <asp:Parameter Name="strClass" Type="String" />
                                        <asp:Parameter Name="strTeacher" Type="String" />
                                        <asp:Parameter Name="strRemark" Type="String" />
                                        <asp:Parameter Name="applyid" Type="String" />                
                                        <asp:Parameter Name="id" Type="String" />
                                    </InsertParameters>
                                </asp:SqlDataSource>                            
                                <asp:GridView ID="GVRoomApply" runat="server" DataKeyNames="applyid,id" AutoGenerateColumns="False" AllowSorting="True"
                                     AutoGenerateDeleteButton="False" AutoGenerateEditButton="True" Showfooter="true" emptydatatext="No data available."
                                     OnRowDeleted="GVRoomApply_RowDeleted" OnRowUpdated="GVRoomApply_RowUpdated" OnRowDeleting="GVRoomApply_RowDeleting"
                                     OnRowEditing="GVRoomApply_RowEditing" OnRowUpdating="GVRoomApply_RowUpdating"
                                     OnRowCancelingEdit="GVRoomApply_RowCancelingEdit" OnSorting="GVRoomApply_Sorting" 
                                     OnRowDataBound="GVRoomApply_RowDataBound" OnRowCommand="GVRoomApply_RowCommand" OnPreRender="GVRoomApply_PreRender"
                                     DataSourceID="sqsRoomApply" AllowPaging="True" PageSize="15">
                                    <Columns>                                    
                                        <asp:TemplateField ShowHeader="False">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lbDeleteS" runat="server" CausesValidation="False"
                                                        CommandName="Delete" Text="删除" OnClientClick="return confirm('是否删除该记录？');"></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ShowHeader="False">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lbCopySub" runat="server" CausesValidation="False"
                                                        CommandName="CopySub" Text="复制" ></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="subid" SortExpression="id" Visible="false">
                                           <EditItemTemplate>
                                                <asp:Label ID="lbid" runat="server" Text='<%# Eval("id") %>'></asp:Label>                     
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lbid" runat="server" Text='<%# Eval("id") %>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lbid" runat="server" Text='<%# Eval("id") %>'></asp:Label>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="教室" SortExpression="strRoomName">
                                            <EditItemTemplate>
                                                <asp:DropDownList ID="ddlRoomGVRAedit" runat="server" DataSourceID="SqlDataSourceRoom" DataTextField="strRoomName" DataValueField="roomid" SelectedValue='<%# Bind("roomid") %>'> </asp:DropDownList>                      
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lbStrRoom" runat="server" Text='<%# Eval("strRoomName") %>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:DropDownList ID="ddlRoomGVRAadd" runat="server" DataSourceID="SqlDataSourceRoom" DataTextField="strRoomName" DataValueField="roomid" SelectedValue='<%# Bind("roomid") %>'> </asp:DropDownList>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="日期（星期几）" SortExpression="intDay" >
                                            <EditItemTemplate>
                                                <%--<asp:TextBox ID="tbIntDayE" Text='<%# Eval("intDay") %>' runat="server"  Width="15"></asp:TextBox>--%>
                                                <asp:DropDownList ID="ddlDayE" runat="server" SelectedValue='<%# Bind("intDay") %>'>
                                                    <asp:ListItem>1</asp:ListItem>
                                                    <asp:ListItem>2</asp:ListItem>
                                                    <asp:ListItem>3</asp:ListItem>
                                                    <asp:ListItem>4</asp:ListItem>
                                                    <asp:ListItem>5</asp:ListItem>
                                                    <asp:ListItem>6</asp:ListItem>
                                                    <asp:ListItem>7</asp:ListItem>
                                                </asp:DropDownList>                     
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lbIntDay" runat="server" Text='<%# Eval("intDay") %>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:DropDownList ID="ddlDayA" runat="server">
                                                    <asp:ListItem>1</asp:ListItem>
                                                    <asp:ListItem>2</asp:ListItem>
                                                    <asp:ListItem>3</asp:ListItem>
                                                    <asp:ListItem>4</asp:ListItem>
                                                    <asp:ListItem>5</asp:ListItem>
                                                    <asp:ListItem>6</asp:ListItem>
                                                    <asp:ListItem>7</asp:ListItem>
                                                </asp:DropDownList>                     
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="开始节次" SortExpression="intStartNum">
                                            <EditItemTemplate>
                                                <asp:DropDownList ID="ddlStartNE" runat="server" AutoPostBack="True" SelectedValue='<%# Bind("intStartNum") %>' OnSelectedIndexChanged="ddlStartNE_SelectedIndexChanged">
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
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lbIntStartNum" runat="server" Text='<%# Eval("intStartNum") %>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:DropDownList ID="ddlStartNA" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlStartNA_SelectedIndexChanged">
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
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="结束节次" SortExpression="intEndNum">
                                            <EditItemTemplate>
                                                <asp:DropDownList ID="ddlEndNE" runat="server" AutoPostBack="False" SelectedValue='<%# Bind("intEndNum") %>'>
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
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lbIntEndNum" runat="server" Text='<%# Eval("intEndNum") %>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:DropDownList ID="ddlEndNA" runat="server" AutoPostBack="False">
                                                    <asp:ListItem>1</asp:ListItem>
                                                    <asp:ListItem Selected="True">2</asp:ListItem>
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
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="班级" SortExpression="strClass">
                                            <EditItemTemplate>
                                                <asp:TextBox ID="tbStrClassE" Text='<%# Eval("strClass") %>' runat="server"  Width="90"></asp:TextBox>                     
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lbStrClass" runat="server" Text='<%# Eval("strClass") %>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox ID="tbStrClassA" Text='' runat="server" Width="90"></asp:TextBox>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="教师" SortExpression="strTeacher">
                                            <EditItemTemplate>
                                                <asp:TextBox ID="tbStrTeacherE" Text='<%# Eval("strTeacher") %>' runat="server"  Width="40"></asp:TextBox>                     
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lbStrTeacher" runat="server" Text='<%# Eval("strTeacher") %>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox ID="tbStrTeacherA" Text='' runat="server"  Width="40"></asp:TextBox>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="strWeekReg" SortExpression="strWeekReg">
                                            <EditItemTemplate>
                                                <asp:TextBox ID="tbStrWeekRegE" Text='<%# Eval("strWeekReg") %>' runat="server"></asp:TextBox>                     
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lbStrWeekReg" runat="server" Text='<%# Eval("strWeekReg") %>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox ID="tbStrWeekRegA" Text='' runat="server"></asp:TextBox>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="备注" SortExpression="strRemark">
                                            <EditItemTemplate>
                                                <asp:TextBox ID="tbStrRemarkE" Text='<%# Eval("strRemark") %>' runat="server"></asp:TextBox>                     
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lbStrRemark" runat="server" Text='<%# Eval("strRemark") %>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox ID="tbStrRemarkA" Text='' runat="server"></asp:TextBox>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="strWeekData" SortExpression="strWeekData">
                                            <EditItemTemplate>
                                                <asp:Label ID="lbStrWeekData" runat="server" Text='<%# Eval("strWeekData") %>'></asp:Label>                     
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lbStrWeekData" runat="server" Text='<%# Eval("strWeekData") %>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:LinkButton ID="linkAddRA" CommandName="AddRA" runat="server">增加子记录</asp:LinkButton>
                                            </FooterTemplate>
                                        </asp:TemplateField>                                        
                                    </Columns>
                                    <PagerSettings FirstPageText="首页" LastPageText="末页" Mode="NumericFirstLast" NextPageText="下一页" PreviousPageText="上一页" />
                                    </asp:GridView> 
                                    </div>
                             </td>
                        </tr>                                                        
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:LinkButton ID="linkAddAL" CommandName="AddAL" runat="server">增加主记录（申请单）</asp:LinkButton>
                        </FooterTemplate>                        
                    </asp:TemplateField>
                </Columns>
                <PagerSettings FirstPageText="首页" LastPageText="末页" Mode="NumericFirstLast" NextPageText="下一页" PreviousPageText="上一页" />
            </asp:GridView>
        </div>
    </form>
</body>
</html>
