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
        LabelID.Visible = false;
        LabelMsg.Visible = false;
        if(ViewState["selectCom_fil"] != null)
        {
            SqlDataSourceRoomApply.SelectCommand = ViewState["selectCom_fil"].ToString();
        }
        
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
        SqlDataSourceRoomApply.UpdateParameters["id"].DefaultValue = e.Keys["id"].ToString();

        SqlDataSourceRoomApply.Update();
        GridView10.DataBind();
        LabelMsg.Visible = false;
        btDepFlit.Visible = true;
        Response.Write("<script>alert('操作成功')</script>");
    }

    protected void GridView10_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        LabelID.Text = GridView10.Rows[e.NewSelectedIndex].Cells[1].Text;
    }

    protected void GridView10_RowEditing(object sender, GridViewEditEventArgs e)
    {
        LabelID.Text = GridView10.Rows[e.NewEditIndex].Cells[1].Text;
        btDepFlit.Visible = false;
        if (ViewState["selectCom_fil"] != null)
        {
            SqlDataSourceRoomApply.SelectCommand = ViewState["selectCom_fil"].ToString();
            GridView10.DataBind();
        }
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
        int newSW = Convert.ToInt16(e.NewValues["intStartWeek"]);
        int newEW = Convert.ToInt16(e.NewValues["intEndWeek"]);
        string idN = e.Keys["id"].ToString();
        string checkmsg = CommonClass.CheckApply(roomN, dayW, newSN, newEN, newSW, newEW, idN);

        if (checkmsg != "OK")
        {
            LabelMsg.Visible = true;
            LabelMsg.Text = checkmsg;
            e.Cancel = true;
        }
                    
                    
    }


    protected void GridView10_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        btDepFlit.Visible = true;
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
        btDepFlit.Visible = true;
        Response.Write("<script>alert('操作成功')</script>");
    }

    protected void btDepFlit_Click(object sender, EventArgs e)
    {
        SqlDataSourceRoomApply.SelectParameters.Clear();
        SqlDataSourceRoomApply.SelectCommand = "select distinct a.id,a.strRoom,a.intDay,a.intStartNum,a.intEndNum,a.intStartWeek,a.intEndWeek,RTRIM(a.strName) as strName,RTRIM(a.strClass) as strClass,RTRIM(a.strTeacher) as strTeacher,a.yearID from RoomApply a ,RoomDetail d,TitleStartEnd w where a.strRoom = d.strRoomName  and a.yearID = w.yearID and w.currentFlag = 'true' and d.strDepart = @depN_CP order by a.id desc";
        ControlParameter depN_CP = new ControlParameter();
        depN_CP.Name = "depN_CP";
        depN_CP.Type = TypeCode.String;
        depN_CP.ControlID = "DropDownListDepart";
        depN_CP.PropertyName = "SelectedValue";
        SqlDataSourceRoomApply.SelectParameters.Add(depN_CP);
        ViewState["selectCom_fil"] = SqlDataSourceRoomApply.SelectCommand;
        GridView10.DataBind();
    }

    protected void btWeekFlit_Click(object sender, EventArgs e)
    {
        string s = string.Empty;
        foreach (ListItem li in liboRoom.Items)
        {
            if (li.Selected == true)
                s += "'" + li.Value.Trim() + "',";
        }

        if (s != string.Empty)
        {
            s = s.Substring(0, s.Length - 1); // chop off trailing ,   
        }
        else
        {
            foreach (ListItem li in liboRoom.Items)
            {
                s += "'" + li.Value.Trim() + "',";                
            }
            s = s.Substring(0, s.Length - 1); // chop off trailing , 
        }

        SqlDataSourceRoomApply.SelectParameters.Clear();
        //SqlDataSourceRoomApply.SelectCommand = "select distinct a.id,a.strRoom,a.intDay,a.intStartNum,a.intEndNum,a.intStartWeek,a.intEndWeek,RTRIM(a.strName) as strName,RTRIM(a.strClass) as strClass,RTRIM(a.strTeacher) as strTeacher,a.yearID from RoomApply a ,RoomDetail d,TitleStartEnd w where a.strRoom = d.strRoomName  and a.yearID = w.yearID and w.currentFlag = 'true' and a.intStartWeek <= @week_CP and a.intEndWeek >= @week_CP and d.strDepart = @depN_CP and a.strRoom in (@room_P) order by a.id desc";
        SqlDataSourceRoomApply.SelectCommand = String.Format("select distinct a.id,a.strRoom,a.intDay,a.intStartNum,a.intEndNum,a.intStartWeek,a.intEndWeek,RTRIM(a.strName) as strName,RTRIM(a.strClass) as strClass,RTRIM(a.strTeacher) as strTeacher,a.yearID from RoomApply a ,RoomDetail d,TitleStartEnd w where a.strRoom = d.strRoomName  and a.yearID = w.yearID and w.currentFlag = 'true' and a.intStartWeek <= @week_CP and a.intEndWeek >= @week_CP and d.strDepart = @depN_CP and a.strRoom in ({0}) order by a.id desc", s);
        ControlParameter week_CP = new ControlParameter();
        week_CP.Name = "week_CP";
        week_CP.Type = TypeCode.Int16;
        week_CP.ControlID = "ddlWeek";
        week_CP.PropertyName = "SelectedValue";
        SqlDataSourceRoomApply.SelectParameters.Add(week_CP);
        ControlParameter depN_CP = new ControlParameter();
        depN_CP.Name = "depN_CP";
        depN_CP.Type = TypeCode.String;
        depN_CP.ControlID = "DropDownListDepart";
        depN_CP.PropertyName = "SelectedValue";
        SqlDataSourceRoomApply.SelectParameters.Add(depN_CP);
        ViewState["selectCom_fil"] = SqlDataSourceRoomApply.SelectCommand;
        GridView10.DataBind();

        //Parameter room_P = new Parameter();
        //room_P.Name = "room_P";
        //room_P.Type = TypeCode.String;
        //room_P.DefaultValue = Session["liboxSel"].ToString();
        //SqlDataSourceRoomApply.SelectParameters.Add(room_P);
        //ViewState["selectCom_fil"] = SqlDataSourceRoomApply.SelectCommand;
        //GridView10.DataBind();
    }

    protected void btAbandon_Click(object sender, EventArgs e)
    {
        Session.Abandon();
        Response.Redirect("templogin.aspx");
    }

    //protected void liboRoom_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    string s = string.Empty;
    //    foreach (ListItem li in liboRoom.Items)
    //    {
    //        if (li.Selected == true)
    //            s += "'"+li.Value.Trim() + "',";
    //    }

    //    if (s != string.Empty)
    //    {
    //        s = s.Substring(0, s.Length - 1); // chop off trailing ,
    //        Session["liboxSel"] += s;

    //    }
    //}

    protected void btRoomAll_Click(object sender, EventArgs e)
    {
        foreach (ListItem li in liboRoom.Items)
        {
            li.Selected = true;
        }
    }

    protected void btRoomNone_Click(object sender, EventArgs e)
    {
        foreach (ListItem li in liboRoom.Items)
        {
            li.Selected = false;
        }
    }

    protected void btRoomFlit_Click(object sender, EventArgs e)
    {
        string s = string.Empty;
        foreach (ListItem li in liboRoom.Items)
        {
            if (li.Selected == true)
                s += "'" + li.Value.Trim() + "',";
        }

        if (s != string.Empty)
        {
            s = s.Substring(0, s.Length - 1); // chop off trailing ,   
        }
        else
        {
            foreach (ListItem li in liboRoom.Items)
            {
                s += "'" + li.Value.Trim() + "',";
            }
            s = s.Substring(0, s.Length - 1); // chop off trailing , 
        }


        SqlDataSourceRoomApply.SelectParameters.Clear();
        //SqlDataSourceRoomApply.SelectCommand = "select distinct a.id,a.strRoom,a.intDay,a.intStartNum,a.intEndNum,a.intStartWeek,a.intEndWeek,RTRIM(a.strName) as strName,RTRIM(a.strClass) as strClass,RTRIM(a.strTeacher) as strTeacher,a.yearID from RoomApply a ,RoomDetail d,TitleStartEnd w where a.strRoom = d.strRoomName  and a.yearID = w.yearID and w.currentFlag = 'true' and a.intStartWeek <= @week_CP and a.intEndWeek >= @week_CP and d.strDepart = @depN_CP and a.strRoom in (@room_P) order by a.id desc";
        SqlDataSourceRoomApply.SelectCommand = String.Format("select distinct a.id,a.strRoom,a.intDay,a.intStartNum,a.intEndNum,a.intStartWeek,a.intEndWeek,RTRIM(a.strName) as strName,RTRIM(a.strClass) as strClass,RTRIM(a.strTeacher) as strTeacher,a.yearID from RoomApply a ,RoomDetail d,TitleStartEnd w where a.strRoom = d.strRoomName  and a.yearID = w.yearID and w.currentFlag = 'true' and a.strRoom in ({0}) order by a.id desc", s);        
        ViewState["selectCom_fil"] = SqlDataSourceRoomApply.SelectCommand;
        GridView10.DataBind();
    }
}
