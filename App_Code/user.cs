using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
//下载于51aspx.com
/// <summary>
///user 的摘要说明
/// </summary>
public class user
{
	public user()
	{
		//
		//TODO: 在此处添加构造函数逻辑
		//
	}
    static public bool redirectSet(string str)
    {
        if (str.Trim() == "" || str.Trim() == null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
