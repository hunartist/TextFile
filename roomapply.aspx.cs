using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Text.RegularExpressions;


public partial class roomapply : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {        
        PrintTab("501");
    }       

    protected void PrintTab(string roomName)
    {
        SqlConnection con = GetSqlConnection();
        SqlDataAdapter sda = new SqlDataAdapter();
        sda.SelectCommand = new SqlCommand("select distinct a.strRoom,a.intDay,a.intStartNum,intEndNum,a.strName,a.strClass,a.strTeacher from RoomApply a inner join RoomApplySub s on a.Id=s.F_id where strRoom = "+roomName,con);
        DataSet ds = new DataSet();
        sda.Fill(ds);
        DataTable table = new DataTable();
        table = ds.Tables[0];

        DataTable dtSchedule = new DataTable();
        //添加八列
        dtSchedule.Columns.Add(roomName);
        for (int i=1;i<8;i++)
        {
            dtSchedule.Columns.Add(Convert.ToString(i));
        }

        //添加八行
        for (int i = 0; i < 8; i++)
        {
            dtSchedule.Rows.Add();
        }

        //添加左侧固定信息（第几节）
        for (int i = 0; i < 8; i++)
        {
            dtSchedule.Rows[i][0] = (i + 1) ;
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

            for (int weekCount = 1;weekCount < 8;weekCount++)//确定本条数据将来显示在哪一列
            {
                if(week==Convert.ToString(dtSchedule.Columns[weekCount].ColumnName))//跟星期做比较，确定数据应该写在那一列
                {
                    tempArray[i][0] = weekCount;//记录星期（确定将来的数据显示在哪一列）
                    break;
                }
            }

            for (int j=0;j<dtSchedule.Rows.Count;j++)//确定课程的开始时间和结束时间，并填写数据
            {
                string section = Convert.ToString(dtSchedule.Rows[j][0]);//当前行是第几节课
                if(section==startNum)//判断课程开始时间，确定位置，填写数据
                {
                    tempArray[i][1] = j;//记录上课开始时间（确定数据显示在哪一行）
                    dtSchedule.Rows[j][tempArray[i][0]] = Convert.ToString(table.Rows[i]["strName"]) + "<br />" + Convert.ToString(table.Rows[i]["strClass"]) + "<br />" + Convert.ToString(table.Rows[i]["strTeacher"]);
                }
                if(section==endNum)//判断课程结束时间，记录位置
                {
                    tempArray[i][2] = j;//记录课结束时间
                    break;
                }
            }
        }
        
        GridView1.DataSource = dtSchedule;
        GridView1.DataBind();

        //合并单元格
        for (int i=0;i<table.Rows.Count;i++)
            GroupCol(GridView1, tempArray[i][0], tempArray[i][1], tempArray[i][2]);
    }

    /// <summary>
    /// 合并某列中的多个单元格
    /// </summary>
    /// <param name="GridView1"></param>
    /// <param name="cols">要合并的那一列</param>
    /// <param name="sRow">开始行</param>
    /// <param name="eRow">结束行</param>
    public static void GroupCol(GridView GridView1,int cols,int sRow,int eRow)
    {
        TableCell oldTc = GridView1.Rows[sRow].Cells[cols];
        for (int i = 1;i<=eRow-sRow;i++)
        {
            TableCell tc = GridView1.Rows[sRow + i].Cells[cols];
            tc.Visible = false;
            if (oldTc.RowSpan == 0)
            {
                oldTc.RowSpan = 1;
            }
            oldTc.RowSpan++;
            oldTc.VerticalAlign = VerticalAlign.Middle;
        }
    }


    protected void GridView1_RowDataBound1(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            TableCellCollection cells = e.Row.Cells;
            foreach (TableCell cell in cells)
            {
                cell.Text = Server.HtmlDecode(cell.Text); //注意：此处所有的列所有的html代码都会按照html格式输出，如果只需要其中的哪一列的数据需要转换，此处需要小的修改即可。
            }
        }
    }

    /// <summary> 
    /// 1.获取数据库的连接，返回值需判断是否为null 
    /// </summary> 
    /// <returns></returns> 
    public static SqlConnection GetSqlConnection()
    {
        string strCnn = "Data Source=192.168.0.11;Initial Catalog=webTest;User ID=sa;Password=config;";
        try
        {
            SqlConnection sqlCnn = new SqlConnection(strCnn);
            sqlCnn.Open();
            return sqlCnn;
        }
        catch (Exception ee)
        {
            string temp = ee.Message;
            return null;
        }
    }
    /// <summary> 
    /// 获取SqlCommand对象 
    /// </summary> 
    /// <returns></returns> 
    public static SqlCommand GetSqlCommand()
    {
        SqlConnection sqlCnn = GetSqlConnection();
        if (sqlCnn == null)
            return null;
        else
        {
            SqlCommand sqlCmm = new SqlCommand();
            sqlCmm.Connection = sqlCnn;
            return sqlCmm;
        }
    }

}

