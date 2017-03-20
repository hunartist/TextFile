using System;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

public partial class _Default : System.Web.UI.Page 
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
            setTB();
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        if (TextBox1.Text == "test" && TextBox2.Text == "ipconfig") //可以从数据库中用户
        {
            Session["user"] = TextBox1.Text;
            Response.Redirect("ApplyEdit.aspx");
        }
        else
        {
            Page.RegisterStartupScript("javascript", "<script language=javascript>alert('用户名或密码错误！');</script>");
            setTB();
            return;
        }
    }
    protected void Button2_Click(object sender, EventArgs e)
    {
        setTB();
    }
    protected void Button3_Click(object sender, EventArgs e)
    {
        Session.Abandon();
    }

    public void setTB()
    {
        TextBox1.Text = "";
        TextBox2.Text = "";
        TextBox1.Focus();
    }
}
