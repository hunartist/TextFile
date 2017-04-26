using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class emptyRoom : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack == false)
        {
            int CweekNum = CommonClass.getCurrentWeek();
            int maxWeek = CommonClass.getCurMaxWeek();

            if (Session["ERdep"] != null) { ddlDep.SelectedValue = Session["ERdep"].ToString(); }
            if (Session["ERweek1"] != null) { ddlWeek.SelectedValue = Session["ERweek1"].ToString(); }
            else { ddlWeek.SelectedValue = CweekNum.ToString(); }
            if (Session["ERweek2"] != null) { ddlWeek2.SelectedValue = Session["ERweek2"].ToString(); }
            else { ddlWeek2.SelectedValue = maxWeek.ToString(); }
            if (Session["ERday"] != null) { ddlDay.SelectedValue = Session["ERday"].ToString(); }
            if (Session["ERnum1"] != null) { ddlNum1.SelectedValue = Session["ERnum1"].ToString(); }
            if (Session["ERnum2"] != null) { ddlNum2.SelectedValue = Session["ERnum2"].ToString(); }

        }
    }

    protected void btSearch_Click(object sender, EventArgs e)
    {
        if (Convert.ToInt16(ddlWeek2.SelectedValue) < (Convert.ToInt16(ddlWeek.SelectedValue)))
        {
            ddlWeek2.SelectedValue = ddlWeek.SelectedValue;
        }
        if (Convert.ToInt16(ddlNum2.SelectedValue) < (Convert.ToInt16(ddlNum1.SelectedValue)))
        {
            ddlNum2.SelectedValue = ddlNum1.SelectedValue;
        }

        string dep = ddlDep.SelectedValue.ToString();
        int week1 = Convert.ToInt16(ddlWeek.SelectedValue);
        int week2 = Convert.ToInt16(ddlWeek2.SelectedValue);
        int day = Convert.ToInt16(ddlDay.SelectedValue);
        int num1 = Convert.ToInt16(ddlNum1.SelectedValue);
        int num2 = Convert.ToInt16(ddlNum2.SelectedValue);
        

        this.Repeater1.DataSource = getRoomTab(dep, week1, week2, day, num1, num2);
        this.Repeater1.DataBind();

        for (int i = 1; i <= 1; i++) // 遍历每一列
        {
            string tdTd = "td";
            string tdIdName = tdTd + i.ToString();
            MergeCell(tdIdName); // 把当前列的td的ID文本作为方法的参数
        }

        Session["ERdep"] = ddlDep.SelectedValue.ToString();
        Session["ERweek1"] = ddlWeek.SelectedValue.ToString();
        Session["ERweek2"] = ddlWeek2.SelectedValue.ToString();
        Session["ERday"] = ddlDay.SelectedValue.ToString();
        Session["ERnum1"] = ddlNum1.SelectedValue.ToString();
        Session["ERnum2"] = ddlNum2.SelectedValue.ToString();
    }

    protected DataTable getRoomTab(string depT,int week1T,int week2T,int dayT,int num1T,int num2T)
    {
        string yearid = CommonClass.getCurYearID();
        SqlConnection con = CommonClass.GetSqlConnection();
        SqlDataAdapter sdaRoom = new SqlDataAdapter();
        sdaRoom.SelectCommand = new SqlCommand("select distinct RTRIM(d.strRoomName) as strRoomName,w.intWeek,'0123456789' as num from RoomDetail d right join WeekStartEnd w on 1=1 and w.yearID = '" + yearid + "' where d.strDepart = '" + depT + "' and w.intWeek >= "+week1T+" and w.intWeek <= "+week2T, con);
        DataSet dsRoom = new DataSet();
        sdaRoom.Fill(dsRoom);
        DataTable roomTable = new DataTable();
        roomTable = dsRoom.Tables[0];

        SqlDataAdapter sda = new SqlDataAdapter();
        sda.SelectCommand = new SqlCommand("select aaa.strRoom,aaa.intWeek,aaa.intStartNum,aaa.intEndNum from (select  aa.*,d.strRoomName,d.strDepart,d.strCDep from (select distinct s.intWeek ,RTRIM(a.strRoom) as strRoom, a.intDay,a.intStartNum,a.intEndNum,a.yearID ,a.strWeekReg from RoomApply a inner join RoomApplySub s on a.id = s.F_id) as aa inner join RoomDetail d on aa.strRoom = d.strRoomName where d.strDepart= '" + depT + "' and aa.intWeek >= " + week1T + " and aa.intWeek <= " + week2T + " and aa.intDay = " + dayT + " and (( aa.intStartNum  >= " + num1T + " and aa.intStartNum <= " + num2T + " ) or(aa.intEndNum  >= " + num1T + " and aa.intEndNum <= " + num2T + " ) or((aa.intStartNum  < " + num1T + " and aa.intEndNum > " + num2T + " )))) as aaa inner join TitleStartEnd t on aaa.yearID = t.yearID and t.currentFlag = 'true' order by 1", con);
        DataSet ds = new DataSet();
        sda.Fill(ds);
        DataTable table = new DataTable();
        table = ds.Tables[0];

        for (int i = 0;i < table.Rows.Count;i++)
        {
            int SN = Convert.ToInt16(table.Rows[i]["intStartNum"])-1;
            int EN = Convert.ToInt16(table.Rows[i]["intEndNum"]) - 1;            
            DataRow[] roomRows = roomTable.Select("strRoomName = " + table.Rows[i]["strRoom"] + " and intWeek = "+table.Rows[i]["intWeek"]);
            for (int j =SN;j<=EN;j++)
            {
                roomRows[0]["num"] = roomRows[0]["num"].ToString().Replace(j.ToString(), "");
            }
            
        }

        DataTable roomRowsFlited = new DataTable();
        for (int i = num1T-1;i<=num2T-1;i++)
        {
            DataRow[] roomRowsNumFlit = roomTable.Select("num not like '%" + i + "%'");
            foreach (DataRow row in roomRowsNumFlit)
            {
                row.Delete();
            }
        } 

        return roomTable;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="tdIdName">当前列当前行的 td 的ID文本</param>
    private void MergeCell(string tdIdName)
    {
        for (int i = Repeater1.Items.Count - 1; i > 0; i--) // Repeater1.Items.Count - 1 数据总行数（数据从0开始）  遍历当前列的每一行
        {
            MergeCellSet(tdIdName, i);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="tdIdName1">当前列当前行的 td 的ID文本</param>
    /// <param name="i">当前行</param>
    private void MergeCellSet(string tdIdName1, int i)
    {
        HtmlTableCell cellPrev = Repeater1.Items[i - 1].FindControl(tdIdName1) as HtmlTableCell; // 获取下一行当前列的 td 所在的单元格
        HtmlTableCell cell = Repeater1.Items[i].FindControl(tdIdName1) as HtmlTableCell; // 获取当前行当前列的 td 所在的单元格
        cell.RowSpan = (cell.RowSpan == -1) ? 1 : cell.RowSpan; // 获取当前行当前列单元格跨越的行数 
        cellPrev.RowSpan = (cellPrev.RowSpan == -1) ? 1 : cellPrev.RowSpan; // 获取下一行当前列单元格跨越的行数 
        if (cell.InnerText == cellPrev.InnerText)
        {
            // 让下一行的当前单元格的跨越行数 + 当前行的跨越行数
            cellPrev.RowSpan += cell.RowSpan;
            cell.Visible = false;  // 隐藏当前行

            //关键代码，再判断执行第2列的合并单元格方法
        }
    }

}