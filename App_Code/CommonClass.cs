using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;

/// <summary>
/// CommonClass 的摘要说明
/// </summary>
public class CommonClass
{
    /// <summary> 
    /// 1.获取数据库的连接，返回值需判断是否为null 
    /// </summary> 
    /// <returns></returns> 
    public static SqlConnection GetSqlConnection()
    {
        string strCnn = "Data Source=.;Initial Catalog=test;User ID=sa;Password=config;";
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

    //根据sqlCommand返回DataTable类型变量
    public static DataTable getDataTable(string sqlCom)
    {
        SqlConnection con = CommonClass.GetSqlConnection();
        SqlDataAdapter sda = new SqlDataAdapter();
        sda.SelectCommand = new SqlCommand(sqlCom, con);
        DataSet ds = new DataSet();
        sda.Fill(ds);
        DataTable table = new DataTable();
        table = ds.Tables[0];
        return table;
    }

    public static string CheckApply(string roomN,int dayW,int newSN,int newEN,int newSW,int newEW,string idN)
    {
        string msg = "OK";

        //取修改记录所对应的教室(strRoom)在特定日期（周一至周日intDay）的以下信息：哪些周（intWeek）、哪些节次（intStartNum至intEndNum）有课，记入临时表table（不包含待修改记录本身）
        SqlConnection con = CommonClass.GetSqlConnection();
        SqlDataAdapter sda = new SqlDataAdapter();
        sda.SelectCommand = new SqlCommand(" select aa.intWeek,aa.intStartNum,aa.intEndNum from (select distinct s.intWeek,a.intStartNum,a.intEndNum,a.yearID from RoomApply a,RoomApplySub s  where a.id = s.f_id and a.strRoom = '" + roomN + "' and  a.intDay = " + dayW + " and a.id != '" + idN + "' ) as aa inner join TitleStartEnd t on aa.yearID = t.yearID and t.currentFlag = 'true' ", con);
        DataSet ds = new DataSet();
        sda.Fill(ds);
        DataTable table = new DataTable();
        table = ds.Tables[0];

        //将新信息和临时表中的所有记录依次比较

        for (int i = 0; i < table.Rows.Count; i++)
        {
            int weekN = Convert.ToInt16(table.Rows[i]["intWeek"]);
            int oldSN = Convert.ToInt16(table.Rows[i]["intStartNum"]);
            int oldEN = Convert.ToInt16(table.Rows[i]["intEndNum"]);
            if ((weekN >= newSW ) && (weekN <= newEW))
            {
                if (((newSN < oldSN) && (newEN < oldSN)) || ((newSN > oldEN) && (newEN > oldEN)))//开始节次和结束节次均小于原开始节次，或者均大于原结束节次，该教室才可以排课
                {
                    msg = "OK";
                }
                else
                {
                    msg = roomN + " 第" + weekN + "周 星期" + dayW + " " + "第" + oldSN + "节至第" + oldEN + "节" + " " + "课程冲突";
                    break;
                }
            }
            
        }
        
        

        con.Dispose();
        table.Dispose();
        ds.Dispose();
        sda.Dispose();

        return msg;
    }

    public static int getCurrentWeek()
    {
        //获取当前日期在该学期第几周内
        SqlConnection con = CommonClass.GetSqlConnection();
        SqlDataAdapter sda = new SqlDataAdapter();
        sda.SelectCommand = new SqlCommand("select * from TitleStartEnd where currentFlag = 'true'", con);
        DataSet ds = new DataSet();
        sda.Fill(ds);
        DataTable table = new DataTable();
        table = ds.Tables[0];

        int startDate = Convert.ToDateTime(table.Rows[0][2]).DayOfYear;
        int maxWeek = Convert.ToInt16(table.Rows[0][1]);
        //一.找到第一周的最后一天（先获取1月1日是星期几，从而得知第一周周末是几）
        int firstWeekend = 7 - Convert.ToInt32(DateTime.Parse(DateTime.Today.Year + "-1-1").DayOfWeek)+1;
        //二.获取今天是一年当中的第几天
        int currentDay = DateTime.Today.DayOfYear;
        //三.（今天 减去 第一周周末）/7 等于 距第一周有多少周 再加上第一周的1 就是今天是今年的第几周了
        //    刚好考虑了惟一的特殊情况就是，今天刚好在第一周内，那么距第一周就是0 再加上第一周的1 最后还是1
        int weekTemp = Convert.ToInt32(Math.Ceiling((startDate - firstWeekend) / 7.0));
        int weekNum = Convert.ToInt32(Math.Ceiling((currentDay - firstWeekend) / 7.0)) + 1 - weekTemp;

        table.Dispose();
        ds.Dispose();
        sda.Dispose();
        con.Dispose();

        if ((weekNum < maxWeek) && (weekNum>1))
        {
            return weekNum;
        }
        else return 1;
        
    }

    public static string getTitle()
    {
        //获取currentFlag为true的学期标题
        SqlConnection con = CommonClass.GetSqlConnection();
        SqlDataAdapter sda = new SqlDataAdapter();
        sda.SelectCommand = new SqlCommand("select top 1 * from TitleStartEnd where currentFlag = 'true'", con);
        DataSet ds = new DataSet();
        sda.Fill(ds);
        DataTable table = new DataTable();
        table = ds.Tables[0];

        
        return table.Rows[0][3].ToString(); 
        
    }


    public static string ConvertToChinese(int x)
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
    public static string WeekConvertToChinese(int x)
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


}