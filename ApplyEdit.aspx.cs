using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
//下载于51aspx.com
public partial class NextWebF : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //if (!IsPostBack)    //Page.IsPostBack
        //{
        //    if (user.redirectSet(Convert.ToString(Session["user"])))
        //        Response.Redirect("tempLogin.aspx");

        //}
    }

    protected void ButtonNew_Click(object sender, EventArgs e)
    {

    }

    protected void GridView10_RowUpdated(object sender, GridViewUpdatedEventArgs e)
    {
        SqlDataSourceRoomApply.UpdateCommand = "UPDATE [RoomApply] SET [strRoom] = @strRoom , [intDay] = @intDay , [intStartNum] = @intStartNum , [intEndNum] = @intEndNum , [intStartWeek] = @intStartWeek , [intEndWeek] = @intEndWeek , [strName] = @strName , [strClass] = @strClass , [strTeacher] = @strTeacher WHERE [id] = @original_id";
        SqlDataSourceRoomApply.UpdateParameters["strRoom"].DefaultValue = e.NewValues["strRoom"].ToString();
        SqlDataSourceRoomApply.UpdateParameters["intDay"].DefaultValue = e.NewValues["intDay"].ToString();
        SqlDataSourceRoomApply.UpdateParameters["intStartNum"].DefaultValue = e.NewValues["intStartNum"].ToString();
        SqlDataSourceRoomApply.UpdateParameters["intEndNum"].DefaultValue = e.NewValues["intEndNum"].ToString();
        SqlDataSourceRoomApply.UpdateParameters["intStartWeek"].DefaultValue = e.NewValues["intStartWeek"].ToString();
        SqlDataSourceRoomApply.UpdateParameters["intEndWeek"].DefaultValue = e.NewValues["intEndWeek"].ToString();
        SqlDataSourceRoomApply.UpdateParameters["strName"].DefaultValue = e.NewValues["strName"].ToString();
        SqlDataSourceRoomApply.UpdateParameters["strClass"].DefaultValue = e.NewValues["strClass"].ToString();
        SqlDataSourceRoomApply.UpdateParameters["strTeacher"].DefaultValue = e.NewValues["strTeacher"].ToString();
        SqlDataSourceRoomApply.UpdateParameters["original_id"].DefaultValue = LabelID.Text;

        SqlDataSourceRoomApply.Update();
        GridView10.DataBind();
    }

    protected void GridView10_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        LabelID.Text = GridView10.Rows[e.NewSelectedIndex].Cells[1].Text;
    }

    protected void GridView10_RowEditing(object sender, GridViewEditEventArgs e)
    {
        LabelID.Text = GridView10.Rows[e.NewEditIndex].Cells[1].Text;
    }
}
