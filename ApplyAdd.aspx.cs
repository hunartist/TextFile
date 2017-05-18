using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ApplyAdd : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)    //Page.IsPostBack
        {
            if (user.redirectSet(Convert.ToString(Session["user"])))
            { Response.Redirect("tempLogin.aspx"); }

            if (Session["addRoom"] != null)
            { ddlRoom.Text = Session["addRoom"].ToString(); }
            if (Session["addDay"] != null)
            { ddlDay.Text = Session["addDay"].ToString(); }
            if (Session["addSN"] != null)
            { ddlStartN.Text = Session["addSN"].ToString(); }
            if (Session["addEN"] != null)
            { ddlEndN.Text = Session["addEN"].ToString(); }
            if (Session["addweekReg"] != null)
            { tbWeekReg.Text = Session["addweekReg"].ToString(); }
            if (Session["addweekData"] != null)
            { tbWeekData.Text = Session["addweekData"].ToString(); }
            if (Session["addName"] != null)
            { tbName.Text = Session["addName"].ToString(); }
            if (Session["addClass"] != null)
            { tbClass.Text = Session["addClass"].ToString(); }
            if (Session["addTeacher"] != null)
            { tbTeacher.Text = Session["addTeacher"].ToString(); }            
        }        
    }

    protected void ButtonSave_Click(object sender, EventArgs e)
    {
        string roomN = ddlRoom.Text;
        int dayW = Convert.ToInt16(ddlDay.Text);
        int startN = Convert.ToInt16(ddlStartN.Text);
        int endN = Convert.ToInt16(ddlEndN.Text);
        string weekReg = tbWeekReg.Text;
        string weekData = tbWeekData.Text;
        string idN = string.Format("{0:yyyyMMddHHmmssffff}", DateTime.Now);    

        string NameN = tbName.Text;
        string ClassN = tbClass.Text;
        string TeacherN = tbTeacher.Text;

        SqlDataSourceRoomApply.InsertParameters["action"].DefaultValue = "insert";
        SqlDataSourceRoomApply.InsertParameters["strRoom"].DefaultValue = ddlRoom.Text;
        SqlDataSourceRoomApply.InsertParameters["intDay"].DefaultValue = ddlDay.Text;
        SqlDataSourceRoomApply.InsertParameters["intStartNum"].DefaultValue = ddlStartN.Text;
        SqlDataSourceRoomApply.InsertParameters["intEndNum"].DefaultValue = ddlEndN.Text;
        SqlDataSourceRoomApply.InsertParameters["strWeekReg"].DefaultValue = tbWeekReg.Text;
        SqlDataSourceRoomApply.InsertParameters["strWeekData"].DefaultValue = tbWeekData.Text;
        SqlDataSourceRoomApply.InsertParameters["strName"].DefaultValue = tbName.Text;
        SqlDataSourceRoomApply.InsertParameters["strClass"].DefaultValue = tbClass.Text;
        SqlDataSourceRoomApply.InsertParameters["strTeacher"].DefaultValue = tbTeacher.Text;
        SqlDataSourceRoomApply.InsertParameters["strYearID"].DefaultValue = ddlYear.Text;        
        SqlDataSourceRoomApply.InsertParameters["id"].DefaultValue = idN;

        if (weekReg == null)
        {
            Response.Write("<script>alert('周输入不可为空')</script>");            
            return;
        }
        string regData = "ini";
        //验证reg到data的转换
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
                SqlDataSourceRoomApply.InsertParameters["strWeekData"].DefaultValue = weekData;
            }
        }
        if (rTdFalg == false)
        {
            Response.Write("<script>alert('" + regData + "')</script>");            
            return;
        }

        if (startN > endN)
        {
            Response.Write("<script>alert('开始节次不能大于结束节次')</script>");
            return ;
        }
        
        if (NameN == "")
        {
            Response.Write("<script>alert('课程名未填写')</script>");
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

        string checkmsg = CommonClass.CheckApply(roomN, dayW, startN, endN, weekData, idN, ClassN, TeacherN);
        if (checkmsg != "OK")
        {
            Response.Write("<script>alert('" + checkmsg + "')</script>");
            return;
        }

        SqlDataSourceRoomApply.Insert();
        string strMsg = "操作成功，是否继续增加，选择“确定”继续增加，选择“取消”进入编辑界面";
        string strUrl_Yes = "ApplyAdd.aspx";
        string strUrl_No = "ApplyEdit.aspx";
        Response.Write("<Script Language='JavaScript'>if ( window.confirm('" + strMsg + "')) {  window.location.href='" + strUrl_Yes +
                                "' } else {window.location.href='" + strUrl_No + "' };</script>");

        Session["addRoom"] = ddlRoom.Text;
        Session["addDay"] = ddlDay.Text;
        Session["addSN"] = ddlStartN.Text;
        Session["addEN"] = ddlEndN.Text;
        Session["addweekReg"] = tbWeekReg.Text;
        Session["addweekData"] = tbWeekData.Text;        
        Session["addName"] = tbName.Text;
        Session["addClass"] = tbClass.Text;
        Session["addTeacher"] = tbTeacher.Text;        
       
    }



    protected void ddlStartN_SelectedIndexChanged(object sender, EventArgs e)
    {
        if((Convert.ToInt16(ddlStartN.SelectedValue) == 1) || ((Convert.ToInt16(ddlStartN.SelectedValue) == 3 )) || ((Convert.ToInt16(ddlStartN.SelectedValue) == 5)) || ((Convert.ToInt16(ddlStartN.SelectedValue) == 7)) || ((Convert.ToInt16(ddlStartN.SelectedValue) == 9)))
        {
            ddlEndN.SelectedValue = (Convert.ToInt16(ddlStartN.SelectedValue) + 1).ToString();
        }
        if ((Convert.ToInt16(ddlStartN.SelectedValue) == 99))
        {
            ddlEndN.SelectedValue = ddlStartN.SelectedValue;
        }
    }
}