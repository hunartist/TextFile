﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ApplyAdd : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void ButtonSave_Click(object sender, EventArgs e)
    {
        string roomN = ddlRoom.Text;
        int dayW = Convert.ToInt16(ddlDay.Text);
        int startN = Convert.ToInt16(ddlStartN.Text);
        int endN = Convert.ToInt16(ddlEndN.Text);
        int startW = Convert.ToInt16(ddlStartW.Text);
        int endW = Convert.ToInt16(ddlEndW.Text);
        string idN = string.Format("{0:yyyyMMddHHmmssffff}", DateTime.Now);

        SqlDataSourceRoomApply.InsertParameters["action"].DefaultValue = "insert";
        SqlDataSourceRoomApply.InsertParameters["strRoom"].DefaultValue = ddlRoom.Text;
        SqlDataSourceRoomApply.InsertParameters["intDay"].DefaultValue = ddlDay.Text;
        SqlDataSourceRoomApply.InsertParameters["intStartNum"].DefaultValue = ddlStartN.Text;
        SqlDataSourceRoomApply.InsertParameters["intEndNum"].DefaultValue = ddlEndN.Text;
        SqlDataSourceRoomApply.InsertParameters["intStartWeek"].DefaultValue = ddlStartW.Text;
        SqlDataSourceRoomApply.InsertParameters["intEndWeek"].DefaultValue = ddlEndW.Text;
        SqlDataSourceRoomApply.InsertParameters["strName"].DefaultValue = tbName.Text;
        SqlDataSourceRoomApply.InsertParameters["strClass"].DefaultValue = tbClass.Text;
        SqlDataSourceRoomApply.InsertParameters["strTeacher"].DefaultValue = tbTeacher.Text;
        SqlDataSourceRoomApply.InsertParameters["id"].DefaultValue = idN;

        
        if (startN > endN)
        {
            Response.Write("<script>alert('开始节次不能大于结束节次')</script>");
            return ;
        }
        if(startW > endW)
        {
            Response.Write("<script>alert('开始周不能大于结束周')</script>");
            return;
        }

        string checkmsg = CommonClass.CheckApply(roomN, dayW, startN, endN, idN);
        if (checkmsg != "OK")
        {
            Response.Write("<script>alert('"+checkmsg+"')</script>");
            return;
        }

        SqlDataSourceRoomApply.Insert();
        string strMsg = "操作成功，选择“确定”进入查询界面，选择“取消”进入编辑界面";
        string strUrl_Yes = "RoomApply.aspx";
        string strUrl_No = "ApplyEdit.aspx";
        Response.Write("<Script Language='JavaScript'>if ( window.confirm('" + strMsg + "')) {  window.location.href='" + strUrl_Yes +
                                "' } else {window.location.href='" + strUrl_No + "' };</script>");
    }
}