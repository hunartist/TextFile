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

    protected DataTable PrintTab(int weekNum, string departmentName)
    {
        SqlConnection con = CommonClass.GetSqlConnection();
        SqlDataAdapter sda = new SqlDataAdapter();
        sda.SelectCommand = new SqlCommand("select * from (select distinct s.intWeek, a.strRoom,a.intDay,a.intStartNum,a.intEndNum from RoomApply a inner join RoomApplySub s on a.id = s.F_id) as aa inner join RoomDetail d on aa.strRoom = d.strRoomName where aa.intWeek = '" + weekNum + "' and d.strDepart= '" + departmentName + "'", con);
        DataSet ds = new DataSet();
        sda.Fill(ds);
        DataTable table = new DataTable();
        table = ds.Tables[0];
        DataTable dtSchedule = new DataTable();

        
        //添加八列
        dtSchedule.Columns.Add("查询");
        for (int i = 1; i < 8; i++)
        {
            dtSchedule.Columns.Add(Convert.ToString(i));
        }

        //添加10行
        for (int i = 0; i < 10; i++)
        {
            dtSchedule.Rows.Add();
        }

        //添加左侧固定信息（第几节）
        for (int i = 0; i < 10; i++)
        {
            dtSchedule.Rows[i][0] = (i + 1);
        }

        //此数组用于存放需要合并的单元格信息。如：需要合并第一列的一、二单元格
        //那么，数组中一行的三个数分别为1,1,2
        int[][] tempArray = new int[table.Rows.Count][];
        for (int i = 0; i < table.Rows.Count; i++)
        {
            tempArray[i] = new int[3];
            for (int j = 0; j < 3; j++)
            {
                tempArray[i][j] = 0;
            }
        }

        //遍历table，将每条课表信息填在tab中适当的位置。
        for (int i = 0; i < table.Rows.Count; i++)
        {
            //Day
            string week = Convert.ToString(table.Rows[i]["intDay"]);
            //StartNum
            string startNum = Convert.ToString(table.Rows[i]["intStartNum"]);
            //EndNum
            string endNum = Convert.ToString(table.Rows[i]["intEndNum"]);

            for (int weekCount = 1; weekCount < 8; weekCount++)//确定本条数据将来显示在哪一列
            {
                if (week == Convert.ToString(dtSchedule.Columns[weekCount].ColumnName))//跟星期做比较，确定数据应该写在那一列
                {
                    tempArray[i][0] = weekCount;//记录星期（确定将来的数据显示在哪一列）
                    break;
                }
            }

            for (int j = 0; j < dtSchedule.Rows.Count; j++)//确定课程的开始时间和结束时间，并填写数据
            {
                string section = Convert.ToString(dtSchedule.Rows[j][0]);//当前行是第几节课
                if (section == startNum)//判断课程开始时间，确定位置，填写数据
                {
                    tempArray[i][1] = j;//记录上课开始时间（确定数据显示在哪一行）
                    dtSchedule.Rows[j][tempArray[i][0]] = dtSchedule.Rows[j][tempArray[i][0]].ToString() + Convert.ToString(table.Rows[i]["strRoom"]) + "<br />";
                }
                if (section == endNum)//判断课程结束时间，记录位置
                {
                    tempArray[i][2] = j;//记录课结束时间
                    break;
                }
            }
        }

        //修改行标题
        for (int i = 1; i < 8; i++)
        {
            dtSchedule.Columns[i].ColumnName = "星期" + WeekConvertToChinese(i);
        }
        //修改列标题
        for (int i = 0; i < 10; i++)
        {
            dtSchedule.Rows[i][0] = "第" + ConvertToChinese(i + 1) + "节";
        }

        //dispose        
        table.Dispose();
        ds.Dispose();
        sda.Dispose();
        con.Dispose();

        return dtSchedule;

        

    }

    string ConvertToChinese(int x)
    {
        string cstr = "";
        switch (x)
        {
            case 0: cstr = "零"; break;
            case 1: cstr = "一"; break;
            case 2: cstr = "二"; break;
            case 3: cstr = "三"; break;
            case 4: cstr = "四"; break;
            case 5: cstr = "五"; break;
            case 6: cstr = "六"; break;
            case 7: cstr = "七"; break;
            case 8: cstr = "八"; break;
            case 9: cstr = "九"; break;
            case 10: cstr = "十"; break;
        }
        return (cstr);
    }
    //转换星期几
    string WeekConvertToChinese(int x)
    {
        string cstr = "";
        switch (x)
        {
            case 1: cstr = "一"; break;
            case 2: cstr = "二"; break;
            case 3: cstr = "三"; break;
            case 4: cstr = "四"; break;
            case 5: cstr = "五"; break;
            case 6: cstr = "六"; break;
            case 7: cstr = "日"; break;
        }
        return (cstr);
    }



    protected void btSearch_Click(object sender, EventArgs e)
    {
        DataBindRepeater();        
    }

    private void DataBindRepeater()
    {
        this.Repeater1.DataSource = PrintTab(Convert.ToInt16(ddlWeek.SelectedValue), ddlDep.SelectedValue);
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