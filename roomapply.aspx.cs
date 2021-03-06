﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Text.RegularExpressions;
using System.Web.UI.HtmlControls;

public partial class roomapply : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Label1.Visible = false;

        int weekNum = CommonClass.getCurrentWeek();

        if (Page.IsPostBack == false)
        {
            DropDownListWeek.SelectedValue = weekNum.ToString();
            if(Session["RADep"] != null) { DropDownListDepart.SelectedValue = Session["RADep"].ToString(); }
        }

        string title = CommonClass.getTitle();
        lbTitle.Text = title;

        


    }       

    protected void PrintTab(int weekNum,string RoomName,string gvName,string departmentName)
    {
        SqlConnection con = CommonClass.GetSqlConnection();
        SqlDataAdapter sda = new SqlDataAdapter();
        sda.SelectCommand = new SqlCommand("select RTRIM(t.currentFlag) as currentFlag  ,dp.strDepart ,aaa.* from(select aa.*,w.datePeriod,w.intWeek as wWeek ,w.yearID as wYear,d.strRoomName,d.depid from (select l.applyid,RTRIM(l.strName) as strName,RTRIM(a.strClass) as strClass,RTRIM(a.strTeacher) as strTeacher,a.strRemark ,l.yearID, a.roomid, a.intDay, a.intStartNum, a.intEndNum, a.strWeekReg, a.strWeekData, s.intweek from ApplyList l inner join RoomApply a on l.applyid = a.applyid inner join RoomApplySub s on a.id = s.F_id  where a.roomid = '" + RoomName + "' and s.intWeek = " + weekNum + ") as aa	right join WeekStartEnd w on w.yearID = aa.yearID right join RoomDetail d on d.depid = '" + departmentName + "' where d.roomid =  '" + RoomName + "' and w.intWeek = " + weekNum + ") as aaa left join TitleStartEnd t on aaa.wYear = t.yearID and t.currentFlag = 'true' inner join Department dp on aaa.depid = dp.depid where t.currentFlag = 'true'", con);
        DataSet ds = new DataSet();
        sda.Fill(ds);
        DataTable table = new DataTable();
        table = ds.Tables[0];
        DataTable dtSchedule = new DataTable();

        //增加标题
        Label lbTitle = new Label();
        lbTitle.ID = gvName + "Title";
        lbTitle.CssClass = "gvTitle";
        lbTitle.BorderColor = System.Drawing.Color.Transparent;//无色
        //lbTitle.GridLines = GridLines.Horizontal;
        lbTitle.BorderWidth = 0;
        //lbTitle.ShowHeader = false;
        string sRoomName = table.Rows[0]["strRoomName"].ToString().TrimEnd();

        DataTable dtTitle = new DataTable();
        dtTitle.Columns.Add(RoomName);
        dtTitle.Rows.Add(sRoomName + " 第" + weekNum + "周 " + table.Rows[0]["strDepart"].ToString() + " " + table.Rows[0]["datePeriod"].ToString());
        lbTitle.Text = dtTitle.Rows[0][0].ToString();        
        GridViewPlaceHolder.Controls.Add(lbTitle);
        dtTitle.Dispose();
        lbTitle.Dispose();
        //GridView gvTitle = new GridView();
        //gvTitle.ID = gvName + "Title";
        //gvTitle.HorizontalAlign = HorizontalAlign.Center;
        //gvTitle.BorderColor = System.Drawing.Color.Transparent;//无色
        //gvTitle.GridLines = GridLines.Horizontal;
        //gvTitle.BorderWidth = 0;
        //gvTitle.ShowHeader = false;

        //DataTable dtTitle = new DataTable();
        //dtTitle.Columns.Add(RoomName);
        //dtTitle.Rows.Add(RoomName + " 第" + weekNum + "周 " + table.Rows[0]["strDepart"].ToString() + " "+ table.Rows[0]["datePeriod"].ToString());
        //gvTitle.DataSource = dtTitle;
        //gvTitle.DataBind();
        //GridViewPlaceHolder.Controls.Add(gvTitle);
        //dtTitle.Dispose();
        //gvTitle.Dispose();

        GridView gvTemp = new GridView();
        gvTemp.ID = gvName;
        gvTemp.HorizontalAlign = HorizontalAlign.Center;
        GridViewPlaceHolder.Controls.Add(gvTemp);

        HtmlGenericControl spanTemp = new HtmlGenericControl("div");//创建一个span标签
        spanTemp.ID= "div" + gvName;
        spanTemp.Style.Add("page-break-before", "always");//属性
        GridViewPlaceHolder.Controls.Add(spanTemp); //添加到页面 

        //添加八列
        dtSchedule.Columns.Add("<a href='RoomDetail.aspx?qsRoomName=" + RoomName + "' target='_blank'>星期<br />节次</a>");
        for (int i=1;i<8;i++)
        {
            dtSchedule.Columns.Add(Convert.ToString(i));
        }

        //添加10行
        for (int i = 0; i < 11; i++)
        {
            dtSchedule.Rows.Add();
        }

        //添加左侧固定信息（第几节）
        for (int i = 0; i < 11; i++)
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
            if(table.Rows[i]["applyid"].ToString() != "")
            { 
                if ((Convert.ToInt16(table.Rows[i]["intWeek"]) == Convert.ToInt16(table.Rows[i]["wWeek"]))&&((table.Rows[i]["currentFlag"].ToString()) == "true"))
                {
                    //Day
                    string week = Convert.ToString(table.Rows[i]["intDay"]);
                    //StartNum
                    string startNum = "";
                    if (Convert.ToInt16(table.Rows[i]["intStartNum"]) <=4)
                    {
                        startNum = Convert.ToString(table.Rows[i]["intStartNum"]);
                    }
                    if ((Convert.ToInt16(table.Rows[i]["intStartNum"]) >= 5)&& (Convert.ToInt16(table.Rows[i]["intStartNum"]) <= 10))
                    {
                        startNum = (Convert.ToInt16(table.Rows[i]["intStartNum"])+1).ToString();
                    }
                    if (Convert.ToInt16(table.Rows[i]["intStartNum"]) == 99)
                    {
                        startNum = "5";
                    }


                    //EndNum
                    //string endNum = Convert.ToString(table.Rows[i]["intEndNum"]);
                    string endNum = "";
                    if (Convert.ToInt16(table.Rows[i]["intEndNum"]) <= 4)
                    {
                        endNum = Convert.ToString(table.Rows[i]["intEndNum"]);
                    }
                    if ((Convert.ToInt16(table.Rows[i]["intEndNum"]) >= 5) && (Convert.ToInt16(table.Rows[i]["intEndNum"]) <= 10))
                    {
                        endNum = (Convert.ToInt16(table.Rows[i]["intEndNum"]) + 1).ToString();
                    }
                    if (Convert.ToInt16(table.Rows[i]["intEndNum"]) == 99)
                    {
                        endNum = "5";
                    }


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
                            if(Convert.ToString(table.Rows[i]["strName"]) != string.Empty)
                            {
                                string Sremark = " " + Convert.ToString(table.Rows[i]["strRemark"]);
                                dtSchedule.Rows[j][tempArray[i][0]] = Convert.ToString(table.Rows[i]["strName"]) + Sremark + "<br />" + Convert.ToString(table.Rows[i]["strClass"]) + "<br />" + Convert.ToString(table.Rows[i]["strTeacher"]);
                            }
                            else
                            {
                                dtSchedule.Rows[j][tempArray[i][0]] = Convert.ToString(table.Rows[i]["strName"]) + "<br />" + Convert.ToString(table.Rows[i]["strClass"]) + "<br />" + Convert.ToString(table.Rows[i]["strTeacher"]);
                            }
                        }
                        if (section == endNum)//判断课程结束时间，记录位置
                        {
                            tempArray[i][2] = j;//记录课结束时间
                            break;
                        }
                    }
                }
            }
        }

        //修改行标题
        for (int i =1;i<8; i++)
        {
            dtSchedule.Columns[i].ColumnName ="星期"+ WeekConvertToChinese(i);
        }
        //修改列标题
        for (int i=0;i<4;i++)
        {
            dtSchedule.Rows[i][0] = "第" + ConvertToChinese2(i + 1) + "节";
        }
        dtSchedule.Rows[4][0] = "中午";
        for (int i = 5; i < 11; i++)
        {
            dtSchedule.Rows[i][0] = "第" + ConvertToChinese2(i + 1) + "节";
        }

        //gridview不写死
        //GridView1.DataSource = dtSchedule;
        //GridView1.DataBind();
        DynamicGenerateColumns(gvTemp, dtSchedule);
        gvTemp.DataSource = dtSchedule;
        gvTemp.DataBind();
        gvTemp.RowStyle.HorizontalAlign = HorizontalAlign.Center;

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

        string ConvertToChinese2(int x)
        {
            string cstr = "";   
            switch (x)   
            {   
                case 0: cstr = "零"; break;   
                case 1: cstr = "一"; break;   
                case 2: cstr = "二"; break;   
                case 3: cstr = "三"; break;   
                case 4: cstr = "四"; break;   
                //case 5: cstr = "五"; break;   
                case 6: cstr = "五"; break;   
                case 7: cstr = "六"; break;   
                case 8: cstr = "七"; break;   
                case 9: cstr = "八"; break;
                case 10: cstr = "九"; break;
                case 11: cstr = "十"; break;
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
                sqlQuery = "select roomid from RoomDetail where depid = @depid";
                //SqlCommand comm = new SqlCommand(sqlQuery,con);
                //comm.Parameters.AddWithValue("@strDepart", DropDownListDepart.SelectedValue);
                SqlDataAdapter sda = new SqlDataAdapter(sqlQuery,con);
                sda.SelectCommand.Parameters.AddWithValue("@depid", DropDownListDepart.SelectedValue);
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
        Session["RADep"] = DropDownListDepart.SelectedValue.ToString();
    }



    protected void btExcel_Click(object sender, EventArgs e)
    {
        string html = HdnValue.Value;
        ExportToExcel(ref html, "MyReport");
    }

    public void ExportToExcel(ref string html, string fileName)
    {
        html = html.Replace("&gt;", ">");
        html = html.Replace("&lt;", "<");
        HttpContext.Current.Response.ClearContent();
        HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.UTF8;
        HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=" + System.Web.HttpUtility.UrlEncode(fileName, System.Text.Encoding.UTF8) + "_" + DateTime.Now.ToString("M_dd_yyyy_H_M_s") + ".doc");
        HttpContext.Current.Response.ContentType = "application/ms-word";
        HttpContext.Current.Response.Write(html);
        HttpContext.Current.Response.End();
    }
}

