using IntakeSheet.BLL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.IO;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.IO.Compression;
using System.Xml;
using System.Reflection;
using System.Diagnostics;
using Xceed.Words.NET;
using System.Text;
using System.Net;

public partial class PatientIntakeList : System.Web.UI.Page
{
    DBHelperClass db = new DBHelperClass();

    public int iCounter = 1;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["uname"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            Session["patientFUId"] = "";
            BindPatientIEDetails();
            bindLocation();
            txtSearch.Attributes.Add("onkeydown", "funfordefautenterkey1(" + btnSearch.ClientID + ",event)");
            txtFromDate.Attributes.Add("onkeydown", "funfordefautenterkey1(" + btnSearch.ClientID + ",event)");
            txtEndDate.Attributes.Add("onkeydown", "funfordefautenterkey1(" + btnSearch.ClientID + ",event)");
        }
    }

    protected void BindPatientIEDetails(string patientId = null, string searchText = null)
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString))
        {
            SqlCommand cmd = new SqlCommand("nusp_GetPatientIEDetails", con);

            if (!string.IsNullOrEmpty(patientId))
            {
                cmd.Parameters.AddWithValue("@Patient_Id", hfPatientId.Value);
            }
            else if (!string.IsNullOrEmpty(searchText) && string.IsNullOrEmpty(patientId))
            {
                string keyword = searchText.TrimStart(("Mrs. ").ToCharArray());
                cmd.Parameters.AddWithValue("@SearchText", keyword);
            }
            else
            {
                if (Session["Location"] != null)
                {
                    cmd.Parameters.AddWithValue("@LocationId", Convert.ToString(Session["Location"]));
                }
            }

            if (!string.IsNullOrEmpty(txtFromDate.Text) && !string.IsNullOrEmpty(txtEndDate.Text))
            {
                cmd.Parameters.AddWithValue("@SDate", txtFromDate.Text);
                cmd.Parameters.AddWithValue("@EDate", txtEndDate.Text);

            }
            else if (!string.IsNullOrEmpty(txtFromDate.Text) && string.IsNullOrEmpty(txtEndDate.Text))
            {
                cmd.Parameters.AddWithValue("@SDate", txtFromDate.Text);
            }

            cmd.CommandType = CommandType.StoredProcedure;
            con.Open();
            DataTable dt = new DataTable();
            dt.Load(cmd.ExecuteReader());

            string _query = "";
            DataRow row;
            if (rbllisttype.SelectedItem.Value == "0")
            {
                _query = " DOE is not null";

            }
            else
                _query = " DOE is null";

            if (ddl_location.SelectedIndex > 0)
            {
                if (string.IsNullOrEmpty(_query))
                {
                    _query = " Location_ID=" + ddl_location.SelectedItem.Value;
                }
                else
                    _query = _query + " and Location_ID=" + ddl_location.SelectedItem.Value;
            }
            else
            {
                if (string.IsNullOrEmpty(_query))
                    _query = " Location_ID in (" + Session["Locations"].ToString() + ")";
                else
                    _query = _query + " and Location_ID in (" + Session["Locations"].ToString() + ")";
            }



            try
            {
                dt = dt.Select(_query).CopyToDataTable();
                DataView dv = dt.DefaultView;
                dv.Sort = "LastTestDate desc";
                dt = dv.ToTable();
            }
            catch (Exception ex)
            {
                dt = null;
            }


            con.Close();
            Session["iedata"] = dt;

            gvPatientDetails.DataSource = dt;
            gvPatientDetails.DataBind();
            hfPatientId.Value = null;
        }
    }

    protected void gvPatientDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvPatientDetails.PageIndex = e.NewPageIndex;
        BindPatientIEDetails(hfPatientId.Value, txtSearch.Text.Trim());
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        BindPatientIEDetails(hfPatientId.Value, txtSearch.Text.Trim());
    }

    protected void gvPatientFUDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView gvPatientFUDetails = (sender as GridView);
        hfCurrentlyOpened.Value = gvPatientFUDetails.ToolTip;
        gvPatientFUDetails.PageIndex = e.NewPageIndex;
        bindFUDetails(gvPatientFUDetails);
    }

    protected void bindFUDetails(GridView gvPatientFUDetails)
    {
        BusinessLogic bl = new BusinessLogic();
        gvPatientFUDetails.DataSource = Session["dtfu"] = bl.GetFUDetails(Convert.ToInt32(gvPatientFUDetails.ToolTip));
        gvPatientFUDetails.DataBind();
    }

    protected void OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string patientIEId = gvPatientDetails.DataKeys[e.Row.RowIndex].Value.ToString();
            BusinessLogic bl = new BusinessLogic();
            GridView gvPatientFUDetails = e.Row.FindControl("gvPatientFUDetails") as GridView;

            System.Web.UI.WebControls.Image img = e.Row.FindControl("plusimg") as System.Web.UI.WebControls.Image;



            gvPatientFUDetails.ToolTip = patientIEId;
            gvPatientFUDetails.DataSource = bl.GetFUDetails(Convert.ToInt32(patientIEId));
            gvPatientFUDetails.DataBind();

            if (gvPatientFUDetails.Rows.Count == 0)
                img.Attributes.Add("style", "display:none");
            else
                img.Attributes.Add("style", "display:block");
        }
    }

    protected void gvPatientDetails_PageIndexChanging1(object sender, GridViewPageEventArgs e)
    {
        gvPatientDetails.PageIndex = e.NewPageIndex;
        BindPatientIEDetails(hfPatientId.Value, txtSearch.Text.Trim());
    }

    protected void lbtnLogout_Click(object sender, EventArgs e)
    {
        Session.Abandon();
        Response.Redirect("~/Login.aspx");
    }

    protected void btnAddNew_Click(object sender, EventArgs e)
    {
        Session["PatientIE_ID"] = null;
        Response.Redirect("Page1.aspx");
    }

    protected void lnk_openIE_Click(object sender, EventArgs e)
    {
        LinkButton btn = sender as LinkButton;
        Response.Redirect("Page1.aspx?id=" + btn.CommandArgument);
    }

    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/PatientIntakeList.aspx");
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string UpdatePrintStatus(string flag, Int64 id)
    {
        string tempFileName = DateTime.Now.ToString("yyyyMMdd_") + flag + "_" + id;
        string tempFilePath = ConfigurationSettings.AppSettings["downloadpath"].ToString();
        string fileGetPath = ConfigurationSettings.AppSettings["fileGetPath"].ToString();
        string zipCreatePath = System.Web.Hosting.HostingEnvironment.MapPath(tempFilePath + "/" + tempFileName + ".zip");
        string[] filePaths = Directory.GetFiles(HttpContext.Current.Server.MapPath(fileGetPath), "*_" + id + "_*.*");

        if (File.Exists(zipCreatePath))
        {
            File.Delete(zipCreatePath);
            if (filePaths.Count() > 0)
            {
                foreach (var item in filePaths)
                {
                    File.Delete(item);
                }
            }
        }

        //if (filePaths.Length <= 0)
        //    return "";
        //using (ZipArchive archive = ZipFile.Open(zipCreatePath, ZipArchiveMode.Create))
        //{
        //    foreach (string filePath in filePaths)
        //    {
        //        string filename = filePath.Substring(filePath.LastIndexOf("\\") + 1);
        //        archive.CreateEntryFromFile(filePath, filename);
        //    }
        //}

        List<string> _patients = new List<string>();
        using (SqlConnection conn = new SqlConnection())
        {
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString;
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.CommandText = "nusp_UpdatePrintStatus";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@flag", flag);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Connection = conn;
                conn.Open();
                using (SqlDataReader sdr = cmd.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        _patients.Add(sdr["RESULT"].ToString());
                    }
                }
                conn.Close();
            }
            return "";
        }
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string CheckDownload(string flag, Int64 id)
    {
        string tempFileName = DateTime.Now.ToString("yyyyMMdd_") + flag + "_" + id;
        string tempFilePath = ConfigurationSettings.AppSettings["downloadpath"].ToString();
        string fileGetPath = ConfigurationSettings.AppSettings["fileGetPath"].ToString();
        string zipCreatePath = System.Web.Hosting.HostingEnvironment.MapPath(tempFilePath + "/" + tempFileName + ".zip");
        string[] filePaths = Directory.GetFiles(HttpContext.Current.Server.MapPath(fileGetPath), "*_" + id + "_*.*");
        if (File.Exists(zipCreatePath))
        {
            File.Delete(zipCreatePath);
        }
        if (filePaths.Length <= 0)
            return "";
        using (ZipArchive archive = ZipFile.Open(zipCreatePath, ZipArchiveMode.Create))
        {
            foreach (string filePath in filePaths)
            {
                string filename = filePath.Substring(filePath.LastIndexOf("\\") + 1);
                archive.CreateEntryFromFile(filePath, filename);
            }
        }
        using (SqlConnection conn = new SqlConnection())
        {
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString;
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.CommandText = "nusp_UpdatePrintStatus";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@flag", flag);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@Isdownload", "1");
                cmd.Connection = conn;
                conn.Open();
                using (SqlDataReader sdr = cmd.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        //_patients.Add(sdr["RESULT"].ToString());
                    }
                }
                conn.Close();
            }
        }
        return tempFileName;
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string UpdatePrintStatusRod(string flag, Int64 id)
    {
        string tempFileName = "_" + id + "_" + flag + "_Rod";
        string tempFilePath = ConfigurationSettings.AppSettings["downloadpath"].ToString();
        string fileGetPath = ConfigurationSettings.AppSettings["fileGetPath"].ToString();
        string zipCreatePath = System.Web.Hosting.HostingEnvironment.MapPath(tempFilePath + "/" + tempFileName + ".zip");
        string[] filePaths = Directory.GetFiles(HttpContext.Current.Server.MapPath(fileGetPath), "*_" + id + "_*.*");

        if (File.Exists(zipCreatePath))
        {
            File.Delete(zipCreatePath);
            if (filePaths.Count() > 0)
            {
                foreach (var item in filePaths)
                {
                    File.Delete(item);
                }
            }
        }

        List<string> _patients = new List<string>();
        using (SqlConnection conn = new SqlConnection())
        {
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString;
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.CommandText = "nusp_UpdatePrintStatusRoD";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@flag", flag);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Connection = conn;
                conn.Open();
                using (SqlDataReader sdr = cmd.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        _patients.Add(sdr["RESULT"].ToString());
                    }
                }
                conn.Close();
            }
            return "";
        }
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string CheckDownloadRod(string flag, Int64 id)
    {
        string tempFileName = "_" + id + "_" + flag + "_Rod";
        string tempFilePath = ConfigurationSettings.AppSettings["downloadpath"].ToString();
        string fileGetPath = ConfigurationSettings.AppSettings["fileGetPath"].ToString() + "/ROD";
        string zipCreatePath = System.Web.Hosting.HostingEnvironment.MapPath(tempFilePath + "/" + tempFileName + ".zip");
        string[] filePaths = Directory.GetFiles(HttpContext.Current.Server.MapPath(fileGetPath), "*_" + id + "_*.*");
        if (File.Exists(zipCreatePath))
        {
            File.Delete(zipCreatePath);
            filePaths = Directory.GetFiles(HttpContext.Current.Server.MapPath(fileGetPath), "*_" + id + "_*.*");
        }
        if (filePaths.Length <= 0)
            return "";
        using (ZipArchive archive = ZipFile.Open(zipCreatePath, ZipArchiveMode.Create))
        {
            foreach (string filePath in filePaths)
            {
                string filename = filePath.Substring(filePath.LastIndexOf("\\") + 1);
                archive.CreateEntryFromFile(filePath, filename);
            }
        }
        using (SqlConnection conn = new SqlConnection())
        {
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString;
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.CommandText = "nusp_UpdatePrintStatusRoD";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@flag", flag);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@Isdownload", "1");
                cmd.Connection = conn;
                conn.Open();
                using (SqlDataReader sdr = cmd.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        //_patients.Add(sdr["RESULT"].ToString());
                    }
                }
                conn.Close();
            }
        }
        return tempFileName;
    }

    protected void ddlPage_SelectedIndexChanged(object sender, EventArgs e)
    {
        gvPatientDetails.PageSize = Convert.ToInt16(ddlPage.SelectedItem.Value);
        BindPatientIEDetails();
    }

    private void BindRODDeafultValues(DataView dv, bool IsFromFU = false)
    {
        try
        {
            XmlTextReader xmlreader = new XmlTextReader(Server.MapPath("~/XML/Default_Rod.xml"));
            DataSet ds = new DataSet();
            ds.ReadXml(xmlreader);
            xmlreader.Close();
            if (dv != null)
            {

                string clause = string.Empty;
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(Server.MapPath("~/XML/Clause.xml"));
                XmlNodeList nodeList = xmlDoc.DocumentElement.SelectNodes("/Clauses");
                foreach (XmlNode node in nodeList)
                {
                    if (!IsFromFU)
                    {
                        clause = node.SelectSingleNode(Convert.ToString(dv[0].Row.ItemArray[7])) == null ? string.Empty : node.SelectSingleNode(Convert.ToString(dv[0].Row.ItemArray[7])).InnerText;
                    }
                    else
                    {
                        clause = node.SelectSingleNode(Convert.ToString(dv[0].Row.ItemArray[13])) == null ? string.Empty : node.SelectSingleNode(Convert.ToString(dv[0].Row.ItemArray[13])).InnerText;
                    }

                }

                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        if (row["Name"].ToString().Contains("##Name##"))
                        {
                            string temp = row["Name"].ToString();
                            if (!IsFromFU)
                            {
                                temp = temp.Replace("##Name##", Convert.ToString(dv[0].Row.ItemArray[1]) + " " + Convert.ToString(dv[0].Row.ItemArray[3]) + " " + Convert.ToString(dv[0].Row.ItemArray[2])).Replace("##IEdate##", Convert.ToString(Convert.ToDateTime(dv[0].Row.ItemArray[9]).ToString("MM/dd/yyyy"))).Replace("##DOA##", Convert.ToString(Convert.ToDateTime(dv[0].Row.ItemArray[5]).ToString("MM/dd/yyyy"))).Replace("##cause##", clause).Replace("##FUVisitdate##", " ___(last DOS)___ ");
                            }
                            else
                            {
                                temp = temp.Replace("##Name##", Convert.ToString(dv[0].Row.ItemArray[6]) + " " + Convert.ToString(dv[0].Row.ItemArray[4]) + " " + Convert.ToString(dv[0].Row.ItemArray[5])).Replace("##FUVisitdate##", Convert.ToString(Convert.ToDateTime(dv[0].Row.ItemArray[7]).ToString("MM/dd/yyyy"))).Replace("##DOA##", Convert.ToString(Convert.ToDateTime(dv[0].Row.ItemArray[11]).ToString("MM/dd/yyyy"))).Replace("##cause##", clause).Replace("##IEdate##", Convert.ToString(Convert.ToDateTime(dv[0].Row.ItemArray[12]).ToString("MM/dd/yyyy")));
                            }
                            row.SetField("Name", temp);
                        }
                        else if (row["Name"].ToString().Contains("##DOA##"))
                        {
                            string temp = row["Name"].ToString();
                            if (!IsFromFU)
                            {
                                temp = temp.Replace("##DOA##", Convert.ToString(Convert.ToDateTime(dv[0].Row.ItemArray[5]).ToString("MM/dd/yyyy")));
                            }
                            else
                            {
                                temp = temp.Replace("##DOA##", Convert.ToString(Convert.ToDateTime(dv[0].Row.ItemArray[11]).ToString("MM/dd/yyyy")));
                            }
                            row.SetField("Name", temp);
                        }
                    }

                    repRoD.DataSource = ds.Tables[0];
                    repRoD.DataBind();
                }
            }
        }
        catch (Exception)
        {

            throw;
        }


    }

    protected void btnrodsave_Click(object sender, EventArgs e)
    {
        //string id = hdnrodieid.Value;
        SqlConnection oSQLConn = new SqlConnection();
        SqlCommand oSQLCmd = new SqlCommand();
        string _ieID = Convert.ToString(hdnrodieid.Value);
        string _fuieid = Convert.ToString(hdnrodeditedfuieid.Value);
        string _fufuid = Convert.ToString(hdnrodeditedfuid.Value);

        string _ieMode = "";
        string sProvider = ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString;
        string SqlStr = "";
        oSQLConn.ConnectionString = sProvider;
        oSQLConn.Open();
        if (string.IsNullOrEmpty(_fuieid) && string.IsNullOrEmpty(_fufuid))
        {
            SqlStr = "Select * from tblrod WHERE patietn_IE = " + _ieID;
        }
        else
        {
            SqlStr = "Select * from tblrod WHERE patietn_IE = " + Convert.ToInt64(_fuieid) + " and Patiend_FUID = " + Convert.ToInt64(_fufuid);
        }

        SqlDataAdapter sqlAdapt = new SqlDataAdapter(SqlStr, oSQLConn);
        SqlCommandBuilder sqlCmdBuilder = new SqlCommandBuilder(sqlAdapt);
        DataTable sqlTbl = new DataTable();
        sqlAdapt.Fill(sqlTbl);
        DataRow TblRow;

        if (sqlTbl.Rows.Count == 0)
            _ieMode = "New";
        else if (sqlTbl.Rows.Count == 0)
            _ieMode = "None";
        else if (sqlTbl.Rows.Count > 0)
            _ieMode = "Update";
        else
            _ieMode = "Delete";

        if (_ieMode == "New")
            TblRow = sqlTbl.NewRow();
        else if (_ieMode == "Update" || _ieMode == "Delete")
        {
            TblRow = sqlTbl.Rows[0];
            TblRow.AcceptChanges();
        }
        else
            TblRow = null;

        if (_ieMode == "Update" || _ieMode == "New")
        {
            TblRow["patietn_IE"] = !string.IsNullOrEmpty(_ieID) ? _ieID : _fuieid;

            if (!string.IsNullOrEmpty(_fufuid))
            {
                TblRow["Patiend_FUID"] = _fufuid;
            }

            TblRow["Content"] = txtrodFulldetails.Text;
            TblRow["Contentdelimit"] = bindRodPrintvalue();
            TblRow["Bodypartdetails"] = hdbodyparts.Value;
            TblRow["Newlinedetails"] = hdnewline.Value;
            TblRow["Plandetails"] = "test";
            TblRow["Plandelimit"] = "test";
            TblRow["Clientnote"] = "test";
            TblRow["Signpath"] = "test";

            if (_ieMode == "New")
            {
                sqlTbl.Rows.Add(TblRow);
            }
            sqlAdapt.Update(sqlTbl);
        }
        else if (_ieMode == "Delete")
        {
            TblRow.Delete();
            sqlAdapt.Update(sqlTbl);
        }
        if (TblRow != null)
            TblRow.Table.Dispose();
        sqlTbl.Dispose();
        sqlCmdBuilder.Dispose();
        sqlAdapt.Dispose();
        oSQLConn.Close();





        if (string.IsNullOrEmpty(_fuieid) && string.IsNullOrEmpty(_fufuid))
        {
            LinkButton btn = new LinkButton();
            btn.Text = "RoD";
            btn.CommandArgument = _ieID;
            lnkierod_Click(btn, e);

        }
        else
        {

            LinkButton btn = new LinkButton();
            btn.Text = "RoD";
            btn.CommandArgument = _fufuid + "-" + _fuieid;
            lnkfurod_Click(btn, e);
        }

    }

    protected void chk_CheckedChanged(object sender, EventArgs e)
    {
        bindRodPrintvalue();
    }

    protected void txtRod_TextChanged(object sender, EventArgs e)
    {
        bindRodPrintvalue();
    }

    private string bindRodPrintvalue()
    {
        string str = "";
        string strDelimit = "";
        string bodypartselected = string.Empty;
        string bodypartUnselected = string.Empty;
        string planselected = string.Empty;
        string planunselected = string.Empty;
        string bodypart = string.Empty;
        string strbp = string.Empty, strnewline = string.Empty;
        for (int i = 0; i < repRoD.Items.Count; i++)
        {

            TextBox txt = i == 0 || i == 13 || i == 15 ? repRoD.Items[i].FindControl("txtRod") as TextBox : repRoD.Items[i].FindControl("txtRod1") as TextBox;
            CheckBox chk = repRoD.Items[i].FindControl("chk") as CheckBox;
            HiddenField hdbodypart = repRoD.Items[i].FindControl("bodypart") as HiddenField;
            HiddenField hdisnewline = repRoD.Items[i].FindControl("isnewline") as HiddenField;
            if (chk.Checked)
            {
                if (hdisnewline.Value == "1")
                    str = str + @"\n" + txt.Text;
                else if (hdisnewline.Value == "2")
                    str = str + @"\n\n" + txt.Text;
                else
                    str = str + txt.Text;

                strDelimit = strDelimit + "^" + txt.Text;
                bodypart += hdbodypart.Value + ",";
                if (hdbodypart.Value.Split('-').Count() > 1)
                {
                    if (hdbodypart.Value.Split('-')[1].Equals("b"))
                    {
                        bodypartselected += hdbodypart.Value + ",";
                    }
                    else if (hdbodypart.Value.Split('-')[1].Equals("p"))
                    {
                        planselected += hdbodypart.Value + ",";
                    }
                }


            }
            else
            {

                // str = !string.IsNullOrEmpty(txt.Text) ? str.Replace(txt.Text, "") : str;
                strDelimit = strDelimit + "^@" + txt.Text;

                if (hdbodypart.Value.Split('-').Count() > 1)
                {
                    if (hdbodypart.Value.Split('-')[1].Equals("b"))
                    {
                        bodypartUnselected += hdbodypart.Value + ",";
                    }
                    else if (hdbodypart.Value.Split('-')[1].Equals("p"))
                    {
                        planunselected += hdbodypart.Value + ",";
                    }
                }
            }
            if (string.IsNullOrEmpty(strbp))
                strbp = hdbodypart.Value + ",";
            else
                strbp += hdbodypart.Value + ",";

            if (string.IsNullOrEmpty(strnewline))
                strnewline = hdisnewline.Value + ",";
            else
                strnewline += hdisnewline.Value + ",";

        }

        foreach (var item in bodypartselected.TrimEnd(',').Split(','))
        {
            for (int i = 0; i < repRoD.Items.Count; i++)
            {
                TextBox txt1 = i == 0 || i == 13 || i == 15 ? repRoD.Items[i].FindControl("txtRod") as TextBox : repRoD.Items[i].FindControl("txtRod1") as TextBox;
                CheckBox chk1 = repRoD.Items[i].FindControl("chk") as CheckBox;
                HiddenField hdbodypart1 = repRoD.Items[i].FindControl("bodypart") as HiddenField;
                if (hdbodypart1.Value.Split('-').Count() > 1)
                {
                    if (item.Split('-')[0].Equals(hdbodypart1.Value.Split('-')[0]) && hdbodypart1.Value.Split('-')[1].Equals("p"))
                    {
                        chk1.Checked = true;
                    }
                    else if (hdbodypart1.Value.Split('-')[1].Equals("p") && chk1.Checked && !bodypartselected.Contains(hdbodypart1.Value.Split('-')[0]))
                    {
                        chk1.Checked = false;
                    }
                }
            }
        }

        foreach (var v in bodypart.Split(','))
        {
            for (int i = 0; i < repRoD.Items.Count; i++)
            {
                TextBox txt1 = i == 0 || i == 13 || i == 15 ? repRoD.Items[i].FindControl("txtRod") as TextBox : repRoD.Items[i].FindControl("txtRod1") as TextBox;
                CheckBox chk1 = repRoD.Items[i].FindControl("chk") as CheckBox;
                HiddenField hdbodypart1 = repRoD.Items[i].FindControl("bodypart") as HiddenField;

                if (v.Split('-')[0].Equals(Convert.ToString(hdbodypart1.Value).Split('-')[0]) && !chk1.Checked && v.Split('-').Count() > 1)
                {
                    if (v.Split('-')[1] != hdbodypart1.Value.Split('-')[1] && hdbodypart1.Value.Split('-')[1].Equals("p"))
                    {
                        chk1.Checked = true;
                    }
                    else
                    {
                        if (hdbodypart1.Value.Split('-')[1] == "b" && !chk1.Checked)
                        {
                            if (v.Split('-')[1] == "p")
                            {
                                chk1.Checked = false;
                            }
                        }
                    }

                }
                if (v.Equals(hdbodypart1.Value) && chk1.Checked)
                {
                    chk1.Checked = chk1.Checked;
                }
            }
        }





        txtrodFulldetails.Text = str;

        strDelimit = strDelimit.TrimStart('^');
        hdbodyparts.Value = strbp;
        hdnewline.Value = strnewline;

        return strDelimit;
    }

    protected void lnkierod_Click(object sender, EventArgs e)
    {
        try
        {
            LinkButton btn = (LinkButton)(sender);
            DataTable dt = (DataTable)(Session["iedata"]);
            hdnrodieid.Value = btn.CommandArgument;
            DataView dv = new DataView(dt);
            dv.RowFilter = "PatientIE_ID=" + Convert.ToInt32(btn.CommandArgument); // query example = "id = 10"

            SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString);
            DBHelperClass db = new DBHelperClass();
            string query = ("select * from tblROD where patietn_IE= " + btn.CommandArgument + " and Patiend_FUID is null");
            SqlCommand cm = new SqlCommand(query, cn);
            SqlDataAdapter da = new SqlDataAdapter(cm);
            cn.Open();
            DataSet ds = new DataSet();
            da.Fill(ds);
            string printStatus = "Print";
            string downloadStatus = "";



            if (ds.Tables[0].Rows.Count == 0)
            {
                BindRODDeafultValues(dv);
                btnRODDelete.Visible = false;

            }
            else
            {
                BindRODEditValues(ds.Tables[0].Rows[0]["Contentdelimit"].ToString(), ds.Tables[0].Rows[0]["Bodypartdetails"].ToString(), ds.Tables[0].Rows[0]["Newlinedetails"].ToString());
                printStatus = string.IsNullOrEmpty(ds.Tables[0].Rows[0]["PrintStatus"].ToString()) ? "Print" : ds.Tables[0].Rows[0]["PrintStatus"].ToString();

                if (ds.Tables[0].Rows[0]["PrintStatus"].ToString().Equals("Download"))
                {
                    printStatus = "Print";
                    downloadStatus = "Download";
                }
                else if (ds.Tables[0].Rows[0]["PrintStatus"].ToString().Equals("Downloaded"))
                {
                    printStatus = "Print";
                    downloadStatus = "Downloaded";
                }
                btnRODDelete.Visible = true;
                ViewState["rodid"] = ds.Tables[0].Rows[0]["id"].ToString();
            }


            ltrprint.Text = "<a class='btn btn-link PrintClickRod' data-FUIE='IE' data-id='" + Convert.ToString(btn.CommandArgument) + "'>" + printStatus + "</a> ";
            if (!string.IsNullOrEmpty(downloadStatus))
                ltrdownload.Text = "<a class='btn btn-link PrintClickRod' data-FUIE='IE' data-id='" + Convert.ToString(btn.CommandArgument) + "'>" + downloadStatus + "</a>";

            ClientScript.RegisterStartupScript(this.GetType(), "Popup", "openModelPopup();", true);
        }
        catch (Exception)
        {
            throw;
        }

    }

    protected void lnkfurod_Click(object sender, EventArgs e)
    {
        try
        {

            BusinessLogic bl = new BusinessLogic();
            LinkButton btn = (LinkButton)(sender);

            hdnrodeditedfuid.Value = btn.CommandArgument.Split('-')[0];
            hdnrodeditedfuieid.Value = btn.CommandArgument.Split('-')[1];

            DataTable dt = ToDataTable(bl.GetFUDetails(Convert.ToInt32(hdnrodeditedfuieid.Value)));
            DataView dv = new DataView(dt);
            dv.RowFilter = "PatientFUId=" + Convert.ToInt32(hdnrodeditedfuid.Value); // query example = "id = 10"
            SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString);
            DBHelperClass db = new DBHelperClass();
            string query = ("select * from tblROD where Patiend_FUID= " + hdnrodeditedfuid.Value + "");
            SqlCommand cm = new SqlCommand(query, cn);
            SqlDataAdapter da = new SqlDataAdapter(cm);
            cn.Open();
            DataSet ds = new DataSet();
            da.Fill(ds);
            string printStatus = "Print";
            string downloadStatus = "";

            if (ds.Tables[0].Rows.Count == 0)
            {
                BindRODDeafultValues(dv, true);
                btnRODDelete.Visible = false;
            }
            else
            {
                BindRODEditValues(ds.Tables[0].Rows[0]["Contentdelimit"].ToString(), ds.Tables[0].Rows[0]["Bodypartdetails"].ToString(), ds.Tables[0].Rows[0]["Newlinedetails"].ToString());
                printStatus = string.IsNullOrEmpty(ds.Tables[0].Rows[0]["PrintStatus"].ToString()) ? "Print" : ds.Tables[0].Rows[0]["PrintStatus"].ToString();

                if (ds.Tables[0].Rows[0]["PrintStatus"].ToString().Equals("Download"))
                {
                    printStatus = "Print";
                    downloadStatus = "Download";
                }
                else if (ds.Tables[0].Rows[0]["PrintStatus"].ToString().Equals("Downloaded"))
                {
                    printStatus = "Print";
                    downloadStatus = "Downloaded";
                }
                btnRODDelete.Visible = true;
                ViewState["rodid"] = ds.Tables[0].Rows[0]["id"].ToString();
            }


            ltrprint.Text = "<a class='btn btn-link PrintClickRod' data-FUIE='FU' data-id='" + hdnrodeditedfuid.Value + "'>" + printStatus + "</a> ";
            if (!string.IsNullOrEmpty(downloadStatus))
                ltrdownload.Text = "<a class='btn btn-link PrintClickRod' data-FUIE='FU' data-id='" + hdnrodeditedfuid.Value + "'>" + downloadStatus + "</a>";

            ClientScript.RegisterStartupScript(this.GetType(), "Popup", "openModelPopup();", true);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public static DataTable ToDataTable<T>(List<T> items)
    {
        DataTable dataTable = new DataTable(typeof(T).Name);

        //Get all the properties
        PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
        foreach (PropertyInfo prop in Props)
        {
            //Defining type of data column gives proper data table 
            var type = (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType);
            //Setting column names as Property names
            dataTable.Columns.Add(prop.Name, type);
        }
        foreach (T item in items)
        {
            var values = new object[Props.Length];
            for (int i = 0; i < Props.Length; i++)
            {
                //inserting property values to datatable rows
                values[i] = Props[i].GetValue(item, null);
            }
            dataTable.Rows.Add(values);
        }
        //put a breakpoint here and check datatable
        return dataTable;
    }

    private void BindRODEditValues(string val, string bpstr, string newlinestr)
    {
        try
        {
            if (!string.IsNullOrEmpty(val))
            {
                string[] str = val.Split('^');
                string[] strbp = bpstr.Split(',');
                string[] strnl = newlinestr.Split(',');

                DataTable dt = new DataTable();

                dt.Columns.AddRange(new DataColumn[4] { new DataColumn("isChecked", typeof(string)),
                            new DataColumn("name", typeof(string)),
            new DataColumn("bodypart", typeof(string)),
              new DataColumn("isnewline", typeof(string))});

                for (int i = 0; i < str.Length; i++)
                {
                    dt.Rows.Add(string.IsNullOrEmpty(str[i]) ? "False" : str[i].Substring(0, 1) == "@" ? "False" : "True", str[i].TrimStart('@'), strbp[i], strnl[i]);
                    // dt.Rows.Add(str[i].Substring(0, 1) == "@" ? "False" : "True", string.IsNullOrEmpty(str[i]) ? str[i] : str[i].TrimStart('@'));
                }

                repRoD.DataSource = dt;
                repRoD.DataBind();

                //  bindTeratMentPrintvalue();

            }

        }
        catch (Exception ex)
        {

        }
    }

    protected void btnRODDelete_Click(object sender, EventArgs e)
    {
        DBHelperClass dB = new DBHelperClass();
        int val = dB.executeQuery("delete from tblROD where id=" + ViewState["rodid"].ToString());
        if (val > 0)
        {
            string _ieID = Convert.ToString(hdnrodieid.Value);
            string _fuieid = Convert.ToString(hdnrodeditedfuieid.Value);
            string _fufuid = Convert.ToString(hdnrodeditedfuid.Value);

            //if (string.IsNullOrEmpty(_fuieid) && string.IsNullOrEmpty(_fufuid))
            //{
            //    LinkButton btn = new LinkButton();
            //    btn.Text = "RoD";
            //    btn.CommandArgument = _ieID;
            //    lnkierod_Click(btn, e);

            //}
            //else
            //{

            //    LinkButton btn = new LinkButton();
            //    btn.Text = "RoD";
            //    btn.CommandArgument = _fufuid + "-" + _fuieid;
            //    lnkfurod_Click(btn, e);
            //}
        }
    }

    protected void lnkprint_Click(object sender, EventArgs e)
    {
        //try
        //{
        PrintDocumentHelper helper = new PrintDocumentHelper();

        String str = File.ReadAllText(Server.MapPath("~/Template/DocumentPrintIE.html"));

        string prstrCC = "", prstrPE = "", docname = "";

        LinkButton lnk = sender as LinkButton;
        SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString);
        DBHelperClass db = new DBHelperClass();




        //page1 printing
        string query = ("select * from View_PatientIE where PatientIE_ID= " + lnk.CommandArgument + "");
        DataSet ds = db.selectData(query);


        docname = CommonConvert.UppercaseFirst(ds.Tables[0].Rows[0]["LastName"].ToString()) + ", " + CommonConvert.UppercaseFirst(ds.Tables[0].Rows[0]["FirstName"].ToString()) + "_" + lnk.CommandArgument + "_IE_" + CommonConvert.DateFormatPrint(ds.Tables[0].Rows[0]["DOE"].ToString()) + "_" + CommonConvert.DateFormatPrint(ds.Tables[0].Rows[0]["DOA"].ToString());

        string gender = ds.Tables[0].Rows[0]["Sex"].ToString() == "Mr." ? "He" : "She";
        string sex = ds.Tables[0].Rows[0]["Sex"].ToString() == "Mr." ? "male" : "female";
        string name = CommonConvert.UppercaseFirst(ds.Tables[0].Rows[0]["FirstName"].ToString()) + " " + ds.Tables[0].Rows[0]["MiddleName"].ToString() + " " + CommonConvert.UppercaseFirst(ds.Tables[0].Rows[0]["LastName"].ToString());
        string doa = CommonConvert.DateFormat(ds.Tables[0].Rows[0]["DOA"].ToString());
        string doe = CommonConvert.DateFormat(ds.Tables[0].Rows[0]["DOE"].ToString());
        str = str.Replace("#patientname", name);
        str = str.Replace("#dob", CommonConvert.DateFormat(ds.Tables[0].Rows[0]["DOB"].ToString()));
        str = str.Replace("#doi", doa);
        str = str.Replace("#dos", doe);
        string age = ds.Tables[0].Rows[0]["AGE"].ToString();

        string printpage1str = printPage1(lnk.CommandArgument, age, doa, ds.Tables[0].Rows[0]["Location_Id"].ToString());


        printpage1str = printpage1str.Replace("#gender", gender);
        printpage1str = printpage1str.Replace("#sex", sex);
        printpage1str = printpage1str.Replace("#doe", doe);
        printpage1str = printpage1str.Replace("#name", name);

        str = str.Replace("#history", printpage1str);

        string cclist = getBodyPartswithnumber(lnk.CommandArgument, ds.Tables[0].Rows[0]["Compensation"].ToString(), doa);

        str = str.Replace("#cclist", cclist);


        //header printing

        query = ("select * from tblLocations where Location_ID=" + ds.Tables[0].Rows[0]["Location_Id"]);
        ds = db.selectData(query);

        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            str = str.Replace("#drName", ds.Tables[0].Rows[0]["DrFName"].ToString() + " " + ds.Tables[0].Rows[0]["DrLName"].ToString());
            str = str.Replace("#drlname", ds.Tables[0].Rows[0]["DrLName"].ToString());
            str = str.Replace("#address", ds.Tables[0].Rows[0]["Address"].ToString() + "<br/>" + ds.Tables[0].Rows[0]["City"].ToString() + ", " + ds.Tables[0].Rows[0]["State"].ToString() + " " + ds.Tables[0].Rows[0]["Zip"].ToString());
        }


        String strheader = File.ReadAllText(Server.MapPath("~/Template/Header/Default.html"));



        //page1 priting
        query = ("select topSectionHTML from tblPage1HTMLContent where PatientIE_ID= " + lnk.CommandArgument + "");
        ds = db.selectData(query);

        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            Dictionary<string, string> page1 = new PrintDocumentHelper().getPage1String(ds.Tables[0].Rows[0]["topSectionHTML"].ToString());

            str = str.Replace("#pastmedicalhistory", string.IsNullOrEmpty(page1["txt_PMH"]) ? "" : "<b>PAST MEDICAL HISTORY: </b>" + page1["txt_PMH"].TrimEnd('.') + ".<br />");
            str = str.Replace("#pastsurgicalhistory", string.IsNullOrEmpty(page1["PSH"]) ? "" : "<b>PAST SURGICAL HISTORY: </b>" + page1["PSH"].TrimEnd('.') + ".<br/>");
            str = str.Replace("#pastmedications", string.IsNullOrEmpty(page1["Medication"]) ? "" : "<b>MEDICATIONS: </b>" + page1["Medication"].TrimEnd('.') + ".<br/>");
            str = str.Replace("#allergies", string.IsNullOrEmpty(page1["Allergies"]) ? "" : "<b>ALLERGIES: </b>" + page1["Allergies"].TrimEnd('.').ToUpper() + ".<br/>");
            //str = str.Replace("#familyhistory", string.IsNullOrEmpty(page1["FamilyHistory"]) ? "" : "<b>Family History: </b>" + page1["FamilyHistory"].TrimEnd('.') + ".<br/><br/>");
            str = str.Replace("#familyhistory", "");

        }

        query = ("select socialSectionHTML from tblPage1HTMLContent where PatientIE_ID= " + lnk.CommandArgument + "");
        ds = db.selectData(query);

        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            string strstatus = new PrintDocumentHelper().getDocumentString(ds.Tables[0].Rows[0]["socialSectionHTML"].ToString());
            str = str.Replace("#socialhistory", "<b>SOCIAL HISTORY: </b>" + strstatus.TrimEnd('.').Replace(" .", "") + ".<br/>");
        }
        else
        {
            str = str.Replace("#socialhistory", "");
        }

        query = ("select accidentHTML from tblPage1HTMLContent where PatientIE_ID= " + lnk.CommandArgument + "");
        ds = db.selectData(query);


        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            Dictionary<string, string> page1_accident = new PrintDocumentHelper().getPage1String(ds.Tables[0].Rows[0]["accidentHTML"].ToString());

            string work_status = "", accidentdetails = "", strdod = "";

            if (page1_accident.ContainsKey("txt_details"))
            {
                if (!string.IsNullOrEmpty(page1_accident["txt_details"]))
                    accidentdetails = accidentdetails + page1_accident["txt_details"].TrimEnd('.') + ". ";
            }


            if (page1_accident["rblPatial"] == "true")
                strdod = "Partial";
            else if (page1_accident["rbl25"] == "true")
                strdod = "25%";
            else if (page1_accident["rbl50"] == "true")
                strdod = "50%";
            else if (page1_accident["rbl75"] == "true")
                strdod = "75%";
            else if (page1_accident["rbl100"] == "true")
                strdod = "100%";
            else if (page1_accident["rblNone"] == "true")
                strdod = "None";

            if (!string.IsNullOrEmpty(strdod))
                str = str.Replace("#dod", "<b>DEGREE OF DISABILITY: </b>" + strdod);
            else
                str = str.Replace("#dod", "");

            //if (!string.IsNullOrEmpty(page1_accident["txt_accident_desc"]))
            //    accidentdetails = accidentdetails + gender + " " + page1_accident["txt_accident_desc"].TrimEnd('.') + ". ";

            //str = str.Replace("#accidentdetails", accidentdetails);

            if (page1_accident.ContainsKey("txt_vital"))
            {
                if (!string.IsNullOrEmpty(page1_accident["txt_vital"]))
                    str = str.Replace("#vital", "<b>VITAL: </b>" + page1_accident["txt_vital"].TrimEnd('.').Replace(" .", "") + "<br/>");
            }

            //if (page1_accident.ContainsKey("txt_gait_desc"))
            //{
            //    if (!string.IsNullOrEmpty(page1_accident["txt_gait_desc"]))
            //        str = str.Replace("#gait", "<br/><br/><b>GAIT</b>: The patient " + page1_accident["txt_gait_desc"].Trim() + ".");
            //}
            //else
            //    str = str.Replace("#gait", "");

            if (!string.IsNullOrEmpty(page1_accident["txt_work_status"]))
                work_status = work_status + page1_accident["txt_work_status"].TrimEnd('.') + ". ";

            if (!string.IsNullOrEmpty(page1_accident["txtMissed"]))
                work_status = work_status + page1_accident["txtMissed"] + " ";

            if (page1_accident["rblStatus"] == "true")
                work_status = work_status + "Able to go back to work.";
            else if (page1_accident["rblrblStatus1"] == "true")
                work_status = work_status + "Working.";
            else if (page1_accident["rblStatus2"] == "true")
                work_status = work_status + "Not Working.";
            else if (page1_accident["rblStatus3"] == "true")
                work_status = work_status + "Partially Working.";


            str = str.Replace("#work_status", string.IsNullOrEmpty(work_status) ? "" : "<br/><b>WORK STATUS: </b>" + work_status);

            string pastinjury = "";
            if (page1_accident["rdbinjuyes"] == "true")
            {
                pastinjury = gender + " had an  injury to " + page1_accident["txt_injur_past_bp"] + " because of a " + page1_accident["txt_injur_past_how"].TrimEnd('.') + ". ";
            }

            str = str.Replace("#pastinjury", string.IsNullOrEmpty(pastinjury) ? "" : "<b><u>PAST INJURY</u>: </b>" + pastinjury + "<br/><br/>");
            //if (page1_accident["rdbdocyes"] == "true")
            //{
            //    work_status = work_status + gender + " was seen by " + page1_accident["txt_docname"] + " for that injury. ";
            //}


            if (!string.IsNullOrEmpty(page1_accident["txt_accident_desc_3"].Trim()))
                str = str.Replace("#cc1", page1_accident["txt_accident_desc_3"].Trim() + "<br/><br/>");
            else
                str = str.Replace("#cc1", "");


            //if (!string.IsNullOrEmpty(page1_accident["txt_accident_desc_4"].Trim()))
            //    str = str.Replace("#cc2", page1_accident["txt_accident_desc_4"].Trim() + "<br/><br/>");
            //else
            //    str = str.Replace("#cc2", "");

            str = str.Replace("#cc2", "");


        }

        //treatment priting
        query = ("Select TreatMentDetails from tblbpOtherPart WHERE PatientIE_ID=" + lnk.CommandArgument + "");
        ds = db.selectData(query);

        string treatment = "";
        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            treatment = ds.Tables[0].Rows[0]["TreatMentDetails"].ToString();
        }

        if (!string.IsNullOrEmpty(treatment))
            str = str.Replace("#treatment", treatment + "<br/>");
        else
            str = str.Replace("#treatment", "");


        //page2 printing
        query = ("select * from tblPage2HTMLContent where PatientIE_ID= " + lnk.CommandArgument + "");
        ds = db.selectData(query);

        string strRos = "", strRosDenis = "", strComplain = "", strDOD = "", strRestriction = "", strWorkStatus = "", strMedi = "", strAffect = "";

        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            strRos = helper.getDocumentString(ds.Tables[0].Rows[0]["rosSectionHTML"].ToString());


            if (!string.IsNullOrEmpty(strRos))
                strRos = "The patient admits to " + strRos.TrimEnd() + ". ";

            strRosDenis = helper.getDocumentStringDenies(ds.Tables[0].Rows[0]["rosSectionHTML"].ToString());
            if (!string.IsNullOrEmpty(strRosDenis))
                strRosDenis = "The patient denies " + strRosDenis.TrimEnd() + ".";
        }
        str = str.Replace("#ros", strRos + strRosDenis);

        //strComplain = "";

        //if (ds != null && ds.Tables[0].Rows.Count > 0)
        //{
        //    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["topSectionHTML"].ToString()))
        //    {
        //        string cmp = helper.getDocumentString(ds.Tables[0].Rows[0]["topSectionHTML"].ToString());
        //        if (!string.IsNullOrEmpty(cmp))
        //            strComplain = "The patient also complains of  " + helper.getDocumentString(ds.Tables[0].Rows[0]["topSectionHTML"].ToString()) + ".";
        //    }
        //}

        //str = str.Replace("#complain", !string.IsNullOrEmpty(strComplain) ? strComplain + "<br/><br/>" : "");


        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            Dictionary<string, string> page2 = new PrintDocumentHelper().getPage1String(ds.Tables[0].Rows[0]["degreeSectionHTML"].ToString());


            //if (!string.IsNullOrEmpty(page2["txtGeneral"]))
            //    str = str.Replace("#general", string.IsNullOrEmpty(page2["txtGeneral"]) ? "" : "<b>GENERAL: </b>" + page2["txtGeneral"].TrimEnd('.') + ".<br/>");
            //if (!string.IsNullOrEmpty(page2["txtHEENT"]))
            //    str = str.Replace("#heent", string.IsNullOrEmpty(page2["txtHEENT"]) ? "" : "HEENT: " + page2["txtHEENT"].TrimEnd('.') + ".<br/>");
            //if (!string.IsNullOrEmpty(page2["txtOcc"]))
            //    str = str.Replace("#occ_head", string.IsNullOrEmpty(page2["txtOcc"]) ? "" : "Occpital headaches: " + page2["txtOcc"].TrimEnd('.') + ".<br/>");
            //if (!string.IsNullOrEmpty(page2["txtCCA"]))
            //    str = str.Replace("#cca", string.IsNullOrEmpty(page2["txtCCA"]) ? "" : "Chest, Cardiovascular, Abdomen: " + page2["txtCCA"].TrimEnd('.') + ".<br/>");
            //if (!string.IsNullOrEmpty(page2["txtPhy"]))
            //    str = str.Replace("#phy_ne", string.IsNullOrEmpty(page2["txtPhy"]) ? "" : "Psych/Neuro: " + page2["txtPhy"].Trim().TrimEnd('.') + ".<br/>");
            if (!string.IsNullOrEmpty(page2["txtgait"]))
                str = str.Replace("#gait", string.IsNullOrEmpty(page2["txtgait"]) ? "" : "<b>GAIT: </b>" + page2["txtgait"].TrimEnd('.') + ".<br/>");


            //if (page2["rblPatial"] == "true")
            //    strDOD = "Partial";
            //else if (page2["rbl25"] == "true")
            //    strDOD = "25%";
            //else if (page2["rbl50"] == "true")
            //    strDOD = "50%";
            //else if (page2["rbl75"] == "true")
            //    strDOD = "75%";
            //else if (page2["rbl100"] == "true")
            //    strDOD = "100%";
            //else if (page2["rblNone"] == "true")
            //    strDOD = "None";

            //if (!string.IsNullOrEmpty(strDOD))
            //    str = str.Replace("#dod", "<b><u>DEGREE OF DISABILITY</u>: </b>" + strDOD + "<br/>");
            //else
            //    str = str.Replace("#dod", "");

            //if (page2["chkhousework"] == "true")
            //    strAffect = "housework, ";
            //if (page2["chkwork-related"] == "true")
            //    strAffect = strAffect + "job work-related duties, ";
            //if (page2["chkdriving"] == "true")
            //    strAffect = strAffect + "driving, ";
            //if (page2["chksittingincar"] == "true")
            //    strAffect = strAffect + "sitting in car, ";
            //if (page2["chkwalking"] == "true")
            //    strAffect = strAffect + "walking up/down downstairs, ";

            if (!string.IsNullOrEmpty(strAffect))
                str = str.Replace("#activityaffected", "<b><u>Activities of Daily living affected</u>: </b>" + strAffect.TrimEnd(',') + "<br/>");
            else
                str = str.Replace("#activityaffected", "");





            //if (page2["chkBending"] == "true")
            //    strRestriction = "Bending, ";
            //if (page2["chkClimbing"] == "true")
            //    strRestriction = strRestriction + "Climbing stairs/ladders, ";
            //if (page2["chkEnvironmental"] == "true")
            //    strRestriction = strRestriction + "Environmental conditions, ";
            //if (page2["chkKneeling"] == "true")
            //    strRestriction = strRestriction + "Kneeling, ";
            //if (page2["chkLifting"] == "true")
            //    strRestriction = strRestriction + "Lifting, ";
            //if (page2["chkOperatingHeavy"] == "true")
            //    strRestriction = strRestriction + "Operating heavy equipment, ";
            //if (page2["chkOperatingofmotor"] == "true")
            //    strRestriction = strRestriction + "Operation of motor vehicles, ";
            //if (page2["chkPersonal"] == "true")
            //    strRestriction = strRestriction + "Personal protective equipment, ";
            //if (page2["chkSitting"] == "true")
            //    strRestriction = strRestriction + "Sitting, ";
            //if (page2["chkStanding"] == "true")
            //    strRestriction = strRestriction + "Standing, ";
            //if (page2["chkUseofPublic"] == "true")
            //    strRestriction = strRestriction + "Use of public transportation, ";
            //if (page2["chkUseofUpper"] == "true")
            //    strRestriction = strRestriction + "Use of upper extremities, ";

            if (!string.IsNullOrEmpty(strRestriction))
                str = str.Replace("#restriction", "<b><u>RESTRICTION</u>: </b>" + strRestriction.TrimEnd(',') + "<br/>");
            else
                str = str.Replace("#restriction", "");

            //if (page2["chkAbletoWork"] == "true")
            //    strWorkStatus = "Able to go back to work " + page2["txtbackwork"] + ", ";
            //if (page2["chkWorking"] == "true")
            //    strWorkStatus = strWorkStatus + "Working " + page2["txtWorking"] + ", ";
            //if (page2["chkNotWorking"] == "true")
            //    strWorkStatus = strWorkStatus + "Not Working " + page2["txtNotWorking"] + ", ";
            //if (page2["chkPartiallyWorking"] == "true")
            //    strWorkStatus = strWorkStatus + "Partially Working " + page2["txtPartiallyWorking"] + ", ";

            if (!string.IsNullOrEmpty(strWorkStatus))
                str = str.Replace("#workstatus", "<b><u>WORK STATUS</u>: </b>" + strWorkStatus.TrimEnd(',') + "<br/>");
            else
                str = str.Replace("#workstatus", "");

        }
        else
        {
            str = str.Replace("#dod", "");
            str = str.Replace("#restriction", "");
            str = str.Replace("#workstatus", "");
            str = str.Replace("#activityaffected", "");
        }
        //page3 printing
        query = ("select * from tblPage3HTMLContent where PatientIE_ID= " + lnk.CommandArgument + "");
        ds = db.selectData(query);


        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {


            Dictionary<string, string> page3 = new PrintDocumentHelper().getPage1String(ds.Tables[0].Rows[0]["topSectionHTML"].ToString());

            string strGAIT = !(string.IsNullOrEmpty(page3["txtGAIT"])) ? page3["txtGAIT"] + "." : "";

            if (!string.IsNullOrEmpty(page3["txtAmbulates"]))
            {
                strGAIT = strGAIT + page3["txtAmbulates"].ToString();

                if (page3["chkFootslap"] == "true")
                    strGAIT = strGAIT + ", foot slap/drop";
                if (page3["chkKneehyperextension"] == "true")
                    strGAIT = strGAIT + ", knee hyperextension";
                if (page3["chkUnabletohealwalk"] == "true")
                    strGAIT = strGAIT + ", unable to heel walk";
                if (page3["chkUnabletotoewalk"] == "true")
                    strGAIT = strGAIT + ", unable to toe walk";
                if (!string.IsNullOrEmpty(page3["txtOther"]))
                    strGAIT = strGAIT + " and " + page3["txtOther"];
            }

            //if (!string.IsNullOrEmpty(strGAIT))
            //    str = str.Replace("#gait", "<b><u>GAIT</u>: </b>" + strGAIT + "<br/><br/>");
            //else
            //    str = str.Replace("#gait", "");


            Dictionary<string, string> page3_1 = new PrintDocumentHelper().getPage1String(ds.Tables[0].Rows[0]["HTMLContent"].ToString());

            string strNR = "The patient is alert and cooperative and responding appropriately. Cranial nerves - II-XII are grossly intact ";

            if (!string.IsNullOrEmpty(page3_1["txtIntactExcept"]))
                strNR = strNR + "except " + page3_1["txtIntactExcept"].TrimEnd('.');

            if (!string.IsNullOrEmpty(strNR))
                str = str.Replace("#nerologicalexam", "<b><u>NEUROLOGICAL EXAM</u>: </b>" + strNR.TrimEnd('.') + ".<br/><br/> ");
            else
                str = str.Replace("#nerologicalexam", "");






            string strExceptions = "";
            if (string.IsNullOrEmpty(page3_1["txtDTR1"]) && string.IsNullOrEmpty(page3_1["txtDTR1"]))
            {
                if (!string.IsNullOrEmpty(page3_1["LTricepstxt"]) && page3_1["LTricepstxt"] != "2")
                    strExceptions = "left triceps " + page3_1["LTricepstxt"] + "/2";
                if (!string.IsNullOrEmpty(page3_1["RTricepstxt"]) && page3_1["RTricepstxt"] != "2")
                    strExceptions = strExceptions + ", " + "right triceps " + page3_1["RTricepstxt"] + "/2";
                if (!string.IsNullOrEmpty(page3_1["LBicepstxt"]) && page3_1["LBicepstxt"] != "2")
                    strExceptions = strExceptions + ", " + "left biceps " + page3_1["LBicepstxt"] + "/2";
                if (!string.IsNullOrEmpty(page3_1["RBicepstxt"]) && page3_1["RBicepstxt"] != "2")
                    strExceptions = strExceptions + ", " + "right biceps " + page3_1["RBicepstxt"] + "/2";
                if (!string.IsNullOrEmpty(page3_1["LBrachioradialis"]) && page3_1["LBrachioradialis"] != "2")
                    strExceptions = strExceptions + ", " + "left brachioradialis " + page3_1["LBrachioradialis"] + "/2";
                if (!string.IsNullOrEmpty(page3_1["RBrachioradialis"]) && page3_1["RBrachioradialis"] != "2")
                    strExceptions = strExceptions + ", " + "right brachioradialis " + page3_1["RBrachioradialis"] + "/2";

                if (!string.IsNullOrEmpty(page3_1["LKnee"]) && page3_1["LKnee"] != "2")
                    strExceptions = strExceptions + ", left knee " + page3_1["LKnee"] + "/2";
                if (!string.IsNullOrEmpty(page3_1["RKnee"]) && page3_1["RKnee"] != "2")
                    strExceptions = strExceptions + ", " + "right knee " + page3_1["RKnee"] + "/2";
                if (!string.IsNullOrEmpty(page3_1["LAnkle"]) && page3_1["LAnkle"] != "2")
                    strExceptions = strExceptions + ", " + "left ankle " + page3_1["LAnkle"] + "/2";
                if (!string.IsNullOrEmpty(page3_1["RAnkle"]) && page3_1["RAnkle"] != "2")
                    strExceptions = strExceptions + ", " + "right ankle " + page3_1["RAnkle"] + "/2";
            }
            else
            {

                if (!string.IsNullOrEmpty(page3_1["txtDTR1"]))
                    strExceptions = page3_1["txtDTR1"];
                if (!string.IsNullOrEmpty(page3_1["txtDTR2"]))
                    strExceptions = strExceptions + " " + page3_1["txtDTR2"];
            }


            if (!string.IsNullOrEmpty(strExceptions))
                str = str.Replace("#reflexexam", "<b>REFLEX EXAMINATION: </b>" + strExceptions + "<br/><br/>");
            else
                str = str.Replace("#reflexexam", "");

            string strRE = "", strRElist = "";

            //if (page3_1["chkPinPrick"] == "true")
            //    strRElist = "pinprick";

            //if (page3_1["chkLighttouch"] == "true")
            //    strRElist = strRElist + "," + "light touch. ";

            //if (!string.IsNullOrEmpty(strRElist))
            //    strRElist = "Is checked by " + strRElist.TrimStart(',');


            //if (!string.IsNullOrEmpty(page3_1["txtSensory"]))
            //    strRElist = strRElist + " It is " + page3_1["txtSensory"];

            //strRE = strRElist;

            if (!string.IsNullOrEmpty(page3_1["txtSensory"]))
                str = str.Replace("#sen_exm", "<b>SENSORY EXAMINATION: </b>" + page3_1["txtSensory"] + "<br/><br/>");
            else
            {


                strExceptions = "";
                if (!string.IsNullOrEmpty(page3_1["LLateralarm"]))
                    strExceptions = page3_1["LLateralarm"] + " at left lateral arm (C5)";
                if (!string.IsNullOrEmpty(page3_1["RLateralarm"]))
                    strExceptions = page3_1["RLateralarm"] + " at right lateral arm (C5)";

                if (!string.IsNullOrEmpty(page3_1["LLateralforearm"]))
                    strExceptions = strExceptions + ", " + page3_1["LLateralforearm"] + " at left lateral forearm, thumb, index (C6)";
                if (!string.IsNullOrEmpty(page3_1["RLateralforearm"]))
                    strExceptions = strExceptions + ", " + page3_1["RLateralforearm"] + " at right lateral forearm, thumb, index (C6)";

                if (!string.IsNullOrEmpty(page3_1["LMiddlefinger"]))
                    strExceptions = strExceptions + ", " + page3_1["LMiddlefinger"] + " at left middle finger (C7)";
                if (!string.IsNullOrEmpty(page3_1["RMiddlefinger"]))
                    strExceptions = strExceptions + ", " + page3_1["RMiddlefinger"] + " at right middle finger (C7)";

                if (!string.IsNullOrEmpty(page3_1["LMidialForearm"]))
                    strExceptions = strExceptions + ", " + page3_1["LMidialForearm"] + " at left medial forearm, ring, little finger (C8)";
                if (!string.IsNullOrEmpty(page3_1["RMidialForearm"]))
                    strExceptions = strExceptions + ", " + page3_1["RMidialForearm"] + " at right medial forearm, ring, little finger (C8)";

                if (!string.IsNullOrEmpty(page3_1["LMidialarm"]))
                    strExceptions = strExceptions + ", " + page3_1["LMidialarm"] + " at left medial arm (T1)";
                if (!string.IsNullOrEmpty(page3_1["RMidialarm"]))
                    strExceptions = strExceptions + ", " + page3_1["RMidialarm"] + " at right medial arm (T1)";

                if (!string.IsNullOrEmpty(page3_1["LCervical"]))
                    strExceptions = strExceptions + ", " + page3_1["LCervical"] + " at left cervical paraspinals";
                if (!string.IsNullOrEmpty(page3_1["RCervical"]))
                    strExceptions = strExceptions + ", " + page3_1["RCervical"] + " at right cervical paraspinals";

                if (!string.IsNullOrEmpty(page3_1["LtxtDMTL3"]))
                    strExceptions = strExceptions + ", " + page3_1["LtxtDMTL3"] + " at left distal medial thigh (L3)";
                if (!string.IsNullOrEmpty(page3_1["RtxtDMTL3"]))
                    strExceptions = strExceptions + ", " + page3_1["RtxtDMTL3"] + " at right distal medial thigh (L3)";

                if (!string.IsNullOrEmpty(page3_1["LtxtMLFL4"]))
                    strExceptions = strExceptions + ", " + page3_1["LtxtMLFL4"] + " at left medial left foot (L4)";
                if (!string.IsNullOrEmpty(page3_1["RtxtMLFL4"]))
                    strExceptions = strExceptions + ", " + page3_1["RtxtMLFL4"] + " at right medial left foot (L4)";

                if (!string.IsNullOrEmpty(page3_1["LtxtDOFL5"]))
                    strExceptions = strExceptions + ", " + page3_1["LtxtDOFL5"] + " at left dorsum of the foot (L5)";
                if (!string.IsNullOrEmpty(page3_1["RtxtDOFL5"]))
                    strExceptions = strExceptions + ", " + page3_1["RtxtDOFL5"] + " at right dorsum of the foot (L5)";

                if (!string.IsNullOrEmpty(page3_1["LtxtLTS1"]))
                    strExceptions = strExceptions + ", " + page3_1["LtxtLTS1"] + " at left lateral foot (S1)";
                if (!string.IsNullOrEmpty(page3_1["RtxtLTS1"]))
                    strExceptions = strExceptions + ", " + page3_1["RtxtLTS1"] + " at right lateral foot (S1)";

                if (!string.IsNullOrEmpty(page3_1["LtxtLP"]))
                    strExceptions = strExceptions + ", " + page3_1["LtxtLP"] + " at left lumbar paraspinals";
                if (!string.IsNullOrEmpty(page3_1["RtxtLP"]))
                    strExceptions = strExceptions + ", " + page3_1["RtxtLP"] + " at right lumbar paraspinals";



                string senexam = strExceptions.Trim(',');

                if (!string.IsNullOrEmpty(senexam))
                    str = str.Replace("#sen_exm", "<b>SENSORY EXAMINATION: </b>" + senexam + "<br/><br/>");
                else
                    str = str.Replace("#sen_exm", "");

            }


            if (!string.IsNullOrEmpty(page3_1["txtMST"]))
                str = str.Replace("#mmst", "<b>MOTOR EXAMINATION: </b>" + page3_1["txtMST"] + "<br/><br/>");
            else
            {


                strExceptions = "";
                if (!string.IsNullOrEmpty(page3_1["LAbduction"]))
                    strExceptions = "left shoulder abduction " + page3_1["LAbduction"] + "/5";
                if (!string.IsNullOrEmpty(page3_1["RAbduction"]))
                    strExceptions = strExceptions + ", " + "right shoulder abduction  " + page3_1["RAbduction"] + "/5";

                if (!string.IsNullOrEmpty(page3_1["LFlexion"]))
                    strExceptions = strExceptions + ", " + "left shoulder flexion " + page3_1["LFlexion"] + "/5";
                if (!string.IsNullOrEmpty(page3_1["RFlexion"]))
                    strExceptions = strExceptions + ", " + "right shoulder flexion " + page3_1["RFlexion"] + "/5";


                if (!string.IsNullOrEmpty(page3_1["LElbowExtension"]))
                    strExceptions = strExceptions + ", " + "left elbow extension " + page3_1["LElbowExtension"] + "/5";
                if (!string.IsNullOrEmpty(page3_1["RElbowExtension"]))
                    strExceptions = strExceptions + ", " + "right elbow extension " + page3_1["RElbowExtension"] + "/5";

                if (!string.IsNullOrEmpty(page3_1["LElbowFlexion"]))
                    strExceptions = strExceptions + ", " + "left elbow flexion " + page3_1["LElbowFlexion"] + "/5";
                if (!string.IsNullOrEmpty(page3_1["RElbowFlexion"]))
                    strExceptions = strExceptions + ", " + "right elbow flexion " + page3_1["RElbowFlexion"] + "/5";

                if (!string.IsNullOrEmpty(page3_1["LSupination"]))
                    strExceptions = strExceptions + ", " + "left elbow supination " + page3_1["LSupination"] + "/5";
                if (!string.IsNullOrEmpty(page3_1["RSupination"]))
                    strExceptions = strExceptions + ", " + "right elbow supination " + page3_1["RSupination"] + "/5";


                if (!string.IsNullOrEmpty(page3_1["LPronation"]))
                    strExceptions = strExceptions + ", " + "left elbow pronation " + page3_1["LPronation"] + "/5";
                if (!string.IsNullOrEmpty(page3_1["RPronation"]))
                    strExceptions = strExceptions + ", " + "right elbow pronation " + page3_1["RPronation"] + "/5";


                if (!string.IsNullOrEmpty(page3_1["LWristFlexion"]))
                    strExceptions = strExceptions + ", " + "left wrist flexion " + page3_1["LWristFlexion"] + "/5";
                if (!string.IsNullOrEmpty(page3_1["RWristFlexion"]))
                    strExceptions = strExceptions + ", " + "right wrist flexion " + page3_1["RWristFlexion"] + "/5";

                if (!string.IsNullOrEmpty(page3_1["LWristExtension"]))
                    strExceptions = strExceptions + ", " + "left wrist extension " + page3_1["LWristExtension"] + "/5";
                if (!string.IsNullOrEmpty(page3_1["RWristExtension"]))
                    strExceptions = strExceptions + ", " + "right wrist extension " + page3_1["RWristExtension"] + "/5";


                if (!string.IsNullOrEmpty(page3_1["LGrip"]))
                    strExceptions = strExceptions + ", " + "left hand grip strength " + page3_1["LGrip"] + "/5";
                if (!string.IsNullOrEmpty(page3_1["RGrip"]))
                    strExceptions = strExceptions + ", " + "right hand grip strength " + page3_1["RGrip"] + "/5";

                if (!string.IsNullOrEmpty(page3_1["LFinger"]))
                    strExceptions = strExceptions + ", " + "left hand finger abduction	 " + page3_1["LFinger"] + "/5";
                if (!string.IsNullOrEmpty(page3_1["RFinger"]))
                    strExceptions = strExceptions + ", " + "right hand finger abduction	 " + page3_1["RFinger"] + "/5";

                if (!string.IsNullOrEmpty(page3_1["LHipFlexion"]))
                    strExceptions = strExceptions + ", " + "left hip flexion " + page3_1["LHipFlexion"] + "/5";
                if (!string.IsNullOrEmpty(page3_1["RHipFlexion"]))
                    strExceptions = strExceptions + ", " + "right hip flexion " + page3_1["RHipFlexion"] + "/5";

                if (!string.IsNullOrEmpty(page3_1["LHipAbduction"]))
                    strExceptions = strExceptions + ", left hip abduction " + page3_1["LHipAbduction"] + "/5";
                if (!string.IsNullOrEmpty(page3_1["RHipAbduction"]))
                    strExceptions = strExceptions + ", " + "right hip abduction " + page3_1["RHipAbduction"] + "/5";

                if (!string.IsNullOrEmpty(page3_1["LKneeExtension"]))
                    strExceptions = strExceptions + ", left knee extension " + page3_1["LKneeExtension"] + "/5";
                if (!string.IsNullOrEmpty(page3_1["RKneeExtension"]))
                    strExceptions = strExceptions + ", " + "right knee extension " + page3_1["RKneeExtension"] + "/5";

                if (!string.IsNullOrEmpty(page3_1["LKneeFlexion"]))
                    strExceptions = strExceptions + ", left knee flexion " + page3_1["LKneeFlexion"] + "/5";
                if (!string.IsNullOrEmpty(page3_1["RKneeFlexion"]))
                    strExceptions = strExceptions + ", " + "right knee flexion " + page3_1["RKneeFlexion"] + "/5";

                if (!string.IsNullOrEmpty(page3_1["LDorsiflexion"]))
                    strExceptions = strExceptions + ", left ankle dorsiflexion " + page3_1["LDorsiflexion"] + "/5";
                if (!string.IsNullOrEmpty(page3_1["RDorsiflexion"]))
                    strExceptions = strExceptions + ", " + "right ankle dorsiflexion " + page3_1["RDorsiflexion"] + "/5";

                if (!string.IsNullOrEmpty(page3_1["LPlantar"]))
                    strExceptions = strExceptions + ", left ankle plantar flexion " + page3_1["LPlantar"] + "/5";
                if (!string.IsNullOrEmpty(page3_1["RPlantar"]))
                    strExceptions = strExceptions + ", " + "right ankle plantar flexion " + page3_1["RPlantar"] + "/5";

                if (!string.IsNullOrEmpty(page3_1["LExtensor"]))
                    strExceptions = strExceptions + ", left ankle extensor hallucis longus " + page3_1["LExtensor"] + "/5";
                if (!string.IsNullOrEmpty(page3_1["RExtensor"]))
                    strExceptions = strExceptions + ", " + "right ankle extensor hallucis longus " + page3_1["RExtensor"] + "/5";


                if (!string.IsNullOrEmpty(strExceptions))
                    str = str.Replace("#mmst", "<b>MOTOR EXAMINATION: </b>" + strExceptions.TrimStart(',').TrimEnd('.') + ".<br/><br/>");
                else
                    str = str.Replace("#mmst", "");
            }
        }
        else
        {
            str = str.Replace("#nerologicalexam", "");
            str = str.Replace("#reflexexam", "");
            str = str.Replace("#sensoryexam", "");
            str = str.Replace("#motorexam", "");
            str = str.Replace("#gait", "");
            str = str.Replace("#dtr-ue", "");
            str = str.Replace("#dtr-le", "");
            str = str.Replace("#sen_exm", "");
            str = str.Replace("#mmst", "");

        }

        //page4 printing
        query = "Select * from tblPatientIEDetailPage3 WHERE PatientIE_ID=" + lnk.CommandArgument;
        ds = db.selectData(query);

        string strprocedures = "", strCare = "", strDaignosis = "", strshoulderrightmri = "", strshoulderleftmri = "", strkneerighttmri = "", strkneeleftmri = "";

        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {


            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagCervialBulgeDate"].ToString()))
            {
                strDaignosis = Convert.ToDateTime(ds.Tables[0].Rows[0]["DiagCervialBulgeDate"].ToString()).ToString("MM/dd/yyyy") + " - ";

                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagCervialBulgeStudy"].ToString()))
                    strDaignosis = strDaignosis + " " + ds.Tables[0].Rows[0]["DiagCervialBulgeStudy"].ToString() + " of the ";

                // if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagCervialBulgeText"].ToString()))
                strDaignosis = strDaignosis + " Cervical spine " + ds.Tables[0].Rows[0]["DiagCervialBulgeText"].ToString() + ",";

                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagCervialBulgeHNP1"].ToString()))
                    strDaignosis = strDaignosis + " HNP at " + ds.Tables[0].Rows[0]["DiagCervialBulgeHNP1"].ToString().TrimEnd('.') + ".";

                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagCervialBulgeHNP2"].ToString()))
                    strDaignosis = strDaignosis + ds.Tables[0].Rows[0]["DiagCervialBulgeHNP2"].ToString().TrimEnd('.') + ".";

            }

            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagThoracicBulgeDate"].ToString()))
            {
                strDaignosis = (!string.IsNullOrEmpty(strDaignosis) ? (strDaignosis + "<br/>") : "") + Convert.ToDateTime(ds.Tables[0].Rows[0]["DiagThoracicBulgeDate"].ToString()).ToString("MM/dd/yyyy") + " - ";

                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagThoracicBulgeStudy"].ToString()))
                    strDaignosis = strDaignosis + " " + ds.Tables[0].Rows[0]["DiagThoracicBulgeStudy"].ToString() + " of the ";

                //if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagThoracicBulgeText"].ToString()))
                strDaignosis = strDaignosis + " Thoracic spine " + ds.Tables[0].Rows[0]["DiagThoracicBulgeText"].ToString() + ", ";

                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagThoracicBulgeHNP1"].ToString()))
                    strDaignosis = strDaignosis + " HNP at " + ds.Tables[0].Rows[0]["DiagThoracicBulgeHNP1"].ToString().TrimEnd('.') + ". ";

                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagThoracicBulgeHNP2"].ToString()))
                    strDaignosis = strDaignosis + ds.Tables[0].Rows[0]["DiagThoracicBulgeHNP2"].ToString().TrimEnd('.') + ". ";

            }

            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagLumberBulgeDate"].ToString()))
            {
                strDaignosis = (!string.IsNullOrEmpty(strDaignosis) ? (strDaignosis + "<br/>") : "") + Convert.ToDateTime(ds.Tables[0].Rows[0]["DiagLumberBulgeDate"].ToString()).ToString("MM/dd/yyyy") + " - ";

                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagLumberBulgeStudy"].ToString()))
                    strDaignosis = strDaignosis + " " + ds.Tables[0].Rows[0]["DiagLumberBulgeStudy"].ToString() + " of the ";

                //  if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagLumberBulgeText"].ToString()))
                strDaignosis = strDaignosis + " Lumber spine " + ds.Tables[0].Rows[0]["DiagLumberBulgeText"].ToString() + ", ";

                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagLumberBulgeHNP1"].ToString()))
                    strDaignosis = strDaignosis + " HNP at " + ds.Tables[0].Rows[0]["DiagLumberBulgeHNP1"].ToString().TrimEnd('.') + ". ";

                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagLumberBulgeHNP2"].ToString()))
                    strDaignosis = strDaignosis + ds.Tables[0].Rows[0]["DiagLumberBulgeHNP2"].ToString().TrimEnd('.') + ". ";

            }

            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagLeftShoulderDate"].ToString()))
            {
                strDaignosis = (!string.IsNullOrEmpty(strDaignosis) ? (strDaignosis + "<br/>") : "") + Convert.ToDateTime(ds.Tables[0].Rows[0]["DiagLeftShoulderDate"].ToString()).ToString("MM/dd/yyyy") + " - ";

                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagLeftShoulderStudy"].ToString()))
                    strDaignosis = strDaignosis + " " + ds.Tables[0].Rows[0]["DiagLeftShoulderStudy"].ToString() + " of the ";

                // if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagLeftShoulderText"].ToString()))
                strDaignosis = strDaignosis + " left shoulder " + ds.Tables[0].Rows[0]["DiagLeftShoulderText"].ToString().TrimEnd('.') + ". ";

                //if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagLumberBulgeHNP1"].ToString()))
                //    strDaignosis = strDaignosis + " HNP at " + ds.Tables[0].Rows[0]["DiagLumberBulgeHNP1"].ToString() + ".";

                //if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagLumberBulgeHNP2"].ToString()))
                //    strDaignosis = strDaignosis + ds.Tables[0].Rows[0]["DiagLumberBulgeHNP2"].ToString() + ".";

            }
            //if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagLeftShoulderDate"].ToString()))
            //{
            //    // strshoulderleftmri = Convert.ToDateTime(ds.Tables[0].Rows[0]["DiagLeftShoulderDate"].ToString()).ToString("MM/dd/yyyy") + " - ";

            //    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagLeftShoulderStudy"].ToString()))
            //        strshoulderleftmri = "<b>" + ds.Tables[0].Rows[0]["DiagLeftShoulderStudy"].ToString() + " of the left shoulder:</b> ";


            //    strshoulderleftmri = strshoulderleftmri + ds.Tables[0].Rows[0]["DiagLeftShoulderText"].ToString().TrimEnd('.') + ". ";


            //}

            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagRightShoulderDate"].ToString()))
            {
                strDaignosis = (!string.IsNullOrEmpty(strDaignosis) ? (strDaignosis + "<br/>") : "") + Convert.ToDateTime(ds.Tables[0].Rows[0]["DiagRightShoulderDate"].ToString()).ToString("MM/dd/yyyy") + " - ";

                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagRightShoulderStudy"].ToString()))
                    strDaignosis = strDaignosis + " " + ds.Tables[0].Rows[0]["DiagRightShoulderStudy"].ToString() + " of the ";

                //if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagRightShoulderText"].ToString()))
                strDaignosis = strDaignosis + " right shoulder " + ds.Tables[0].Rows[0]["DiagRightShoulderText"].ToString().TrimEnd('.') + ". ";

            }

            //if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagRightShoulderDate"].ToString()))
            //{
            //    // strshoulderrightmri = Convert.ToDateTime(ds.Tables[0].Rows[0]["DiagRightShoulderDate"].ToString()).ToString("MM/dd/yyyy") + " - ";

            //    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagRightShoulderStudy"].ToString()))
            //        strshoulderrightmri = "<b>" + ds.Tables[0].Rows[0]["DiagRightShoulderStudy"].ToString() + " of the right shoulder:</b> ";

            //    //if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagRightShoulderText"].ToString()))
            //    strshoulderrightmri = strshoulderrightmri + ds.Tables[0].Rows[0]["DiagRightShoulderText"].ToString().TrimEnd('.') + ". ";

            //}

            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagLeftKneeDate"].ToString()))
            {
                strDaignosis = (!string.IsNullOrEmpty(strDaignosis) ? (strDaignosis + "<br/>") : "") + Convert.ToDateTime(ds.Tables[0].Rows[0]["DiagLeftKneeDate"].ToString()).ToString("MM/dd/yyyy") + " - ";

                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagLeftKneeStudy"].ToString()))
                    strDaignosis = strDaignosis + " " + ds.Tables[0].Rows[0]["DiagLeftKneeStudy"].ToString() + " of the ";

                // if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagLeftKneeText"].ToString()))
                strDaignosis = strDaignosis + " left knee " + ds.Tables[0].Rows[0]["DiagLeftKneeText"].ToString().TrimEnd('.') + ". ";

            }
            //if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagLeftKneeDate"].ToString()))
            //{
            //    str = (!string.IsNullOrEmpty(strDaignosis) ? (strDaignosis + "<br/>") : "") + Convert.ToDateTime(ds.Tables[0].Rows[0]["DiagLeftKneeDate"].ToString()).ToString("MM/dd/yyyy") + " - ";

            //    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagLeftKneeStudy"].ToString()))
            //        strDaignosis = strDaignosis + " " + ds.Tables[0].Rows[0]["DiagLeftKneeStudy"].ToString() + " of the ";

            //    // if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagLeftKneeText"].ToString()))
            //    strDaignosis = strDaignosis + " left knee " + ds.Tables[0].Rows[0]["DiagLeftKneeText"].ToString().TrimEnd('.') + ". ";

            //}
            //if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagLeftKneeDate"].ToString()))
            //{
            //    //strkneeleftmri = Convert.ToDateTime(ds.Tables[0].Rows[0]["DiagLeftKneeDate"].ToString()).ToString("MM/dd/yyyy") + " - ";

            //    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagLeftKneeStudy"].ToString()))
            //        strkneeleftmri = "<b>" + ds.Tables[0].Rows[0]["DiagLeftKneeStudy"].ToString() + " of the left knee:</b> ";

            //    // if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagLeftKneeText"].ToString()))
            //    strkneeleftmri = strkneeleftmri + ds.Tables[0].Rows[0]["DiagLeftKneeText"].ToString().TrimEnd('.') + ". ";

            //}

            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagRightKneeDate"].ToString()))
            {
                strDaignosis = (!string.IsNullOrEmpty(strDaignosis) ? (strDaignosis + "<br/>") : "") + Convert.ToDateTime(ds.Tables[0].Rows[0]["DiagRightKneeDate"].ToString()).ToString("MM/dd/yyyy") + " - ";

                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagRightKneeStudy"].ToString()))
                    strDaignosis = strDaignosis + " " + ds.Tables[0].Rows[0]["DiagRightKneeStudy"].ToString() + " of the ";

                //  if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagRightKneeText"].ToString()))
                strDaignosis = strDaignosis + " right knee " + ds.Tables[0].Rows[0]["DiagRightKneeText"].ToString().TrimEnd('.') + ". ";

            }

            //if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagRightKneeDate"].ToString()))
            //{
            //    // strkneerighttmri = Convert.ToDateTime(ds.Tables[0].Rows[0]["DiagRightKneeDate"].ToString()).ToString("MM/dd/yyyy") + " - ";

            //    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagRightKneeStudy"].ToString()))
            //        strkneerighttmri = "<b>" + ds.Tables[0].Rows[0]["DiagRightKneeStudy"].ToString() + " of the right knee:</b> ";

            //    //  if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagRightKneeText"].ToString()))
            //    strkneerighttmri = strkneerighttmri + ds.Tables[0].Rows[0]["DiagRightKneeText"].ToString().TrimEnd('.') + ". ";

            //}

            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["Other1Date"].ToString()))
            {
                strDaignosis = (!string.IsNullOrEmpty(strDaignosis) ? (strDaignosis + "<br/>") : "") + Convert.ToDateTime(ds.Tables[0].Rows[0]["Other1Date"].ToString()).ToString("MM/dd/yyyy") + " - ";

                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["Other1Study"].ToString()))
                    strDaignosis = strDaignosis + " " + ds.Tables[0].Rows[0]["Other1Study"].ToString() + " of the ";

                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["Other1Text"].ToString()))
                    strDaignosis = strDaignosis + ds.Tables[0].Rows[0]["Other1Text"].ToString().TrimEnd('.') + ". ";

            }

            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["Other2Date"].ToString()))
            {
                strDaignosis = (!string.IsNullOrEmpty(strDaignosis) ? (strDaignosis + "<br/>") : "") + Convert.ToDateTime(ds.Tables[0].Rows[0]["Other2Date"].ToString()).ToString("MM/dd/yyyy") + " - ";

                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["Other2Study"].ToString()))
                    strDaignosis = strDaignosis + " " + ds.Tables[0].Rows[0]["Other2Study"].ToString() + " of the ";

                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["Other2Text"].ToString()))
                    strDaignosis = strDaignosis + ds.Tables[0].Rows[0]["Other2Text"].ToString().TrimEnd('.') + ". ";

            }

            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["Other3Date"].ToString()))
            {
                strDaignosis = (!string.IsNullOrEmpty(strDaignosis) ? (strDaignosis + "<br/>") : "") + Convert.ToDateTime(ds.Tables[0].Rows[0]["Other3Date"].ToString()).ToString("MM/dd/yyyy") + " - ";

                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["Other3Study"].ToString()))
                    strDaignosis = strDaignosis + " " + ds.Tables[0].Rows[0]["Other3Study"].ToString() + " of the ";

                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["Other3Text"].ToString()))
                    strDaignosis = strDaignosis + ds.Tables[0].Rows[0]["Other3Text"].ToString().TrimEnd('.') + ". ";

            }

            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["Other4Date"].ToString()))
            {
                strDaignosis = (!string.IsNullOrEmpty(strDaignosis) ? (strDaignosis + "<br/>") : "") + Convert.ToDateTime(ds.Tables[0].Rows[0]["Other4Date"].ToString()).ToString("MM/dd/yyyy") + " - ";

                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["Other4Study"].ToString()))
                    strDaignosis = strDaignosis + " " + ds.Tables[0].Rows[0]["Other4Study"].ToString() + " of the ";

                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["Other4Text"].ToString()))
                    strDaignosis = strDaignosis + ds.Tables[0].Rows[0]["Other4Text"].ToString().TrimEnd('.') + ". ";

            }

            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["Other5Date"].ToString()))
            {
                strDaignosis = (!string.IsNullOrEmpty(strDaignosis) ? (strDaignosis + "<br/>") : "") + Convert.ToDateTime(ds.Tables[0].Rows[0]["Other5Date"].ToString()).ToString("MM/dd/yyyy") + " - ";

                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["Other5Study"].ToString()))
                    strDaignosis = strDaignosis + " " + ds.Tables[0].Rows[0]["Other5Study"].ToString() + " of the ";

                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["Other5Text"].ToString()))
                    strDaignosis = strDaignosis + ds.Tables[0].Rows[0]["Other5Text"].ToString().TrimEnd('.') + ". ";

            }

            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["Other6Date"].ToString()))
            {
                strDaignosis = (!string.IsNullOrEmpty(strDaignosis) ? (strDaignosis + "<br/>") : "") + Convert.ToDateTime(ds.Tables[0].Rows[0]["Other6Date"].ToString()).ToString("MM/dd/yyyy") + " - ";

                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["Other6Study"].ToString()))
                    strDaignosis = strDaignosis + " " + ds.Tables[0].Rows[0]["Other6Study"].ToString() + " of the ";

                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["Other6Text"].ToString()))
                    strDaignosis = strDaignosis + ds.Tables[0].Rows[0]["Other6Text"].ToString().TrimEnd('.') + ". ";

            }

            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["Other7Date"].ToString()))
            {
                strDaignosis = (!string.IsNullOrEmpty(strDaignosis) ? (strDaignosis + "<br/>") : "") + Convert.ToDateTime(ds.Tables[0].Rows[0]["Other7Date"].ToString()).ToString("MM/dd/yyyy") + " - ";

                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["Other7Study"].ToString()))
                    strDaignosis = strDaignosis + " " + ds.Tables[0].Rows[0]["Other7Study"].ToString() + " of the ";

                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["Other7Text"].ToString()))
                    strDaignosis = strDaignosis + ds.Tables[0].Rows[0]["Other7sText"].ToString().TrimEnd('.') + ". ";
            }

            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["OtherMedicine"].ToString()))
            {
                strDaignosis = (!string.IsNullOrEmpty(strDaignosis) ? (strDaignosis + "<br/>") : "") + ds.Tables[0].Rows[0]["OtherMedicine"].ToString();
            }

            query = "Select * from tblMedicationRx WHERE PatientIE_ID = " + lnk.CommandArgument + " Order By Medicine";
            DataSet dsDaig = db.selectData(query);

            if (dsDaig != null && dsDaig.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsDaig.Tables[0].Rows.Count; i++)
                {
                    strDaignosis = (!string.IsNullOrEmpty(strDaignosis) ? (strDaignosis + "<br/>") : "") + dsDaig.Tables[0].Rows[i]["Medicine"].ToString();
                }
            }


            if (!string.IsNullOrEmpty(strDaignosis))
                str = str.Replace("#diagnostic", "<b>DIAGNOSTIC STUDIES: </b><br/>" + strDaignosis + "<br/><br/>");
            else
                str = str.Replace("#diagnostic", "");


            str = str.Replace("#goal", "<b>GOALS: </b>To increase range of motion, strength, flexibility, to decrease pain and to improve body biomechanics and activities of daily living and improve the functional status.<br/><br/>");
            //str = str.Replace("#goal", "");

            strDaignosis = "";


            if (CommonConvert.ToBoolean(ds.Tables[0].Rows[0]["Procedures"].ToString()))
                strprocedures = "If the patient continues to have tender palpable taut bands/trigger points with referral patterns as noted in the future on examination, I will consider doing trigger point injections. ";

            str = str.Replace("#procedures", string.IsNullOrEmpty(strprocedures) ? "" : "<b>PRECAUTIONS: </b>" + strprocedures + "<br/><br/>");

            if (CommonConvert.ToBoolean(ds.Tables[0].Rows[0]["Acupuncture"].ToString()))
                strCare = strCare + ", Acupuncture";

            if (CommonConvert.ToBoolean(ds.Tables[0].Rows[0]["Chiropratic"].ToString()))
                strCare = strCare + ", Chiropratic";

            if (CommonConvert.ToBoolean(ds.Tables[0].Rows[0]["PhysicalTherapy"].ToString()))
                strCare = strCare + ", PhysicalTherapy";

            if (CommonConvert.ToBoolean(ds.Tables[0].Rows[0]["AvoidHeavyLifting"].ToString()))
                strCare = strCare + ", AvoidHeavyLifting";

            if (CommonConvert.ToBoolean(ds.Tables[0].Rows[0]["Carrying"].ToString()))
                strCare = strCare + ", Carrying";

            if (CommonConvert.ToBoolean(ds.Tables[0].Rows[0]["ExcessiveBend"].ToString()))
                strCare = strCare + ", ExcessiveBend";

            if (CommonConvert.ToBoolean(ds.Tables[0].Rows[0]["ProlongedSitStand"].ToString()))
                strCare = strCare + ", ProlongedSitStand";

            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["CareOther"].ToString()))
                strCare = strCare + ", " + ds.Tables[0].Rows[0]["CareOther"].ToString();

            strCare = strCare.TrimStart(',');

            StringBuilder sb = new StringBuilder();
            sb.Append(strCare);

            if (sb.ToString().LastIndexOf(",") > 0)
            {
                sb.Replace(",", " and ", sb.ToString().LastIndexOf(","), 1);
            }

            str = str.Replace("#care", string.IsNullOrEmpty(strCare.TrimStart(',')) ? "" : "<b>CARE: </b>" + sb.ToString().TrimEnd('.') + ".<br/><br/>");


            strprocedures = "";

            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["Precautions"].ToString()))
                strprocedures = ds.Tables[0].Rows[0]["Precautions"].ToString();

            string strproceduresTemp = "";

            //if (CommonConvert.ToBoolean(ds.Tables[0].Rows[0]["Cardiac"].ToString()))
            //    strproceduresTemp = strproceduresTemp + ", Cardiac";

            //if (CommonConvert.ToBoolean(ds.Tables[0].Rows[0]["WeightBearing"].ToString()))
            //    strproceduresTemp = strproceduresTemp + ", Weight Bearing";



            if (!string.IsNullOrEmpty(strproceduresTemp))
                strprocedures = strprocedures + strproceduresTemp.TrimStart(',');

            if (!string.IsNullOrEmpty(strprocedures))
            {
                sb = new StringBuilder();
                sb.Append(strprocedures);

                if (sb.ToString().LastIndexOf(",") > 0)
                {
                    sb.Replace(",", " and ", sb.ToString().LastIndexOf(","), 1);
                }

                strprocedures = sb.ToString() + ". ";
            }

            if (CommonConvert.ToBoolean(ds.Tables[0].Rows[0]["EducationProvided"].ToString()))
                strprocedures = strprocedures + " Patient education provided via";

            if (CommonConvert.ToBoolean(ds.Tables[0].Rows[0]["ViaPhysician"].ToString()))
                strprocedures = strprocedures + ", physician";

            if (CommonConvert.ToBoolean(ds.Tables[0].Rows[0]["ViaPrintedMaterial"].ToString()))
                strprocedures = strprocedures + ", printed material";


            if (CommonConvert.ToBoolean(ds.Tables[0].Rows[0]["ViaWebsite"].ToString()))
                strprocedures = strprocedures + ", online website references";

            if (CommonConvert.ToBoolean(ds.Tables[0].Rows[0]["IsViaVedio"].ToString()))
            {
                strprocedures = strprocedures + ", video";

                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["ViaVideo"].ToString()))
                    strprocedures = strprocedures + " about " + ds.Tables[0].Rows[0]["ViaVideo"].ToString();
            }



            if (!string.IsNullOrEmpty(strprocedures))
            {
                strprocedures = strprocedures.Trim('.') + ".";

                sb = new StringBuilder();
                sb.Append(strprocedures);

                if (strprocedures.IndexOf("and") == 0)
                {
                    if (sb.ToString().LastIndexOf(",") > 0)
                    {
                        sb.Replace(",", " and ", sb.ToString().LastIndexOf(","), 1);
                    }
                }

                str = str.Replace("#precautions", string.IsNullOrEmpty(sb.ToString().TrimStart(',')) ? "" : "<b>PRECAUTIONS: </b>" + (sb.ToString().TrimStart(',').TrimEnd('.').Replace(",,", ",").Replace("..", ".")) + ".<br/><br/>");
            }
            else
            {
                str = str.Replace("#precautions", "");
            }

            strComplain = "";
            if (CommonConvert.ToBoolean(ds.Tables[0].Rows[0]["ConsultNeuro"].ToString()))
                strComplain = strComplain + ", Neurologist";

            if (CommonConvert.ToBoolean(ds.Tables[0].Rows[0]["ConsultOrtho"].ToString()))
                strComplain = strComplain + ", orthopedist";

            if (CommonConvert.ToBoolean(ds.Tables[0].Rows[0]["ConsultPsych"].ToString()))
                strComplain = strComplain + ", psychiatrist";

            if (CommonConvert.ToBoolean(ds.Tables[0].Rows[0]["ConsultPodiatrist"].ToString()))
                strComplain = strComplain + ", podiatrist";


            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["ConsultOther"].ToString()))
                strComplain = strComplain + ", " + ds.Tables[0].Rows[0]["ConsultOther"].ToString();

            sb = new StringBuilder();
            sb.Append(strComplain);

            if (sb.ToString().LastIndexOf(",") > 0)
            {
                sb.Replace(",", " and ", sb.ToString().LastIndexOf(","), 1);
            }


            str = str.Replace("#consultation", string.IsNullOrEmpty(sb.ToString().TrimStart(',')) ? "" : "<b><u>CONSULTATION</u>: </b>" + sb.ToString().ToLower().TrimStart(',') + ".<br/><br/> ");

            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["FollowUpIn"].ToString().Trim()))
                str = str.Replace("#follow-up", "<b>FOLLOW-UP: </b>" + ds.Tables[0].Rows[0]["FollowUpIn"].ToString().Trim() + "<br/><br/>");
            else
                str = str.Replace("#follow-up", "");

            query = "Select * from tblMedicationRx WHERE PatientIE_ID=" + lnk.CommandArgument;
            ds = db.selectData(query);

            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    strMedi = strMedi + ds.Tables[0].Rows[i]["Medicine"].ToString() + "<br/>";
                }
            }

            str = str.Replace("#medications", string.IsNullOrEmpty(strMedi) ? "" : "<b><u>MEDICATIONS</u>: </b><br/>" + strMedi + "<br/><br/>");
        }
        else
        {
            str = str.Replace("#medications", "");
            str = str.Replace("#follow-up", "");
            str = str.Replace("#precautions", "");
            str = str.Replace("#care", "");
            str = str.Replace("#procedures", "");
            str = str.Replace("#diagnostic", "");
            str = str.Replace("#consultation", "");
        }

        //diagnoses printing for all body parts

        strDaignosis = "";

        strDaignosis = this.getDiagnosis("Neck", lnk.CommandArgument);
        strDaignosis = strDaignosis + this.getDiagnosis("Midback", lnk.CommandArgument);
        strDaignosis = strDaignosis + this.getDiagnosis("Lowback", lnk.CommandArgument);
        strDaignosis = strDaignosis + this.getDiagnosisRightLeft(lnk.CommandArgument);


        if (!string.IsNullOrEmpty(strDaignosis))
            str = str.Replace("#diagnoses", "<b>DIAGNOSES: </b>" + strDaignosis + "<br/><br/>");
        else
            str = str.Replace("#diagnoses", "");


        //plan printing for all body parts


        string strPlan = "";
        if (!string.IsNullOrEmpty(this.getPOC("Neck", lnk.CommandArgument)))
            strPlan = strPlan + "<br/>";
        strPlan = strPlan + this.getPOC("Neck", lnk.CommandArgument);

        strPlan = strPlan + (string.IsNullOrEmpty(this.getPlan("tblbpNeck", lnk.CommandArgument)) == false ? this.getPlan("tblbpNeck", lnk.CommandArgument) : "");

        if (!string.IsNullOrEmpty(this.getPOC("MidBack", lnk.CommandArgument)))
            strPlan = strPlan + "<br/>";
        strPlan = strPlan + this.getPOC("MidBack", lnk.CommandArgument);

        strPlan = strPlan + (string.IsNullOrEmpty(this.getPlan("tblbpMidback", lnk.CommandArgument)) == false ? this.getPlan("tblbpMidback", lnk.CommandArgument) : "");

        if (!string.IsNullOrEmpty(this.getPOC("LowBack", lnk.CommandArgument)))
            strPlan = strPlan + "<br/>";
        strPlan = strPlan + this.getPOC("LowBack", lnk.CommandArgument);

        strPlan = strPlan + (string.IsNullOrEmpty(this.getPlan("tblbpLowback", lnk.CommandArgument)) == false ? this.getPlan("tblbpLowback", lnk.CommandArgument) : "");

        if (!string.IsNullOrEmpty(this.getPOC("Shoulder", lnk.CommandArgument)))
            strPlan = strPlan + "<br/>";
        strPlan = strPlan + this.getPOC("Shoulder", lnk.CommandArgument);

        strPlan = strPlan + (string.IsNullOrEmpty(this.getPlan("tblbpShoulder", lnk.CommandArgument)) == false ? this.getPlan("tblbpShoulder", lnk.CommandArgument) : "");

        if (!string.IsNullOrEmpty(this.getPOC("Knee", lnk.CommandArgument)))
            strPlan = strPlan + "<br/>";
        strPlan = strPlan + this.getPOC("Knee", lnk.CommandArgument);

        strPlan = strPlan + (string.IsNullOrEmpty(this.getPlan("tblbpKnee", lnk.CommandArgument)) == false ? this.getPlan("tblbpKnee", lnk.CommandArgument) : "");

        if (!string.IsNullOrEmpty(this.getPOC("Elbow", lnk.CommandArgument)))
            strPlan = strPlan + "<br/>";
        strPlan = strPlan + this.getPOC("Elbow", lnk.CommandArgument);

        strPlan = strPlan + (string.IsNullOrEmpty(this.getPlan("tblbpElbow", lnk.CommandArgument)) == false ? this.getPlan("tblbpElbow", lnk.CommandArgument) : "");

        if (!string.IsNullOrEmpty(this.getPOC("Wrist", lnk.CommandArgument)))
            strPlan = strPlan + "<br/>";
        strPlan = strPlan + this.getPOC("Wrist", lnk.CommandArgument);

        strPlan = strPlan + (string.IsNullOrEmpty(this.getPlan("tblbpWrist", lnk.CommandArgument)) == false ? this.getPlan("tblbpWrist", lnk.CommandArgument) : "");

        if (!string.IsNullOrEmpty(this.getPOC("Hip", lnk.CommandArgument)))
            strPlan = strPlan + "<br/>";
        strPlan = strPlan + this.getPOC("Hip", lnk.CommandArgument);

        strPlan = strPlan + (string.IsNullOrEmpty(this.getPlan("tblbpHip", lnk.CommandArgument)) == false ? this.getPlan("tblbpHip", lnk.CommandArgument) : "");

        if (!string.IsNullOrEmpty(this.getPOC("Ankle", lnk.CommandArgument)))
            strPlan = strPlan + "<br/>";
        strPlan = strPlan + this.getPOC("Ankle", lnk.CommandArgument);

        strPlan = strPlan + (string.IsNullOrEmpty(this.getPlan("tblbpAnkle", lnk.CommandArgument)) == false ? this.getPlan("tblbpAnkle", lnk.CommandArgument) : "");

        if (!string.IsNullOrEmpty(this.getPOC("OtherPart", lnk.CommandArgument)))
            strPlan = strPlan + "<br/>";
        strPlan = strPlan + this.getPOC("OtherPart", lnk.CommandArgument);

        strPlan = strPlan + (string.IsNullOrEmpty(this.getPlan("tblbpOtherPart", lnk.CommandArgument)) == false ? this.getPlan("tblbpOtherPart", lnk.CommandArgument) : "");


        str = str.Replace("#plan", string.IsNullOrEmpty(strPlan) ? "" : "<br/>" + "<b>RECOMMENDATIONS: </b>" + strPlan);


        //neck printing string

        query = ("select CCvalue from tblbpNeck where PatientIE_ID= " + lnk.CommandArgument + "");
        SqlCommand cm = new SqlCommand(query, cn);
        SqlDataAdapter da = new SqlDataAdapter(cm);
        cn.Open();
        ds = new DataSet();
        da.Fill(ds);


        string neckCC = "", neckTP = "", lowbackCC = "", shoudlerCC = "", kneeCC = "", elbowCC = "", wristCC = "", hipCC = "", ankleCC = "";



        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["CCvalue"].ToString()))
            {
                neckCC = helper.getDocumentString(ds.Tables[0].Rows[0]["CCvalue"].ToString());
                neckCC = formatString(neckCC);
                str = str.Replace("#neck", neckCC + "<br/><br/>");
            }
            else
            {
                str = str.Replace("#neck", "");

            }
        }
        else
        {
            str = str.Replace("#neck", "");

        }

        //neck PE printing string
        query = ("select PEvalue,PESides,PESidesText,NameROM,LeftROM,RightROM,NormalROM,CNameROM,CROM,CNormalROM from tblbpNeck where PatientIE_ID= " + lnk.CommandArgument + "");
        string neckPE = "";
        cm = new SqlCommand(query, cn);
        da = new SqlDataAdapter(cm);
        ds = new DataSet();
        da.Fill(ds);

        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["PEvalue"].ToString()))
            {
                neckPE = helper.getDocumentString(ds.Tables[0].Rows[0]["PEvalue"].ToString());
                neckTP = this.getTPString(ds.Tables[0].Rows[0]["PESides"].ToString(), ds.Tables[0].Rows[0]["PESidesText"].ToString());
            }

            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["NameROM"].ToString()))
            {
                string romstrleft = this.getROMString(ds.Tables[0].Rows[0]["NameROM"].ToString(), ds.Tables[0].Rows[0]["LeftROM"].ToString(), ds.Tables[0].Rows[0]["NormalROM"].ToString(), "left", "IE", "Neck");

                string romstrright = this.getROMString(ds.Tables[0].Rows[0]["NameROM"].ToString(), ds.Tables[0].Rows[0]["RightROM"].ToString(), ds.Tables[0].Rows[0]["NormalROM"].ToString(), "right", "IE", "Neck");
                string romstrC = this.getROMString(ds.Tables[0].Rows[0]["CNameROM"].ToString(), ds.Tables[0].Rows[0]["CROM"].ToString(), ds.Tables[0].Rows[0]["CNormalROM"].ToString(), "", "IE");
                string romstr = romstrleft.Replace(".", ";") + " " + romstrright;

                string finalrom = "";
                if (!string.IsNullOrEmpty(romstrC))
                    finalrom = "ROM is as follows:" + romstrC;

                if (!string.IsNullOrEmpty(romstr) && romstr != " ")
                {
                    if (string.IsNullOrEmpty(romstrC))
                        finalrom = "ROM is as follows: " + romstr.TrimEnd(';') + ".";
                    else
                        finalrom = finalrom.Replace(".", "") + ";" + romstr.TrimEnd(';') + ".";
                }

                if (!string.IsNullOrEmpty(neckTP))
                {
                    neckTP = " There are palpable taut bands/trigger points at " + neckTP.TrimStart(',') + ". ";

                    finalrom = finalrom.Replace("..", ".") + neckTP;
                }

                if (!string.IsNullOrEmpty(finalrom))

                    neckPE = neckPE.Replace("#romneck", finalrom);
                else
                    neckPE = neckPE.Replace("#romneck", "");

                neckPE = neckPE.Replace("#necknotebp", "<br/><br/>");

            }


            if (!string.IsNullOrEmpty(neckPE))
            {
                neckPE = formatString(neckPE);
                str = str.Replace("#PENeck", "<b>CERVICAL SPINE EXAMINATION: </b>" + neckPE + "<br/><br/>");
            }
            else
                str = str.Replace("#PENeck", "");

        }
        else
            str = str.Replace("#PENeck", neckPE);


        //lowback printing string
        query = ("select CCvalue from tblbpLowback where PatientIE_ID= " + lnk.CommandArgument + "");
        cm = new SqlCommand(query, cn);
        da = new SqlDataAdapter(cm);
        ds = new DataSet();
        da.Fill(ds);

        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["CCvalue"].ToString()))
            {
                lowbackCC = helper.getDocumentString(ds.Tables[0].Rows[0]["CCvalue"].ToString());
                lowbackCC = formatString(lowbackCC);
                str = str.Replace("#lowback", lowbackCC + "<br/><br/>");

            }
            else
                str = str.Replace("#lowback", lowbackCC);
        }
        else
            str = str.Replace("#lowback", lowbackCC);


        //lowback PE printing string
        query = ("select PEvalue,PESides,PESidesText,NameROM,LeftROM,RightROM,NormalROM,CNameROM,CROM,CNormalROM,NameTest,LeftTest,RightTest,TextVal  from tblbpLowback where PatientIE_ID= " + lnk.CommandArgument + "");
        string lowbackPE = "", lowbackTP = "";
        cm = new SqlCommand(query, cn);
        da = new SqlDataAdapter(cm);
        ds = new DataSet();
        da.Fill(ds);

        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["PEvalue"].ToString()))
            {
                lowbackPE = helper.getDocumentString(ds.Tables[0].Rows[0]["PEvalue"].ToString());
                lowbackTP = this.getTPString(ds.Tables[0].Rows[0]["PESides"].ToString(), ds.Tables[0].Rows[0]["PESidesText"].ToString());


            }

            string finalrom = "";
            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["NameROM"].ToString()))
            {
                string romstrleft = this.getROMString(ds.Tables[0].Rows[0]["NameROM"].ToString(), ds.Tables[0].Rows[0]["LeftROM"].ToString(), ds.Tables[0].Rows[0]["NormalROM"].ToString(), "left", "IE", "Lowback");
                string romstrright = this.getROMString(ds.Tables[0].Rows[0]["NameROM"].ToString(), ds.Tables[0].Rows[0]["RightROM"].ToString(), ds.Tables[0].Rows[0]["NormalROM"].ToString(), "right", "IE", "Lowback");
                string romstrC = this.getROMString(ds.Tables[0].Rows[0]["CNameROM"].ToString(), ds.Tables[0].Rows[0]["CROM"].ToString(), ds.Tables[0].Rows[0]["CNormalROM"].ToString(), "", "IE");
                string romstr = romstrleft.Replace(",", ";") + " " + romstrright.TrimStart(';');



                if (!string.IsNullOrEmpty(romstrC))
                    finalrom = "<br/>ROM is as follows: " + romstrC.TrimStart(';') + ". ";

                if (!string.IsNullOrEmpty(romstr) && romstr != " ")
                {
                    if (string.IsNullOrEmpty(romstrC))
                        finalrom = "ROM is as follows: " + romstr.TrimEnd(';') + ".";
                    else
                        finalrom = finalrom.Replace(".", "") + ";" + romstr.TrimEnd(';') + ".";
                }

            }



            if (!string.IsNullOrEmpty(lowbackTP))
            {
                lowbackTP = " There are palpable taut bands/trigger points at " + lowbackTP.TrimStart(',') + ". ";

                finalrom = finalrom.Replace("..", ".") + lowbackTP;
            }

            if (!string.IsNullOrEmpty(finalrom))

                lowbackPE = lowbackPE.Replace("#romlowback", finalrom);
            else
                lowbackPE = lowbackPE.Replace("#romlowback", "");

            lowbackPE = lowbackPE.Replace("#lowbacknotebp", "");


            //get test string

            string strTest = helper.getLowbackTestString(ds.Tables[0].Rows[0]["NameTest"].ToString(), ds.Tables[0].Rows[0]["LeftTest"].ToString(), ds.Tables[0].Rows[0]["RightTest"].ToString(), ds.Tables[0].Rows[0]["TextVal"].ToString());

            if (!string.IsNullOrEmpty(strTest))
                lowbackPE = lowbackPE + "." + strTest.TrimStart(',') + ".";

            if (!string.IsNullOrEmpty(lowbackPE))
            {
                lowbackPE = formatString(lowbackPE);
                str = str.Replace("#PELowback", "<b>LUMBAR SPINE EXAMINATION: </b>" + lowbackPE + "<br/><br/>");
            }
            else
                str = str.Replace("#PELowback", "");

        }
        else
            str = str.Replace("#PELowback", lowbackPE);

        //midback printing string
        string midbackCC = "";
        query = ("select CCvalue from tblbpMidback where PatientIE_ID= " + lnk.CommandArgument + "");
        cm = new SqlCommand(query, cn);
        da = new SqlDataAdapter(cm);
        ds = new DataSet();
        da.Fill(ds);

        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["CCvalue"].ToString()))
            {
                midbackCC = helper.getDocumentString(ds.Tables[0].Rows[0]["CCvalue"].ToString());
                midbackCC = formatString(midbackCC);
                str = str.Replace("#midback", midbackCC.Replace(" /", "/") + "<br/><br/>");
            }
            else
                str = str.Replace("#midback", midbackCC);
        }
        else
            str = str.Replace("#midback", midbackCC);

        //midback PE printing string
        string midbackPE = "", midbackTP = "";
        query = ("select PEvalue,PESides,PESidesText from tblbpMidback where PatientIE_ID= " + lnk.CommandArgument + "");
        cm = new SqlCommand(query, cn);
        da = new SqlDataAdapter(cm);
        ds = new DataSet();
        da.Fill(ds);

        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["PEvalue"].ToString()))
            {
                midbackPE = helper.getDocumentString(ds.Tables[0].Rows[0]["PEvalue"].ToString());
                midbackPE = midbackPE.Replace(",,", ",");
                midbackPE = this.formatString(midbackPE);
                midbackTP = this.getTPString(ds.Tables[0].Rows[0]["PESides"].ToString(), ds.Tables[0].Rows[0]["PESidesText"].ToString());
                if (!string.IsNullOrEmpty(midbackTP))
                    midbackPE = midbackPE + "There are palpable taut bands/trigger points at " + midbackTP.TrimStart(',') + ". ";

                midbackPE = formatString(midbackPE);


                str = str.Replace("#PEMidback", "<b>THORACIC SPINE EXAMINATION: </b>" + midbackPE + "<br/><br/>");

            }
            else
                str = str.Replace("#PEMidback", midbackPE);
        }
        else
            str = str.Replace("#PEMidback", midbackPE);

        //shoulder printing string
        query = ("select CCvalue from tblbpShoulder where PatientIE_ID= " + lnk.CommandArgument + "");
        cm = new SqlCommand(query, cn);
        da = new SqlDataAdapter(cm);
        ds = new DataSet();
        da.Fill(ds);

        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["CCvalue"].ToString()))
            {
                shoudlerCC = helper.getDocumentStringLeftRight(ds.Tables[0].Rows[0]["CCvalue"].ToString(), "Shoulder");

                shoudlerCC = formatString(shoudlerCC);

                str = str.Replace("#shoulder", shoudlerCC.Replace(" /", "/") + "<br/><br/>");
            }
            else
                str = str.Replace("#shoulder", shoudlerCC);
        }
        else
            str = str.Replace("#shoulder", shoudlerCC);

        //shoulder PE printing string
        query = ("select PEvalue,NameROM,LeftROM,RightROM,NormalROM,PESides,PESidesText from tblbpshoulder where PatientIE_ID= " + lnk.CommandArgument + "");
        string shoulderPE = "", shoulderTP = "";
        cm = new SqlCommand(query, cn);
        da = new SqlDataAdapter(cm);
        ds = new DataSet();
        da.Fill(ds);


        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["PEvalue"].ToString()))
            {
                shoulderPE = helper.getDocumentStringLeftRightPE(ds.Tables[0].Rows[0]["PEvalue"].ToString());
                shoulderPE = shoulderPE.Replace(",,", ", ").Replace(" ,", ", ");
                shoulderPE = shoulderPE.Replace("Positive for,", "Test positive for ").Replace("Positive for and ", "positive for ");
                //        shoulderTP = this.getTPString(ds.Tables[0].Rows[0]["PESides"].ToString(), ds.Tables[0].Rows[0]["PESidesText"].ToString());

            }

            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["LeftROM"].ToString()))
            {
                string romstr = this.getROMString(ds.Tables[0].Rows[0]["NameROM"].ToString(), ds.Tables[0].Rows[0]["LeftROM"].ToString(), ds.Tables[0].Rows[0]["NormalROM"].ToString(), "left");
                if (!string.IsNullOrEmpty(romstr))
                    shoulderPE = shoulderPE.Replace("#shoulderleftrom", "<br/>ROM is as follows: " + romstr);
                else
                    shoulderPE = shoulderPE.Replace("#shoulderleftrom", "");
            }

            shoulderPE = shoulderPE.Replace("#shoulderleftmri", string.IsNullOrEmpty(strshoulderleftmri) ? "" : "<br/><br/>" + strshoulderleftmri);


            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["RightROM"].ToString()))
            {
                string romstr = this.getROMString(ds.Tables[0].Rows[0]["NameROM"].ToString(), ds.Tables[0].Rows[0]["RightROM"].ToString(), ds.Tables[0].Rows[0]["NormalROM"].ToString(), "right");
                if (!string.IsNullOrEmpty(romstr))
                    shoulderPE = shoulderPE.Replace("#shoulderrightrom", "<br/>ROM is as follows: " + romstr);
                else
                    shoulderPE = shoulderPE.Replace("#shoulderrightrom", "");
            }

            shoulderPE = shoulderPE.Replace("#shoulderrightmri", string.IsNullOrEmpty(strshoulderrightmri) ? "" : "<br/><br/>" + strshoulderrightmri);


            //if (!string.IsNullOrEmpty(shoulderTP))
            //    shoulderPE = shoulderPE + "There are palpable taut bands/trigger points at " + shoulderTP.TrimStart(',') + " with referral to the scapula. " +
            //        "";

            if (!string.IsNullOrEmpty(shoulderPE))
            {
                shoulderPE = shoulderPE.Replace("#rightshouldertitle", "<b>RIGHT SHOULDER EXAMINATION: </b> ");
                shoulderPE = shoulderPE.Replace("#leftshouldertitle", "<b>LEFT SHOULDER EXAMINATION: </b>");
                shoulderPE = formatString(shoulderPE);
                str = str.Replace("#PEShoudler", shoulderPE + "<br/><br/>");
            }
            else
            {
                str = str.Replace("#PEShoudler", "");
                shoulderPE = shoulderPE.Replace("#rightshouldertitle", "");
                shoulderPE = shoulderPE.Replace("#leftshouldertitle", "");
            }

        }
        else
            str = str.Replace("#PEShoudler", shoulderPE);



        //knee printing string
        query = ("select CCvalue from tblbpKnee where PatientIE_ID= " + lnk.CommandArgument + "");
        cm = new SqlCommand(query, cn);
        da = new SqlDataAdapter(cm);
        ds = new DataSet();
        da.Fill(ds);

        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["CCvalue"].ToString()))
            {
                kneeCC = helper.getDocumentStringLeftRight(ds.Tables[0].Rows[0]["CCvalue"].ToString(), "Knee");

                kneeCC = formatString(kneeCC);

                str = str.Replace("#knee", kneeCC.Replace(" /", "/") + "<br/><br/>");
            }
            else
                str = str.Replace("#knee", kneeCC);
        }
        else
            str = str.Replace("#knee", kneeCC);

        //knee PE printing string
        query = ("select PEvalue,NameROM,LeftROM,RightROM,NormalROM from tblbpKnee where PatientIE_ID= " + lnk.CommandArgument + "");
        string kneePE = "";
        cm = new SqlCommand(query, cn);
        da = new SqlDataAdapter(cm);
        ds = new DataSet();
        da.Fill(ds);

        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["PEvalue"].ToString()))
            {

                kneePE = helper.getDocumentStringLeftRightPE(ds.Tables[0].Rows[0]["PEvalue"].ToString());
                kneePE = kneePE.Replace(",,", ",");
                kneePE = kneePE.Replace("Positive for,", "Test positive for ").Replace("Positive for and ", "positive for ");
            }

            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["LeftROM"].ToString()))
            {
                string romstr = this.getROMString(ds.Tables[0].Rows[0]["NameROM"].ToString(), ds.Tables[0].Rows[0]["LeftROM"].ToString(), ds.Tables[0].Rows[0]["NormalROM"].ToString(), "left");

                if (!string.IsNullOrEmpty(romstr))
                    kneePE = kneePE.Replace("#kneeleftrom", "<br/>ROM is as follows: " + romstr);
                else
                    kneePE = kneePE.Replace("#kneeleftrom", "");


            }

            kneePE = kneePE.Replace("#kneerightmri", string.IsNullOrEmpty(strkneerighttmri) ? "" : "<br/><br/>" + strkneerighttmri);


            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["RightROM"].ToString()))
            {
                string romstr = this.getROMString(ds.Tables[0].Rows[0]["NameROM"].ToString(), ds.Tables[0].Rows[0]["RightROM"].ToString(), ds.Tables[0].Rows[0]["NormalROM"].ToString(), "right");
                if (!string.IsNullOrEmpty(romstr))
                    kneePE = kneePE.Replace("#kneerightrom", "<br/>ROM is as follows: " + romstr);
                else
                    kneePE = kneePE.Replace("#kneerightrom", "");
            }

            kneePE = kneePE.Replace("#kneeleftmri", string.IsNullOrEmpty(strkneeleftmri) ? "" : "<br/><br/>" + strkneeleftmri);


            if (!string.IsNullOrEmpty(kneePE))
            {
                kneePE = kneePE.Replace("#leftkneetitle", "<b>LEFT KNEE EXAMINATION: </b>");
                kneePE = kneePE.Replace("#rightkneetitle", "<b>RIGHT KNEE EXAMINATION: </b>");

                kneePE = formatString(kneePE);

                str = str.Replace("#PEKnee", kneePE + "<br/><br/>");

            }
            else
            {
                kneePE = kneePE.Replace("#leftkneetitle", "");
                kneePE = kneePE.Replace("#rightkneetitle", "");
                str = str.Replace("#PEKnee", "");
            }

        }
        else
            str = str.Replace("#PEKnee", kneePE);

        //elbow printing string
        query = ("select CCvalue from tblbpElbow where PatientIE_ID= " + lnk.CommandArgument + "");
        cm = new SqlCommand(query, cn);
        da = new SqlDataAdapter(cm);
        ds = new DataSet();
        da.Fill(ds);

        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["CCvalue"].ToString()))
            {
                elbowCC = helper.getDocumentStringLeftRight(ds.Tables[0].Rows[0]["CCvalue"].ToString(), "Elbow");

                elbowCC = formatString(elbowCC);

                str = str.Replace("#elbow", elbowCC.Replace(" /", "/") + "<br/><br/>");
            }
            else
                str = str.Replace("#elbow", elbowCC);
        }
        else
            str = str.Replace("#elbow", elbowCC);

        //elbow PE printing string
        string elbowPE = "";
        query = ("select  PEvalue,NameROM,LeftROM,RightROM,NormalROM  from tblbpElbow where PatientIE_ID= " + lnk.CommandArgument + "");
        cm = new SqlCommand(query, cn);
        da = new SqlDataAdapter(cm);
        ds = new DataSet();
        da.Fill(ds);

        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["PEvalue"].ToString()))
            {
                elbowPE = helper.getDocumentStringLeftRightPE(ds.Tables[0].Rows[0]["PEvalue"].ToString());
                elbowPE = elbowPE.Replace(",,", ",");
            }

            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["LeftROM"].ToString()))
            {
                string romstr = this.getROMString(ds.Tables[0].Rows[0]["NameROM"].ToString(), ds.Tables[0].Rows[0]["LeftROM"].ToString(), ds.Tables[0].Rows[0]["NormalROM"].ToString(), "left");
                if (!string.IsNullOrEmpty(romstr))
                    elbowPE = elbowPE + " ROM is as follows: " + romstr;
            }

            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["RightROM"].ToString()))
            {
                string romstr = this.getROMString(ds.Tables[0].Rows[0]["NameROM"].ToString(), ds.Tables[0].Rows[0]["RightROM"].ToString(), ds.Tables[0].Rows[0]["NormalROM"].ToString(), "right");
                if (!string.IsNullOrEmpty(romstr))
                    elbowPE = elbowPE + " ROM is as follows: " + romstr;
            }



            if (!string.IsNullOrEmpty(elbowPE))
            {
                elbowPE = elbowPE.Replace("#leftelbowtitle", "<b>LEFT ELBOW EXAMINATION: </b>");
                elbowPE = elbowPE.Replace("#rightelbowtitle", "<b>RIGHT ELBOW EXAMINATION: </b>");
                elbowPE = formatString(elbowPE);
                str = str.Replace("#PEElbow", elbowPE + "<br/><br/>");
            }
            else
            {
                str = str.Replace("#PEElbow", "");
                elbowPE = elbowPE.Replace("#leftelbowtitle", "");
                elbowPE = elbowPE.Replace("#rightelbowtitle", "");
            }

        }
        else
            str = str.Replace("#PEElbow", elbowPE);

        //wrist printing string
        query = ("select CCvalue from tblbpWrist where PatientIE_ID= " + lnk.CommandArgument + "");
        cm = new SqlCommand(query, cn);
        da = new SqlDataAdapter(cm);
        ds = new DataSet();
        da.Fill(ds);

        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["CCvalue"].ToString()))
            {
                wristCC = helper.getDocumentStringLeftRight(ds.Tables[0].Rows[0]["CCvalue"].ToString(), "Wrist");
                wristCC = formatString(wristCC);
                str = str.Replace("#wrist", wristCC.Replace(" /", "/") + "<br/><br/>");

            }
            else
                str = str.Replace("#wrist", wristCC);
        }
        else
            str = str.Replace("#wrist", wristCC);

        //hip printing string
        query = ("select CCvalue from tblbpHip where PatientIE_ID= " + lnk.CommandArgument + "");
        cm = new SqlCommand(query, cn);
        da = new SqlDataAdapter(cm);
        ds = new DataSet();
        da.Fill(ds);

        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["CCvalue"].ToString()))
            {
                hipCC = helper.getDocumentStringLeftRight(ds.Tables[0].Rows[0]["CCvalue"].ToString(), "Hip");
                hipCC = formatString(hipCC);
                str = str.Replace("#hip", hipCC.Replace(" /", "/") + "<br/><br/>");

            }
            else
                str = str.Replace("#hip", hipCC);
        }
        else
            str = str.Replace("#hip", hipCC);

        //hip PE printing string
        string hipPE = "";
        query = ("select PEvalue,NameROM,LeftROM,RightROM,NormalROM from tblbpHip where PatientIE_ID= " + lnk.CommandArgument + "");
        cm = new SqlCommand(query, cn);
        da = new SqlDataAdapter(cm);
        ds = new DataSet();
        da.Fill(ds);

        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["PEvalue"].ToString()))
            {
                hipPE = helper.getDocumentStringLeftRightPE(ds.Tables[0].Rows[0]["PEvalue"].ToString());
                hipPE = hipPE.Replace(",,", ",");
            }

            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["LeftROM"].ToString()))
            {
                string romstr = this.getROMString(ds.Tables[0].Rows[0]["NameROM"].ToString(), ds.Tables[0].Rows[0]["LeftROM"].ToString(), ds.Tables[0].Rows[0]["NormalROM"].ToString(), "left");
                if (!string.IsNullOrEmpty(romstr))
                    hipPE = hipPE + " ROM is as follows: " + romstr;
            }

            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["RightROM"].ToString()))
            {
                string romstr = this.getROMString(ds.Tables[0].Rows[0]["NameROM"].ToString(), ds.Tables[0].Rows[0]["RightROM"].ToString(), ds.Tables[0].Rows[0]["NormalROM"].ToString(), "right");
                if (!string.IsNullOrEmpty(romstr))
                    hipPE = hipPE + " ROM is as follows: " + romstr;
            }

            if (!string.IsNullOrEmpty(hipPE))
            {

                hipPE = hipPE.Replace("#lefthiptitle", "<b>LEFT HIP EXAMINATION: </b>");
                hipPE = hipPE.Replace("#rigthhiptitle", "<b>RIGHT HIP EXAMINATION: </b>");

                hipPE = formatString(hipPE);

                str = str.Replace("#PEHip", hipPE + "<br/><br/>");
            }
            else
            {
                hipPE = hipPE.Replace("#lefthiptitle", "");
                hipPE = hipPE.Replace("#rigthhiptitle", "");
                str = str.Replace("#PEHip", "");
            }
        }
        else
            str = str.Replace("#PEHip", "");


        //ankle printing string
        query = ("select CCvalue from tblbpAnkle where PatientIE_ID= " + lnk.CommandArgument + "");
        cm = new SqlCommand(query, cn);
        da = new SqlDataAdapter(cm);
        ds = new DataSet();
        da.Fill(ds);

        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["CCvalue"].ToString()))
            {
                ankleCC = helper.getDocumentStringLeftRight(ds.Tables[0].Rows[0]["CCvalue"].ToString(), "Ankle");
                ankleCC = formatString(ankleCC);

                str = str.Replace("#ankle", ankleCC.Replace(" /", "/") + "<br/><br/>");

            }
            else
                str = str.Replace("#ankle", ankleCC);
        }
        else
            str = str.Replace("#ankle", ankleCC);


        //ankle PE printing string
        string anklePE = "";
        query = ("select PEvalue,NameROM,LeftROM,RightROM,NormalROM from tblbpAnkle where PatientIE_ID= " + lnk.CommandArgument + "");
        cm = new SqlCommand(query, cn);
        da = new SqlDataAdapter(cm);
        ds = new DataSet();
        da.Fill(ds);

        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["PEvalue"].ToString()))
            {
                anklePE = helper.getDocumentStringLeftRightPE(ds.Tables[0].Rows[0]["PEvalue"].ToString());
                anklePE = anklePE.Replace(",,", ",");
            }

            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["LeftROM"].ToString()))
            {
                string romstr = this.getROMString(ds.Tables[0].Rows[0]["NameROM"].ToString(), ds.Tables[0].Rows[0]["LeftROM"].ToString(), ds.Tables[0].Rows[0]["NormalROM"].ToString(), "left");
                if (!string.IsNullOrEmpty(romstr))
                    anklePE = anklePE + " ROM is as follows: " + romstr;
            }

            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["RightROM"].ToString()))
            {
                string romstr = this.getROMString(ds.Tables[0].Rows[0]["NameROM"].ToString(), ds.Tables[0].Rows[0]["RightROM"].ToString(), ds.Tables[0].Rows[0]["NormalROM"].ToString(), "right");
                if (!string.IsNullOrEmpty(romstr))
                    anklePE = anklePE + " ROM is as follows: " + romstr;
            }

            if (!string.IsNullOrEmpty(anklePE))
            {

                anklePE = anklePE.Replace("#leftankletitle", "<b>LEFT ANKLE EXAMINATION: </b>");
                anklePE = anklePE.Replace("#rigthankletitle", "<b>RIGHT ANKLE EXAMINATION: </b>");
                anklePE = formatString(anklePE);
                str = str.Replace("#PEAnkle", anklePE + "<br/><br/>");

            }
            else
            {
                anklePE = anklePE.Replace("#leftankletitle", "");
                anklePE = anklePE.Replace("#rigthankletitle", "");
                str = str.Replace("#PEAnkle", "");
            }

        }
        else
            str = str.Replace("#PEAnkle", anklePE);



        //wrist PE printing string
        string wristPE = "";
        query = ("select PEvalue,NameROM,LeftROM,RightROM,NormalROM from tblbpWrist where PatientIE_ID= " + lnk.CommandArgument + "");
        cm = new SqlCommand(query, cn);
        da = new SqlDataAdapter(cm);
        ds = new DataSet();
        da.Fill(ds);

        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["PEvalue"].ToString()))
            {
                wristPE = helper.getDocumentStringLeftRightPE(ds.Tables[0].Rows[0]["PEvalue"].ToString());
                wristPE = wristPE.Replace(",,", ",");
            }
            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["LeftROM"].ToString()))
            {
                string romstr = this.getROMString(ds.Tables[0].Rows[0]["NameROM"].ToString(), ds.Tables[0].Rows[0]["LeftROM"].ToString(), ds.Tables[0].Rows[0]["NormalROM"].ToString(), "left");
                if (!string.IsNullOrEmpty(romstr))
                    wristPE = wristPE + " ROM is as follows: " + romstr.TrimStart(';');
            }

            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["RightROM"].ToString()))
            {
                string romstr = this.getROMString(ds.Tables[0].Rows[0]["NameROM"].ToString(), ds.Tables[0].Rows[0]["RightROM"].ToString(), ds.Tables[0].Rows[0]["NormalROM"].ToString(), "right");
                if (!string.IsNullOrEmpty(romstr))
                    wristPE = wristPE + " ROM is as follows: " + romstr.TrimStart(';');


            }



            if (!string.IsNullOrEmpty(wristPE))
            {
                wristPE = wristPE.Replace("#leftwristtitle", "<b>LEFT WRIST EXAMINATION: </b>");
                wristPE = wristPE.Replace("#rightwristtitle", "<b>RIGHT WRIST EXAMINATION: </b>");
                wristPE = formatString(wristPE);
                str = str.Replace("#PEWrist", wristPE + "<br/><br/>");
            }
            else
            {
                str = str.Replace("#PEWrist", "");
                wristPE = wristPE.Replace("#leftwristtitle", "");
                wristPE = wristPE.Replace("#rightwristtitle", "");
            }
        }
        else
            str = str.Replace("#PEWrist", "");

        query = ("Select * from tblbpOtherPart WHERE PatientIE_ID=" + lnk.CommandArgument + "");
        cm = new SqlCommand(query, cn);
        da = new SqlDataAdapter(cm);
        ds = new DataSet();
        da.Fill(ds);

        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            str = str.Replace("#otherCC", !string.IsNullOrEmpty(ds.Tables[0].Rows[0]["OthersCC"].ToString()) ? ds.Tables[0].Rows[0]["OthersCC"].ToString() + "<br /><br />" : "");
            str = str.Replace("#otherPE", !string.IsNullOrEmpty(ds.Tables[0].Rows[0]["OthersPE"].ToString()) ? ds.Tables[0].Rows[0]["OthersPE"].ToString() + "<br /><br />" : "");
        }
        else
        {
            str = str.Replace("#otherCC", "");
            str = str.Replace("#otherPE", "");
        }


        //print sign

        //string path = "http://aeiuat.dynns.com:82/V3_Test/sign/21.jpg";
        str = str.Replace("#signsrc", "");



        string printStr = str;

        divPrint.InnerHtml = printStr;

        printStr = prstrCC + "\n" + prstrPE;


        createWordDocument(str, docname, lnk.CommandArgument, "");

        string folderPath = Server.MapPath("~/Reports/" + lnk.CommandArgument);

        // DownloadFiles(folderPath, "IE");

        savePrintRequest(lnk.CommandArgument, "0");

        BindPatientIEDetails();
        // ClientScript.RegisterStartupScript(this.GetType(), "Popup", "alert('Documents will be available for download after 5 min.')", true);
        //}
        //catch (Exception ex)
        //{
        //}
    }

    private void savePrintRequest(string PatientIEID = "0", string PatientFUID = "0")
    {
        DBHelperClass db = new DBHelperClass();
        string query = "";
        if (PatientFUID == "0")
            query = "delete from tblPrintRequestTime where PatientIE_Id=" + PatientIEID;
        else
            query = "delete from tblPrintRequestTime where PatientFU_Id=" + PatientFUID;
        db.executeQuery(query);

        query = "insert into tblPrintRequestTime values(" + PatientIEID + "," + PatientFUID + ",getdate())";

        db.executeQuery(query);
    }

    public bool downloadVisible(string PatientIEID = "0", string PatientFUID = "0")
    {
        DBHelperClass db = new DBHelperClass();
        string query = "";
        if (PatientIEID != "0")
            query = "select * from tblPrintRequestTime where PatientIE_Id=" + PatientIEID;
        else
            query = "select * from tblPrintRequestTime where PatientFU_Id=" + PatientFUID;

        DataSet ds = db.selectData(query);

        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            return true;
        }
        else
            return false;

    }

    public string getTPString(string sides, string sidesText)
    {
        string str = "";


        if (!string.IsNullOrEmpty(sides))
        {
            string[] val = sides.Split(',');
            string[] valText = sidesText.Split(',');

            for (int i = 0; i < val.Length; i++)
            {
                if (val[i] != "" && val[i] != null && valText[i] != "")
                {
                    str = str + ", " + val[i] + " " + valText[i].ToString();
                }
            }
        }

        return str;
    }

    public string getROMString(string nameROM, string valROM, string normalROM, string side = "", string IEFU = "IE", string bodypart = "")
    {
        string str = "";


        if (!string.IsNullOrEmpty(nameROM))
        {
            string[] nameText = nameROM.Split(',');
            string[] valText = valROM.Split(',');
            string[] normalText = normalROM.Split(',');

            for (int i = 0; i < valText.Length; i++)
            {
                if (valText[i] != "")
                {
                    if (bodypart == "Neck" || bodypart == "Midback" || bodypart == "Lowback")
                    {
                        if (IEFU == "IE")
                        {
                            if (string.IsNullOrEmpty(side))
                                str = str + " " + nameText[i] + " is " + valText[i] + " degrees, normal is " + normalText[i] + " degrees;";
                            else
                                str = str + " " + side + " " + nameText[i] + " is " + valText[i] + " degrees, normal is " + normalText[i] + " degrees;";
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(side))
                                str = str + " " + nameText[i] + " is " + valText[i] + " degrees;";
                            else
                                str = str + " " + side + " " + nameText[i] + " is " + valText[i] + " degrees;";
                        }
                    }
                    else
                    {
                        if (IEFU == "IE")
                        {
                            str = str + " " + nameText[i] + " is " + valText[i] + " degrees, normal is " + normalText[i] + " degrees;";
                        }
                        else
                        {

                            str = str + " " + nameText[i] + " is " + valText[i] + " degrees;";
                        }
                    }
                }
            }
        }

        if (!string.IsNullOrEmpty(str))
            return (str.TrimEnd(';') + ".").ToLower();
        else
            return str.ToLower();
    }

    public string printPage1(string patientIE_ID, string age = "", string doa = "", string location_id = "")
    {

        SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString);
        DBHelperClass db = new DBHelperClass();



        //page 1 printing string
        string query = ("select accidentHTML,historyHTML,historyHTMLValue from tblPage1HTMLContent where PatientIE_ID= " + patientIE_ID + "");
        SqlCommand cm = new SqlCommand(query, cn);
        SqlDataAdapter da = new SqlDataAdapter(cm);
        cn.Open();
        DataSet ds = new DataSet();
        da.Fill(ds);

        string str = "";
        Dictionary<string, string> page1 = new PrintDocumentHelper().getPage1String(ds.Tables[0].Rows[0]["accidentHTML"].ToString());
        // string page1 = new PrintDocumentHelper().getDocumentString(ds.Tables[0].Rows[0]["historyHTML"].ToString());



        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            // str = "The patient is a #age-year-old male who was the #restrained of a vehicle that was involved in a #involve collision on #doa. ";

            str = "On #doe, #name, a right-handed #age-year-old #sex #accident_desc which occurred on #doa. ";

            str = str.Replace("#age", age);
            str = str.Replace("#doa", doa);

            if (!string.IsNullOrEmpty(page1["txt_accident_desc"]))
                str = str.Replace("#accident_desc", page1["txt_accident_desc"]);
            else
                str = str.Replace("#accident_desc", "");

            if (!string.IsNullOrEmpty(page1["txt_belt"]))
                str = str.Replace("#restrained", page1["txt_belt"].ToString());

            if (!string.IsNullOrEmpty(page1["txt_invovledin"]))
                str = str.Replace("#involve", page1["txt_invovledin"].ToString());



            if (!string.IsNullOrEmpty(page1["txt_accident_desc_1"]))
                str = str + page1["txt_accident_desc_1"];




            //if (!string.IsNullOrEmpty(strBodypart))
            //    str = str + " #gender sustained multiple skeletal injuries including injury to " + strBodypart.TrimStart(',') + ". ";

            //if (page1.ContainsKey("txtWeek"))
            //{
            //    if (!string.IsNullOrEmpty(page1["txtWeek"]))
            //        str = str + "The patient has been undergoing PT/chiro for the past " + page1["txtWeek"] + ". ";
            //}


            //string fullname = ds.Tables[0].Rows[0]["Sex"].ToString() + " " + ds.Tables[0].Rows[0]["FirstName"].ToString() + " " + ds.Tables[0].Rows[0]["MiddleName"].ToString() + " " + ds.Tables[0].Rows[0]["LastName"].ToString();
            //string sex = ds.Tables[0].Rows[0]["Sex"].ToString() == "Mr." ? "male" : "female";
            //string gender = ds.Tables[0].Rows[0]["Sex"].ToString() == "Mr." ? "He" : "She";


            string location = "";
            query = ("select * from tblLocations where Location_ID=" + location_id);
            ds = db.selectData(query);

            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["Location"].ToString().ToLower().Contains("office"))
                    location = ds.Tables[0].Rows[0]["Location"].ToString();
                else
                    location = ds.Tables[0].Rows[0]["Location"].ToString() + " office.";
            }

            str = str + "The patient is seen at " + location.Replace(".", "") + ". ";

            if (page1.ContainsKey("txt_invovledin"))
            {
                if (!string.IsNullOrEmpty(page1["txt_invovledin"]))
                    str = str + " Patient's vehicle was hit on the " + page1["txt_invovledin"] + ". ";
            }

            //// str = "On " + CommonConvert.DateFormat(ds.Tables[0].Rows[0]["DOE"].ToString()) + ", " + fullname + ", a " + ds.Tables[0].Rows[0]["AGE"].ToString().Trim() + "-year-old " + sex + " " + page1["txt_accident_desc"] + " which occurred on the date of " + CommonConvert.DateFormat(ds.Tables[0].Rows[0]["DOA"].ToString()) + ". The patient was seen at the " + location + " ";
            //str = "On " + CommonConvert.DateFormat(ds.Tables[0].Rows[0]["DOE"].ToString()) + ", " + fullname + ", a " + ds.Tables[0].Rows[0]["AGE"].ToString().Trim() + "-year-old " + sex + " " + page1["txt_accident_desc"] + " which occurred on the date of " + CommonConvert.DateFormat(ds.Tables[0].Rows[0]["DOA"].ToString()) + ". ";


            //if (!string.IsNullOrEmpty(page1["txt_belt"]))
            //    str = str + "The patient states " + gender.ToLower() + " was the " + page1["txt_belt"] + " of a vehicle which was involved in " + page1["txt_invovledin"] + " collision. ";

            //if (page1.ContainsKey("txt_details"))
            //{
            //    if (!string.IsNullOrEmpty(page1["txt_details"]))
            //        str = str + page1["txt_details"] + " ";
            //}

            ////if (!string.IsNullOrEmpty(page1["txt_details"]))
            ////    str = str + page1["txt_details"];

            if (!string.IsNullOrEmpty(page1["txt_EMS"]))
                str = str + "The patient states that an EMS team " + page1["txt_EMS"] + ". ";



            if (page1["rdbhospyes"] == "true")
            {
                str = str + "#gender went to " + page1["txt_hospital"].Replace("hospital", "").Replace("Hospital", "").Replace("HOSPITAL", "") + " Hospital";

                str = str + " via " + page1["txt_via"];

                if (page1["rdbwhospno"] == "true")
                    str = str + " on the same day the accident occurred. ";
                else
                    str = str + (page1["txt_day"] == "1" ? "1 day" : page1["txt_day"] + " days") + " after the accident occurred.";


            }


            //if (page1["chk_mri"] == "true" || page1["chk_CT"] == "true" || page1["chk_xray"] == "true")
            //{
            //    str = str + "At the hospital, the patient had ";

            //    string strtemp = "";

            //    if (page1["chk_mri"] == "true" && !string.IsNullOrEmpty(page1["txt_mri"]))
            //        strtemp = strtemp + ", MRI of the " + page1["txt_mri"];
            //    if (page1["chk_CT"] == "true" && !string.IsNullOrEmpty(page1["txt_CT"]))
            //        strtemp = strtemp + ", CT of the " + page1["txt_CT"];
            //    if (page1["chk_xray"] == "true" && !string.IsNullOrEmpty(page1["txt_x_ray"]))
            //        strtemp = strtemp + ", X-ray of the" + page1["txt_x_ray"];

            //    if (!string.IsNullOrEmpty(strtemp))
            //        str = str + strtemp.TrimStart(',') + " done. ";

            //}

            //if (!string.IsNullOrEmpty(page1["txt_prescription"]))
            //    str = str + "At the hospital prescription was given for " + page1["txt_prescription"];


            //if (page1["chk_headinjury"] == "true")
            //{
            //    str = str + "The patient reports injury to the head and";
            //    if (page1["chk_headinjury"] != "true")
            //        str = str + " no loss of consciousness.";
            //    else
            //        str = str + " loss of consciousness for " + page1["txt_howlong"] + " " + page1["txthowlong"] + ". ";

            //}


            //if (page1["rdbdocyes"] == "true")
            //    str = str + "The patient visited " + page1["txt_docname"].ToString() + " since the incident. ";

            //if (page1["chkComplainingofHeadaches"] == "true")
            //{REFLEX EXAMINATION
            //    str = str + gender + " is complaining of headaches as a result of the accident. ";

            //    if (!string.IsNullOrEmpty(page1["txtPersistent"]))
            //        str = str + "The headaches started after the accident and are " + page1["txtPersistent"] + ". ";
            //}

            //if (page1["chkHeadechesAssociated"] == "true")
            //{
            //    str = str + " The headaches are associated with nausea and dizziness. ";
            //}

            //string strOp = "";

            //if (page1["chkfrontal"] == "true")
            //    strOp = "frontal";

            //if (page1["chkLeftParietal"] == "true")
            //    strOp = strOp + ", left Parietal";

            //if (page1["chkRightParietal"] == "true")
            //    strOp = strOp + ", right Parietal";

            //if (page1["chkLeftTemporal"] == "true")
            //    strOp = strOp + ", left Temporal";

            //if (page1["chkRightTemporal"] == "true")
            //    strOp = strOp + ", right Temporal";

            //if (page1["chkOccipital"] == "true")
            //    strOp = strOp + ", occipital";

            //if (page1["chkGlobal"] == "true")
            //    strOp = strOp + ", global";

            //if (!string.IsNullOrEmpty(strOp))
            //    str = str + "The headaches are " + strOp.TrimStart(',') + ". ";


            Int64 idId = Convert.ToInt64(patientIE_ID);
            StringBuilder strBodypart = new StringBuilder().Append(getBodyParts(idId));

            if (strBodypart.ToString().LastIndexOf(",") >= 0)
                strBodypart.Replace(",", " and ", strBodypart.ToString().LastIndexOf(","), 1);


            str = str.Trim() + " During the accident the patient reports injuries to " + strBodypart.ToString() + ".";



        }


        //query = ("select * from tblInjuredBodyParts where PatientIE_ID= " + patientIE_ID + "");
        //ds = db.selectData(query);

        //if (ds != null && ds.Tables[0].Rows.Count > 0)
        //{

        //    //str = str + "#accidentdetails ";
        //    // str = str + "#accidentdetails During the accident, the patient reports injuries to ";
        //    string strPresent = "The patient presents for an orthopedic evaluation of ";

        //    str = str + "My evaluation is limited to ";

        //    string strbodypart = "";

        //    if (ds.Tables[0].Rows[0]["Neck"].ToString() == "True")
        //        strbodypart = "neck";

        //    if (ds.Tables[0].Rows[0]["MidBack"].ToString() == "True")
        //        strbodypart = strbodypart + ", midback";

        //    if (ds.Tables[0].Rows[0]["LowBack"].ToString() == "True")
        //        strbodypart = strbodypart + ", lowback";

        //    if (ds.Tables[0].Rows[0]["LeftShoulder"].ToString() == "True" && ds.Tables[0].Rows[0]["RightShoulder"].ToString() == "True")
        //        strbodypart = strbodypart + ", bilateral shoulders";
        //    else
        //    {
        //        if (ds.Tables[0].Rows[0]["LeftShoulder"].ToString() == "True")
        //            strbodypart = strbodypart + ", left shoulder";
        //        else if (ds.Tables[0].Rows[0]["RightShoulder"].ToString() == "True")
        //            strbodypart = strbodypart + ", right shoulder";
        //    }

        //    if (ds.Tables[0].Rows[0]["LeftKnee"].ToString() == "True" && ds.Tables[0].Rows[0]["RightKnee"].ToString() == "True")
        //        strbodypart = strbodypart + ", bilateral knees";
        //    else
        //    {
        //        if (ds.Tables[0].Rows[0]["LeftKnee"].ToString() == "True")
        //            strbodypart = strbodypart + ", left knee";
        //        else if (ds.Tables[0].Rows[0]["RightKnee"].ToString() == "True")
        //            strbodypart = strbodypart + ", right knee";
        //    }

        //    if (ds.Tables[0].Rows[0]["LeftElbow"].ToString() == "True" && ds.Tables[0].Rows[0]["RightElbow"].ToString() == "True")
        //        strbodypart = strbodypart + ", bilateral elbows";
        //    else
        //    {
        //        if (ds.Tables[0].Rows[0]["LeftElbow"].ToString() == "True")
        //            strbodypart = strbodypart + ", left elbow";
        //        else if (ds.Tables[0].Rows[0]["RightElbow"].ToString() == "True")
        //            strbodypart = strbodypart + ", right elbow";
        //    }

        //    if (ds.Tables[0].Rows[0]["LeftWrist"].ToString() == "True" && ds.Tables[0].Rows[0]["RightWrist"].ToString() == "True")
        //        strbodypart = strbodypart + ", bilateral wrists";
        //    else
        //    {
        //        if (ds.Tables[0].Rows[0]["LeftWrist"].ToString() == "True")
        //            strbodypart = strbodypart + ", left wrist";
        //        else if (ds.Tables[0].Rows[0]["RightWrist"].ToString() == "True")
        //            strbodypart = strbodypart + ", right wrist";
        //    }

        //    if (ds.Tables[0].Rows[0]["LeftHip"].ToString() == "True" && ds.Tables[0].Rows[0]["RightHip"].ToString() == "True")
        //        strbodypart = strbodypart + ", bilateral hips";
        //    else
        //    {
        //        if (ds.Tables[0].Rows[0]["LeftHip"].ToString() == "True")
        //            strbodypart = strbodypart + ", left hip";
        //        else if (ds.Tables[0].Rows[0]["RightHip"].ToString() == "True")
        //            strbodypart = strbodypart + ", right hip";
        //    }

        //    if (ds.Tables[0].Rows[0]["LeftAnkle"].ToString() == "True" && ds.Tables[0].Rows[0]["RightAnkle"].ToString() == "True")
        //        strbodypart = strbodypart + ", bilateral ankles";
        //    else
        //    {
        //        if (ds.Tables[0].Rows[0]["LeftAnkle"].ToString() == "True")
        //            strbodypart = strbodypart + ", left ankle";
        //        else if (ds.Tables[0].Rows[0]["RightAnkle"].ToString() == "True")
        //            strbodypart = strbodypart + ", right ankle";
        //    }

        //    StringBuilder sb = new StringBuilder(strbodypart.TrimStart(','));
        //    if (sb.ToString().LastIndexOf(",") >= 0)
        //        sb.Replace(",", " and ", sb.ToString().LastIndexOf(","), 1);

        //    ViewState["generalText"] = "If the given history is correct, the injury to the " + sb.ToString() + " is related to the accident of ";

        //    str = str + sb.ToString() + ".";
        //    strPresent = strPresent + sb.ToString() + ".";
        //    ViewState["present"] = strPresent;

        //    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["Others"].ToString()))
        //    {
        //        if (ds.Tables[0].Rows[0]["Others"].ToString() != ",")
        //            str = str + " " + ds.Tables[0].Rows[0]["Others"].ToString().TrimEnd('.') + ".";
        //    }
        //}
        return str;
    }

    public void createWordDocument(string strHTML, string docname, string patientIE_ID = "", string patientFU_ID = "")
    {
        try
        {

            StringWriter sw = new StringWriter();

            HtmlTextWriter hw = new HtmlTextWriter(sw);

            System.Web.UI.HtmlControls.HtmlGenericControl createDiv =
   new System.Web.UI.HtmlControls.HtmlGenericControl("DIV");

            createDiv.InnerHtml = strHTML;

            string strFileName = docname + ".doc";

            createDiv.DataBind();
            createDiv.RenderControl(hw);

            string strPath = "";
            strPath = Server.MapPath("~/Reports");

            //if (!string.IsNullOrEmpty(patientIE_ID))
            //    strPath = Server.MapPath("~/Reports/" + patientIE_ID + "/print");
            //else
            //    strPath = Server.MapPath("~/Reports/" + patientFU_ID + "/print");

            if (Directory.Exists(strPath) == false)
                Directory.CreateDirectory(strPath);


            StreamWriter sWriter = new StreamWriter(strPath + "/" + strFileName);
            sWriter.Write(sw.ToString());
            sWriter.Close();

            downloadfile(strFileName);

            //HttpContext.Current.Response.Clear();
            //HttpContext.Current.Response.Charset = "";
            //HttpContext.Current.Response.ContentType = "application/msword";
            //string strFileName = docname + ".doc";
            //HttpContext.Current.Response.AddHeader("Content-Disposition", "inline;filename=" + strFileName);
            //StringBuilder strHTMLContent = new StringBuilder();
            //strHTMLContent.Append("<html><body>" + strHTML + "</body></html>");


            //HttpContext.Current.Response.Write(strHTMLContent);
            //HttpContext.Current.Response.End();
            //HttpContext.Current.Response.Flush();

            //HttpContext.Current.ApplicationInstance.CompleteRequest();
        }
        catch (Exception ex)
        {
        }

    }

    protected void lnkprintFU_Click(object sender, EventArgs e)
    {
        LinkButton lnkfu = sender as LinkButton;
        string val = lnkfu.CommandArgument;
        //try
        //{
        string PatientFU_ID = val.Split(',')[1];
        string PatientIE_ID = val.Split(',')[0];
        PrintDocumentHelper helper = new PrintDocumentHelper();

        String str = File.ReadAllText(Server.MapPath("~/Template/DocumentPrintFU.html"));

        string prstrCC = "", prstrPE = "", docname = "";

        LinkButton lnk = sender as LinkButton;
        SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString);
        DBHelperClass db = new DBHelperClass();


        //page1 printing
        string query = ("select * from View_PatientFU where PatientFU_ID= " + PatientFU_ID + "");
        DataSet ds = db.selectData(query);

        string IEDOE = CommonConvert.DateFormatPrint(ds.Tables[0].Rows[0]["IEDOE"].ToString());

        // docname = ds.Tables[0].Rows[0]["FirstName"].ToString() + ", " + ds.Tables[0].Rows[0]["LastName"].ToString() + "_" + PatientFU_ID + "_FU_" + CommonConvert.DateFormatPrint(ds.Tables[0].Rows[0]["DOE"].ToString()) + "_" + CommonConvert.DateFormatPrint(ds.Tables[0].Rows[0]["IEDOA"].ToString());
        docname = ds.Tables[0].Rows[0]["LastName"].ToString() + ", " + ds.Tables[0].Rows[0]["FirstName"].ToString() + "_" + PatientFU_ID + "_FU_" + CommonConvert.DateFormatPrint(ds.Tables[0].Rows[0]["DOE"].ToString()) + "_" + CommonConvert.DateFormatPrint(ds.Tables[0].Rows[0]["IEDOA"].ToString());

        string name = ds.Tables[0].Rows[0]["FirstName"].ToString() + " " + ds.Tables[0].Rows[0]["MiddleName"].ToString() + " " + ds.Tables[0].Rows[0]["LastName"].ToString();
        string gender = ds.Tables[0].Rows[0]["Sex"].ToString() == "Mr." ? "He" : "She";
        str = str.Replace("#patientname", name);
        str = str.Replace("#doe", CommonConvert.DateFormat(ds.Tables[0].Rows[0]["DOE"].ToString()));
        str = str.Replace("#1ed", CommonConvert.DateFormat(ds.Tables[0].Rows[0]["IEDOE"].ToString()));
        str = str.Replace("#doi", CommonConvert.DateFormat(ds.Tables[0].Rows[0]["IEDOA"].ToString()));

        this.printPTPreport(PatientIE_ID, ds.Tables[0].Rows[0]["FirstName"].ToString(), ds.Tables[0].Rows[0]["LastName"].ToString(),
        ds.Tables[0].Rows[0]["DOB"].ToString(),
        ds.Tables[0].Rows[0]["DOE"].ToString(),
         PatientFU_ID);

        //this.printCFreport(PatientIE_ID, ds.Tables[0].Rows[0]["FirstName"].ToString(), ds.Tables[0].Rows[0]["LastName"].ToString(),
        //ds.Tables[0].Rows[0]["DOB"].ToString(),
        //ds.Tables[0].Rows[0]["DOE"].ToString(),
        //    ds.Tables[0].Rows[0]["location"].ToString(), PatientFU_ID);

        //this.printPNreport(PatientIE_ID, ds.Tables[0].Rows[0]["FirstName"].ToString(), ds.Tables[0].Rows[0]["LastName"].ToString(),
        //   ds.Tables[0].Rows[0]["IEDOA"].ToString(),
        // ds.Tables[0].Rows[0]["DOE"].ToString(),
        //   ds.Tables[0].Rows[0]["location"].ToString(), ds.Tables[0].Rows[0]["DOB"].ToString(), PatientFU_ID);

        str = str.Replace("#history", "");

        //header printing

        query = ("select * from tblLocations where Location_ID=" + ds.Tables[0].Rows[0]["Location_Id"]);
        ds = db.selectData(query);

        String strheader = File.ReadAllText(Server.MapPath("~/Template/Header/Default.html"));


        //if (ds != null && ds.Tables[0].Rows.Count > 0)
        //{
        //    strheader = strheader.Replace("#headername", ds.Tables[0].Rows[0]["ContactPerson"].ToString());
        //    strheader = strheader.Replace("#address1", ds.Tables[0].Rows[0]["Address"].ToString());
        //    strheader = strheader.Replace("#address2", ds.Tables[0].Rows[0]["City"].ToString() + ", " + ds.Tables[0].Rows[0]["State"].ToString()) + " " + ds.Tables[0].Rows[0]["Zip"].ToString();
        //    strheader = strheader.Replace("#tel", "Tel #:" + ds.Tables[0].Rows[0]["Telephone"].ToString());
        //    // str = str.Replace("#fax", "FAX #:" + ds.Tables[0].Rows[0]["Telephone"].ToString());
        //}

        //str = str.Replace("#header", strheader);





        query = ("select topSectionHTML from tblPage1HTMLContent where PatientIE_ID= " + PatientIE_ID + "");
        ds = db.selectData(query);

        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            Dictionary<string, string> page1 = new PrintDocumentHelper().getPage1String(ds.Tables[0].Rows[0]["topSectionHTML"].ToString());

            str = str.Replace("#pmh", page1["PMH"]);
            str = str.Replace("#pshh", page1["PSH"]);
            str = str.Replace("#pastmedications", page1["Medication"]);
            str = str.Replace("#allergies", page1["Allergies"]);

        }

        query = ("select socialSectionHTML from tblPage1FUHTMLContent where PateintFU_ID= " + PatientFU_ID + "");
        ds = db.selectData(query);

        //if (ds != null && ds.Tables[0].Rows.Count > 0)
        //{
        //    string strstatus = new PrintDocumentHelper().getDocumentString(ds.Tables[0].Rows[0]["socialSectionHTML"].ToString());
        //    str = str.Replace("#socialhistory", "<b>SOCIAL HISTORY: </b>" + strstatus + "<br/>");
        //}
        //else
        //{
        //    str = str.Replace("#socialhistory", "");
        //}

        query = ("select accidentHTML from tblPage1HTMLContent where PatientIE_ID= " + PatientIE_ID + "");
        ds = db.selectData(query);



        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            string strDOD = "";
            Dictionary<string, string> page1_accident = new PrintDocumentHelper().getPage1String(ds.Tables[0].Rows[0]["accidentHTML"].ToString());

            if (page1_accident["rblPatial"] == "true")
                strDOD = "Partial";
            else if (page1_accident["rbl25"] == "true")
                strDOD = "25%";
            else if (page1_accident["rbl50"] == "true")
                strDOD = "50%";
            else if (page1_accident["rbl75"] == "true")
                strDOD = "75%";
            else if (page1_accident["rbl100"] == "true")
                strDOD = "100%";
            else if (page1_accident["rblNone"] == "true")
                strDOD = "None";

            if (!string.IsNullOrEmpty(strDOD))
                str = str.Replace("#dod", "<b>DEGREE OF DISABILITY: </b>" + strDOD);
            else
                str = str.Replace("#dod", "");

            //string work_status = "";

            //if (!string.IsNullOrEmpty(page1_accident["txt_work_status"]))
            //    work_status = work_status + page1_accident["txt_work_status"] + ". ";

            //if (!string.IsNullOrEmpty(page1_accident["txtMissed"]))
            //    work_status = work_status + gender + " has missed " + page1_accident["txtMissed"] + " of work after the accident. ";

            //if (!string.IsNullOrEmpty(page1_accident["txtReturnedToWork"]))
            //    work_status = work_status + page1_accident["txtReturnedToWork"] + ". ";



            //str = str.Replace("#work_status", work_status);

        }



        //ROS printing
        query = ("select * from tblPage2HTMLContent where PatientIE_ID= " + PatientIE_ID + "");
        ds = db.selectData(query);

        string strRos = "", strRosDenis = "";

        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["rosSectionHTML"].ToString()))
            {


                strRos = helper.getDocumentString(ds.Tables[0].Rows[0]["rosSectionHTML"].ToString());


                if (!string.IsNullOrEmpty(strRos))
                    strRos = "The patient admits to " + strRos + ". ";

                strRosDenis = helper.getDocumentStringDenies(ds.Tables[0].Rows[0]["rosSectionHTML"].ToString());
                if (!string.IsNullOrEmpty(strRosDenis))
                    strRosDenis = "The patient denies " + strRosDenis + ". ";
            }
        }
        str = str.Replace("#ROS", strRos + strRosDenis);

        query = "select * from tblPage1FUHTMLContent where PateintFU_ID=" + PatientFU_ID;
        ds = db.selectData(query);

        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            string strDOD = "", strRestriction = "", strWorkStatus = "", strActivity = "";

            Dictionary<string, string> page2 = new PrintDocumentHelper().getPage1String(ds.Tables[0].Rows[0]["degreeSectionHTML"].ToString());



            if (page2["chkhousework"] == "true")
                strActivity = strActivity + ", housework";
            if (page2["chkwork-related"] == "true")
                strActivity = strActivity + ", job work-related duties";
            if (page2["chkdriving"] == "true")
                strActivity = strActivity + ", driving";
            if (page2["chksittingincar"] == "true")
                strActivity = strActivity + ", sitting in car";
            if (page2["chkwalking"] == "true")
                strActivity = strActivity + ", walking up/down downstairs";

            StringBuilder sb = new StringBuilder(strActivity.TrimStart(','));

            if (sb.ToString().LastIndexOf(",") >= 0)
                sb.Replace(",", " and ", sb.ToString().LastIndexOf(","), 1);


            if (!string.IsNullOrEmpty(sb.ToString()))
                str = str.Replace("#activity", "<b>ACTIVITIES OF DAILY LIVING AFFECTED: </b>" + sb.ToString());
            else
                str = str.Replace("#activity", "");



            if (page2["chkBending"] == "true")
                strRestriction = "Bending";
            if (page2["chkClimbing"] == "true")
                strRestriction = strRestriction + ", Climbing stairs/ladders";
            if (page2["chkEnvironmental"] == "true")
                strRestriction = strRestriction + ", Environmental conditions";
            if (page2["chkKneeling"] == "true")
                strRestriction = strRestriction + ", Kneeling";
            if (page2["chkLifting"] == "true")
                strRestriction = strRestriction + ", Lifting";
            if (page2["chkOperatingHeavy"] == "true")
                strRestriction = strRestriction + ", Operating heavy equipment";
            if (page2["chkOperatingofmotor"] == "true")
                strRestriction = strRestriction + ", Operation of motor vehicles";
            if (page2["chkPersonal"] == "true")
                strRestriction = strRestriction + ", Personal protective equipment";
            if (page2["chkSitting"] == "true")
                strRestriction = strRestriction + ", Sitting";
            if (page2["chkStanding"] == "true")
                strRestriction = strRestriction + ", Standing, ";
            if (page2["chkUseofPublic"] == "true")
                strRestriction = strRestriction + ", Use of public transportation";
            if (page2["chkUseofUpper"] == "true")
                strRestriction = strRestriction + ", Use of upper extremities";

            sb = new StringBuilder(strRestriction.TrimStart(','));

            if (sb.ToString().LastIndexOf(",") >= 0)
                sb.Replace(",", " and ", sb.ToString().LastIndexOf(","), 1);




            if (!string.IsNullOrEmpty(sb.ToString()))
                str = str.Replace("#restriction", "<b>RESTRICTION: </b>" + sb.ToString());
            else
                str = str.Replace("#restriction", "");

            if (page2["chkAbletoWork"] == "true")
                strWorkStatus = "Able to go back to work " + page2["txtbackwork"];
            if (page2["chkWorking"] == "true")
                strWorkStatus = strWorkStatus + ", Working " + page2["txtWorking"];
            if (page2["chkNotWorking"] == "true")
                strWorkStatus = strWorkStatus + ", Not Working " + page2["txtNotWorking"];
            if (page2["chkPartiallyWorking"] == "true")
                strWorkStatus = strWorkStatus + ", Partially Working " + page2["txtPartiallyWorking"];

            sb = new StringBuilder(strWorkStatus.TrimStart(','));

            if (sb.ToString().LastIndexOf(",") >= 0)
                sb.Replace(",", " and ", sb.ToString().LastIndexOf(","), 1);

            if (!string.IsNullOrEmpty(strWorkStatus))
                str = str.Replace("#workstatus", "<b>WORK STATUS: </b>" + sb.ToString());
            else
                str = str.Replace("#workstatus", "");

        }

        //plan printing 

        string strPlan = "";

        strPlan = strPlan + this.getPOCFU("Neck", PatientIE_ID, PatientFU_ID);
        strPlan = strPlan + (string.IsNullOrEmpty(this.getPlanFU("tblFUbpNeck", PatientFU_ID)) == false ? "<br />" + this.getPlanFU("tblFUbpNeck", PatientFU_ID) : "");

        strPlan = strPlan + this.getPOCFU("MidBack", PatientIE_ID, PatientFU_ID);
        strPlan = strPlan + (string.IsNullOrEmpty(this.getPlanFU("tblFUbpMidback", PatientFU_ID)) == false ? "<br />" + this.getPlanFU("tblFUbpMidback", PatientFU_ID) : "");

        strPlan = strPlan + this.getPOCFU("LowBack", PatientIE_ID, PatientFU_ID);
        strPlan = strPlan + (string.IsNullOrEmpty(this.getPlanFU("tblFUbpLowback", PatientFU_ID)) == false ? "<br />" + this.getPlanFU("tblFUbpLowback", PatientFU_ID) : "");

        strPlan = strPlan + this.getPOCFU("Shoulder", PatientIE_ID, PatientFU_ID);
        strPlan = strPlan + (string.IsNullOrEmpty(this.getPlanFU("tblFUbpShoulder", PatientFU_ID)) == false ? "<br />" + this.getPlanFU("tblFUbpShoulder", PatientFU_ID) : "");

        strPlan = strPlan + this.getPOCFU("Knee", PatientIE_ID, PatientFU_ID);
        strPlan = strPlan + (string.IsNullOrEmpty(this.getPlanFU("tblFUbpKnee", PatientFU_ID)) == false ? "<br />" + this.getPlanFU("tblFUbpKnee", PatientFU_ID) : "");

        strPlan = strPlan + this.getPOCFU("Elbow", PatientIE_ID, PatientFU_ID);
        strPlan = strPlan + (string.IsNullOrEmpty(this.getPlanFU("tblFUbpElbow", PatientFU_ID)) == false ? "<br />" + this.getPlanFU("tblFUbpElbow", PatientFU_ID) : "");

        strPlan = strPlan + this.getPOCFU("Wrist", PatientIE_ID, PatientFU_ID);
        strPlan = strPlan + (string.IsNullOrEmpty(this.getPlanFU("tblFUbpWrist", PatientFU_ID)) == false ? "<br />" + this.getPlanFU("tblFUbpWrist", PatientFU_ID) : "");

        strPlan = strPlan + this.getPOCFU("Hip", PatientIE_ID, PatientFU_ID);
        strPlan = strPlan + (string.IsNullOrEmpty(this.getPlanFU("tblFUbpHip", PatientFU_ID)) == false ? "<br />" + this.getPlanFU("tblFUbpHip", PatientFU_ID) : "");

        strPlan = strPlan + this.getPOCFU("Ankle", PatientIE_ID, PatientFU_ID);
        strPlan = strPlan + (string.IsNullOrEmpty(this.getPlanFU("tblFUbpAnkle", PatientFU_ID)) == false ? "<br />" + this.getPlanFU("tblFUbpAnkle", PatientFU_ID) : "");

        strPlan = strPlan + this.getPOCFU("OtherPart", PatientIE_ID, PatientFU_ID);
        strPlan = strPlan + (string.IsNullOrEmpty(this.getPlanFU("tblFUbpOtherPart", PatientFU_ID)) == false ? "<br />" + this.getPlanFU("tblFUbpOtherPart", PatientFU_ID) : "");


        str = str.Replace("#plan", string.IsNullOrEmpty(strPlan) ? strPlan : "<b>PLAN:</b><br/>" + strPlan);


        //page4 printing
        query = "Select * from tblFUPatientFUDetailPage1 WHERE PatientFU_ID=" + PatientFU_ID;
        ds = db.selectData(query);

        string strDaignosis = "", strshoulderrightmri = "", strshoulderleftmri = "", strkneeleftmri = "", strkneerightmri = "";

        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {


            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagCervialBulgeDate"].ToString()))
            {
                strDaignosis = Convert.ToDateTime(ds.Tables[0].Rows[0]["DiagCervialBulgeDate"].ToString()).ToString("MM/dd/yyyy") + " - ";

                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagCervialBulgeStudy"].ToString()))
                    strDaignosis = strDaignosis + " " + ds.Tables[0].Rows[0]["DiagCervialBulgeStudy"].ToString() + " of the ";

                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagCervialBulgeText"].ToString()))
                    strDaignosis = strDaignosis + " Cervical spine bulge at " + ds.Tables[0].Rows[0]["DiagCervialBulgeText"].ToString() + ", ";

                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagCervialBulgeHNP1"].ToString()))
                    strDaignosis = strDaignosis + " HNP at " + ds.Tables[0].Rows[0]["DiagCervialBulgeHNP1"].ToString() + ". ";


                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagCervialBulgeHNP2"].ToString()))
                    strDaignosis = strDaignosis + ds.Tables[0].Rows[0]["DiagCervialBulgeHNP2"].ToString() + ". ";

            }

            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagThoracicBulgeDate"].ToString()))
            {
                strDaignosis = strDaignosis + "<br/>" + Convert.ToDateTime(ds.Tables[0].Rows[0]["DiagThoracicBulgeDate"].ToString()).ToString("MM/dd/yyyy") + " - ";

                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagThoracicBulgeStudy"].ToString()))
                    strDaignosis = strDaignosis + " " + ds.Tables[0].Rows[0]["DiagThoracicBulgeStudy"].ToString() + " of the ";

                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagThoracicBulgeText"].ToString()))
                    strDaignosis = strDaignosis + " Thoracic spine bulge at " + ds.Tables[0].Rows[0]["DiagThoracicBulgeText"].ToString() + ", ";

                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagThoracicBulgeHNP1"].ToString()))
                    strDaignosis = strDaignosis + " HNP at " + ds.Tables[0].Rows[0]["DiagThoracicBulgeHNP1"].ToString() + ". ";

                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagThoracicBulgeHNP2"].ToString()))
                    strDaignosis = strDaignosis + ds.Tables[0].Rows[0]["DiagThoracicBulgeHNP2"].ToString() + ". ";

            }

            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagLumberBulgeDate"].ToString()))
            {
                strDaignosis = strDaignosis + "<br/>" + Convert.ToDateTime(ds.Tables[0].Rows[0]["DiagLumberBulgeDate"].ToString()).ToString("MM/dd/yyyy") + " - ";

                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagLumberBulgeStudy"].ToString()))
                    strDaignosis = strDaignosis + " " + ds.Tables[0].Rows[0]["DiagLumberBulgeStudy"].ToString() + " of the ";

                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagLumberBulgeText"].ToString()))
                    strDaignosis = strDaignosis + " Lumber spine bulge at " + ds.Tables[0].Rows[0]["DiagLumberBulgeText"].ToString() + ", ";

                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagLumberBulgeHNP1"].ToString()))
                    strDaignosis = strDaignosis + " HNP at " + ds.Tables[0].Rows[0]["DiagLumberBulgeHNP1"].ToString() + ". ";

                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagLumberBulgeHNP2"].ToString()))
                    strDaignosis = strDaignosis + ds.Tables[0].Rows[0]["DiagLumberBulgeHNP2"].ToString() + ". ";

            }

            //if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagLeftShoulderDate"].ToString()))
            //{
            //    strDaignosis = strDaignosis + "<br/>" + Convert.ToDateTime(ds.Tables[0].Rows[0]["DiagLeftShoulderDate"].ToString()).ToString("MM/dd/yyyy") + " - ";

            //    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagLeftShoulderStudy"].ToString()))
            //        strDaignosis = strDaignosis + " " + ds.Tables[0].Rows[0]["DiagLeftShoulderStudy"].ToString() + " of the ";

            //    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagLeftShoulderText"].ToString()))
            //        strDaignosis = strDaignosis + " left shoulder reveals  " + ds.Tables[0].Rows[0]["DiagLeftShoulderText"].ToString() + ". ";



            //}

            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagLeftShoulderDate"].ToString()))
            {
                //strshoulderleftmri = Convert.ToDateTime(ds.Tables[0].Rows[0]["DiagLeftShoulderDate"].ToString()).ToString("MM/dd/yyyy") + " - ";

                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagLeftShoulderStudy"].ToString()))
                    strshoulderleftmri = "<b>" + ds.Tables[0].Rows[0]["DiagLeftShoulderStudy"].ToString() + " of the left shoulder:</b> ";

                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagLeftShoulderText"].ToString()))
                    strshoulderleftmri = strshoulderleftmri + ds.Tables[0].Rows[0]["DiagLeftShoulderText"].ToString() + ". ";
            }



            //if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagRightShoulderDate"].ToString()))
            //{
            //    strDaignosis = strDaignosis + "<br/>" + Convert.ToDateTime(ds.Tables[0].Rows[0]["DiagRightShoulderDate"].ToString()).ToString("MM/dd/yyyy") + " - ";

            //    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagRightShoulderStudy"].ToString()))
            //        strDaignosis = strDaignosis + " " + ds.Tables[0].Rows[0]["DiagRightShoulderStudy"].ToString() + " of the ";

            //    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagRightShoulderText"].ToString()))
            //        strDaignosis = strDaignosis + " right shoulder reveals " + ds.Tables[0].Rows[0]["DiagRightShoulderText"].ToString() + ". ";

            //}

            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagRightShoulderDate"].ToString()))
            {
                //strshoulderrightmri = Convert.ToDateTime(ds.Tables[0].Rows[0]["DiagRightShoulderDate"].ToString()).ToString("MM/dd/yyyy") + " - ";

                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagRightShoulderStudy"].ToString()))
                    strshoulderrightmri = "<b>" + ds.Tables[0].Rows[0]["DiagRightShoulderStudy"].ToString() + " of the right shoulder:</b> ";

                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagRightShoulderText"].ToString()))
                    strshoulderrightmri = strshoulderrightmri + ds.Tables[0].Rows[0]["DiagRightShoulderText"].ToString() + ". ";

            }

            //if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagLeftKneeDate"].ToString()))
            //{
            //    strDaignosis = strDaignosis + "<br/>" + Convert.ToDateTime(ds.Tables[0].Rows[0]["DiagLeftKneeDate"].ToString()).ToString("MM/dd/yyyy") + " - ";

            //    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagLeftKneeStudy"].ToString()))
            //        strDaignosis = strDaignosis + " " + ds.Tables[0].Rows[0]["DiagLeftKneeStudy"].ToString() + " of the ";

            //    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagLeftKneeText"].ToString()))
            //        strDaignosis = strDaignosis + " left knee reveals bruises " + ds.Tables[0].Rows[0]["DiagLeftKneeText"].ToString() + ". ";

            //}

            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagLeftKneeDate"].ToString()))
            {
                // strkneeleftmri = Convert.ToDateTime(ds.Tables[0].Rows[0]["DiagLeftKneeDate"].ToString()).ToString("MM/dd/yyyy") + " - ";

                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagLeftKneeStudy"].ToString()))
                    strkneeleftmri = "<b>" + ds.Tables[0].Rows[0]["DiagLeftKneeStudy"].ToString() + " of the left knee:</b> ";

                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagLeftKneeText"].ToString()))
                    strkneeleftmri = strkneeleftmri + ds.Tables[0].Rows[0]["DiagLeftKneeText"].ToString() + ". ";

            }

            //if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagRightKneeDate"].ToString()))
            //{
            //    strDaignosis = strDaignosis + "<br/>" + Convert.ToDateTime(ds.Tables[0].Rows[0]["DiagRightKneeDate"].ToString()).ToString("MM/dd/yyyy") + " - ";

            //    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagRightKneeStudy"].ToString()))
            //        strDaignosis = strDaignosis + " " + ds.Tables[0].Rows[0]["DiagRightKneeStudy"].ToString() + " of the ";

            //    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagRightKneeText"].ToString()))
            //        strDaignosis = strDaignosis + " right knee reveals sprain " + ds.Tables[0].Rows[0]["DiagRightKneeText"].ToString() + ". ";



            //}
            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagRightKneeDate"].ToString()))
            {
                //strkneerightmri = Convert.ToDateTime(ds.Tables[0].Rows[0]["DiagRightKneeDate"].ToString()).ToString("MM/dd/yyyy") + " - ";

                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagRightKneeStudy"].ToString()))
                    strkneerightmri = "<b>" + ds.Tables[0].Rows[0]["DiagRightKneeStudy"].ToString() + " of the right knee:</b> ";

                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagRightKneeText"].ToString()))
                    strkneerightmri = strkneerightmri + ds.Tables[0].Rows[0]["DiagRightKneeText"].ToString() + ". ";



            }


            str = str.Replace("#diagnostic", strDaignosis);
            str = str.Replace("#follow-up", ds.Tables[0].Rows[0]["FollowUpIn"].ToString().Trim());

            //query = "Select * from tblFUMedicationRx WHERE PatientFUid_ID=" + PatientFU_ID;
            //ds = db.selectData(query);

            //if (ds != null && ds.Tables[0].Rows.Count > 0)
            //{
            //    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            //    {
            //        strMedi = strMedi + ds.Tables[0].Rows[0]["Medicine"].ToString() + "<br/>";
            //    }
            //}

            //str = str.Replace("#medications", strMedi);


            strDaignosis = "";
            query = "Select * from tblDiagCodesDetail WHERE PatientFU_ID=" + PatientFU_ID;
            ds = db.selectData(query);

            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    strDaignosis = strDaignosis + "<br/>" + ds.Tables[0].Rows[i]["Description"].ToString();
                }
            }

            str = str.Replace("#diagnoses", strDaignosis);

        }
        else
        {
            str = str.Replace("#diagnostic", "");
        }



        //neck printing string
        query = ("select CCvalue from tblFUbpNeck where PatientFU_ID= " + PatientFU_ID + "");
        SqlCommand cm = new SqlCommand(query, cn);
        SqlDataAdapter da = new SqlDataAdapter(cm);
        cn.Open();
        ds = new DataSet();
        da.Fill(ds);


        string neckCC = "", neckTP = "", lowbackCC = "", shoudlerCC = "", kneeCC = "", elbowCC = "", wristCC = "", hipCC = "", ankleCC = "";



        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["CCvalue"].ToString()))
            {
                neckCC = helper.getDocumentString(ds.Tables[0].Rows[0]["CCvalue"].ToString());
                str = str.Replace("#neck", neckCC + "<br/><br/>");
            }
            else
            {
                str = str.Replace("#neck", "");

            }
        }
        else
        {
            str = str.Replace("#neck", "");

        }

        //neck PE printing string
        query = ("select PEvalue,PESides,PESidesText,NameROM,LeftROM,RightROM,NormalROM,CNameROM,CROM,CNormalROM from tblFUbpNeck where PatientFU_ID= " + PatientFU_ID + "");
        string neckPE = "";
        cm = new SqlCommand(query, cn);
        da = new SqlDataAdapter(cm);
        ds = new DataSet();
        da.Fill(ds);

        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["PEvalue"].ToString()))
            {
                neckPE = helper.getDocumentString(ds.Tables[0].Rows[0]["PEvalue"].ToString());
                neckTP = this.getTPString(ds.Tables[0].Rows[0]["PESides"].ToString(), ds.Tables[0].Rows[0]["PESidesText"].ToString());
            }

            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["NameROM"].ToString()))
            {
                string romstrleft = this.getROMString(ds.Tables[0].Rows[0]["NameROM"].ToString(), ds.Tables[0].Rows[0]["LeftROM"].ToString(), ds.Tables[0].Rows[0]["NormalROM"].ToString(), "left", "FU");

                string romstrright = this.getROMString(ds.Tables[0].Rows[0]["NameROM"].ToString(), ds.Tables[0].Rows[0]["RightROM"].ToString(), ds.Tables[0].Rows[0]["NormalROM"].ToString(), "right", "FU");
                string romstrC = this.getROMString(ds.Tables[0].Rows[0]["CNameROM"].ToString(), ds.Tables[0].Rows[0]["CROM"].ToString(), ds.Tables[0].Rows[0]["CNormalROM"].ToString(), "FU");
                string romstr = romstrleft.TrimStart(';') + " " + romstrright.TrimStart(';');

                if (!string.IsNullOrEmpty(romstrC))
                    neckPE = neckPE + "<br/>ROM is as follows:" + romstrC.TrimStart(';') + ". ";

                if (!string.IsNullOrEmpty(romstr))
                {
                    if (string.IsNullOrEmpty(romstrC))
                        neckPE = neckPE + "<br/>ROM is as follows: " + romstr + ". ";
                    else
                        neckPE = neckPE + romstr + ".";
                }


                if (!string.IsNullOrEmpty(neckTP))
                    neckPE = neckPE + " There are palpable taut bands/trigger points at " + neckTP.TrimStart(',') + " with referral to the scapula. ";

            }


            if (!string.IsNullOrEmpty(neckPE))
            {
                str = str.Replace("#PENeck", "<b>Cervical Spine Examination: </b>" + neckPE + "<br/><br/>");
            }
            else
                str = str.Replace("#PENeck", "");

        }
        else
            str = str.Replace("#PENeck", neckPE);


        //lowback printing string
        query = ("select CCvalue from tblFUbpLowback where PatientFU_ID= " + PatientFU_ID + "");
        cm = new SqlCommand(query, cn);
        da = new SqlDataAdapter(cm);
        ds = new DataSet();
        da.Fill(ds);

        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["CCvalue"].ToString()))
            {
                lowbackCC = helper.getDocumentString(ds.Tables[0].Rows[0]["CCvalue"].ToString());
                str = str.Replace("#lowback", lowbackCC + "<br/><br/>");

            }
            else
                str = str.Replace("#lowback", lowbackCC);
        }
        else
            str = str.Replace("#lowback", lowbackCC);


        //lowback PE printing string
        query = ("select PEvalue,PESides,PESidesText,NameROM,LeftROM,RightROM,NormalROM,CNameROM,CROM,CNormalROM,NameTest,LeftTest,RightTest,TextVal  from tblFUbpLowback where PatientFU_ID= " + PatientFU_ID + "");
        string lowbackPE = "", lowbackTP = "";
        cm = new SqlCommand(query, cn);
        da = new SqlDataAdapter(cm);
        ds = new DataSet();
        da.Fill(ds);

        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["PEvalue"].ToString()))
            {
                lowbackPE = helper.getDocumentString(ds.Tables[0].Rows[0]["PEvalue"].ToString());
                lowbackTP = this.getTPString(ds.Tables[0].Rows[0]["PESides"].ToString(), ds.Tables[0].Rows[0]["PESidesText"].ToString());


            }

            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["NameROM"].ToString()))
            {
                string romstrleft = this.getROMString(ds.Tables[0].Rows[0]["NameROM"].ToString(), ds.Tables[0].Rows[0]["LeftROM"].ToString(), ds.Tables[0].Rows[0]["NormalROM"].ToString(), "left", "FU");
                string romstrright = this.getROMString(ds.Tables[0].Rows[0]["NameROM"].ToString(), ds.Tables[0].Rows[0]["RightROM"].ToString(), ds.Tables[0].Rows[0]["NormalROM"].ToString(), "right", "FU");
                string romstrC = this.getROMString(ds.Tables[0].Rows[0]["CNameROM"].ToString(), ds.Tables[0].Rows[0]["CROM"].ToString(), ds.Tables[0].Rows[0]["CNormalROM"].ToString(), "FU");
                string romstr = romstrleft.TrimStart(';') + " " + romstrright.TrimStart(';');



                if (!string.IsNullOrEmpty(romstrC))
                    lowbackPE = lowbackPE + "<br/>ROM is as follows:" + romstrC.TrimStart(';') + ". ";

                if (!string.IsNullOrEmpty(romstr))
                {
                    if (string.IsNullOrEmpty(romstrC))
                        lowbackPE = lowbackPE + "<br/>ROM is as follows: " + romstr + ". ";
                    else
                        lowbackPE = lowbackPE + romstr + ". ";
                }

            }

            if (!string.IsNullOrEmpty(neckTP))
                lowbackPE = lowbackPE + "There are palpable taut bands/trigger points at " + lowbackTP.TrimStart(',') + " with referral patterns laterally to the region in a fan-like pattern";


            //get test string

            string strTest = helper.getLowbackTestString(ds.Tables[0].Rows[0]["NameTest"].ToString(), ds.Tables[0].Rows[0]["LeftTest"].ToString(), ds.Tables[0].Rows[0]["RightTest"].ToString(), ds.Tables[0].Rows[0]["TextVal"].ToString());

            if (!string.IsNullOrEmpty(strTest))
                lowbackPE = lowbackPE + "." + strTest.TrimStart(',') + ". ";

            if (!string.IsNullOrEmpty(lowbackPE))
            {
                str = str.Replace("#PELowback", "<b>Lumbar Spine Examination: </b>" + lowbackPE + "<br/><br/>");

            }
            else
                str = str.Replace("#PELowback", "");

        }
        else
            str = str.Replace("#PELowback", lowbackPE);

        //midback printing string
        string midbackCC = "";
        query = ("select CCvalue from tblFUbpMidback where PatientFU_ID= " + PatientFU_ID + "");
        cm = new SqlCommand(query, cn);
        da = new SqlDataAdapter(cm);
        ds = new DataSet();
        da.Fill(ds);

        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["CCvalue"].ToString()))
            {
                midbackCC = helper.getDocumentString(ds.Tables[0].Rows[0]["CCvalue"].ToString());
                str = str.Replace("#midback", midbackCC + "<br/><br/>");

            }
            else
                str = str.Replace("#midback", midbackCC);
        }
        else
            str = str.Replace("#midback", midbackCC);

        //midback PE printing string
        string midbackPE = "", midbackTP = "";
        query = ("select PEvalue,PESides,PESidesText from tblFUbpMidback where PatientFU_ID= " + PatientFU_ID + "");
        cm = new SqlCommand(query, cn);
        da = new SqlDataAdapter(cm);
        ds = new DataSet();
        da.Fill(ds);

        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["PEvalue"].ToString()))
            {
                midbackPE = helper.getDocumentString(ds.Tables[0].Rows[0]["PEvalue"].ToString());
                midbackTP = this.getTPString(ds.Tables[0].Rows[0]["PESides"].ToString(), ds.Tables[0].Rows[0]["PESidesText"].ToString());
                if (!string.IsNullOrEmpty(midbackTP))
                    midbackPE = midbackPE + "There are palpable taut bands/trigger points at " + midbackTP.TrimStart(',') + "  with referral to the scapula.";
                str = str.Replace("#PEMidback", "<b>Thoracic Spine Examination: </b>" + midbackPE + "<br/><br/>");
                prstrPE = prstrPE + midbackPE + "\n\n";
            }
            else
                str = str.Replace("#PEMidback", midbackPE);
        }
        else
            str = str.Replace("#PEMidback", midbackPE);

        //shoulder printing string
        query = ("select CCvalue from tblFUbpShoulder where PatientFU_ID= " + PatientFU_ID + "");
        cm = new SqlCommand(query, cn);
        da = new SqlDataAdapter(cm);
        ds = new DataSet();
        da.Fill(ds);

        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["CCvalue"].ToString()))
            {
                shoudlerCC = helper.getDocumentStringLeftRight(ds.Tables[0].Rows[0]["CCvalue"].ToString(), "Shoulder");
                str = str.Replace("#shoulder", shoudlerCC + "<br/><br/>");

            }
            else
                str = str.Replace("#shoulder", shoudlerCC);
        }
        else
            str = str.Replace("#shoulder", shoudlerCC);

        //shoulder PE printing string
        query = ("select PEvalue,NameROM,LeftROM,RightROM,NormalROM,PESides,PESidesText from tblFUbpshoulder where PatientFU_ID= " + PatientFU_ID + "");
        string shoulderPE = "", shoulderTP = "";
        cm = new SqlCommand(query, cn);
        da = new SqlDataAdapter(cm);
        ds = new DataSet();
        da.Fill(ds);

        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["PEvalue"].ToString()))
            {
                shoulderPE = helper.getDocumentStringLeftRightPE(ds.Tables[0].Rows[0]["PEvalue"].ToString());
                shoulderTP = this.getTPString(ds.Tables[0].Rows[0]["PESides"].ToString(), ds.Tables[0].Rows[0]["PESidesText"].ToString());

            }

            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["LeftROM"].ToString()))
            {
                string romstr = this.getROMString(ds.Tables[0].Rows[0]["NameROM"].ToString(), ds.Tables[0].Rows[0]["LeftROM"].ToString(), ds.Tables[0].Rows[0]["NormalROM"].ToString(), "left", "FU");
                if (!string.IsNullOrEmpty(romstr))
                    shoulderPE = shoulderPE + " ROM is as follows: " + romstr;
            }

            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["RightROM"].ToString()))
            {
                string romstr = this.getROMString(ds.Tables[0].Rows[0]["NameROM"].ToString(), ds.Tables[0].Rows[0]["RightROM"].ToString(), ds.Tables[0].Rows[0]["NormalROM"].ToString(), "right", "FU");
                if (!string.IsNullOrEmpty(romstr))
                    shoulderPE = shoulderPE + " ROM is as follows: " + romstr;
            }

            if (!string.IsNullOrEmpty(shoulderTP))
                shoulderPE = shoulderPE + "There are palpable taut bands/trigger points at " + shoulderTP.TrimStart(',') + " with referral to the scapula. ";


            if (!string.IsNullOrEmpty(shoulderPE))
            {
                shoulderPE = shoulderPE.Replace("#rightshouldertitle", "<b>RIGHT SHOULDER :</b>");
                shoulderPE = shoulderPE.Replace("#shoulderrightrom", "");
                shoulderPE = shoulderPE.Replace("#shoulderrightmri", "<br/><br/>" + strshoulderrightmri);

                shoulderPE = shoulderPE.Replace("#leftshouldertitle", "<b>LEFT SHOULDER :</b>");
                shoulderPE = shoulderPE.Replace("#shoulderleftrom", "");
                shoulderPE = shoulderPE.Replace("#shoulderleftmri", "<br/><br/>" + strshoulderleftmri);

                str = str.Replace("#PEShoudler", shoulderPE + "<br/><br/>");


            }
            else
                str = str.Replace("#PEShoudler", "");

        }
        else
            str = str.Replace("#PEShoudler", shoulderPE);

        //knee printing string
        query = ("select CCvalue from tblFUbpKnee where PatientFU_ID= " + PatientFU_ID + "");
        cm = new SqlCommand(query, cn);
        da = new SqlDataAdapter(cm);
        ds = new DataSet();
        da.Fill(ds);

        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["CCvalue"].ToString()))
            {
                kneeCC = helper.getDocumentStringLeftRight(ds.Tables[0].Rows[0]["CCvalue"].ToString(), "Knee");
                str = str.Replace("#knee", kneeCC + "<br/><br/>");

            }
            else
                str = str.Replace("#knee", kneeCC);
        }
        else
            str = str.Replace("#knee", kneeCC);

        //knee PE printing string
        query = ("select PEvalue,NameROM,LeftROM,RightROM,NormalROM from tblFUbpKnee where PatientFU_ID= " + PatientFU_ID + "");
        string kneePE = "";
        cm = new SqlCommand(query, cn);
        da = new SqlDataAdapter(cm);
        ds = new DataSet();
        da.Fill(ds);

        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["PEvalue"].ToString()))
            {
                kneePE = helper.getDocumentStringLeftRightPE(ds.Tables[0].Rows[0]["PEvalue"].ToString(), "Knee");

            }

            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["LeftROM"].ToString()))
            {
                string romstr = this.getROMString(ds.Tables[0].Rows[0]["NameROM"].ToString(), ds.Tables[0].Rows[0]["LeftROM"].ToString(), ds.Tables[0].Rows[0]["NormalROM"].ToString(), "left", "FU");
                if (!string.IsNullOrEmpty(romstr))
                    kneePE = kneePE + " ROM is as follows: " + romstr;
            }

            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["RightROM"].ToString()))
            {
                string romstr = this.getROMString(ds.Tables[0].Rows[0]["NameROM"].ToString(), ds.Tables[0].Rows[0]["RightROM"].ToString(), ds.Tables[0].Rows[0]["NormalROM"].ToString(), "right", "FU");
                if (!string.IsNullOrEmpty(romstr))
                    kneePE = kneePE + " ROM is as follows: " + romstr;
            }

            if (!string.IsNullOrEmpty(kneePE))
            {

                kneePE = kneePE.Replace("#rightkneetitle", "");
                kneePE = kneePE.Replace("#kneerightrom", "");
                kneePE = kneePE.Replace("#kneerightmri", "<br/><br/>" + strkneerightmri);

                kneePE = kneePE.Replace("#leftkneetitle", "");
                kneePE = kneePE.Replace("#kneeleftrom", "");
                kneePE = kneePE.Replace("#kneeleftmri", "<br/><br/>" + strkneeleftmri);

                str = str.Replace("#PEKnee", kneePE + "<br/><br/>");

            }
            else
                str = str.Replace("#PEKnee", "");

        }
        else
            str = str.Replace("#PEKnee", kneePE);

        //elbow printing string
        query = ("select CCvalue from tblFUbpElbow where PatientFU_ID= " + PatientFU_ID + "");
        cm = new SqlCommand(query, cn);
        da = new SqlDataAdapter(cm);
        ds = new DataSet();
        da.Fill(ds);

        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["CCvalue"].ToString()))
            {
                elbowCC = helper.getDocumentStringLeftRight(ds.Tables[0].Rows[0]["CCvalue"].ToString(), "Elbow");
                str = str.Replace("#elbow", elbowCC + "<br/><br/>");

            }
            else
                str = str.Replace("#elbow", elbowCC);
        }
        else
            str = str.Replace("#elbow", elbowCC);

        //elbow PE printing string
        string elbowPE = "";
        query = ("select  PEvalue,NameROM,LeftROM,RightROM,NormalROM  from tblFUbpElbow where PatientFU_ID= " + PatientFU_ID + "");
        cm = new SqlCommand(query, cn);
        da = new SqlDataAdapter(cm);
        ds = new DataSet();
        da.Fill(ds);

        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["PEvalue"].ToString()))
            {
                elbowPE = helper.getDocumentStringLeftRightPE(ds.Tables[0].Rows[0]["PEvalue"].ToString(), "Elbow");
            }

            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["LeftROM"].ToString()))
            {
                string romstr = this.getROMString(ds.Tables[0].Rows[0]["NameROM"].ToString(), ds.Tables[0].Rows[0]["LeftROM"].ToString(), ds.Tables[0].Rows[0]["NormalROM"].ToString(), "left", "FU");
                if (!string.IsNullOrEmpty(romstr))
                    elbowPE = elbowPE + " ROM is as follows: " + romstr;
            }

            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["RightROM"].ToString()))
            {
                string romstr = this.getROMString(ds.Tables[0].Rows[0]["NameROM"].ToString(), ds.Tables[0].Rows[0]["RightROM"].ToString(), ds.Tables[0].Rows[0]["NormalROM"].ToString(), "right", "FU");
                if (!string.IsNullOrEmpty(romstr))
                    elbowPE = elbowPE + " ROM is as follows: " + romstr;
            }

            if (!string.IsNullOrEmpty(elbowPE))
            {

                str = str.Replace("#PEElbow", elbowPE + "<br/><br/>");
            }
            else
                str = str.Replace("#PEElbow", "");

        }
        else
            str = str.Replace("#PEElbow", elbowPE);

        //wrist printing string
        query = ("select CCvalue from tblFUbpWrist where PatientFU_ID= " + PatientFU_ID + "");
        cm = new SqlCommand(query, cn);
        da = new SqlDataAdapter(cm);
        ds = new DataSet();
        da.Fill(ds);

        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["CCvalue"].ToString()))
            {
                wristCC = helper.getDocumentStringLeftRight(ds.Tables[0].Rows[0]["CCvalue"].ToString(), "Wrist");
                str = str.Replace("#wrist", wristCC + "<br/><br/>");

            }
            else
                str = str.Replace("#wrist", wristCC);
        }
        else
            str = str.Replace("#wrist", wristCC);

        //hip printing string
        query = ("select CCvalue from tblFUbpHip where PatientFU_ID= " + PatientFU_ID + "");
        cm = new SqlCommand(query, cn);
        da = new SqlDataAdapter(cm);
        ds = new DataSet();
        da.Fill(ds);

        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["CCvalue"].ToString()))
            {
                hipCC = helper.getDocumentStringLeftRight(ds.Tables[0].Rows[0]["CCvalue"].ToString(), "Hip");
                str = str.Replace("#hip", hipCC + "<br/><br/>");

            }
            else
                str = str.Replace("#hip", hipCC);
        }
        else
            str = str.Replace("#hip", hipCC);

        //hip PE printing string
        string hipPE = "";
        query = ("select PEvalue,NameROM,LeftROM,RightROM,NormalROM from tblFUbpHip where PatientFU_ID= " + PatientFU_ID + "");
        cm = new SqlCommand(query, cn);
        da = new SqlDataAdapter(cm);
        ds = new DataSet();
        da.Fill(ds);

        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["PEvalue"].ToString()))
            {
                hipPE = helper.getDocumentStringLeftRightPE(ds.Tables[0].Rows[0]["PEvalue"].ToString(), "Hip");
            }

            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["LeftROM"].ToString()))
            {
                string romstr = this.getROMString(ds.Tables[0].Rows[0]["NameROM"].ToString(), ds.Tables[0].Rows[0]["LeftROM"].ToString(), ds.Tables[0].Rows[0]["NormalROM"].ToString(), "left", "FU");
                if (!string.IsNullOrEmpty(romstr))
                    hipPE = hipPE + " ROM is as follows: " + romstr;
            }

            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["RightROM"].ToString()))
            {
                string romstr = this.getROMString(ds.Tables[0].Rows[0]["NameROM"].ToString(), ds.Tables[0].Rows[0]["RightROM"].ToString(), ds.Tables[0].Rows[0]["NormalROM"].ToString(), "right", "FU");
                if (!string.IsNullOrEmpty(romstr))
                    hipPE = hipPE + " ROM is as follows: " + romstr;
            }

            if (!string.IsNullOrEmpty(hipPE))
            {
                str = str.Replace("#PEHip", hipPE + "<br/><br/>");

            }
            else
                str = str.Replace("#PEHip", "");
        }
        else
            str = str.Replace("#PEHip", "");


        //ankle printing string
        query = ("select CCvalue from tblFUbpAnkle where PatientFU_ID=" + PatientFU_ID + "");
        cm = new SqlCommand(query, cn);
        da = new SqlDataAdapter(cm);
        ds = new DataSet();
        da.Fill(ds);

        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["CCvalue"].ToString()))
            {
                ankleCC = helper.getDocumentStringLeftRight(ds.Tables[0].Rows[0]["CCvalue"].ToString(), "Ankle");
                str = str.Replace("#ankle", ankleCC + "<br/><br/>");

            }
            else
                str = str.Replace("#ankle", ankleCC);
        }
        else
            str = str.Replace("#ankle", ankleCC);


        //ankle PE printing string
        string anklePE = "";
        query = ("select PEvalue,NameROM,LeftROM,RightROM,NormalROM from tblFUbpAnkle where PatientFU_ID= " + PatientFU_ID + "");
        cm = new SqlCommand(query, cn);
        da = new SqlDataAdapter(cm);
        ds = new DataSet();
        da.Fill(ds);

        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["PEvalue"].ToString()))
            {
                anklePE = helper.getDocumentStringLeftRightPE(ds.Tables[0].Rows[0]["PEvalue"].ToString(), "Ankle");

            }

            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["LeftROM"].ToString()))
            {
                string romstr = this.getROMString(ds.Tables[0].Rows[0]["NameROM"].ToString(), ds.Tables[0].Rows[0]["LeftROM"].ToString(), ds.Tables[0].Rows[0]["NormalROM"].ToString(), "left", "FU");
                if (!string.IsNullOrEmpty(romstr))
                    anklePE = anklePE + " ROM is as follows: " + romstr;
            }

            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["RightROM"].ToString()))
            {
                string romstr = this.getROMString(ds.Tables[0].Rows[0]["NameROM"].ToString(), ds.Tables[0].Rows[0]["RightROM"].ToString(), ds.Tables[0].Rows[0]["NormalROM"].ToString(), "right", "FU");
                if (!string.IsNullOrEmpty(romstr))
                    anklePE = anklePE + " ROM is as follows: " + romstr;
            }

            if (!string.IsNullOrEmpty(anklePE))
            {
                str = str.Replace("#PEAnkle", anklePE + "<br/><br/>");

            }
            else
                str = str.Replace("#PEAnkle", "");

        }
        else
            str = str.Replace("#PEAnkle", anklePE);



        //wrist PE printing string
        string wristPE = "";
        query = ("select PEvalue,NameROM,LeftROM,RightROM,NormalROM from tblFUbpWrist where PatientFU_ID= " + PatientFU_ID + "");
        cm = new SqlCommand(query, cn);
        da = new SqlDataAdapter(cm);
        ds = new DataSet();
        da.Fill(ds);

        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["PEvalue"].ToString()))
            {
                wristPE = helper.getDocumentStringLeftRightPE(ds.Tables[0].Rows[0]["PEvalue"].ToString(), "Wrist");
            }
            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["LeftROM"].ToString()))
            {
                string romstr = this.getROMString(ds.Tables[0].Rows[0]["NameROM"].ToString(), ds.Tables[0].Rows[0]["LeftROM"].ToString(), ds.Tables[0].Rows[0]["NormalROM"].ToString(), "left", "FU");
                if (!string.IsNullOrEmpty(romstr))
                    wristPE = wristPE + " ROM is as follows: " + romstr.TrimStart(';');
            }

            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["RightROM"].ToString()))
            {
                string romstr = this.getROMString(ds.Tables[0].Rows[0]["NameROM"].ToString(), ds.Tables[0].Rows[0]["RightROM"].ToString(), ds.Tables[0].Rows[0]["NormalROM"].ToString(), "right", "FU");
                if (!string.IsNullOrEmpty(romstr))
                    wristPE = wristPE + " ROM is as follows: " + romstr.TrimStart(';');
            }

            if (!string.IsNullOrEmpty(wristPE))
            {
                str = str.Replace("#PEWrist", wristPE + "<br/><br/>");
            }
            else
                str = str.Replace("#PEWrist", "");
        }
        else
            str = str.Replace("#PEWrist", "");


        //page3 printing
        //query = ("select * from tblPage3FUHTMLContent where PatientFU_ID=" + PatientFU_ID + "");
        //ds = db.selectData(query);


        //if (ds != null && ds.Tables[0].Rows.Count > 0)
        //{
        //    Dictionary<string, string> page3 = new PrintDocumentHelper().getPage1String(ds.Tables[0].Rows[0]["topSectionHTML"].ToString());

        //    //string strGAIT = page3["txtGAIT"] + ".";
        //    string strGAIT ="";

        //    if (!string.IsNullOrEmpty(page3["txtAmbulates"]))
        //    {
        //        strGAIT = strGAIT + page3["txtAmbulates"].ToString();

        //        if (page3["chkFootslap"] == "true")
        //            strGAIT = strGAIT + ", foot slap/drop";
        //        if (page3["chkKneehyperextension"] == "true")
        //            strGAIT = strGAIT + ", knee hyperextension";
        //        if (page3["chkUnabletohealwalk"] == "true")
        //            strGAIT = strGAIT + ", unable to heel walk";
        //        if (page3["chkUnabletotoewalk"] == "true") ;
        //        strGAIT = strGAIT + ", unable to toe walk";
        //        if (!string.IsNullOrEmpty(page3["txtOther"]))
        //            strGAIT = strGAIT + " and " + page3["txtOther"];
        //    }

        //    if (!string.IsNullOrEmpty(strGAIT))
        //        str = str.Replace("#gait", strGAIT + ".<br/><br/>");
        //    else
        //        str = str.Replace("#gait", "");


        //    Dictionary<string, string> page3_1 = new PrintDocumentHelper().getPage1String(ds.Tables[0].Rows[0]["HTMLContent"].ToString());

        //    string strNR = "The patient is alert and cooperative and responding appropriately. Cranial nerves - II-XII are grossly intact ";

        //    if (!string.IsNullOrEmpty(page3_1["txtIntactExcept"]))
        //        strNR = strNR + "except " + page3_1["txtIntactExcept"];

        //    str = str.Replace("#nerologicalexam", !string.IsNullOrEmpty(strNR) ? strNR + ". " : "");

        //    string strUpperExtremity = "", strLowerExtremity = "", strExceptions = "";
        //    if (!string.IsNullOrEmpty(page3_1["LTricepstxt"]))
        //        strUpperExtremity = "left triceps " + page3_1["LTricepstxt"];
        //    if (!string.IsNullOrEmpty(page3_1["RTricepstxt"]))
        //        strUpperExtremity = strUpperExtremity + ", " + "right triceps " + page3_1["RTricepstxt"];
        //    if (!string.IsNullOrEmpty(page3_1["LBicepstxt"]))
        //        strUpperExtremity = strUpperExtremity + ", " + "left biceps " + page3_1["LBicepstxt"];
        //    if (!string.IsNullOrEmpty(page3_1["RBicepstxt"]))
        //        strUpperExtremity = strUpperExtremity + ", " + "right biceps " + page3_1["RBicepstxt"];
        //    if (!string.IsNullOrEmpty(page3_1["LBrachioradialis"]))
        //        strUpperExtremity = strUpperExtremity + ", " + "left brachioradialis " + page3_1["LBrachioradialis"];
        //    if (!string.IsNullOrEmpty(page3_1["RBrachioradialis"]))
        //        strUpperExtremity = strUpperExtremity + ", " + "right brachioradialis " + page3_1["RBrachioradialis"];



        //    if (!string.IsNullOrEmpty(page3_1["LKnee"]))
        //        strLowerExtremity = "left knee " + page3_1["LKnee"];
        //    if (!string.IsNullOrEmpty(page3_1["RKnee"]))
        //        strLowerExtremity = strLowerExtremity + ", " + "right knee " + page3_1["RKnee"];
        //    if (!string.IsNullOrEmpty(page3_1["LAnkle"]))
        //        strLowerExtremity = strLowerExtremity + ", " + "left ankle " + page3_1["LAnkle"];
        //    if (!string.IsNullOrEmpty(page3_1["RAnkle"]))
        //        strLowerExtremity = strLowerExtremity + ", " + "right ankle " + page3_1["RAnkle"];


        //    strExceptions = strUpperExtremity + ", " + strLowerExtremity;


        //    if (!string.IsNullOrEmpty(strExceptions))
        //        strExceptions = "Deep tendon reflexes are 2+ and equal with the following exceptions: " + strExceptions;
        //    else
        //        strExceptions = "Deep tendon reflexes are 2+ and equal.";



        //    str = str.Replace("#reflexexam", !string.IsNullOrEmpty(strExceptions) ? strExceptions : "");

        //    string strRE = "", strRElist = "";

        //    if (page3_1["chkPinPrick"] == "true")
        //        strRElist = "pinprick";

        //    if (page3_1["chkLighttouch"] == "true")
        //        strRElist = strRElist + ", " + "light touch.";

        //    if (!string.IsNullOrEmpty(strRElist))
        //        strRElist = "Is checked by " + strRElist.TrimStart(',');


        //    if (!string.IsNullOrEmpty(page3_1["txtSensory"]))
        //        strRElist = strRElist + " It is " + page3_1["txtSensory"];

        //    strRE = strRElist;

        //    strUpperExtremity = ""; strLowerExtremity = ""; strExceptions = "";
        //    if (!string.IsNullOrEmpty(page3_1["LLateralarm"]))
        //        strUpperExtremity = page3_1["LLateralarm"] + " at left lateral arm (C5)";
        //    if (!string.IsNullOrEmpty(page3_1["RLateralarm"]))
        //        strUpperExtremity = page3_1["RLateralarm"] + " at right lateral arm (C5)";

        //    if (!string.IsNullOrEmpty(page3_1["LLateralforearm"]))
        //        strUpperExtremity = strUpperExtremity + ", " + page3_1["LLateralforearm"] + " at left lateral forearm, thumb, index (C6)";
        //    if (!string.IsNullOrEmpty(page3_1["RLateralforearm"]))
        //        strUpperExtremity = strUpperExtremity + ", " + page3_1["RLateralforearm"] + " at right lateral forearm, thumb, index (C6)";

        //    if (!string.IsNullOrEmpty(page3_1["LMiddlefinger"]))
        //        strUpperExtremity = strUpperExtremity + ", " + page3_1["LMiddlefinger"] + " at left middle finger (C7)";
        //    if (!string.IsNullOrEmpty(page3_1["RMiddlefinger"]))
        //        strUpperExtremity = strUpperExtremity + ", " + page3_1["RMiddlefinger"] + " at right middle finger (C7)";

        //    if (!string.IsNullOrEmpty(page3_1["LMidialForearm"]))
        //        strUpperExtremity = strUpperExtremity + ", " + page3_1["LMidialForearm"] + " at left medial forearm, ring, little finger (C8)";
        //    if (!string.IsNullOrEmpty(page3_1["RMidialForearm"]))
        //        strUpperExtremity = strUpperExtremity + ", " + page3_1["RMidialForearm"] + " at right medial forearm, ring, little finger (C8)";

        //    if (!string.IsNullOrEmpty(page3_1["LMidialarm"]))
        //        strUpperExtremity = strUpperExtremity + ", " + page3_1["LMidialarm"] + " at left medial arm (T1)";
        //    if (!string.IsNullOrEmpty(page3_1["RMidialarm"]))
        //        strUpperExtremity = strUpperExtremity + ", " + page3_1["RMidialarm"] + " at right medial arm (T1)";

        //    if (!string.IsNullOrEmpty(page3_1["LCervical"]))
        //        strUpperExtremity = strUpperExtremity + ", " + page3_1["LCervical"] + " at left cervical paraspinals";
        //    if (!string.IsNullOrEmpty(page3_1["RCervical"]))
        //        strUpperExtremity = strUpperExtremity + ", " + page3_1["RCervical"] + " at right cervical paraspinals";

        //    if (!string.IsNullOrEmpty(page3_1["LtxtDMTL3"]))
        //        strLowerExtremity = page3_1["LtxtDMTL3"] + " at left distal medial thigh (L3)";
        //    if (!string.IsNullOrEmpty(page3_1["RtxtDMTL3"]))
        //        strLowerExtremity = page3_1["RtxtDMTL3"] + " at right distal medial thigh (L3)";

        //    if (!string.IsNullOrEmpty(page3_1["LtxtMLFL4"]))
        //        strLowerExtremity = strLowerExtremity + ", " + page3_1["LtxtMLFL4"] + " at left medial left foot (L4)";
        //    if (!string.IsNullOrEmpty(page3_1["RtxtMLFL4"]))
        //        strLowerExtremity = strLowerExtremity + ", " + page3_1["RtxtMLFL4"] + " at right medial left foot (L4)";

        //    if (!string.IsNullOrEmpty(page3_1["LtxtDOFL5"]))
        //        strLowerExtremity = strLowerExtremity + ", " + page3_1["LtxtDOFL5"] + " at left dorsum of the foot (L5)";
        //    if (!string.IsNullOrEmpty(page3_1["RtxtDOFL5"]))
        //        strLowerExtremity = strLowerExtremity + ", " + page3_1["RtxtDOFL5"] + " at right dorsum of the foot (L5)";

        //    if (!string.IsNullOrEmpty(page3_1["LtxtLTS1"]))
        //        strLowerExtremity = strLowerExtremity + ", " + page3_1["LtxtLTS1"] + " at left lateral foot (S1)";
        //    if (!string.IsNullOrEmpty(page3_1["RtxtLTS1"]))
        //        strLowerExtremity = strLowerExtremity + ", " + page3_1["RtxtLTS1"] + " at right lateral foot (S1)";

        //    if (!string.IsNullOrEmpty(page3_1["LtxtLP"]))
        //        strLowerExtremity = strLowerExtremity + ", " + page3_1["LtxtLP"] + " at left lumbar paraspinals";
        //    if (!string.IsNullOrEmpty(page3_1["RtxtLP"]))
        //        strLowerExtremity = strLowerExtremity + ", " + page3_1["RtxtLP"] + " at right lumbar paraspinals";

        //    strExceptions = strUpperExtremity + "," + strLowerExtremity;


        //    string senexam = strRE + strExceptions;

        //    if (!string.IsNullOrEmpty(senexam))
        //        str = str.Replace("#sensoryexam", "Is checked by light touch with the following exceptions: " + senexam);
        //    else
        //        str = str.Replace("#sensoryexam", senexam);




        //    strUpperExtremity = ""; strLowerExtremity = ""; strExceptions = "";
        //    if (!string.IsNullOrEmpty(page3_1["LAbduction"]))
        //        strUpperExtremity = "left shoulder abduction " + page3_1["LAbduction"];
        //    if (!string.IsNullOrEmpty(page3_1["RAbduction"]))
        //        strUpperExtremity = strUpperExtremity + ", " + "right shoulder abduction  " + page3_1["RAbduction"];

        //    if (!string.IsNullOrEmpty(page3_1["LElbowExtension"]))
        //        strUpperExtremity = strUpperExtremity + ", " + "left shoulder flexion " + page3_1["LElbowExtension"];
        //    if (!string.IsNullOrEmpty(page3_1["RElbowExtension"]))
        //        strUpperExtremity = strUpperExtremity + ", " + "right shoulder flexion " + page3_1["RElbowExtension"];


        //    if (!string.IsNullOrEmpty(page3_1["LElbowFlexion"]))
        //        strUpperExtremity = strUpperExtremity + ", " + "left elbow extension " + page3_1["LElbowFlexion"];
        //    if (!string.IsNullOrEmpty(page3_1["RElbowFlexion"]))
        //        strUpperExtremity = strUpperExtremity + ", " + "right elbow extension " + page3_1["RElbowFlexion"];

        //    if (!string.IsNullOrEmpty(page3_1["LElbowFlexion"]))
        //        strUpperExtremity = strUpperExtremity + ", " + "left elbow flexion " + page3_1["LElbowFlexion"];
        //    if (!string.IsNullOrEmpty(page3_1["RElbowFlexion"]))
        //        strUpperExtremity = strUpperExtremity + ", " + "right elbow flexion " + page3_1["RElbowFlexion"];

        //    if (!string.IsNullOrEmpty(page3_1["LSupination"]))
        //        strUpperExtremity = strUpperExtremity + ", " + "left elbow supination " + page3_1["LSupination"];
        //    if (!string.IsNullOrEmpty(page3_1["RSupination"]))
        //        strUpperExtremity = strUpperExtremity + ", " + "right elbow supination " + page3_1["RSupination"];


        //    if (!string.IsNullOrEmpty(page3_1["LPronation"]))
        //        strUpperExtremity = strUpperExtremity + ", " + "left elbow pronation " + page3_1["LPronation"];
        //    if (!string.IsNullOrEmpty(page3_1["RPronation"]))
        //        strUpperExtremity = strUpperExtremity + ", " + "right elbow pronation " + page3_1["RPronation"];


        //    if (!string.IsNullOrEmpty(page3_1["LWristFlexion"]))
        //        strUpperExtremity = strUpperExtremity + ", " + "left wrist flexion " + page3_1["LWristFlexion"];
        //    if (!string.IsNullOrEmpty(page3_1["RWristFlexion"]))
        //        strUpperExtremity = strUpperExtremity + ", " + "right wrist flexion " + page3_1["RWristFlexion"];

        //    if (!string.IsNullOrEmpty(page3_1["LWristExtension"]))
        //        strUpperExtremity = strUpperExtremity + ", " + "left wrist extension " + page3_1["LWristExtension"];
        //    if (!string.IsNullOrEmpty(page3_1["RWristExtension"]))
        //        strUpperExtremity = strUpperExtremity + ", " + "right wrist extension " + page3_1["RWristExtension"];


        //    if (!string.IsNullOrEmpty(page3_1["LGrip"]))
        //        strUpperExtremity = strUpperExtremity + ", " + "left hand grip strength " + page3_1["LGrip"];
        //    if (!string.IsNullOrEmpty(page3_1["RGrip"]))
        //        strUpperExtremity = strUpperExtremity + ", " + "right hand grip strength " + page3_1["RGrip"];

        //    if (!string.IsNullOrEmpty(page3_1["LFinger"]))
        //        strUpperExtremity = strUpperExtremity + ", " + "left hand finger abduction	 " + page3_1["LFinger"];
        //    if (!string.IsNullOrEmpty(page3_1["RFinger"]))
        //        strUpperExtremity = strUpperExtremity + ", " + "right hand finger abduction	 " + page3_1["RFinger"];

        //    if (!string.IsNullOrEmpty(page3_1["LHipFlexion"]))
        //        strLowerExtremity = "left hip flexion " + page3_1["LHipFlexion"];
        //    if (!string.IsNullOrEmpty(page3_1["RFinger"]))
        //        strLowerExtremity = strLowerExtremity + ", " + "right hip flexion " + page3_1["RFinger"];

        //    if (!string.IsNullOrEmpty(page3_1["LHipAbduction"]))
        //        strLowerExtremity = strLowerExtremity + ", " + "left hip abduction " + page3_1["LHipAbduction"];
        //    if (!string.IsNullOrEmpty(page3_1["RHipAbduction"]))
        //        strLowerExtremity = strLowerExtremity + ", " + "right hip abduction " + page3_1["RHipAbduction"];

        //    if (!string.IsNullOrEmpty(page3_1["LKneeExtension"]))
        //        strLowerExtremity = strLowerExtremity + ", " + "left knee extension " + page3_1["LKneeExtension"];
        //    if (!string.IsNullOrEmpty(page3_1["RKneeExtension"]))
        //        strLowerExtremity = strLowerExtremity + ", " + "right knee extension " + page3_1["RKneeExtension"];

        //    if (!string.IsNullOrEmpty(page3_1["LKneeFlexion"]))
        //        strLowerExtremity = strLowerExtremity + ", " + "left knee flexion " + page3_1["LKneeFlexion"];
        //    if (!string.IsNullOrEmpty(page3_1["RKneeFlexion"]))
        //        strLowerExtremity = strLowerExtremity + ", " + "right knee flexion " + page3_1["RKneeFlexion"];

        //    if (!string.IsNullOrEmpty(page3_1["LDorsiflexion"]))
        //        strLowerExtremity = strLowerExtremity + ", " + "left ankle dorsiflexion " + page3_1["LDorsiflexion"];
        //    if (!string.IsNullOrEmpty(page3_1["RDorsiflexion"]))
        //        strLowerExtremity = strLowerExtremity + ", " + "right ankle dorsiflexion " + page3_1["RDorsiflexion"];

        //    if (!string.IsNullOrEmpty(page3_1["LPlantar"]))
        //        strLowerExtremity = strLowerExtremity + ", " + "left ankle plantar flexion " + page3_1["LPlantar"];
        //    if (!string.IsNullOrEmpty(page3_1["RPlantar"]))
        //        strLowerExtremity = strLowerExtremity + ", " + "right ankle plantar flexion " + page3_1["RPlantar"];

        //    if (!string.IsNullOrEmpty(page3_1["LExtensor"]))
        //        strLowerExtremity = strLowerExtremity + ", " + "left ankle extensor hallucis longus " + page3_1["LExtensor"];
        //    if (!string.IsNullOrEmpty(page3_1["RExtensor"]))
        //        strLowerExtremity = strLowerExtremity + ", " + "right ankle extensor hallucis longus " + page3_1["RExtensor"];

        //    strExceptions = strUpperExtremity + "," + strLowerExtremity;

        //    if (!string.IsNullOrEmpty(strExceptions))
        //        strExceptions = "Testing is 5/5 normal with the following exceptions: " + strExceptions + ".";

        //    str = str.Replace("#motorexam", strExceptions);
        //}
        //else
        //{
        //    str = str.Replace("#motorexam", "");
        //    str = str.Replace("#sensoryexam", "");
        //    str = str.Replace("#reflexexam", "");
        //    str = str.Replace("#nerologicalexam", "");
        //    str = str.Replace("#gait", "");
        //}




        query = ("Select* from tblFUbpOtherPart WHERE PatientFU_ID=" + PatientFU_ID + "");
        cm = new SqlCommand(query, cn);
        da = new SqlDataAdapter(cm);
        ds = new DataSet();
        da.Fill(ds);

        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            str = str.Replace("#otherCC", !string.IsNullOrEmpty(ds.Tables[0].Rows[0]["OthersCC"].ToString()) ? ds.Tables[0].Rows[0]["OthersCC"].ToString() : "");
            str = str.Replace("#otherPE", !string.IsNullOrEmpty(ds.Tables[0].Rows[0]["OthersCC"].ToString()) ? ds.Tables[0].Rows[0]["OthersPE"].ToString() : "");

        }
        else
        {
            str = str.Replace("#otherCC", "");
            str = str.Replace("#otherPE", "");
        }

        bool isDisplayCC = displayCCinPrint();

        if (isDisplayCC)
        {
            str = str.Replace("#display", "block");
        }
        else
        {
            str = str.Replace("#display", "none");
        }

        string printStr = str;

        divPrint.InnerHtml = printStr;

        createWordDocument(str, docname, PatientFU_ID, "");


        savePrintRequest("0", PatientFU_ID);

        // string folderPath = Server.MapPath("~/Reports/" + PatientFU_ID);

        // DownloadFiles(folderPath, "FU");

    }

    private string getDiagnosis(string bodypart, string PatientIE_ID)
    {
        DBHelperClass db = new DBHelperClass();
        string query = "Select * from tblDiagCodesDetail WHERE PatientIE_ID = " + PatientIE_ID + " and PatientFU_ID is null AND BodyPart LIKE '%" + bodypart + "%' Order By BodyPart, Description";

        DataSet dsDaigCode = db.selectData(query);

        string strDaignosis = "";
        if (dsDaigCode != null && dsDaigCode.Tables[0].Rows.Count > 0)
        {
            //strDaignosis = strDaignosis + "<br/><br/><b>" + bodypart + ":</b>";
            for (int i = 0; i < dsDaigCode.Tables[0].Rows.Count; i++)
            {
                //strDaignosis = strDaignosis + "<br/>" + dsDaigCode.Tables[0].Rows[i]["Description"].ToString() + " - " + dsDaigCode.Tables[0].Rows[i]["DiagCode"].ToString();
                strDaignosis = strDaignosis + "<br/>" + dsDaigCode.Tables[0].Rows[i]["Description"].ToString();
            }
        }

        if (bodypart != "Other")
        {
            query = "select FreeFormA from tblbp" + bodypart + " where PatientIE_ID = " + PatientIE_ID;
        }
        else
        {
            query = "select OthersA from tblbpOtherPart where PatientIE_ID = " + PatientIE_ID;
        }
        dsDaigCode = null;
        dsDaigCode = db.selectData(query);
        if (dsDaigCode != null && dsDaigCode.Tables[0].Rows.Count > 0)
        {
            if (!string.IsNullOrEmpty(dsDaigCode.Tables[0].Rows[0][0].ToString()))
            {
                strDaignosis = strDaignosis + "<br/>" + dsDaigCode.Tables[0].Rows[0][0].ToString();
            }
        }


        return strDaignosis;
    }

    private string getDiagnosisRightLeft(string PatientIE_ID)
    {
        DBHelperClass db = new DBHelperClass();

        SqlParameter[] parameters = new SqlParameter[1];
        parameters[0] = new SqlParameter("@PatientIE_ID", PatientIE_ID);

        DataSet dsDaigCode = db.executeSelectSP("nusp_getDiagnosis", parameters);

        string strDaignosis = "";
        if (dsDaigCode != null && dsDaigCode.Tables[0].Rows.Count > 0)
        {
            //strDaignosis = strDaignosis + "<br/><br/><b>" + bodypart + ":</b>";
            for (int i = 0; i < dsDaigCode.Tables[0].Rows.Count; i++)
            {
                //strDaignosis = strDaignosis + "<br/>" + dsDaigCode.Tables[0].Rows[i]["Description"].ToString() + " - " + dsDaigCode.Tables[0].Rows[i]["DiagCode"].ToString();
                strDaignosis = strDaignosis + "<br/>" + dsDaigCode.Tables[0].Rows[i]["Description"].ToString();
            }
        }

        return strDaignosis;
    }

    private string getPlan(string tablename, string PatientIE_ID)
    {
        DBHelperClass db = new DBHelperClass();
        string query = "";
        if (tablename != "tblbpOtherPart")
            query = "Select FreeFormP from " + tablename + " WHERE PatientIE_ID = " + PatientIE_ID;
        else
            query = "Select OthersP from " + tablename + " WHERE PatientIE_ID = " + PatientIE_ID;

        DataSet dsDaigCode = db.selectData(query);

        string strPlan = "";
        if (dsDaigCode != null && dsDaigCode.Tables[0].Rows.Count > 0)
        {
            if (!string.IsNullOrEmpty(dsDaigCode.Tables[0].Rows[0][0].ToString()))
            {
                strPlan = strPlan + "<br/>" + dsDaigCode.Tables[0].Rows[0][0].ToString();
            }
        }
        return strPlan;
    }

    private string getPlanFU(string tablename, string PatientFU_ID)
    {
        DBHelperClass db = new DBHelperClass();
        string query = "";
        if (tablename != "tblFUbpOtherPart")
            query = "Select FreeFormP from " + tablename + " WHERE PatientFU_ID = " + PatientFU_ID;
        else
            query = "Select OthersP from " + tablename + " WHERE PatientFU_ID = " + PatientFU_ID;

        DataSet dsDaigCode = db.selectData(query);

        string strPlan = "";
        if (dsDaigCode != null && dsDaigCode.Tables[0].Rows.Count > 0)
        {
            if (!string.IsNullOrEmpty(dsDaigCode.Tables[0].Rows[0][0].ToString()))
            {
                strPlan = strPlan + "<br/>" + dsDaigCode.Tables[0].Rows[0][0].ToString();
            }
        }
        return strPlan;
    }

    private string getPOC(string bodypart, string PatientIE_ID)
    {
        DBHelperClass db = new DBHelperClass();


        string SqlStr = @"Select 
                        CASE 
                              WHEN p.Requested is not null 
                               THEN Convert(varchar,p.ProcedureDetail_ID) +'_R'
                              ELSE 
                        		case when p.Scheduled is not null
                        			THEN  Convert(varchar,p.ProcedureDetail_ID) +'_S'
                        		ELSE
                        		   CASE
                        				WHEN p.Executed is not null
                        				THEN Convert(varchar,p.ProcedureDetail_ID) +'_E'
                              END  END END as ID, 
                        CASE 
                              WHEN p.Requested is not null 
                               THEN p.Heading
                              ELSE 
                        		case when p.Scheduled is not null
                        			THEN p.S_Heading
                        		ELSE
                        		   CASE
                        				WHEN p.Executed is not null
                        				THEN p.E_Heading
                              END  END END as Heading, 
                        	  CASE 
                              WHEN p.Requested is not null 
                               THEN p.PDesc
                              ELSE 
                        		case when p.Scheduled is not null
                        			THEN p.S_PDesc
                        		ELSE
                        		   CASE
                        				WHEN p.Executed is not null
                        				THEN p.E_PDesc
                              END  END END as PDesc
                        	 -- ,p.Requested,p.Heading RequestedHeading,p.Scheduled,p.S_Heading ScheduledHeading,p.Executed,p.E_Heading ExecutedHeading
                         from tblProceduresDetail p WHERE PatientIE_ID = " + PatientIE_ID + " AND BodyPart = '" + bodypart + "'  and IsConsidered=0 Order By BodyPart,Heading"; ;


        DataSet dsPOC = db.selectData(SqlStr);

        string strPoc = "";
        if (dsPOC != null && dsPOC.Tables[0].Rows.Count > 0)
        {

            for (int i = 0; i < dsPOC.Tables[0].Rows.Count; i++)
            {
                if (!string.IsNullOrEmpty(dsPOC.Tables[0].Rows[i]["Heading"].ToString()))
                {
                    //if (i != dsPOC.Tables[0].Rows.Count - 1)
                    //    strPoc = strPoc + "<b style='text-transform:uppercase'>" + dsPOC.Tables[0].Rows[i]["Heading"].ToString().TrimEnd(':') + ": </b>" + dsPOC.Tables[0].Rows[i]["PDesc"].ToString() + "<br/><br/>";
                    //else
                    strPoc = strPoc + "<b style='text-transform:uppercase'>" + dsPOC.Tables[0].Rows[i]["Heading"].ToString().TrimEnd(':') + ": </b>" + dsPOC.Tables[0].Rows[i]["PDesc"].ToString() + "<br/><br/>";
                }
            }
        }
        return strPoc;
    }

    private string getPOCFU(string bodypart, string PatientIE_ID, string PatientFU_ID)
    {
        DBHelperClass db = new DBHelperClass();
        string SqlStr = @"Select 
                        CASE 
                              WHEN p.Requested is not null 
                               THEN Convert(varchar,p.ProcedureDetail_ID) +'_R'
                              ELSE 
                        		case when p.Scheduled is not null
                        			THEN  Convert(varchar,p.ProcedureDetail_ID) +'_S'
                        		ELSE
                        		   CASE
                        				WHEN p.Executed is not null
                        				THEN Convert(varchar,p.ProcedureDetail_ID) +'_E'
                              END  END END as ID, 
                        CASE 
                              WHEN p.Requested is not null 
                               THEN p.Heading
                              ELSE 
                        		case when p.Scheduled is not null
                        			THEN p.S_Heading
                        		ELSE
                        		   CASE
                        				WHEN p.Executed is not null
                        				THEN p.E_Heading
                              END  END END as Heading, 
                        	  CASE 
                              WHEN p.Requested is not null 
                               THEN p.PDesc
                              ELSE 
                        		case when p.Scheduled is not null
                        			THEN p.S_PDesc
                        		ELSE
                        		   CASE
                        				WHEN p.Executed is not null
                        				THEN p.E_PDesc
                              END  END END as PDesc
                        	 -- ,p.Requested,p.Heading RequestedHeading,p.Scheduled,p.S_Heading ScheduledHeading,p.Executed,p.E_Heading ExecutedHeading
                         from tblProceduresDetail p WHERE PatientIE_ID = " + PatientIE_ID + " AND BodyPart = '" + bodypart + "' AND PatientFU_ID = '" + PatientFU_ID + "'  and IsConsidered=0 Order By BodyPart,Heading"; ;


        DataSet dsPOC = db.selectData(SqlStr);

        string strPoc = "";
        if (dsPOC != null && dsPOC.Tables[0].Rows.Count > 0)
        {

            for (int i = 0; i < dsPOC.Tables[0].Rows.Count; i++)
            {
                if (!string.IsNullOrEmpty(dsPOC.Tables[0].Rows[i]["Heading"].ToString()))
                {
                    if (i != dsPOC.Tables[0].Rows.Count - 1)
                        strPoc = strPoc + "<br/><br/><b>" + dsPOC.Tables[0].Rows[i]["Heading"].ToString() + "</b>" + dsPOC.Tables[0].Rows[i]["PDesc"].ToString() + "<br/><br/>";
                    else
                        strPoc = strPoc + "<br/><br/><b>" + dsPOC.Tables[0].Rows[i]["Heading"].ToString() + "</b>" + dsPOC.Tables[0].Rows[i]["PDesc"].ToString();
                }
            }
        }
        return strPoc;
    }

    private bool displayCCinPrint()
    {
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(Server.MapPath("~/Template/Default_Admin.xml"));
        XmlNodeList nodeList = xmlDoc.DocumentElement.SelectNodes("/Defaults/Settings");
        bool display = Convert.ToBoolean(nodeList[0].SelectSingleNode("displayCCprint").InnerText);
        return display;
    }

    private void printPTPreport(string PatientIE_ID, string fname, string lname, string dob, string doe, string PatientFU_ID = "0")
    {
        String str = File.ReadAllText(Server.MapPath("~/Template/PTP.html"));
        string query = "", docname = "";

        if (PatientFU_ID == "0")
        {
            query = "Select * from tblInjuredBodyParts Where PatientIE_ID =" + PatientIE_ID;
            docname = lname + "," + fname + "_" + PatientIE_ID + "_PTP_" + CommonConvert.DateFormatPrint(doe) + "_" + CommonConvert.DateFormatPrint(dob);
        }
        else
        {
            query = "Select * from tblInjuredBodyParts Where PatientFU_ID =" + PatientFU_ID;
            docname = lname + "," + fname + "_" + PatientFU_ID + "_PTP_" + CommonConvert.DateFormatPrint(doe) + "_" + CommonConvert.DateFormatPrint(dob);
        }

        DBHelperClass db = new DBHelperClass();
        DataSet ds = db.selectData(query);

        str = str.Replace("#patient", fname + " " + lname);
        str = str.Replace("#date", CommonConvert.DateFormat(doe));
        str = str.Replace("#dob", CommonConvert.DateFormat(dob));

        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            if (ds.Tables[0].Rows[0]["Neck"].ToString().ToLower() == "true")
            {
                str = str.Replace("#c01", "x");
                str = str.Replace("#c03", "x");
                str = str.Replace("#c04", "x");
            }
            else
            {

                str = str.Replace("#c01", " ");
                str = str.Replace("#c03", " ");
                str = str.Replace("#c04", " ");
            }
            str = str.Replace("#c02", " ");
            str = str.Replace("#c05", " ");
            str = str.Replace("#c06", " ");
            str = str.Replace("#c07", " ");
            str = str.Replace("#c08", " ");
            str = str.Replace("#c09", " ");
            str = str.Replace("#c10", " ");

            if (ds.Tables[0].Rows[0]["LowBack"].ToString().ToLower() == "true")
            {
                str = str.Replace("#l01", "x");
                str = str.Replace("#l03", "x");
                str = str.Replace("#l04", "x");
            }
            else
            {
                str = str.Replace("#l01", "");
                str = str.Replace("#l03", "");
                str = str.Replace("#l04", "");
            }

            str = str.Replace("#l02", " ");
            str = str.Replace("#l05", " ");
            str = str.Replace("#l06", " ");
            str = str.Replace("#l07", " ");
            str = str.Replace("#l08", " ");
            str = str.Replace("#l09", " ");
            str = str.Replace("#l10", " ");
            str = str.Replace("#l11", " ");
            str = str.Replace("#l12", " ");
            str = str.Replace("#l13", " ");
            str = str.Replace("#l14", " ");
            str = str.Replace("#l15", " ");
            str = str.Replace("#l16", " ");
            str = str.Replace("#l17", " ");
            str = str.Replace("#l18", " ");
            str = str.Replace("#l19", " ");
            str = str.Replace("#l20", " ");

            if (ds.Tables[0].Rows[0]["LeftShoulder"].ToString().ToLower() == "true" && ds.Tables[0].Rows[0]["RightShoulder"].ToString().ToLower() == "true")
                str = str.Replace("#shoulderaspect", "Bilateral");
            else if (ds.Tables[0].Rows[0]["LeftShoulder"].ToString().ToLower() == "true")
                str = str.Replace("#shoulderaspect", "Left");
            else if (ds.Tables[0].Rows[0]["RightShoulder"].ToString().ToLower() == "true")
                str = str.Replace("#shoulderaspect", "Right");
            else
                str = str.Replace("#shoulderaspect", "Shoulder");

            if (ds.Tables[0].Rows[0]["LeftShoulder"].ToString().ToLower() == "true" || ds.Tables[0].Rows[0]["RightShoulder"].ToString().ToLower() == "true")
            {
                str = str.Replace("#s01", "x");
                str = str.Replace("#s02", "x");
                str = str.Replace("#s03", "x");
                str = str.Replace("#s05", "x");
            }
            else
            {
                str = str.Replace("#s01", "");
                str = str.Replace("#s02", "");
                str = str.Replace("#s03", "");
                str = str.Replace("#s05", "");
            }

            str = str.Replace("#s04", "");
            str = str.Replace("#s06", "");
            str = str.Replace("#s07", "");
            str = str.Replace("#s08", "");
            str = str.Replace("#s09", "");
            str = str.Replace("#s10", "");

            if (ds.Tables[0].Rows[0]["LeftKnee"].ToString().ToLower() == "true" && ds.Tables[0].Rows[0]["RightKnee"].ToString().ToLower() == "true")
                str = str.Replace("#kneeaspect", "Bilateral");
            else if (ds.Tables[0].Rows[0]["LeftKnee"].ToString().ToLower() == "true")
                str = str.Replace("#kneeaspect", "Left");
            else if (ds.Tables[0].Rows[0]["RightKnee"].ToString().ToLower() == "true")
                str = str.Replace("#kneeaspect", "Right");
            else
                str = str.Replace("#kneeaspect", "Kneess");

            if (ds.Tables[0].Rows[0]["LeftKnee"].ToString().ToLower() == "true" || ds.Tables[0].Rows[0]["RightKnee"].ToString().ToLower() == "true")
            {
                str = str.Replace("#k01", "x");
                str = str.Replace("#k02", "x");
                str = str.Replace("#k03", "x");
                str = str.Replace("#k05", "x");
            }
            else
            {
                str = str.Replace("#k01", "");
                str = str.Replace("#k02", "");
                str = str.Replace("#k03", "");
                str = str.Replace("#k05", "");
            }

            str = str.Replace("#k04", "");
            str = str.Replace("#k06", "");
            str = str.Replace("#k07", "");
            str = str.Replace("#k08", "");
            str = str.Replace("#k09", "");
            str = str.Replace("#k10", "");

            str = str.Replace("#o11", "");
            str = str.Replace("#o12", "");
            str = str.Replace("#o13", "");
            str = str.Replace("#o14", "");
            str = str.Replace("#o15", "");
            str = str.Replace("#o16", "");
            str = str.Replace("#o17", "");
            str = str.Replace("#o18", "");
            str = str.Replace("#o19", "");
            str = str.Replace("#o20", "");


            if (PatientFU_ID == "0")
                createWordDocument(str, docname, PatientIE_ID);
            else
                createWordDocument(str, docname, "", PatientFU_ID);

        }

    }

    private void printCFreport(string PatientIE_ID, string fname, string lname, string dob, string doe, string location, string PatientFU_ID = "0")
    {
        String str = File.ReadAllText(Server.MapPath("~/Template/CF.html"));

        string sSQLCustomQuery = " Muscle, Sides, Level, CASE WHEN ISDATE(Requested) = 1 THEN ADesc ";
        sSQLCustomQuery = sSQLCustomQuery + "Else CASE WHEN ISDATE(Scheduled) = 1 THEN S_ADesc Else ";
        sSQLCustomQuery = sSQLCustomQuery + "CASE WHEN ISDATE(Executed) = 1 THEN E_ADesc ELSE NULL END ";
        sSQLCustomQuery = sSQLCustomQuery + " End END AS ADesc,CASE WHEN ISDATE(Requested) = 1 THEN PDesc ";
        sSQLCustomQuery = sSQLCustomQuery + " Else CASE WHEN ISDATE(Scheduled) = 1 THEN S_PDesc ";
        sSQLCustomQuery = sSQLCustomQuery + " Else CASE WHEN ISDATE(Executed) = 1 THEN E_PDesc ELSE NULL END ";
        sSQLCustomQuery = sSQLCustomQuery + " End END AS PDesc,CASE WHEN ISDATE(Requested) = 1 THEN CCDesc ";
        sSQLCustomQuery = sSQLCustomQuery + " Else CASE WHEN ISDATE(Scheduled) = 1 THEN S_CCDesc ";
        sSQLCustomQuery = sSQLCustomQuery + " Else CASE WHEN ISDATE(Executed) = 1 THEN E_CCDesc ELSE NULL END ";
        sSQLCustomQuery = sSQLCustomQuery + " End END AS CCDesc,CASE WHEN ISDATE(Requested) = 1 THEN PEDesc ";
        sSQLCustomQuery = sSQLCustomQuery + " Else CASE WHEN ISDATE(Scheduled) = 1 THEN S_PEDesc ";
        sSQLCustomQuery = sSQLCustomQuery + " Else CASE WHEN ISDATE(Executed) = 1 THEN E_PEDesc ELSE NULL END ";
        sSQLCustomQuery = sSQLCustomQuery + " End END AS PEDesc, CASE WHEN ISDATE(Requested) = 1 THEN Requested ";
        sSQLCustomQuery = sSQLCustomQuery + " Else CASE WHEN ISDATE(Scheduled) = 1 THEN Scheduled ";
        sSQLCustomQuery = sSQLCustomQuery + " Else  CASE WHEN ISDATE(Executed) = 1 THEN Executed ELSE NULL END ";
        sSQLCustomQuery = sSQLCustomQuery + " End END AS PDATE, CASE WHEN ISDATE(Requested) = 1 THEN Requested_Position ";
        sSQLCustomQuery = sSQLCustomQuery + " Else CASE WHEN ISDATE(Scheduled) = 1 THEN Scheduled_Position ";
        sSQLCustomQuery = sSQLCustomQuery + " Else  CASE WHEN ISDATE(Executed) = 1 THEN Executed_Position ELSE NULL END ";
        sSQLCustomQuery = sSQLCustomQuery + " End END AS Position, CASE WHEN ISDATE(Requested) = 1 THEN Heading ";
        sSQLCustomQuery = sSQLCustomQuery + " Else CASE WHEN ISDATE(Scheduled) = 1 THEN S_Heading ";
        sSQLCustomQuery = sSQLCustomQuery + " Else CASE WHEN ISDATE(Executed) = 1 THEN E_Heading ELSE NULL END ";
        sSQLCustomQuery = sSQLCustomQuery + " End END As Heading ";

        string query = "", docname = "";
        if (PatientFU_ID != "0")
        {
            query = "SELECT " + sSQLCustomQuery + " FROM tblProceduresDetail WHERE IsConsidered<> 1 AND(CF = 'True') AND PatientFU_ID IS NULL AND(PatientIE_ID = " + PatientIE_ID + ")";
            docname = lname + "," + fname + "_" + PatientFU_ID + "_CF_" + CommonConvert.DateFormatPrint(doe) + "_" + CommonConvert.DateFormatPrint(dob);
        }
        else
        {
            query = "SELECT " + sSQLCustomQuery + " FROM tblProceduresDetail WHERE IsConsidered<> 1 AND(CF = 'True') AND PatientFU_ID=" + PatientFU_ID + " AND(PatientIE_ID = " + PatientIE_ID + ")";
            docname = lname + "," + fname + "_" + PatientIE_ID + "_CF_" + CommonConvert.DateFormatPrint(doe) + "_" + CommonConvert.DateFormatPrint(dob);

        }


        DBHelperClass db = new DBHelperClass();
        DataSet ds = db.selectData(query);

        string strHeading = "";

        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[i]["Heading"].ToString()))
                {
                    strHeading = strHeading + "," + ds.Tables[0].Rows[i]["Heading"].ToString();
                    strHeading = strHeading.Replace("Procedure:", "");
                    strHeading = strHeading.Replace("Request:", "");

                    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[i]["Sides"].ToString()))
                    {
                        strHeading = strHeading.Replace("(side)", ds.Tables[0].Rows[i]["Sides"].ToString());
                    }

                    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[i]["Level"].ToString()))
                    {
                        strHeading = strHeading.Replace("(level)", ds.Tables[0].Rows[i]["Level"].ToString());
                    }
                }
            }
        }



        StringBuilder sb = new StringBuilder(strHeading.TrimStart(','));

        if (sb.ToString().LastIndexOf(",") >= 0)
            sb.Replace(",", " and ", sb.ToString().LastIndexOf(","), 1);

        str = str.Replace("#patient", fname + " " + lname);
        str = str.Replace("#date", doe);
        str = str.Replace("#dob", dob);
        str = str.Replace("#location", location);
        str = str.Replace("#headings", sb.ToString());
        str = str.Replace("#dos", "<u>&nbsp;&nbsp;&nbsp;&nbsp;" + doe + "&nbsp;&nbsp;&nbsp;&nbsp;</u>");



        if (PatientFU_ID == "0")
            createWordDocument(str, docname, PatientIE_ID);
        else
            createWordDocument(str, docname, "", PatientFU_ID);

        //}

    }

    private void printPNreport(string PatientIE_ID, string fname, string lname, string doa, string doe, string location, string dob, string PatientFU_ID = "0")
    {

        string printtype = "";

        string sSQLCustomQuery = " Muscle, Sides, Level, CASE WHEN ISDATE(Requested) = 1 THEN ADesc ";
        sSQLCustomQuery = sSQLCustomQuery + "Else CASE WHEN ISDATE(Scheduled) = 1 THEN S_ADesc Else ";
        sSQLCustomQuery = sSQLCustomQuery + "CASE WHEN ISDATE(Executed) = 1 THEN E_ADesc ELSE NULL END ";
        sSQLCustomQuery = sSQLCustomQuery + " End END AS ADesc,CASE WHEN ISDATE(Requested) = 1 THEN PDesc ";
        sSQLCustomQuery = sSQLCustomQuery + " Else CASE WHEN ISDATE(Scheduled) = 1 THEN S_PDesc ";
        sSQLCustomQuery = sSQLCustomQuery + " Else CASE WHEN ISDATE(Executed) = 1 THEN E_PDesc ELSE NULL END ";
        sSQLCustomQuery = sSQLCustomQuery + " End END AS PDesc,CASE WHEN ISDATE(Requested) = 1 THEN CCDesc ";
        sSQLCustomQuery = sSQLCustomQuery + " Else CASE WHEN ISDATE(Scheduled) = 1 THEN S_CCDesc ";
        sSQLCustomQuery = sSQLCustomQuery + " Else CASE WHEN ISDATE(Executed) = 1 THEN E_CCDesc ELSE NULL END ";
        sSQLCustomQuery = sSQLCustomQuery + " End END AS CCDesc,CASE WHEN ISDATE(Requested) = 1 THEN PEDesc ";
        sSQLCustomQuery = sSQLCustomQuery + " Else CASE WHEN ISDATE(Scheduled) = 1 THEN S_PEDesc ";
        sSQLCustomQuery = sSQLCustomQuery + " Else CASE WHEN ISDATE(Executed) = 1 THEN E_PEDesc ELSE NULL END ";
        sSQLCustomQuery = sSQLCustomQuery + " End END AS PEDesc, CASE WHEN ISDATE(Requested) = 1 THEN Requested ";
        sSQLCustomQuery = sSQLCustomQuery + " Else CASE WHEN ISDATE(Scheduled) = 1 THEN Scheduled ";
        sSQLCustomQuery = sSQLCustomQuery + " Else  CASE WHEN ISDATE(Executed) = 1 THEN Executed ELSE NULL END ";
        sSQLCustomQuery = sSQLCustomQuery + " End END AS PDATE, CASE WHEN ISDATE(Requested) = 1 THEN Requested_Position ";
        sSQLCustomQuery = sSQLCustomQuery + " Else CASE WHEN ISDATE(Scheduled) = 1 THEN Scheduled_Position ";
        sSQLCustomQuery = sSQLCustomQuery + " Else  CASE WHEN ISDATE(Executed) = 1 THEN Executed_Position ELSE NULL END ";
        sSQLCustomQuery = sSQLCustomQuery + " End END AS Position, CASE WHEN ISDATE(Requested) = 1 THEN Heading ";
        sSQLCustomQuery = sSQLCustomQuery + " Else CASE WHEN ISDATE(Scheduled) = 1 THEN S_Heading ";
        sSQLCustomQuery = sSQLCustomQuery + " Else CASE WHEN ISDATE(Executed) = 1 THEN E_Heading ELSE NULL END ";
        sSQLCustomQuery = sSQLCustomQuery + " End END As Heading ";

        string query = "", docname = "";
        if (PatientFU_ID == "0")
        {
            printtype = "IE";
            query = "SELECT ProcedureDetail_ID, " + sSQLCustomQuery + ", BodyPart, MCODE, DBO.GETPROCMED(MCODE,SubCode) as Medications FROM tblProceduresDetail WHERE IsConsidered <> 1 AND (PN = 'True') AND PatientFU_ID IS NULL AND (PatientIE_ID = (Select PatientIE_ID from tblInjuredBodyParts Where PatientIE_ID = " + PatientIE_ID + "))";
            //    docname = lname + "," + fname + "_" + PatientIE_ID + "_CF_" + Convert.ToDateTime(doe).ToString("mmddyyyy") + "_" + Convert.ToDateTime(dob).ToString("mmddyyyy");
        }
        else
        {
            printtype = "FU";
            query = "SELECT ProcedureDetail_ID, " + sSQLCustomQuery + ", BodyPart, MCODE, DBO.GETPROCMED(MCODE,SubCode) as Medications FROM tblProceduresDetail WHERE IsConsidered <> 1 AND (PN = 'True') AND PatientFU_ID=" + PatientFU_ID + " AND (PatientIE_ID = (Select PatientIE_ID from tblInjuredBodyParts Where PatientIE_ID = " + PatientIE_ID + "))";
            //  docname = lname + "," + fname + "_" + PatientFU_ID + "_CF_" + Convert.ToDateTime(doe).ToString("mmddyyyy") + "_" + Convert.ToDateTime(dob).ToString("mmddyyyy");
        }

        DBHelperClass db = new DBHelperClass();
        DataSet ds = db.selectData(query);

        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                string docpath = Server.MapPath("~/Template/PN/" + ds.Tables[0].Rows[i]["MCODE"].ToString() + ".html");

                if (File.Exists(docpath))
                {
                    String str = File.ReadAllText(docpath);

                    str = str.Replace("#name", fname + " " + lname);
                    str = str.Replace("#date", CommonConvert.DateFormat(doe));
                    str = str.Replace("#dob", CommonConvert.DateFormat(dob));
                    str = str.Replace("#location", location);


                    docname = lname + ", " + fname + "_" + printtype + "_" + ds.Tables[0].Rows[i]["MCODE"].ToString() + "_PN_" + i + 1 + "_" + CommonConvert.DateFormatPrint(doe) + "_" + CommonConvert.DateFormatPrint(doa) + ".docx";


                    string sMuscle = "", sSide = "", sLevel = "", sMedications = "";

                    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[i]["Muscle"].ToString()))
                    {
                        sMuscle = ds.Tables[0].Rows[i]["Muscle"].ToString().Replace("~", " ");

                        if (!string.IsNullOrEmpty(ds.Tables[0].Rows[i]["Sides"].ToString()))
                        {
                            sSide = ds.Tables[0].Rows[i]["Sides"].ToString();
                            sSide = sSide.ToString().Substring(0, 1);

                        }

                        sMuscle = sMuscle.Replace("_X_", "_" + sSide + "_");
                    }

                    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[i]["Level"].ToString()))
                    {
                        sLevel = ds.Tables[0].Rows[i]["Level"].ToString().Replace("~", " ");
                    }

                    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[i]["Medications"].ToString()))
                    {
                        sMedications = ds.Tables[0].Rows[i]["Medications"].ToString().Replace("~", " ");
                    }

                    if (!string.IsNullOrEmpty(sSide))
                    {
                        if (sSide == "L")
                            str = str.Replace("(side)", "left");
                        else if (sSide == "R")
                            str = str.Replace("(side)", "right");
                        else if (sSide == "B")
                            str = str.Replace("(side)", "bilaterals");
                    }



                    if (PatientFU_ID == "0")
                        createWordDocument(str, docname, PatientIE_ID);
                    else
                        createWordDocument(str, docname, "", PatientFU_ID);
                }


            }
        }


    }

    protected void DownloadFiles(string folderPath, string fullname, string IEFU)
    {

        if (Directory.Exists(folderPath))
        {

            using (Ionic.Zip.ZipFile zip = new Ionic.Zip.ZipFile())
            {
                zip.AlternateEncodingUsage = Ionic.Zip.ZipOption.AsNecessary;
                zip.AddDirectoryByName("Files");



                foreach (string file in Directory.EnumerateFiles(folderPath))
                {
                    string contents = file;
                    zip.AddFile(file, "Files");
                }

                //foreach (GridViewRow row in GridView1.Rows)   
                //{
                //    if ((row.FindControl("chkSelect") as CheckBox).Checked)
                //    {
                //        string filePath = (row.FindControl("lblFilePath") as Label).Text;
                //        zip.AddFile(filePath, "Files");
                //    }
                //}


                Response.Clear();
                Response.BufferOutput = false;
                string zipName = String.Format("Zip_{0}_{1}_{2}.zip", fullname, IEFU, DateTime.Now.ToString("yyyy-MMM-dd-HHmmss"));
                Response.ContentType = "application/zip";
                Response.AddHeader("content-disposition", "attachment; filename=" + zipName);
                zip.Save(Response.OutputStream);
                Response.End();
            }
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Popup", "alert('Documents will be available soon.')", true);
        }
    }

    protected void rbllisttype_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindPatientIEDetails();
    }

    protected void lnkDownloadIE_Click(object sender, EventArgs e)
    {
        LinkButton lnk = sender as LinkButton;
        string path = Server.MapPath("~/Reports/Done/" + lnk.CommandArgument.Split(',')[0]);
        string fullname = lnk.CommandArgument.Split(',')[2] + '_' + lnk.CommandArgument.Split(',')[1];
        DownloadFiles(path, fullname, "IE");
    }

    protected void lnkDownloadFU_Click(object sender, EventArgs e)
    {
        LinkButton lnk = sender as LinkButton;
        string path = Server.MapPath("~/Reports/Done/" + lnk.CommandArgument.Split(',')[0]);
        string fullname = lnk.CommandArgument.Split(',')[2] + '_' + lnk.CommandArgument.Split(',')[1];
        DownloadFiles(path, fullname, "FU");
    }

    protected void btnSaveSign_Click(object sender, EventArgs e)
    {
        byte[] blob = null;
        if (string.IsNullOrEmpty(hidBlobServer.Value) == false)
        {
            try
            {
                string blobstring = hidBlobServer.Value.Split(',')[1];
                blob = Convert.FromBase64String(blobstring);


                DirectoryInfo hdDirectoryInWhichToSearch = new DirectoryInfo(Server.MapPath("~/Sign/"));
                FileInfo[] filesInDir = hdDirectoryInWhichToSearch.GetFiles(patientIEIDServer.Value.ToString() + "*.*");
                string fullName = string.Empty;
                foreach (FileInfo foundFile in filesInDir)
                {
                    foundFile.Delete();
                }


                string path = HttpContext.Current.Server.MapPath("~/Sign/");
                string fname = patientIEIDServer.Value.ToString() + "_" + System.DateTime.Now.Millisecond.ToString() + ".jpg";

                string fullpath = path + fname;

                File.WriteAllBytes(fullpath, blob);

                DBHelperClass db = new DBHelperClass();
                string query = "";
                if (patientFUIDServer.Value == "0")
                    query = "delete from tblPatientIESign where PatientIE_ID=" + patientIEIDServer.Value;
                else if (patientIEIDServer.Value == "0")
                    query = "delete from tblPatientIESign where PatinetFU_ID=" + patientFUIDServer.Value;

                db.executeQuery(query);
                query = "insert into tblPatientIESign values(" + patientIEIDServer.Value + ",'" + fullpath + "'," + patientFUIDServer.Value + ",'" + hidBlobServer.Value + "',getdate(),1)";
                db.executeQuery(query);


                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "closeSignModelPopup();", true);

                string name = "";



            }
            catch (Exception ex)
            {
            }

        }
    }

    protected void lnkSignFU_Click(object sender, EventArgs e)
    {
        LinkButton lnk = sender as LinkButton;
        DBHelperClass db = new DBHelperClass();
        DataSet ds = db.selectData("select * from tblPatientIESign where PatinetFU_ID=" + lnk.CommandArgument);
        bool flag = false;
        string filename = "";
        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            flag = true;
            filename = ds.Tables[0].Rows[0]["sign_path"].ToString();
        }

        ClientScript.RegisterStartupScript(this.GetType(), "PopupFU", "openSignModelPopup(0," + lnk.CommandArgument + ",'" + flag + "','" + filename + "');", true);
    }

    protected void lnkSignIE_Click(object sender, EventArgs e)
    {
        LinkButton lnk = sender as LinkButton;

        string[] str = lnk.CommandArgument.Split(',');

        DBHelperClass db = new DBHelperClass();
        DataSet ds = db.selectData("select * from tblPatientIESign where PatientIE_ID=" + str[0]);
        bool flag = false;
        string filename = "";
        string pname = str[2] + " " + str[3];
        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            flag = true;
            filename = ds.Tables[0].Rows[0]["sign_path"].ToString();
        }
        bindSignHTML(str[1], pname);
        ClientScript.RegisterStartupScript(this.GetType(), "PopupIE", "openSignModelPopup(" + str[0] + ",0,'" + flag + "','" + filename + "');", true);
    }

    protected void lnkuploadsign_Click(object sender, EventArgs e)
    {
        LinkButton lnk = sender as LinkButton;

        DBHelperClass db = new DBHelperClass();
        DataSet ds = db.selectData("select * from tblPatientIESign where PatientIE_ID=" + lnk.CommandArgument);
        bool flag = false;
        string filename = "";
        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            flag = true;
            filename = ds.Tables[0].Rows[0]["sign_path"].ToString();
        }

        ClientScript.RegisterStartupScript(this.GetType(), "PopupIE", "opensignupload(" + lnk.CommandArgument + ",0,'" + flag + "','" + filename + "');", true);
    }

    protected void btnuploadimage_Click(object sender, EventArgs e)
    {
        try
        {

            string path = HttpContext.Current.Server.MapPath("~/Sign/");
            string fname = patientIEIDServer.Value.ToString() + "_" + System.DateTime.Now.Millisecond.ToString() + ".jpg";
            string fullpath = path + "//" + fname;
            //if (File.Exists(fullpath))
            //{
            //    File.Delete(fullpath);
            //}
            DirectoryInfo hdDirectoryInWhichToSearch = new DirectoryInfo(Server.MapPath("~/Sign/"));
            FileInfo[] filesInDir = hdDirectoryInWhichToSearch.GetFiles(patientIEIDServer.Value.ToString() + "*.*");
            string fullName = string.Empty;
            foreach (FileInfo foundFile in filesInDir)
            {
                foundFile.Delete();
            }

            if (fupuploadsign.HasFile)
            { fupuploadsign.SaveAs(fullpath); }

            string query = "";
            if (patientFUIDServer.Value == "0")
                query = "delete from tblPatientIESign where PatientIE_ID=" + patientIEIDServer.Value;
            else if (patientIEIDServer.Value == "0")
                query = "delete from tblPatientIESign where PatinetFU_ID=" + patientFUIDServer.Value;

            db.executeQuery(query);
            query = "insert into tblPatientIESign values(" + patientIEIDServer.Value + ",'" + fname + "'," + patientFUIDServer.Value + ",'" + hidBlobServer.Value + "',getdate(),0)";
            db.executeQuery(query);

            ClientScript.RegisterStartupScript(this.GetType(), "Popup", "closeSignuploadModalPopup();", true);
            string name = "";
        }
        catch (Exception ex)
        {
        }
    }

    private void bindLocation()
    {
        DataSet ds = new DataSet();

        string query = "select Location,Location_ID from tblLocations ";
        if (!string.IsNullOrEmpty(Session["Locations"].ToString()))
        {
            query = query + " where Location_ID in (" + Session["Locations"] + ")";
        }
        query = query + " Order By Location";

        ds = db.selectData(query);
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddl_location.DataValueField = "Location_ID";
            ddl_location.DataTextField = "Location";

            ddl_location.DataSource = ds;
            ddl_location.DataBind();

            ddl_location.Items.Insert(0, new ListItem("-- All --", "0"));


        }

    }

    protected void lnkPONReport_Click(object sender, EventArgs e)
    {
        try
        {

            LinkButton linkButton = sender as LinkButton;
            string[] val = linkButton.CommandArgument.Split(',');
            string filename = val[3] + "," + val[4] + "_" + val[1] + "_PON_Report";
            DataSet ds = db.selectData("select PONPrint from tblFUbpOtherPart where PatientFU_ID=" + val[1]);
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                string body = File.ReadAllText(Server.MapPath("~/Template/PON.html"));

                body = body.Replace("#content", ds.Tables[0].Rows[0]["PONPrint"].ToString());
                body = body.Replace("#date", val[2].ToString());
                body = body.Replace("#pname", val[4].ToString() + " " + val[3].ToString());

                createWordDocument(body, filename, val[0]);
                downloadfile(filename + ".doc");
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "AlertPopup", "javascript:alert('Please Add Report first from OtherPart menu.')", true);
            }
        }
        catch (Exception ex)
        {

        }
    }

    private void downloadfile(string filename)
    {
        string fname = "~/Reports/" + filename;
        if (fname != string.Empty)
        {
            WebClient req = new WebClient();
            HttpResponse response = HttpContext.Current.Response;
            string filePath = fname;
            response.Clear();
            response.ClearContent();
            response.ClearHeaders();
            response.Buffer = true;
            response.AddHeader("Content-Disposition", "attachment;filename=\"" + filename + "\"");
            byte[] data = req.DownloadData(Server.MapPath(filePath));
            response.BinaryWrite(data);
            response.End();
        }
    }

    protected void lnkDelete_Click(object sender, EventArgs e)
    {
        try
        {
            LinkButton lnkdel = sender as LinkButton;
            SqlParameter[] parameters = new SqlParameter[1];
            parameters[0] = new SqlParameter("@patientIEID", lnkdel.CommandArgument);

            int val = db.executeSP("nusp_Delete_PatientIE", parameters);
            if (val > 0)
                BindPatientIEDetails();
        }
        catch (Exception ex)
        {
            Logger.Error(ex);
        }
    }

    protected void lnkDelete_FU_Click(object sender, EventArgs e)
    {
        try
        {
            LinkButton lnkdel = sender as LinkButton;
            SqlParameter[] parameters = new SqlParameter[1];
            parameters[0] = new SqlParameter("@patientFUID", lnkdel.CommandArgument);

            int val = db.executeSP("nusp_Delete_PatientFU", parameters);
            if (val > 0)
                BindPatientIEDetails();
        }
        catch (Exception ex)
        {
            Logger.Error(ex);
        }
    }

    private string getBodyParts(Int64 patientIEID)
    {

        List<string> _injured = new BusinessLogic().getInjuredParts(patientIEID).Distinct<string>().ToList<string>();
        string str = "";

        if (_injured.Contains("Neck"))
            str = str + ", neck";

        if (_injured.Contains("MidBack"))
            str = str + ", midback";

        if (_injured.Contains("LownBack"))
            str = str + ", lownback";

        if (_injured.Contains("RightAnkle"))
            str = str + ", right ankle";

        if (_injured.Contains("LeftAnkle"))
            str = str + ", left ankle";

        if (_injured.Contains("LeftElbow"))
            str = str + ", left elbow";

        if (_injured.Contains("RightElbow"))
            str = str + ", right elbow";

        if (_injured.Contains("LeftHip"))
            str = str + ", left hip";

        if (_injured.Contains("RightHip"))
            str = str + ", right hip";

        if (_injured.Contains("LeftKnee"))
            str = str + ", left knee";

        if (_injured.Contains("RightKnee"))
            str = str + ", right knee";

        if (_injured.Contains("LeftShoulder"))
            str = str + ", left shoulder";

        if (_injured.Contains("RightShoulder"))
            str = str + ", right shoulder";

        if (_injured.Contains("LeftWrist"))
            str = str + ", left wrist";

        if (_injured.Contains("RightWrist"))
            str = str + ", right wrist";

        return str.TrimStart(',');
    }

    private string getBodyPartswithnumber(string patientIEID, string casetype, string doa)
    {

        Int64 ieId = Convert.ToInt64(patientIEID);
        List<string> _injured = new BusinessLogic().getInjuredParts(ieId).Distinct<string>().ToList<string>();
        string _part = "", str = "";

        int i = 0;
        foreach (string part in _injured)
        {
            if (part.ToLower() != "other")
            {

                i++;
                if (part.Contains("Left"))
                {
                    _part = part.Insert(4, " ");
                }
                else if (part.Contains("Right"))
                {
                    _part = part.Insert(5, " ");
                }
                else
                    _part = part;

                str = str + i + ". " + _part + " Pain. ";
            }
        }
        if (casetype.ToLower() == "nf")
        {
            i++;
            str = str + i + ". S/P MVA injury on " + doa + ".";
        }
        return str.TrimStart(',');
    }

    public void bindSignHTML(string type, string pname)
    {
        string path = "";

        if (type.ToLower() == "nf")
            path = Server.MapPath("~/Template/NFSignpade.html");
        else if (type.ToLower() == "wc")
            path = Server.MapPath("~/Template/WCSignpade.html");

        string body = File.ReadAllText(path);

        body = body.Replace("#date", System.DateTime.Now.ToString("MM/dd/yyyy"));
        body = body.Replace("#pname", pname);

        divSignHTML.InnerHtml = body;


    }

    public string formatString(string str)
    {
        string _str = "";
        _str = str.Replace(" /", "/");
        _str = _str.Replace(". .", ".");
        _str = _str.Replace(".  .", ".");
        _str = _str.Replace(", .", ".");
        _str = _str.Replace("..", ".");

        return _str;
    }
}