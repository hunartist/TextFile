using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class objectQuery : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        int weekNum = CommonClass.getCurrentWeek();
        if (Page.IsPostBack == false)
        {
            ddlWeek1.SelectedValue = weekNum.ToString();
            ddlWeek2.SelectedValue = weekNum.ToString();
        }
    }

    protected void btSearch_Click(object sender, EventArgs e)
    {
        string depid = ddlDepart.SelectedValue.ToString();
        string roomid = ddlRoom.SelectedValue.ToString();
        string roomname = ddlRoom.SelectedItem.Text.TrimEnd();
        int week1 = Convert.ToInt16(ddlWeek1.SelectedValue);
        int week2 = Convert.ToInt16(ddlWeek2.SelectedValue);
        string sqlStr = "select RTRIM(t.currentFlag) as currentFlag  ,dp.strDepart ,aaa.* from(select aa.*,w.datePeriod,w.intWeek as wWeek ,w.yearID as wYear,d.strRoomName,d.depid from (select l.applyid,a.id,RTRIM(l.strName) as strName,RTRIM(a.strClass) as strClass,RTRIM(a.strTeacher) as strTeacher,a.strRemark ,l.yearID, a.roomid, a.intDay, a.intStartNum, a.intEndNum, a.strWeekReg, a.strWeekData, s.intweek from ApplyList l inner join RoomApply a on l.applyid = a.applyid inner join RoomApplySub s on a.id = s.F_id  where a.roomid = '" + roomid + "' and s.intWeek >= " + week1 + " and s.intWeek <= " + week2 + " ) as aa	right join WeekStartEnd w on w.yearID = aa.yearID right join RoomDetail d on d.depid =  '" + depid + "' where d.roomid =  '" + roomid + "' and w.intWeek = aa.intWeek and d.roomid = aa.roomid) as aaa left join TitleStartEnd t on aaa.wYear = t.yearID inner join Department dp on aaa.depid = dp.depid where t.currentFlag = 'true'";
        string printStr = "NCT";

        Label lbTitle = new Label();
        lbTitle.ID = roomname + "Title";
        lbTitle.Text = roomname;
        GridViewPlaceHolder.Controls.Add(lbTitle);

        for (int i = week1; i <= week2; i++)
        {
            Label lbWeek = new Label();
            lbWeek.ID = "week" + i;
            lbWeek.Text = "第" + i + "周";
            GridViewPlaceHolder.Controls.Add(lbWeek);

            DataTable dt = PrintTabClass.PrintTab_Week_DT(i, sqlStr, printStr);

            GridView gvTemp = new GridView();
            gvTemp.ID = "gv" + i;
            gvTemp.HorizontalAlign = HorizontalAlign.Center;
            gvTemp = PrintTabClass.DynamicGenerateColumns(gvTemp, dt);
            gvTemp.DataSource = dt;
            gvTemp.DataBind();
            GridViewPlaceHolder.Controls.Add(gvTemp);

            HtmlGenericControl spanTemp = new HtmlGenericControl("div");//创建一个span标签
            spanTemp.ID = "div" + i;
            spanTemp.Style.Add("page-break-before", "always");//属性
            GridViewPlaceHolder.Controls.Add(spanTemp); //添加到页面             
        }

    }

    protected void btSearch2_Click(object sender, EventArgs e)
    {
        string depid = ddlDepart.SelectedValue.ToString();
        string roomid = ddlRoom.SelectedValue.ToString();
        string classname = tbClass.Text;
        string roomname = ddlRoom.SelectedItem.Text.TrimEnd();
        int week1 = Convert.ToInt16(ddlWeek1.SelectedValue);
        int week2 = Convert.ToInt16(ddlWeek2.SelectedValue);
        string sqlStr = "select RTRIM(t.currentFlag) as currentFlag  ,dp.strDepart ,aaa.* from(select aa.*,w.datePeriod,w.intWeek as wWeek ,w.yearID as wYear,d.strRoomName,d.depid from (select l.applyid,a.id,RTRIM(l.strName) as strName,RTRIM(a.strClass) as strClass,RTRIM(a.strTeacher) as strTeacher,a.strRemark ,l.yearID, a.roomid, a.intDay, a.intStartNum, a.intEndNum, a.strWeekReg, a.strWeekData, s.intweek from ApplyList l inner join RoomApply a on l.applyid = a.applyid inner join RoomApplySub s on a.id = s.F_id  where a.strClass like '%" + classname + "%' and s.intWeek >= " + week1 + " and s.intWeek <= " + week2 + " ) as aa	right join WeekStartEnd w on w.yearID = aa.yearID right join RoomDetail d on d.roomid = aa.roomid where aa.strClass like  '%" + classname + "%' and w.intWeek = aa.intWeek and  d.roomid = aa.roomid) as aaa left join TitleStartEnd t on aaa.wYear = t.yearID inner join Department dp on aaa.depid = dp.depid where t.currentFlag = 'true'";
        string printStr = "RNT";

        Label lbTitle = new Label();
        lbTitle.ID = classname + "Title";
        lbTitle.Text = classname;
        GridViewPlaceHolder.Controls.Add(lbTitle);

        for (int i = week1; i <= week2; i++)
        {
            Label lbWeek = new Label();
            lbWeek.ID = "week" + i;
            lbWeek.Text = "第" + i + "周";
            GridViewPlaceHolder.Controls.Add(lbWeek);

            DataTable dt = PrintTabClass.PrintTab_Week_DT(i, sqlStr, printStr);

            GridView gvTemp = new GridView();
            gvTemp.ID = "gv" + i;
            gvTemp.HorizontalAlign = HorizontalAlign.Center;
            gvTemp = PrintTabClass.DynamicGenerateColumns(gvTemp, dt);
            gvTemp.DataSource = dt;
            gvTemp.DataBind();
            GridViewPlaceHolder.Controls.Add(gvTemp);

            HtmlGenericControl spanTemp = new HtmlGenericControl("div");//创建一个span标签
            spanTemp.ID = "div" + i;
            spanTemp.Style.Add("page-break-before", "always");//属性
            GridViewPlaceHolder.Controls.Add(spanTemp); //添加到页面             
        }

    }

    protected void btSearch3_Click(object sender, EventArgs e)
    {
        string depid = ddlDepart.SelectedValue.ToString();
        string roomid = ddlRoom.SelectedValue.ToString();
        string teachername = tbTeacher.Text;
        string roomname = ddlRoom.SelectedItem.Text.TrimEnd();
        int week1 = Convert.ToInt16(ddlWeek1.SelectedValue);
        int week2 = Convert.ToInt16(ddlWeek2.SelectedValue);
        string sqlStr = "select RTRIM(t.currentFlag) as currentFlag  ,dp.strDepart ,aaa.* from(select aa.*,w.datePeriod,w.intWeek as wWeek ,w.yearID as wYear,d.strRoomName,d.depid from (select l.applyid,a.id,RTRIM(l.strName) as strName,RTRIM(a.strClass) as strClass,RTRIM(a.strTeacher) as strTeacher,a.strRemark ,l.yearID, a.roomid, a.intDay, a.intStartNum, a.intEndNum, a.strWeekReg, a.strWeekData, s.intweek from ApplyList l inner join RoomApply a on l.applyid = a.applyid inner join RoomApplySub s on a.id = s.F_id  where a.strTeacher like '%" + teachername + "%' and s.intWeek >= " + week1 + " and s.intWeek <= " + week2 + " ) as aa	right join WeekStartEnd w on w.yearID = aa.yearID right join RoomDetail d on d.roomid = aa.roomid where aa.strTeacher like  '%" + teachername + "%' and w.intWeek = aa.intWeek and d.roomid = aa.roomid) as aaa left join TitleStartEnd t on aaa.wYear = t.yearID inner join Department dp on aaa.depid = dp.depid where t.currentFlag = 'true'";
        string printStr = "RNC";

        Label lbTitle = new Label();
        lbTitle.ID = teachername + "Title";
        lbTitle.Text = teachername;
        GridViewPlaceHolder.Controls.Add(lbTitle);

        for (int i = week1; i <= week2; i++)
        {
            Label lbWeek = new Label();
            lbWeek.ID = "week" + i;
            lbWeek.Text = "第" + i + "周";
            GridViewPlaceHolder.Controls.Add(lbWeek);

            DataTable dt = PrintTabClass.PrintTab_Week_DT(i, sqlStr, printStr);

            GridView gvTemp = new GridView();
            gvTemp.ID = "gv" + i;
            gvTemp.HorizontalAlign = HorizontalAlign.Center;
            gvTemp = PrintTabClass.DynamicGenerateColumns(gvTemp, dt);
            gvTemp.DataSource = dt;
            gvTemp.DataBind();
            GridViewPlaceHolder.Controls.Add(gvTemp);

            HtmlGenericControl spanTemp = new HtmlGenericControl("div");//创建一个span标签
            spanTemp.ID = "div" + i;
            spanTemp.Style.Add("page-break-before", "always");//属性
            GridViewPlaceHolder.Controls.Add(spanTemp); //添加到页面             
        }

    }
}