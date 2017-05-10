using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
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
        string uName = TextBox1.Text;
        string uPW = TextBox2.Text;
                
        DataTable table = new DataTable();
        table = CommonClass.getDataTable("select * from raUser where name = '" + uName + "' and pw = '" + uPW + "'");     

        if (table.Rows.Count == 1)
        {
            Session["user"] = table.Rows[0]["name"].ToString();
            Session["cdep"] = table.Rows[0]["cdepid"].ToString();
            Response.Redirect("ApplyEdit.aspx");
        }
        else
        {
            Response.Write("<script>alert('用户名或密码错误！')</script>");
            setTB();
            return;
        }

        //if (TextBox1.Text == "test" && TextBox2.Text == "ipconfig") //可以从数据库中用户
        //{
        //    Session["user"] = TextBox1.Text;
        //    Session["dep"] = "0s"; 
        //    Response.Redirect("ApplyEdit.aspx");
        //}
        //if (TextBox1.Text == "other" && TextBox2.Text == "ipconfig") //可以从数据库中用户
        //{
        //    Session["user"] = TextBox1.Text;
        //    Session["dep"] = "1o";
        //    Response.Redirect("ApplyEdit.aspx");
        //}
        //else
        //{
        //    //Page.RegisterStartupScript("javascript", "<script language=javascript>alert('用户名或密码错误！');</script>");
        //    Response.Write("<script>alert('用户名或密码错误！')</script>");
        //    setTB();
        //    return;
        //}
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
