using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Text.RegularExpressions;
using System.Configuration;

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
        //string strCnn = "Data Source=.;Initial Catalog=test;User ID=sa;Password=config;";
        string strCnn = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["webTestConnectionString"].ToString();
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

    public static string CheckApply(string roomN,int dayW,int newSN,int newEN,string weekData,string idN)
    {
        string msg = "OK"; 

        //取修改记录所对应的教室(strRoom)在特定日期（周一至周日intDay）的以下信息：哪些周（intWeek）、哪些节次（intStartNum至intEndNum）有课，记入临时表table（不包含待修改记录本身）
        SqlConnection con = CommonClass.GetSqlConnection();
        SqlDataAdapter sda = new SqlDataAdapter();
        string constr = string.Format("select aa.intWeek, aa.intStartNum, aa.intEndNum from(select distinct s.intWeek, a.intStartNum, a.intEndNum, l.yearID from RoomApply a, RoomApplySub s, ApplyList l where a.id = s.f_id and a.applyid = l.applyid and a.strRoom = '" + roomN + "' and  a.intDay = " + dayW + " and a.id != '" + idN + "') as aa inner join TitleStartEnd t on aa.yearID = t.yearID and t.currentFlag = 'true' and aa.intWeek in ({0})", weekData);
        sda.SelectCommand = new SqlCommand(constr, con);
        DataSet ds = new DataSet();
        sda.Fill(ds);
        DataTable table = new DataTable();
        table = ds.Tables[0];

        //临时表中的所有记录依次和新信息比较
        string[] newWeek = Regex.Split(weekData, ",");

        for (int i = 0; i < table.Rows.Count; i++)
        {
            foreach (string item in newWeek)
            {
                try
                {
                    int week = Convert.ToInt16(item);
                }
                catch (Exception)
                {
                    msg = "regular int error";
                    return msg;
                }
                int weekN = Convert.ToInt16(item);
                int weekOld = Convert.ToInt16(table.Rows[i]["intWeek"]);                

                if (weekN == weekOld)
                {
                    int oldSN = Convert.ToInt16(table.Rows[i]["intStartNum"]);
                    int oldEN = Convert.ToInt16(table.Rows[i]["intEndNum"]);
                    if (((newSN < oldSN) && (newEN < oldSN)) || ((newSN > oldEN) && (newEN > oldEN)))//开始节次和结束节次均小于原开始节次，或者均大于原结束节次，该教室才可以排课
                    {
                        msg = "OK";
                    }
                    else
                    {
                        msg = roomN + " 第" + weekN + "周 星期" + dayW + " " + "第" + oldSN + "节至第" + oldEN + "节" + " " + "课程冲突";
                        return msg;
                    }
                }
                    
            }
        }
            


        ////临时表中的所有记录依次和新信息比较

        //for (int i = 0; i < table.Rows.Count; i++)
        //{
        //    int weekN = Convert.ToInt16(table.Rows[i]["intWeek"]);
        //    int oldSN = Convert.ToInt16(table.Rows[i]["intStartNum"]);
        //    int oldEN = Convert.ToInt16(table.Rows[i]["intEndNum"]);
        //    if ((weekN >= newSW) && (weekN <= newEW))
        //    {
        //        if (((newSN < oldSN) && (newEN < oldSN)) || ((newSN > oldEN) && (newEN > oldEN)))//开始节次和结束节次均小于原开始节次，或者均大于原结束节次，该教室才可以排课
        //        {
        //            msg = "OK";
        //        }
        //        else
        //        {
        //            msg = roomN + " 第" + weekN + "周 星期" + dayW + " " + "第" + oldSN + "节至第" + oldEN + "节" + " " + "课程冲突";
        //            return msg;
        //        }
        //    }

        //}





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

    public static string getCurYearID()
    {
        //获取currentFlag为true的学期id
        SqlConnection con = CommonClass.GetSqlConnection();
        SqlDataAdapter sda = new SqlDataAdapter();
        sda.SelectCommand = new SqlCommand("select top 1 * from TitleStartEnd where currentFlag = 'true'", con);
        DataSet ds = new DataSet();
        sda.Fill(ds);
        DataTable table = new DataTable();
        table = ds.Tables[0];

        return table.Rows[0][4].ToString();

    }

    public static int getCurMaxWeek()
    {
        //获取currentFlag为true的学期结束周
        SqlConnection con = CommonClass.GetSqlConnection();
        SqlDataAdapter sda = new SqlDataAdapter();
        sda.SelectCommand = new SqlCommand("select top 1 * from TitleStartEnd where currentFlag = 'true'", con);
        DataSet ds = new DataSet();
        sda.Fill(ds);
        DataTable table = new DataTable();
        table = ds.Tables[0];

        return Convert.ToInt16(table.Rows[0][1]);
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

    //“周”正则转换为存储过程可用的逗号分隔数据
    public static bool regToData(string strReg,out string strData)
    {
        int maxWeek = getCurMaxWeek();
        //两位数字+横杆（-）+两位数字 或 数字 + 逗号/横杠（总字符数1至50 + 4）（逗号/横杠不可在开头，不可在结尾，可以为纯数字）
        RegexStringValidator regweek = new RegexStringValidator("^[1-9]{0,1}[0-9]{1,1}[-][1-9]{0,1}[0-9]{1,1}$|^[1-9]{0,1}[0-9]{1,1}[0-9,-]{0,50}[1-9]{0,1}[0-9]{0,1}$");
        RegexStringValidator regweekMN = new RegexStringValidator("^[1-9]{0,1}[0-9]{1,1}[-][1-9]{0,1}[0-9]{1,1}$");
        string strWeekRegToData = "";
        try
        {
            regweek.Validate(strReg);
        }
        catch (Exception)
        {
            strData = "存在非法字符或格式不正确，输入示例：1-3或1,3,5,6,8或1";
            return false;
        }        
        if (strReg.Contains("-"))
        {
            if (strReg.Contains(","))//reg含有逗号，横杆，数字
            {
                string[] weekD = strReg.Split(',');//按逗号切割reg
                List<string> weekOut = new List<string>();
                for (int i = 0; i < weekD.Count(); i++)
                {
                    string weekDataI = weekD[i];
                    if (weekDataI == "")
                    {
                        strData = "逗号前后必须都有数字";
                        return false;
                    }                    
                    if (weekDataI.Contains("-"))
                    {
                        try
                        {
                            regweekMN.Validate(weekDataI);
                        }
                        catch (Exception)
                        {
                            strData = "按逗号切割后存在非法字符，输入示例：1-3或1,3,5,6,8或1";
                            return false;
                        }
                        //切割后数据为m-n
                        string[] weekMN = weekDataI.Split('-');
                        int firstW = Convert.ToInt16(weekMN[0]);
                        int lastW = Convert.ToInt16(weekMN[1]);
                        //周数不可为0
                        if ((firstW == 0) | (lastW == 0))
                        {
                            strData = "切割后周数不可为0";
                            return false;
                        }
                        //开始周不可大于结束周
                        if (firstW > lastW)
                        {
                            strData = "切割后开始周不可大于结束周";
                            return false;
                        }
                        //结束周不可大于当前学期最大周数
                        if (lastW > maxWeek)
                        {
                            strData = "切割后结束周不可大于当前学期最大周数";
                            return false;
                        }
                        //m-n转换为m,m+1,...,n-1,n
                        for (int j = firstW; j <= lastW; j++)
                        {
                            //strWeekRegToData = strWeekRegToData + j + ",";
                            weekOut.Add(j + ",");
                        }                                     
                    }
                    else
                    {
                        //切割后数据为m
                        if (Convert.ToInt16(weekDataI) == 0)
                        {
                            strData = "周数不可为0";
                            return false;
                        }
                        if (Convert.ToInt16(weekDataI) > maxWeek)
                        {
                            strData = "周不可大于当前学期最大周数";
                            return false;
                        }
                        //strWeekRegToData = strWeekRegToData + weekDataI + ",";
                        weekOut.Add(weekDataI + ",");
                    }
                }
                if (weekOut.Distinct().Count() != weekOut.Count())
                {
                    strData = "切割后周有重复";
                    return false;
                }
                //weekOut.Sort();
                weekOut = weekOut.OrderBy(s => int.Parse(Regex.Match(s, @"\d+").Value)).ToList();
                for (int i = 0; i < weekOut.Count; i++)
                {
                    strWeekRegToData = strWeekRegToData + weekOut[i];
                }
                if (strWeekRegToData != "")
                {
                    strWeekRegToData = strWeekRegToData.Substring(0, strWeekRegToData.Length - 1);
                    strData = strWeekRegToData;
                    return true;
                }
                else
                {
                    strData = "切割后reg转data失败,只有“-”和数字";
                    return false;
                }
            }
            else
            {
                //weekReg字段只有“-”和数字
                try
                {
                    regweekMN.Validate(strReg);
                }
                catch (Exception)
                {
                    strData = "存在非法字符，只有“-”和数字输入示例：1-3";
                    return false;
                }

                string[] weekD = strReg.Split('-');
                
                //横杠两端必须有数字
                if ((weekD[0] == "") || (weekD[1] == ""))
                {
                    strData = "横杠两端必须有数字";
                    return false;
                }

                int firstWeek = Convert.ToInt16(weekD[0]);
                int lastWeek = Convert.ToInt16(weekD[1]);
                //横杠数只能为1
                if (weekD.Count() !=2)
                {
                    strData = "横杠数只能为1";
                    return false;
                }                
                //周数不可为0
                if ((firstWeek == 0) | (lastWeek == 0))
                {
                    strData = "周数不可为0";
                    return false;
                }
                //开始周不可大于结束周
                if (firstWeek > lastWeek)
                {
                    strData = "开始周不可大于结束周";
                    return false;
                }
                //结束周不可大于当前学期最大周数
                if (lastWeek > maxWeek)
                {
                    strData = "结束周不可大于当前学期最大周数";
                    return false;
                }
                //m-n转换为m,m+1,...,n-1,n
                for (int i = firstWeek; i <= lastWeek; i++)
                {
                    strWeekRegToData = strWeekRegToData + i + ",";
                }
                if (strWeekRegToData != "")
                {
                    strWeekRegToData = strWeekRegToData.Substring(0, strWeekRegToData.Length - 1);
                    strData = strWeekRegToData;
                    return true;
                }
                else
                {
                    strData = "reg转data失败,只有“-”和数字";
                    return false;
                }
            }
            
                       
        }
        else if (strReg.Contains(","))//reg含逗号和数字，逗号不在开头结尾
        {
            string[] weekD = strReg.Split(',');
            for (int i = 0;i < weekD.Count(); i++)
            {
                if (weekD[i] == "")
                {
                    strData = "逗号前后必须都有数字";
                    return false;
                }
                if (Convert.ToInt16(weekD[i]) == 0)
                {
                    strData = "周数不可为0";
                    return false;
                }
                if (Convert.ToInt16(weekD[i]) > maxWeek)
                {
                    strData = "周不可大于当前学期最大周数";
                    return false;
                }
                if (weekD.Distinct().Count() != weekD.Length)
                {
                    strData = "周有重复";
                    return false;
                }
                

            strWeekRegToData = strWeekRegToData + weekD[i] + ",";
            }
            if (strWeekRegToData != "")
            {
                strWeekRegToData = strWeekRegToData.Substring(0, strWeekRegToData.Length - 1);
                strData = strWeekRegToData;
                return true;
            }
            else
            {
                strData = "reg转data失败,逗号和数字";
                return false;
            }

        }
        else //reg只有数字
        {
            if (Convert.ToInt16(strReg) == 0)
            {
                strData = "周数不可为0";
                return false;
            }
            if (Convert.ToInt16(strReg) > maxWeek)
            {
                strData = "周不可大于当前学期最大周数";
                return false;
            }            
        }



        strData = strReg;
        return true;
    }

    public static string normalCheck(string weekRegC,int startNC,int endNC,int dayWC,string ClassNC,string TeacherNC)
    {
        string msg = "OK";
        if (weekRegC == string.Empty)
        {
            msg = "周输入不可为空";
            return msg;
        }        

        RegexStringValidator regday = new RegexStringValidator("^[1-7]{1}$");
        RegexStringValidator regnum = new RegexStringValidator("^[1-9]{1}$|^1[10]{1}");
        try
        {
            regday.Validate(dayWC.ToString());
        }
        catch
        {            
            msg = "日期只能填数字1至数字7";
            return msg;
        }
        try
        {
            regnum.Validate(startNC.ToString());
            regnum.Validate(endNC.ToString());
        }
        catch
        {
            msg = "节次只能填数字1至数字11";
            return msg;
        }

        if ((startNC != 11)&&(endNC !=11))
        {
            if (startNC > endNC)
            {
                msg = "开始节次不能大于结束节次";
                return msg;
            }
        }
        if ((startNC == 11)|| (endNC == 11))
        {
            if(startNC != endNC)
            {
                msg = "节次如果是中午,则开始和结束都必须是中午";
                return msg;
            }
        }
        

        if (ClassNC == string.Empty)
        {
            msg = "班级名未填写";
            return msg;
        }
        if (TeacherNC == string.Empty)
        {
            msg = "教师名未填写";
            return msg;
        }
        return msg;
    }


}