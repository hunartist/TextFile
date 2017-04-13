using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Text.RegularExpressions;
using System.Web.UI.HtmlControls;

public partial class roomQuery : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //PrintTab(7, "gvTest", "多媒体");
        int weekNum = CommonClass.getCurrentWeek();        

        if (Page.IsPostBack == false)
        {
            ddlWeek.SelectedValue = weekNum.ToString();
        }
        
    }

    protected void btSearch_Click(object sender, EventArgs e)
    {
        DataBindRepeater();        
    }

    private void DataBindRepeater()
    {
        int prtWeekNum = Convert.ToInt16(ddlWeek.SelectedValue);
        string prtDep = ddlDep.SelectedValue;
        string prtsql = "select aaa.*,t.currentFlag from (select  aa.*,d.strRoomName,d.strDepart,d.strCDep from (select distinct s.intWeek, a.strRoom,a.intDay,a.intStartNum,a.intEndNum,a.yearID from RoomApply a inner join RoomApplySub s on a.id = s.F_id) as aa inner join RoomDetail d on aa.strRoom = d.strRoomName where aa.intWeek = " + prtWeekNum + " and d.strDepart= '" + prtDep + "') as aaa inner join TitleStartEnd t on aaa.yearID = t.yearID and t.currentFlag = 'true'";
        this.Repeater1.DataSource = PrintTabClass.PrintTab_SUM_DT(prtWeekNum, prtDep, prtsql);
        this.Repeater1.DataBind();

        for (int i = 1; i <= 7; i++) // 遍历每一列
        {
            string tdTd = "td";
            string tdIdName = tdTd + i.ToString();
            MergeCell(tdIdName); // 把当前列的td的ID文本作为方法的参数
        }
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