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
    int showFooter = 1;//1 show,0 hide
    

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

    public static void renderEmptyGridView(GridView EmptyGridView, string FieldNames)
    {
        //将GridView变成只有Header和Footer列，以及被隐藏的空白资料列    
        DataTable dTable = new DataTable();
        char[] delimiterChars = { ',' };
        string[] colName = FieldNames.Split(delimiterChars);
        foreach (string myCol in colName)
        {
            DataColumn dColumn = new DataColumn(myCol.Trim());
            dTable.Columns.Add(dColumn);
        }
        DataRow dRow = dTable.NewRow();
        foreach (string myCol in colName)
        {
            dRow[myCol.Trim()] = DBNull.Value;
        }
        dTable.Rows.Add(dRow);
        EmptyGridView.DataSourceID = null;
        EmptyGridView.DataSource = dTable;
        EmptyGridView.DataBind();
        EmptyGridView.Rows[0].Visible = false;        
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)    //Page.IsPostBack
        {
            if (user.redirectSet(Convert.ToString(Session["user"])))
            { Response.Redirect("tempLogin.aspx"); }
                        

        }
        if (Session["cdep"] == null)
        {
            Response.Redirect("tempLogin.aspx");
        }
        if ((ViewState["ApplyListSelStr"] != null) && (sqsApplyList.SelectCommand != ViewState["ApplyListSelStr"].ToString()))
        {
            sqsApplyList.SelectCommand = ViewState["ApplyListSelStr"].ToString();
            //GVApplyList.DataBind();
        }
        if (Convert.ToInt16(Session["CopySubFlag"]) == 1)
        {
            Session["CopySubFlag"] = 0;
        }


    }



    #region gridviewEvent

    protected void GVApplyList_RowDataBound(object sender, GridViewRowEventArgs e)
    {        
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            SqlDataSource sqsRoomApply;
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
                    sqsRoomApply.SelectCommand = (sqsRoomApply.SelectCommand).Replace(" order by a.id desc", "");
                    sqsRoomApply.SelectCommand = sqsRoomApply.SelectCommand + strSort;
                    ViewState["roomapplySelStr"] = sqsRoomApply.SelectCommand;
                }
                //show footer
                if(showFooter == 0)
                {
                    gv.ShowFooter = false;                    
                }
                if(showFooter == 1)
                {
                    gv.ShowFooter = true;
                }                

                //Expand the Child grid
                ClientScript.RegisterStartupScript(GetType(), "Expand", "<SCRIPT LANGUAGE='javascript'>expandcollapse('div" + ((DataRowView)e.Row.DataItem)["applyid"].ToString() + "','one');</script>");
            }
            if (ViewState["roomapplySelStr"] != null)
            {
                sqsRoomApply.SelectCommand = ViewState["roomapplySelStr"].ToString();
            }

            //gv.DataSource = sqsRoomApply;
            gv.DataBind();
        }

        //ClientScript.RegisterStartupScript(GetType(), "Expand", "<SCRIPT LANGUAGE='javascript'>expandcollapse('div" + ((DataRowView)e.Row.DataItem)["applyid"].ToString() + "','one');</script>");
        
    }

    protected void GVRoomApply_RowDataBound(object sender, GridViewRowEventArgs e)
    {

    }

    protected void GVApplyList_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "AddAL")
        {
            try
            {   
                string NameN = ((TextBox)GVApplyList.FooterRow.FindControl("tbStrNameA")).Text;
                string YearID = CommonClass.getCurYearID();
                string CDep = Session["cdep"].ToString();
                string RemarkN = ((TextBox)GVApplyList.FooterRow.FindControl("tbStrRemarkA")).Text;
                string applyidN = string.Format("{0:yyyyMMddHHmmssffff}", DateTime.Now);

                if (NameN == string.Empty)
                {
                    Response.Write("<script>alert('课程名称不可为空')</script>");
                    return;
                }

                sqsApplyList.InsertParameters["Action"].DefaultValue = "insert";
                sqsApplyList.InsertParameters["strName"].DefaultValue = NameN;
                sqsApplyList.InsertParameters["strYearID"].DefaultValue = YearID;
                sqsApplyList.InsertParameters["cdepid"].DefaultValue = CDep;
                sqsApplyList.InsertParameters["strRemark"].DefaultValue = RemarkN;
                sqsApplyList.InsertParameters["applyid"].DefaultValue = applyidN;

                sqsApplyList.Insert();
                GVApplyList.DataBind();
                leftTool.Visible = true;

                ClientScript.RegisterClientScriptBlock(GetType(), "", "<script>alert(\'操作成功!\');setTimeout(function(){location.href='ApplyEdit.aspx'},10);  </script>");
                //Response.Redirect("ApplyEdit.aspx");

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
                int dayW = Convert.ToInt16(((DropDownList)gvTemp.FooterRow.FindControl("ddlDayA")).SelectedValue);
                int startN = Convert.ToInt16(((DropDownList)gvTemp.FooterRow.FindControl("ddlStartNA")).SelectedValue);
                int endN = Convert.ToInt16(((DropDownList)gvTemp.FooterRow.FindControl("ddlEndNA")).SelectedValue);
                string ClassN = ((TextBox)gvTemp.FooterRow.FindControl("tbStrClassA")).Text;
                string TeacherN = ((TextBox)gvTemp.FooterRow.FindControl("tbStrTeacherA")).Text;
                string RemarkN = ((TextBox)gvTemp.FooterRow.FindControl("tbStrRemarkA")).Text;
                string weekReg = ((TextBox)gvTemp.FooterRow.FindControl("tbStrWeekRegA")).Text;
                string weekData = string.Empty;
                string applyid = ((Label)gvTemp.Parent.Parent.FindControl("lbapplyid")).Text;
                string idN = string.Format("{0:yyyyMMddHHmmssffff}", DateTime.Now);

                //通常验证
                string norC = CommonClass.normalCheck(weekReg, startN, endN, dayW, ClassN, TeacherN);
                if (norC != "OK")
                {
                    Response.Write("<script>alert('" + norC + "')</script>");
                    ClientScript.RegisterStartupScript(GetType(), "Expand", "<SCRIPT LANGUAGE='javascript'>expandcollapse('div" + applyid + "','one');</script>");
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
                        ClientScript.RegisterStartupScript(GetType(), "Expand", "<SCRIPT LANGUAGE='javascript'>expandcollapse('div" + applyid + "','one');</script>");
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
                    ClientScript.RegisterStartupScript(GetType(), "Expand", "<SCRIPT LANGUAGE='javascript'>expandcollapse('div" + applyid + "','one');</script>");
                    return;
                }
                                
                //验证重复数据
                string checkmsg = CommonClass.CheckApply(roomN, dayW, startN, endN, weekData, idN, ClassN, TeacherN);
                if (checkmsg != "OK")
                {
                    Response.Write("<script>alert('" + checkmsg + "')</script>");
                    ClientScript.RegisterStartupScript(GetType(), "Expand", "<SCRIPT LANGUAGE='javascript'>expandcollapse('div" + applyid + "','one');</script>");
                    return;
                }

                //Prepare the Insert Command of the DataSource control
                sqsRoomApply.InsertParameters["Action"].DefaultValue = "insert";
                sqsRoomApply.InsertParameters["roomid"].DefaultValue = roomN;
                sqsRoomApply.InsertParameters["intDay"].DefaultValue = dayW.ToString();
                sqsRoomApply.InsertParameters["intStartNum"].DefaultValue = startN.ToString();
                sqsRoomApply.InsertParameters["intEndNum"].DefaultValue = endN.ToString();
                sqsRoomApply.InsertParameters["strWeekReg"].DefaultValue = weekReg;
                sqsRoomApply.InsertParameters["strWeekData"].DefaultValue = weekData;
                sqsRoomApply.InsertParameters["strClass"].DefaultValue = ClassN;
                sqsRoomApply.InsertParameters["strTeacher"].DefaultValue = TeacherN;
                sqsRoomApply.InsertParameters["strRemark"].DefaultValue = RemarkN;
                sqsRoomApply.InsertParameters["applyid"].DefaultValue = applyid;
                sqsRoomApply.InsertParameters["id"].DefaultValue = idN;

                sqsRoomApply.Insert();
                GVApplyList.DataBind();
                leftTool.Visible = true;
                ClientScript.RegisterStartupScript(GetType(), "Expand", "<SCRIPT LANGUAGE='javascript'>expandcollapse('div" + applyid + "','one');</script>");
                Response.Write("<script>alert('操作成功')</script>");
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('" + ex.Message.ToString().Replace("'", "") + "');</script>");
                return;
            }
        }

        if (e.CommandName == "CopySub")
        {
            Session["CopySubFlag"] = 1;
            LinkButton curLB = (LinkButton)e.CommandSource;
            GridViewRow curRow = (GridViewRow)curLB.Parent.Parent;
            int curRowIndex = curRow.RowIndex;
            //GridView gvTemp = (GridView)curLB.Parent.Parent.Parent.Parent;
            GridView gvTemp = (GridView)sender;
            gvUniqueID = gvTemp.UniqueID;

            Session["roomN"] = ((Label)gvTemp.Rows[curRowIndex].FindControl("lbroomid")).Text;
            Session["dayW"] = ((Label)gvTemp.Rows[curRowIndex].FindControl("lbIntDay")).Text.TrimEnd();
            Session["startN"] = ((Label)gvTemp.Rows[curRowIndex].FindControl("lbIntStartNum")).Text.TrimEnd();
            Session["endN"] = ((Label)gvTemp.Rows[curRowIndex].FindControl("lbIntEndNum")).Text.TrimEnd();
            Session["ClassN"] = ((Label)gvTemp.Rows[curRowIndex].FindControl("lbStrClass")).Text.TrimEnd();
            Session["TeacherN"] = ((Label)gvTemp.Rows[curRowIndex].FindControl("lbStrTeacher")).Text.TrimEnd();
            Session["weekReg"] = ((Label)gvTemp.Rows[curRowIndex].FindControl("lbStrWeekReg")).Text.TrimEnd();
            Session["remarkN"] = ((Label)gvTemp.Rows[curRowIndex].FindControl("lbStrRemark")).Text.TrimEnd();

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
        string UsrDep = Session["cdep"].ToString();
        string NameN = ((TextBox)GVApplyList.Rows[e.RowIndex].FindControl("tbStrNameE")).Text;
        if (NameN == string.Empty)
        {
            Response.Write("<script>alert('课程名称不可为空')</script>");
            e.Cancel = true;
            return;
        }

        sqsApplyList.UpdateParameters["Action"].DefaultValue = "update";
        sqsApplyList.UpdateParameters["strName"].DefaultValue = NameN;
        sqsApplyList.UpdateParameters["strYearID"].DefaultValue = CurYear;
        sqsApplyList.UpdateParameters["cdepid"].DefaultValue = UsrDep;
        sqsApplyList.UpdateParameters["strRemark"].DefaultValue = ((TextBox)GVApplyList.Rows[e.RowIndex].FindControl("tbStrRemarkE")).Text;
        sqsApplyList.UpdateParameters["applyid"].DefaultValue = e.Keys["applyid"].ToString();

        sqsApplyList.Update();
        GVApplyList.DataBind();
        leftTool.Visible = true;
        GVApplyList.ShowFooter = true;
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

        string roomN = ((DropDownList)gvTemp.Rows[e.RowIndex].FindControl("ddlRoomGVRAedit")).SelectedValue;
        int dayW = Convert.ToInt16(((DropDownList)gvTemp.Rows[e.RowIndex].FindControl("ddlDayE")).SelectedValue);
        int startN = Convert.ToInt16(((DropDownList)gvTemp.Rows[e.RowIndex].FindControl("ddlStartNE")).SelectedValue);
        int endN = Convert.ToInt16(((DropDownList)gvTemp.Rows[e.RowIndex].FindControl("ddlEndNE")).SelectedValue);
        string ClassN = ((TextBox)gvTemp.Rows[e.RowIndex].FindControl("tbStrClassE")).Text;
        string TeacherN = ((TextBox)gvTemp.Rows[e.RowIndex].FindControl("tbStrTeacherE")).Text;
        string RemarkN = ((TextBox)gvTemp.Rows[e.RowIndex].FindControl("tbStrRemarkE")).Text;
        string weekReg = ((TextBox)gvTemp.Rows[e.RowIndex].FindControl("tbStrWeekRegE")).Text;
        string weekData = string.Empty;        
        //string idN = ((Label)gvTemp.Rows[e.RowIndex].FindControl("lbid")).Text;
        string idN = gvTemp.DataKeys[e.RowIndex]["id"].ToString();

        //通常验证
        string norC = CommonClass.normalCheck(weekReg, startN, endN, dayW, ClassN, TeacherN);
        if (norC != "OK")
        {
            Response.Write("<script>alert('" + norC + "')</script>");
            e.Cancel = true;            
            ClientScript.RegisterStartupScript(GetType(), "Expand", "<SCRIPT LANGUAGE='javascript'>expandcollapse('div" + gvTemp.DataKeys[0].Value.ToString() + "','one');</script>");            
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
                ClientScript.RegisterStartupScript(GetType(), "Expand", "<SCRIPT LANGUAGE='javascript'>expandcollapse('div" + gvTemp.DataKeys[0].Value.ToString() + "','one');</script>");
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
            ClientScript.RegisterStartupScript(GetType(), "Expand", "<SCRIPT LANGUAGE='javascript'>expandcollapse('div" + gvTemp.DataKeys[0].Value.ToString() + "','one');</script>");
            return;
        }        

        //验证重复数据
        string checkmsg = CommonClass.CheckApply(roomN, dayW, startN, endN, weekData, idN, ClassN, TeacherN);
        if (checkmsg != "OK")
        {
            Response.Write("<script>alert('" + checkmsg + "')</script>");
            e.Cancel = true;
            ClientScript.RegisterStartupScript(GetType(), "Expand", "<SCRIPT LANGUAGE='javascript'>expandcollapse('div" + gvTemp.DataKeys[0].Value.ToString() + "','one');</script>");
            return;
        }

        //更新数据
        sqsRoomApply.UpdateParameters["action"].DefaultValue = "update";
        sqsRoomApply.UpdateParameters["roomid"].DefaultValue = roomN;
        sqsRoomApply.UpdateParameters["intDay"].DefaultValue = dayW.ToString();
        sqsRoomApply.UpdateParameters["intStartNum"].DefaultValue = startN.ToString();
        sqsRoomApply.UpdateParameters["intEndNum"].DefaultValue = endN.ToString();
        sqsRoomApply.UpdateParameters["strWeekReg"].DefaultValue = weekReg;
        sqsRoomApply.UpdateParameters["strWeekData"].DefaultValue = weekData;
        sqsRoomApply.UpdateParameters["strClass"].DefaultValue = ClassN;
        sqsRoomApply.UpdateParameters["strTeacher"].DefaultValue = TeacherN;
        sqsRoomApply.UpdateParameters["strRemark"].DefaultValue = RemarkN;
        sqsRoomApply.UpdateParameters["id"].DefaultValue = idN;

        sqsRoomApply.Update();
        leftTool.Visible = true;
        showFooter = 1;
        GVApplyList.DataBind();
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
        GVApplyList.ShowFooter = false;
    }

    protected void GVRoomApply_RowEditing(object sender, GridViewEditEventArgs e)
    {
        GridView gvTemp = (GridView)sender;
        leftTool.Visible = false;
        showFooter = 0;
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
        GVApplyList.ShowFooter = true;
    }

    protected void GVRoomApply_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView gvTemp = (GridView)sender;
        leftTool.Visible = true;
        showFooter = 1;
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

    protected void ddlStartNE_SelectedIndexChanged(object sender, EventArgs e)
    {

        DropDownList ddlStartN = (DropDownList)sender;
        DropDownList ddlEndN = (DropDownList)ddlStartN.Parent.Parent.FindControl("ddlEndNE");

        if ((Convert.ToInt16(ddlStartN.SelectedValue) == 1) || ((Convert.ToInt16(ddlStartN.SelectedValue) == 3)) || ((Convert.ToInt16(ddlStartN.SelectedValue) == 5)) || ((Convert.ToInt16(ddlStartN.SelectedValue) == 7)) || ((Convert.ToInt16(ddlStartN.SelectedValue) == 9)))
        {
            ddlEndN.SelectedValue = (Convert.ToInt16(ddlStartN.SelectedValue) + 1).ToString();
        }
        if ((Convert.ToInt16(ddlStartN.SelectedValue) == 99))
        {
            ddlEndN.SelectedValue = ddlStartN.SelectedValue;
        }
        //子记录保持打开
        GridView gvTemp = (GridView)ddlStartN.Parent.Parent.Parent.Parent;
        ClientScript.RegisterStartupScript(GetType(), "Expand", "<SCRIPT LANGUAGE='javascript'>expandcollapse('div" + gvTemp.DataKeys[0].Value.ToString() + "','one');</script>");

    }

    protected void ddlStartNA_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddlStartN = (DropDownList)sender;
        DropDownList ddlEndN = (DropDownList)ddlStartN.Parent.Parent.FindControl("ddlEndNA");

        if ((Convert.ToInt16(ddlStartN.SelectedValue) == 1) || ((Convert.ToInt16(ddlStartN.SelectedValue) == 3)) || ((Convert.ToInt16(ddlStartN.SelectedValue) == 5)) || ((Convert.ToInt16(ddlStartN.SelectedValue) == 7)) || ((Convert.ToInt16(ddlStartN.SelectedValue) == 9)))
        {
            ddlEndN.SelectedValue = (Convert.ToInt16(ddlStartN.SelectedValue) + 1).ToString();
        }
        if ((Convert.ToInt16(ddlStartN.SelectedValue) == 99))
        {
            ddlEndN.SelectedValue = ddlStartN.SelectedValue;
        }
        //子记录保持打开
        string strApplyid = ((Label)ddlStartN.Parent.Parent.Parent.Parent.Parent.Parent.FindControl("lbApplyid")).Text;
        ClientScript.RegisterStartupScript(GetType(), "Expand", "<SCRIPT LANGUAGE='javascript'>expandcollapse('div" + strApplyid + "','one');</script>");

    }

    protected void GVRoomApply_PreRender(object sender, EventArgs e)
    {
        GridView gvTemp = (GridView)sender;
        //check empty row
        if (gvTemp.Rows.Count == 0)
        {
            renderEmptyGridView(gvTemp, "applyid, id,roomid, strRoomName, intDay, intStartNum, intEndNum, strClass, strTeacher, strWeekReg, strWeekData, strRemark");
        }
        
        if ((Convert.ToInt16(Session["CopySubFlag"]) == 1)/*&&(gvTemp.UniqueID == gvUniqueID)*/)
        {            
            ((DropDownList)gvTemp.FooterRow.FindControl("ddlRoomGVRAadd")).SelectedValue = Session["roomN"].ToString();
            ((DropDownList)gvTemp.FooterRow.FindControl("ddlDayA")).SelectedValue = Session["dayW"].ToString();
            ((DropDownList)gvTemp.FooterRow.FindControl("ddlStartNA")).SelectedValue = Session["startN"].ToString();
            ((DropDownList)gvTemp.FooterRow.FindControl("ddlEndNA")).SelectedValue = Session["endN"].ToString();
            ((TextBox)gvTemp.FooterRow.FindControl("tbStrClassA")).Text = Session["ClassN"].ToString();
            ((TextBox)gvTemp.FooterRow.FindControl("tbStrTeacherA")).Text = Session["TeacherN"].ToString(); ;
            ((TextBox)gvTemp.FooterRow.FindControl("tbStrWeekRegA")).Text = Session["weekReg"].ToString();
            ((TextBox)gvTemp.FooterRow.FindControl("tbStrRemarkA")).Text = Session["remarkN"].ToString();
            //ClientScript.RegisterStartupScript(GetType(), "Expand", "<SCRIPT LANGUAGE='javascript'>expandcollapse('div" + gvTemp.DataKeys[0].Value.ToString() + "','one');</script>");
            //Session["CopySubFlag"] = 0;
        }
        if (gvTemp.UniqueID == gvUniqueID)
        {
            ClientScript.RegisterStartupScript(GetType(), "Expand", "<SCRIPT LANGUAGE='javascript'>expandcollapse('div" + gvTemp.DataKeys[0].Value.ToString() + "','one');</script>");
        }


    }

    protected void GVApplyList_PreRender(object sender, EventArgs e)
    {
        if (GVApplyList.Rows.Count == 0)
        {
            renderEmptyGridView(GVApplyList, "applyid, strName, yearID, cdepid, strRemark");
            //Response.Write("<script>alert('找不到数据，请检查筛选条件或重新打开页面（不要刷新）')</script>");
        }
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
        tbSearch.Text = string.Empty;
        //reset select
        int CDep = Convert.ToInt16(Session["cdep"]);
        sqsApplyList.SelectParameters.Clear();
        sqsApplyList.SelectCommand = String.Format("SELECT l.applyid,RTRIM(l.strName) as strName,l.yearID,l.cdepid,l.strRemark FROM ApplyList l,TitleStartEnd t WHERE l.yearID = t.yearID and t.currentFlag = 'true' and cdepid = {0}  order by applyid desc", CDep);
        ViewState["ApplyListSelStr"] = sqsApplyList.SelectCommand;
        sqsApplyList.DataBind();
        
        for (int i = 0; i < GVApplyList.Rows.Count; i++)
        {
            SqlDataSource sqsRoomApply;
            sqsRoomApply = GVApplyList.Rows[i].FindControl("sqsRoomApply") as SqlDataSource;            
            sqsRoomApply.SelectCommand = string.Format("SELECT a.id, a.applyid, a.roomid,d.strRoomName, a.intDay, a.intStartNum, a.intEndNum, RTRIM(a.strClass) as strClass, RTRIM(a.strTeacher) as strTeacher, a.strWeekReg, a.strWeekData, a.strRemark FROM RoomApply a,RoomDetail d WHERE a.applyid = @applyid and a.roomid = d.roomid order by a.id desc");
            ViewState["roomapplySelStr"] = sqsRoomApply.SelectCommand;
            sqsRoomApply.DataBind();
        }
        
    }

    protected void btTotalSearch_Click(object sender, EventArgs e)
    {
        string sRoom = string.Empty;
        foreach (ListItem li in liboRoom.Items)
        {
            if (li.Selected == true)
                sRoom += li.Value.Trim() + ",";
        }

        if (sRoom != string.Empty)
        {
            sRoom = sRoom.Substring(0, sRoom.Length - 1); // chop off trailing ,   
        }
        else
        {
            foreach (ListItem li in liboRoom.Items)
            {
                sRoom += li.Value.Trim() + ",";
            }
            sRoom = sRoom.Substring(0, sRoom.Length - 1); // chop off trailing , 
        }

        string sWeek = string.Empty;
        foreach (ListItem li in liboWeek.Items)
        {
            if (li.Selected == true)
                sWeek += li.Value.Trim() + ",";
        }

        if (sWeek != string.Empty)
        {
            sWeek = sWeek.Substring(0, sWeek.Length - 1); // chop off trailing ,   
        }
        else
        {
            foreach (ListItem li in liboWeek.Items)
            {
                sWeek += li.Value.Trim() + ",";
            }
            sWeek = sWeek.Substring(0, sWeek.Length - 1); // chop off trailing , 
        }

        string sDay = string.Empty;
        foreach (ListItem li in liboDay.Items)
        {
            if (li.Selected == true)
                sDay += li.Value.Trim() + ",";
        }

        if (sDay != string.Empty)
        {
            sDay = sDay.Substring(0, sDay.Length - 1); // chop off trailing ,   
        }
        else
        {
            foreach (ListItem li in liboDay.Items)
            {
                sDay += li.Value.Trim() + ",";
            }
            sDay = sDay.Substring(0, sDay.Length - 1); // chop off trailing , 
        }

        string sTextbox = tbSearch.Text;
        if (sTextbox == string.Empty)
        {
            sTextbox = "init";
        }
        //主记录筛选
        int CDep = Convert.ToInt16(Session["cdep"]);
        sqsApplyList.SelectParameters.Clear();
        sqsApplyList.SelectCommand = String.Format("SELECT distinct l.applyid,RTRIM(l.strName) as strName,l.yearID,l.cdepid,l.strRemark FROM ApplyList l, RoomApply a, RoomApplySub s, RoomDetail d, TitleStartEnd w WHERE l.cdepid = {0} and l.applyid = a.applyid and a.id = s.F_id and a.roomid = d.roomid and l.yearID = w.yearID and w.currentFlag = 'true' and a.roomid in ({1}) and s.intWeek in ({2}) and a.intDay in ({3}) and ((l.strName like '%{4}%') or (a.strTeacher like '%{4}%') or (a.strClass like '%{4}%') or ('{4}' = 'init')) order by l.applyid desc", CDep,sRoom,sWeek,sDay,sTextbox);
        ViewState["ApplyListSelStr"] = sqsApplyList.SelectCommand;
        sqsApplyList.DataBind();
        if (GVApplyList.DataSourceID == string.Empty)
        {
            GVApplyList.DataSource = sqsApplyList;
        }


        //子记录筛选
        for (int i = 0; i < GVApplyList.Rows.Count; i++)
        {
            SqlDataSource sqsRoomApply;
            sqsRoomApply = GVApplyList.Rows[i].FindControl("sqsRoomApply") as SqlDataSource;
            //sqsRoomApply.SelectParameters["applyid"].DefaultValue = (GVApplyList.Rows[i].DataItem as DataRowView)["applyid"].ToString();
            sqsRoomApply.SelectCommand = string.Format("SELECT distinct a.id, a.applyid,a.roomid, d.strRoomName, a.intDay, a.intStartNum, a.intEndNum, RTRIM(l.strName) as strName, RTRIM(a.strClass) as strClass, RTRIM(a.strTeacher) as strTeacher,a.strWeekReg, a.strWeekData, a.strRemark FROM RoomApply a, RoomApplySub s, RoomDetail d, TitleStartEnd w, ApplyList l WHERE a.applyid = @applyid and a.id = s.F_id and a.roomid = d.roomid and a.applyid = l.applyid and l.yearID = w.yearID and w.currentFlag = 'true' and a.roomid in ({0}) and s.intWeek in ({1}) and a.intDay in ({2}) and ((l.strName like '%{3}%') or (a.strTeacher like '%{3}%') or (a.strClass like '%{3}%') or ('{3}' = 'init')) order by a.id desc", sRoom, sWeek, sDay, sTextbox);
            ViewState["roomapplySelStr"] = sqsRoomApply.SelectCommand;
            sqsRoomApply.DataBind();
        }
        GVApplyList.DataBind();
    }

    #endregion

    


    
}
