using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
//下载于51aspx.com
public partial class NextWebF : System.Web.UI.Page
{
    int indexedit;
    string gvUniqueID = String.Empty;
    int gvNewPageIndex = 0;
    int gvEditIndex = -1;
    string gvSortExpr = String.Empty;
    private string gvSortDir
    {

        get { return ViewState["SortDirection"] as string ?? "ASC"; }

        set { ViewState["SortDirection"] = value; }

    }
    //This procedure returns the Sort Direction
    private string GetSortDirection()
    {
        switch (gvSortDir)
        {
            case "ASC":
                gvSortDir = "DESC";
                break;

            case "DESC":
                gvSortDir = "ASC";
                break;
        }
        return gvSortDir;
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)    //Page.IsPostBack
        {
            if (user.redirectSet(Convert.ToString(Session["user"])))
            { Response.Redirect("tempLogin.aspx"); }

            if (Session["ddlDep"] != null)
            {
                //DropDownListDepart.SelectedValue = Session["ddlDep"].ToString();
            }
            

        }
        if(ViewState["roomapplySelStr"] != null)
        {
            //SqlDataSourceRoomApply.SelectCommand = ViewState["roomapplySelStr"].ToString();
        }
        

    }



    #region gridviewEvent

    protected void GVApplyList_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        SqlDataSource sqsRoomApply;
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            sqsRoomApply = e.Row.FindControl("sqsRoomApply") as SqlDataSource;            
            sqsRoomApply.SelectParameters["applyid"].DefaultValue = (e.Row.DataItem as DataRowView)["applyid"].ToString();
            
            //Find Child GridView control
            GridViewRow row = e.Row;
            string strSort = string.Empty;
            GridView gv = new GridView();
            gv = (GridView)row.FindControl("GVRoomApply");

            //Check if any additional conditions (Paging, Sorting, Editing, etc) to be applied on child GridView
            if (gv.UniqueID == gvUniqueID)
            {
                gv.PageIndex = gvNewPageIndex;
                gv.EditIndex = gvEditIndex;
                //Check if Sorting used
                if (gvSortExpr != string.Empty)
                {
                    GetSortDirection();
                    strSort = " ORDER BY " + string.Format("{0} {1}", gvSortExpr, gvSortDir);
                    sqsRoomApply.SelectCommand = sqsRoomApply.SelectCommand + strSort;
                    ViewState["roomapplySelStr"] = sqsRoomApply.SelectCommand;
                }
                //Expand the Child grid
                ClientScript.RegisterStartupScript(GetType(), "Expand", "<SCRIPT LANGUAGE='javascript'>expandcollapse('div" + ((DataRowView)e.Row.DataItem)["applyid"].ToString() + "','one');</script>");
            }
            if (ViewState["roomapplySelStr"] != null)
            {
                sqsRoomApply.SelectCommand = ViewState["roomapplySelStr"].ToString();
            }


            gv.DataSource = sqsRoomApply;
            gv.DataBind();
        }

        //ClientScript.RegisterStartupScript(GetType(), "Expand", "<SCRIPT LANGUAGE='javascript'>expandcollapse('div" + ((DataRowView)e.Row.DataItem)["applyid"].ToString() + "','one');</script>");
        
    }

    protected void GVRoomApply_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        ////Check if this is our Blank Row being databound, if so make the row invisible
        //if (e.Row.RowType == DataControlRowType.DataRow)
        //{
        //    if (((DataRowView)e.Row.DataItem)["id"].ToString() == String.Empty) e.Row.Visible = false;
        //}
    }

    protected void GVApplyList_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "AddAL")
        {
            try
            {   
                string NameN = ((TextBox)GVApplyList.FooterRow.FindControl("tbStrNameA")).Text;
                string YearID = CommonClass.getCurYearID();
                string CDep = Session["dep"].ToString();
                string RemarkN = ((TextBox)GVApplyList.FooterRow.FindControl("tbStrRemarkA")).Text;
                string applyidN = string.Format("{0:yyyyMMddHHmmssffff}", DateTime.Now);                

                sqsApplyList.InsertParameters["Action"].DefaultValue = "insert";
                sqsApplyList.InsertParameters["strName"].DefaultValue = NameN;
                sqsApplyList.InsertParameters["strYearID"].DefaultValue = YearID;
                sqsApplyList.InsertParameters["strCdep"].DefaultValue = CDep;
                sqsApplyList.InsertParameters["strRemark"].DefaultValue = RemarkN;
                sqsApplyList.InsertParameters["applyid"].DefaultValue = applyidN;

                sqsApplyList.Insert();
                GVApplyList.DataBind();
                leftTool.Visible = true;
                Response.Write("<script>alert('操作成功')</script>");
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('" + ex.Message.ToString().Replace("'", "") + "');</script>");
            }
        }
    }

    protected void GVRoomApply_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "AddRA")
        {
            try
            {
                GridView gvTemp = (GridView)sender;
                gvUniqueID = gvTemp.UniqueID;
                SqlDataSource sqsRoomApply = (SqlDataSource)gvTemp.Parent.FindControl("sqsRoomApply");

                //Get the values stored in the text boxes 
                string roomN = ((DropDownList)gvTemp.FooterRow.FindControl("ddlRoomGVRAadd")).SelectedValue;
                int dayW = Convert.ToInt16(((TextBox)gvTemp.FooterRow.FindControl("tbIntDayA")).Text);
                int startN = Convert.ToInt16(((TextBox)gvTemp.FooterRow.FindControl("tbIntStartNumA")).Text);
                int endN = Convert.ToInt16(((TextBox)gvTemp.FooterRow.FindControl("tbIntEndNumA")).Text);
                string ClassN = ((TextBox)gvTemp.FooterRow.FindControl("tbStrClassA")).Text;
                string TeacherN = ((TextBox)gvTemp.FooterRow.FindControl("tbStrTeacherA")).Text;
                string weekReg = ((TextBox)gvTemp.FooterRow.FindControl("tbStrWeekRegA")).Text;
                string weekData = string.Empty;
                string applyid = gvTemp.DataKeys[0].Value.ToString().TrimEnd();
                string idN = string.Format("{0:yyyyMMddHHmmssffff}", DateTime.Now);

                //验证reg到data的转换
                string regData = "ini";
                bool rTdFalg = CommonClass.regToData(weekReg, out regData);
                if (rTdFalg == true)
                {
                    if (regData == "ini")
                    {
                        Response.Write("<script>alert('data=ini')</script>");
                        return;
                    }
                    else
                    {
                        weekData = regData;
                        sqsRoomApply.InsertParameters["strWeekData"].DefaultValue = weekData;
                    }
                }

                if (rTdFalg == false)
                {
                    Response.Write("<script>alert('" + regData + "')</script>");
                    return;
                }

                if (weekReg == null)
                {
                    Response.Write("<script>alert('周输入不可为空')</script>");
                    return;
                }              

                if (startN > endN)
                {
                    Response.Write("<script>alert('开始节次不能大于结束节次')</script>");
                    return;
                }
                
                if (ClassN == "")
                {
                    Response.Write("<script>alert('班级名未填写')</script>");
                    return;
                }
                if (TeacherN == "")
                {
                    Response.Write("<script>alert('教师名未填写')</script>");
                    return;
                }

                string checkmsg = CommonClass.CheckApply(roomN, dayW, startN, endN, weekData, idN);
                if (checkmsg != "OK")
                {
                    Response.Write("<script>alert('" + checkmsg + "')</script>");
                    return;
                }


                //Prepare the Insert Command of the DataSource control
                sqsRoomApply.InsertParameters["Action"].DefaultValue = "insert";
                sqsRoomApply.InsertParameters["strRoom"].DefaultValue = roomN;
                sqsRoomApply.InsertParameters["intDay"].DefaultValue = dayW.ToString();
                sqsRoomApply.InsertParameters["intStartNum"].DefaultValue = startN.ToString();
                sqsRoomApply.InsertParameters["intEndNum"].DefaultValue = endN.ToString();
                sqsRoomApply.InsertParameters["strWeekReg"].DefaultValue = weekReg;
                //sqsRoomApply.InsertParameters["strWeekData"].DefaultValue = weekData;                
                sqsRoomApply.InsertParameters["strClass"].DefaultValue = ClassN;
                sqsRoomApply.InsertParameters["strTeacher"].DefaultValue = TeacherN;
                sqsRoomApply.InsertParameters["applyid"].DefaultValue = applyid;
                sqsRoomApply.InsertParameters["id"].DefaultValue = idN;

                sqsRoomApply.Insert();
                GVApplyList.DataBind();
                leftTool.Visible = true;
                Response.Write("<script>alert('操作成功')</script>");
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('" + ex.Message.ToString().Replace("'", "") + "');</script>");
            }
        }
    }

    protected void GVApplyList_RowDeleted(object sender, GridViewDeletedEventArgs e)
    {

    }

    protected void GVRoomApply_RowDeleted(object sender, GridViewDeletedEventArgs e)
    {
        
    }

    protected void GVApplyList_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        string CurYear = CommonClass.getCurYearID();
        string UsrDep = Session["dep"].ToString();

        sqsApplyList.UpdateParameters["Action"].DefaultValue = "update";
        sqsApplyList.UpdateParameters["strName"].DefaultValue = ((TextBox)GVApplyList.Rows[e.RowIndex].FindControl("tbStrNameE")).Text;
        sqsApplyList.UpdateParameters["strYearID"].DefaultValue = CurYear;
        sqsApplyList.UpdateParameters["strCDep"].DefaultValue = UsrDep;
        sqsApplyList.UpdateParameters["strRemark"].DefaultValue = ((TextBox)GVApplyList.Rows[e.RowIndex].FindControl("tbStrRemarkE")).Text;
        sqsApplyList.UpdateParameters["applyid"].DefaultValue = e.Keys["applyid"].ToString();

        sqsApplyList.Update();
        GVApplyList.DataBind();
        leftTool.Visible = true;
        Response.Write("<script>alert('操作成功')</script>");
    }

    protected void GVRoomApply_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        GridView gvTemp = (GridView)sender;
        
        gvUniqueID = gvTemp.UniqueID;

        //int ALRowIndex = e.Keys["applyid"]

        SqlDataSource sqsRoomApply = (SqlDataSource)gvTemp.Parent.FindControl("sqsRoomApply");
        //SqlDataSource sqsRoomApply = (SqlDataSource)gvTemp.DataSource;
        //string applyid = gvTemp.DataKeys[0].Value.ToString();

        string roomN = ((DropDownList)gvTemp.Rows[e.RowIndex].FindControl("ddlRoomGVRA")).SelectedValue;
        int dayW = Convert.ToInt16(((TextBox)gvTemp.Rows[e.RowIndex].FindControl("tbIntDayE")).Text);
        int startN = Convert.ToInt16(((TextBox)gvTemp.Rows[e.RowIndex].FindControl("tbIntStartNumE")).Text);
        int endN = Convert.ToInt16(((TextBox)gvTemp.Rows[e.RowIndex].FindControl("tbIntEndNumE")).Text);
        string ClassN = ((TextBox)gvTemp.Rows[e.RowIndex].FindControl("tbStrClassE")).Text;
        string TeacherN = ((TextBox)gvTemp.Rows[e.RowIndex].FindControl("tbStrTeacherE")).Text;
        string weekReg = ((TextBox)gvTemp.Rows[e.RowIndex].FindControl("tbStrWeekRegE")).Text;
        string weekData = string.Empty;        
        string idN = ((Label)gvTemp.Rows[e.RowIndex].FindControl("lbid")).Text;

        //验证
        if (weekReg == string.Empty)
        {
            Response.Write("<script>alert('周输入不可为空')</script>");
            e.Cancel = true;
            GVApplyList.DataBind();
            return;
        }

        //验证reg到data的转换
        string regData = "ini";
        bool rTdFalg = CommonClass.regToData(weekReg, out regData);
        if (rTdFalg == true)
        {
            if (regData == "ini")
            {
                Response.Write("<script>alert('data=ini')</script>");
                e.Cancel = true;
                GVApplyList.DataBind();
                return;
            }
            else
            {
                weekData = regData;
            }
        }
        if (rTdFalg == false)
        {
            Response.Write("<script>alert('" + regData + "')</script>");
            e.Cancel = true;
            GVApplyList.DataBind();
            return;
        }

        RegexStringValidator regday = new RegexStringValidator("^[1-7]{1}$");
        RegexStringValidator regnum = new RegexStringValidator("^[1-9]{1}$|^1[10]{1}");
        try
        {
            regday.Validate(dayW.ToString());
        }
        catch
        {
            //LabelMsg.Visible = true;
            //LabelMsg.Text = "日期只能填数字1至数字7";
            Response.Write("<script>alert('日期只能填数字1至数字7')</script>");
            e.Cancel = true;
            GVApplyList.DataBind();
            return;
        }
        try
        {
            regnum.Validate(startN.ToString());
            regnum.Validate(endN.ToString());
        }
        catch
        {
            Response.Write("<script>alert('节次只能填数字1至数字11')</script>");
            e.Cancel = true;
            GVApplyList.DataBind();
            return;
        }

        if (startN > endN)
        {
            Response.Write("<script>alert('开始节次不能大于结束节次')</script>");
            e.Cancel = true;
            GVApplyList.DataBind();
            return;
        }

        if (ClassN == string.Empty)
        {
            Response.Write("<script>alert('班级未填写')</script>");
            e.Cancel = true;
            GVApplyList.DataBind();
            return;
        }

        if (TeacherN == string.Empty)
        {
            Response.Write("<script>alert('教师未填写')</script>");
            e.Cancel = true;
            GVApplyList.DataBind();
            return;
        }

        string checkmsg = CommonClass.CheckApply(roomN, dayW, startN, endN, weekData, idN);
        if (checkmsg != "OK")
        {
            Response.Write("<script>alert('" + checkmsg + "')</script>");
            e.Cancel = true;
            GVApplyList.DataBind();
            return;
        }

        //更新数据
        sqsRoomApply.UpdateParameters["action"].DefaultValue = "update";
        sqsRoomApply.UpdateParameters["strRoom"].DefaultValue = ((DropDownList)gvTemp.Rows[e.RowIndex].FindControl("ddlRoomGVRA")).SelectedValue;
        sqsRoomApply.UpdateParameters["intDay"].DefaultValue = ((TextBox)gvTemp.Rows[e.RowIndex].FindControl("tbIntDayE")).Text;
        sqsRoomApply.UpdateParameters["intStartNum"].DefaultValue = ((TextBox)gvTemp.Rows[e.RowIndex].FindControl("tbIntStartNumE")).Text;
        sqsRoomApply.UpdateParameters["intEndNum"].DefaultValue = ((TextBox)gvTemp.Rows[e.RowIndex].FindControl("tbIntEndNumE")).Text;
        sqsRoomApply.UpdateParameters["strWeekReg"].DefaultValue = ((TextBox)gvTemp.Rows[e.RowIndex].FindControl("tbStrWeekRegE")).Text;
        sqsRoomApply.UpdateParameters["strWeekData"].DefaultValue = weekData;
        sqsRoomApply.UpdateParameters["strClass"].DefaultValue = ((TextBox)gvTemp.Rows[e.RowIndex].FindControl("tbStrClassE")).Text;
        sqsRoomApply.UpdateParameters["strTeacher"].DefaultValue = ((TextBox)gvTemp.Rows[e.RowIndex].FindControl("tbStrTeacherE")).Text;
        sqsRoomApply.UpdateParameters["id"].DefaultValue = ((Label)gvTemp.Rows[e.RowIndex].FindControl("lbid")).Text;

        sqsRoomApply.Update();
        GVApplyList.DataBind();        
        leftTool.Visible = true;
        Response.Write("<script>alert('操作成功')</script>");
    }

    protected void GVApplyList_RowUpdated(object sender, GridViewUpdatedEventArgs e)
    {

    }

    protected void GVRoomApply_RowUpdated(object sender, GridViewUpdatedEventArgs e)
    {
        
    }

    protected void GVApplyList_RowEditing(object sender, GridViewEditEventArgs e)
    {
        leftTool.Visible = false;
    }

    protected void GVRoomApply_RowEditing(object sender, GridViewEditEventArgs e)
    {
        leftTool.Visible = false;
        GridView gvTemp = (GridView)sender;
        gvUniqueID = gvTemp.UniqueID;
        gvEditIndex = e.NewEditIndex;
        GVApplyList.DataBind();
    }

    protected void GVApplyList_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        sqsApplyList.DeleteParameters["Action"].DefaultValue = "delete";
        sqsApplyList.DeleteParameters["applyid"].DefaultValue = e.Keys["applyid"].ToString();

        sqsApplyList.Delete();
        GVApplyList.DataBind();
        leftTool.Visible = true;
        Response.Write("<script>alert('操作成功')</script>");
    }

    protected void GVRoomApply_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        GridView gvTemp = (GridView)sender;
        gvUniqueID = gvTemp.UniqueID;
        SqlDataSource sqsRoomApply = (SqlDataSource)gvTemp.Parent.FindControl("sqsRoomApply");

        sqsRoomApply.DeleteParameters["action"].DefaultValue = "delete";
        sqsRoomApply.DeleteParameters["id"].DefaultValue = ((Label)gvTemp.Rows[e.RowIndex].FindControl("lbid")).Text;

        sqsRoomApply.Delete();
        GVApplyList.DataBind();
        leftTool.Visible = true;
        Response.Write("<script>alert('操作成功')</script>");
    }

    protected void GVApplyList_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        leftTool.Visible = true;        
    }

    protected void GVRoomApply_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        leftTool.Visible = true;
        GridView gvTemp = (GridView)sender;
        gvUniqueID = gvTemp.UniqueID;
        gvEditIndex = -1;
        GVApplyList.DataBind();
    }

    protected void GVRoomApply_Sorting(object sender, GridViewSortEventArgs e)
    {
        GridView gvTemp = (GridView)sender;
        gvUniqueID = gvTemp.UniqueID;
        gvSortExpr = e.SortExpression;
        GVApplyList.DataBind();
    }

    #endregion

    #region leftEvent

    protected void btAbandon_Click(object sender, EventArgs e)
    {
        Session.Abandon();
        Response.Redirect("templogin.aspx");
    }


    protected void btliboAll_Click(object sender, EventArgs e)
    {
        foreach (ListItem li in liboRoom.Items)
        {
            li.Selected = true;
        }
        foreach (ListItem li in liboWeek.Items)
        {
            li.Selected = true;
        }
        foreach (ListItem li in liboDay.Items)
        {
            li.Selected = true;
        }
    }

    protected void btliboNone_Click(object sender, EventArgs e)
    {
        foreach (ListItem li in liboRoom.Items)
        {
            li.Selected = false;
        }
        foreach (ListItem li in liboWeek.Items)
        {
            li.Selected = false;
        }
        foreach (ListItem li in liboDay.Items)
        {
            li.Selected = false;
        }
    }



    protected void btTotalSearch_Click(object sender, EventArgs e)
    {
        //string sRoom = string.Empty;
        //foreach (ListItem li in liboRoom.Items)
        //{
        //    if (li.Selected == true)
        //        sRoom += "'" + li.Value.Trim() + "',";
        //}

        //if (sRoom != string.Empty)
        //{
        //    sRoom = sRoom.Substring(0, sRoom.Length - 1); // chop off trailing ,   
        //}
        //else
        //{
        //    foreach (ListItem li in liboRoom.Items)
        //    {
        //        sRoom += "'" + li.Value.Trim() + "',";
        //    }
        //    sRoom = sRoom.Substring(0, sRoom.Length - 1); // chop off trailing , 
        //}

        //string sWeek = string.Empty;
        //foreach (ListItem li in liboWeek.Items)
        //{
        //    if (li.Selected == true)
        //        sWeek += li.Value.Trim() + ",";
        //}

        //if (sWeek != string.Empty)
        //{
        //    sWeek = sWeek.Substring(0, sWeek.Length - 1); // chop off trailing ,   
        //}
        //else
        //{
        //    foreach (ListItem li in liboWeek.Items)
        //    {
        //        sWeek += li.Value.Trim() + ",";
        //    }
        //    sWeek = sWeek.Substring(0, sWeek.Length - 1); // chop off trailing , 
        //}

        //string sDay = string.Empty;
        //foreach (ListItem li in liboDay.Items)
        //{
        //    if (li.Selected == true)
        //        sDay += li.Value.Trim() + ",";
        //}

        //if (sDay != string.Empty)
        //{
        //    sDay = sDay.Substring(0, sDay.Length - 1); // chop off trailing ,   
        //}
        //else
        //{
        //    foreach (ListItem li in liboDay.Items)
        //    {
        //        sDay += li.Value.Trim() + ",";
        //    }
        //    sDay = sDay.Substring(0, sDay.Length - 1); // chop off trailing , 
        //}

        //SqlDataSourceRoomApply.SelectParameters.Clear();
        //SqlDataSourceRoomApply.SelectCommand = String.Format("select distinct a.id,a.strRoom,a.intDay,a.intStartNum,a.intEndNum,a.strWeekReg,a.strWeekData,RTRIM(a.strName) as strName,RTRIM(a.strClass) as strClass,RTRIM(a.strTeacher) as strTeacher,a.yearID from RoomApply a ,RoomDetail d,TitleStartEnd w,RoomApplySub s where a.strRoom = d.strRoomName  and a.yearID = w.yearID and w.currentFlag = 'true' and a.id = s.F_id and d.strDepart = @depN_CP and a.strRoom in ({0}) and s.intWeek in ({1}) and a.intDay in ({2}) and ((a.strName like '%'+ @searchTextBox_CP + '%') or (a.strTeacher like '%'+ @searchTextBox_CP + '%') or (a.strClass like '%'+ @searchTextBox_CP + '%') or (@searchTextBox_CP = 'init')) order by a.id desc", sRoom, sWeek, sDay);
        //ControlParameter searchTextBox_CP = new ControlParameter();
        //searchTextBox_CP.Name = "searchTextBox_CP";
        //searchTextBox_CP.Type = TypeCode.String;
        //searchTextBox_CP.ControlID = "tbSearch";
        //searchTextBox_CP.PropertyName = "Text";
        //searchTextBox_CP.DefaultValue = "init";
        //SqlDataSourceRoomApply.SelectParameters.Add(searchTextBox_CP);
        //ControlParameter depN_CP = new ControlParameter();
        //depN_CP.Name = "depN_CP";
        //depN_CP.Type = TypeCode.String;
        //depN_CP.ControlID = "DropDownListDepart";
        //depN_CP.PropertyName = "SelectedValue";
        //SqlDataSourceRoomApply.SelectParameters.Add(depN_CP);
        //ViewState["roomapplySelStr"] = SqlDataSourceRoomApply.SelectCommand;
        //GridView10.DataBind();
    }

    #endregion    


}
