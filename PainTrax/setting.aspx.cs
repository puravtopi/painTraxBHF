using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

public partial class setting : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
            bindValues();
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            XmlDocument xmlDoc = new XmlDocument();
            string fileName = Server.MapPath("~/Template/Default_Admin.xml");
            xmlDoc.Load(fileName);
            XmlNode node = xmlDoc.DocumentElement.SelectSingleNode("/Defaults/Settings");

            node.SelectSingleNode("forwardCC").InnerText = chkCC.Checked.ToString();
            node.SelectSingleNode("forwardPE").InnerText = chkPE.Checked.ToString();
            node.SelectSingleNode("forwardROM").InnerText = chkROM.Checked.ToString();

            xmlDoc.Save(fileName);
            divSuccess.Attributes.Add("style", "display:block");
            divfail.Attributes.Add("style", "display:none");
        }
        catch (Exception ex)
        {
            divfail.Attributes.Add("style", "display:block");
            divSuccess.Attributes.Add("style", "display:none");
        }
    }

    private void bindValues()
    {
        try
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(Server.MapPath("~/Template/Default_Admin.xml"));
            XmlNodeList nodeList = xmlDoc.DocumentElement.SelectNodes("/Defaults/Settings");
            foreach (XmlNode node in nodeList)
            {
                chkCC.Checked = node.SelectSingleNode("forwardCC") == null ? chkCC.Checked : Convert.ToBoolean(node.SelectSingleNode("forwardCC").InnerText);
                chkPE.Checked = node.SelectSingleNode("forwardPE") == null ? chkPE.Checked : Convert.ToBoolean(node.SelectSingleNode("forwardPE").InnerText);
                chkROM.Checked = node.SelectSingleNode("forwardROM") == null ? chkPE.Checked : Convert.ToBoolean(node.SelectSingleNode("forwardROM").InnerText);
            }
        }
        catch (Exception ex)
        {
        }

    }
}