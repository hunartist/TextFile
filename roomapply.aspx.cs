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
        //PrintTab(1,"501", GridView1, Label1, "text");
            }       

    protected void PrintTab(int weekNum,string RoomName,string gvName,string lbName)
    {
        SqlConnection con = CommonClass.GetSqlConnection();
        SqlDataAdapter sda = new SqlDataAdapter();
        sda.SelectCommand = new SqlCommand("select * from (select distinct s.intWeek, a.strRoom,a.intDay,a.intStartNum,a.intEndNum,a.strName,a.strClass,a.strTeacher from RoomApply a inner join RoomApplySub s on a.Id = s.F_id) as aa right join RoomDetail d on aa.strRoom = d.Id where (aa.intWeek ="+weekNum+ "or aa.intWeek is null) and d.Id ="+RoomName, con);
        DataSet ds = new DataSet();
        sda.Fill(ds);
        DataTable table = new DataTable();
        table = ds.Tables[0];
        DataTable dtSchedule = new DataTable();

        //增加gridview
        GridView gvTemp = new GridView();
        gvTemp.ID = gvName;
        GridViewPlaceHolder.Controls.Add(gvTemp);

        //增加label
        Label lbTemp = new Label();
        lbTemp.ID = lbName;
        lbTemp.Text = lbName;
        PanelLable.Controls.Add(lbTemp);

        //添加八列
        dtSchedule.Columns.Add("kcb");
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

        //gridview不写死
        //GridView1.DataSource = dtSchedule;
        //GridView1.DataBind();
        DynamicGenerateColumns(gvTemp, dtSchedule);
        gvTemp.DataSource = dtSchedule;
        gvTemp.DataBind();

        //合并单元格
        for (int i=0;i<table.Rows.Count;i++)
            GroupCol(gvTemp, tempArray[i][0], tempArray[i][1], tempArray[i][2]);
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



    protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
    {
        //PrintTab(Convert.ToInt16(DropDownList1.SelectedItem.Value), "501", GridView1, Label1, "501是");
        //PrintTab(Convert.ToInt16(DropDownList1.SelectedItem.Value), "505", GridView2, Label2, "505这");
        //PrintTab(Convert.ToInt16(DropDownList1.SelectedItem.Value), "506", GridView3, Label3, "506样");
        //PrintTab(Convert.ToInt16(DropDownList1.SelectedItem.Value), "507", GridView4, Label4, "507吗");
        //PrintTab(Convert.ToInt16(DropDownList1.SelectedItem.Value), "508", GridView5, Label5, "508？");
        //PrintTab(Convert.ToInt16(DropDownList1.SelectedItem.Value), "802", GridView6, Label6, "802？");
        //PrintTab(Convert.ToInt16(DropDownList1.SelectedItem.Value), "804", GridView7, Label7, "804？");
        //PrintTab(Convert.ToInt16(DropDownList1.SelectedItem.Value), "809", GridView8, Label8, "809？");

        SqlConnection con = CommonClass.GetSqlConnection();
        SqlDataAdapter sda = new SqlDataAdapter();
        sda.SelectCommand = new SqlCommand("select Id from RoomDetail", con);
        DataSet ds = new DataSet();
        sda.Fill(ds);
        DataTable table = new DataTable();
        table = ds.Tables[0];
        for (int i = 0; i < table.Rows.Count; i++)
        {
            PrintTab(Convert.ToInt16(DropDownList1.SelectedItem.Value), Convert.ToString(table.Rows[i][0]), "Gridview" + i,Convert.ToString(i));
        }
        table.Dispose();
        ds.Dispose();
        sda.Dispose();
        con.Dispose();
    }
}

