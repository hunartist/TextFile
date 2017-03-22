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
        LabelMsg.Visible = false;
    }

    protected void ButtonNew_Click(object sender, EventArgs e)
    {

    }

    protected void GridView10_RowUpdated(object sender, GridViewUpdatedEventArgs e)
    {
        //SqlDataSourceRoomApply.UpdateCommand = "UPDATE [RoomApply] SET [strRoom] = @strRoom , [intDay] = @intDay , [intStartNum] = @intStartNum , [intEndNum] = @intEndNum , [intStartWeek] = @intStartWeek , [intEndWeek] = @intEndWeek , [strName] = @strName , [strClass] = @strClass , [strTeacher] = @strTeacher WHERE [id] = @original_id";
        SqlDataSourceRoomApply.UpdateParameters["action"].DefaultValue = "update";
        SqlDataSourceRoomApply.UpdateParameters["strRoom"].DefaultValue = e.NewValues["strRoom"].ToString();
        SqlDataSourceRoomApply.UpdateParameters["intDay"].DefaultValue = e.NewValues["intDay"].ToString();
        SqlDataSourceRoomApply.UpdateParameters["intStartNum"].DefaultValue = e.NewValues["intStartNum"].ToString();
        SqlDataSourceRoomApply.UpdateParameters["intEndNum"].DefaultValue = e.NewValues["intEndNum"].ToString();
        SqlDataSourceRoomApply.UpdateParameters["intStartWeek"].DefaultValue = e.NewValues["intStartWeek"].ToString();
        SqlDataSourceRoomApply.UpdateParameters["intEndWeek"].DefaultValue = e.NewValues["intEndWeek"].ToString();
        SqlDataSourceRoomApply.UpdateParameters["strName"].DefaultValue = e.NewValues["strName"].ToString();
        SqlDataSourceRoomApply.UpdateParameters["strClass"].DefaultValue = e.NewValues["strClass"].ToString();
        SqlDataSourceRoomApply.UpdateParameters["strTeacher"].DefaultValue = e.NewValues["strTeacher"].ToString();
        SqlDataSourceRoomApply.UpdateParameters["id"].DefaultValue = LabelID.Text;

        SqlDataSourceRoomApply.Update();
        GridView10.DataBind();
        LabelMsg.Visible = false;
        DropDownListDepart.Visible = true;
        Response.Write("<script>alert('操作成功')</script>");
    }

    protected void GridView10_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        LabelID.Text = GridView10.Rows[e.NewSelectedIndex].Cells[1].Text;
    }

    protected void GridView10_RowEditing(object sender, GridViewEditEventArgs e)
    {
        LabelID.Text = GridView10.Rows[e.NewEditIndex].Cells[1].Text;
        DropDownListDepart.Visible = false;
    }

    protected void GridView10_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        RegexStringValidator regday = new RegexStringValidator("^[1-7]{1}$");
        RegexStringValidator regnum = new RegexStringValidator("^[1-9]{1}$|10");

        try
        {
            regday.Validate(e.NewValues["intDay"]);
        }
        catch
        {
            LabelMsg.Visible = true;
            LabelMsg.Text = "日期只能填数字1至数字7";
            e.Cancel = true;
        }
        try
        {
            regnum.Validate(e.NewValues["intStartNum"]);
            regnum.Validate(e.NewValues["intEndNum"]);
        }
        catch
        {
            LabelMsg.Visible = true;
            LabelMsg.Text = "节次只能填数字1至数字10";
            e.Cancel = true;
        }
        if (Convert.ToInt16(e.NewValues["intStartNum"])> Convert.ToInt16(e.NewValues["intEndNum"]))
        {
            LabelMsg.Visible = true;
            LabelMsg.Text = "开始节次不能大于结束节次";
            e.Cancel = true;
        }
        if (Convert.ToInt16(e.NewValues["intStartWeek"]) > Convert.ToInt16(e.NewValues["intEndWeek"]))
        {
            LabelMsg.Visible = true;
            LabelMsg.Text = "开始周不能大于周";
            e.Cancel = true;
        }

    }

}
