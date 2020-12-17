using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class EditTemplate1 : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string[] filePaths = Directory.GetFiles(Server.MapPath("~/Template"));
            for (int i = 0; i < filePaths.Length; i++)
            {
                if (filePaths[i].EndsWith("htm") || filePaths[i].EndsWith("html"))
                    ListFile.Items.Add(filePaths[i].Substring(filePaths[i].LastIndexOf("\\") + 1));
            }
        }
        ListFile.Focus();
    }
    protected void BtnSelect_Click(object sender, EventArgs e)
    {
        Session["HtmlFile"] = ListFile.Text;
        Response.Redirect("EditTemplate.aspx");
    }
}