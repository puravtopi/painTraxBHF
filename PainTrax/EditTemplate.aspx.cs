using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class EditTemplate : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            HTMLCon.InnerHtml = File.ReadAllText(Server.MapPath("~/Template/DocumentPrintIE.html"));

        }
        else
        {
            string path = Server.MapPath("~/Template/Demo.html");
            File.WriteAllText(path, HtmlContent.Value);
            HTMLCon.InnerHtml = File.ReadAllText(Server.MapPath("DocumentPrintIE.html"));
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        //string content = hdHtml.Value;
        //string path = Server.MapPath("~/Template/Demo.html");
        //System.IO.File.WriteAllText(path, content);
    }
}