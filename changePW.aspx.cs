using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class changePW : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string uName = Session["user"].ToString();
        DataTable table = new DataTable();
        table = CommonClass.getDataTable("select top 1 * from raUser where name = '" + uName + "'");

        tbName.Text = table.Rows[0]["name"].ToString();
    }

    protected void btOK_Click(object sender, EventArgs e)
    {
        string uName = Session["user"].ToString();
        DataTable table = new DataTable();
        table = CommonClass.getDataTable("select top 1 * from raUser where name = '" + uName + "'");

        if(tbOldPW.Text == table.Rows[0]["pw"].ToString())
        {
            if (tbNewPW1.Text == tbNewPW2.Text)
            {
                if(tbNewPW1.Text == "")
                {
                    Response.Write("<script>alert('新密码不能为空！')</script>");
                    return;
                }
                else
                {
                    SqlConnection con = CommonClass.GetSqlConnection();
                    SqlDataAdapter sda = new SqlDataAdapter("select * from raUser where name = '"+ uName+"'", con);
                    sda.UpdateCommand = new SqlCommand("update raUser set pw = @pw where name = @name", con);

                    sda.UpdateCommand.Parameters.Add("@pw", SqlDbType.NVarChar, 50, "pw");
                    SqlParameter parameter = sda.UpdateCommand.Parameters.Add("@name", SqlDbType.NVarChar, 50, "name");
                    parameter.SourceVersion = DataRowVersion.Current;

                    DataTable dt = new DataTable();
                    sda.Fill(dt);

                    DataRow nameRow = dt.Rows[0];
                    nameRow["pw"] = tbNewPW1.Text;

                    sda.Update(dt);
                    Response.Write("<script>alert('修改成功！')</script>");
                    Response.Redirect("templogin.aspx");
                }
                    

            }
            else
            {
                Response.Write("<script>alert('两次新密码不一致！')</script>");
                tbNewPW1.Text = "";
                tbNewPW2.Text = "";
                return;
            }
        }
        else
        {
            Response.Write("<script>alert('旧密码错误！')</script>");
            tbOldPW.Text = "";
            return;
        }
    }
}