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
        if(ViewState["selectCom_fil"] != null)
        {
            //SqlDataSourceRoomApply.SelectCommand = ViewState["selectCom_fil"].ToString();
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
                }
                //Expand the Child grid
                ClientScript.RegisterStartupScript(GetType(), "Expand", "<SCRIPT LANGUAGE='javascript'>expandcollapse('div" + ((DataRowView)e.Row.DataItem)["applyid"].ToString() + "','one');</script>");
            }
            gv.DataSource = sqsRoomApply;
            gv.DataBind();
        }

        //ClientScript.RegisterStartupScript(GetType(), "Expand", "<SCRIPT LANGUAGE='javascript'>expandcollapse('div" + ((DataRowView)e.Row.DataItem)["applyid"].ToString() + "','one');</script>");
        
    }

    protected void GVApplyList_RowDeleted(object sender, GridViewDeletedEventArgs e)
    {
        sqsApplyList.DeleteParameters["Action"].DefaultValue = "delete"; 
        sqsApplyList.DeleteParameters["strName"].DefaultValue = null;
        sqsApplyList.DeleteParameters["strYearID"].DefaultValue = null;
        sqsApplyList.DeleteParameters["strCDep"].DefaultValue = null;
        sqsApplyList.DeleteParameters["strRemark"].DefaultValue = null;
        sqsApplyList.DeleteParameters["applyid"].DefaultValue = e.Keys["applyid"].ToString();

        sqsApplyList.Delete();
        GVApplyList.DataBind();
        leftTool.Visible = true;
        Response.Write("<script>alert('操作成功')</script>");
    }

    protected void GVRoomApply_RowDeleted(object sender, GridViewDeletedEventArgs e)
    {
        //sqsRoomApply.DeleteParameters["action"].DefaultValue = "delete";
        //sqsRoomApply.DeleteParameters["strRoom"].DefaultValue = null;
        //sqsRoomApply.DeleteParameters["intDay"].DefaultValue = null;
        //sqsRoomApply.DeleteParameters["intStartNum"].DefaultValue = null;
        //sqsRoomApply.DeleteParameters["intEndNum"].DefaultValue = null;
        //sqsRoomApply.DeleteParameters["strWeekReg"].DefaultValue = null;
        //sqsRoomApply.DeleteParameters["strWeekData"].DefaultValue = null;
        //sqsRoomApply.DeleteParameters["strName"].DefaultValue = null;
        //sqsRoomApply.DeleteParameters["strClass"].DefaultValue = null;
        //sqsRoomApply.DeleteParameters["strTeacher"].DefaultValue = null;
        //sqsRoomApply.DeleteParameters["id"].DefaultValue = e.Keys["id"].ToString();

        //sqsRoomApply.Delete();
        //GVRoomApply.DataBind();
        //leftTool.Visible = true;
        //Response.Write("<script>alert('操作成功')</script>");
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


        sqsRoomApply.UpdateParameters["action"].DefaultValue = "update";
        sqsRoomApply.UpdateParameters["strRoom"].DefaultValue = ((DropDownList)gvTemp.Rows[e.RowIndex].FindControl("ddlRoomGVRA")).SelectedValue;
        sqsRoomApply.UpdateParameters["intDay"].DefaultValue = ((TextBox)gvTemp.Rows[e.RowIndex].FindControl("tbIntDayE")).Text;
        sqsRoomApply.UpdateParameters["intStartNum"].DefaultValue = ((TextBox)gvTemp.Rows[e.RowIndex].FindControl("tbIntStartNumE")).Text;
        sqsRoomApply.UpdateParameters["intEndNum"].DefaultValue = ((TextBox)gvTemp.Rows[e.RowIndex].FindControl("tbIntEndNumE")).Text;
        sqsRoomApply.UpdateParameters["strWeekReg"].DefaultValue = ((TextBox)gvTemp.Rows[e.RowIndex].FindControl("tbStrWeekRegE")).Text;
        sqsRoomApply.UpdateParameters["strWeekData"].DefaultValue = ((Label)gvTemp.Rows[e.RowIndex].FindControl("lbStrWeekData")).Text;
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
        //string CurYear = CommonClass.getCurYearID();
        //string UsrDep = Session["dep"].ToString();

        //sqsApplyList.UpdateParameters["Action"].DefaultValue = "update";
        //sqsApplyList.UpdateParameters["strName"].DefaultValue = Convert.ToString(e.NewValues["strName"]);
        //sqsApplyList.UpdateParameters["strYearID"].DefaultValue = CurYear;
        //sqsApplyList.UpdateParameters["strCDep"].DefaultValue = UsrDep;
        //sqsApplyList.UpdateParameters["strRemark"].DefaultValue = Convert.ToString(e.NewValues["strRemark"]);
        //sqsApplyList.UpdateParameters["applyid"].DefaultValue = e.Keys["applyid"].ToString();

        //sqsApplyList.Update();
        //GVApplyList.DataBind();
        //leftTool.Visible = true;
        //Response.Write("<script>alert('操作成功')</script>");
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

    }

    protected void GVRoomApply_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

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
        //ViewState["selectCom_fil"] = SqlDataSourceRoomApply.SelectCommand;
        //GridView10.DataBind();
    }

    #endregion





}
