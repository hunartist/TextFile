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
using System.IO;

public partial class webText_webFile : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
	uploadText.Text = "选择文件(500M以内)";
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        if (FileUpload1.HasFile)
        {
            try
            {
                FileUpload1.SaveAs(@"D:\ftp_file\webFile\" + FileUpload1.FileName);
            }
            catch (InvalidCastException ee)
            {
                throw (ee);
            }
            uploadText.Text = FileUpload1.PostedFile.FileName + "已上传至" + @"D:\ftp_file\webFile\";
            readFolder();
        }
        else
        {
            uploadText.Text = "选择文件(500M以内)";
        }
       
    }

    

    protected void Button2_Click(object sender, EventArgs e)
    {
        readFolder();
    }

    public void readFolder()
    {
        DataTable dt = new DataTable();
        int i;

        DirectoryInfo foldinfo = new DirectoryInfo(@"D:\ftp_file\webFile\");
        FileSystemInfo[] dirs = foldinfo.GetFileSystemInfos();
        dt.Columns.Add("filename");
        for (i = 0; i < dirs.GetLength(0); i++)
        {
            dt.Rows.Add(dirs[i]);
        }
        this.GridView1.DataSource = dt;
        this.GridView1.DataBind();
    }

}
