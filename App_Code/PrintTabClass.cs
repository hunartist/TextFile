using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

/// <summary>
/// PrintTab 的摘要说明
/// </summary>
public class PrintTabClass
{
    /// <summary>
    /// 根据连接字符串返回总体课表
    /// </summary>
    /// <param name="sqlstr">连接字符串（需包含第几周intDay第几节intStartNum,intEndNum等信息）</param>
    public static DataTable PrintTab_SUM_DT(string sqlstr)
    {
        SqlConnection con = CommonClass.GetSqlConnection();
        SqlDataAdapter sda = new SqlDataAdapter();
        sda.SelectCommand = new SqlCommand(sqlstr, con);
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
        for (int i = 0; i < 11; i++)
        {
            dtSchedule.Rows.Add();
        }

        //添加左侧固定信息（第几节）
        for (int i = 0; i < 11; i++)
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
            //string startNum = Convert.ToString(table.Rows[i]["intStartNum"]);
            string startNum = "";
            if (Convert.ToInt16(table.Rows[i]["intStartNum"]) <=4)
            {
                startNum = Convert.ToString(table.Rows[i]["intStartNum"]);
            }
            if ((Convert.ToInt16(table.Rows[i]["intStartNum"]) >= 5)&& (Convert.ToInt16(table.Rows[i]["intStartNum"]) <= 10))
            {
                startNum = (Convert.ToInt16(table.Rows[i]["intStartNum"]) + 1).ToString();
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
                string sRoom = Convert.ToString(table.Rows[i]["strRoomName"]);
                string sName = Convert.ToString(table.Rows[i]["strName"]);
                string sClass = Convert.ToString(table.Rows[i]["strClass"]);
                string sTeacher = Convert.ToString(table.Rows[i]["strTeacher"]);
                string sStartN = Convert.ToString(table.Rows[i]["intStartNum"]);
                string sEndN = Convert.ToString(table.Rows[i]["intEndNum"]);
                string sDay = Convert.ToString(table.Rows[i]["intDay"]);
                string sTemp = sRoom + "-" + sName + "-" + sClass + "-" + sTeacher + "-" + sStartN + sEndN + "-星期" + sDay + "<br />";
                if (section == startNum)//判断课程开始时间，确定位置，填写数据
                {
                    tempArray[i][1] = j;//记录上课开始时间（确定数据显示在哪一行）
                    dtSchedule.Rows[j][tempArray[i][0]] = dtSchedule.Rows[j][tempArray[i][0]].ToString() + sTemp;
                }
                if ((Convert.ToInt16(section) > Convert.ToInt16(startNum))&&(Convert.ToInt16(section) < Convert.ToInt16(endNum)))
                {
                    dtSchedule.Rows[j][tempArray[i][0]] = dtSchedule.Rows[j][tempArray[i][0]].ToString() + sTemp;
                }
                if (section == endNum)//判断课程结束时间，记录位置
                {
                    tempArray[i][2] = j;//记录课结束时间
                    if(startNum != endNum)//如开始时间等于结束时间则会重复记录
                    {
                        dtSchedule.Rows[j][tempArray[i][0]] = dtSchedule.Rows[j][tempArray[i][0]].ToString() + sTemp;
                    }
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
        //for (int i = 0; i < 10; i++)
        //{
        //    dtSchedule.Rows[i][0] = "第" + CommonClass.ConvertToChinese(i + 1) + "节";
        //}
        for (int i=0;i<4;i++)
         {
             dtSchedule.Rows[i][0] = "第" + ConvertToChinese2(i + 1) + "节";
         }
        dtSchedule.Rows[4][0] = "中午";
        for (int i = 5; i < 11; i++)
        {
            dtSchedule.Rows[i][0] = "第" + ConvertToChinese2(i + 1) + "节";
        }

        //dispose        
        table.Dispose();
        ds.Dispose();
        sda.Dispose();
        con.Dispose();

        return dtSchedule;

    }

    /// <summary>
    /// 根据连接字符串返回单周课表
    /// </summary>
    /// <param name="weekNum">第几周</param>
    /// <param name="sqlstr">连接字符串（需包含第几周intDay第几节intStartNum,intEndNum等信息）</param>
    /// <param name="printstr">单元格要显示的内容</param>
    public static DataTable PrintTab_Week_DT(int weekNum,string sqlstr,string printstr)
    {
        sqlstr = sqlstr + " and intweek = "+ weekNum;
        SqlConnection con = CommonClass.GetSqlConnection();
        SqlDataAdapter sda = new SqlDataAdapter();
        sda.SelectCommand = new SqlCommand(sqlstr, con);
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
        for (int i = 0; i < 11; i++)
        {
            dtSchedule.Rows.Add();
        }

        //添加左侧固定信息（第几节）
        for (int i = 0; i < 11; i++)
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
            //string startNum = Convert.ToString(table.Rows[i]["intStartNum"]);
            string startNum = "";
            if (Convert.ToInt16(table.Rows[i]["intStartNum"]) <=4)
            {
                startNum = Convert.ToString(table.Rows[i]["intStartNum"]);
            }
            if ((Convert.ToInt16(table.Rows[i]["intStartNum"]) >= 5)&& (Convert.ToInt16(table.Rows[i]["intStartNum"]) <= 10))
            {
                startNum = (Convert.ToInt16(table.Rows[i]["intStartNum"]) + 1).ToString();
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
                string sRoom = Convert.ToString(table.Rows[i]["strRoomName"]);
                string sName = Convert.ToString(table.Rows[i]["strName"]);
                string sClass = Convert.ToString(table.Rows[i]["strClass"]);
                string sTeacher = Convert.ToString(table.Rows[i]["strTeacher"]);
                string sStartN = Convert.ToString(table.Rows[i]["intStartNum"]);
                string sEndN = Convert.ToString(table.Rows[i]["intEndNum"]);
                string sDay = Convert.ToString(table.Rows[i]["intDay"]);
                string sTemp = string.Empty;
                switch(printstr)
                {
                    case "NCT": sTemp = sName + "<br />" + sClass + "<br />" + sTeacher;break;
                    case "RNT": sTemp = sRoom + "<br />" + sName + "<br />" + sTeacher;break;
                    case "RNC": sTemp = sRoom + "<br />" + sName + "<br />" + sClass; break;
                    default: sTemp = sRoom + "-" + sName + "-" + sClass + "-" + sTeacher + "-" + sStartN + sEndN + "-星期" + sDay + "<br />"; break;
                }

                if (section == startNum)//判断课程开始时间，确定位置，填写数据
                {
                    tempArray[i][1] = j;//记录上课开始时间（确定数据显示在哪一行）
                    dtSchedule.Rows[j][tempArray[i][0]] = dtSchedule.Rows[j][tempArray[i][0]].ToString() + sTemp;
                }
                if ((Convert.ToInt16(section) > Convert.ToInt16(startNum))&&(Convert.ToInt16(section) < Convert.ToInt16(endNum)))
                {
                    dtSchedule.Rows[j][tempArray[i][0]] = dtSchedule.Rows[j][tempArray[i][0]].ToString() + sTemp;
                }
                if (section == endNum)//判断课程结束时间，记录位置
                {
                    tempArray[i][2] = j;//记录课结束时间
                    if(startNum != endNum)//如开始时间等于结束时间则会重复记录
                    {
                        dtSchedule.Rows[j][tempArray[i][0]] = dtSchedule.Rows[j][tempArray[i][0]].ToString() + sTemp;
                    }
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
        //for (int i = 0; i < 10; i++)
        //{
        //    dtSchedule.Rows[i][0] = "第" + CommonClass.ConvertToChinese(i + 1) + "节";
        //}
        for (int i=0;i<4;i++)
         {
             dtSchedule.Rows[i][0] = "第" + ConvertToChinese2(i + 1) + "节";
         }
        dtSchedule.Rows[4][0] = "中午";
        for (int i = 5; i < 11; i++)
        {
            dtSchedule.Rows[i][0] = "第" + ConvertToChinese2(i + 1) + "节";
        }

        //dispose        
        table.Dispose();
        ds.Dispose();
        sda.Dispose();
        con.Dispose();

        return dtSchedule;
    }

    public static string ConvertToChinese2(int x)
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
            BoundField col = new BoundField();
            col.HeaderText = item.ColumnName;
            col.HtmlEncode = false;
            col.DataField = item.ColumnName;
            col.Visible = true;
            gv.Columns.Add(col);
        }
        return gv;
    }
}