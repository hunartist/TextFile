using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Text.RegularExpressions;

public partial class roomQueryReverse : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //PrintTab(7, "gvTest", "多媒体");
        int CweekNum = CommonClass.getCurrentWeek();        

        if (Page.IsPostBack == false)
        {
            ddlWeek.SelectedValue = CweekNum.ToString();
        }

    }

    protected DataTable PrintTab(int weekNum,  string departmentName)
    {
        SqlConnection con = CommonClass.GetSqlConnection();
        SqlDataAdapter sda = new SqlDataAdapter();
        sda.SelectCommand = new SqlCommand("select * from (select distinct s.intWeek, RTRIM(a.strRoom) as strRoom,a.intDay,a.intStartNum,a.intEndNum from RoomApply a inner join RoomApplySub s on a.id = s.F_id) as aa inner join RoomDetail d on aa.strRoom = d.strRoomName where aa.intWeek = '" + weekNum + "' and d.strDepart= '" + departmentName + "'", con);
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
        
        for (int i = 0;i<10;i++)
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



    protected void gvTest_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            TableCellCollection cells = e.Row.Cells;
            foreach (TableCell cell in cells)
            {
                cell.Text = Server.HtmlDecode(cell.Text); //注意：此处所有的列所有的html代码都会按照html格式输出，如果只需要其中的哪一列的数据需要转换，此处需要小的修改即可。
            }
        }
        e.Row.Attributes.Add("style", "word-break:break-all;word-wrap:break-word");        
    }


    protected void btSearch_Click(object sender, EventArgs e)
    {
        //gvTest.DataSource = PrintTab(Convert.ToInt16(ddlWeek.SelectedValue), ddlDep.SelectedValue);
        //gvTest.DataBind();

        Repeater1.DataSource = PrintTab(Convert.ToInt16(ddlWeek.SelectedValue), ddlDep.SelectedValue);
        Repeater1.DataBind();

        //for (int i = 0; i < gvTest.Rows.Count; i++)
        //{
        //    for (int j = 0; j < gvTest.Rows[0].Cells.Count; j++)
        //    {
        //        gvTest.Rows[i].Cells[j].Width = 80;
        //    }
        //}

    }


}