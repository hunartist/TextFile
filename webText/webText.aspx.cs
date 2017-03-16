using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class webText_webText : System.Web.UI.Page
{


    protected void GridView1_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        //TextBox1.Text = GridView1.Rows[e.NewSelectedIndex].Cells[2].Text;
        TextBox1.Text = Server.HtmlDecode(GridView1.Rows[e.NewSelectedIndex].Cells[2].Text);
        Label1.Text = "edit:";
        Label2.Text = GridView1.Rows[e.NewSelectedIndex].Cells[1].Text;
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        if (Label2.Text != "Label")
        {
            //SqlDataSource1.UpdateCommand = "UPDATE [record] SET [vc_text] = '" + TextBox1.Text + "' , [dt_date] = getdate() WHERE [id] = " + Label2.Text;
            //SqlDataSource1.InsertCommand = "INSERT INTO [record] ([vc_text], [dt_date]) VALUES ('" + TextBox1.Text + "', getdate())";

            SqlDataSource1.UpdateCommand = "UPDATE [record] SET [vc_text] = @vc_text , [dt_date] = getdate() WHERE [id] = @original_id";
            SqlDataSource1.InsertCommand = "INSERT INTO [record] ([vc_text], [dt_date]) VALUES (@vc_text, getdate())";
            SqlDataSource1.UpdateParameters["vc_text"].DefaultValue = Server.HtmlEncode(TextBox1.Text);
            SqlDataSource1.UpdateParameters["original_id"].DefaultValue = Label2.Text;
            //SqlDataSource1.InsertParameters["vc_text"].DefaultValue = TextBox1.Text;
            SqlDataSource1.InsertParameters["vc_text"].DefaultValue = Server.HtmlEncode(TextBox1.Text);

            if (Label2.Text == "new")
            {
                SqlDataSource1.Insert();
            }
            else
            {
                SqlDataSource1.Update();
            }            
            GridView1.DataBind();
        }
            
    }
    protected void GridView1_RowDeleted(object sender, GridViewDeletedEventArgs e)
    {
        Label1.Text = "Label";
        Label2.Text = "Label";
    }
    protected void Button2_Click(object sender, EventArgs e)
    {
        Label1.Text = "insert";
        Label2.Text = "new";
        
    }


    protected void Button3_Click(object sender, EventArgs e)
    {
        if (Label2.Text != "Label")
        {
            SqlDataSource1.UpdateCommand = "UPDATE [record] SET [vc_text] = '" + TextBox1.Text + "' , [dt_date] = getdate() WHERE [id] = " + Label2.Text;
            SqlDataSource1.InsertCommand = "INSERT INTO [record] ([vc_text], [dt_date]) VALUES ('" + TextBox1.Text + "', getdate())";
            if (Label2.Text == "new")
            {
                SqlDataSource1.Insert();
            }
            else
            {
                SqlDataSource1.Update();
            }
            GridView1.DataBind();
        }
    }
}
