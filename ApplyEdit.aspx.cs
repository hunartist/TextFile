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
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)    //Page.IsPostBack
        {
            if (user.redirectSet(Convert.ToString(Session["user"])))
            { Response.Redirect("tempLogin.aspx"); }

            if (Session["ddlDep"] != null)
            {
                DropDownListDepart.SelectedValue = Session["ddlDep"].ToString();
            }

        }
        //LabelID.Visible = true;
        //LabelMsg.Visible = false;
        if(ViewState["selectCom_fil"] != null)
        {
            SqlDataSourceRoomApply.SelectCommand = ViewState["selectCom_fil"].ToString();
        }

        LiteralID.Text = "";
        GridView10.DataKeyNames = new string[] { "id" };
        

    }




    protected void GridView10_RowUpdated(object sender, GridViewUpdatedEventArgs e)
    {        
        SqlDataSourceRoomApply.UpdateParameters["action"].DefaultValue = "update";
        SqlDataSourceRoomApply.UpdateParameters["strRoom"].DefaultValue = e.NewValues["strRoom"].ToString();
        SqlDataSourceRoomApply.UpdateParameters["intDay"].DefaultValue = e.NewValues["intDay"].ToString();
        SqlDataSourceRoomApply.UpdateParameters["intStartNum"].DefaultValue = e.NewValues["intStartNum"].ToString();
        SqlDataSourceRoomApply.UpdateParameters["intEndNum"].DefaultValue = e.NewValues["intEndNum"].ToString();
        SqlDataSourceRoomApply.UpdateParameters["strWeekReg"].DefaultValue = e.NewValues["strWeekReg"].ToString();
        SqlDataSourceRoomApply.UpdateParameters["strWeekData"].DefaultValue = e.NewValues["strWeekData"].ToString();
        SqlDataSourceRoomApply.UpdateParameters["strName"].DefaultValue = Convert.ToString(e.NewValues["strName"]);
        SqlDataSourceRoomApply.UpdateParameters["strClass"].DefaultValue = Convert.ToString(e.NewValues["strClass"]);
        SqlDataSourceRoomApply.UpdateParameters["strTeacher"].DefaultValue = Convert.ToString(e.NewValues["strTeacher"]);
        SqlDataSourceRoomApply.UpdateParameters["id"].DefaultValue = e.Keys["id"].ToString();

        SqlDataSourceRoomApply.Update();
        GridView10.DataBind();
        btDepFlit.Visible = true;
        leftTool.Visible = true;
        Response.Write("<script>alert('操作成功')</script>");
    }

    protected void GridView10_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        //LabelID.Text = GridView10.Rows[e.NewSelectedIndex].Cells[1].Text;
    }

    protected void GridView10_RowEditing(object sender, GridViewEditEventArgs e)
    {
        LiteralID.Text = GridView10.DataKeys[e.NewEditIndex]["id"].ToString();
        leftTool.Visible = false;
        if (ViewState["selectCom_fil"] != null)
        {
            SqlDataSourceRoomApply.SelectCommand = ViewState["selectCom_fil"].ToString();
            GridView10.DataBind();
        }
    }

    protected void GridView10_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        if (e.NewValues["strWeekReg"] == null)
        {
            Response.Write("<script>alert('周输入不可为空')</script>");
            e.Cancel = true;
            return;
        }
        string regData = "ini";
        //验证reg到data的转换
        bool rTdFalg = CommonClass.regToData(e.NewValues["strWeekReg"].ToString(), out regData);
        if (rTdFalg == true)
        {
            if (regData == "ini")
            {
                Response.Write("<script>alert('data=ini')</script>");
                e.Cancel = true;
                return;
            }
            else
            {
                e.NewValues["strWeekData"] = regData;
            }
        }
        if (rTdFalg == false)
        {
            Response.Write("<script>alert('"+ regData + "')</script>");
            e.Cancel = true;
            return;
        }



        RegexStringValidator regday = new RegexStringValidator("^[1-7]{1}$");
        RegexStringValidator regnum = new RegexStringValidator("^[1-9]{1}$|10");

        try
        {
            regday.Validate(e.NewValues["intDay"]);
        }
        catch
        {
            //LabelMsg.Visible = true;
            //LabelMsg.Text = "日期只能填数字1至数字7";
            Response.Write("<script>alert('日期只能填数字1至数字7')</script>");
            e.Cancel = true;
            return;
        }
        try
        {
            regnum.Validate(e.NewValues["intStartNum"]);
            regnum.Validate(e.NewValues["intEndNum"]);
        }
        catch
        {            
            Response.Write("<script>alert('节次只能填数字1至数字10')</script>");
            e.Cancel = true;
            return;
        }
        if (Convert.ToInt16(e.NewValues["intStartNum"])> Convert.ToInt16(e.NewValues["intEndNum"]))
        {            
            Response.Write("<script>alert('开始节次不能大于结束节次')</script>");
            e.Cancel = true;
            return;
        }

        string NameN = Convert.ToString(e.NewValues["strName"]);
        string ClassN = Convert.ToString(e.NewValues["strClass"]);
        string TeacherN = Convert.ToString(e.NewValues["strTeacher"]);
        if (NameN == "")
        {
            Response.Write("<script>alert('课程名未填写')</script>");
            e.Cancel = true;
            return;
        }
        if (ClassN == "")
        {
            Response.Write("<script>alert('班级未填写')</script>");
            e.Cancel = true;
            return;
        }
        if (TeacherN == "")
        {
            Response.Write("<script>alert('教师未填写')</script>");
            e.Cancel = true;
            return;
        }

        string roomN = e.NewValues["strRoom"].ToString();
        int dayW = Convert.ToInt16(e.NewValues["intDay"]);
        int newSN = Convert.ToInt16(e.NewValues["intStartNum"]);
        int newEN = Convert.ToInt16(e.NewValues["intEndNum"]);
        string weekData = e.NewValues["strWeekData"].ToString();        
        string idN = e.Keys["id"].ToString();
        string checkmsg = CommonClass.CheckApply(roomN, dayW, newSN, newEN, weekData, idN);

        if (checkmsg != "OK")
        {
            Response.Write("<script>alert('" + checkmsg + "')</script>");
            e.Cancel = true;
            return;
        }


    }


    protected void GridView10_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        btDepFlit.Visible = true;
        leftTool.Visible = true;
        LiteralID.Text = "";
    }

    protected void GridView10_RowDeleted(object sender, GridViewDeletedEventArgs e)
    {
        SqlDataSourceRoomApply.DeleteParameters["action"].DefaultValue = "delete";
        SqlDataSourceRoomApply.DeleteParameters["strRoom"].DefaultValue = null;
        SqlDataSourceRoomApply.DeleteParameters["intDay"].DefaultValue = null;
        SqlDataSourceRoomApply.DeleteParameters["intStartNum"].DefaultValue = null;
        SqlDataSourceRoomApply.DeleteParameters["intEndNum"].DefaultValue = null;
        SqlDataSourceRoomApply.DeleteParameters["strWeekReg"].DefaultValue = null;
        SqlDataSourceRoomApply.DeleteParameters["strWeekData"].DefaultValue = null;
        SqlDataSourceRoomApply.DeleteParameters["strName"].DefaultValue = null;
        SqlDataSourceRoomApply.DeleteParameters["strClass"].DefaultValue = null;
        SqlDataSourceRoomApply.DeleteParameters["strTeacher"].DefaultValue = null;
        SqlDataSourceRoomApply.DeleteParameters["id"].DefaultValue = e.Keys["id"].ToString();

        SqlDataSourceRoomApply.Delete();
        GridView10.DataBind();
        //LabelMsg.Visible = false;
        btDepFlit.Visible = true;
        leftTool.Visible = true;
        Response.Write("<script>alert('操作成功')</script>");
    }

    protected void btDepFlit_Click(object sender, EventArgs e)
    {
        SqlDataSourceRoomApply.SelectParameters.Clear();
        SqlDataSourceRoomApply.SelectCommand = "select distinct a.id,a.strRoom,a.intDay,a.intStartNum,a.intEndNum,a.strWeekReg,a.strWeekData,RTRIM(a.strName) as strName,RTRIM(a.strClass) as strClass,RTRIM(a.strTeacher) as strTeacher,a.yearID from RoomApply a ,RoomDetail d,TitleStartEnd w where a.strRoom = d.strRoomName  and a.yearID = w.yearID and w.currentFlag = 'true' and d.strDepart = @depN_CP order by a.id desc";
        ControlParameter depN_CP = new ControlParameter();
        depN_CP.Name = "depN_CP";
        depN_CP.Type = TypeCode.String;
        depN_CP.ControlID = "DropDownListDepart";
        depN_CP.PropertyName = "SelectedValue";
        SqlDataSourceRoomApply.SelectParameters.Add(depN_CP);
        ViewState["selectCom_fil"] = SqlDataSourceRoomApply.SelectCommand;
        GridView10.DataBind();
    }
 

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
    }

    protected void btliboNone_Click(object sender, EventArgs e)
    {
        foreach (ListItem li in liboRoom.Items)
        {
            li.Selected = false;
        }
        foreach(ListItem li in liboWeek.Items)
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
        SqlDataSourceRoomApply.SelectCommand = String.Format("select distinct a.id,a.strRoom,a.intDay,a.intStartNum,a.intEndNum,a.strWeekReg,a.strWeekData,RTRIM(a.strName) as strName,RTRIM(a.strClass) as strClass,RTRIM(a.strTeacher) as strTeacher,a.yearID from RoomApply a ,RoomDetail d,TitleStartEnd w where a.strRoom = d.strRoomName  and a.yearID = w.yearID and w.currentFlag = 'true' and a.strRoom in ({0}) order by a.id desc", s);        
        ViewState["selectCom_fil"] = SqlDataSourceRoomApply.SelectCommand;
        GridView10.DataBind();
    }

    protected void btTotalSearch_Click(object sender, EventArgs e)
    {
        string sRoom = string.Empty;
        foreach (ListItem li in liboRoom.Items)
        {
            if (li.Selected == true)
                sRoom += "'" + li.Value.Trim() + "',";
        }

        if (sRoom != string.Empty)
        {
            sRoom = sRoom.Substring(0, sRoom.Length - 1); // chop off trailing ,   
        }
        else
        {
            foreach (ListItem li in liboRoom.Items)
            {
                sRoom += "'" + li.Value.Trim() + "',";
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

        SqlDataSourceRoomApply.SelectParameters.Clear();
        SqlDataSourceRoomApply.SelectCommand = String.Format("select distinct a.id,a.strRoom,a.intDay,a.intStartNum,a.intEndNum,a.strWeekReg,a.strWeekData,RTRIM(a.strName) as strName,RTRIM(a.strClass) as strClass,RTRIM(a.strTeacher) as strTeacher,a.yearID from RoomApply a ,RoomDetail d,TitleStartEnd w,RoomApplySub s where a.strRoom = d.strRoomName  and a.yearID = w.yearID and w.currentFlag = 'true' and a.id = s.F_id and d.strDepart = @depN_CP and a.strRoom in ({0}) and s.intWeek in ({1}) and ((a.strName like '%'+ @searchTextBox_CP + '%') or (a.strTeacher like '%'+ @searchTextBox_CP + '%') or (a.strClass like '%'+ @searchTextBox_CP + '%') or (@searchTextBox_CP = 'init')) order by a.id desc", sRoom , sWeek);
        ControlParameter searchTextBox_CP = new ControlParameter();
        searchTextBox_CP.Name = "searchTextBox_CP";
        searchTextBox_CP.Type = TypeCode.String;
        searchTextBox_CP.ControlID = "tbSearch";
        searchTextBox_CP.PropertyName = "Text";
        searchTextBox_CP.DefaultValue = "init";
        SqlDataSourceRoomApply.SelectParameters.Add(searchTextBox_CP);
        ControlParameter depN_CP = new ControlParameter();
        depN_CP.Name = "depN_CP";
        depN_CP.Type = TypeCode.String;
        depN_CP.ControlID = "DropDownListDepart";
        depN_CP.PropertyName = "SelectedValue";
        SqlDataSourceRoomApply.SelectParameters.Add(depN_CP);
        ViewState["selectCom_fil"] = SqlDataSourceRoomApply.SelectCommand;
        GridView10.DataBind();
    }

    protected void DropDownListDepart_SelectedIndexChanged(object sender, EventArgs e)
    {
        Session["ddlDep"] = DropDownListDepart.SelectedValue.ToString();
    }
}
