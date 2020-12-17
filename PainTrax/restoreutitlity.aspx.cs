using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Data.SqlClient;
using System.Configuration;
using System.Xml;

public partial class restoreutitlity : System.Web.UI.Page
{
    DBHelperClass db = new DBHelperClass();

    string ccOrg = "", peOrg = "", ccValue = "", _ccValue = "", peValue = "", path = "", painVal = "", id = "", painValR = "", painValL = "";
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnIE_Click(object sender, EventArgs e)
    {
        try
        {

            //neck IE restoration

            DataSet ds = db.selectData("select * from tblbpNeck  where CreatedDate<='10/18/2020'");
            //DataSet ds = db.selectData("select * from tblbpNeck  where  PatientIE_ID=15627");

            int cnt = ds.Tables[0].Rows.Count;



            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {

                path = Server.MapPath("~/Template/Restore/NeckCC.html");
                ccValue = File.ReadAllText(path);

                path = Server.MapPath("~/Template/NeckCC.html");
                ccOrg = File.ReadAllText(path);

                path = Server.MapPath("~/Template/NeckPE.html");
                peValue = peOrg = File.ReadAllText(path);

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                //for (int i = 0; i < 1; i++)
                {

                    painVal = ds.Tables[0].Rows[i]["PainScale"].ToString();
                    id = ds.Tables[0].Rows[i]["PatientDetail_ID"].ToString();
                    _ccValue = ccValue;

                    _ccValue = _ccValue.Replace("#neckpain", painVal);

                    _ccValue = _ccValue.Replace("#chkweakness", (string.IsNullOrEmpty(ds.Tables[0].Rows[i]["WeeknessIn"].ToString()) ? "" : "checked='checked'"));
                    _ccValue = _ccValue.Replace("#txtweakness", ds.Tables[0].Rows[i]["WeeknessIn"].ToString());

                    if (ds.Tables[0].Rows[i]["WorseDriving"].ToString() == "True" || ds.Tables[0].Rows[i]["WorseTwisting"].ToString() == "True"
                        || string.IsNullOrEmpty(ds.Tables[0].Rows[i]["WorseOther"].ToString()) == false)
                        _ccValue = _ccValue.Replace("#chkworsewith", "checked='checked'");
                    else
                        _ccValue = _ccValue.Replace("#chkworsewith", "");

                    _ccValue = _ccValue.Replace("#chkdriving", (ds.Tables[0].Rows[i]["WorseDriving"].ToString() == "True" ? "checked='checked'" : ""));
                    _ccValue = _ccValue.Replace("#chktwisting", (ds.Tables[0].Rows[i]["WorseTwisting"].ToString() == "True" ? "checked='checked'" : ""));
                    _ccValue = _ccValue.Replace("#txtworsend", ds.Tables[0].Rows[i]["WorseOther"].ToString());


                    if (ds.Tables[0].Rows[i]["ImprovedResting"].ToString() == "True" || ds.Tables[0].Rows[i]["ImprovedMedication"].ToString() == "True"
                        || ds.Tables[0].Rows[i]["ImprovedTherapy"].ToString() == "True"
                        || ds.Tables[0].Rows[i]["ImprovedSleeping"].ToString() == "True"
                        || ds.Tables[0].Rows[i]["ImprovedMovement"].ToString() == "True")
                        _ccValue = _ccValue.Replace("#chkimprovedwith", "checked='checked'");
                    else
                        _ccValue = _ccValue.Replace("#chkimprovedwith", "");

                    _ccValue = _ccValue.Replace("#chkresting", (ds.Tables[0].Rows[i]["ImprovedResting"].ToString() == "True" ? "checked='checked'" : ""));
                    _ccValue = _ccValue.Replace("#chkmedication", (ds.Tables[0].Rows[i]["ImprovedMedication"].ToString() == "True" ? "checked='checked'" : ""));
                    _ccValue = _ccValue.Replace("#chktherapy", (ds.Tables[0].Rows[i]["ImprovedTherapy"].ToString() == "True" ? "checked='checked'" : ""));
                    _ccValue = _ccValue.Replace("#chksleeping", (ds.Tables[0].Rows[i]["ImprovedSleeping"].ToString() == "True" ? "checked='checked'" : ""));
                    _ccValue = _ccValue.Replace("#chkmovement", (ds.Tables[0].Rows[i]["ImprovedMovement"].ToString() == "True" ? "checked='checked'" : ""));


                    this.neckLBROM(ds.Tables[0].Rows[i]["FwdFlexRight"].ToString(), ds.Tables[0].Rows[i]["ExtensionRight"].ToString(), ds.Tables[0].Rows[i]["RotationRight"].ToString(),
                        ds.Tables[0].Rows[i]["RotationLeft"].ToString(), ds.Tables[0].Rows[i]["LateralFlexRight"].ToString(), ds.Tables[0].Rows[i]["LateralFlexLeft"].ToString(),
                        ds.Tables[0].Rows[i]["PatientDetail_ID"].ToString(), "Neck");


                    this.updateCCPE(_ccValue, ccOrg, peValue, peValue, id, "tblbpNeck");
                }

                lblMess.InnerText = "Total " + ds.Tables[0].Rows.Count + " Restore";
            }

        }
        catch (Exception ex)
        {

        }
    }

    private void updateCCPE(string ccValue, string ccOrg, string peValue, string peOrg, string id, string tblName)
    {
        string query = "update " + tblName + " set CCvalue=@CCValue,CCvalueoriginal=@CCvalueoriginal,PEvalue=@PEvalue,PEvalueoriginal=@PEvalueoriginal where PatientDetail_ID=@id";

        using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString))
        using (SqlCommand command = new SqlCommand(query, connection))
        {
            command.Parameters.AddWithValue("@id", id);
            command.Parameters.AddWithValue("@CCValue", ccValue);
            command.Parameters.AddWithValue("@CCvalueoriginal", ccOrg);
            command.Parameters.AddWithValue("@PEvalue", peValue);
            command.Parameters.AddWithValue("@PEvalueoriginal", peOrg);


            connection.Open();
            var results = command.ExecuteNonQuery();
            connection.Close();
        }


    }

    private void neckLBROM(string FwdFlexRight, string ExtensionRight, string RotationRight, string RotationLeft, string LateralFlexRight, string LateralFlexLeft, string id, string tblName, bool isFU = false)
    {
        try
        {
            string leftROM = "", rightROM = "", cROM = "", strsideName = "", strsideNormal = "", strCName = "", strCNormal = "";

            cROM = FwdFlexRight + "," + ExtensionRight;
            leftROM = RotationLeft;
            rightROM = RotationRight;

            //open the tender xml file  
            XmlTextReader xmlreader = new XmlTextReader(Server.MapPath("~/XML/" + tblName + ".xml"));
            //reading the xml data  
            DataSet ds = new DataSet();
            ds.ReadXml(xmlreader);
            xmlreader.Close();

            if (ds.Tables.Count != 0)
            {

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    strsideName = strsideName + "," + ds.Tables[0].Rows[i]["name"].ToString();
                    strsideNormal = strsideNormal + "," + ds.Tables[0].Rows[i]["normal"].ToString();
                }

                for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                {
                    strCName = strCName + "," + ds.Tables[1].Rows[i]["name"].ToString();
                    strCNormal = strCNormal + "," + ds.Tables[1].Rows[i]["normal"].ToString();
                }
            }

            strCName = strCName.TrimStart(',');
            strCNormal = strCNormal.TrimStart(',');
            strsideName = strsideName.TrimStart(',');
            strsideNormal = strsideNormal.TrimStart(',');

            if (!isFU)
                tblName = "tblbp" + tblName;
            else
                tblName = "tblFUbp" + tblName;

            string q = " update " + tblName + " set CNameROM='" + strCName + "',CROM='" + cROM + "',CNormalROM='" + strCNormal + "',NameROM='" + strsideName + "',NormalROM='" + strsideNormal + "',LeftROM='" + leftROM + "',RightROM='" + rightROM + "' where PatientDetail_ID=" + id;

            db.executeQuery(q);

        }
        catch (Exception ex)
        {
        }
    }

    private void shoulderROM(string AbductionLeft, string FlexionLeft, string ExtRotationLeft, string IntRotationLeft, string AbductionRight, string FlexionRight, string ExtRotationRight, string IntRotationRight, string id, bool isFU = false)
    {
        try
        {
            string leftROM = "", rightROM = "", cROM = "", strCName = "", strCNormal = "";


            leftROM = AbductionLeft + "," + FlexionLeft + "," + ExtRotationLeft + "," + IntRotationLeft;
            rightROM = AbductionRight + "," + FlexionRight + "," + ExtRotationRight + "," + IntRotationRight;

            //open the tender xml file  
            XmlTextReader xmlreader = new XmlTextReader(Server.MapPath("~/XML/Shoulder.xml"));
            //reading the xml data  
            DataSet ds = new DataSet();
            ds.ReadXml(xmlreader);
            xmlreader.Close();

            if (ds.Tables.Count != 0)
            {


                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    strCName = strCName + "," + ds.Tables[0].Rows[i]["name"].ToString();
                    strCNormal = strCNormal + "," + ds.Tables[0].Rows[i]["normal"].ToString();
                }
            }

            strCName = strCName.TrimStart(',');
            strCNormal = strCNormal.TrimStart(',');

            string tblName = "tblbpshoulder";

            if (isFU)
                tblName = "tblFUbpshoulder";

            string q = " update " + tblName + " set NameROM='" + strCName + "',NormalROM='" + strCNormal + "',LeftROM='" + leftROM + "',RightROM='" + rightROM + "' where PatientDetail_ID=" + id;

            db.executeQuery(q);

        }
        catch (Exception ex)
        {
        }
    }

    private void kneeROM(string FlexionROM1, string FlexionROM2, string ExtensionROM1, string ExtensionROM2, string id, bool isFU = false)
    {
        try
        {
            string leftROM = "", rightROM = "", strCName = "", strCNormal = "";


            leftROM = FlexionROM1 + "," + ExtensionROM1;
            rightROM = FlexionROM2 + "," + ExtensionROM2;

            //open the tender xml file  
            XmlTextReader xmlreader = new XmlTextReader(Server.MapPath("~/XML/Knee.xml"));
            //reading the xml data  
            DataSet ds = new DataSet();
            ds.ReadXml(xmlreader);
            xmlreader.Close();

            if (ds.Tables.Count != 0)
            {


                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    strCName = strCName + "," + ds.Tables[0].Rows[i]["name"].ToString();
                    strCNormal = strCNormal + "," + ds.Tables[0].Rows[i]["normal"].ToString();
                }
            }

            strCName = strCName.TrimStart(',');
            strCNormal = strCNormal.TrimStart(',');


            string tblname = "tblbpknee";
            if (isFU)
                tblname = "tblFUbpknee";

            string q = " update " + tblname + " set NameROM='" + strCName + "',NormalROM='" + strCNormal + "',LeftROM='" + leftROM + "',RightROM='" + rightROM + "' where PatientDetail_ID=" + id;

            db.executeQuery(q);

        }
        catch (Exception ex)
        {
        }
    }

    private void ElbowROM(string FlexionROM1, string FlexionROM2, string ExtensionROM1, string ExtensionROM2, string SupinationROM1, string SupinationROM2, string PronationROM1, string PronationROM2, string id, bool isFU = false)
    {
        try
        {
            string leftROM = "", rightROM = "", strCName = "", strCNormal = "";


            leftROM = ExtensionROM1 + "," + FlexionROM1 + "," + SupinationROM1 + "," + PronationROM1;
            rightROM = ExtensionROM2 + "," + FlexionROM2 + "," + SupinationROM2 + "," + PronationROM2;

            //open the tender xml file  
            XmlTextReader xmlreader = new XmlTextReader(Server.MapPath("~/XML/Elbow.xml"));
            //reading the xml data  
            DataSet ds = new DataSet();
            ds.ReadXml(xmlreader);
            xmlreader.Close();

            if (ds.Tables.Count != 0)
            {


                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    strCName = strCName + "," + ds.Tables[0].Rows[i]["name"].ToString();
                    strCNormal = strCNormal + "," + ds.Tables[0].Rows[i]["normal"].ToString();
                }
            }

            strCName = strCName.TrimStart(',');
            strCNormal = strCNormal.TrimStart(',');

            string tblname = "tblbpelbow";
            if (isFU)
                tblname = "tblFUbpelbow";


            string q = " update " + tblname + " set NameROM='" + strCName + "',NormalROM='" + strCNormal + "',LeftROM='" + leftROM + "',RightROM='" + rightROM + "' where PatientDetail_ID=" + id;

            db.executeQuery(q);

        }
        catch (Exception ex)
        {
        }
    }

    private void WristROM(string FlexionROMLeft, string FlexionROMRight, string PainUponDorsiFlexionLeft, string PainUponDorsiFlexionRight, string PainUponUlnarDeviationLeft, string PainUponUlnarDeviationRight, string RadialDeviationROMLeft, string RadialDeviationROMRight, string id, bool isFU = false)
    {
        try
        {
            string leftROM = "", rightROM = "", strCName = "", strCNormal = "";


            leftROM = FlexionROMLeft + "," + PainUponDorsiFlexionLeft + "," + PainUponUlnarDeviationLeft + "," + RadialDeviationROMLeft + ",";
            rightROM = FlexionROMRight + "," + PainUponDorsiFlexionRight + "," + PainUponUlnarDeviationRight + "," + RadialDeviationROMRight + ",";

            //open the tender xml file  
            XmlTextReader xmlreader = new XmlTextReader(Server.MapPath("~/XML/Wrist.xml"));
            //reading the xml data  
            DataSet ds = new DataSet();
            ds.ReadXml(xmlreader);
            xmlreader.Close();

            if (ds.Tables.Count != 0)
            {


                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    strCName = strCName + "," + ds.Tables[0].Rows[i]["name"].ToString();
                    strCNormal = strCNormal + "," + ds.Tables[0].Rows[i]["normal"].ToString();
                }
            }

            strCName = strCName.TrimStart(',');
            strCNormal = strCNormal.TrimStart(',');

            string tblname = "tblbpwrist";
            if (isFU)
                tblname = "tblFUbpwrist";


            string q = " update " + tblname + " set NameROM='" + strCName + "',NormalROM='" + strCNormal + "',LeftROM='" + leftROM + "',RightROM='" + rightROM + "' where PatientDetail_ID=" + id;

            db.executeQuery(q);

        }
        catch (Exception ex)
        {
        }
    }

    protected void btnFU_Click(object sender, EventArgs e)
    {
        try
        {
            string ccOrg = "", peOrg = "", ccValue = "", peValue = "", path = "", painVal = "", id = "", painValR = "", painValL = "";

            //neck FU restoration

            DataSet ds = db.selectData("select * from tblFUbpNeck  where CreatedDate<='10/18/2020'");


            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {

                path = Server.MapPath("~/Template/Restore/NeckCC.html");
                _ccValue = File.ReadAllText(path);

                path = Server.MapPath("~/Template/NeckCC.html");
                ccOrg = File.ReadAllText(path);

                path = Server.MapPath("~/Template/NeckPE.html");
                peValue = peOrg = File.ReadAllText(path);

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                // for (int i = 0; i < 1; i++)
                {
                    ccValue = _ccValue;
                    painVal = ds.Tables[0].Rows[i]["PainScale"].ToString();
                    id = ds.Tables[0].Rows[i]["PatientDetail_ID"].ToString();
                    ccValue = ccValue.Replace("#neckpain", painVal);

                    ccValue = ccValue.Replace("#chkweakness", (string.IsNullOrEmpty(ds.Tables[0].Rows[i]["WeeknessIn"].ToString()) ? "" : "checked='checked'"));
                    ccValue = ccValue.Replace("#txtweakness", ds.Tables[0].Rows[i]["WeeknessIn"].ToString());

                    if (ds.Tables[0].Rows[i]["WorseDriving"].ToString() == "True" || ds.Tables[0].Rows[i]["WorseTwisting"].ToString() == "True"
                        || string.IsNullOrEmpty(ds.Tables[0].Rows[i]["WorseOther"].ToString()) == false)
                        ccValue = ccValue.Replace("#chkworsewith", "checked='checked'");
                    else
                        ccValue = ccValue.Replace("#chkworsewith", "");

                    ccValue = ccValue.Replace("#chkdriving", (ds.Tables[0].Rows[i]["WorseDriving"].ToString() == "True" ? "checked='checked'" : ""));
                    ccValue = ccValue.Replace("#chktwisting", (ds.Tables[0].Rows[i]["WorseTwisting"].ToString() == "True" ? "checked='checked'" : ""));
                    ccValue = ccValue.Replace("#txtworsend", ds.Tables[0].Rows[i]["WorseOther"].ToString());


                    if (ds.Tables[0].Rows[i]["ImprovedResting"].ToString() == "True" || ds.Tables[0].Rows[i]["ImprovedMedication"].ToString() == "True"
                        || ds.Tables[0].Rows[i]["ImprovedTherapy"].ToString() == "True"
                        || ds.Tables[0].Rows[i]["ImprovedSleeping"].ToString() == "True"
                        || ds.Tables[0].Rows[i]["ImprovedMovement"].ToString() == "True")
                        ccValue = ccValue.Replace("#chkimprovedwith", "checked='checked'");
                    else
                        ccValue = ccValue.Replace("#chkimprovedwith", "");

                    ccValue = ccValue.Replace("#chkresting", (ds.Tables[0].Rows[i]["ImprovedResting"].ToString() == "True" ? "checked='checked'" : ""));
                    ccValue = ccValue.Replace("#chkmedication", (ds.Tables[0].Rows[i]["ImprovedMedication"].ToString() == "True" ? "checked='checked'" : ""));
                    ccValue = ccValue.Replace("#chktherapy", (ds.Tables[0].Rows[i]["ImprovedTherapy"].ToString() == "True" ? "checked='checked'" : ""));
                    ccValue = ccValue.Replace("#chksleeping", (ds.Tables[0].Rows[i]["ImprovedSleeping"].ToString() == "True" ? "checked='checked'" : ""));
                    ccValue = ccValue.Replace("#chkmovement", (ds.Tables[0].Rows[i]["ImprovedMovement"].ToString() == "True" ? "checked='checked'" : ""));


                    this.neckLBROM(ds.Tables[0].Rows[i]["FwdFlexRight"].ToString(), ds.Tables[0].Rows[i]["ExtensionRight"].ToString(), ds.Tables[0].Rows[i]["RotationRight"].ToString(),
                        ds.Tables[0].Rows[i]["RotationLeft"].ToString(), ds.Tables[0].Rows[i]["LateralFlexRight"].ToString(), ds.Tables[0].Rows[i]["LateralFlexLeft"].ToString(),
                        ds.Tables[0].Rows[i]["PatientDetail_ID"].ToString(), "Neck", true);

                    this.updateCCPE(ccValue, ccOrg, peValue, peValue, id, "tblFUbpNeck");
                }

                lblMess.InnerText = "Total " + ds.Tables[0].Rows.Count + " Restore";
            }
        }
        catch (Exception ex)
        {

        }
    }

    protected void btnpage1_Click(object sender, EventArgs e)
    {
        try
        {
            string strtop = "", strsocial = "", straccident1 = "", straccident2 = "", path = "", strdegree = "", strhostory = "";

            //IE restoration

            DataSet ds = db.selectData("select * from tblPatientIE");

            path = Server.MapPath("~/Template/Page1_top.html");
            string body = File.ReadAllText(path);

            strtop = body;

            path = Server.MapPath("~/Template/Page1_social.html");
            body = File.ReadAllText(path);

            strsocial = body;

            path = Server.MapPath("~/Template/Page1_accident.html");
            body = File.ReadAllText(path);

            straccident1 = body;

            path = Server.MapPath("~/Template/Page1_accident_2.html");
            body = File.ReadAllText(path);

            straccident2 = body;

            path = Server.MapPath("~/Template/Page1_degree.html");
            body = File.ReadAllText(path);

            strdegree = body;



            path = Server.MapPath("~/Template/Page1_history.html");
            body = File.ReadAllText(path);

            strhostory = body;


            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString))

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    //for (int i = 0; i < 1; i++)
                    {
                        string query = "select id from tblPage1HTMLContent where PatientIE_ID=" + ds.Tables[0].Rows[i]["PatientIE_ID"].ToString();

                        DataSet dscnt = db.selectData(query);

                        if (dscnt.Tables[0].Rows.Count == 0)
                        {
                            query = "insert into tblPage1HTMLContent(topSectionHTML,socialSectionHTML,accidentHTML,PatientIE_ID,PatientFU_ID,historyHTML,accident_1_HTML,degreeHTML)";
                            query = query + "values(@topSectionHTML,@socialSectionHTML,@accidentHTML,@PatientIE_ID,@PatientFU_ID,@historyHTML,@accident_1_HTML,@degreeHTML)";

                            using (SqlCommand command = new SqlCommand(query, connection))
                            {
                                command.Parameters.AddWithValue("@topSectionHTML", strtop);
                                command.Parameters.AddWithValue("@socialSectionHTML", strsocial);
                                command.Parameters.AddWithValue("@accidentHTML", straccident1);
                                command.Parameters.AddWithValue("@PatientIE_ID", ds.Tables[0].Rows[i]["PatientIE_ID"].ToString());
                                command.Parameters.AddWithValue("@PatientFU_ID", "0");
                                command.Parameters.AddWithValue("@historyHTML", strhostory);
                                command.Parameters.AddWithValue("@accident_1_HTML", straccident2);
                                command.Parameters.AddWithValue("@degreeHTML", strdegree);


                                connection.Open();
                                var results = command.ExecuteNonQuery();
                                connection.Close();
                            }

                        }
                    }
            }



            //FU restoration

            ds = db.selectData("select * from tblFUPatient");

            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString))

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    //for (int i = 0; i < 1; i++)
                    {
                        string query = "select id from tblPage1FUHTMLContent where PateintFU_ID=" + ds.Tables[0].Rows[i]["PatientFU_ID"].ToString();

                        DataSet dscnt = db.selectData(query);

                        if (dscnt.Tables[0].Rows.Count == 0)
                        {
                            query = "insert into tblPage1FUHTMLContent(degreeSectionHTML,socialSectionHTML,PatientIE_ID,PateintFU_ID,topSectionHTML)";
                            query = query + "values(@degreeSectionHTML,@socialSectionHTML,@PatientIE_ID,@PateintFU_ID,@topSectionHTML)";

                            using (SqlCommand command = new SqlCommand(query, connection))
                            {
                                command.Parameters.AddWithValue("@degreeSectionHTML", strdegree);
                                command.Parameters.AddWithValue("@socialSectionHTML", strsocial);
                                command.Parameters.AddWithValue("@PatientIE_ID", ds.Tables[0].Rows[i]["PatientIE_ID"].ToString());
                                command.Parameters.AddWithValue("@PateintFU_ID", ds.Tables[0].Rows[i]["PatientFU_ID"].ToString());
                                command.Parameters.AddWithValue("@topSectionHTML", strtop);


                                connection.Open();
                                var results = command.ExecuteNonQuery();
                                connection.Close();
                            }

                        }
                    }
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void btnpage2_Click(object sender, EventArgs e)
    {
        try
        {
            string strtop = "", strros = "", strcomp = "", path = "", strdegree = "";

            //neck IE restoration

            DataSet ds = db.selectData("select * from tblPatientIE");

            path = Server.MapPath("~/Template/Page2_top.html");
            string body = File.ReadAllText(path);

            strtop = body;

            path = Server.MapPath("~/Template/Page2_degree.html");
            body = File.ReadAllText(path);

            strdegree = body;

            path = Server.MapPath("~/Template/Page2_ros.html");
            body = File.ReadAllText(path);

            strros = body;

            path = Server.MapPath("~/Template/Page2_complain.html");
            body = File.ReadAllText(path);

            strcomp = body;



            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString))

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    //for (int i = 0; i < 1; i++)
                    {
                        string query = "select id from tblPage2HTMLContent where PatientIE_ID=" + ds.Tables[0].Rows[i]["PatientIE_ID"].ToString();

                        DataSet dscnt = db.selectData(query);

                        if (dscnt.Tables[0].Rows.Count == 0)
                        {
                            query = "insert into tblPage2HTMLContent(rosSectionHTML,topSectionHTML,degreeSectionHTML,PatientIE_ID,PatientFU_ID,complainSectionHTML)";
                            query = query + "values(@rosSectionHTML,@topSectionHTML,@degreeSectionHTML,@PatientIE_ID,@PatientFU_ID,@complainSectionHTML)";

                            using (SqlCommand command = new SqlCommand(query, connection))
                            {
                                command.Parameters.AddWithValue("@topSectionHTML", strtop);
                                command.Parameters.AddWithValue("@rosSectionHTML", strros);

                                command.Parameters.AddWithValue("@PatientIE_ID", ds.Tables[0].Rows[i]["PatientIE_ID"].ToString());
                                command.Parameters.AddWithValue("@PatientFU_ID", "0");
                                command.Parameters.AddWithValue("@degreeSectionHTML", strdegree);
                                command.Parameters.AddWithValue("@complainSectionHTML", strcomp);


                                connection.Open();
                                var results = command.ExecuteNonQuery();
                                connection.Close();
                            }

                        }
                    }
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void btnpage3_Click(object sender, EventArgs e)
    {
        try
        {
            string strtop = "", strpage3 = "", path = "";

            //IE restoration

            DataSet ds = db.selectData("select * from tblPatientIEDetailPage2");
            // DataSet ds = db.selectData("select * from tblPatientIEDetailPage2 where PatientIE_ID in (select PatientIE_ID from tblPatientIE where CreatedDate <= '10/18/2020')");





            path = Server.MapPath("~/Template/Page3_top.html");
            string body = File.ReadAllText(path);

            strtop = body;


            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString))
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)

                    {
                        string query = "select id from tblPage3HTMLContent where PatientIE_ID=" + ds.Tables[0].Rows[i]["PatientIE_ID"].ToString();

                        DataSet dscnt = db.selectData(query);

                        if (dscnt.Tables[0].Rows.Count == 0)
                        {
                            path = Server.MapPath("~/Template/Restore/Page3.html");
                            body = File.ReadAllText(path);
                            strpage3 = body;

                            strpage3 = strpage3.Replace("#LTricepstxt", ds.Tables[0].Rows[i]["DTRtricepsLeft"].ToString());
                            strpage3 = strpage3.Replace("#RTricepstxt", ds.Tables[0].Rows[i]["DTRtricepsRight"].ToString());
                            strpage3 = strpage3.Replace("#LBicepstxt", ds.Tables[0].Rows[i]["DTRtricepsLeft"].ToString());
                            strpage3 = strpage3.Replace("#RBicepstxt", ds.Tables[0].Rows[i]["DTRBicepsRight"].ToString());
                            strpage3 = strpage3.Replace("#LBrachioradialis", ds.Tables[0].Rows[i]["DTRBrachioLeft"].ToString());
                            strpage3 = strpage3.Replace("#RBrachioradialis", ds.Tables[0].Rows[i]["DTRBrachioRight"].ToString());


                            strpage3 = strpage3.Replace("#LLateralarm", ds.Tables[0].Rows[i]["UEC5Left"].ToString());
                            strpage3 = strpage3.Replace("#RLateralarm", ds.Tables[0].Rows[i]["UEC5Right"].ToString());
                            strpage3 = strpage3.Replace("#LLateralforearm", ds.Tables[0].Rows[i]["UEC6Left"].ToString());
                            strpage3 = strpage3.Replace("#RLateralforearm", ds.Tables[0].Rows[i]["UEC6Right"].ToString());
                            strpage3 = strpage3.Replace("#LMiddlefinger", ds.Tables[0].Rows[i]["UEC7Left"].ToString());
                            strpage3 = strpage3.Replace("#RMiddlefinger", ds.Tables[0].Rows[i]["UEC7Right"].ToString());
                            strpage3 = strpage3.Replace("#LMidialForearm", ds.Tables[0].Rows[i]["UEC8Left"].ToString());
                            strpage3 = strpage3.Replace("#RMidialForearm", ds.Tables[0].Rows[i]["UEC8Right"].ToString());
                            strpage3 = strpage3.Replace("#LMidialarm", ds.Tables[0].Rows[i]["UET1Left"].ToString());
                            strpage3 = strpage3.Replace("#RMidialarm", ds.Tables[0].Rows[i]["UET1Right"].ToString());
                            strpage3 = strpage3.Replace("#LCervical", ds.Tables[0].Rows[i]["UECervicalParaspinalsLeft"].ToString());
                            strpage3 = strpage3.Replace("#RCervical", ds.Tables[0].Rows[i]["UECervicalParaspinalsRight"].ToString());
                            strpage3 = strpage3.Replace("#LtxtDMTL3", ds.Tables[0].Rows[i]["LEL3Left"].ToString());
                            strpage3 = strpage3.Replace("#RtxtDMTL3", ds.Tables[0].Rows[i]["LEL3Right"].ToString());
                            strpage3 = strpage3.Replace("#LtxtMLFL4", ds.Tables[0].Rows[i]["LEL4Left"].ToString());
                            strpage3 = strpage3.Replace("#RtxtMLFL4", ds.Tables[0].Rows[i]["LEL4Right"].ToString());
                            strpage3 = strpage3.Replace("#LtxtDOFL5", ds.Tables[0].Rows[i]["LEL5Left"].ToString());
                            strpage3 = strpage3.Replace("#RtxtDOFL5", ds.Tables[0].Rows[i]["LEL5Right"].ToString());
                            strpage3 = strpage3.Replace("#LtxtLTS1", ds.Tables[0].Rows[i]["LES1Left"].ToString());
                            strpage3 = strpage3.Replace("#RtxtLTS1", ds.Tables[0].Rows[i]["LES1Right"].ToString());
                            strpage3 = strpage3.Replace("#LtxtLP", ds.Tables[0].Rows[i]["LELumberParaspinalsLeft"].ToString());
                            strpage3 = strpage3.Replace("#RtxtLP", ds.Tables[0].Rows[i]["LELumberParaspinalsRight"].ToString());


                            strpage3 = strpage3.Replace("#LAbduction", ds.Tables[0].Rows[i]["UEShoulderAbductionLeft"].ToString());
                            strpage3 = strpage3.Replace("#RAbduction", ds.Tables[0].Rows[i]["UEShoulderAbductionRight"].ToString());
                            strpage3 = strpage3.Replace("#LFlexion", ds.Tables[0].Rows[i]["UEShoulderFlexionLeft"].ToString());
                            strpage3 = strpage3.Replace("#RFlexion", ds.Tables[0].Rows[i]["UEShoulderFlexionRight"].ToString());
                            strpage3 = strpage3.Replace("#LElbowExtension", ds.Tables[0].Rows[i]["UEElbowExtensionLeft"].ToString());
                            strpage3 = strpage3.Replace("#RElbowExtension", ds.Tables[0].Rows[i]["UEElbowExtensionRight"].ToString());
                            strpage3 = strpage3.Replace("#LElbowFlexion", ds.Tables[0].Rows[i]["UEElbowFlexionLeft"].ToString());
                            strpage3 = strpage3.Replace("#RElbowFlexion", ds.Tables[0].Rows[i]["UEElbowFlexionRight"].ToString());
                            strpage3 = strpage3.Replace("#LSupination", ds.Tables[0].Rows[i]["UEElbowSupinationLeft"].ToString());
                            strpage3 = strpage3.Replace("#RSupination", ds.Tables[0].Rows[i]["UEElbowSupinationRight"].ToString());
                            strpage3 = strpage3.Replace("#LPronation", ds.Tables[0].Rows[i]["UEElbowPronationLeft"].ToString());
                            strpage3 = strpage3.Replace("#RPronation", ds.Tables[0].Rows[i]["UEElbowPronationRight"].ToString());
                            strpage3 = strpage3.Replace("#LWristFlexion", ds.Tables[0].Rows[i]["UEWristFlexionLeft"].ToString());
                            strpage3 = strpage3.Replace("#RWristFlexion", ds.Tables[0].Rows[i]["UEWristFlexionRight"].ToString());
                            strpage3 = strpage3.Replace("#LWristExtension", ds.Tables[0].Rows[i]["UEWristExtensionLeft"].ToString());
                            strpage3 = strpage3.Replace("#RWristExtension", ds.Tables[0].Rows[i]["UEWristExtensionRight"].ToString());
                            strpage3 = strpage3.Replace("#LGrip", ds.Tables[0].Rows[i]["UEHandGripStrengthLeft"].ToString());
                            strpage3 = strpage3.Replace("#RGrip", ds.Tables[0].Rows[i]["UEHandGripStrengthRight"].ToString());
                            strpage3 = strpage3.Replace("#LFinger", ds.Tables[0].Rows[i]["UEHandFingerAbductorsLeft"].ToString());
                            strpage3 = strpage3.Replace("#RFinger", ds.Tables[0].Rows[i]["UEHandFingerAbductorsRight"].ToString());
                            strpage3 = strpage3.Replace("#LHipFlexion", ds.Tables[0].Rows[i]["LEHipFlexionLeft"].ToString());
                            strpage3 = strpage3.Replace("#RHipFlexion", ds.Tables[0].Rows[i]["LEHipFlexionRight"].ToString());
                            strpage3 = strpage3.Replace("#LHipAbduction", ds.Tables[0].Rows[i]["LEHipAbductionLeft"].ToString());
                            strpage3 = strpage3.Replace("#RHipAbduction", ds.Tables[0].Rows[i]["LEHipAbductionRight"].ToString());
                            strpage3 = strpage3.Replace("#LKneeExtension", ds.Tables[0].Rows[i]["LEKneeExtensionLeft"].ToString());
                            strpage3 = strpage3.Replace("#RKneeExtension", ds.Tables[0].Rows[i]["LEKneeExtensionRight"].ToString());
                            strpage3 = strpage3.Replace("#LKneeFlexion", ds.Tables[0].Rows[i]["LEKneeFlexionLeft"].ToString());
                            strpage3 = strpage3.Replace("#RKneeFlexion", ds.Tables[0].Rows[i]["LEKneeFlexionRight"].ToString());
                            strpage3 = strpage3.Replace("#LDorsiflexion", ds.Tables[0].Rows[i]["LEAnkleDorsiLeft"].ToString());
                            strpage3 = strpage3.Replace("#RDorsiflexion", ds.Tables[0].Rows[i]["LEAnkleDorsiRight"].ToString());
                            strpage3 = strpage3.Replace("#LPlantar", ds.Tables[0].Rows[i]["LEAnklePlantarLeft"].ToString());
                            strpage3 = strpage3.Replace("#RPlantar", ds.Tables[0].Rows[i]["LEAnklePlantarRight"].ToString());
                            strpage3 = strpage3.Replace("#LExtensor", ds.Tables[0].Rows[i]["LEAnkleExtensorLeft"].ToString());
                            strpage3 = strpage3.Replace("#RExtensor", ds.Tables[0].Rows[i]["LEAnkleExtensorRight"].ToString());

                            strpage3 = strpage3.Replace("#txtIntactExcept", ds.Tables[0].Rows[i]["intactexcept"].ToString());
                            strpage3 = strpage3.Replace("#txtSensory", ds.Tables[0].Rows[i]["Sensory"].ToString());

                            strpage3 = strpage3.Replace("#LKnee", ds.Tables[0].Rows[i]["DTRKneeLeft"].ToString());
                            strpage3 = strpage3.Replace("#RKnee", ds.Tables[0].Rows[i]["DTRKneeRight"].ToString());
                            strpage3 = strpage3.Replace("#LAnkle", ds.Tables[0].Rows[i]["DTRAnkleLeft"].ToString());
                            strpage3 = strpage3.Replace("#RAnkle", ds.Tables[0].Rows[i]["DTRAnkleRight"].ToString());



                            query = "insert into tblPage3HTMLContent(topSectionHTML,HTMLContent,PatientIE_ID)";
                            query = query + "values(@topSectionHTML,@HTMLContent,@PatientIE_ID)";

                            using (SqlCommand command = new SqlCommand(query, connection))
                            {
                                command.Parameters.AddWithValue("@topSectionHTML", strtop);
                                command.Parameters.AddWithValue("@HTMLContent", strpage3);

                                command.Parameters.AddWithValue("@PatientIE_ID", ds.Tables[0].Rows[i]["PatientIE_ID"].ToString());



                                connection.Open();
                                var results = command.ExecuteNonQuery();
                                connection.Close();
                            }

                        }
                    }
                }

            }

            //FU restoration

            ds = db.selectData("select * from tblFUNeurologicalExam");
            //ds = db.selectData("select * from tblFUNeurologicalExam where PatientFU_ID in (select PatientFU_ID from tblFUPatient where CreatedDate <= '10/18/2020')");

            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString))

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    // for (int i = 0; i < 2; i++)
                    {
                        string query = "select id from tblPage3FUHTMLContent where PatientFU_ID=" + ds.Tables[0].Rows[i]["PatientFU_ID"].ToString();

                        DataSet dscnt = db.selectData(query);

                        if (dscnt.Tables[0].Rows.Count == 0)
                        {

                            path = Server.MapPath("~/Template/Restore/Page3.html");
                            body = File.ReadAllText(path);
                            strpage3 = body;

                            strpage3 = strpage3.Replace("#LTricepstxt", ds.Tables[0].Rows[i]["DTRtricepsLeft"].ToString());
                            strpage3 = strpage3.Replace("#RTricepstxt", ds.Tables[0].Rows[i]["DTRtricepsRight"].ToString());
                            strpage3 = strpage3.Replace("#LBicepstxt", ds.Tables[0].Rows[i]["DTRtricepsLeft"].ToString());
                            strpage3 = strpage3.Replace("#RBicepstxt", ds.Tables[0].Rows[i]["DTRBicepsRight"].ToString());
                            strpage3 = strpage3.Replace("#LBrachioradialis", ds.Tables[0].Rows[i]["DTRBrachioLeft"].ToString());
                            strpage3 = strpage3.Replace("#RBrachioradialis", ds.Tables[0].Rows[i]["DTRBrachioRight"].ToString());
                            strpage3 = strpage3.Replace("#LKnee", ds.Tables[0].Rows[i]["DTRKneeLeft"].ToString());
                            strpage3 = strpage3.Replace("#RKnee", ds.Tables[0].Rows[i]["DTRKneeRight"].ToString());
                            strpage3 = strpage3.Replace("#LAnkle", ds.Tables[0].Rows[i]["DTRAnkleLeft"].ToString());
                            strpage3 = strpage3.Replace("#RAnkle", ds.Tables[0].Rows[i]["DTRAnkleRight"].ToString());

                            strpage3 = strpage3.Replace("#LLateralarm", ds.Tables[0].Rows[i]["UEC5Left"].ToString());
                            strpage3 = strpage3.Replace("#RLateralarm", ds.Tables[0].Rows[i]["UEC5Right"].ToString());
                            strpage3 = strpage3.Replace("#LLateralforearm", ds.Tables[0].Rows[i]["UEC6Left"].ToString());
                            strpage3 = strpage3.Replace("#RLateralforearm", ds.Tables[0].Rows[i]["UEC6Right"].ToString());
                            strpage3 = strpage3.Replace("#LMiddlefinger", ds.Tables[0].Rows[i]["UEC7Left"].ToString());
                            strpage3 = strpage3.Replace("#RMiddlefinger", ds.Tables[0].Rows[i]["UEC7Right"].ToString());
                            strpage3 = strpage3.Replace("#LMidialForearm", ds.Tables[0].Rows[i]["UEC8Left"].ToString());
                            strpage3 = strpage3.Replace("#RMidialForearm", ds.Tables[0].Rows[i]["UEC8Right"].ToString());
                            strpage3 = strpage3.Replace("#LMidialarm", ds.Tables[0].Rows[i]["UET1Left"].ToString());
                            strpage3 = strpage3.Replace("#RMidialarm", ds.Tables[0].Rows[i]["UET1Right"].ToString());
                            strpage3 = strpage3.Replace("#LCervical", ds.Tables[0].Rows[i]["UECervicalParaspinalsLeft"].ToString());
                            strpage3 = strpage3.Replace("#RCervical", ds.Tables[0].Rows[i]["UECervicalParaspinalsRight"].ToString());
                            strpage3 = strpage3.Replace("#LtxtDMTL3", ds.Tables[0].Rows[i]["LEL3Left"].ToString());
                            strpage3 = strpage3.Replace("#RtxtDMTL3", ds.Tables[0].Rows[i]["LEL3Right"].ToString());
                            strpage3 = strpage3.Replace("#LtxtMLFL4", ds.Tables[0].Rows[i]["LEL4Left"].ToString());
                            strpage3 = strpage3.Replace("#RtxtMLFL4", ds.Tables[0].Rows[i]["LEL4Right"].ToString());
                            strpage3 = strpage3.Replace("#LtxtDOFL5", ds.Tables[0].Rows[i]["LEL5Left"].ToString());
                            strpage3 = strpage3.Replace("#RtxtDOFL5", ds.Tables[0].Rows[i]["LEL5Right"].ToString());
                            strpage3 = strpage3.Replace("#LtxtLTS1", "");
                            strpage3 = strpage3.Replace("#RtxtLTS1", "");
                            strpage3 = strpage3.Replace("#LtxtLP", ds.Tables[0].Rows[i]["LELumberParaspinalsLeft"].ToString());
                            strpage3 = strpage3.Replace("#RtxtLP", ds.Tables[0].Rows[i]["LELumberParaspinalsRight"].ToString());


                            strpage3 = strpage3.Replace("#LAbduction", ds.Tables[0].Rows[i]["UEShoulderAbductionLeft"].ToString());
                            strpage3 = strpage3.Replace("#RAbduction", ds.Tables[0].Rows[i]["UEShoulderAbductionRight"].ToString());
                            strpage3 = strpage3.Replace("#LFlexion", ds.Tables[0].Rows[i]["UEShoulderFlexionLeft"].ToString());
                            strpage3 = strpage3.Replace("#RFlexion", ds.Tables[0].Rows[i]["UEShoulderFlexionRight"].ToString());
                            strpage3 = strpage3.Replace("#LElbowExtension", ds.Tables[0].Rows[i]["UEElbowExtensionLeft"].ToString());
                            strpage3 = strpage3.Replace("#RElbowExtension", ds.Tables[0].Rows[i]["UEElbowExtensionRight"].ToString());
                            strpage3 = strpage3.Replace("#LElbowFlexion", ds.Tables[0].Rows[i]["UEElbowFlexionLeft"].ToString());
                            strpage3 = strpage3.Replace("#RElbowFlexion", ds.Tables[0].Rows[i]["UEElbowFlexionRight"].ToString());
                            strpage3 = strpage3.Replace("#LSupination", ds.Tables[0].Rows[i]["UEElbowSupinationLeft"].ToString());
                            strpage3 = strpage3.Replace("#RSupination", ds.Tables[0].Rows[i]["UEElbowSupinationRight"].ToString());
                            strpage3 = strpage3.Replace("#LPronation", ds.Tables[0].Rows[i]["UEElbowPronationLeft"].ToString());
                            strpage3 = strpage3.Replace("#RPronation", ds.Tables[0].Rows[i]["UEElbowPronationRight"].ToString());
                            strpage3 = strpage3.Replace("#LWristFlexion", ds.Tables[0].Rows[i]["UEWristFlexionLeft"].ToString());
                            strpage3 = strpage3.Replace("#RWristFlexion", ds.Tables[0].Rows[i]["UEWristFlexionRight"].ToString());
                            strpage3 = strpage3.Replace("#LWristExtension", ds.Tables[0].Rows[i]["UEWristExtensionLeft"].ToString());
                            strpage3 = strpage3.Replace("#RWristExtension", ds.Tables[0].Rows[i]["UEWristExtensionRight"].ToString());
                            strpage3 = strpage3.Replace("#LGrip", ds.Tables[0].Rows[i]["UEHandGripStrengthLeft"].ToString());
                            strpage3 = strpage3.Replace("#RGrip", ds.Tables[0].Rows[i]["UEHandGripStrengthRight"].ToString());
                            strpage3 = strpage3.Replace("#LFinger", ds.Tables[0].Rows[i]["UEHandFingerAbductorsLeft"].ToString());
                            strpage3 = strpage3.Replace("#RFinger", ds.Tables[0].Rows[i]["UEHandFingerAbductorsRight"].ToString());
                            strpage3 = strpage3.Replace("#LHipFlexion", ds.Tables[0].Rows[i]["LEHipFlexionLeft"].ToString());
                            strpage3 = strpage3.Replace("#RHipFlexion", ds.Tables[0].Rows[i]["LEHipFlexionRight"].ToString());
                            strpage3 = strpage3.Replace("#LHipAbduction", ds.Tables[0].Rows[i]["LEHipAbductionLeft"].ToString());
                            strpage3 = strpage3.Replace("#RHipAbduction", ds.Tables[0].Rows[i]["LEHipAbductionRight"].ToString());
                            strpage3 = strpage3.Replace("#LKneeExtension", ds.Tables[0].Rows[i]["LEKneeExtensionLeft"].ToString());
                            strpage3 = strpage3.Replace("#RKneeExtension", ds.Tables[0].Rows[i]["LEKneeExtensionRight"].ToString());
                            strpage3 = strpage3.Replace("#LKneeFlexion", ds.Tables[0].Rows[i]["LEKneeFlexionLeft"].ToString());
                            strpage3 = strpage3.Replace("#RKneeFlexion", ds.Tables[0].Rows[i]["LEKneeFlexionRight"].ToString());
                            strpage3 = strpage3.Replace("#LDorsiflexion", ds.Tables[0].Rows[i]["LEAnkleDorsiLeft"].ToString());
                            strpage3 = strpage3.Replace("#RDorsiflexion", ds.Tables[0].Rows[i]["LEAnkleDorsiRight"].ToString());
                            strpage3 = strpage3.Replace("#LPlantar", ds.Tables[0].Rows[i]["LEAnklePlantarLeft"].ToString());
                            strpage3 = strpage3.Replace("#RPlantar", ds.Tables[0].Rows[i]["LEAnklePlantarRight"].ToString());
                            strpage3 = strpage3.Replace("#LExtensor", ds.Tables[0].Rows[i]["LEAnkleExtensorLeft"].ToString());
                            strpage3 = strpage3.Replace("#RExtensor", ds.Tables[0].Rows[i]["LEAnkleExtensorRight"].ToString());

                            strpage3 = strpage3.Replace("#txtIntactExcept", ds.Tables[0].Rows[i]["intactexcept"].ToString());
                            strpage3 = strpage3.Replace("#txtSensory", ds.Tables[0].Rows[i]["Sensory"].ToString());


                            query = "insert into tblPage3FUHTMLContent(topSectionHTML,HTMLContent,PatientIE_ID,PatientFU_ID)";
                            query = query + "values(@topSectionHTML,@HTMLContent,@PatientIE_ID,@PatientFU_ID)";

                            using (SqlCommand command = new SqlCommand(query, connection))
                            {
                                command.Parameters.AddWithValue("@topSectionHTML", strtop);
                                command.Parameters.AddWithValue("@HTMLContent", strpage3);
                                command.Parameters.AddWithValue("@PatientIE_ID", 0);
                                command.Parameters.AddWithValue("@PatientFU_ID", ds.Tables[0].Rows[i]["PatientFU_ID"].ToString());

                                connection.Open();
                                var results = command.ExecuteNonQuery();
                                connection.Close();
                            }

                        }
                    }
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void btnMBIE_Click(object sender, EventArgs e)
    {

        //midback IE restoration

        DataSet ds = db.selectData("select * from tblbpMidBack  where CreatedDate<='10/18/2020'");


        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {



            path = Server.MapPath("~/Template/Restore/MidbackCC.html");
            _ccValue = File.ReadAllText(path);

            path = Server.MapPath("~/Template/MidbackCC.html");
            ccOrg = File.ReadAllText(path);

            path = Server.MapPath("~/Template/MidbackPE.html");
            peValue = peOrg = File.ReadAllText(path);

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            //for (int i = 0; i < 1; i++)
            {
                ccValue = _ccValue;
                painVal = ds.Tables[0].Rows[i]["PainScale"].ToString();
                id = ds.Tables[0].Rows[i]["PatientDetail_ID"].ToString();
                ccValue = ccValue.Replace("#midbackpain", painVal);





                if (ds.Tables[0].Rows[i]["WorseSitting"].ToString() == "True" || ds.Tables[0].Rows[i]["WorseStanding"].ToString() == "True"
                                       || string.IsNullOrEmpty(ds.Tables[0].Rows[i]["WorseOtherText"].ToString()) == false)
                    ccValue = ccValue.Replace("#chkworsewith", "checked='checked'");
                else
                    ccValue = ccValue.Replace("#chkworsewith", "");

                ccValue = ccValue.Replace("#chksitting", (ds.Tables[0].Rows[i]["WorseSitting"].ToString() == "True" ? "checked='checked'" : ""));
                ccValue = ccValue.Replace("#chkstanding", (ds.Tables[0].Rows[i]["WorseStanding"].ToString() == "True" ? "checked='checked'" : ""));

                ccValue = ccValue.Replace("#txtworsend", ds.Tables[0].Rows[i]["WorseOtherText"].ToString());


                ccValue = ccValue.Replace("#chkresting", (ds.Tables[0].Rows[i]["ImprovedResting"].ToString() == "True" ? "checked='checked'" : ""));
                ccValue = ccValue.Replace("#chkmedication", (ds.Tables[0].Rows[i]["ImprovedMedication"].ToString() == "True" ? "checked='checked'" : ""));
                ccValue = ccValue.Replace("#chktherapy", (ds.Tables[0].Rows[i]["ImprovedTherapy"].ToString() == "True" ? "checked='checked'" : ""));
                ccValue = ccValue.Replace("#chksleeping", (ds.Tables[0].Rows[i]["ImprovedSleeping"].ToString() == "True" ? "checked='checked'" : ""));
                ccValue = ccValue.Replace("#chkmovement", (ds.Tables[0].Rows[i]["ImprovedMovement"].ToString() == "True" ? "checked='checked'" : ""));

                if (ds.Tables[0].Rows[i]["ImprovedResting"].ToString() == "True" || ds.Tables[0].Rows[i]["ImprovedMedication"].ToString() == "True"
                     || ds.Tables[0].Rows[i]["ImprovedTherapy"].ToString() == "True"
                     || ds.Tables[0].Rows[i]["ImprovedSleeping"].ToString() == "True"
                     || ds.Tables[0].Rows[i]["ImprovedMovement"].ToString() == "True")
                    ccValue = ccValue.Replace("#chkimprovedwith", "checked='checked'");
                else
                    ccValue = ccValue.Replace("#chkimprovedwith", "");



                this.updateCCPE(ccValue, ccOrg, peValue, peValue, id, "tblbpMidBack");
            }
            lblMess.InnerText = "Total " + ds.Tables[0].Rows.Count + " Restore";
        }
    }

    protected void btnLBIE_Click(object sender, EventArgs e)
    {
        //lowback IE restoration

        DataSet ds = db.selectData("select * from tblbpLowBack  where CreatedDate<='10/18/2020'");


        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {

            path = Server.MapPath("~/Template/Restore/LowbackCC.html");
            _ccValue = File.ReadAllText(path);

            path = Server.MapPath("~/Template/LowbackCC.html");
            ccOrg = File.ReadAllText(path);

            path = Server.MapPath("~/Template/LowbackPE.html");
            peValue = peOrg = File.ReadAllText(path);

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            //for (int i = 0; i < 1; i++)
            {
                ccValue = _ccValue;
                painVal = ds.Tables[0].Rows[i]["PainScale"].ToString();
                id = ds.Tables[0].Rows[i]["PatientDetail_ID"].ToString();
                ccValue = ccValue.Replace("#lowbackpain", painVal);

                ccValue = ccValue.Replace("#chkweakness", (string.IsNullOrEmpty(ds.Tables[0].Rows[i]["WeeknessIn"].ToString()) ? "" : "checked='checked'"));
                ccValue = ccValue.Replace("#txtweakness", ds.Tables[0].Rows[i]["WeeknessIn"].ToString());




                if (ds.Tables[0].Rows[i]["WorseSitting"].ToString() == "True" || ds.Tables[0].Rows[i]["WorseStanding"].ToString() == "True"
                    || ds.Tables[0].Rows[i]["WorseLifting"].ToString() == "True"
                    || string.IsNullOrEmpty(ds.Tables[0].Rows[i]["WorseOtherText"].ToString()) == false)
                    ccValue = ccValue.Replace("#chkworsewith", "checked='checked'");
                else
                    ccValue = ccValue.Replace("#chkworsewith", "");

                ccValue = ccValue.Replace("#chksitting", (ds.Tables[0].Rows[i]["WorseSitting"].ToString() == "True" ? "checked='checked'" : ""));
                ccValue = ccValue.Replace("#chkstanding", (ds.Tables[0].Rows[i]["WorseStanding"].ToString() == "True" ? "checked='checked'" : ""));
                ccValue = ccValue.Replace("#chklifting", (ds.Tables[0].Rows[i]["WorseLifting"].ToString() == "True" ? "checked='checked'" : ""));
                ccValue = ccValue.Replace("#txtworsend", ds.Tables[0].Rows[i]["WorseOtherText"].ToString());


                ccValue = ccValue.Replace("#chkresting", (ds.Tables[0].Rows[i]["ImprovedResting"].ToString() == "True" ? "checked='checked'" : ""));
                ccValue = ccValue.Replace("#chkmedication", (ds.Tables[0].Rows[i]["ImprovedMedication"].ToString() == "True" ? "checked='checked'" : ""));
                ccValue = ccValue.Replace("#chktherapy", (ds.Tables[0].Rows[i]["ImprovedTherapy"].ToString() == "True" ? "checked='checked'" : ""));
                ccValue = ccValue.Replace("#chksleeping", (ds.Tables[0].Rows[i]["ImprovedSleeping"].ToString() == "True" ? "checked='checked'" : ""));
                ccValue = ccValue.Replace("#chkmovement", (ds.Tables[0].Rows[i]["ImprovedMovement"].ToString() == "True" ? "checked='checked'" : ""));

                if (ds.Tables[0].Rows[i]["ImprovedResting"].ToString() == "True" || ds.Tables[0].Rows[i]["ImprovedMedication"].ToString() == "True"
                     || ds.Tables[0].Rows[i]["ImprovedTherapy"].ToString() == "True"
                     || ds.Tables[0].Rows[i]["ImprovedSleeping"].ToString() == "True"
                     || ds.Tables[0].Rows[i]["ImprovedMovement"].ToString() == "True")
                    ccValue = ccValue.Replace("#chkimprovedwith", "checked='checked'");
                else
                    ccValue = ccValue.Replace("#chkimprovedwith", "");


                this.neckLBROM(ds.Tables[0].Rows[i]["FwdFlex"].ToString(), ds.Tables[0].Rows[i]["Extension"].ToString(), ds.Tables[0].Rows[i]["RotationRight"].ToString(),
                ds.Tables[0].Rows[i]["RotationLeft"].ToString(), ds.Tables[0].Rows[i]["LateralFlexRight"].ToString(), ds.Tables[0].Rows[i]["LateralFlexLeft"].ToString(),
                ds.Tables[0].Rows[i]["PatientDetail_ID"].ToString(), "LowBack");

                this.updateCCPE(ccValue, ccOrg, peValue, peValue, id, "tblbpLowBack");
            }
            lblMess.InnerText = "Total " + ds.Tables[0].Rows.Count + " Restore";
        }
    }

    protected void btnElbowIE_Click(object sender, EventArgs e)
    {
        //elbow IE restoration

        DataSet ds = db.selectData("select * from tblbpElbow  where CreatedDate<='10/18/2020'");


        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {

            path = Server.MapPath("~/Template/Restore/ElbowCC.html");
            _ccValue = File.ReadAllText(path);

            path = Server.MapPath("~/Template/ElbowCC.html");
            ccOrg = File.ReadAllText(path);

            path = Server.MapPath("~/Template/ElbowPE.html");
            peValue = peOrg = File.ReadAllText(path);

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            //for (int i = 0; i < 1; i++)
            {

                painValL = ds.Tables[0].Rows[i]["PainScaleLeft"].ToString();
                painValR = ds.Tables[0].Rows[i]["PainScaleRight"].ToString();
                id = ds.Tables[0].Rows[i]["PatientDetail_ID"].ToString();

                ccValue = _ccValue;

                ccValue = ccValue.Replace("#painR", painValR);
                ccValue = ccValue.Replace("#painL", painValL);





                ccValue = ccValue.Replace("#chkmedialL", (ds.Tables[0].Rows[i]["MedEpicondyleLeft"].ToString() == "True" ? "checked='checked'" : ""));
                ccValue = ccValue.Replace("#chkmedialR", (ds.Tables[0].Rows[i]["MedEpicondyleRight"].ToString() == "True" ? "checked='checked'" : ""));

                ccValue = ccValue.Replace("#chklateralL", (ds.Tables[0].Rows[i]["LatEpicondyleLeft"].ToString() == "True" ? "checked='checked'" : ""));
                ccValue = ccValue.Replace("#chklateralR", (ds.Tables[0].Rows[i]["LatEpicondyleRight"].ToString() == "True" ? "checked='checked'" : ""));

                ccValue = ccValue.Replace("#chkolecranonL", (ds.Tables[0].Rows[i]["OlecranonLeft"].ToString() == "True" ? "checked='checked'" : ""));
                ccValue = ccValue.Replace("#chkolecranonR", (ds.Tables[0].Rows[i]["OlecranonRight"].ToString() == "True" ? "checked='checked'" : ""));


                if (ds.Tables[0].Rows[i]["OlecranonLeft"].ToString() == "True" || ds.Tables[0].Rows[i]["LatEpicondyleLeft"].ToString() == "True"
                    || ds.Tables[0].Rows[i]["MedEpicondyleLeft"].ToString() == "True")
                    ccValue = ccValue.Replace("#chkspecL", "checked='checked'");
                else
                    ccValue = ccValue.Replace("#chkspecL", "");


                if (ds.Tables[0].Rows[i]["OlecranonRight"].ToString() == "True" || ds.Tables[0].Rows[i]["LatEpicondyleRight"].ToString() == "True"
                    || ds.Tables[0].Rows[i]["MedEpicondyleRight"].ToString() == "True")
                    ccValue = ccValue.Replace("#chkspecR", "checked='checked'");
                else
                    ccValue = ccValue.Replace("#chkspecR", "");


                ccValue = ccValue.Replace("#chkraisingL", (ds.Tables[0].Rows[i]["RaisingArmLeft"].ToString() == "True" ? "checked='checked'" : ""));
                ccValue = ccValue.Replace("#chkraisingR", (ds.Tables[0].Rows[i]["RaisingArmRight"].ToString() == "True" ? "checked='checked'" : ""));

                ccValue = ccValue.Replace("#chkliftingL", (ds.Tables[0].Rows[i]["LiftingObjectLeft"].ToString() == "True" ? "checked='checked'" : ""));
                ccValue = ccValue.Replace("#chkliftingR", (ds.Tables[0].Rows[i]["LiftingObjectRight"].ToString() == "True" ? "checked='checked'" : ""));

                ccValue = ccValue.Replace("#chkrotationL", (ds.Tables[0].Rows[i]["RotationLeft"].ToString() == "True" ? "checked='checked'" : ""));
                ccValue = ccValue.Replace("#chkrotationR", (ds.Tables[0].Rows[i]["RotationRight"].ToString() == "True" ? "checked='checked'" : ""));

                ccValue = ccValue.Replace("#chkworkingL", (ds.Tables[0].Rows[i]["WorkingLeft"].ToString() == "True" ? "checked='checked'" : ""));
                ccValue = ccValue.Replace("#chkworkingR", (ds.Tables[0].Rows[i]["WorkingRight"].ToString() == "True" ? "checked='checked'" : ""));

                ccValue = ccValue.Replace("#txtPainScaleL2", ds.Tables[0].Rows[i]["NoteLeft"].ToString());
                ccValue = ccValue.Replace("#txtPainScaleR2", ds.Tables[0].Rows[i]["NoteRight"].ToString());


                if (ds.Tables[0].Rows[i]["RaisingArmLeft"].ToString() == "True" || ds.Tables[0].Rows[i]["LiftingObjectLeft"].ToString() == "True"
                 || ds.Tables[0].Rows[i]["RotationLeft"].ToString() == "True" || ds.Tables[0].Rows[i]["WorkingLeft"].ToString() == "True"
                 || !string.IsNullOrEmpty(ds.Tables[0].Rows[i]["NoteLeft"].ToString()))
                    ccValue = ccValue.Replace("#chkspecL", "checked='checked'");
                else
                    ccValue = ccValue.Replace("#chkspecL", "");


                if (ds.Tables[0].Rows[i]["RaisingArmRight"].ToString() == "True" || ds.Tables[0].Rows[i]["LiftingObjectRight"].ToString() == "True"
               || ds.Tables[0].Rows[i]["RotationRight"].ToString() == "True" || ds.Tables[0].Rows[i]["WorkingRight"].ToString() == "True"
               || !string.IsNullOrEmpty(ds.Tables[0].Rows[i]["NoteRight"].ToString()))
                    ccValue = ccValue.Replace("#chkspecR", "checked='checked'");
                else
                    ccValue = ccValue.Replace("#chkspecR", "");





                this.ElbowROM(ds.Tables[0].Rows[i]["FlexionROM1"].ToString(), ds.Tables[0].Rows[i]["FlexionROM2"].ToString(), ds.Tables[0].Rows[i]["ExtensionROM1"].ToString(),
           ds.Tables[0].Rows[i]["ExtensionROM2"].ToString(), ds.Tables[0].Rows[i]["SupinationROM1"].ToString(), ds.Tables[0].Rows[i]["SupinationROM2"].ToString(),
           ds.Tables[0].Rows[i]["PronationROM1"].ToString(), ds.Tables[0].Rows[i]["PronationROM2"].ToString(),
           ds.Tables[0].Rows[i]["PatientDetail_ID"].ToString());

                this.updateCCPE(ccValue, ccOrg, peValue, peValue, id, "tblbpElbow");
            }
            lblMess.InnerText = "Total " + ds.Tables[0].Rows.Count + " Restore";
        }

    }

    protected void btnWristIE_Click(object sender, EventArgs e)
    {
        //wrist IE restoration

        DataSet ds = db.selectData("select * from tblbpWrist  where CreatedDate<='10/18/2020'");


        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {

            path = Server.MapPath("~/Template/Restore/WristCC.html");
            _ccValue = File.ReadAllText(path);

            path = Server.MapPath("~/Template/WristCC.html");
            ccOrg = File.ReadAllText(path);

            path = Server.MapPath("~/Template/WristPE.html");
            peValue = peOrg = File.ReadAllText(path);

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            //for (int i = 0; i < 1; i++)
            {

                painValL = ds.Tables[0].Rows[i]["PainScaleLeft"].ToString();
                painValR = ds.Tables[0].Rows[i]["PainScaleRight"].ToString();
                id = ds.Tables[0].Rows[i]["PatientDetail_ID"].ToString();

                ccValue = _ccValue;

                ccValue = ccValue.Replace("#painR", painValR);
                ccValue = ccValue.Replace("#painL", painValL);


                //ccValue = ccValue.Replace("#txtNoteL", ds.Tables[0].Rows[i]["NoteLeft"].ToString());
                //ccValue = ccValue.Replace("#txtNoteR", ds.Tables[0].Rows[i]["NoteRight"].ToString());


                ccValue = ccValue.Replace("#chkulnarL", (ds.Tables[0].Rows[i]["UlnarLeft"].ToString() == "True" ? "checked='checked'" : ""));
                ccValue = ccValue.Replace("#chkulnarR", (ds.Tables[0].Rows[i]["UlnarRight"].ToString() == "True" ? "checked='checked'" : ""));

                ccValue = ccValue.Replace("#chkradiusL", (ds.Tables[0].Rows[i]["RadialLeft"].ToString() == "True" ? "checked='checked'" : ""));
                ccValue = ccValue.Replace("#chkradiusR", (ds.Tables[0].Rows[i]["RadialRight"].ToString() == "True" ? "checked='checked'" : ""));

                ccValue = ccValue.Replace("#chkdorsalL", (ds.Tables[0].Rows[i]["DorsalLeft"].ToString() == "True" ? "checked='checked'" : ""));
                ccValue = ccValue.Replace("#chkdorsalR", (ds.Tables[0].Rows[i]["DorsalRight"].ToString() == "True" ? "checked='checked'" : ""));

                ccValue = ccValue.Replace("#chkpalmarL", (ds.Tables[0].Rows[i]["PalmarLeft"].ToString() == "True" ? "checked='checked'" : ""));
                ccValue = ccValue.Replace("#chkpalmarR", (ds.Tables[0].Rows[i]["PalmarRight"].ToString() == "True" ? "checked='checked'" : ""));

                if (ds.Tables[0].Rows[i]["UlnarLeft"].ToString() == "True" || ds.Tables[0].Rows[i]["RadialLeft"].ToString() == "True"
                    || ds.Tables[0].Rows[i]["DorsalLeft"].ToString() == "True" || ds.Tables[0].Rows[i]["PalmarLeft"].ToString() == "True")
                    ccValue = ccValue.Replace("#chkspecL", "checked='checked'");
                else
                    ccValue = ccValue.Replace("#chkspecL", "");


                if (ds.Tables[0].Rows[i]["UlnarRight"].ToString() == "True" || ds.Tables[0].Rows[i]["RadialRight"].ToString() == "True"
                || ds.Tables[0].Rows[i]["DorsalRight"].ToString() == "True" || ds.Tables[0].Rows[i]["PalmarRight"].ToString() == "True")
                    ccValue = ccValue.Replace("#chkspecR", "checked='checked'");
                else
                    ccValue = ccValue.Replace("#chkspecR", "");


                ccValue = ccValue.Replace("#chkliftingL", (ds.Tables[0].Rows[i]["LiftingObjectLeft"].ToString() == "True" ? "checked='checked'" : ""));
                ccValue = ccValue.Replace("#chkliftingR", (ds.Tables[0].Rows[i]["LiftingObjectRight"].ToString() == "True" ? "checked='checked'" : ""));

                ccValue = ccValue.Replace("#chkrotationL", (ds.Tables[0].Rows[i]["RotationLeft"].ToString() == "True" ? "checked='checked'" : ""));
                ccValue = ccValue.Replace("#chkrotationR", (ds.Tables[0].Rows[i]["RotationRight"].ToString() == "True" ? "checked='checked'" : ""));


                ccValue = ccValue.Replace("#chkmovementL", (ds.Tables[0].Rows[i]["MovementLeft"].ToString() == "True" ? "checked='checked'" : ""));
                ccValue = ccValue.Replace("#chkmovementR", (ds.Tables[0].Rows[i]["MovementRight"].ToString() == "True" ? "checked='checked'" : ""));

                ccValue = ccValue.Replace("#chkworkingL", (ds.Tables[0].Rows[i]["WorkingLeft"].ToString() == "True" ? "checked='checked'" : ""));
                ccValue = ccValue.Replace("#chkworkingR", (ds.Tables[0].Rows[i]["WorkingRight"].ToString() == "True" ? "checked='checked'" : ""));



                if (ds.Tables[0].Rows[i]["LiftingObjectLeft"].ToString() == "True" || ds.Tables[0].Rows[i]["RotationLeft"].ToString() == "True"
                    || ds.Tables[0].Rows[i]["MovementLeft"].ToString() == "True" || ds.Tables[0].Rows[i]["WorkingLeft"].ToString() == "True")
                    ccValue = ccValue.Replace("#chkworsedL", "checked='checked'");
                else
                    ccValue = ccValue.Replace("#chkworsedL", "");


                if (ds.Tables[0].Rows[i]["LiftingObjectRight"].ToString() == "True" || ds.Tables[0].Rows[i]["RotationRight"].ToString() == "True"
                || ds.Tables[0].Rows[i]["MovementRight"].ToString() == "True" || ds.Tables[0].Rows[i]["WorkingRight"].ToString() == "True")
                    ccValue = ccValue.Replace("#chkworsedR", "checked='checked'");
                else
                    ccValue = ccValue.Replace("#chkworsedR", "");

                //      this.WristROM(ds.Tables[0].Rows[i]["FlexionROMLeft"].ToString(), ds.Tables[0].Rows[i]["FlexionROMRight"].ToString(), ds.Tables[0].Rows[i]["PainUponDorsiFlexionLeft"].ToString(),
                //ds.Tables[0].Rows[i]["PainUponDorsiFlexionRight"].ToString(), ds.Tables[0].Rows[i]["UlnarDeviationROMLeft"].ToString(), ds.Tables[0].Rows[i]["UlnarDeviationROMRight"].ToString(),
                //ds.Tables[0].Rows[i]["RadialDeviationROMLeft"].ToString(), ds.Tables[0].Rows[i]["RadialDeviationROMRight"].ToString(),
                //ds.Tables[0].Rows[i]["PatientDetail_ID"].ToString());

                this.updateCCPE(ccValue, ccOrg, peValue, peValue, id, "tblbpWrist");
            }
            lblMess.InnerText = "Total " + ds.Tables[0].Rows.Count + " Restore";
        }

    }

    protected void btnKneeIE_Click(object sender, EventArgs e)
    { //knee IE restoration

        DataSet ds = db.selectData("select * from tblbpKnee  where CreatedDate<='10/18/2020'");


        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {

            path = Server.MapPath("~/Template/Restore/KneeCC.html");
            _ccValue = File.ReadAllText(path);

            path = Server.MapPath("~/Template/KneeCC.html");
            ccOrg = File.ReadAllText(path);

            path = Server.MapPath("~/Template/KneePE.html");
            peValue = peOrg = File.ReadAllText(path);

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            //for (int i = 0; i < 1; i++)
            {

                painValL = ds.Tables[0].Rows[i]["PainScaleLeft"].ToString();
                painValR = ds.Tables[0].Rows[i]["PainScaleRight"].ToString();
                id = ds.Tables[0].Rows[i]["PatientDetail_ID"].ToString();


                ccValue = _ccValue;

                ccValue = ccValue.Replace("#painR", painValR);
                ccValue = ccValue.Replace("#painL", painValL);


                ccValue = ccValue.Replace("#chksquattingL", (ds.Tables[0].Rows[i]["WorseSquattingLeft"].ToString() == "True" ? "checked='checked'" : ""));
                ccValue = ccValue.Replace("#chksquattingR", (ds.Tables[0].Rows[i]["WorseSquattingRight"].ToString() == "True" ? "checked='checked'" : ""));

                ccValue = ccValue.Replace("#chkwalkingL", (ds.Tables[0].Rows[i]["WorseWalkingLeft"].ToString() == "True" ? "checked='checked'" : ""));
                ccValue = ccValue.Replace("#chkwalkingR", (ds.Tables[0].Rows[i]["WorseWalkingRight"].ToString() == "True" ? "checked='checked'" : ""));

                ccValue = ccValue.Replace("#chkclimbingL", (ds.Tables[0].Rows[i]["WorseStairsLeft"].ToString() == "True" ? "checked='checked'" : ""));
                ccValue = ccValue.Replace("#chkclimbingR", (ds.Tables[0].Rows[i]["WorseStairsRight"].ToString() == "True" ? "checked='checked'" : ""));

                ccValue = ccValue.Replace("#txtPainScaleL2", ds.Tables[0].Rows[i]["WorseOtherTextLeft"].ToString());
                ccValue = ccValue.Replace("#txtPainScaleLR", ds.Tables[0].Rows[i]["WorseOtherTextRight"].ToString());


                if (ds.Tables[0].Rows[i]["WorseSquattingLeft"].ToString() == "True" ||
                   ds.Tables[0].Rows[i]["WorseWalkingLeft"].ToString() == "True" ||
                   ds.Tables[0].Rows[i]["WorseStairsLeft"].ToString() == "True" ||
                  !string.IsNullOrEmpty(ds.Tables[0].Rows[i]["WorseOtherTextLeft"].ToString()))
                    ccValue = ccValue.Replace("#chkworsedL", "checked='checked'");
                else
                    ccValue = ccValue.Replace("#chkworsedL", "");


                if (ds.Tables[0].Rows[i]["WorseSquattingRight"].ToString() == "True" ||
                ds.Tables[0].Rows[i]["WorseWalkingRight"].ToString() == "True" ||
                ds.Tables[0].Rows[i]["WorseStairsRight"].ToString() == "True" ||
               !string.IsNullOrEmpty(ds.Tables[0].Rows[i]["WorseOtherTextRight"].ToString()))
                    ccValue = ccValue.Replace("#chkworsedR", "checked='checked'");
                else
                    ccValue = ccValue.Replace("#chkworsedR", "");


                ccValue = ccValue.Replace("#chkrestingR", (ds.Tables[0].Rows[i]["ImprovedRestingRight"].ToString() == "True" ? "checked='checked'" : ""));
                ccValue = ccValue.Replace("#chkrestingL", (ds.Tables[0].Rows[i]["ImprovedRestingLeft"].ToString() == "True" ? "checked='checked'" : ""));

                ccValue = ccValue.Replace("#chkmedicationR", (ds.Tables[0].Rows[i]["ImprovedMedicationRight"].ToString() == "True" ? "checked='checked'" : ""));
                ccValue = ccValue.Replace("#chkmedicationL", (ds.Tables[0].Rows[i]["ImprovedMedicationLeft"].ToString() == "True" ? "checked='checked'" : ""));

                ccValue = ccValue.Replace("#chktherapyR", (ds.Tables[0].Rows[i]["ImprovedTherapyRight"].ToString() == "True" ? "checked='checked'" : ""));
                ccValue = ccValue.Replace("#chktherapyL", (ds.Tables[0].Rows[i]["ImprovedTherapyLeft"].ToString() == "True" ? "checked='checked'" : ""));

                ccValue = ccValue.Replace("#chksleepingR", (ds.Tables[0].Rows[i]["ImprovedSleepingRight"].ToString() == "True" ? "checked='checked'" : ""));
                ccValue = ccValue.Replace("#chksleepingL", (ds.Tables[0].Rows[i]["ImprovedSleepingLeft"].ToString() == "True" ? "checked='checked'" : ""));


                if (ds.Tables[0].Rows[i]["ImprovedRestingLeft"].ToString() == "True" ||
               ds.Tables[0].Rows[i]["ImprovedMedicationLeft"].ToString() == "True" ||
               ds.Tables[0].Rows[i]["ImprovedTherapyLeft"].ToString() == "True" ||
                ds.Tables[0].Rows[i]["ImprovedSleepingLeft"].ToString() == "True"
             )
                    ccValue = ccValue.Replace("#chkimprovedL", "checked='checked'");
                else
                    ccValue = ccValue.Replace("#chkimprovedL", "");


                if (ds.Tables[0].Rows[i]["ImprovedRestingRight"].ToString() == "True" ||
               ds.Tables[0].Rows[i]["ImprovedMedicationRight"].ToString() == "True" ||
               ds.Tables[0].Rows[i]["ImprovedTherapyRight"].ToString() == "True" ||
                ds.Tables[0].Rows[i]["ImprovedSleepingRight"].ToString() == "True"
             )
                    ccValue = ccValue.Replace("#chkimprovedR", "checked='checked'");
                else
                    ccValue = ccValue.Replace("#chkimprovedR", "");

                this.kneeROM(ds.Tables[0].Rows[i]["FlexionROM1"].ToString(), ds.Tables[0].Rows[i]["FlexionROM2"].ToString(), ds.Tables[0].Rows[i]["ExtensionROM1"].ToString(),
                 ds.Tables[0].Rows[i]["ExtensionROM2"].ToString(), ds.Tables[0].Rows[i]["PatientDetail_ID"].ToString());


                this.updateCCPE(ccValue, ccOrg, peValue, peValue, id, "tblbpKnee");
            }
            lblMess.InnerText = "Total " + ds.Tables[0].Rows.Count + " Restore";
        }

    }

    protected void btnHipIE_Click(object sender, EventArgs e)
    {

        //hip IE restoration

        DataSet ds = db.selectData("select * from tblbpHip  where CreatedDate<='10/18/2020'");

        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {

            path = Server.MapPath("~/Template/Restore/HipCC.html");
            _ccValue = File.ReadAllText(path);

            path = Server.MapPath("~/Template/HipCC.html");
            ccOrg = File.ReadAllText(path);

            path = Server.MapPath("~/Template/HipPE.html");
            peValue = peOrg = File.ReadAllText(path);

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            //for (int i = 0; i < 1; i++)
            {

                painValL = ds.Tables[0].Rows[i]["PainScaleLeft"].ToString();
                painValR = ds.Tables[0].Rows[i]["PainScaleRight"].ToString();
                id = ds.Tables[0].Rows[i]["PatientDetail_ID"].ToString();

                ccValue = _ccValue;

                ccValue = ccValue.Replace("#painR", painValR);
                ccValue = ccValue.Replace("#painL", painValL);


                ccValue = ccValue.Replace("#txtNoteL", ds.Tables[0].Rows[i]["NoteLeft"].ToString());
                ccValue = ccValue.Replace("#txtNoteR", ds.Tables[0].Rows[i]["NoteRight"].ToString());


                ccValue = ccValue.Replace("#chkconstantL", (ds.Tables[0].Rows[i]["ConstantLeft"].ToString() == "True" ? "checked='checked'" : ""));
                ccValue = ccValue.Replace("#chkconstantR", (ds.Tables[0].Rows[i]["ConstantRight"].ToString() == "True" ? "checked='checked'" : ""));

                ccValue = ccValue.Replace("#chkintermittentL", (ds.Tables[0].Rows[i]["IntermittentLeft"].ToString() == "True" ? "checked='checked'" : ""));
                ccValue = ccValue.Replace("#chkintermittentR", (ds.Tables[0].Rows[i]["IntermittentRight"].ToString() == "True" ? "checked='checked'" : ""));

                ccValue = ccValue.Replace("#chksharpL", (ds.Tables[0].Rows[i]["SharpLeft"].ToString() == "True" ? "checked='checked'" : ""));
                ccValue = ccValue.Replace("#chksharpR", (ds.Tables[0].Rows[i]["SharpRight"].ToString() == "True" ? "checked='checked'" : ""));

                ccValue = ccValue.Replace("#chkelectricL", (ds.Tables[0].Rows[i]["ElectricLeft"].ToString() == "True" ? "checked='checked'" : ""));
                ccValue = ccValue.Replace("#chkelectricR", (ds.Tables[0].Rows[i]["ElectricRight"].ToString() == "True" ? "checked='checked'" : ""));

                ccValue = ccValue.Replace("#chkshootingL", (ds.Tables[0].Rows[i]["ShootingLeft"].ToString() == "True" ? "checked='checked'" : ""));
                ccValue = ccValue.Replace("#chkshootingR", (ds.Tables[0].Rows[i]["ShootingRight"].ToString() == "True" ? "checked='checked'" : ""));

                ccValue = ccValue.Replace("#chkthrobbingL", (ds.Tables[0].Rows[i]["ThrobblingLeft"].ToString() == "True" ? "checked='checked'" : ""));
                ccValue = ccValue.Replace("#chkthrobbingR", (ds.Tables[0].Rows[i]["ThrobblingRight"].ToString() == "True" ? "checked='checked'" : ""));

                ccValue = ccValue.Replace("#chkpulsatingL", (ds.Tables[0].Rows[i]["PulsatingLeft"].ToString() == "True" ? "checked='checked'" : ""));
                ccValue = ccValue.Replace("#chkpulsatingR", (ds.Tables[0].Rows[i]["PulsatingRight"].ToString() == "True" ? "checked='checked'" : ""));

                ccValue = ccValue.Replace("#chkdullL", (ds.Tables[0].Rows[i]["DullLeft"].ToString() == "True" ? "checked='checked'" : ""));
                ccValue = ccValue.Replace("#chkdullR", (ds.Tables[0].Rows[i]["DullRight"].ToString() == "True" ? "checked='checked'" : ""));

                ccValue = ccValue.Replace("#chkachyL", (ds.Tables[0].Rows[i]["AchyLeft"].ToString() == "True" ? "checked='checked'" : ""));
                ccValue = ccValue.Replace("#chkachyR", (ds.Tables[0].Rows[i]["AchyRight"].ToString() == "True" ? "checked='checked'" : ""));


                if (ds.Tables[0].Rows[i]["WorseSittingLeft"].ToString() == "True" || ds.Tables[0].Rows[i]["WorseStandingLeft"].ToString() == "True"
                    || ds.Tables[0].Rows[i]["WorseMovementLeft"].ToString() == "True" || ds.Tables[0].Rows[i]["WorseActivitiesLeft"].ToString() == "True"
                    || !string.IsNullOrEmpty(ds.Tables[0].Rows[i]["WorseOtherLeft"].ToString()))
                    ccValue = ccValue.Replace("#chkspecL", "checked='checked'");
                else
                    ccValue = ccValue.Replace("#chkspecL", "");

                ccValue = ccValue.Replace("#chksittingL", (ds.Tables[0].Rows[i]["WorseSittingLeft"].ToString() == "True" ? "checked='checked'" : ""));
                ccValue = ccValue.Replace("#chkstandingL", (ds.Tables[0].Rows[i]["WorseStandingLeft"].ToString() == "True" ? "checked='checked'" : ""));
                ccValue = ccValue.Replace("#chkmovementL", (ds.Tables[0].Rows[i]["WorseMovementLeft"].ToString() == "True" ? "checked='checked'" : ""));
                ccValue = ccValue.Replace("#chkactivitiesL", (ds.Tables[0].Rows[i]["WorseActivitiesLeft"].ToString() == "True" ? "checked='checked'" : ""));
                ccValue = ccValue.Replace("#txtspecL", ds.Tables[0].Rows[i]["WorseOtherLeft"].ToString());


                if (ds.Tables[0].Rows[i]["WorseSittingRight"].ToString() == "True" || ds.Tables[0].Rows[i]["WorseStandingRight"].ToString() == "True"
                    || ds.Tables[0].Rows[i]["WorseMovementRight"].ToString() == "True" || ds.Tables[0].Rows[i]["WorseActivitiesRight"].ToString() == "True"
                    || !string.IsNullOrEmpty(ds.Tables[0].Rows[i]["WorseOtherRight"].ToString()))
                    ccValue = ccValue.Replace("#chkspecR", "checked='checked'");
                else
                    ccValue = ccValue.Replace("#chkspecR", "");

                ccValue = ccValue.Replace("#chksittingR", (ds.Tables[0].Rows[i]["WorseSittingRight"].ToString() == "True" ? "checked='checked'" : ""));
                ccValue = ccValue.Replace("#chkstandingR", (ds.Tables[0].Rows[i]["WorseStandingRight"].ToString() == "True" ? "checked='checked'" : ""));
                ccValue = ccValue.Replace("#chkmovementR", (ds.Tables[0].Rows[i]["WorseMovementRight"].ToString() == "True" ? "checked='checked'" : ""));
                ccValue = ccValue.Replace("#chkactivitiesR", (ds.Tables[0].Rows[i]["WorseActivitiesRight"].ToString() == "True" ? "checked='checked'" : ""));
                ccValue = ccValue.Replace("#txtspecR", ds.Tables[0].Rows[i]["WorseOtherRight"].ToString());

                this.updateCCPE(ccValue, ccOrg, peValue, peValue, id, "tblbpHip");
            }

            lblMess.InnerText = "Total " + ds.Tables[0].Rows.Count + " Restore";
        }
    }

    protected void btnAnkleIE_Click(object sender, EventArgs e)
    {
        //ankle IE restoration

        DataSet ds = db.selectData("select * from tblbpAnkle  where CreatedDate<='10/18/2020'");


        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {

            path = Server.MapPath("~/Template/Restore/AnkleCC.html");
            _ccValue = File.ReadAllText(path);

            path = Server.MapPath("~/Template/AnkleCC.html");
            ccOrg = File.ReadAllText(path);

            path = Server.MapPath("~/Template/AnklePE.html");
            peValue = peOrg = File.ReadAllText(path);

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            //for (int i = 0; i < 1; i++)
            {

                painValL = ds.Tables[0].Rows[i]["PainScaleLeft"].ToString();
                painValR = ds.Tables[0].Rows[i]["PainScaleRight"].ToString();
                id = ds.Tables[0].Rows[i]["PatientDetail_ID"].ToString();

                ccValue = _ccValue;

                ccValue = ccValue.Replace("#painR", painValR);
                ccValue = ccValue.Replace("#painL", painValL);

                ccValue = ccValue.Replace("#txtNoteL", ds.Tables[0].Rows[i]["NoteLeft"].ToString());
                ccValue = ccValue.Replace("#txtNoteR", ds.Tables[0].Rows[i]["NoteRight"].ToString());


                ccValue = ccValue.Replace("#chkconstantL", (ds.Tables[0].Rows[i]["ConstantLeft"].ToString() == "True" ? "checked='checked'" : ""));
                ccValue = ccValue.Replace("#chkconstantR", (ds.Tables[0].Rows[i]["ConstantRight"].ToString() == "True" ? "checked='checked'" : ""));

                ccValue = ccValue.Replace("#chkintermittentL", (ds.Tables[0].Rows[i]["IntermittentLeft"].ToString() == "True" ? "checked='checked'" : ""));
                ccValue = ccValue.Replace("#chkintermittentR", (ds.Tables[0].Rows[i]["IntermittentRight"].ToString() == "True" ? "checked='checked'" : ""));

                ccValue = ccValue.Replace("#chksharpL", (ds.Tables[0].Rows[i]["SharpLeft"].ToString() == "True" ? "checked='checked'" : ""));
                ccValue = ccValue.Replace("#chksharpR", (ds.Tables[0].Rows[i]["SharpRight"].ToString() == "True" ? "checked='checked'" : ""));

                ccValue = ccValue.Replace("#chkelectricL", (ds.Tables[0].Rows[i]["ElectricLeft"].ToString() == "True" ? "checked='checked'" : ""));
                ccValue = ccValue.Replace("#chkelectricR", (ds.Tables[0].Rows[i]["ElectricRight"].ToString() == "True" ? "checked='checked'" : ""));

                ccValue = ccValue.Replace("#chkshootingL", (ds.Tables[0].Rows[i]["ShootingLeft"].ToString() == "True" ? "checked='checked'" : ""));
                ccValue = ccValue.Replace("#chkshootingR", (ds.Tables[0].Rows[i]["ShootingRight"].ToString() == "True" ? "checked='checked'" : ""));

                ccValue = ccValue.Replace("#chkthrobbingL", (ds.Tables[0].Rows[i]["ThrobblingLeft"].ToString() == "True" ? "checked='checked'" : ""));
                ccValue = ccValue.Replace("#chkthrobbingR", (ds.Tables[0].Rows[i]["ThrobblingRight"].ToString() == "True" ? "checked='checked'" : ""));

                ccValue = ccValue.Replace("#chkpulsatingL", (ds.Tables[0].Rows[i]["PulsatingLeft"].ToString() == "True" ? "checked='checked'" : ""));
                ccValue = ccValue.Replace("#chkpulsatingR", (ds.Tables[0].Rows[i]["PulsatingRight"].ToString() == "True" ? "checked='checked'" : ""));

                ccValue = ccValue.Replace("#chkdullL", (ds.Tables[0].Rows[i]["DullLeft"].ToString() == "True" ? "checked='checked'" : ""));
                ccValue = ccValue.Replace("#chkdullR", (ds.Tables[0].Rows[i]["DullRight"].ToString() == "True" ? "checked='checked'" : ""));

                ccValue = ccValue.Replace("#chkachyL", (ds.Tables[0].Rows[i]["AchyLeft"].ToString() == "True" ? "checked='checked'" : ""));
                ccValue = ccValue.Replace("#chkachyR", (ds.Tables[0].Rows[i]["AchyRight"].ToString() == "True" ? "checked='checked'" : ""));


                if (ds.Tables[0].Rows[i]["MedMalleolusLeft"].ToString() == "True" || ds.Tables[0].Rows[i]["LatMalleolusLeft"].ToString() == "True"
                    || ds.Tables[0].Rows[i]["AchillesLeft"].ToString() == "True")
                    ccValue = ccValue.Replace("#chkspecL", "checked='checked'");
                else
                    ccValue = ccValue.Replace("#chkspecL", "");

                ccValue = ccValue.Replace("#chkmedialL", (ds.Tables[0].Rows[i]["MedMalleolusLeft"].ToString() == "True" ? "checked='checked'" : ""));
                ccValue = ccValue.Replace("#chklateralL", (ds.Tables[0].Rows[i]["LatMalleolusLeft"].ToString() == "True" ? "checked='checked'" : ""));
                ccValue = ccValue.Replace("#chkachillesL", (ds.Tables[0].Rows[i]["AchillesLeft"].ToString() == "True" ? "checked='checked'" : ""));


                if (ds.Tables[0].Rows[i]["MedMalleolusRight"].ToString() == "True" || ds.Tables[0].Rows[i]["LatMalleolusRight"].ToString() == "True"
                  || ds.Tables[0].Rows[i]["AchillesRight"].ToString() == "True")
                    ccValue = ccValue.Replace("#chkspecR", "checked='checked'");
                else
                    ccValue = ccValue.Replace("#chkspecR", "");

                ccValue = ccValue.Replace("#chkmedialR", (ds.Tables[0].Rows[i]["MedMalleolusRight"].ToString() == "True" ? "checked='checked'" : ""));
                ccValue = ccValue.Replace("#chklateralR", (ds.Tables[0].Rows[i]["LatMalleolusRight"].ToString() == "True" ? "checked='checked'" : ""));
                ccValue = ccValue.Replace("#chkachillesR", (ds.Tables[0].Rows[i]["AchillesRight"].ToString() == "True" ? "checked='checked'" : ""));



                this.updateCCPE(ccValue, ccOrg, peValue, peValue, id, "tblbpAnkle");
            }

            lblMess.InnerText = "Total " + ds.Tables[0].Rows.Count + " Restore";
        }
    }

    protected void btnShoulder_Click(object sender, EventArgs e)
    {

        //shoulder IE restoration

        DataSet ds = db.selectData("select * from tblbpShoulder  where CreatedDate<='10/18/2020'");


        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {

            path = Server.MapPath("~/Template/Restore/ShoulderCC.html");
            _ccValue = File.ReadAllText(path);

            path = Server.MapPath("~/Template/ShoulderCC.html");
            ccOrg = File.ReadAllText(path);

            path = Server.MapPath("~/Template/ShoulderPE.html");
            peValue = peOrg = File.ReadAllText(path);

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            //for (int i = 0; i < 1; i++)
            {

                painValL = ds.Tables[0].Rows[i]["PainScaleLeft"].ToString();
                painValR = ds.Tables[0].Rows[i]["PainScaleRight"].ToString();
                id = ds.Tables[0].Rows[i]["PatientDetail_ID"].ToString();

                ccValue = _ccValue;

                ccValue = ccValue.Replace("#painR", painValR);
                ccValue = ccValue.Replace("#painL", painValL);


                ccValue = ccValue.Replace("#chkraisingL", (ds.Tables[0].Rows[i]["WorseRaisingLeft"].ToString() == "True" ? "checked='checked'" : ""));
                ccValue = ccValue.Replace("#chkraisingR", (ds.Tables[0].Rows[i]["WorseRaisingRight"].ToString() == "True" ? "checked='checked'" : ""));

                ccValue = ccValue.Replace("#chkliftingL", (ds.Tables[0].Rows[i]["WorseLiftingLeft"].ToString() == "True" ? "checked='checked'" : ""));
                ccValue = ccValue.Replace("#chkliftingR", (ds.Tables[0].Rows[i]["WorseLiftingRight"].ToString() == "True" ? "checked='checked'" : ""));

                ccValue = ccValue.Replace("#chkrotationL", (ds.Tables[0].Rows[i]["WorseRotationLeft"].ToString() == "True" ? "checked='checked'" : ""));
                ccValue = ccValue.Replace("#chkrotationR", (ds.Tables[0].Rows[i]["WorseRotationRight"].ToString() == "True" ? "checked='checked'" : ""));

                ccValue = ccValue.Replace("#chkworkingL", (ds.Tables[0].Rows[i]["WorseWorkingLeft"].ToString() == "True" ? "checked='checked'" : ""));
                ccValue = ccValue.Replace("#chkworkingR", (ds.Tables[0].Rows[i]["WorseWorkingRight"].ToString() == "True" ? "checked='checked'" : ""));

                ccValue = ccValue.Replace("#chkoverheadL", (ds.Tables[0].Rows[i]["WorseActivitiesLeft"].ToString() == "True" ? "checked='checked'" : ""));
                ccValue = ccValue.Replace("#chkoverheadR", (ds.Tables[0].Rows[i]["WorseActivitiesRight"].ToString() == "True" ? "checked='checked'" : ""));


                if (ds.Tables[0].Rows[i]["WorseRaisingLeft"].ToString() == "True" || ds.Tables[0].Rows[i]["WorseLiftingLeft"].ToString() == "True"
                    || ds.Tables[0].Rows[i]["WorseRotationLeft"].ToString() == "True" || ds.Tables[0].Rows[i]["WorseWorkingLeft"].ToString() == "True"
                    || ds.Tables[0].Rows[i]["WorseActivitiesLeft"].ToString() == "True")
                    ccValue = ccValue.Replace("#chkworsedL", "checked='checked'");
                else
                    ccValue = ccValue.Replace("#chkworsedL", "");


                if (ds.Tables[0].Rows[i]["WorseRaisingRight"].ToString() == "True" || ds.Tables[0].Rows[i]["WorseLiftingRight"].ToString() == "True"
                  || ds.Tables[0].Rows[i]["WorseRotationRight"].ToString() == "True" || ds.Tables[0].Rows[i]["WorseWorkingRight"].ToString() == "True"
                  || ds.Tables[0].Rows[i]["WorseActivitiesRight"].ToString() == "True")
                    ccValue = ccValue.Replace("#chkworsedR", "checked='checked'");
                else
                    ccValue = ccValue.Replace("#chkworsedR", "");

                ccValue = ccValue.Replace("#chkrestingL", (ds.Tables[0].Rows[i]["ImprovedRestingLeft"].ToString() == "True" ? "checked='checked'" : ""));
                ccValue = ccValue.Replace("#chkrestingR", (ds.Tables[0].Rows[i]["ImprovedRestingRight"].ToString() == "True" ? "checked='checked'" : ""));

                ccValue = ccValue.Replace("#chkmedicationL", (ds.Tables[0].Rows[i]["ImprovedMedicationLeft"].ToString() == "True" ? "checked='checked'" : ""));
                ccValue = ccValue.Replace("#chkmedicationR", (ds.Tables[0].Rows[i]["ImprovedMedicationRight"].ToString() == "True" ? "checked='checked'" : ""));

                ccValue = ccValue.Replace("#chktherapyL", (ds.Tables[0].Rows[i]["ImprovedTherapyLeft"].ToString() == "True" ? "checked='checked'" : ""));
                ccValue = ccValue.Replace("#chktherapyR", (ds.Tables[0].Rows[i]["ImprovedTherapyRight"].ToString() == "True" ? "checked='checked'" : ""));

                ccValue = ccValue.Replace("#chksleepingL", (ds.Tables[0].Rows[i]["ImprovedSleepingLeft"].ToString() == "True" ? "checked='checked'" : ""));
                ccValue = ccValue.Replace("#chksleepingR", (ds.Tables[0].Rows[i]["ImprovedSleepingRight"].ToString() == "True" ? "checked='checked'" : ""));

                if (ds.Tables[0].Rows[i]["ImprovedRestingLeft"].ToString() == "True" || ds.Tables[0].Rows[i]["ImprovedMedicationLeft"].ToString() == "True"
                  || ds.Tables[0].Rows[i]["ImprovedTherapyLeft"].ToString() == "True" || ds.Tables[0].Rows[i]["ImprovedSleepingLeft"].ToString() == "True"
                 )
                    ccValue = ccValue.Replace("#chkimprovedL", "checked='checked'");
                else
                    ccValue = ccValue.Replace("#chkimprovedL", "");

                if (ds.Tables[0].Rows[i]["ImprovedRestingRight"].ToString() == "True" || ds.Tables[0].Rows[i]["ImprovedMedicationRight"].ToString() == "True"
                 || ds.Tables[0].Rows[i]["ImprovedTherapyRight"].ToString() == "True" || ds.Tables[0].Rows[i]["ImprovedSleepingRight"].ToString() == "True"
                )
                    ccValue = ccValue.Replace("#chkimprovedR", "checked='checked'");
                else
                    ccValue = ccValue.Replace("#chkimprovedR", "");





                this.shoulderROM(ds.Tables[0].Rows[i]["AbductionLeft"].ToString(), ds.Tables[0].Rows[i]["FlexionLeft"].ToString(), ds.Tables[0].Rows[i]["ExtRotationLeft"].ToString(),
              ds.Tables[0].Rows[i]["IntRotationLeft"].ToString(), ds.Tables[0].Rows[i]["AbductionRight"].ToString(), ds.Tables[0].Rows[i]["FlexionRight"].ToString(),
              ds.Tables[0].Rows[i]["ExtRotationRight"].ToString(), ds.Tables[0].Rows[i]["IntRotationRight"].ToString(), ds.Tables[0].Rows[i]["PatientDetail_ID"].ToString());

                this.updateCCPE(ccValue, ccOrg, peValue, peValue, id, "tblbpShoulder");
            }

            lblMess.InnerText = "Total " + ds.Tables[0].Rows.Count + " Restore";
        }

    }

    protected void btnMBFU_Click(object sender, EventArgs e)
    {
        //midback FU restoration

        DataSet ds = db.selectData("select * from tblFUbpMidBack  where CreatedDate<='10/18/2020'");


        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {

            path = Server.MapPath("~/Template/Restore/MidbackCC.html");
            _ccValue = File.ReadAllText(path);

            path = Server.MapPath("~/Template/MidbackCC.html");
            ccOrg = File.ReadAllText(path);

            path = Server.MapPath("~/Template/MidbackPE.html");
            peValue = peOrg = File.ReadAllText(path);

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            //for (int i = 0; i < 1; i++)
            {

                painVal = ds.Tables[0].Rows[i]["PainScale"].ToString();
                id = ds.Tables[0].Rows[i]["PatientDetail_ID"].ToString();

                ccValue = _ccValue;

                ccValue = ccValue.Replace("#midbackpain", painVal);

                if (ds.Tables[0].Rows[i]["WorseSitting"].ToString() == "True" || ds.Tables[0].Rows[i]["WorseStanding"].ToString() == "True"
                                     || string.IsNullOrEmpty(ds.Tables[0].Rows[i]["WorseOtherText"].ToString()) == false)
                    ccValue = ccValue.Replace("#chkworsewith", "checked='checked'");
                else
                    ccValue = ccValue.Replace("#chkworsewith", "");

                ccValue = ccValue.Replace("#chksitting", (ds.Tables[0].Rows[i]["WorseSitting"].ToString() == "True" ? "checked='checked'" : ""));
                ccValue = ccValue.Replace("#chkstanding", (ds.Tables[0].Rows[i]["WorseStanding"].ToString() == "True" ? "checked='checked'" : ""));

                ccValue = ccValue.Replace("#txtworsend", ds.Tables[0].Rows[i]["WorseOtherText"].ToString());


                ccValue = ccValue.Replace("#chkresting", (ds.Tables[0].Rows[i]["ImprovedResting"].ToString() == "True" ? "checked='checked'" : ""));
                ccValue = ccValue.Replace("#chkmedication", (ds.Tables[0].Rows[i]["ImprovedMedication"].ToString() == "True" ? "checked='checked'" : ""));
                ccValue = ccValue.Replace("#chktherapy", (ds.Tables[0].Rows[i]["ImprovedTherapy"].ToString() == "True" ? "checked='checked'" : ""));
                ccValue = ccValue.Replace("#chksleeping", (ds.Tables[0].Rows[i]["ImprovedSleeping"].ToString() == "True" ? "checked='checked'" : ""));
                ccValue = ccValue.Replace("#chkmovement", (ds.Tables[0].Rows[i]["ImprovedMovement"].ToString() == "True" ? "checked='checked'" : ""));

                if (ds.Tables[0].Rows[i]["ImprovedResting"].ToString() == "True" || ds.Tables[0].Rows[i]["ImprovedMedication"].ToString() == "True"
                     || ds.Tables[0].Rows[i]["ImprovedTherapy"].ToString() == "True"
                     || ds.Tables[0].Rows[i]["ImprovedSleeping"].ToString() == "True"
                     || ds.Tables[0].Rows[i]["ImprovedMovement"].ToString() == "True")
                    ccValue = ccValue.Replace("#chkimprovedwith", "checked='checked'");
                else
                    ccValue = ccValue.Replace("#chkimprovedwith", "");



                this.updateCCPE(ccValue, ccOrg, peValue, peValue, id, "tblFUbpMidBack");
            }

            lblMess.InnerText = "Total " + ds.Tables[0].Rows.Count + " Restore";
        }

    }

    protected void btnLBFU_Click(object sender, EventArgs e)
    {
        //lowback FU restoration

        DataSet ds = db.selectData("select * from tblFUbpLowBack  where CreatedDate<='10/18/2020'");


        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {

            path = Server.MapPath("~/Template/Restore/LowbackCC.html");
            _ccValue = File.ReadAllText(path);

            path = Server.MapPath("~/Template/LowbackCC.html");
            ccOrg = File.ReadAllText(path);

            path = Server.MapPath("~/Template/LowbackPE.html");
            peValue = peOrg = File.ReadAllText(path);

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            //for (int i = 0; i < 1; i++)
            {

                painVal = ds.Tables[0].Rows[i]["PainScale"].ToString();
                id = ds.Tables[0].Rows[i]["PatientDetail_ID"].ToString();

                ccValue = _ccValue;

                ccValue = ccValue.Replace("#lowbackpain", painVal);


                ccValue = ccValue.Replace("#chkweakness", (string.IsNullOrEmpty(ds.Tables[0].Rows[i]["WeeknessIn"].ToString()) ? "" : "checked='checked'"));
                ccValue = ccValue.Replace("#txtweakness", ds.Tables[0].Rows[i]["WeeknessIn"].ToString());




                if (ds.Tables[0].Rows[i]["WorseSitting"].ToString() == "True" || ds.Tables[0].Rows[i]["WorseStanding"].ToString() == "True"
                    || ds.Tables[0].Rows[i]["WorseLifting"].ToString() == "True"
                    || string.IsNullOrEmpty(ds.Tables[0].Rows[i]["WorseOtherText"].ToString()) == false)
                    ccValue = ccValue.Replace("#chkworsewith", "checked='checked'");
                else
                    ccValue = ccValue.Replace("#chkworsewith", "");

                ccValue = ccValue.Replace("#chksitting", (ds.Tables[0].Rows[i]["WorseSitting"].ToString() == "True" ? "checked='checked'" : ""));
                ccValue = ccValue.Replace("#chkstanding", (ds.Tables[0].Rows[i]["WorseStanding"].ToString() == "True" ? "checked='checked'" : ""));
                ccValue = ccValue.Replace("#chklifting", (ds.Tables[0].Rows[i]["WorseLifting"].ToString() == "True" ? "checked='checked'" : ""));
                ccValue = ccValue.Replace("#txtworsend", ds.Tables[0].Rows[i]["WorseOtherText"].ToString());


                ccValue = ccValue.Replace("#chkresting", (ds.Tables[0].Rows[i]["ImprovedResting"].ToString() == "True" ? "checked='checked'" : ""));
                ccValue = ccValue.Replace("#chkmedication", (ds.Tables[0].Rows[i]["ImprovedMedication"].ToString() == "True" ? "checked='checked'" : ""));
                ccValue = ccValue.Replace("#chktherapy", (ds.Tables[0].Rows[i]["ImprovedTherapy"].ToString() == "True" ? "checked='checked'" : ""));
                ccValue = ccValue.Replace("#chksleeping", (ds.Tables[0].Rows[i]["ImprovedSleeping"].ToString() == "True" ? "checked='checked'" : ""));
                ccValue = ccValue.Replace("#chkmovement", (ds.Tables[0].Rows[i]["ImprovedMovement"].ToString() == "True" ? "checked='checked'" : ""));

                if (ds.Tables[0].Rows[i]["ImprovedResting"].ToString() == "True" || ds.Tables[0].Rows[i]["ImprovedMedication"].ToString() == "True"
                     || ds.Tables[0].Rows[i]["ImprovedTherapy"].ToString() == "True"
                     || ds.Tables[0].Rows[i]["ImprovedSleeping"].ToString() == "True"
                     || ds.Tables[0].Rows[i]["ImprovedMovement"].ToString() == "True")
                    ccValue = ccValue.Replace("#chkimprovedwith", "checked='checked'");
                else
                    ccValue = ccValue.Replace("#chkimprovedwith", "");


                this.neckLBROM(ds.Tables[0].Rows[i]["FwdFlex"].ToString(), ds.Tables[0].Rows[i]["Extension"].ToString(), ds.Tables[0].Rows[i]["RotationRight"].ToString(),
                ds.Tables[0].Rows[i]["RotationLeft"].ToString(), ds.Tables[0].Rows[i]["LateralFlexRight"].ToString(), ds.Tables[0].Rows[i]["LateralFlexLeft"].ToString(),
                ds.Tables[0].Rows[i]["PatientDetail_ID"].ToString(), "LowBack", true);

                this.updateCCPE(ccValue, ccOrg, peValue, peValue, id, "tblFUbpLowBack");
            }
            lblMess.InnerText = "Total " + ds.Tables[0].Rows.Count + " Restore";
        }

    }

    protected void btnSHoulderFU_Click(object sender, EventArgs e)
    {
        //shoulder FU restoration

        DataSet ds = db.selectData("select * from tblFUbpShoulder  where CreatedDate<='10/18/2020'");


        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {

            path = Server.MapPath("~/Template/Restore/ShoulderCC.html");
            _ccValue = File.ReadAllText(path);

            path = Server.MapPath("~/Template/ShoulderCC.html");
            ccOrg = File.ReadAllText(path);

            path = Server.MapPath("~/Template/ShoulderPE.html");
            peValue = peOrg = File.ReadAllText(path);

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            //for (int i = 0; i < 1; i++)
            {

                painValL = ds.Tables[0].Rows[i]["PainScaleLeft"].ToString();
                painValR = ds.Tables[0].Rows[i]["PainScaleRight"].ToString();
                id = ds.Tables[0].Rows[i]["PatientDetail_ID"].ToString();

                ccValue = _ccValue;

                ccValue = ccValue.Replace("#painR", painValR);
                ccValue = ccValue.Replace("#painL", painValL);


                ccValue = ccValue.Replace("#chkraisingL", (ds.Tables[0].Rows[i]["WorseRaisingLeft"].ToString() == "True" ? "checked='checked'" : ""));
                ccValue = ccValue.Replace("#chkraisingR", (ds.Tables[0].Rows[i]["WorseRaisingRight"].ToString() == "True" ? "checked='checked'" : ""));

                ccValue = ccValue.Replace("#chkliftingL", (ds.Tables[0].Rows[i]["WorseLiftingLeft"].ToString() == "True" ? "checked='checked'" : ""));
                ccValue = ccValue.Replace("#chkliftingR", (ds.Tables[0].Rows[i]["WorseLiftingRight"].ToString() == "True" ? "checked='checked'" : ""));

                ccValue = ccValue.Replace("#chkrotationL", (ds.Tables[0].Rows[i]["WorseRotationLeft"].ToString() == "True" ? "checked='checked'" : ""));
                ccValue = ccValue.Replace("#chkrotationR", (ds.Tables[0].Rows[i]["WorseRotationRight"].ToString() == "True" ? "checked='checked'" : ""));

                ccValue = ccValue.Replace("#chkworkingL", (ds.Tables[0].Rows[i]["WorseWorkingLeft"].ToString() == "True" ? "checked='checked'" : ""));
                ccValue = ccValue.Replace("#chkworkingR", (ds.Tables[0].Rows[i]["WorseWorkingRight"].ToString() == "True" ? "checked='checked'" : ""));

                ccValue = ccValue.Replace("#chkoverheadL", (ds.Tables[0].Rows[i]["WorseActivitiesLeft"].ToString() == "True" ? "checked='checked'" : ""));
                ccValue = ccValue.Replace("#chkoverheadR", (ds.Tables[0].Rows[i]["WorseActivitiesRight"].ToString() == "True" ? "checked='checked'" : ""));


                if (ds.Tables[0].Rows[i]["WorseRaisingLeft"].ToString() == "True" || ds.Tables[0].Rows[i]["WorseLiftingLeft"].ToString() == "True"
                    || ds.Tables[0].Rows[i]["WorseRotationLeft"].ToString() == "True" || ds.Tables[0].Rows[i]["WorseWorkingLeft"].ToString() == "True"
                    || ds.Tables[0].Rows[i]["WorseActivitiesLeft"].ToString() == "True")
                    ccValue = ccValue.Replace("#chkworsedL", "checked='checked'");
                else
                    ccValue = ccValue.Replace("#chkworsedL", "");


                if (ds.Tables[0].Rows[i]["WorseRaisingRight"].ToString() == "True" || ds.Tables[0].Rows[i]["WorseLiftingRight"].ToString() == "True"
                  || ds.Tables[0].Rows[i]["WorseRotationRight"].ToString() == "True" || ds.Tables[0].Rows[i]["WorseWorkingRight"].ToString() == "True"
                  || ds.Tables[0].Rows[i]["WorseActivitiesRight"].ToString() == "True")
                    ccValue = ccValue.Replace("#chkworsedR", "checked='checked'");
                else
                    ccValue = ccValue.Replace("#chkworsedR", "");

                ccValue = ccValue.Replace("#chkrestingL", (ds.Tables[0].Rows[i]["ImprovedRestingLeft"].ToString() == "True" ? "checked='checked'" : ""));
                ccValue = ccValue.Replace("#chkrestingR", (ds.Tables[0].Rows[i]["ImprovedRestingRight"].ToString() == "True" ? "checked='checked'" : ""));

                ccValue = ccValue.Replace("#chkmedicationL", (ds.Tables[0].Rows[i]["ImprovedMedicationLeft"].ToString() == "True" ? "checked='checked'" : ""));
                ccValue = ccValue.Replace("#chkmedicationR", (ds.Tables[0].Rows[i]["ImprovedMedicationRight"].ToString() == "True" ? "checked='checked'" : ""));

                ccValue = ccValue.Replace("#chktherapyL", (ds.Tables[0].Rows[i]["ImprovedTherapyLeft"].ToString() == "True" ? "checked='checked'" : ""));
                ccValue = ccValue.Replace("#chktherapyR", (ds.Tables[0].Rows[i]["ImprovedTherapyRight"].ToString() == "True" ? "checked='checked'" : ""));

                ccValue = ccValue.Replace("#chksleepingL", (ds.Tables[0].Rows[i]["ImprovedSleepingLeft"].ToString() == "True" ? "checked='checked'" : ""));
                ccValue = ccValue.Replace("#chksleepingR", (ds.Tables[0].Rows[i]["ImprovedSleepingRight"].ToString() == "True" ? "checked='checked'" : ""));

                if (ds.Tables[0].Rows[i]["ImprovedRestingLeft"].ToString() == "True" || ds.Tables[0].Rows[i]["ImprovedMedicationLeft"].ToString() == "True"
                  || ds.Tables[0].Rows[i]["ImprovedTherapyLeft"].ToString() == "True" || ds.Tables[0].Rows[i]["ImprovedSleepingLeft"].ToString() == "True"
                 )
                    ccValue = ccValue.Replace("#chkimprovedL", "checked='checked'");
                else
                    ccValue = ccValue.Replace("#chkimprovedL", "");

                if (ds.Tables[0].Rows[i]["ImprovedRestingRight"].ToString() == "True" || ds.Tables[0].Rows[i]["ImprovedMedicationRight"].ToString() == "True"
                 || ds.Tables[0].Rows[i]["ImprovedTherapyRight"].ToString() == "True" || ds.Tables[0].Rows[i]["ImprovedSleepingRight"].ToString() == "True"
                )
                    ccValue = ccValue.Replace("#chkimprovedR", "checked='checked'");
                else
                    ccValue = ccValue.Replace("#chkimprovedR", "");


                this.shoulderROM(ds.Tables[0].Rows[i]["AbductionLeft"].ToString(), ds.Tables[0].Rows[i]["FlexionLeft"].ToString(), ds.Tables[0].Rows[i]["ExtRotationLeft"].ToString(),
              ds.Tables[0].Rows[i]["IntRotationLeft"].ToString(), ds.Tables[0].Rows[i]["AbductionRight"].ToString(), ds.Tables[0].Rows[i]["FlexionRight"].ToString(),
              ds.Tables[0].Rows[i]["ExtRotationRight"].ToString(), ds.Tables[0].Rows[i]["IntRotationRight"].ToString(), ds.Tables[0].Rows[i]["PatientDetail_ID"].ToString(), true);

                this.updateCCPE(ccValue, ccOrg, peValue, peValue, id, "tblFUbpShoulder");
            }

            lblMess.InnerText = "Total " + ds.Tables[0].Rows.Count + " Restore";
        }

    }

    protected void btnELbowFU_Click(object sender, EventArgs e)
    {
        //elbow IE restoration

        DataSet ds = db.selectData("select * from tblFUbpElbow  where CreatedDate<='10/18/2020'");


        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {

            path = Server.MapPath("~/Template/Restore/ElbowCC.html");
            _ccValue = File.ReadAllText(path);

            path = Server.MapPath("~/Template/ElbowCC.html");
            ccOrg = File.ReadAllText(path);

            path = Server.MapPath("~/Template/ElbowPE.html");
            peValue = peOrg = File.ReadAllText(path);

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            //for (int i = 0; i < 1; i++)
            {

                painValL = ds.Tables[0].Rows[i]["PainScaleLeft"].ToString();
                painValR = ds.Tables[0].Rows[i]["PainScaleRight"].ToString();
                id = ds.Tables[0].Rows[i]["PatientDetail_ID"].ToString();

                ccValue = _ccValue;


                ccValue = ccValue.Replace("#painR", painValR);
                ccValue = ccValue.Replace("#painL", painValL);


                ccValue = ccValue.Replace("#chkmedialL", (ds.Tables[0].Rows[i]["MedEpicondyleLeft"].ToString() == "True" ? "checked='checked'" : ""));
                ccValue = ccValue.Replace("#chkmedialR", (ds.Tables[0].Rows[i]["MedEpicondyleRight"].ToString() == "True" ? "checked='checked'" : ""));

                ccValue = ccValue.Replace("#chklateralL", (ds.Tables[0].Rows[i]["LatEpicondyleLeft"].ToString() == "True" ? "checked='checked'" : ""));
                ccValue = ccValue.Replace("#chklateralR", (ds.Tables[0].Rows[i]["LatEpicondyleRight"].ToString() == "True" ? "checked='checked'" : ""));

                ccValue = ccValue.Replace("#chkolecranonL", (ds.Tables[0].Rows[i]["OlecranonLeft"].ToString() == "True" ? "checked='checked'" : ""));
                ccValue = ccValue.Replace("#chkolecranonR", (ds.Tables[0].Rows[i]["OlecranonRight"].ToString() == "True" ? "checked='checked'" : ""));


                if (ds.Tables[0].Rows[i]["OlecranonLeft"].ToString() == "True" || ds.Tables[0].Rows[i]["LatEpicondyleLeft"].ToString() == "True"
                    || ds.Tables[0].Rows[i]["MedEpicondyleLeft"].ToString() == "True")
                    ccValue = ccValue.Replace("#chkspecL", "checked='checked'");
                else
                    ccValue = ccValue.Replace("#chkspecL", "");


                if (ds.Tables[0].Rows[i]["OlecranonRight"].ToString() == "True" || ds.Tables[0].Rows[i]["LatEpicondyleRight"].ToString() == "True"
                    || ds.Tables[0].Rows[i]["MedEpicondyleRight"].ToString() == "True")
                    ccValue = ccValue.Replace("#chkspecR", "checked='checked'");
                else
                    ccValue = ccValue.Replace("#chkspecR", "");


                ccValue = ccValue.Replace("#chkraisingL", (ds.Tables[0].Rows[i]["RaisingArmLeft"].ToString() == "True" ? "checked='checked'" : ""));
                ccValue = ccValue.Replace("#chkraisingR", (ds.Tables[0].Rows[i]["RaisingArmRight"].ToString() == "True" ? "checked='checked'" : ""));

                ccValue = ccValue.Replace("#chkliftingL", (ds.Tables[0].Rows[i]["LiftingObjectLeft"].ToString() == "True" ? "checked='checked'" : ""));
                ccValue = ccValue.Replace("#chkliftingR", (ds.Tables[0].Rows[i]["LiftingObjectRight"].ToString() == "True" ? "checked='checked'" : ""));

                ccValue = ccValue.Replace("#chkrotationL", (ds.Tables[0].Rows[i]["RotationLeft"].ToString() == "True" ? "checked='checked'" : ""));
                ccValue = ccValue.Replace("#chkrotationR", (ds.Tables[0].Rows[i]["RotationRight"].ToString() == "True" ? "checked='checked'" : ""));

                ccValue = ccValue.Replace("#chkworkingL", (ds.Tables[0].Rows[i]["WorkingLeft"].ToString() == "True" ? "checked='checked'" : ""));
                ccValue = ccValue.Replace("#chkworkingR", (ds.Tables[0].Rows[i]["WorkingRight"].ToString() == "True" ? "checked='checked'" : ""));

                ccValue = ccValue.Replace("#txtPainScaleL2", ds.Tables[0].Rows[i]["NoteLeft"].ToString());
                ccValue = ccValue.Replace("#txtPainScaleR2", ds.Tables[0].Rows[i]["NoteRight"].ToString());


                if (ds.Tables[0].Rows[i]["RaisingArmLeft"].ToString() == "True" || ds.Tables[0].Rows[i]["LiftingObjectLeft"].ToString() == "True"
                 || ds.Tables[0].Rows[i]["RotationLeft"].ToString() == "True" || ds.Tables[0].Rows[i]["WorkingLeft"].ToString() == "True"
                 || !string.IsNullOrEmpty(ds.Tables[0].Rows[i]["NoteLeft"].ToString()))
                    ccValue = ccValue.Replace("#chkspecL", "checked='checked'");
                else
                    ccValue = ccValue.Replace("#chkspecL", "");


                if (ds.Tables[0].Rows[i]["RaisingArmRight"].ToString() == "True" || ds.Tables[0].Rows[i]["LiftingObjectRight"].ToString() == "True"
               || ds.Tables[0].Rows[i]["RotationRight"].ToString() == "True" || ds.Tables[0].Rows[i]["WorkingRight"].ToString() == "True"
               || !string.IsNullOrEmpty(ds.Tables[0].Rows[i]["NoteRight"].ToString()))
                    ccValue = ccValue.Replace("#chkspecR", "checked='checked'");
                else
                    ccValue = ccValue.Replace("#chkspecR", "");



                //     this.ElbowROM(ds.Tables[0].Rows[i]["FlexionROM1"].ToString(), ds.Tables[0].Rows[i]["FlexionROM2"].ToString(), ds.Tables[0].Rows[i]["ExtensionROM1"].ToString(),
                //ds.Tables[0].Rows[i]["ExtensionROM2"].ToString(), ds.Tables[0].Rows[i]["SupinationROM1"].ToString(), ds.Tables[0].Rows[i]["SupinationROM2"].ToString(),
                //ds.Tables[0].Rows[i]["PronationROM1"].ToString(), ds.Tables[0].Rows[i]["PronationROM2"].ToString(),
                //ds.Tables[0].Rows[i]["PatientDetail_ID"].ToString());

                this.updateCCPE(ccValue, ccOrg, peValue, peValue, id, "tblFUbpElbow");
            }

            lblMess.InnerText = "Total " + ds.Tables[0].Rows.Count + " Restore";
        }
    }

    protected void btnWristFU_Click(object sender, EventArgs e)
    { //wrist FU restoration

        DataSet ds = db.selectData("select * from tblFUbpWrist  where CreatedDate<='10/18/2020'");


        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {

            path = Server.MapPath("~/Template/Restore/WristCC.html");
            _ccValue = File.ReadAllText(path);

            path = Server.MapPath("~/Template/WristCC.html");
            ccOrg = File.ReadAllText(path);

            path = Server.MapPath("~/Template/WristPE.html");
            peValue = peOrg = File.ReadAllText(path);

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            //for (int i = 0; i < 1; i++)
            {

                painValL = ds.Tables[0].Rows[i]["PainScaleLeft"].ToString();
                painValR = ds.Tables[0].Rows[i]["PainScaleRight"].ToString();
                id = ds.Tables[0].Rows[i]["PatientDetail_ID"].ToString();

                ccValue = _ccValue;
                ccValue = ccValue.Replace("#painR", painValR);
                ccValue = ccValue.Replace("#painL", painValL);


                ccValue = ccValue.Replace("#chkulnarL", (ds.Tables[0].Rows[i]["UlnarLeft"].ToString() == "True" ? "checked='checked'" : ""));
                ccValue = ccValue.Replace("#chkulnarR", (ds.Tables[0].Rows[i]["UlnarRight"].ToString() == "True" ? "checked='checked'" : ""));

                ccValue = ccValue.Replace("#chkradiusL", (ds.Tables[0].Rows[i]["RadialLeft"].ToString() == "True" ? "checked='checked'" : ""));
                ccValue = ccValue.Replace("#chkradiusR", (ds.Tables[0].Rows[i]["RadialRight"].ToString() == "True" ? "checked='checked'" : ""));

                ccValue = ccValue.Replace("#chkdorsalL", (ds.Tables[0].Rows[i]["DorsalLeft"].ToString() == "True" ? "checked='checked'" : ""));
                ccValue = ccValue.Replace("#chkdorsalR", (ds.Tables[0].Rows[i]["DorsalRight"].ToString() == "True" ? "checked='checked'" : ""));

                ccValue = ccValue.Replace("#chkpalmarL", (ds.Tables[0].Rows[i]["PalmarLeft"].ToString() == "True" ? "checked='checked'" : ""));
                ccValue = ccValue.Replace("#chkpalmarR", (ds.Tables[0].Rows[i]["PalmarRight"].ToString() == "True" ? "checked='checked'" : ""));

                if (ds.Tables[0].Rows[i]["UlnarLeft"].ToString() == "True" || ds.Tables[0].Rows[i]["RadialLeft"].ToString() == "True"
                    || ds.Tables[0].Rows[i]["DorsalLeft"].ToString() == "True" || ds.Tables[0].Rows[i]["PalmarLeft"].ToString() == "True")
                    ccValue = ccValue.Replace("#chkspecL", "checked='checked'");
                else
                    ccValue = ccValue.Replace("#chkspecL", "");


                if (ds.Tables[0].Rows[i]["UlnarRight"].ToString() == "True" || ds.Tables[0].Rows[i]["RadialRight"].ToString() == "True"
                || ds.Tables[0].Rows[i]["DorsalRight"].ToString() == "True" || ds.Tables[0].Rows[i]["PalmarRight"].ToString() == "True")
                    ccValue = ccValue.Replace("#chkspecR", "checked='checked'");
                else
                    ccValue = ccValue.Replace("#chkspecR", "");


                ccValue = ccValue.Replace("#chkliftingL", (ds.Tables[0].Rows[i]["LiftingObjectLeft"].ToString() == "True" ? "checked='checked'" : ""));
                ccValue = ccValue.Replace("#chkliftingR", (ds.Tables[0].Rows[i]["LiftingObjectRight"].ToString() == "True" ? "checked='checked'" : ""));

                ccValue = ccValue.Replace("#chkrotationL", (ds.Tables[0].Rows[i]["RotationLeft"].ToString() == "True" ? "checked='checked'" : ""));
                ccValue = ccValue.Replace("#chkrotationR", (ds.Tables[0].Rows[i]["RotationRight"].ToString() == "True" ? "checked='checked'" : ""));


                ccValue = ccValue.Replace("#chkmovementL", (ds.Tables[0].Rows[i]["MovementLeft"].ToString() == "True" ? "checked='checked'" : ""));
                ccValue = ccValue.Replace("#chkmovementR", (ds.Tables[0].Rows[i]["MovementRight"].ToString() == "True" ? "checked='checked'" : ""));

                ccValue = ccValue.Replace("#chkworkingL", (ds.Tables[0].Rows[i]["WorkingLeft"].ToString() == "True" ? "checked='checked'" : ""));
                ccValue = ccValue.Replace("#chkworkingR", (ds.Tables[0].Rows[i]["WorkingRight"].ToString() == "True" ? "checked='checked'" : ""));



                if (ds.Tables[0].Rows[i]["LiftingObjectLeft"].ToString() == "True" || ds.Tables[0].Rows[i]["RotationLeft"].ToString() == "True"
                    || ds.Tables[0].Rows[i]["MovementLeft"].ToString() == "True" || ds.Tables[0].Rows[i]["WorkingLeft"].ToString() == "True")
                    ccValue = ccValue.Replace("#chkworsedL", "checked='checked'");
                else
                    ccValue = ccValue.Replace("#chkworsedL", "");


                if (ds.Tables[0].Rows[i]["LiftingObjectRight"].ToString() == "True" || ds.Tables[0].Rows[i]["RotationRight"].ToString() == "True"
                || ds.Tables[0].Rows[i]["MovementRight"].ToString() == "True" || ds.Tables[0].Rows[i]["WorkingRight"].ToString() == "True")
                    ccValue = ccValue.Replace("#chkworsedR", "checked='checked'");
                else
                    ccValue = ccValue.Replace("#chkworsedR", "");

                //      this.WristROM(ds.Tables[0].Rows[i]["FlexionROMLeft"].ToString(), ds.Tables[0].Rows[i]["FlexionROMRight"].ToString(), ds.Tables[0].Rows[i]["PainUponDorsiFlexionLeft"].ToString(),
                //ds.Tables[0].Rows[i]["PainUponDorsiFlexionRight"].ToString(), ds.Tables[0].Rows[i]["UlnarDeviationROMLeft"].ToString(), ds.Tables[0].Rows[i]["UlnarDeviationROMRight"].ToString(),
                //ds.Tables[0].Rows[i]["RadialDeviationROMLeft"].ToString(), ds.Tables[0].Rows[i]["RadialDeviationROMRight"].ToString(),
                //ds.Tables[0].Rows[i]["PatientDetail_ID"].ToString());

                this.updateCCPE(ccValue, ccOrg, peValue, peValue, id, "tblFUbpWrist");
            }
            lblMess.InnerText = "Total " + ds.Tables[0].Rows.Count + " Restore";
        }

    }

    protected void btnKneeFU_Click(object sender, EventArgs e)
    {
        //knee FU restoration

        DataSet ds = db.selectData("select * from tblFUbpKnee  where CreatedDate<='10/18/2020'");


        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {

            path = Server.MapPath("~/Template/Restore/KneeCC.html");
            _ccValue = File.ReadAllText(path);

            path = Server.MapPath("~/Template/KneeCC.html");
            ccOrg = File.ReadAllText(path);

            path = Server.MapPath("~/Template/KneePE.html");
            peValue = peOrg = File.ReadAllText(path);

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            //for (int i = 0; i < 1; i++)
            {

                painValL = ds.Tables[0].Rows[i]["PainScaleLeft"].ToString();
                painValR = ds.Tables[0].Rows[i]["PainScaleRight"].ToString();
                id = ds.Tables[0].Rows[i]["PatientDetail_ID"].ToString();

                ccValue = _ccValue;

                ccValue = ccValue.Replace("#painR", painValR);
                ccValue = ccValue.Replace("#painL", painValL);


                ccValue = ccValue.Replace("#chksquattingL", (ds.Tables[0].Rows[i]["WorseSquattingLeft"].ToString() == "True" ? "checked='checked'" : ""));
                ccValue = ccValue.Replace("#chksquattingR", (ds.Tables[0].Rows[i]["WorseSquattingRight"].ToString() == "True" ? "checked='checked'" : ""));

                ccValue = ccValue.Replace("#chkwalkingL", (ds.Tables[0].Rows[i]["WorseWalkingLeft"].ToString() == "True" ? "checked='checked'" : ""));
                ccValue = ccValue.Replace("#chkwalkingR", (ds.Tables[0].Rows[i]["WorseWalkingRight"].ToString() == "True" ? "checked='checked'" : ""));

                ccValue = ccValue.Replace("#chkclimbingL", (ds.Tables[0].Rows[i]["WorseStairsLeft"].ToString() == "True" ? "checked='checked'" : ""));
                ccValue = ccValue.Replace("#chkclimbingR", (ds.Tables[0].Rows[i]["WorseStairsRight"].ToString() == "True" ? "checked='checked'" : ""));

                ccValue = ccValue.Replace("#txtPainScaleL2", ds.Tables[0].Rows[i]["WorseOtherTextLeft"].ToString());
                ccValue = ccValue.Replace("#txtPainScaleLR", ds.Tables[0].Rows[i]["WorseOtherTextRight"].ToString());


                if (ds.Tables[0].Rows[i]["WorseSquattingLeft"].ToString() == "True" ||
                   ds.Tables[0].Rows[i]["WorseWalkingLeft"].ToString() == "True" ||
                   ds.Tables[0].Rows[i]["WorseStairsLeft"].ToString() == "True" ||
                  !string.IsNullOrEmpty(ds.Tables[0].Rows[i]["WorseOtherTextLeft"].ToString()))
                    ccValue = ccValue.Replace("#chkworsedL", "checked='checked'");
                else
                    ccValue = ccValue.Replace("#chkworsedL", "");


                if (ds.Tables[0].Rows[i]["WorseSquattingRight"].ToString() == "True" ||
                ds.Tables[0].Rows[i]["WorseWalkingRight"].ToString() == "True" ||
                ds.Tables[0].Rows[i]["WorseStairsRight"].ToString() == "True" ||
               !string.IsNullOrEmpty(ds.Tables[0].Rows[i]["WorseOtherTextRight"].ToString()))
                    ccValue = ccValue.Replace("#chkworsedR", "checked='checked'");
                else
                    ccValue = ccValue.Replace("#chkworsedR", "");


                ccValue = ccValue.Replace("#chkrestingR", (ds.Tables[0].Rows[i]["ImprovedRestingRight"].ToString() == "True" ? "checked='checked'" : ""));
                ccValue = ccValue.Replace("#chkrestingL", (ds.Tables[0].Rows[i]["ImprovedRestingLeft"].ToString() == "True" ? "checked='checked'" : ""));

                ccValue = ccValue.Replace("#chkmedicationR", (ds.Tables[0].Rows[i]["ImprovedMedicationRight"].ToString() == "True" ? "checked='checked'" : ""));
                ccValue = ccValue.Replace("#chkmedicationL", (ds.Tables[0].Rows[i]["ImprovedMedicationLeft"].ToString() == "True" ? "checked='checked'" : ""));

                ccValue = ccValue.Replace("#chktherapyR", (ds.Tables[0].Rows[i]["ImprovedTherapyRight"].ToString() == "True" ? "checked='checked'" : ""));
                ccValue = ccValue.Replace("#chktherapyL", (ds.Tables[0].Rows[i]["ImprovedTherapyLeft"].ToString() == "True" ? "checked='checked'" : ""));

                ccValue = ccValue.Replace("#chksleepingR", (ds.Tables[0].Rows[i]["ImprovedSleepingRight"].ToString() == "True" ? "checked='checked'" : ""));
                ccValue = ccValue.Replace("#chksleepingL", (ds.Tables[0].Rows[i]["ImprovedSleepingLeft"].ToString() == "True" ? "checked='checked'" : ""));


                if (ds.Tables[0].Rows[i]["ImprovedRestingLeft"].ToString() == "True" ||
               ds.Tables[0].Rows[i]["ImprovedMedicationLeft"].ToString() == "True" ||
               ds.Tables[0].Rows[i]["ImprovedTherapyLeft"].ToString() == "True" ||
                ds.Tables[0].Rows[i]["ImprovedSleepingLeft"].ToString() == "True"
             )
                    ccValue = ccValue.Replace("#chkimprovedL", "checked='checked'");
                else
                    ccValue = ccValue.Replace("#chkimprovedL", "");


                if (ds.Tables[0].Rows[i]["ImprovedRestingRight"].ToString() == "True" ||
               ds.Tables[0].Rows[i]["ImprovedMedicationRight"].ToString() == "True" ||
               ds.Tables[0].Rows[i]["ImprovedTherapyRight"].ToString() == "True" ||
                ds.Tables[0].Rows[i]["ImprovedSleepingRight"].ToString() == "True"
             )
                    ccValue = ccValue.Replace("#chkimprovedR", "checked='checked'");
                else
                    ccValue = ccValue.Replace("#chkimprovedR", "");


                this.kneeROM(ds.Tables[0].Rows[i]["LEFlexionLeft"].ToString(), ds.Tables[0].Rows[i]["LEFlexionRight"].ToString(), ds.Tables[0].Rows[i]["LEExtensionLeft"].ToString(),
             ds.Tables[0].Rows[i]["LEExtensionRight"].ToString(), ds.Tables[0].Rows[i]["PatientDetail_ID"].ToString(), true);


                this.updateCCPE(ccValue, ccOrg, peValue, peValue, id, "tblFUbpKnee");
            }

            lblMess.InnerText = "Total " + ds.Tables[0].Rows.Count + " Restore";
        }
    }

    protected void btnHipFU_Click(object sender, EventArgs e)
    {
        //hip FU restoration

        DataSet ds = db.selectData("select * from tblFUbpHip  where CreatedDate<='10/18/2020'");

        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {

            path = Server.MapPath("~/Template/Restore/HipCC.html");
            _ccValue = File.ReadAllText(path);

            path = Server.MapPath("~/Template/HipCC.html");
            ccOrg = File.ReadAllText(path);

            path = Server.MapPath("~/Template/HipPE.html");
            peValue = peOrg = File.ReadAllText(path);

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            //for (int i = 0; i < 1; i++)
            {

                painValL = ds.Tables[0].Rows[i]["PainScaleLeft"].ToString();
                painValR = ds.Tables[0].Rows[i]["PainScaleRight"].ToString();
                id = ds.Tables[0].Rows[i]["PatientDetail_ID"].ToString();

                ccValue = _ccValue;

                ccValue = ccValue.Replace("#painR", painValR);
                ccValue = ccValue.Replace("#painL", painValL);

                ccValue = ccValue.Replace("#txtNoteL", ds.Tables[0].Rows[i]["NoteLeft"].ToString());
                ccValue = ccValue.Replace("#txtNoteR", ds.Tables[0].Rows[i]["NoteRight"].ToString());


                ccValue = ccValue.Replace("#chkconstantL", (ds.Tables[0].Rows[i]["ConstantLeft"].ToString() == "True" ? "checked='checked'" : ""));
                ccValue = ccValue.Replace("#chkconstantR", (ds.Tables[0].Rows[i]["ConstantRight"].ToString() == "True" ? "checked='checked'" : ""));

                ccValue = ccValue.Replace("#chkintermittentL", (ds.Tables[0].Rows[i]["IntermittentLeft"].ToString() == "True" ? "checked='checked'" : ""));
                ccValue = ccValue.Replace("#chkintermittentR", (ds.Tables[0].Rows[i]["IntermittentRight"].ToString() == "True" ? "checked='checked'" : ""));

                ccValue = ccValue.Replace("#chksharpL", (ds.Tables[0].Rows[i]["SharpLeft"].ToString() == "True" ? "checked='checked'" : ""));
                ccValue = ccValue.Replace("#chksharpR", (ds.Tables[0].Rows[i]["SharpRight"].ToString() == "True" ? "checked='checked'" : ""));

                ccValue = ccValue.Replace("#chkelectricL", (ds.Tables[0].Rows[i]["ElectricLeft"].ToString() == "True" ? "checked='checked'" : ""));
                ccValue = ccValue.Replace("#chkelectricR", (ds.Tables[0].Rows[i]["ElectricRight"].ToString() == "True" ? "checked='checked'" : ""));

                ccValue = ccValue.Replace("#chkshootingL", (ds.Tables[0].Rows[i]["ShootingLeft"].ToString() == "True" ? "checked='checked'" : ""));
                ccValue = ccValue.Replace("#chkshootingR", (ds.Tables[0].Rows[i]["ShootingRight"].ToString() == "True" ? "checked='checked'" : ""));

                ccValue = ccValue.Replace("#chkthrobbingL", (ds.Tables[0].Rows[i]["ThrobblingLeft"].ToString() == "True" ? "checked='checked'" : ""));
                ccValue = ccValue.Replace("#chkthrobbingR", (ds.Tables[0].Rows[i]["ThrobblingRight"].ToString() == "True" ? "checked='checked'" : ""));

                ccValue = ccValue.Replace("#chkpulsatingL", (ds.Tables[0].Rows[i]["PulsatingLeft"].ToString() == "True" ? "checked='checked'" : ""));
                ccValue = ccValue.Replace("#chkpulsatingR", (ds.Tables[0].Rows[i]["PulsatingRight"].ToString() == "True" ? "checked='checked'" : ""));

                ccValue = ccValue.Replace("#chkdullL", (ds.Tables[0].Rows[i]["DullLeft"].ToString() == "True" ? "checked='checked'" : ""));
                ccValue = ccValue.Replace("#chkdullR", (ds.Tables[0].Rows[i]["DullRight"].ToString() == "True" ? "checked='checked'" : ""));

                ccValue = ccValue.Replace("#chkachyL", (ds.Tables[0].Rows[i]["AchyLeft"].ToString() == "True" ? "checked='checked'" : ""));
                ccValue = ccValue.Replace("#chkachyR", (ds.Tables[0].Rows[i]["AchyRight"].ToString() == "True" ? "checked='checked'" : ""));


                if (ds.Tables[0].Rows[i]["WorseSittingLeft"].ToString() == "True" || ds.Tables[0].Rows[i]["WorseStandingLeft"].ToString() == "True"
                    || ds.Tables[0].Rows[i]["WorseMovementLeft"].ToString() == "True" || ds.Tables[0].Rows[i]["WorseActivitiesLeft"].ToString() == "True"
                    || !string.IsNullOrEmpty(ds.Tables[0].Rows[i]["WorseOtherLeft"].ToString()))
                    ccValue = ccValue.Replace("#chkspecL", "checked='checked'");
                else
                    ccValue = ccValue.Replace("#chkspecL", "");

                ccValue = ccValue.Replace("#chksittingL", (ds.Tables[0].Rows[i]["WorseSittingLeft"].ToString() == "True" ? "checked='checked'" : ""));
                ccValue = ccValue.Replace("#chkstandingL", (ds.Tables[0].Rows[i]["WorseStandingLeft"].ToString() == "True" ? "checked='checked'" : ""));
                ccValue = ccValue.Replace("#chkmovementL", (ds.Tables[0].Rows[i]["WorseMovementLeft"].ToString() == "True" ? "checked='checked'" : ""));
                ccValue = ccValue.Replace("#chkactivitiesL", (ds.Tables[0].Rows[i]["WorseActivitiesLeft"].ToString() == "True" ? "checked='checked'" : ""));
                ccValue = ccValue.Replace("#txtspecL", ds.Tables[0].Rows[i]["WorseOtherLeft"].ToString());


                if (ds.Tables[0].Rows[i]["WorseSittingRight"].ToString() == "True" || ds.Tables[0].Rows[i]["WorseStandingRight"].ToString() == "True"
                    || ds.Tables[0].Rows[i]["WorseMovementRight"].ToString() == "True" || ds.Tables[0].Rows[i]["WorseActivitiesRight"].ToString() == "True"
                    || !string.IsNullOrEmpty(ds.Tables[0].Rows[i]["WorseOtherRight"].ToString()))
                    ccValue = ccValue.Replace("#chkspecR", "checked='checked'");
                else
                    ccValue = ccValue.Replace("#chkspecR", "");

                ccValue = ccValue.Replace("#chksittingR", (ds.Tables[0].Rows[i]["WorseSittingRight"].ToString() == "True" ? "checked='checked'" : ""));
                ccValue = ccValue.Replace("#chkstandingR", (ds.Tables[0].Rows[i]["WorseStandingRight"].ToString() == "True" ? "checked='checked'" : ""));
                ccValue = ccValue.Replace("#chkmovementR", (ds.Tables[0].Rows[i]["WorseMovementRight"].ToString() == "True" ? "checked='checked'" : ""));
                ccValue = ccValue.Replace("#chkactivitiesR", (ds.Tables[0].Rows[i]["WorseActivitiesRight"].ToString() == "True" ? "checked='checked'" : ""));
                ccValue = ccValue.Replace("#txtspecR", ds.Tables[0].Rows[i]["WorseOtherRight"].ToString());


                this.updateCCPE(ccValue, ccOrg, peValue, peValue, id, "tblFUbpHip");
            }
            lblMess.InnerText = "Total " + ds.Tables[0].Rows.Count + " Restore";
        }
    }

    protected void btnAnkleFU_Click(object sender, EventArgs e)
    {
        //ankle FU restoration

        DataSet ds = db.selectData("select * from tblFUbpAnkle  where CreatedDate<='10/18/2020'");


        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {

            path = Server.MapPath("~/Template/Restore/AnkleCC.html");
            _ccValue = File.ReadAllText(path);

            path = Server.MapPath("~/Template/AnkleCC.html");
            ccOrg = File.ReadAllText(path);

            path = Server.MapPath("~/Template/AnklePE.html");
            peValue = peOrg = File.ReadAllText(path);

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            //for (int i = 0; i < 1; i++)
            {

                painValL = ds.Tables[0].Rows[i]["PainScaleLeft"].ToString();
                painValR = ds.Tables[0].Rows[i]["PainScaleRight"].ToString();
                id = ds.Tables[0].Rows[i]["PatientDetail_ID"].ToString();

                ccValue = _ccValue;


                ccValue = ccValue.Replace("#painR", painValR);
                ccValue = ccValue.Replace("#painL", painValL);

                //ccValue = ccValue.Replace("#txtNoteL", ds.Tables[0].Rows[i]["NoteLeft"].ToString());
                //ccValue = ccValue.Replace("#txtNoteR", ds.Tables[0].Rows[i]["NoteRight"].ToString());


                ccValue = ccValue.Replace("#chkconstantL", (ds.Tables[0].Rows[i]["ConstantLeft"].ToString() == "True" ? "checked='checked'" : ""));
                ccValue = ccValue.Replace("#chkconstantR", (ds.Tables[0].Rows[i]["ConstantRight"].ToString() == "True" ? "checked='checked'" : ""));

                ccValue = ccValue.Replace("#chkintermittentL", (ds.Tables[0].Rows[i]["IntermittentLeft"].ToString() == "True" ? "checked='checked'" : ""));
                ccValue = ccValue.Replace("#chkintermittentR", (ds.Tables[0].Rows[i]["IntermittentRight"].ToString() == "True" ? "checked='checked'" : ""));

                ccValue = ccValue.Replace("#chksharpL", (ds.Tables[0].Rows[i]["SharpLeft"].ToString() == "True" ? "checked='checked'" : ""));
                ccValue = ccValue.Replace("#chksharpR", (ds.Tables[0].Rows[i]["SharpRight"].ToString() == "True" ? "checked='checked'" : ""));

                ccValue = ccValue.Replace("#chkelectricL", (ds.Tables[0].Rows[i]["ElectricLeft"].ToString() == "True" ? "checked='checked'" : ""));
                ccValue = ccValue.Replace("#chkelectricR", (ds.Tables[0].Rows[i]["ElectricRight"].ToString() == "True" ? "checked='checked'" : ""));

                ccValue = ccValue.Replace("#chkshootingL", (ds.Tables[0].Rows[i]["ShootingLeft"].ToString() == "True" ? "checked='checked'" : ""));
                ccValue = ccValue.Replace("#chkshootingR", (ds.Tables[0].Rows[i]["ShootingRight"].ToString() == "True" ? "checked='checked'" : ""));

                ccValue = ccValue.Replace("#chkthrobbingL", (ds.Tables[0].Rows[i]["ThrobblingLeft"].ToString() == "True" ? "checked='checked'" : ""));
                ccValue = ccValue.Replace("#chkthrobbingR", (ds.Tables[0].Rows[i]["ThrobblingRight"].ToString() == "True" ? "checked='checked'" : ""));

                ccValue = ccValue.Replace("#chkpulsatingL", (ds.Tables[0].Rows[i]["PulsatingLeft"].ToString() == "True" ? "checked='checked'" : ""));
                ccValue = ccValue.Replace("#chkpulsatingR", (ds.Tables[0].Rows[i]["PulsatingRight"].ToString() == "True" ? "checked='checked'" : ""));

                ccValue = ccValue.Replace("#chkdullL", (ds.Tables[0].Rows[i]["DullLeft"].ToString() == "True" ? "checked='checked'" : ""));
                ccValue = ccValue.Replace("#chkdullR", (ds.Tables[0].Rows[i]["DullRight"].ToString() == "True" ? "checked='checked'" : ""));

                ccValue = ccValue.Replace("#chkachyL", (ds.Tables[0].Rows[i]["AchyLeft"].ToString() == "True" ? "checked='checked'" : ""));
                ccValue = ccValue.Replace("#chkachyR", (ds.Tables[0].Rows[i]["AchyRight"].ToString() == "True" ? "checked='checked'" : ""));


                if (ds.Tables[0].Rows[i]["MedMalleolusLeft"].ToString() == "True" || ds.Tables[0].Rows[i]["LatMalleolusLeft"].ToString() == "True"
                    || ds.Tables[0].Rows[i]["AchillesLeft"].ToString() == "True")
                    ccValue = ccValue.Replace("#chkspecL", "checked='checked'");
                else
                    ccValue = ccValue.Replace("#chkspecL", "");

                ccValue = ccValue.Replace("#chkmedialL", (ds.Tables[0].Rows[i]["MedMalleolusLeft"].ToString() == "True" ? "checked='checked'" : ""));
                ccValue = ccValue.Replace("#chklateralL", (ds.Tables[0].Rows[i]["LatMalleolusLeft"].ToString() == "True" ? "checked='checked'" : ""));
                ccValue = ccValue.Replace("#chkachillesL", (ds.Tables[0].Rows[i]["AchillesLeft"].ToString() == "True" ? "checked='checked'" : ""));


                if (ds.Tables[0].Rows[i]["MedMalleolusRight"].ToString() == "True" || ds.Tables[0].Rows[i]["LatMalleolusRight"].ToString() == "True"
                  || ds.Tables[0].Rows[i]["AchillesRight"].ToString() == "True")
                    ccValue = ccValue.Replace("#chkspecR", "checked='checked'");
                else
                    ccValue = ccValue.Replace("#chkspecR", "");

                ccValue = ccValue.Replace("#chkmedialR", (ds.Tables[0].Rows[i]["MedMalleolusRight"].ToString() == "True" ? "checked='checked'" : ""));
                ccValue = ccValue.Replace("#chklateralR", (ds.Tables[0].Rows[i]["LatMalleolusRight"].ToString() == "True" ? "checked='checked'" : ""));
                ccValue = ccValue.Replace("#chkachillesR", (ds.Tables[0].Rows[i]["AchillesRight"].ToString() == "True" ? "checked='checked'" : ""));


                this.updateCCPE(ccValue, ccOrg, peValue, peValue, id, "tblFUbpAnkle");
            }
            lblMess.InnerText = "Total " + ds.Tables[0].Rows.Count + " Restore";
        }
    }

    protected void btkPhyIE_Click(object sender, EventArgs e)
    {
        try
        {
            string strtop = "";

            //IE restoration

            DataSet ds = db.selectData("select PMH,PSH,Medications,Allergies,PatientIE_ID from tblPatientIEDetailPage2 where PatientIE_ID in (select PatientIE_ID from tblPatientIE where CreatedDate <= '10/18/2020')");




            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString))
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                //for (int i = 0; i < 1; i++)
                {

                    path = Server.MapPath("~/Template/Restore/Page1_top.html");
                    string body = File.ReadAllText(path);

                    strtop = body;



                    strtop = strtop.Replace("#PMH", ds.Tables[0].Rows[i]["PMH"].ToString());
                    strtop = strtop.Replace("#PSH", ds.Tables[0].Rows[i]["PSH"].ToString());
                    strtop = strtop.Replace("#Allergies", ds.Tables[0].Rows[i]["Allergies"].ToString());
                    strtop = strtop.Replace("#Medication", ds.Tables[0].Rows[i]["Medications"].ToString());

                    string query = "update tblPage1HTMLContent set topSectionHTML=@topSectionHTML where PatientIE_ID=@PatientIE_ID";


                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@topSectionHTML", strtop);
                        command.Parameters.AddWithValue("@PatientIE_ID", ds.Tables[0].Rows[i]["PatientIE_ID"].ToString());

                        connection.Open();
                        var results = command.ExecuteNonQuery();
                        connection.Close();
                    }

                }
                lblMess.InnerText = "Total " + ds.Tables[0].Rows.Count + " Restore";
            }





        }
        catch (Exception ex)
        {
        }
    }

    protected void btnPhyFU_Click(object sender, EventArgs e)
    {
        try
        {
            string strtop = "";

            //IE restoration

            DataSet ds = db.selectData("select PSH,PMH,Medications,Allergies,PatientFU_ID from tblFUPatient where CreatedDate<='10/18/2020'");




            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString))
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                //for (int i = 0; i < 1; i++)
                {

                    path = Server.MapPath("~/Template/Restore/Page1_top.html");
                    string body = File.ReadAllText(path);

                    strtop = body;



                    strtop = strtop.Replace("#PMH", ds.Tables[0].Rows[i]["PMH"].ToString());
                    strtop = strtop.Replace("#PSH", ds.Tables[0].Rows[i]["PSH"].ToString());
                    strtop = strtop.Replace("#Allergies", ds.Tables[0].Rows[i]["Allergies"].ToString());
                    strtop = strtop.Replace("#Medication", ds.Tables[0].Rows[i]["Medications"].ToString());

                    string query = "update tblPage1FUHTMLContent set topSectionHTML=@topSectionHTML where PateintFU_ID=@PatientFU_ID";


                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@topSectionHTML", strtop);
                        command.Parameters.AddWithValue("@PatientFU_ID", ds.Tables[0].Rows[i]["PatientFU_ID"].ToString());

                        connection.Open();
                        var results = command.ExecuteNonQuery();
                        connection.Close();
                    }

                }

                lblMess.InnerText = "Total " + ds.Tables[0].Rows.Count + " Restore";
            }





        }
        catch (Exception ex)
        {
            lblMess.InnerText = ex.Message;
        }
    }

    protected void btnROSUpdate_Click(object sender, EventArgs e)
    {
        try
        {
            string strros = "", body = "";

            //neck IE restoration

            DataSet ds = db.selectData("select PatientIE_ID from tblPatientIE where CreatedDate<='10/18/2020'");



            path = Server.MapPath("~/Template/Page2_ros.html");
            body = File.ReadAllText(path);

            strros = body;




            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString))
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    //for (int i = 0; i < 1; i++)
                    {

                        string query = "update  tblPage2HTMLContent set rosSectionHTML=rosSectionHTML where PatientIE_ID=@PatientIE_ID";

                        using (SqlCommand command = new SqlCommand(query, connection))
                        {

                            command.Parameters.AddWithValue("@rosSectionHTML", strros);

                            command.Parameters.AddWithValue("@PatientIE_ID", ds.Tables[0].Rows[i]["PatientIE_ID"].ToString());


                            connection.Open();
                            var results = command.ExecuteNonQuery();
                            connection.Close();
                        }

                    }

                    lblMess.InnerText = "Total " + ds.Tables[0].Rows.Count + " Restore";
                }
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void btnPage3Update_Click(object sender, EventArgs e)
    {
        //try
        //{
        string strtop = "", strpage3 = "", path = "";

        //IE restoration

        // DataSet ds = db.selectData("select * from tblPatientIEDetailPage2");
        DataSet ds = db.selectData("select * from tblPatientIEDetailPage2 where PatientIE_ID in (select PatientIE_ID from tblPatientIE where CreatedDate <= '10/18/2020')");





        path = Server.MapPath("~/Template/Page3_top.html");
        string body = File.ReadAllText(path);

        strtop = body;


        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString))
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)

                {

                    path = Server.MapPath("~/Template/Restore/Page3.html");
                    body = File.ReadAllText(path);
                    strpage3 = body;

                    strpage3 = strpage3.Replace("#LTricepstxt", ds.Tables[0].Rows[i]["DTRtricepsLeft"].ToString());
                    strpage3 = strpage3.Replace("#RTricepstxt", ds.Tables[0].Rows[i]["DTRtricepsRight"].ToString());
                    strpage3 = strpage3.Replace("#LBicepstxt", ds.Tables[0].Rows[i]["DTRtricepsLeft"].ToString());
                    strpage3 = strpage3.Replace("#RBicepstxt", ds.Tables[0].Rows[i]["DTRBicepsRight"].ToString());
                    strpage3 = strpage3.Replace("#LBrachioradialis", ds.Tables[0].Rows[i]["DTRBrachioLeft"].ToString());
                    strpage3 = strpage3.Replace("#RBrachioradialis", ds.Tables[0].Rows[i]["DTRBrachioRight"].ToString());


                    strpage3 = strpage3.Replace("#LLateralarm", ds.Tables[0].Rows[i]["UEC5Left"].ToString());
                    strpage3 = strpage3.Replace("#RLateralarm", ds.Tables[0].Rows[i]["UEC5Right"].ToString());
                    strpage3 = strpage3.Replace("#LLateralforearm", ds.Tables[0].Rows[i]["UEC6Left"].ToString());
                    strpage3 = strpage3.Replace("#RLateralforearm", ds.Tables[0].Rows[i]["UEC6Right"].ToString());
                    strpage3 = strpage3.Replace("#LMiddlefinger", ds.Tables[0].Rows[i]["UEC7Left"].ToString());
                    strpage3 = strpage3.Replace("#RMiddlefinger", ds.Tables[0].Rows[i]["UEC7Right"].ToString());
                    strpage3 = strpage3.Replace("#LMidialForearm", ds.Tables[0].Rows[i]["UEC8Left"].ToString());
                    strpage3 = strpage3.Replace("#RMidialForearm", ds.Tables[0].Rows[i]["UEC8Right"].ToString());
                    strpage3 = strpage3.Replace("#LMidialarm", ds.Tables[0].Rows[i]["UET1Left"].ToString());
                    strpage3 = strpage3.Replace("#RMidialarm", ds.Tables[0].Rows[i]["UET1Right"].ToString());
                    strpage3 = strpage3.Replace("#LCervical", ds.Tables[0].Rows[i]["UECervicalParaspinalsLeft"].ToString());
                    strpage3 = strpage3.Replace("#RCervical", ds.Tables[0].Rows[i]["UECervicalParaspinalsRight"].ToString());
                    strpage3 = strpage3.Replace("#LtxtDMTL3", ds.Tables[0].Rows[i]["LEL3Left"].ToString());
                    strpage3 = strpage3.Replace("#RtxtDMTL3", ds.Tables[0].Rows[i]["LEL3Right"].ToString());
                    strpage3 = strpage3.Replace("#LtxtMLFL4", ds.Tables[0].Rows[i]["LEL4Left"].ToString());
                    strpage3 = strpage3.Replace("#RtxtMLFL4", ds.Tables[0].Rows[i]["LEL4Right"].ToString());
                    strpage3 = strpage3.Replace("#LtxtDOFL5", ds.Tables[0].Rows[i]["LEL5Left"].ToString());
                    strpage3 = strpage3.Replace("#RtxtDOFL5", ds.Tables[0].Rows[i]["LEL5Right"].ToString());
                    strpage3 = strpage3.Replace("#LtxtLTS1", ds.Tables[0].Rows[i]["LES1Left"].ToString());
                    strpage3 = strpage3.Replace("#RtxtLTS1", ds.Tables[0].Rows[i]["LES1Right"].ToString());
                    strpage3 = strpage3.Replace("#LtxtLP", ds.Tables[0].Rows[i]["LELumberParaspinalsLeft"].ToString());
                    strpage3 = strpage3.Replace("#RtxtLP", ds.Tables[0].Rows[i]["LELumberParaspinalsRight"].ToString());


                    strpage3 = strpage3.Replace("#LAbduction", ds.Tables[0].Rows[i]["UEShoulderAbductionLeft"].ToString());
                    strpage3 = strpage3.Replace("#RAbduction", ds.Tables[0].Rows[i]["UEShoulderAbductionRight"].ToString());
                    strpage3 = strpage3.Replace("#LFlexion", ds.Tables[0].Rows[i]["UEShoulderFlexionLeft"].ToString());
                    strpage3 = strpage3.Replace("#RFlexion", ds.Tables[0].Rows[i]["UEShoulderFlexionRight"].ToString());
                    strpage3 = strpage3.Replace("#LElbowExtension", ds.Tables[0].Rows[i]["UEElbowExtensionLeft"].ToString());
                    strpage3 = strpage3.Replace("#RElbowExtension", ds.Tables[0].Rows[i]["UEElbowExtensionRight"].ToString());
                    strpage3 = strpage3.Replace("#LElbowFlexion", ds.Tables[0].Rows[i]["UEElbowFlexionLeft"].ToString());
                    strpage3 = strpage3.Replace("#RElbowFlexion", ds.Tables[0].Rows[i]["UEElbowFlexionRight"].ToString());
                    strpage3 = strpage3.Replace("#LSupination", ds.Tables[0].Rows[i]["UEElbowSupinationLeft"].ToString());
                    strpage3 = strpage3.Replace("#RSupination", ds.Tables[0].Rows[i]["UEElbowSupinationRight"].ToString());
                    strpage3 = strpage3.Replace("#LPronation", ds.Tables[0].Rows[i]["UEElbowPronationLeft"].ToString());
                    strpage3 = strpage3.Replace("#RPronation", ds.Tables[0].Rows[i]["UEElbowPronationRight"].ToString());
                    strpage3 = strpage3.Replace("#LWristFlexion", ds.Tables[0].Rows[i]["UEWristFlexionLeft"].ToString());
                    strpage3 = strpage3.Replace("#RWristFlexion", ds.Tables[0].Rows[i]["UEWristFlexionRight"].ToString());
                    strpage3 = strpage3.Replace("#LWristExtension", ds.Tables[0].Rows[i]["UEWristExtensionLeft"].ToString());
                    strpage3 = strpage3.Replace("#RWristExtension", ds.Tables[0].Rows[i]["UEWristExtensionRight"].ToString());
                    strpage3 = strpage3.Replace("#LGrip", ds.Tables[0].Rows[i]["UEHandGripStrengthLeft"].ToString());
                    strpage3 = strpage3.Replace("#RGrip", ds.Tables[0].Rows[i]["UEHandGripStrengthRight"].ToString());
                    strpage3 = strpage3.Replace("#LFinger", ds.Tables[0].Rows[i]["UEHandFingerAbductorsLeft"].ToString());
                    strpage3 = strpage3.Replace("#RFinger", ds.Tables[0].Rows[i]["UEHandFingerAbductorsRight"].ToString());
                    strpage3 = strpage3.Replace("#LHipFlexion", ds.Tables[0].Rows[i]["LEHipFlexionLeft"].ToString());
                    strpage3 = strpage3.Replace("#RHipFlexion", ds.Tables[0].Rows[i]["LEHipFlexionRight"].ToString());
                    strpage3 = strpage3.Replace("#LHipAbduction", ds.Tables[0].Rows[i]["LEHipAbductionLeft"].ToString());
                    strpage3 = strpage3.Replace("#RHipAbduction", ds.Tables[0].Rows[i]["LEHipAbductionRight"].ToString());
                    strpage3 = strpage3.Replace("#LKneeExtension", ds.Tables[0].Rows[i]["LEKneeExtensionLeft"].ToString());
                    strpage3 = strpage3.Replace("#RKneeExtension", ds.Tables[0].Rows[i]["LEKneeExtensionRight"].ToString());
                    strpage3 = strpage3.Replace("#LKneeFlexion", ds.Tables[0].Rows[i]["LEKneeFlexionLeft"].ToString());
                    strpage3 = strpage3.Replace("#RKneeFlexion", ds.Tables[0].Rows[i]["LEKneeFlexionRight"].ToString());
                    strpage3 = strpage3.Replace("#LDorsiflexion", ds.Tables[0].Rows[i]["LEAnkleDorsiLeft"].ToString());
                    strpage3 = strpage3.Replace("#RDorsiflexion", ds.Tables[0].Rows[i]["LEAnkleDorsiRight"].ToString());
                    strpage3 = strpage3.Replace("#LPlantar", ds.Tables[0].Rows[i]["LEAnklePlantarLeft"].ToString());
                    strpage3 = strpage3.Replace("#RPlantar", ds.Tables[0].Rows[i]["LEAnklePlantarRight"].ToString());
                    strpage3 = strpage3.Replace("#LExtensor", ds.Tables[0].Rows[i]["LEAnkleExtensorLeft"].ToString());
                    strpage3 = strpage3.Replace("#RExtensor", ds.Tables[0].Rows[i]["LEAnkleExtensorRight"].ToString());

                    strpage3 = strpage3.Replace("#txtIntactExcept", ds.Tables[0].Rows[i]["intactexcept"].ToString());
                    strpage3 = strpage3.Replace("#txtSensory", ds.Tables[0].Rows[i]["Sensory"].ToString());

                    strpage3 = strpage3.Replace("#LKnee", ds.Tables[0].Rows[i]["DTRKneeLeft"].ToString());
                    strpage3 = strpage3.Replace("#RKnee", ds.Tables[0].Rows[i]["DTRKneeRight"].ToString());
                    strpage3 = strpage3.Replace("#LAnkle", ds.Tables[0].Rows[i]["DTRAnkleLeft"].ToString());
                    strpage3 = strpage3.Replace("#RAnkle", ds.Tables[0].Rows[i]["DTRAnkleRight"].ToString());



                    string query = "update tblPage3HTMLContent set HTMLContent=@HTMLContent where PatientIE_ID=@PatientIE_ID";


                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        command.Parameters.AddWithValue("@HTMLContent", strpage3);

                        command.Parameters.AddWithValue("@PatientIE_ID", ds.Tables[0].Rows[i]["PatientIE_ID"].ToString());



                        connection.Open();
                        var results = command.ExecuteNonQuery();
                        connection.Close();
                    }

                }

                lblMess.InnerText = "Total " + ds.Tables[0].Rows.Count + " Restore";
            }
        }



        //FU restoration

        // ds = db.selectData("select * from tblFUNeurologicalExam");
        ds = db.selectData("select * from tblFUNeurologicalExam where PatientFU_ID in (select PatientFU_ID from tblFUPatient where CreatedDate <= '10/18/2020')");

        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString))
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                // for (int i = 0; i < 2; i++)
                {


                    path = Server.MapPath("~/Template/Restore/Page3.html");
                    body = File.ReadAllText(path);
                    strpage3 = body;

                    strpage3 = strpage3.Replace("#LTricepstxt", ds.Tables[0].Rows[i]["DTRtricepsLeft"].ToString());
                    strpage3 = strpage3.Replace("#RTricepstxt", ds.Tables[0].Rows[i]["DTRtricepsRight"].ToString());
                    strpage3 = strpage3.Replace("#LBicepstxt", ds.Tables[0].Rows[i]["DTRtricepsLeft"].ToString());
                    strpage3 = strpage3.Replace("#RBicepstxt", ds.Tables[0].Rows[i]["DTRBicepsRight"].ToString());
                    strpage3 = strpage3.Replace("#LBrachioradialis", ds.Tables[0].Rows[i]["DTRBrachioLeft"].ToString());
                    strpage3 = strpage3.Replace("#RBrachioradialis", ds.Tables[0].Rows[i]["DTRBrachioRight"].ToString());
                    strpage3 = strpage3.Replace("#LKnee", ds.Tables[0].Rows[i]["DTRKneeLeft"].ToString());
                    strpage3 = strpage3.Replace("#RKnee", ds.Tables[0].Rows[i]["DTRKneeRight"].ToString());
                    strpage3 = strpage3.Replace("#LAnkle", ds.Tables[0].Rows[i]["DTRAnkleLeft"].ToString());
                    strpage3 = strpage3.Replace("#RAnkle", ds.Tables[0].Rows[i]["DTRAnkleRight"].ToString());

                    strpage3 = strpage3.Replace("#LLateralarm", ds.Tables[0].Rows[i]["UEC5Left"].ToString());
                    strpage3 = strpage3.Replace("#RLateralarm", ds.Tables[0].Rows[i]["UEC5Right"].ToString());
                    strpage3 = strpage3.Replace("#LLateralforearm", ds.Tables[0].Rows[i]["UEC6Left"].ToString());
                    strpage3 = strpage3.Replace("#RLateralforearm", ds.Tables[0].Rows[i]["UEC6Right"].ToString());
                    strpage3 = strpage3.Replace("#LMiddlefinger", ds.Tables[0].Rows[i]["UEC7Left"].ToString());
                    strpage3 = strpage3.Replace("#RMiddlefinger", ds.Tables[0].Rows[i]["UEC7Right"].ToString());
                    strpage3 = strpage3.Replace("#LMidialForearm", ds.Tables[0].Rows[i]["UEC8Left"].ToString());
                    strpage3 = strpage3.Replace("#RMidialForearm", ds.Tables[0].Rows[i]["UEC8Right"].ToString());
                    strpage3 = strpage3.Replace("#LMidialarm", ds.Tables[0].Rows[i]["UET1Left"].ToString());
                    strpage3 = strpage3.Replace("#RMidialarm", ds.Tables[0].Rows[i]["UET1Right"].ToString());
                    strpage3 = strpage3.Replace("#LCervical", ds.Tables[0].Rows[i]["UECervicalParaspinalsLeft"].ToString());
                    strpage3 = strpage3.Replace("#RCervical", ds.Tables[0].Rows[i]["UECervicalParaspinalsRight"].ToString());
                    strpage3 = strpage3.Replace("#LtxtDMTL3", ds.Tables[0].Rows[i]["LEL3Left"].ToString());
                    strpage3 = strpage3.Replace("#RtxtDMTL3", ds.Tables[0].Rows[i]["LEL3Right"].ToString());
                    strpage3 = strpage3.Replace("#LtxtMLFL4", ds.Tables[0].Rows[i]["LEL4Left"].ToString());
                    strpage3 = strpage3.Replace("#RtxtMLFL4", ds.Tables[0].Rows[i]["LEL4Right"].ToString());
                    strpage3 = strpage3.Replace("#LtxtDOFL5", ds.Tables[0].Rows[i]["LEL5Left"].ToString());
                    strpage3 = strpage3.Replace("#RtxtDOFL5", ds.Tables[0].Rows[i]["LEL5Right"].ToString());
                    strpage3 = strpage3.Replace("#LtxtLTS1", ds.Tables[0].Rows[i]["LES1Left"].ToString());
                    strpage3 = strpage3.Replace("#RtxtLTS1", ds.Tables[0].Rows[i]["LES1Right"].ToString());
                    strpage3 = strpage3.Replace("#LtxtLP", ds.Tables[0].Rows[i]["LELumberParaspinalsLeft"].ToString());
                    strpage3 = strpage3.Replace("#RtxtLP", ds.Tables[0].Rows[i]["LELumberParaspinalsRight"].ToString());


                    strpage3 = strpage3.Replace("#LAbduction", ds.Tables[0].Rows[i]["UEShoulderAbductionLeft"].ToString());
                    strpage3 = strpage3.Replace("#RAbduction", ds.Tables[0].Rows[i]["UEShoulderAbductionRight"].ToString());
                    strpage3 = strpage3.Replace("#LFlexion", ds.Tables[0].Rows[i]["UEShoulderFlexionLeft"].ToString());
                    strpage3 = strpage3.Replace("#RFlexion", ds.Tables[0].Rows[i]["UEShoulderFlexionRight"].ToString());
                    strpage3 = strpage3.Replace("#LElbowExtension", ds.Tables[0].Rows[i]["UEElbowExtensionLeft"].ToString());
                    strpage3 = strpage3.Replace("#RElbowExtension", ds.Tables[0].Rows[i]["UEElbowExtensionRight"].ToString());
                    strpage3 = strpage3.Replace("#LElbowFlexion", ds.Tables[0].Rows[i]["UEElbowFlexionLeft"].ToString());
                    strpage3 = strpage3.Replace("#RElbowFlexion", ds.Tables[0].Rows[i]["UEElbowFlexionRight"].ToString());
                    strpage3 = strpage3.Replace("#LSupination", ds.Tables[0].Rows[i]["UEElbowSupinationLeft"].ToString());
                    strpage3 = strpage3.Replace("#RSupination", ds.Tables[0].Rows[i]["UEElbowSupinationRight"].ToString());
                    strpage3 = strpage3.Replace("#LPronation", ds.Tables[0].Rows[i]["UEElbowPronationLeft"].ToString());
                    strpage3 = strpage3.Replace("#RPronation", ds.Tables[0].Rows[i]["UEElbowPronationRight"].ToString());
                    strpage3 = strpage3.Replace("#LWristFlexion", ds.Tables[0].Rows[i]["UEWristFlexionLeft"].ToString());
                    strpage3 = strpage3.Replace("#RWristFlexion", ds.Tables[0].Rows[i]["UEWristFlexionRight"].ToString());
                    strpage3 = strpage3.Replace("#LWristExtension", ds.Tables[0].Rows[i]["UEWristExtensionLeft"].ToString());
                    strpage3 = strpage3.Replace("#RWristExtension", ds.Tables[0].Rows[i]["UEWristExtensionRight"].ToString());
                    strpage3 = strpage3.Replace("#LGrip", ds.Tables[0].Rows[i]["UEHandGripStrengthLeft"].ToString());
                    strpage3 = strpage3.Replace("#RGrip", ds.Tables[0].Rows[i]["UEHandGripStrengthRight"].ToString());
                    strpage3 = strpage3.Replace("#LFinger", ds.Tables[0].Rows[i]["UEHandFingerAbductorsLeft"].ToString());
                    strpage3 = strpage3.Replace("#RFinger", ds.Tables[0].Rows[i]["UEHandFingerAbductorsRight"].ToString());
                    strpage3 = strpage3.Replace("#LHipFlexion", ds.Tables[0].Rows[i]["LEHipFlexionLeft"].ToString());
                    strpage3 = strpage3.Replace("#RHipFlexion", ds.Tables[0].Rows[i]["LEHipFlexionRight"].ToString());
                    strpage3 = strpage3.Replace("#LHipAbduction", ds.Tables[0].Rows[i]["LEHipAbductionLeft"].ToString());
                    strpage3 = strpage3.Replace("#RHipAbduction", ds.Tables[0].Rows[i]["LEHipAbductionRight"].ToString());
                    strpage3 = strpage3.Replace("#LKneeExtension", ds.Tables[0].Rows[i]["LEKneeExtensionLeft"].ToString());
                    strpage3 = strpage3.Replace("#RKneeExtension", ds.Tables[0].Rows[i]["LEKneeExtensionRight"].ToString());
                    strpage3 = strpage3.Replace("#LKneeFlexion", ds.Tables[0].Rows[i]["LEKneeFlexionLeft"].ToString());
                    strpage3 = strpage3.Replace("#RKneeFlexion", ds.Tables[0].Rows[i]["LEKneeFlexionRight"].ToString());
                    strpage3 = strpage3.Replace("#LDorsiflexion", ds.Tables[0].Rows[i]["LEAnkleDorsiLeft"].ToString());
                    strpage3 = strpage3.Replace("#RDorsiflexion", ds.Tables[0].Rows[i]["LEAnkleDorsiRight"].ToString());
                    strpage3 = strpage3.Replace("#LPlantar", ds.Tables[0].Rows[i]["LEAnklePlantarLeft"].ToString());
                    strpage3 = strpage3.Replace("#RPlantar", ds.Tables[0].Rows[i]["LEAnklePlantarRight"].ToString());
                    strpage3 = strpage3.Replace("#LExtensor", ds.Tables[0].Rows[i]["LEAnkleExtensorLeft"].ToString());
                    strpage3 = strpage3.Replace("#RExtensor", ds.Tables[0].Rows[i]["LEAnkleExtensorRight"].ToString());

                    strpage3 = strpage3.Replace("#txtIntactExcept", ds.Tables[0].Rows[i]["intactexcept"].ToString());
                    strpage3 = strpage3.Replace("#txtSensory", ds.Tables[0].Rows[i]["Sensory"].ToString());


                    string query = "update tblPage3FUHTMLContent set HTMLContent=@HTMLContent where PatientFU_ID=@PatientFU_ID";


                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        command.Parameters.AddWithValue("@HTMLContent", strpage3);

                        command.Parameters.AddWithValue("@PatientFU_ID", ds.Tables[0].Rows[i]["PatientFU_ID"].ToString());

                        connection.Open();
                        var results = command.ExecuteNonQuery();
                        connection.Close();
                    }

                }
                lblMess.InnerText = "Total " + ds.Tables[0].Rows.Count + " Restore";
            }

        }
        //}
        //catch (Exception ex)
        //{
        //}
    }

    protected void btnActivity_Click(object sender, EventArgs e)
    {
        try
        {
            string strtop = "", path = "";

            //neck IE restoration

            DataSet ds = db.selectData("select PateintFU_ID from tblPage1FUHTMLContent");

            path = Server.MapPath("~/Template/Page2_activity_affected.html");
            string body = File.ReadAllText(path);

            strtop = body;





            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString))
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    //for (int i = 0; i < 1; i++)
                    {

                        string query = "update tblPage1FUHTMLContent set activityEffectedHTML=@activityEffectedHTML  where PateintFU_ID=@PatientFU_ID";

                        using (SqlCommand command = new SqlCommand(query, connection))
                        {

                            command.Parameters.AddWithValue("@PatientFU_ID", ds.Tables[0].Rows[i]["PateintFU_ID"].ToString());
                            command.Parameters.AddWithValue("@activityEffectedHTML", strtop);


                            connection.Open();
                            var results = command.ExecuteNonQuery();
                            connection.Close();
                        }

                    }
                }
                lblMess.InnerText = "Total " + ds.Tables[0].Rows.Count + " Restore";
            }
        }
        catch (Exception ex)
        {

        }
    }
}