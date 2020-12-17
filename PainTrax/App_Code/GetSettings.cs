using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;

/// <summary>
/// Summary description for GetSettings
/// </summary>
public class GetSettings
{

    public bool forwardCC { get; set; }
    public bool forwardPE { get; set; }
    public bool forwardROM { get; set; }

    public GetSettings()
    {
        this.settings();
    }

    private void settings()
    {
        try
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(HttpContext.Current.Server.MapPath("~/Template/Default_Admin.xml"));
            XmlNodeList nodeList = xmlDoc.DocumentElement.SelectNodes("/Defaults/Settings");
            foreach (XmlNode node in nodeList)
            {
                forwardCC = node.SelectSingleNode("forwardCC") == null ? false : Convert.ToBoolean(node.SelectSingleNode("forwardCC").InnerText);
                forwardPE = node.SelectSingleNode("forwardPE") == null ? false : Convert.ToBoolean(node.SelectSingleNode("forwardPE").InnerText);
                forwardROM = node.SelectSingleNode("forwardROM") == null ? false : Convert.ToBoolean(node.SelectSingleNode("forwardROM").InnerText);
            }
        }
        catch (Exception ex)
        {
        }

    }
}