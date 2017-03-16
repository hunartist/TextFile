using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;

public partial class testImg : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        DataTable dt = new DataTable();
        int i;

        //DirectoryInfo foldinfo = new DirectoryInfo(@"D:\ftp_file\webFile\");
        DirectoryInfo foldinfo = new DirectoryInfo(@"E:\TextFile\img");
        FileSystemInfo[] dirs = foldinfo.GetFileSystemInfos();
        dt.Columns.Add("imgName");
        for (i = 0; i < dirs.GetLength(0); i++)
        {
            dt.Rows.Add(dirs[i]);
        }
        this.GridView1.DataSource = dt;
        this.GridView1.DataBind();
    }
}