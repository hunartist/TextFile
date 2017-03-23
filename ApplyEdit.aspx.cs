﻿using System;
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
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)    //Page.IsPostBack
        {
            if (user.redirectSet(Convert.ToString(Session["user"])))
                Response.Redirect("tempLogin.aspx");

        }
        LabelMsg.Visible = false;
    }




    protected void GridView10_RowUpdated(object sender, GridViewUpdatedEventArgs e)
    {
        //SqlDataSourceRoomApply.UpdateCommand = "UPDATE [RoomApply] SET [strRoom] = @strRoom , [intDay] = @intDay , [intStartNum] = @intStartNum , [intEndNum] = @intEndNum , [intStartWeek] = @intStartWeek , [intEndWeek] = @intEndWeek , [strName] = @strName , [strClass] = @strClass , [strTeacher] = @strTeacher WHERE [id] = @original_id";
        
        SqlDataSourceRoomApply.UpdateParameters["action"].DefaultValue = "update";
        SqlDataSourceRoomApply.UpdateParameters["strRoom"].DefaultValue = e.NewValues["strRoom"].ToString();
        SqlDataSourceRoomApply.UpdateParameters["intDay"].DefaultValue = e.NewValues["intDay"].ToString();
        SqlDataSourceRoomApply.UpdateParameters["intStartNum"].DefaultValue = e.NewValues["intStartNum"].ToString();
        SqlDataSourceRoomApply.UpdateParameters["intEndNum"].DefaultValue = e.NewValues["intEndNum"].ToString();
        SqlDataSourceRoomApply.UpdateParameters["intStartWeek"].DefaultValue = e.NewValues["intStartWeek"].ToString();
        SqlDataSourceRoomApply.UpdateParameters["intEndWeek"].DefaultValue = e.NewValues["intEndWeek"].ToString();
        SqlDataSourceRoomApply.UpdateParameters["strName"].DefaultValue = Convert.ToString(e.NewValues["strName"]);
        SqlDataSourceRoomApply.UpdateParameters["strClass"].DefaultValue = Convert.ToString(e.NewValues["strClass"]);
        SqlDataSourceRoomApply.UpdateParameters["strTeacher"].DefaultValue = Convert.ToString(e.NewValues["strTeacher"]);
        SqlDataSourceRoomApply.UpdateParameters["id"].DefaultValue = LabelID.Text;

        SqlDataSourceRoomApply.Update();
        GridView10.DataBind();
        LabelMsg.Visible = false;
        DropDownListDepart.Visible = true;
        Response.Write("<script>alert('操作成功')</script>");
    }

    protected void GridView10_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        LabelID.Text = GridView10.Rows[e.NewSelectedIndex].Cells[1].Text;
    }

    protected void GridView10_RowEditing(object sender, GridViewEditEventArgs e)
    {
        LabelID.Text = GridView10.Rows[e.NewEditIndex].Cells[1].Text;
        DropDownListDepart.Visible = false;
    }

    protected void GridView10_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        RegexStringValidator regday = new RegexStringValidator("^[1-7]{1}$");
        RegexStringValidator regnum = new RegexStringValidator("^[1-9]{1}$|10");

        try
        {
            regday.Validate(e.NewValues["intDay"]);
        }
        catch
        {
            LabelMsg.Visible = true;
            LabelMsg.Text = "日期只能填数字1至数字7";
            e.Cancel = true;
        }
        try
        {
            regnum.Validate(e.NewValues["intStartNum"]);
            regnum.Validate(e.NewValues["intEndNum"]);
        }
        catch
        {
            LabelMsg.Visible = true;
            LabelMsg.Text = "节次只能填数字1至数字10";
            e.Cancel = true;
        }
        if (Convert.ToInt16(e.NewValues["intStartNum"])> Convert.ToInt16(e.NewValues["intEndNum"]))
        {
            LabelMsg.Visible = true;
            LabelMsg.Text = "开始节次不能大于结束节次";
            e.Cancel = true;
        }
        if (Convert.ToInt16(e.NewValues["intStartWeek"]) > Convert.ToInt16(e.NewValues["intEndWeek"]))
        {
            LabelMsg.Visible = true;
            LabelMsg.Text = "开始周不能大于结束周";
            e.Cancel = true;
        }

        string NameN = Convert.ToString(e.NewValues["strName"]);
        string ClassN = Convert.ToString(e.NewValues["strClass"]);
        string TeacherN = Convert.ToString(e.NewValues["strTeacher"]);
        if (NameN == "")
        {
            LabelMsg.Visible = true;
            LabelMsg.Text = "课程名未填写";
            e.Cancel = true;            
        }
        if (ClassN == "")
        {
            LabelMsg.Visible = true;
            LabelMsg.Text = "班级未填写";
            e.Cancel = true;            
        }
        if (TeacherN == "")
        {
            LabelMsg.Visible = true;
            LabelMsg.Text = "教师未填写";
            e.Cancel = true;            
        }

        string roomN = e.NewValues["strRoom"].ToString();
        int dayW = Convert.ToInt16(e.NewValues["intDay"]);
        int newSN = Convert.ToInt16(e.NewValues["intStartNum"]);
        int newEN = Convert.ToInt16(e.NewValues["intEndNum"]);
        string idN = e.Keys["id"].ToString();
        string checkmsg = CommonClass.CheckApply(roomN, dayW, newSN, newEN, idN);

        if (checkmsg != "OK")
        {
            LabelMsg.Visible = true;
            LabelMsg.Text = checkmsg;
            e.Cancel = true;
        }
                    
                    
    }


    protected void GridView10_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        DropDownListDepart.Visible = true;
    }

    protected void GridView10_RowDeleted(object sender, GridViewDeletedEventArgs e)
    {
        SqlDataSourceRoomApply.DeleteParameters["action"].DefaultValue = "delete";
        SqlDataSourceRoomApply.DeleteParameters["strRoom"].DefaultValue = null;
        SqlDataSourceRoomApply.DeleteParameters["intDay"].DefaultValue = null;
        SqlDataSourceRoomApply.DeleteParameters["intStartNum"].DefaultValue = null;
        SqlDataSourceRoomApply.DeleteParameters["intEndNum"].DefaultValue = null;
        SqlDataSourceRoomApply.DeleteParameters["intStartWeek"].DefaultValue = null;
        SqlDataSourceRoomApply.DeleteParameters["intEndWeek"].DefaultValue = null;
        SqlDataSourceRoomApply.DeleteParameters["strName"].DefaultValue = null;
        SqlDataSourceRoomApply.DeleteParameters["strClass"].DefaultValue = null;
        SqlDataSourceRoomApply.DeleteParameters["strTeacher"].DefaultValue = null;
        SqlDataSourceRoomApply.DeleteParameters["id"].DefaultValue = e.Keys["id"].ToString();

        SqlDataSourceRoomApply.Delete();
        GridView10.DataBind();
        LabelMsg.Visible = false;
        DropDownListDepart.Visible = true;
        Response.Write("<script>alert('操作成功')</script>");
    }

}
