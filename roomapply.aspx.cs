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
        Label1.Visible = false;

        //get current week
        SqlConnection con = CommonClass.GetSqlConnection();
        SqlDataAdapter sda = new SqlDataAdapter();
        sda.SelectCommand = new SqlCommand("select * from TitleStartEnd", con);
        DataSet ds = new DataSet();
        sda.Fill(ds);
        DataTable table = new DataTable();
        table = ds.Tables[0];

        int startDate = Convert.ToDateTime(table.Rows[0][2]).DayOfYear;
        int maxWeek = Convert.ToInt16(table.Rows[0][1]);
        //一.找到第一周的最后一天（先获取1月1日是星期几，从而得知第一周周末是几）
        int firstWeekend = 7 - Convert.ToInt32(DateTime.Parse(DateTime.Today.Year + "-1-1").DayOfWeek);
        //二.获取今天是一年当中的第几天
        int currentDay = DateTime.Today.DayOfYear;
        //三.（今天 减去 第一周周末）/7 等于 距第一周有多少周 再加上第一周的1 就是今天是今年的第几周了
        //    刚好考虑了惟一的特殊情况就是，今天刚好在第一周内，那么距第一周就是0 再加上第一周的1 最后还是1
        int weekTemp = Convert.ToInt32(Math.Ceiling((startDate - firstWeekend) / 7.0)) ;
        int weekNum = Convert.ToInt32(Math.Ceiling((currentDay - firstWeekend) / 7.0)) + 1 - weekTemp;

        if((weekNum<=maxWeek)&&(Page.IsPostBack == false))
        {
            DropDownListWeek.SelectedValue = weekNum.ToString();
        }

        lbTitle.Text = table.Rows[0][3].ToString();

        table.Dispose();
        ds.Dispose();
        sda.Dispose();
        con.Dispose();


    }       

    protected void PrintTab(int weekNum,string RoomName,string gvName,string departmentName)
    {
        SqlConnection con = CommonClass.GetSqlConnection();
        SqlDataAdapter sda = new SqlDataAdapter();
        sda.SelectCommand = new SqlCommand("select * from (select distinct s.intWeek, a.strRoom,a.intDay,a.intStartNum,a.intEndNum,a.strName,a.strClass,a.strTeacher from RoomApply a inner join RoomApplySub s on a.id = s.F_id) as aa right join RoomDetail d on aa.strRoom = d.strRoomName and aa.intWeek = " + weekNum + " inner join WeekStartEnd w on  w.id = " + weekNum + " where d.strRoomName = '" + RoomName+ "' and d.strDepart= '" + departmentName+"'", con);
        DataSet ds = new DataSet();
        sda.Fill(ds);
        DataTable table = new DataTable();
        table = ds.Tables[0];
        DataTable dtSchedule = new DataTable();

        //增加gridview
        GridView gvTitle = new GridView();
        gvTitle.ID = gvName + "Title";
        gvTitle.HorizontalAlign = HorizontalAlign.Center;
        gvTitle.BorderColor = System.Drawing.Color.Transparent;//无色
        gvTitle.ShowHeader = false;

        DataTable dtTitle = new DataTable();
        dtTitle.Columns.Add(RoomName);
        dtTitle.Rows.Add(RoomName + " 第" + weekNum + "周 " + table.Rows[0]["strDepart"].ToString() + " "+ table.Rows[0]["datePeriod"].ToString());
        gvTitle.DataSource = dtTitle;
        gvTitle.DataBind();
        GridViewPlaceHolder.Controls.Add(gvTitle);
        dtTitle.Dispose();
        gvTitle.Dispose();

        GridView gvTemp = new GridView();
        gvTemp.ID = gvName;
        gvTemp.HorizontalAlign = HorizontalAlign.Center;
        GridViewPlaceHolder.Controls.Add(gvTemp);

        //添加八列
        dtSchedule.Columns.Add("<a href='ipmsg.aspx?ipadd=" + RoomName + "' target='_blank'>详细</a>");
        for (int i=1;i<8;i++)
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
            dtSchedule.Rows[i][0] = (i+1) ;
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

        //修改行标题
        for (int i =1;i<8; i++)
        {
            dtSchedule.Columns[i].ColumnName ="星期"+ WeekConvertToChinese(i);
        }
        //修改列标题
        for (int i=0;i<10;i++)
        {
            dtSchedule.Rows[i][0] = "第" + ConvertToChinese(i + 1) + "节";
        }

        //gridview不写死
        //GridView1.DataSource = dtSchedule;
        //GridView1.DataBind();
        DynamicGenerateColumns(gvTemp, dtSchedule);
        gvTemp.DataSource = dtSchedule;
        gvTemp.DataBind();

        //合并单元格
        for (int i=0;i<table.Rows.Count;i++)
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
    public static void GroupCol(GridView gvTemp,int cols,int sRow,int eRow)
    {
        TableCell oldTc = gvTemp.Rows[sRow].Cells[cols];
        for (int i = 1;i<=eRow-sRow;i++)
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


    //protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    //PrintTab(Convert.ToInt16(DropDownList1.SelectedItem.Value), "501", GridView1, Label1, "501是");
    //    //PrintTab(Convert.ToInt16(DropDownList1.SelectedItem.Value), "505", GridView2, Label2, "505这");
    //    //PrintTab(Convert.ToInt16(DropDownList1.SelectedItem.Value), "506", GridView3, Label3, "506样");
    //    //PrintTab(Convert.ToInt16(DropDownList1.SelectedItem.Value), "507", GridView4, Label4, "507吗");
    //    //PrintTab(Convert.ToInt16(DropDownList1.SelectedItem.Value), "508", GridView5, Label5, "508？");
    //    //PrintTab(Convert.ToInt16(DropDownList1.SelectedItem.Value), "802", GridView6, Label6, "802？");
    //    //PrintTab(Convert.ToInt16(DropDownList1.SelectedItem.Value), "804", GridView7, Label7, "804？");
    //    //PrintTab(Convert.ToInt16(DropDownList1.SelectedItem.Value), "809", GridView8, Label8, "809？");


    //}

    protected void Button1_Click(object sender, EventArgs e)
    {
        if (DropDownListWeek.SelectedValue == null)
        {
            Label1.Visible = true;
            Label1.Text = "not selected";
            return;
        }
        if (DropDownListDepart.SelectedValue == null)
        {
            Label1.Visible = true;
            Label1.Text = "not selected";
            return;
        }
        else
        {
            try
            {
                string sqlQuery;

                SqlConnection con = CommonClass.GetSqlConnection();
                sqlQuery = "select RTRIM(strRoomName) as strRoomName from RoomDetail where strDepart = @strDepart";
                //SqlCommand comm = new SqlCommand(sqlQuery,con);
                //comm.Parameters.AddWithValue("@strDepart", DropDownListDepart.SelectedValue);
                SqlDataAdapter sda = new SqlDataAdapter(sqlQuery,con);
                sda.SelectCommand.Parameters.AddWithValue("@strDepart", DropDownListDepart.SelectedValue);
                DataSet ds = new DataSet();
                sda.Fill(ds);
                DataTable table = new DataTable();
                table = ds.Tables[0];
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    PrintTab(Convert.ToInt16(DropDownListWeek.SelectedValue), Convert.ToString(table.Rows[i][0]), "Gridview" + i,DropDownListDepart.SelectedValue);
                }
                table.Dispose();
                ds.Dispose();
                sda.Dispose();
                con.Dispose();
            }
            catch (Exception ee)
            {
                string temp = ee.Message;
                return;
            }
        }
    }
}

