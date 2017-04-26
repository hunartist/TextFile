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

public partial class roomQueryReverse : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //PrintTab(7, "gvTest", "多媒体");
           

        if (Page.IsPostBack == false)
        {
            int CweekNum = CommonClass.getCurrentWeek();
            int maxWeek = CommonClass.getCurMaxWeek();
            //ddlWeek.SelectedValue = CweekNum.ToString();
            //ddlWeek2.SelectedValue = maxWeek.ToString();

            if (Session["QRdep"] != null) { ddlDep.SelectedValue = Session["QRdep"].ToString(); }
            if (Session["QRweek"] != null) { ddlWeek.SelectedValue = Session["QRweek"].ToString();}
            else { ddlWeek.SelectedValue = CweekNum.ToString(); }
            if (Session["QRnum1"] != null) { ddlNum1.SelectedValue = Session["QRnum1"].ToString(); }
            if (Session["QRnum2"] != null) { ddlNum2.SelectedValue = Session["QRnum2"].ToString(); }

        }

    }

    protected DataTable PrintTab(int weekNum,int startNum1,int endNum2, string departmentName)
    {
        SqlConnection con = CommonClass.GetSqlConnection();
        SqlDataAdapter sda = new SqlDataAdapter();
        //sda.SelectCommand = new SqlCommand("select aaa.*,t.currentFlag from (select  aa.*,d.strRoomName,d.strDepart,d.strCDep from (select distinct s.intWeek, RTRIM(a.strRoom) as strRoom,a.intDay,a.intStartNum,a.intEndNum,a.yearID from RoomApply a inner join RoomApplySub s on a.id = s.F_id) as aa inner join RoomDetail d on aa.strRoom = d.strRoomName where aa.intWeek >= '" + weekNum + "' and aa.intWeek <= '" + weekNum2 + "' and d.strDepart= '" + departmentName + "' and (( aa.intStartNum  >= " + startNum1 + " and aa.intStartNum <= " + @endNum2 + " )or(aa.intEndNum  >= " + @startNum1 + " and aa.intEndNum <= " + @endNum2 + " )or((aa.intStartNum  < " + @startNum1 + " and aa.intEndNum > " + @endNum2 + " )))) as aaa inner join TitleStartEnd t on aaa.yearID = t.yearID and t.currentFlag = 'true'", con);
        sda.SelectCommand = new SqlCommand("select aaa.*,t.currentFlag from (select  aa.*,d.strRoomName,d.strDepart,d.strCDep from (select distinct s.intWeek, RTRIM(a.strRoom) as strRoom,a.intDay,a.intStartNum,a.intEndNum,a.yearID from RoomApply a inner join RoomApplySub s on a.id = s.F_id) as aa inner join RoomDetail d on aa.strRoom = d.strRoomName where aa.intWeek = '" + weekNum + "' and d.strDepart= '" + departmentName + "' and (( aa.intStartNum  >= " + startNum1 + " and aa.intStartNum <= " + @endNum2 + " )or(aa.intEndNum  >= " + @startNum1 + " and aa.intEndNum <= " + @endNum2 + " )or((aa.intStartNum  < " + @startNum1 + " and aa.intEndNum > " + @endNum2 + " )))) as aaa inner join TitleStartEnd t on aaa.yearID = t.yearID and t.currentFlag = 'true'", con);
        DataSet ds = new DataSet();
        sda.Fill(ds);
        DataTable table = new DataTable();
        table = ds.Tables[0];
        DataTable dtSchedule = new DataTable();

        SqlDataAdapter sdaRoom = new SqlDataAdapter();
        sdaRoom.SelectCommand = new SqlCommand("select distinct RTRIM(strRoomName) as strRoomName from RoomDetail where strDepart = '" + departmentName + "'",con);
        DataSet dsRoom = new DataSet();
        sdaRoom.Fill(dsRoom);
        DataTable roomTable = new DataTable();
        roomTable = dsRoom.Tables[0];
        

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

        //temp data
        string iniString = "";
        for (int i = 0; i<roomTable.Rows.Count; i++)
        {
            iniString = iniString + roomTable.Rows[i][0] + "<br />";
        }
        
        for (int i = startNum1-1; i< endNum2; i++)
        {
            for (int j=1;j<8;j++)
            {
                dtSchedule.Rows[i][j] = iniString;
            }
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
                    //dtSchedule.Rows[j][tempArray[i][0]] = dtSchedule.Rows[j][tempArray[i][0]].ToString() + Convert.ToString(table.Rows[i]["strRoom"]) + "<br />";
                    dtSchedule.Rows[j][tempArray[i][0]] = dtSchedule.Rows[j][tempArray[i][0]].ToString().Replace(Convert.ToString(table.Rows[i]["strRoom"]) + "<br />", "");
                }
                if ((Convert.ToInt16(section) > Convert.ToInt16(startNum)) && (Convert.ToInt16(section) < Convert.ToInt16(endNum)))
                {
                    dtSchedule.Rows[j][tempArray[i][0]] = dtSchedule.Rows[j][tempArray[i][0]].ToString().Replace(Convert.ToString(table.Rows[i]["strRoom"]) + "<br />", "");
                }
                if (section == endNum)//判断课程结束时间，记录位置
                {
                    tempArray[i][2] = j;//记录课结束时间
                    dtSchedule.Rows[j][tempArray[i][0]] = dtSchedule.Rows[j][tempArray[i][0]].ToString().Replace(Convert.ToString(table.Rows[i]["strRoom"]) + "<br />", "");
                    break;
                }
            }
        }

        //修改行标题
        for (int i = 1; i < 8; i++)
        {
            dtSchedule.Columns[i].ColumnName = "星期" + CommonClass.WeekConvertToChinese(i);
        }
        //修改列标题
        for (int i = 0; i < 10; i++)
        {
            dtSchedule.Rows[i][0] = "第" + CommonClass.ConvertToChinese(i + 1) + "节";
        }


        //gvTest.DataSource = dtSchedule;
        //gvTest.DataBind(); 

        //dispose
        table.Dispose();
        ds.Dispose();
        sda.Dispose();
        con.Dispose();

        return dtSchedule;

    }

    private void DataBindRepeater()
    {        
        this.Repeater1.DataSource = PrintTab(Convert.ToInt16(ddlWeek.SelectedValue),Convert.ToInt16(ddlNum1.SelectedValue), Convert.ToInt16(ddlNum2.SelectedValue), ddlDep.SelectedValue);
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


    protected void btSearch_Click(object sender, EventArgs e)
    {
        if (Convert.ToInt16(ddlNum2.SelectedValue) < (Convert.ToInt16(ddlNum1.SelectedValue)))
        {
            ddlNum2.SelectedValue = ddlNum1.SelectedValue;
        }

        DataBindRepeater();

        Session["QRdep"] = ddlDep.SelectedValue.ToString();
        Session["QRweek"] = ddlWeek.SelectedValue.ToString();
        Session["QRnum1"] = ddlNum1.SelectedValue.ToString();
        Session["QRnum2"] = ddlNum2.SelectedValue.ToString();



    }

}