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

    public static string CheckApply(string roomN,int dayW,int newSN,int newEN,string idN)
    {
        string msg = "OK";

        //取修改记录所对应的教室(strRoom)在特定日期（周一至周日intDay）的以下信息：哪些周（intWeek）、哪些节次（intStartNum至intEndNum）有课，记入临时表table（不包含待修改记录本身）
        SqlConnection con = CommonClass.GetSqlConnection();
        SqlDataAdapter sda = new SqlDataAdapter();
        sda.SelectCommand = new SqlCommand(" select distinct s.intWeek,a.intStartNum,a.intEndNum from RoomApply a,RoomApplySub s  where a.id = s.f_id and a.strRoom = " + roomN + " and  a.intDay = " + dayW + " and a.id != '" + idN + "'", con);
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
            if (((newSN < oldSN) && (newEN < oldSN)) || ((newSN > oldEN) && (newEN > oldEN)))//开始节次和结束节次均小于原开始节次，或者均大于原结束节次，该教室才可以排课
            {
                msg = "OK";
            }
            else
            {
                msg = roomN + " 第" + weekN + "周 星期" + dayW + " " + "课程冲突";
                break;
            }
        }
        
        

        con.Dispose();
        table.Dispose();
        ds.Dispose();
        sda.Dispose();

        return msg;
    }


}