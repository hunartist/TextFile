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
        int weekNum = CommonClass.getCurrentWeek();        

        if (Page.IsPostBack == false)
        {
            ddlWeek.SelectedValue = weekNum.ToString();
        }
        
    }

    protected void PrintTab(int weekNum, /*string RoomName,*/ string gvName, string departmentName)
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

        ////增加gridview
        //GridView gvTitle = new GridView();
        //gvTitle.ID = gvName + "Title";
        //gvTitle.HorizontalAlign = HorizontalAlign.Center;
        //gvTitle.BorderColor = System.Drawing.Color.Transparent;//无色
        //gvTitle.ShowHeader = false;

        //DataTable dtTitle = new DataTable();
        //dtTitle.Columns.Add(RoomName);
        //dtTitle.Rows.Add(RoomName + " 第" + weekNum + "周 " + table.Rows[0]["strDepart"].ToString() + " " + table.Rows[0]["datePeriod"].ToString());
        //gvTitle.DataSource = dtTitle;
        //gvTitle.DataBind();
        //GridViewPlaceHolder.Controls.Add(gvTitle);
        //dtTitle.Dispose();
        //gvTitle.Dispose();

        GridView gvTemp = new GridView();
        gvTemp.ID = gvName;
        gvTemp.HorizontalAlign = HorizontalAlign.Center;
        GridViewPlaceHolder.Controls.Add(gvTemp);

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
            dtSchedule.Columns[i].ColumnName = "星期" + WeekConvertToChinese(i);
        }
        //修改列标题
        for (int i = 0; i < 10; i++)
        {
            dtSchedule.Rows[i][0] = "第" + ConvertToChinese(i + 1) + "节";
        }

        //gridview不写死
        //GridView1.DataSource = dtSchedule;
        //GridView1.DataBind();
        DynamicGenerateColumns(gvTemp, dtSchedule);
        gvTemp.DataSource = dtSchedule;
        gvTemp.DataBind();
        gvTemp.RowStyle.HorizontalAlign = HorizontalAlign.Center;

        //合并单元格
        for (int i = 0; i < table.Rows.Count; i++)
            GroupCol(gvTemp, tempArray[i][0], tempArray[i][1], tempArray[i][2]);

        //dispose
        dtSchedule.Dispose();
        table.Dispose();
        ds.Dispose();
        sda.Dispose();
        con.Dispose();

    }

    //根据DataTable动态生成GridView
    public static GridView DynamicGenerateColumns(GridView gv, DataTable dt)
    {
        // 把GridView的自动产生列设置为false,否则会出现重复列
        gv.AutoGenerateColumns = false;

        // 清空所有的Columns
        gv.Columns.Clear();

        // 遍历DataTable 的每个Columns,然后添加到GridView中去
        foreach (DataColumn item in dt.Columns)
        {
            //if (item.ColumnName == "选择")
            //{
            //    CheckBoxField chCol = new CheckBoxField();
            //    chCol.HeaderText = item.ColumnName;
            //    chCol.DataField = item.ColumnName;
            //    chCol.Visible = true;
            //    gv.Columns.Add(chCol);
            //    continue;
            //}
            BoundField col = new BoundField();
            col.HeaderText = item.ColumnName;
            col.HtmlEncode = false;
            col.DataField = item.ColumnName;
            col.Visible = true;
            gv.Columns.Add(col);
        }
        return gv;
    }

    /// <summary>
    /// 合并某列中的多个单元格
    /// </summary>
    /// <param name="gvTemp"></param>
    /// <param name="cols">要合并的那一列</param>
    /// <param name="sRow">开始行</param>
    /// <param name="eRow">结束行</param>
    public static void GroupCol(GridView gvTemp, int cols, int sRow, int eRow)
    {
        TableCell oldTc = gvTemp.Rows[sRow].Cells[cols];
        for (int i = 1; i <= eRow - sRow; i++)
        {
            TableCell tc = gvTemp.Rows[sRow + i].Cells[cols];
            tc.Visible = false;
            if (oldTc.RowSpan == 0)
            {
                oldTc.RowSpan = 1;
            }
            oldTc.RowSpan++;
            oldTc.VerticalAlign = VerticalAlign.Middle;
        }
    }


    //protected void GridView1_RowDataBound1(object sender, GridViewRowEventArgs e)
    //{
    //    if (e.Row.RowType == DataControlRowType.DataRow)
    //    {
    //        TableCellCollection cells = e.Row.Cells;
    //        foreach (TableCell cell in cells)
    //        {
    //            cell.Text = Server.HtmlDecode(cell.Text); //注意：此处所有的列所有的html代码都会按照html格式输出，如果只需要其中的哪一列的数据需要转换，此处需要小的修改即可。
    //        }
    //    }
    //}

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

    protected void gvTemp_RowCreated(object sender, GridViewRowEventArgs e)
    {
        //判断创建的行是否为表头行
        if (e.Row.RowType == DataControlRowType.Header)
        {
            //获取表头所在行的所有单元格
            TableCellCollection tcHeader = e.Row.Cells;
            //清除自动生成的表头
            tcHeader.Clear();

            //新添加的第一个表头单元格, 设置为合并7个列, 从而形成一行.
            tcHeader.Add(new TableHeaderCell());
            tcHeader[0].ColumnSpan = 8;

        }
    }

    protected void btSearch_Click(object sender, EventArgs e)
    {
        PrintTab(Convert.ToInt16(ddlWeek.SelectedValue), "gvTest", ddlDep.SelectedValue);
    }
}