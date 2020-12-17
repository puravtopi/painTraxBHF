using IntakeSheet.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Xml;
using System.IO;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Configuration;

public partial class AddFU : System.Web.UI.Page
{
    public int PatientIE_Id;
    DBHelperClass db = new DBHelperClass();
    int count = 0;
    private bool Chk_r_Shoulder = false;
    private bool Chk_L_Shoulder = false;
    private bool Chk_Neck = false;
    private bool Chk_Midback = false;
    private bool Chk_lowback = false;
    private bool Chk_r_Keen = false;
    private bool Chk_L_Keen = false;
    private bool Chk_r_Elbow = false;
    private bool Chk_l_Elbow = false;
    private bool Chk_r_Wrist = false;
    private bool Chk_l_Wrist = false;
    private bool Chk_r_Hip = false;
    private bool Chk_l_Hip = false;
    private bool Chk_r_ankle = false;
    private bool Chk_l_ankle = false;
    private static string redirectpage = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        //if (Session["UserId"] == null)
        //{
        //    Response.Redirect("~/Login.aspx");
        //}
        //
        //ddd
        //Session["patientFUId"] = "";
        if (Session["uname"] == null)
            Response.Redirect("Login.aspx");
        if (!Page.IsPostBack)
        {
            if (Request.UrlReferrer != null) { redirectpage = Convert.ToString(System.IO.Path.GetFileName(Request.UrlReferrer.AbsolutePath)); }

            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[1] { new DataColumn("WorkStatus", typeof(string)) });
            dt.Rows.Add("Able to go back to work");
            dt.Rows.Add("Working");
            dt.Rows.Add("Not Working");
            dt.Rows.Add("Partially Working");
            Repeater1.DataSource = dt;
            Repeater1.DataBind();

            bindLocations();
            bindInsuranceCo();
            bindDropdown();

            if (Request.QueryString["PID"] != null)
            {

                PatientIE_Id = Convert.ToInt32(Request.QueryString["PID"]);
                Session["PatientIE_Id2"] = PatientIE_Id;
                Session["PatientIE_ID"] = Session["PatientIE_Id2"];
                bindFUDetails(PatientIE_Id);
                hfPatientIEId.Value = PatientIE_Id.ToString();
                Session["bodyPartsList"] = null;

                hfPatientFUId.Value = GetFUID(Convert.ToInt64(hfPatientIEId.Value != "" ? hfPatientIEId.Value : "0")).ToString();

                AddFollowUpMaster master = (AddFollowUpMaster)this.Master;
                if (hfPatientFUId.Value == "0")
                {
                    master.bindData("", Session["PatientIE_Id2"].ToString());
                    bindPatientDetails(PatientIE_Id);
                }
                else
                {
                    master.bindData(hfPatientFUId.Value, "");
                    bindPatientDetails(PatientIE_Id, Convert.ToInt32(hfPatientFUId.Value), false);
                }




                if (!string.IsNullOrEmpty(Convert.ToString(Session["patientFUId"])) && !redirectpage.Equals("PatientIntakeList.aspx"))
                {
                    bindRestrictions(Convert.ToInt32(hfPatientIEId.Value), Convert.ToString(Session["patientFUId"]));
                }

                //  Session["patientFUId"] = GetFUID(Convert.ToInt64(hfPatientIEId.Value != "" ? hfPatientIEId.Value : "0")).ToString();

            }
            else if (Session["PatientIE_Id2"] != null)
            {

                hfPatientIEId.Value = Session["PatientIE_Id2"].ToString();
                hfPatientFUId.Value = Session["patientFUId"].ToString();
                Session["PatientIE_ID"] = Session["PatientIE_Id2"];
                bindFUDetails(Convert.ToInt32(hfPatientIEId.Value));

                AddFollowUpMaster master = (AddFollowUpMaster)this.Master;
                //master.bindData(Session["PatientIE_Id2"].ToString());
                master.bindData(hfPatientFUId.Value);

                bindPatientDetails((int)Session["PatientIE_Id2"], Convert.ToInt32(hfPatientFUId.Value));

                if (!string.IsNullOrEmpty(Convert.ToString(Session["patientFUId"])) && !redirectpage.Equals("PatientIntakeList.aspx"))
                {
                    bindRestrictions(Convert.ToInt32(Session["PatientIE_ID"]), Convert.ToString(Session["patientFUId"]));
                }

                //bindRestrictions(Convert.ToInt32(Session["PatientIE_ID"]));
                // Session["patientFUId"] = GetFUID(Convert.ToInt64(hfPatientIEId.Value != "" ? hfPatientIEId.Value : "0")).ToString();
                //hfPatientFUId.Value = GetFUID(Convert.ToInt64(hfPatientIEId.Value != "" ? hfPatientIEId.Value : "0")).ToString();
            }
            else
                PatientIE_Id = 0;

            bindHTMLEdit();

            if (Session["Location"] != null)
            {
                ddlLoaction.SelectedValue = Convert.ToString(Session["Location"]);
            }
            if (Session["Providers"] != null)
            {
                txtMAProviders.Text = Convert.ToString(Session["Providers"]);
            }

        }
        //PatientIntime.Text = DateTime.Now.ToString();
    }

    protected void bindLocations()
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString))
        {
            SqlCommand cmd = new SqlCommand("nusp_GetLocations", con);
            cmd.CommandType = CommandType.StoredProcedure;
            con.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {
                if (!string.IsNullOrEmpty(sdr["Location"].ToString()))
                    ddlLoaction.Items.Add(new ListItem(sdr["Location"].ToString(), sdr["Location_ID"].ToString()));
            }
            con.Close();
        }

        //using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString))
        //{
        //    SqlCommand cmd = new SqlCommand("SELECT DISTINCT Compensation FROM tblPatientIE WHERE (Compensation <> '') OR (Compensation <> NULL) Order By Compensation", con);
        //    cmd.CommandType = CommandType.Text;
        //    con.Open();
        //    SqlDataReader sdr = cmd.ExecuteReader();
        //    while (sdr.Read())
        //    {
        //        if (!string.IsNullOrEmpty(sdr["Compensation"].ToString()))
        //            ddlLoaction.Items.Add(new ListItem(sdr["Compensation"].ToString(), sdr["Compensation"].ToString()));
        //    }
        //    con.Close();


        //}


        //DataSet ds = db.selectData("SELECT DISTINCT Compensation FROM tblPatientIE WHERE (Compensation <> '') OR (Compensation <> NULL) Order By Compensation");
        //if (ds.Tables[0].Rows.Count > 0)
        //{
        //    ddl_casetype.DataValueField = "Compensation";
        //    ddl_casetype.DataTextField = "Compensation";

        //    ddl_casetype.DataSource = ds;
        //    ddl_casetype.DataBind();

        //    ddl_casetype.Items.Insert(0, new ListItem("-- Case Type --", "0"));
        //}
        XmlDocument doc = new XmlDocument();
        doc.Load(Server.MapPath("~/xml/HSMData.xml"));

        foreach (XmlNode node in doc.SelectNodes("//HSM/Compensations/Compensation"))
        {
            ddl_casetype.Items.Add(new ListItem(node.Attributes["name"].InnerText, node.Attributes["name"].InnerText));
        }
    }

    protected void bindInsuranceCo()
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString))
        {
            SqlCommand cmd = new SqlCommand("nusp_GetInsCo", con);
            cmd.CommandType = CommandType.StoredProcedure;
            con.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {
                if (!string.IsNullOrEmpty(sdr["InsCo"].ToString()))
                    ddlInsuranceCo.Items.Add(new ListItem(sdr["InsCo"].ToString(), sdr["InsCo_ID"].ToString()));
            }
            con.Close();

            ddlInsuranceCo.Items.Insert(0, new ListItem("Select Insurance Co", "0"));
        }
    }

    protected void bindFUDetails(int PatientIE_Id)
    {
        BusinessLogic bl = new BusinessLogic();
        gvPatientFUDetails.DataSource = bl.GetFUDetails(PatientIE_Id);
        gvPatientFUDetails.DataBind();
    }

    protected void bindPatientDetails(int PatientIE_Id = 0, int PatientFU_ID = 0, bool isforwad = true)
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString))
        {
            int Patient_Id = 0;
            SqlCommand cmd = new SqlCommand("GetFUdetails", con);
            cmd.Parameters.Add("@PatientIE_Id", PatientIE_Id);
            cmd.CommandType = CommandType.StoredProcedure;
            con.Open();
            SqlDataReader sdr = cmd.ExecuteReader();


            while (sdr.Read())
            {
                // if (!sdr.IsDBNull(0))
                //{
                hfPatientId.Value = sdr["Patient_ID"].ToString();
                //bindRestrictions(PatientIE_Id);
                // hfPatientFUId.Value = sdr["PatientFU_ID"].ToString();
                //PMH.Text = sdr["PMH"].ToString();
                //PSH.Text = sdr["PSH"].ToString();
                //Medication.Text = sdr["Medications"].ToString();
                //Allergies.Text = sdr["Allergies"].ToString();
                FamilyHistory.Text = sdr["FamilyHistory"].ToString();
                Session["FirstNameFU"] = txtFirstName.Text = sdr["FirstName"].ToString();
                Session["LastNameFU"] = txtLastName.Text = sdr["LastName"].ToString();
                ddlLoaction.SelectedValue = sdr["Location_ID"].ToString();
                if (!redirectpage.Equals("PatientIntakeList.aspx"))
                { txtDOV.Text = (sdr["DOE"] != DBNull.Value) ? Convert.ToDateTime(sdr["DOE"]).ToString("MM/dd/yyyy") : ""; }
                else
                { txtDOV.Text = DateTime.Now.ToString("MM/dd/yyyy"); }

                txtDOA.Text = (sdr["DOA"] != DBNull.Value) ? Convert.ToDateTime(sdr["DOA"]).ToString("MM/dd/yyyy") : "";
                txtDOB.Text = (sdr["DOB"] != DBNull.Value) ? Convert.ToDateTime(sdr["DOB"]).ToString("MM/dd/yyyy") : "";
                rblGender.SelectedValue = sdr["Sex"].ToString();
                Session["Gender"] = sdr["Sex"].ToString();
                txtSSN.Text = sdr["SSN"].ToString();
                txtHomePh.Text = sdr["Phone"].ToString();
                txtWorkPh.Text = sdr["work_phone"].ToString();
                txtMobile.Text = sdr["Phone2"].ToString();
                txtAddress.Text = Convert.ToString(sdr["Address1"].ToString());
                txtCity.Text = sdr["City"].ToString();
                txtNote.Text = sdr["Note"].ToString();

                chkDeniessmoking.Checked = !string.IsNullOrEmpty(sdr["DeniesSmoking"].ToString()) ? Convert.ToBoolean(sdr["DeniesSmoking"].ToString()) : false;
                chkDeniesdrinking.Checked = !string.IsNullOrEmpty(sdr["DeniesDrinking"].ToString()) ? Convert.ToBoolean(sdr["DeniesDrinking"].ToString()) : false;
                chkDeniesdrugs.Checked = !string.IsNullOrEmpty(sdr["DeniesDrugs"].ToString()) ? Convert.ToBoolean(sdr["DeniesDrugs"].ToString()) : false;
                chkSocialdrinking.Checked = !string.IsNullOrEmpty(sdr["DeniesSocialDrinking"].ToString()) ? Convert.ToBoolean(sdr["DeniesSocialDrinking"].ToString()) : false;


                if (isforwad)
                {
                    cboReturnToWork.Text = Convert.ToString(sdr["ReturnToWork"]);
                    cboRecevingPhyTherapy.Text = Convert.ToString(sdr["RecevingPhyTherapy"]);
                    cboFeelPainRelief.Text = Convert.ToString(sdr["FeelPainRelief"]);




                    if (PatientFU_ID > 0)
                    {
                        cbonote4.Text = Convert.ToString(sdr["Note4"]);
                        cbonote5.Text = Convert.ToString(sdr["Note5"]);
                    }
                }


                string filePath = Server.MapPath("~/Xml/USStates.xml");
                using (DataSet ds = new DataSet())
                {
                    ds.ReadXml(filePath);
                    ddState.DataSource = ds;
                    ddState.DataTextField = "name";
                    ddState.DataValueField = "name";
                    ddState.DataBind();
                    DataTable Store = ds.Tables["state"];
                    var result = from dm in Store.AsEnumerable()
                                 where ((dm.Field<string>("name") == sdr["State"].ToString()))
                                 select dm;
                    if (result.Any())
                    {
                        ddState.SelectedValue = sdr["State"].ToString();
                    }
                    else
                    { ddState.Items.Insert(0, new ListItem("--Select State--", "0")); }
                }

                txtZip.Text = sdr["Zip"].ToString();
                txtClaim.Text = sdr["ClaimNumber"].ToString();
                txtPolicy.Text = sdr["policy_no"].ToString();
                ddl_casetype.ClearSelection();
                string compensation = Convert.ToString(sdr["Compensation"]);
                if (!string.IsNullOrEmpty(compensation))
                    ddl_casetype.Items.FindByValue(compensation).Selected = true;
                txtAttorneyName.Text = sdr["Attorney"].ToString();
                txtAttorneyPh.Text = sdr["Telephone"].ToString();
                if (!string.IsNullOrEmpty(sdr["InsCo_ID"].ToString()))
                    ddlInsuranceCo.SelectedValue = sdr["InsCo_ID"].ToString();

                Session["FirstNameIE"] = txtFirstName.Text;
                Session["LastNameIE"] = txtLastName.Text;
                Session["DVLbl"] = txtDOV.Text;
                Session["LocLbl"] = ddlLoaction.SelectedItem.Text;
                Session["compensation"] = ddl_casetype.Text;
            }
            //}
            con.Close();
            bindInjuredBodyParts(PatientIE_Id, PatientFU_ID);
        }
    }

    protected void bindInjuredBodyParts(int PatientIE_Id = 0, int PatientFU_Id = 0)
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString))
        {
            SqlCommand cmd = null;
            if (PatientFU_Id == 0)
            {
                cmd = new SqlCommand("nusp_GetInjuredBodyParts", con);
                cmd.Parameters.Add("@PatientIE_ID", PatientIE_Id);
            }
            else
            {
                cmd = new SqlCommand("nusp_GetInjuredBodyParts_FU", con);
                cmd.Parameters.Add("@PatientFU_ID", PatientFU_Id);
            }


            cmd.CommandType = CommandType.StoredProcedure;
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                chk_Neck.Checked = Convert.ToBoolean(dr["Neck"]);
                chk_Midback.Checked = Convert.ToBoolean(dr["MidBack"]);
                chk_lowback.Checked = Convert.ToBoolean(dr["LowBack"]);
                chk_L_Shoulder.Checked = Convert.ToBoolean(dr["LeftShoulder"]);
                chk_r_Shoulder.Checked = Convert.ToBoolean(dr["RightShoulder"]);
                chk_L_Keen.Checked = Convert.ToBoolean(dr["LeftKnee"]);
                chk_r_Keen.Checked = Convert.ToBoolean(dr["RightKnee"]);
                chk_l_Elbow.Checked = Convert.ToBoolean(dr["LeftElbow"]);
                chk_r_Elbow.Checked = Convert.ToBoolean(dr["RightElbow"]);
                chk_l_Wrist.Checked = Convert.ToBoolean(dr["LeftWrist"]);
                chk_r_Wrist.Checked = Convert.ToBoolean(dr["RightWrist"]);
                chk_l_Hip.Checked = Convert.ToBoolean(dr["LeftHip"]);
                chk_r_Hip.Checked = Convert.ToBoolean(dr["RightHip"]);
                chk_l_ankle.Checked = Convert.ToBoolean(dr["LeftAnkle"]);
                chk_r_ankle.Checked = Convert.ToBoolean(dr["RightAnkle"]);

            }
            con.Close();
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString))
            {
                int PatientIE_Id = 0;
                string FreeForm = GetFreeForm();
                SqlCommand cmd = new SqlCommand("nusp_SavePatientDetails", con);
                cmd.Parameters.Add("@Patient_Id", hfPatientId.Value);
                cmd.Parameters.Add("@PatientIE_ID", hfPatientIEId.Value);
                cmd.Parameters.Add("@FreeForm", FreeForm);
                //if (!string.IsNullOrEmpty(hfPatientFUId.Value))
                //{
                //    cmd.Parameters.Add("@PatientFU_ID", hfPatientFUId.Value);
                //    cmd.Parameters.Add("@isEditFU", true);
                //}
                if (!string.IsNullOrEmpty(Session["patientFUId"].ToString()))
                {
                    cmd.Parameters.Add("@PatientFU_ID", Session["patientFUId"].ToString());
                    cmd.Parameters.Add("@isEditFU", true);
                }

                cmd.Parameters.Add("@DOE", DateTime.ParseExact(txtDOV.Text, "MM/dd/yyyy", null));
                if (!string.IsNullOrEmpty(txtDOA.Text.Trim()))
                    cmd.Parameters.Add("@DOA", DateTime.ParseExact(txtDOA.Text, "MM/dd/yyyy", null));
                if (!string.IsNullOrEmpty(txtDOB.Text.Trim()))
                    cmd.Parameters.Add("@DOB", DateTime.ParseExact(txtDOB.Text, "MM/dd/yyyy", null));
                cmd.Parameters.Add("@FirstName", CultureInfo.CurrentCulture.TextInfo.ToTitleCase(txtFirstName.Text.Trim().ToLower()));
                cmd.Parameters.Add("@LastName", CultureInfo.CurrentCulture.TextInfo.ToTitleCase(txtLastName.Text.Trim().ToLower()));
                cmd.Parameters.Add("@Location_ID", ddlLoaction.SelectedValue);
                //if (!string.IsNullOrEmpty(txtDOA.Text.Trim()))
                //    cmd.Parameters.Add("@DOA", Convert.ToDateTime(txtDOA.Text.Trim()));
                cmd.Parameters.Add("@Sex", rblGender.SelectedValue);
                // cmd.Parameters.Add("@DOB", Convert.ToDateTime(txtDOB.Text.Trim()));
                cmd.Parameters.Add("@SSN", txtSSN.Text.Trim());
                cmd.Parameters.Add("@Phone", txtHomePh.Text.Trim());
                cmd.Parameters.Add("@work_phone", txtWorkPh.Text.Trim());
                cmd.Parameters.Add("@Phone2", txtMobile.Text.Trim());
                cmd.Parameters.Add("@Address", txtAddress.Text.Trim());
                cmd.Parameters.Add("@City", txtCity.Text.Trim());
                cmd.Parameters.Add("@Note", txtNote.Text.Trim());
                cmd.Parameters.Add("@State", ddState.SelectedValue.Trim());
                cmd.Parameters.Add("@Zip", txtZip.Text.Trim());
                cmd.Parameters.Add("@InsCo_ID", ddlInsuranceCo.SelectedValue);
                cmd.Parameters.Add("@ClaimNumber", txtClaim.Text.Trim());
                cmd.Parameters.Add("@policy_no", txtPolicy.Text.Trim());
                cmd.Parameters.Add("@Compensation", ddl_casetype.SelectedValue);
                cmd.Parameters.Add("@Attorney", txtAttorneyName.Text.Trim());
                cmd.Parameters.Add("@Telephone", txtAttorneyPh.Text.Trim());
                cmd.Parameters.Add("@MA_Providers", txtMAProviders.Text.Trim());
                cmd.Parameters.Add("@FollowedUpOn", txtFollowedUpOn.Text.Trim());
                //cmd.Parameters.Add("@PMH", PMH.Text.Trim());
                //cmd.Parameters.Add("@PSH", PSH.Text.Trim());
                //cmd.Parameters.Add("@Medications", Medication.Text.Trim());
                //cmd.Parameters.Add("@Allergies", Allergies.Text.Trim());
                cmd.Parameters.Add("@PMH", "");
                cmd.Parameters.Add("@PSH", "");
                cmd.Parameters.Add("@Medications", "");
                cmd.Parameters.Add("@Allergies", "");
                cmd.Parameters.Add("@FamilyHistory", FamilyHistory.Text.Trim());
                cmd.Parameters.Add("@Neck", chk_Neck.Checked);
                cmd.Parameters.Add("@MidBack", chk_Midback.Checked);
                cmd.Parameters.Add("@LowBack", chk_lowback.Checked);
                cmd.Parameters.Add("@LShoulder", chk_L_Shoulder.Checked);
                cmd.Parameters.Add("@RShoulder", chk_r_Shoulder.Checked);
                cmd.Parameters.Add("@cbLKNee", chk_L_Keen.Checked);
                cmd.Parameters.Add("@cbRKnee", chk_r_Keen.Checked);
                cmd.Parameters.Add("@cbLElbow", chk_l_Elbow.Checked);
                cmd.Parameters.Add("@cbRElbow", chk_r_Elbow.Checked);
                cmd.Parameters.Add("@cbLWrist", chk_l_Wrist.Checked);
                cmd.Parameters.Add("@cbRWrist", chk_r_Wrist.Checked);
                cmd.Parameters.Add("@cblAnkle", chk_l_ankle.Checked);
                cmd.Parameters.Add("@cbRAnkle", chk_r_ankle.Checked);
                cmd.Parameters.Add("@cbLHip", chk_l_Hip.Checked);
                cmd.Parameters.Add("@cbRHip", chk_r_Hip.Checked);

                cmd.Parameters.Add("@DeniesSmoking", chkDeniessmoking.Checked);
                cmd.Parameters.Add("@DeniesDrinking", chkDeniesdrinking.Checked);
                cmd.Parameters.Add("@DeniesDrugs", chkDeniesdrugs.Checked);
                cmd.Parameters.Add("@DeniesSocialDrinking", chkSocialdrinking.Checked);
                cmd.Parameters.Add("@Vitals", txtVitals.Text);
                cmd.Parameters.Add("@PatientIntime", PatientIntime.Text);
                cmd.Parameters.Add("@PatientOuttime", PatientOuttime.Text);


                cmd.Parameters.Add("@ReturnToWork", cboReturnToWork.Text);
                cmd.Parameters.Add("@RecevingPhyTherapy", cboRecevingPhyTherapy.Text);
                cmd.Parameters.Add("@FeelPainRelief", cboFeelPainRelief.Text);
                cmd.Parameters.Add("@Note4", cbonote4.Text);
                cmd.Parameters.Add("@Note5", cbonote5.Text);

                SqlParameter outDirectoryId = new SqlParameter("@PatientFUIDS", SqlDbType.Int) { Direction = ParameterDirection.Output };
                cmd.Parameters.Add(outDirectoryId);


                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                int count = cmd.ExecuteNonQuery();







                string alertText = "Follow Up already exsists for this date. Edit the followup";
                if (count > 0)
                {
                    // hfPatientFUId.Value = count.ToString();
                    //Session["patientFUIdnewadded"] = GetFUID(Convert.ToInt64(hfPatientIEId.Value != "" ? hfPatientIEId.Value : "0")).ToString();
                    //hfPatientFUId.Value = GetFUID(Convert.ToInt64(hfPatientIEId.Value != "" ? hfPatientIEId.Value : "0")).ToString();
                    //alertText = "Follow Up Added Successfully";

                    int DirectoryId = outDirectoryId.Value != null && !string.IsNullOrEmpty(Convert.ToString(outDirectoryId.Value)) ? Convert.ToInt32(outDirectoryId.Value) : 0;
                    if (DirectoryId > 0)
                    {
                        Session["patientFUId"] = Convert.ToString(DirectoryId);
                    }

                    alertText = "Follow Up Added Successfully";
                    hfPatientFUId.Value = Session["patientFUId"].ToString();
                    savePage3FU(Session["patientFUId"].ToString());
                    string query = "select top 1 * from tblPage1FUHTMLContent where PateintFU_ID=" + (!string.IsNullOrEmpty(Session["patientFUId"].ToString()) ? Session["patientFUId"].ToString() : "0");
                    DataSet ds = db.selectData(query);
                    if (ds.Tables[0].Rows.Count == 0)
                    {
                        query = "insert into tblPage1FUHTMLContent values(@degreeSectionHTML,@socialSectionHTML,@PatientIE_ID,@PatientFU_ID,@topSectionHTML,@activityEffectedHTML)";
                    }
                    else
                    {
                        query = "update tblPage1FUHTMLContent set degreeSectionHTML=@degreeSectionHTML,socialSectionHTML=@socialSectionHTML,topSectionHTML=@topSectionHTML,activityEffectedHTML=@activityEffectedHTML where PateintFU_ID=@PatientFU_ID";
                    }

                    using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString))
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@PatientIE_ID", Session["PatientIE_ID"].ToString());
                        command.Parameters.AddWithValue("@PatientFU_ID", Session["patientFUId"].ToString());
                        command.Parameters.AddWithValue("@degreeSectionHTML", hddegreeHTMLContent.Value);
                        command.Parameters.AddWithValue("@socialSectionHTML", hdsocialHTMLContent.Value);
                        command.Parameters.AddWithValue("@topSectionHTML", hdtopHTMLContent.Value);
                        command.Parameters.AddWithValue("@activityEffectedHTML", hdactivityeffectedHTMLContent.Value);


                        connection.Open();
                        var results = command.ExecuteNonQuery();
                        connection.Close();
                    }

                }
                con.Close();
                AddFollowUpMaster master = (AddFollowUpMaster)this.Master;

                if (Session["patientFUId"] == null)
                    master.bindData(hfPatientIEId.Value);
                else
                    master.bindData(Session["patientFUId"].ToString());

                if (pageHDN.Value != null)
                {
                    Response.Redirect(Convert.ToString(pageHDN.Value));
                }


            }
        }
        catch (Exception ex)
        {

        }
    }

    private void savePage3FU(string patientFUId)
    {
        try
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString))
            {
                string FreeForm = GetFreeForm();
                SqlCommand cmd = new SqlCommand("usp_saveFUPage3", con);
                cmd.Parameters.Add("@PatientIE_ID", Session["PatientIE_ID"].ToString());
                cmd.Parameters.Add("@PatientFU_ID", patientFUId);

                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }
        catch (Exception ex)
        { }
    }


    protected string GetFreeForm()
    {
        string restrictions = string.Empty;
        foreach (ListItem s in cblRestictions.Items)
        {
            if (s.Selected)
            {
                restrictions += s.Value.ToLower() + ", ";
            }
        }
        string workStatus = string.Empty;
        foreach (RepeaterItem item in this.Repeater1.Items)
        {
            CheckBox chk = item.FindControl("cblWorkStatus") as CheckBox;
            if (chk.Checked)
            {
                string collageName = (item.FindControl("txtCollageName") as TextBox).Text;
                string degree = chk.Text;
                workStatus += degree.ToLower() + "-" + collageName.ToLower() + ", ";
            }
        }

        string FreeForm = string.Empty;
        if (rblDOD.SelectedIndex > -1)
        {
            FreeForm = "Degree of Disability^ " + rblDOD.SelectedValue;
        }
        if (!string.IsNullOrEmpty(restrictions))
        {
            if (!string.IsNullOrEmpty(FreeForm))
            {
                FreeForm += "~";
            }
            FreeForm += "Restrictions^ " + restrictions.TrimEnd(',');
        }
        if (!string.IsNullOrEmpty(txtOtherRestrictions.Text.Trim()))
        {
            if (!string.IsNullOrEmpty(FreeForm))
            {
                FreeForm += "~";
            }
            FreeForm += "Others^ " + txtOtherRestrictions.Text.Trim();
        }
        if (!string.IsNullOrEmpty(workStatus))
        {
            if (!string.IsNullOrEmpty(FreeForm))
            {
                FreeForm += "~";
            }
            FreeForm += "Work Status^ " + workStatus.TrimEnd(',');
        }
        if (!string.IsNullOrEmpty(txtFreeForm.Text))
        {
            if (!string.IsNullOrEmpty(FreeForm))
            {
                FreeForm += "~";
            }
            FreeForm += "Notes^ " + txtFreeForm.Text.TrimEnd(',');
        }
        return (!string.IsNullOrEmpty(FreeForm)) ? FreeForm : "None";
    }

    //protected string GetFreeForm()
    //{
    //    string restrictions = string.Empty;
    //    foreach (ListItem s in cblRestictions.Items)
    //    {
    //        if (s.Selected)
    //        {
    //            restrictions += s.Value.ToLower() + ", ";
    //        }
    //    }
    //    string workStatus = string.Empty;


    //    foreach (RepeaterItem item in this.Repeater1.Items)
    //    {
    //        CheckBox chk = item.FindControl("cblWorkStatus") as CheckBox;
    //        if (chk.Checked)
    //        {
    //            string collageName = (item.FindControl("txtCollageName") as TextBox).Text;
    //            string degree = chk.Text;
    //            workStatus += degree.ToLower() + "-" + collageName.ToLower() + ", ";

    //        }
    //    }

    //    string FreeForm = string.Empty;
    //    if (rblDOD.SelectedIndex > -1)
    //    {
    //        FreeForm = "Degree of Disability: " + rblDOD.SelectedValue;
    //    }
    //    if (!string.IsNullOrEmpty(restrictions))
    //    {
    //        if (!string.IsNullOrEmpty(FreeForm))
    //        {
    //            FreeForm += ";";
    //        }
    //        FreeForm += "Restrictions: " + restrictions.TrimEnd(',');
    //    }
    //    if (!string.IsNullOrEmpty(txtOtherRestrictions.Text.Trim()))
    //    {
    //        if (!string.IsNullOrEmpty(FreeForm))
    //        {
    //            FreeForm += ";";
    //        }
    //        FreeForm += "Others: " + txtOtherRestrictions.Text.Trim();
    //    }
    //    if (!string.IsNullOrEmpty(workStatus))
    //    {
    //        if (!string.IsNullOrEmpty(FreeForm))
    //        {
    //            FreeForm += ";";
    //        }
    //        FreeForm += "Work Status: " + workStatus.TrimEnd(',');
    //    }
    //    if (!string.IsNullOrEmpty(txtFreeForm.Text))
    //    {
    //        if (!string.IsNullOrEmpty(FreeForm))
    //        {
    //            FreeForm += ";";
    //        }
    //        FreeForm += "Notes: " + txtFreeForm.Text.TrimEnd(',');
    //    }

    //    return (!string.IsNullOrEmpty(FreeForm)) ? FreeForm : "None";
    //}

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/PatientIntakeList.aspx");
    }

    protected void lbtnTimeSheet_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/TimeSheet.aspx?PId=" + Request.QueryString["PId"]);
    }
    protected void lbtnLogout_Click(object sender, EventArgs e)
    {
        Session.Abandon();
        Response.Redirect("~/Login.aspx");

    }

    protected void gvPatientDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvPatientFUDetails.PageIndex = e.NewPageIndex;
        bindFUDetails(Convert.ToInt32(hfPatientIEId.Value));
    }
    //#region Check box Event checked changed of body part.
    /// <summary>
    /// Event fires if check box chk_r_Shoulder checked or unchecked in the form.
    /// </summary>
    protected void chk_r_Shoulder_CheckedChanged(object sender, EventArgs e)
    {
        SaveData(); var s = (List<BodyParts>)Session["FUbodyPartsList"];
        if (s != null)
        {

            var cnt = (from i in s.AsEnumerable() where i.Parts == "RightShoulder" && i.Checked == true select i).ToList();
            if (cnt == null) { count = 0; } else { count = cnt.Count(); }
            if (chk_r_Shoulder.Checked)
            {
                if (count == 0)
                {
                    s.RemoveAll(x => x.Parts == "RightShoulder");
                    BodyParts d = new BodyParts();
                    d.Parts = "RightShoulder";
                    d.Checked = true;
                    s.Add(d);
                    Session["RightShoulder"] = 1;
                }
                // if(s.AsEnumerable().Where(s=>s.Any=="pafe"))
                Chk_r_Shoulder = true;
            }
            else
            {
                if (count > 0)
                {
                    s.RemoveAll(x => x.Parts == "RightShoulder");
                    BodyParts d = new BodyParts();
                    d.Parts = "RightShoulder";
                    d.Checked = false;
                    s.Add(d);
                    Session["RightShoulder"] = 0;
                }
                Chk_r_Shoulder = false;
                this.removeROM("rightNormal", hfPatientFUId.Value, "tblFUbpShoulder");
            }
            Session["FUbodyPartsList"] = s;
            AddFollowUpMaster master = (AddFollowUpMaster)this.Master;
            master.bindData(hfPatientFUId.Value);
        }
        else
        {
            List<BodyParts> s1 = new List<BodyParts>();
            if (chk_r_Shoulder.Checked)
            {
                s1.RemoveAll(x => x.Parts == "RightShoulder");
                BodyParts d = new BodyParts();
                d.Parts = "RightShoulder";
                d.Checked = true;
                s1.Add(d);
                Chk_r_Shoulder = true;
                Session["RightShoulder"] = 1;
            }
            else
            {
                s1.RemoveAll(x => x.Parts == "RightShoulder");
                BodyParts d = new BodyParts();
                d.Parts = "RightShoulder";
                d.Checked = false;
                s1.Add(d);

                Chk_r_Shoulder = false;
                Session["RightShoulder"] = 0;
            }
            Session["FUbodyPartsList"] = s1;
            AddFollowUpMaster master = (AddFollowUpMaster)this.Master; master.bindData(hfPatientFUId.Value);

        }

        if (chk_r_Shoulder.Checked == false && chk_L_Shoulder.Checked == false)
            removeBodyParts(hfPatientFUId.Value, "tblFUbpshoulder");
        else
            saveSideIE("shoulder", chk_L_Shoulder.Checked, chk_r_Shoulder.Checked, false);


        if (chk_r_Shoulder.Checked == false)
            removeDaignosis(hfPatientFUId.Value, "shoulder", "right");
    }
    /// <summary>
    /// Event fires if check box chk_L_Shoulder checked or unchecked in the form.
    /// </summary>
    protected void chk_L_Shoulder_CheckedChanged(object sender, EventArgs e)
    {
        SaveData(); var s = (List<BodyParts>)Session["FUbodyPartsList"];
        if (s != null)
        {

            var cnt = (from i in s.AsEnumerable() where i.Parts == "LeftShoulder" && i.Checked == true select i).ToList();
            if (cnt == null) { count = 0; } else { count = cnt.Count(); }
            if (chk_L_Shoulder.Checked)
            {
                if (count == 0)
                {
                    s.RemoveAll(x => x.Parts == "LeftShoulder");
                    BodyParts d = new BodyParts();
                    d.Parts = "LeftShoulder";
                    d.Checked = true;
                    s.Add(d);
                }
                Chk_L_Shoulder = true;
                Session["LeftShoulder"] = 1;
            }
            else
            {
                if (count > 0)
                {
                    s.RemoveAll(x => x.Parts == "LeftShoulder");
                    BodyParts d = new BodyParts();
                    d.Parts = "LeftShoulder";
                    d.Checked = false;
                    s.Add(d);
                }
                Chk_L_Shoulder = false;
                Session["LeftShoulder"] = 0;
                this.removeROM("leftNormal", hfPatientFUId.Value, "tblFUbpShoulder");
            }
            Session["FUbodyPartsList"] = s;
            AddFollowUpMaster master = (AddFollowUpMaster)this.Master; master.bindData(hfPatientFUId.Value);
        }
        else
        {
            List<BodyParts> s1 = new List<BodyParts>();
            if (chk_L_Shoulder.Checked)
            {
                s1.RemoveAll(x => x.Parts == "LeftShoulder");
                BodyParts d = new BodyParts();
                d.Parts = "LeftShoulder";
                d.Checked = true;
                s1.Add(d);

                Chk_L_Shoulder = true;
                Session["LeftShoulder"] = 1;
            }
            else
            {
                s1.RemoveAll(x => x.Parts == "LeftShoulder");
                BodyParts d = new BodyParts();
                d.Parts = "LeftShoulder";
                d.Checked = false;
                s1.Add(d);

                Chk_L_Shoulder = false;
                Session["LeftShoulder"] = 0;
            }
            Session["FUbodyPartsList"] = s1;
            AddFollowUpMaster master = (AddFollowUpMaster)this.Master; master.bindData(hfPatientFUId.Value);

        }
        if (chk_r_Shoulder.Checked == false && chk_L_Shoulder.Checked == false)
            removeBodyParts(hfPatientFUId.Value, "tblFUbpshoulder");
        else
            saveSideIE("shoulder", chk_L_Shoulder.Checked, chk_r_Shoulder.Checked, false);


        if (chk_L_Shoulder.Checked == false)
            removeDaignosis(hfPatientFUId.Value, "shoulder", "left");
    }
    /// <summary>
    /// Event fires if check box chk_Neck checked or unchecked in the form.
    /// </summary>
    protected void chk_Neck_CheckedChanged(object sender, EventArgs e)
    {
        SaveData(); var s = (List<BodyParts>)Session["FUbodyPartsList"];
        if (s != null)
        {
            var cnt = (from i in s.AsEnumerable() where i.Parts == "Neck" && i.Checked == true select i).ToList();
            if (cnt == null) { count = 0; } else { count = cnt.Count(); }
            if (chk_Neck.Checked)
            {
                if (count == 0)
                {
                    s.RemoveAll(x => x.Parts == "Neck");
                    BodyParts d = new BodyParts();
                    d.Parts = "Neck";
                    d.Checked = true;
                    s.Add(d);
                }

                Chk_Neck = true;
            }
            else
            {
                if (count > 0)
                {
                    s.RemoveAll(x => x.Parts == "Neck");
                    BodyParts d = new BodyParts();
                    d.Parts = "Neck";
                    d.Checked = false;
                    s.Add(d);
                }
                Chk_Neck = false;
            }
            Session["FUbodyPartsList"] = s;
            AddFollowUpMaster master = (AddFollowUpMaster)this.Master; master.bindData(hfPatientFUId.Value);
        }
        else
        {
            List<BodyParts> s1 = new List<BodyParts>();
            if (chk_Neck.Checked)
            {
                s1.RemoveAll(x => x.Parts == "Neck");
                BodyParts d = new BodyParts();
                d.Parts = "Neck";
                d.Checked = true;
                s1.Add(d);


                Chk_Neck = true;
            }
            else
            {

                s1.RemoveAll(x => x.Parts == "Neck");
                BodyParts d = new BodyParts();
                d.Parts = "Neck";
                d.Checked = false;
                s1.Add(d);

                Chk_Neck = false;
            }
            Session["FUbodyPartsList"] = s1;
            AddFollowUpMaster master = (AddFollowUpMaster)this.Master; master.bindData(hfPatientFUId.Value);
        }

        if (Chk_Neck == false)
        {
            removeDaignosis(hfPatientFUId.Value, "neck");
            removeBodyParts(hfPatientFUId.Value, "tblFUbpneck");
        }
        else
        {
            this.saveIE("neck", !Chk_Neck);
        }
    }
    /// <summary>
    /// Event fires if check box chk_Midback checked or unchecked in the form.
    /// </summary>
    protected void chk_Midback_CheckedChanged(object sender, EventArgs e)
    {
        SaveData(); var s = (List<BodyParts>)Session["FUbodyPartsList"];
        if (s != null)
        {

            var cnt = (from i in s.AsEnumerable() where i.Parts == "MidBack" && i.Checked == true select i).ToList();
            if (cnt == null) { count = 0; } else { count = cnt.Count(); }
            if (chk_Midback.Checked)
            {

                if (count == 0)
                {
                    BodyParts d = new BodyParts();
                    d.Parts = "MidBack";
                    d.Checked = true;
                    s.Add(d);
                }
                Chk_Midback = true;
            }
            else
            {
                if (count > 0)
                {
                    s.RemoveAll(x => x.Parts == "MidBack");
                    BodyParts d = new BodyParts();
                    d.Parts = "MidBack";
                    d.Checked = false;
                    s.Add(d);
                }
                Chk_Midback = false;
            }
            Session["FUbodyPartsList"] = s;
            AddFollowUpMaster master = (AddFollowUpMaster)this.Master; master.bindData(hfPatientFUId.Value);
        }
        else
        {
            List<BodyParts> s1 = new List<BodyParts>();
            if (chk_Midback.Checked)
            {
                BodyParts d = new BodyParts();
                d.Parts = "MidBack";
                d.Checked = true;
                s1.Add(d);
                Chk_Midback = true;
            }
            else
            {

                s.RemoveAll(x => x.Parts == "MidBack");
                BodyParts d = new BodyParts();
                d.Parts = "MidBack";
                d.Checked = false;
                s1.Add(d);
                Chk_Midback = false;
            }
            Session["FUbodyPartsList"] = s1;
            AddFollowUpMaster master = (AddFollowUpMaster)this.Master; master.bindData(hfPatientFUId.Value);
        }
        if (Chk_Midback == false)
        {
            removeDaignosis(hfPatientFUId.Value, "midback");
            removeBodyParts(hfPatientFUId.Value, "tblFUbpmidback");
        }
        else
        {
            this.saveIE("midback", !Chk_Midback);
        }
    }
    /// <summary>
    /// Event fires if check box chk_lowback checked or unchecked in the form.
    /// </summary>
    protected void chk_lowback_CheckedChanged(object sender, EventArgs e)
    {
        SaveData(); var s = (List<BodyParts>)Session["FUbodyPartsList"];
        if (s != null)
        {
            var cnt = (from i in s.AsEnumerable() where i.Parts == "LowBack" && i.Checked == true select i).ToList();
            if (cnt == null) { count = 0; } else { count = cnt.Count(); }
            if (chk_lowback.Checked)
            {
                if (count == 0)
                {
                    BodyParts d = new BodyParts();
                    d.Parts = "LowBack";
                    d.Checked = true;
                    s.Add(d);
                }

                Chk_lowback = true;
            }
            else
            {
                if (count > 0)
                {
                    s.RemoveAll(x => x.Parts == "LowBack");
                    BodyParts d = new BodyParts();
                    d.Parts = "LowBack";
                    d.Checked = false;
                    s.Add(d);
                }
                Chk_lowback = true;
            }
            Session["FUbodyPartsList"] = s;
            AddFollowUpMaster master = (AddFollowUpMaster)this.Master; master.bindData(hfPatientFUId.Value);
        }
        else
        {
            List<BodyParts> s1 = new List<BodyParts>();
            if (chk_lowback.Checked)
            {
                BodyParts d = new BodyParts();
                d.Parts = "LowBack";
                d.Checked = true;
                s1.Add(d);
                Chk_lowback = true;
            }
            else
            {
                s1.RemoveAll(x => x.Parts == "LowBack");
                BodyParts d = new BodyParts();
                d.Parts = "LowBack";
                d.Checked = false;
                s1.Add(d);
                Chk_lowback = true;
            }
            Session["FUbodyPartsList"] = s1;
            AddFollowUpMaster master = (AddFollowUpMaster)this.Master; master.bindData(hfPatientFUId.Value);
        }

        if (Chk_lowback == false)
        {
            removeDaignosis(hfPatientFUId.Value, "lowback");
            removeBodyParts(hfPatientFUId.Value, "tblFUbplowback");
        }
        else
        {
            this.saveIE("lowback", !Chk_lowback);
        }
    }
    /// <summary>
    /// Event fires if check box chk_r_Keen checked or unchecked in the form.
    /// </summary>
    protected void chk_r_Keen_CheckedChanged(object sender, EventArgs e)
    {
        SaveData(); var s = (List<BodyParts>)Session["FUbodyPartsList"];
        if (s != null)
        {
            var cnt = (from i in s.AsEnumerable() where (i.Parts == "RightKnee" && i.Checked == true) select i).ToList();
            if (cnt == null) { count = 0; } else { count = cnt.Count(); }
            if (chk_r_Keen.Checked)
            {
                if (count == 0)
                {
                    s.RemoveAll(x => x.Parts == "RightKnee");
                    BodyParts d = new BodyParts();
                    d.Parts = "RightKnee";
                    d.Checked = true;
                    s.Add(d);
                }
                Chk_r_Keen = true;
            }
            else
            {
                if (count > 0)
                {

                    s.RemoveAll(x => x.Parts == "RightKnee");
                    BodyParts d = new BodyParts();
                    d.Parts = "RightKnee";
                    d.Checked = false;
                    s.Add(d);
                }
                Chk_r_Keen = false;
                this.removeROM("rightNormal", hfPatientFUId.Value, "tblFUbpKnee");
            }
            Session["FUbodyPartsList"] = s;
            AddFollowUpMaster master = (AddFollowUpMaster)this.Master; master.bindData(hfPatientFUId.Value);
        }
        else
        {
            List<BodyParts> s1 = new List<BodyParts>();
            if (chk_r_Keen.Checked)
            {
                s1.RemoveAll(x => x.Parts == "RightKnee");
                BodyParts d = new BodyParts();
                d.Parts = "RightKnee";
                d.Checked = true;
                s1.Add(d);

                Chk_r_Keen = true;
            }
            else
            {
                s1.RemoveAll(x => x.Parts == "RightKnee");
                BodyParts d = new BodyParts();
                d.Parts = "RightKnee";
                d.Checked = false;
                s1.Add(d);

                Chk_r_Keen = false;

            }
            Session["FUbodyPartsList"] = s1;
            AddFollowUpMaster master = (AddFollowUpMaster)this.Master; master.bindData(hfPatientFUId.Value);

        }

        if (chk_r_Keen.Checked == false && chk_L_Keen.Checked == false)
            removeBodyParts(hfPatientFUId.Value, "tblFubpknee");
        else
            saveSideIE("knee", chk_L_Keen.Checked, chk_r_Keen.Checked, false);



        if (chk_r_Keen.Checked == false)
            removeDaignosis(hfPatientFUId.Value, "knee", "right");
    }
    /// <summary>
    /// Event fires if check box chk_L_Keen checked or unchecked in the form.
    /// </summary>
    protected void chk_L_Keen_CheckedChanged(object sender, EventArgs e)
    {
        SaveData(); var s = (List<BodyParts>)Session["FUbodyPartsList"];
        if (s != null)
        {

            var cnt = (from i in s.AsEnumerable() where i.Parts == "LeftKnee" && i.Checked == true select i).ToList();
            if (cnt == null) { count = 0; } else { count = cnt.Count(); }
            if (chk_L_Keen.Checked)
            {
                if (count == 0)
                {
                    s.RemoveAll(x => x.Parts == "LeftKnee");
                    BodyParts d = new BodyParts();
                    d.Parts = "LeftKnee";
                    d.Checked = true;
                    s.Add(d);
                }
                Chk_L_Keen = true;
            }
            else
            {
                if (count > 0)
                {
                    s.RemoveAll(x => x.Parts == "LeftKnee");
                    BodyParts d = new BodyParts();
                    d.Parts = "LeftKnee";
                    d.Checked = false;
                    s.Add(d);
                }
                Chk_L_Keen = true;
                this.removeROM("leftNormal", hfPatientFUId.Value, "tblFUbpKnee");
            }

            Session["FUbodyPartsList"] = s;
            AddFollowUpMaster master = (AddFollowUpMaster)this.Master; master.bindData(hfPatientFUId.Value);
        }
        else
        {
            List<BodyParts> s1 = new List<BodyParts>();
            if (chk_L_Keen.Checked)
            {
                s1.RemoveAll(x => x.Parts == "LeftKnee");
                BodyParts d = new BodyParts();
                d.Parts = "LeftKnee";
                d.Checked = true;
                s1.Add(d);

                Chk_L_Keen = true;
            }
            else
            {
                s1.RemoveAll(x => x.Parts == "LeftKnee");
                BodyParts d = new BodyParts();
                d.Parts = "LeftKnee";
                d.Checked = false;
                s1.Add(d);

                Chk_L_Keen = true;
            }

            Session["FUbodyPartsList"] = s1;
            AddFollowUpMaster master = (AddFollowUpMaster)this.Master; master.bindData(hfPatientFUId.Value);

        }

        if (chk_r_Keen.Checked == false && chk_L_Keen.Checked == false)
            removeBodyParts(hfPatientFUId.Value, "tblFUbpknee");
        else
            saveSideIE("knee", chk_L_Keen.Checked, chk_r_Keen.Checked, false);

        if (chk_L_Keen.Checked == false)
            removeDaignosis(hfPatientFUId.Value, "knee", "left");
    }
    /// <summary>
    /// Event fires if check box chk_r_Elbow checked or unchecked in the form.
    /// </summary>
    protected void chk_r_Elbow_CheckedChanged(object sender, EventArgs e)
    {
        SaveData(); var s = (List<BodyParts>)Session["FUbodyPartsList"];
        if (s != null)
        {
            var cnt = (from i in s.AsEnumerable() where i.Parts == "RightElbow" && i.Checked == true select i).ToList();
            if (cnt == null) { count = 0; } else { count = cnt.Count(); }
            if (chk_r_Elbow.Checked)
            {
                if (count == 0)
                {
                    s.RemoveAll(x => x.Parts == "RightElbow");
                    BodyParts d = new BodyParts();
                    d.Parts = "RightElbow";
                    d.Checked = true;
                    s.Add(d);
                }
                Chk_r_Elbow = true;
            }
            else
            {
                if (count > 0)
                {

                    s.RemoveAll(x => x.Parts == "RightElbow");
                    BodyParts d = new BodyParts();
                    d.Parts = "RightElbow";
                    d.Checked = false;
                    s.Add(d);
                }
                Chk_r_Elbow = false;
                this.removeROM("rightNormal", hfPatientFUId.Value, "tblFUbpElbow");
            }

            Session["FUbodyPartsList"] = s;
            AddFollowUpMaster master = (AddFollowUpMaster)this.Master; master.bindData(hfPatientFUId.Value);
        }
        else
        {
            List<BodyParts> s1 = new List<BodyParts>();
            if (chk_r_Elbow.Checked)
            {

                s1.RemoveAll(x => x.Parts == "RightElbow");
                BodyParts d = new BodyParts();
                d.Parts = "RightElbow";
                d.Checked = true;
                s1.Add(d);

                Chk_r_Elbow = true;
            }
            else
            {
                s1.RemoveAll(x => x.Parts == "RightElbow");
                BodyParts d = new BodyParts();
                d.Parts = "RightElbow";
                d.Checked = false;
                s1.Add(d);

                Chk_r_Elbow = false;
            }

            Session["FUbodyPartsList"] = s1;
            AddFollowUpMaster master = (AddFollowUpMaster)this.Master; master.bindData(hfPatientFUId.Value);
        }
        if (chk_r_Elbow.Checked == false && chk_l_Elbow.Checked == false)
            removeBodyParts(hfPatientFUId.Value, "tblFUbpElbow");
        else
            saveSideIE("elbow", chk_l_Elbow.Checked, chk_r_Elbow.Checked, false);


        if (chk_r_Elbow.Checked == false)
            removeDaignosis(hfPatientFUId.Value, "elbow", "right");
    }
    /// <summary>
    /// Event fires if check box chk_l_Elbow checked or unchecked in the form.
    /// </summary>
    protected void chk_l_Elbow_CheckedChanged(object sender, EventArgs e)
    {
        SaveData(); var s = (List<BodyParts>)Session["FUbodyPartsList"];
        if (s != null)
        {
            var cnt = (from i in s.AsEnumerable() where i.Parts == "LeftElbow" && i.Checked == true select i).ToList();
            if (cnt == null) { count = 0; } else { count = cnt.Count(); }
            if (chk_l_Elbow.Checked)
            {
                if (count == 0)
                {
                    s.RemoveAll(x => x.Parts == "LeftElbow");
                    BodyParts d = new BodyParts();
                    d.Parts = "LeftElbow";
                    d.Checked = true;
                    s.Add(d);
                }

                Chk_l_Elbow = true;
            }
            else
            {
                if (count > 0)
                {
                    s.RemoveAll(x => x.Parts == "LeftElbow");
                    BodyParts d = new BodyParts();
                    d.Parts = "LeftElbow";
                    d.Checked = false;
                    s.Add(d);
                }
                Chk_l_Elbow = true;
                this.removeROM("leftNormal", hfPatientFUId.Value, "tblFUbpElbow");
            }

            Session["FUbodyPartsList"] = s;
            AddFollowUpMaster master = (AddFollowUpMaster)this.Master; master.bindData(hfPatientFUId.Value);
        }
        else
        {
            List<BodyParts> s1 = new List<BodyParts>();
            if (chk_l_Elbow.Checked)
            {
                s1.RemoveAll(x => x.Parts == "LeftElbow");
                BodyParts d = new BodyParts();
                d.Parts = "LeftElbow";
                d.Checked = true;
                s1.Add(d);
                Chk_l_Elbow = true;
            }
            else
            {
                s1.RemoveAll(x => x.Parts == "LeftElbow");
                BodyParts d = new BodyParts();
                d.Parts = "LeftElbow";
                d.Checked = false;
                s1.Add(d);

                Chk_l_Elbow = true;
            }

            Session["FUbodyPartsList"] = s1;
            AddFollowUpMaster master = (AddFollowUpMaster)this.Master; master.bindData(hfPatientFUId.Value);

        }

        if (chk_r_Elbow.Checked == false && chk_l_Elbow.Checked == false)
            removeBodyParts(hfPatientFUId.Value, "tblFUbpElbow");
        else
            saveSideIE("elbow", chk_l_Elbow.Checked, chk_r_Elbow.Checked, false);

        if (chk_l_Elbow.Checked == false)
            removeDaignosis(hfPatientFUId.Value, "elbow", "left");
    }
    /// <summary>
    /// Event fires if check box chk_r_Wrist checked or unchecked in the form.
    /// </summary>
    protected void chk_r_Wrist_CheckedChanged(object sender, EventArgs e)
    {
        SaveData(); var s = (List<BodyParts>)Session["FUbodyPartsList"];
        if (s != null)
        {
            var cnt = (from i in s.AsEnumerable() where i.Parts == "RightWrist" && i.Checked == true select i).ToList();
            if (cnt == null) { count = 0; } else { count = cnt.Count(); }

            if (chk_r_Wrist.Checked)
            {
                if (count == 0)
                {
                    s.RemoveAll(x => x.Parts == "RightWrist");
                    BodyParts d = new BodyParts();
                    d.Parts = "RightWrist";
                    d.Checked = true;
                    s.Add(d);
                }
                Chk_r_Wrist = true;
            }
            else
            {
                if (count > 0)
                {
                    s.RemoveAll(x => x.Parts == "RightWrist");
                    BodyParts d = new BodyParts();
                    d.Parts = "RightWrist";
                    d.Checked = false;
                    s.Add(d);
                }
                Chk_r_Wrist = false;
                this.removeROM("rightNormal", hfPatientFUId.Value, "tblFUbpWrist");
            }

            Session["FUbodyPartsList"] = s;
            AddFollowUpMaster master = (AddFollowUpMaster)this.Master; master.bindData(hfPatientFUId.Value);
        }
        else
        {
            List<BodyParts> s1 = new List<BodyParts>();
            if (chk_r_Wrist.Checked)
            {
                s1.RemoveAll(x => x.Parts == "RightWrist");
                BodyParts d = new BodyParts();
                d.Parts = "RightWrist";
                d.Checked = true;
                s1.Add(d);
                Chk_r_Wrist = true;
            }
            else
            {
                s1.RemoveAll(x => x.Parts == "RightWrist");
                BodyParts d = new BodyParts();
                d.Parts = "RightWrist";
                d.Checked = false;
                s1.Add(d);
                Chk_r_Wrist = false;
            }

            Session["FUbodyPartsList"] = s1;
            AddFollowUpMaster master = (AddFollowUpMaster)this.Master; master.bindData(hfPatientFUId.Value);
        }

        if (chk_r_Wrist.Checked == false && chk_l_Wrist.Checked == false)
            removeBodyParts(hfPatientFUId.Value, "tblFUbpWrist");
        else
            saveSideIE("wrist", chk_l_Wrist.Checked, chk_r_Wrist.Checked, false);


        if (chk_r_Wrist.Checked == false)
            removeDaignosis(hfPatientFUId.Value, "wrist", "right");
    }
    /// <summary>
    /// Event fires if check box chk_l_Wrist checked or unchecked in the form.
    /// </summary>
    protected void chk_l_Wrist_CheckedChanged(object sender, EventArgs e)
    {
        SaveData(); var s = (List<BodyParts>)Session["FUbodyPartsList"];
        if (s != null)
        {
            var cnt = (from i in s.AsEnumerable() where i.Parts == "LeftWrist" && i.Checked == true select i).ToList();
            if (cnt == null) { count = 0; } else { count = cnt.Count(); }
            if (chk_l_Wrist.Checked)
            {
                if (count == 0)
                {
                    s.RemoveAll(x => x.Parts == "LeftWrist");
                    BodyParts d = new BodyParts();
                    d.Parts = "LeftWrist";
                    d.Checked = true;
                    s.Add(d);
                }
                Chk_l_Wrist = true;
            }
            else
            {
                if (count > 0)
                {
                    s.RemoveAll(x => x.Parts == "LeftWrist");
                    BodyParts d = new BodyParts();
                    d.Parts = "LeftWrist";
                    d.Checked = false;
                    s.Add(d);
                }
                Chk_l_Wrist = true;
                this.removeROM("leftNormal", hfPatientFUId.Value, "tblFUbpWrist");
            }
            Session["FUbodyPartsList"] = s;
            AddFollowUpMaster master = (AddFollowUpMaster)this.Master; master.bindData(hfPatientFUId.Value);
        }
        else
        {
            List<BodyParts> s1 = new List<BodyParts>();
            if (chk_l_Wrist.Checked)
            {
                s1.RemoveAll(x => x.Parts == "LeftWrist");
                BodyParts d = new BodyParts();
                d.Parts = "LeftWrist";
                d.Checked = true;
                s1.Add(d);

                Chk_l_Wrist = true;
            }
            else
            {
                s1.RemoveAll(x => x.Parts == "LeftWrist");
                BodyParts d = new BodyParts();
                d.Parts = "LeftWrist";
                d.Checked = false;
                s1.Add(d);

                Chk_l_Wrist = true;
            }
            Session["FUbodyPartsList"] = s1;
            AddFollowUpMaster master = (AddFollowUpMaster)this.Master; master.bindData(hfPatientFUId.Value);
        }


        if (chk_r_Wrist.Checked == false && chk_l_Wrist.Checked == false)
            removeBodyParts(hfPatientFUId.Value, "tblFUbpWrist");
        else
            saveSideIE("wrist", chk_l_Wrist.Checked, chk_r_Wrist.Checked, false);

        if (chk_l_Wrist.Checked == false)
            removeDaignosis(hfPatientFUId.Value, "wrist", "left");
    }
    /// <summary>
    /// Event fires if check box chk_r_Hip checked or unchecked in the form.
    /// </summary>
    protected void chk_r_Hip_CheckedChanged(object sender, EventArgs e)
    {
        SaveData(); var s = (List<BodyParts>)Session["FUbodyPartsList"];
        if (s != null)
        {
            var cnt = (from i in s.AsEnumerable() where i.Parts == "RightHip" && i.Checked == true select i).ToList();
            if (cnt == null) { count = 0; } else { count = cnt.Count(); }
            if (chk_r_Hip.Checked)
            {
                if (count == 0)
                {
                    s.RemoveAll(x => x.Parts == "RightHip");
                    BodyParts d = new BodyParts();
                    d.Parts = "RightHip";
                    d.Checked = true;
                    s.Add(d);
                }
                Chk_r_Hip = true;
            }
            else
            {
                if (count > 0)
                {
                    s.RemoveAll(x => x.Parts == "RightHip");
                    BodyParts d = new BodyParts();
                    d.Parts = "RightHip";
                    d.Checked = false;
                    s.Add(d);
                }
                Chk_r_Hip = false;
                this.removeROM("rightNormal", hfPatientFUId.Value, "tblFUbpHip");
            }

            Session["FUbodyPartsList"] = s;
            AddFollowUpMaster master = (AddFollowUpMaster)this.Master; master.bindData(hfPatientFUId.Value);
        }
        else
        {
            List<BodyParts> s1 = new List<BodyParts>();
            if (chk_r_Hip.Checked)
            {
                s1.RemoveAll(x => x.Parts == "RightHip");
                BodyParts d = new BodyParts();
                d.Parts = "RightHip";
                d.Checked = true;
                s1.Add(d);

                Chk_r_Hip = true;
            }
            else
            {
                s1.RemoveAll(x => x.Parts == "RightHip");
                BodyParts d = new BodyParts();
                d.Parts = "RightHip";
                d.Checked = false;
                s1.Add(d);

                Chk_r_Hip = false;
            }

            Session["FUbodyPartsList"] = s1;
            AddFollowUpMaster master = (AddFollowUpMaster)this.Master; master.bindData(hfPatientFUId.Value);

        }

        if (chk_r_Hip.Checked == false && chk_l_Hip.Checked == false)
            removeBodyParts(hfPatientFUId.Value, "tblFUbphip");
        else
            saveSideIE("hip", chk_l_Hip.Checked, chk_r_Hip.Checked, false);

        if (chk_r_Hip.Checked == false)
            removeDaignosis(hfPatientFUId.Value, "hip", "right");
    }
    /// <summary>
    /// Event fires if check box chk_l_Hip checked or unchecked in the form.
    /// </summary>
    protected void chk_l_Hip_CheckedChanged(object sender, EventArgs e)
    {
        SaveData(); var s = (List<BodyParts>)Session["FUbodyPartsList"];
        if (s != null)
        {
            var cnt = (from i in s.AsEnumerable() where i.Parts == "LeftHip" && i.Checked == true select i).ToList();
            if (cnt == null) { count = 0; } else { count = cnt.Count(); }
            if (chk_l_Hip.Checked)
            {
                if (count == 0)
                {
                    s.RemoveAll(x => x.Parts == "LeftHip");
                    BodyParts d = new BodyParts();
                    d.Parts = "LeftHip";
                    d.Checked = true;
                    s.Add(d);
                }
                Chk_l_Hip = true;
            }
            else
            {
                if (count > 0)
                {
                    s.RemoveAll(x => x.Parts == "LeftHip");
                    BodyParts d = new BodyParts();
                    d.Parts = "LeftHip";
                    d.Checked = false;
                    s.Add(d);
                }
                Chk_l_Hip = false;
                this.removeROM("leftNormal", hfPatientFUId.Value, "tblFUbpHip");
            }
            Session["FUbodyPartsList"] = s;
            AddFollowUpMaster master = (AddFollowUpMaster)this.Master; master.bindData(hfPatientFUId.Value);
        }
        else
        {
            List<BodyParts> s1 = new List<BodyParts>();
            if (chk_l_Hip.Checked)
            {
                s1.RemoveAll(x => x.Parts == "LeftHip");
                BodyParts d = new BodyParts();
                d.Parts = "LeftHip";
                d.Checked = true;
                s1.Add(d);
                Chk_l_Hip = true;
            }
            else
            {
                s1.RemoveAll(x => x.Parts == "LeftHip");
                BodyParts d = new BodyParts();
                d.Parts = "LeftHip";
                d.Checked = false;
                s1.Add(d);
                Chk_l_Hip = false;
            }
            Session["FUbodyPartsList"] = s1;
            AddFollowUpMaster master = (AddFollowUpMaster)this.Master; master.bindData(hfPatientFUId.Value);
        }

        if (chk_r_Hip.Checked == false && chk_l_Hip.Checked == false)
            removeBodyParts(hfPatientFUId.Value, "tblFUbphip");
        else
            saveSideIE("hip", chk_l_Hip.Checked, chk_r_Hip.Checked, false);

        if (chk_l_Hip.Checked == false)
            removeDaignosis(hfPatientFUId.Value, "hip", "left");
    }
    /// <summary>
    /// Event fires if check box chk_r_ankle checked or unchecked in the form.
    /// </summary>
    protected void chk_r_ankle_CheckedChanged(object sender, EventArgs e)
    {
        SaveData(); var s = (List<BodyParts>)Session["FUbodyPartsList"];
        if (s != null)
        {
            var cnt = (from i in s.AsEnumerable() where i.Parts == "RightAnkle" && i.Checked == true select i).ToList();
            if (cnt == null) { count = 0; } else { count = cnt.Count(); }
            if (chk_r_ankle.Checked)
            {
                if (count == 0)
                {
                    s.RemoveAll(x => x.Parts == "RightAnkle");
                    BodyParts d = new BodyParts();
                    d.Parts = "RightAnkle";
                    d.Checked = true;
                    s.Add(d);
                }
                Chk_r_ankle = true;
            }
            else
            {
                if (count > 0)
                {
                    s.RemoveAll(x => x.Parts == "RightAnkle");
                    BodyParts d = new BodyParts();
                    d.Parts = "RightAnkle";
                    d.Checked = false;
                    s.Add(d);
                }
                Chk_r_ankle = false;
                this.removeROM("rightNormal", hfPatientFUId.Value, "tblFUbpAnkle");
            }
            Session["FUbodyPartsList"] = s;

            AddFollowUpMaster master = (AddFollowUpMaster)this.Master; master.bindData(hfPatientFUId.Value);
        }
        else
        {
            List<BodyParts> s1 = new List<BodyParts>();
            if (chk_r_ankle.Checked)
            {
                s1.RemoveAll(x => x.Parts == "RightAnkle");
                BodyParts d = new BodyParts();
                d.Parts = "RightAnkle";
                d.Checked = true;
                s1.Add(d);
                Chk_r_ankle = true;
            }
            else
            {
                s1.RemoveAll(x => x.Parts == "RightAnkle");
                BodyParts d = new BodyParts();
                d.Parts = "RightAnkle";
                d.Checked = false;
                s1.Add(d);

                Chk_r_ankle = false;
            }
            Session["FUbodyPartsList"] = s1;

            AddFollowUpMaster master = (AddFollowUpMaster)this.Master; master.bindData(hfPatientFUId.Value);

        }

        if (chk_r_ankle.Checked == false && chk_l_ankle.Checked == false)
            removeBodyParts(hfPatientFUId.Value, "tblFUbpankle");
        else
            saveSideIE("ankle", chk_l_ankle.Checked, chk_r_ankle.Checked, false);

        if (chk_r_ankle.Checked == false)
            removeDaignosis(hfPatientFUId.Value, "ankle", "right");
    }
    /// <summary>
    /// Event fires if check box chk_l_ankle checked or unchecked in the form.
    /// </summary>
    protected void chk_l_ankle_CheckedChanged(object sender, EventArgs e)
    {

        SaveData();
        var s = (List<BodyParts>)Session["FUbodyPartsList"];
        if (s != null)
        {
            var cnt = (from i in s.AsEnumerable() where i.Parts == "LeftAnkle" && i.Checked == true select i).ToList();
            if (cnt == null) { count = 0; } else { count = cnt.Count(); }
            if (chk_l_ankle.Checked)
            {
                if (count == 0)
                {
                    s.RemoveAll(x => x.Parts == "LeftAnkle");
                    BodyParts d = new BodyParts();
                    d.Parts = "LeftAnkle";
                    d.Checked = true;
                    s.Add(d);
                }

                Chk_l_ankle = true;
            }
            else
            {
                if (count > 0)
                {
                    s.RemoveAll(x => x.Parts == "LeftAnkle");
                    BodyParts d = new BodyParts();
                    d.Parts = "LeftAnkle";
                    d.Checked = false;
                    s.Add(d);
                }
                Chk_l_ankle = false;
                this.removeROM("leftNormal", hfPatientFUId.Value, "tblFUbpAnkle");
            }
            Session["FUbodyPartsList"] = s;
            AddFollowUpMaster master = (AddFollowUpMaster)this.Master;
            master.bindData(hfPatientFUId.Value);
        }
        else
        {
            List<BodyParts> s1 = new List<BodyParts>();
            if (chk_l_ankle.Checked)
            {
                s1.RemoveAll(x => x.Parts == "LeftAnkle");
                BodyParts d = new BodyParts();
                d.Parts = "LeftAnkle";
                d.Checked = true;
                s1.Add(d);
                Chk_l_ankle = true;
            }
            else
            {
                s1.RemoveAll(x => x.Parts == "LeftAnkle");
                BodyParts d = new BodyParts();
                d.Parts = "LeftAnkle";
                d.Checked = false;
                s1.Add(d);
                Chk_l_ankle = false;
            }
            Session["FUbodyPartsList"] = s1;
            AddFollowUpMaster master = (AddFollowUpMaster)this.Master;
            master.bindData(hfPatientFUId.Value);

        }
        if (chk_r_ankle.Checked == false && chk_l_ankle.Checked == false)
            removeBodyParts(hfPatientFUId.Value, "tblFUbpankle");
        else
            saveSideIE("ankle", chk_l_ankle.Checked, chk_r_ankle.Checked, false);

        if (chk_l_ankle.Checked == false)
            removeDaignosis(hfPatientFUId.Value, "ankle", "left");
    }

    public void SaveData()
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString))
        {
            int PatientIE_Id = 0;
            string FreeForm = GetFreeForm();
            SqlCommand cmd = new SqlCommand("nusp_SavePatientDetails", con);
            cmd.Parameters.Add("@Patient_Id", hfPatientId.Value);
            cmd.Parameters.Add("@PatientIE_ID", hfPatientIEId.Value);
            cmd.Parameters.Add("@FreeForm", FreeForm);
            //if (!string.IsNullOrEmpty(hfPatientFUId.Value))
            //    cmd.Parameters.Add("@PatientFU_ID", hfPatientFUId.Value);

            if (!string.IsNullOrEmpty(Session["patientFUId"].ToString()))
            {
                cmd.Parameters.Add("@PatientFU_ID", Session["patientFUId"].ToString());
                cmd.Parameters.Add("@isEditFU", true);

            }

            cmd.Parameters.Add("@DOE", DateTime.ParseExact(txtDOV.Text, "MM/dd/yyyy", null));
            if (!string.IsNullOrEmpty(txtDOA.Text.Trim()))
                cmd.Parameters.Add("@DOA", DateTime.ParseExact(txtDOA.Text, "MM/dd/yyyy", null));
            if (!string.IsNullOrEmpty(txtDOB.Text.Trim()))
                cmd.Parameters.Add("@DOB", DateTime.ParseExact(txtDOB.Text, "MM/dd/yyyy", null));
            cmd.Parameters.Add("@FirstName", CultureInfo.CurrentCulture.TextInfo.ToTitleCase(txtFirstName.Text.Trim().ToLower()));
            cmd.Parameters.Add("@LastName", CultureInfo.CurrentCulture.TextInfo.ToTitleCase(txtLastName.Text.Trim().ToLower()));
            cmd.Parameters.Add("@Location_ID", ddlLoaction.SelectedValue);
            //if (!string.IsNullOrEmpty(txtDOA.Text.Trim()))
            //    cmd.Parameters.Add("@DOA", Convert.ToDateTime(txtDOA.Text.Trim()));
            cmd.Parameters.Add("@Sex", rblGender.SelectedValue);
            // cmd.Parameters.Add("@DOB", Convert.ToDateTime(txtDOB.Text.Trim()));
            cmd.Parameters.Add("@SSN", txtSSN.Text.Trim());
            cmd.Parameters.Add("@Phone", txtHomePh.Text.Trim());
            cmd.Parameters.Add("@work_phone", txtWorkPh.Text.Trim());
            cmd.Parameters.Add("@Phone2", txtMobile.Text.Trim());
            cmd.Parameters.Add("@Address", txtAddress.Text.Trim());
            cmd.Parameters.Add("@City", txtCity.Text.Trim());
            cmd.Parameters.Add("@State", ddState.SelectedValue.Trim());
            cmd.Parameters.Add("@Zip", txtZip.Text.Trim());
            cmd.Parameters.Add("@InsCo_ID", ddlInsuranceCo.SelectedValue);
            cmd.Parameters.Add("@ClaimNumber", txtClaim.Text.Trim());
            cmd.Parameters.Add("@policy_no", txtPolicy.Text.Trim());
            cmd.Parameters.Add("@Compensation", ddl_casetype.SelectedValue);
            cmd.Parameters.Add("@Attorney", txtAttorneyName.Text.Trim());
            cmd.Parameters.Add("@Telephone", txtAttorneyPh.Text.Trim());
            cmd.Parameters.Add("@MA_Providers", txtMAProviders.Text.Trim());
            cmd.Parameters.Add("@FollowedUpOn", txtFollowedUpOn.Text.Trim());
            //cmd.Parameters.Add("@PMH", PMH.Text.Trim());
            //cmd.Parameters.Add("@PSH", PSH.Text.Trim());
            //cmd.Parameters.Add("@Medications", Medication.Text.Trim());
            //cmd.Parameters.Add("@Allergies", Allergies.Text.Trim());

            cmd.Parameters.Add("@PMH", "");
            cmd.Parameters.Add("@PSH", "");
            cmd.Parameters.Add("@Medications", "");
            cmd.Parameters.Add("@Allergies", "");
            cmd.Parameters.Add("@FamilyHistory", FamilyHistory.Text.Trim());
            cmd.Parameters.Add("@Neck", chk_Neck.Checked);
            cmd.Parameters.Add("@MidBack", chk_Midback.Checked);
            cmd.Parameters.Add("@LowBack", chk_lowback.Checked);
            cmd.Parameters.Add("@LShoulder", chk_L_Shoulder.Checked);
            cmd.Parameters.Add("@RShoulder", chk_r_Shoulder.Checked);
            cmd.Parameters.Add("@cbLKNee", chk_L_Keen.Checked);
            cmd.Parameters.Add("@cbRKnee", chk_r_Keen.Checked);
            cmd.Parameters.Add("@cbLElbow", chk_l_Elbow.Checked);
            cmd.Parameters.Add("@cbRElbow", chk_r_Elbow.Checked);
            cmd.Parameters.Add("@cbLWrist", chk_l_Wrist.Checked);
            cmd.Parameters.Add("@cbRWrist", chk_r_Wrist.Checked);
            cmd.Parameters.Add("@cblAnkle", chk_l_ankle.Checked);
            cmd.Parameters.Add("@cbRAnkle", chk_r_ankle.Checked);
            cmd.Parameters.Add("@cbLHip", chk_l_Hip.Checked);
            cmd.Parameters.Add("@cbRHip", chk_r_Hip.Checked);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@PatientIntime", PatientIntime.Text);
            cmd.Parameters.Add("@PatientOuttime", PatientOuttime.Text);


            cmd.Parameters.Add("@ReturnToWork", cboReturnToWork.Text);
            cmd.Parameters.Add("@RecevingPhyTherapy", cboRecevingPhyTherapy.Text);
            cmd.Parameters.Add("@FeelPainRelief", cboFeelPainRelief.Text);
            cmd.Parameters.Add("@Note4", cbonote4.Text);
            cmd.Parameters.Add("@Note5", cbonote5.Text);

            SqlParameter outDirectoryId = new SqlParameter("@PatientFUIDS", SqlDbType.Int) { Direction = ParameterDirection.Output };
            cmd.Parameters.Add(outDirectoryId);

            con.Open();
            int count = cmd.ExecuteNonQuery();
            string alertText = "Follow Up already exsists for this date. Edit the followup";

            int DirectoryId = !String.IsNullOrEmpty(Convert.ToString(outDirectoryId.Value)) ? Convert.ToInt32(outDirectoryId.Value) : 0;
            if (DirectoryId > 0)
            {
                hfPatientFUId.Value = Convert.ToString(DirectoryId);
                Session["patientFUId"] = hfPatientFUId.Value;
            }
            //if (count > 0)
            //{
            //    // hfPatientFUId.Value = count.ToString();
            //    alertText = "Follow Up Added Successfully";
            //    hfPatientFUId=
            //    //Session["patientFUId"] = GetFUID(Convert.ToInt64(hfPatientIEId.Value != "" ? hfPatientIEId.Value : "0")).ToString();
            //    //hfPatientFUId.Value = GetFUID(Convert.ToInt64(hfPatientIEId.Value != "" ? hfPatientIEId.Value : "0")).ToString();
            //    //Session["patientFUId"] = hfPatientFUId.Value;
            //}
            //else
            //{
            //    Session["patientFUId"] = GetFUID(Convert.ToInt64(hfPatientIEId.Value != "" ? hfPatientIEId.Value : "0")).ToString();
            //    hfPatientFUId.Value = GetFUID(Convert.ToInt64(hfPatientIEId.Value != "" ? hfPatientIEId.Value : "0")).ToString();
            //}

            con.Close();
            //AddFollowUpMaster master = (AddFollowUpMaster)this.Master;
            //master.bindData(hfPatientIEId.Value);

            //  Response.Redirect("AddFU.aspx?PID=" + Request.QueryString["PID"]);
            //this.Page.RegisterStartupScript("Alert", "<script>javascript:alert('" + alertText + "')</script>");
        }
    }

    public int GetFUID(long PatientIE_Id)
    {
        IntakeSheet.DAL.DataAccess _dal = new IntakeSheet.DAL.DataAccess();
        List<SqlParameter> param = new List<SqlParameter>();
        param.Add(new SqlParameter("@PatientIE_Id", PatientIE_Id));
        DataTable _dt = _dal.getDataTable("GetFUID", param);
        var data = _dt.Rows[0]["PatientFU_ID"];
        return (data != DBNull.Value) ? Convert.ToInt32(data) : 0;
    }

    protected void bindRestrictions(int PatientIE_Id, string PatientFU_ID = null)
    {

        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString))
        {
            SqlCommand cmd = new SqlCommand("nusp_GetRestrictionDetails", con);
            cmd.Parameters.Add("@PatientIE_ID", PatientIE_Id);
            cmd.Parameters.Add("@PatientFU_IDnew", PatientFU_ID);
            cmd.CommandType = CommandType.StoredProcedure;
            con.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            string freeForm = string.Empty, locationId = string.Empty, MAProviders = string.Empty;
            while (sdr.Read())
            {
                freeForm = Convert.ToString(sdr["FreeForm"]);
                locationId = Convert.ToString(sdr["Location_Id"]);
                MAProviders = Convert.ToString(sdr["MA_Providers"]);
                txtFollowedUpOn.Text = Convert.ToString(sdr["FollowedUpOn"]);
                break;
            }
            if (!string.IsNullOrEmpty(locationId))
                ddlLoaction.SelectedValue = locationId == "" ? Convert.ToString(Session["Location"]) : locationId;
            txtMAProviders.Text = MAProviders == "" ? Convert.ToString(Session["Providers"]) : MAProviders;
            if (!string.IsNullOrEmpty(freeForm))
            {
                string[] freeFormDetails = freeForm.Split('~');
                for (int i = 0; i < freeFormDetails.Length; i++)
                {
                    if (freeFormDetails[i].Length > 0)
                    {
                        string title = freeFormDetails[i].Split('^')[0].Trim();
                        if (title == "Restrictions")
                        {
                            string details = freeFormDetails[i].Split('^')[1].Trim();
                            string[] restrictionValues = details.Split(',');
                            for (int j = 0; j < restrictionValues.Length; j++)
                            {
                                for (int cblIndex = 0; cblIndex < cblRestictions.Items.Count; cblIndex++)
                                {
                                    if (cblRestictions.Items[cblIndex].Value.Replace(" ", String.Empty).ToLower() == restrictionValues[j].Replace(" ", String.Empty).ToLower())
                                    {
                                        cblRestictions.Items[cblIndex].Selected = true;
                                    }
                                }
                            }
                        }
                        else if (title == "Others")
                        {
                            txtOtherRestrictions.Text = freeFormDetails[i].Split('^')[1].Trim();
                        }
                        else if (title == "Degree of Disability")
                        {
                            rblDOD.SelectedValue = freeFormDetails[i].Split('^')[1].Trim();
                        }
                        else if (title == "Notes")
                        {
                            txtFreeForm.Text = freeFormDetails[i].Split('^')[1].Trim();
                        }
                        else if (title == "Work Status")
                        {
                            string details = freeFormDetails[i].Split('^')[1].Trim();
                            string[] workStatusValues = details.Split(',');
                            for (int j = 0; j < workStatusValues.Length; j++)
                            {
                                foreach (RepeaterItem item in this.Repeater1.Items)
                                {
                                    CheckBox chk = item.FindControl("cblWorkStatus") as CheckBox;
                                    string[] v = null;
                                    List<string> myCollection = new List<string>();
                                    if (workStatusValues[j].Contains('-'))
                                    {
                                        v = workStatusValues[j].Split('-');
                                    }
                                    else
                                    {
                                        string ss = workStatusValues[j];
                                        myCollection.Add(ss);
                                        myCollection.Add("");
                                        v = myCollection.ToArray();
                                    }
                                    string og = chk.Text;
                                    og = Regex.Replace(chk.Text, @"\s+", "");
                                    string cm = v[0];
                                    cm = Regex.Replace(v[0], @"\s+", "");
                                    if (og.ToLower() == cm.ToLower())
                                    {
                                        // string collageName = (item.FindControl("txtCollageName") as TextBox).Text;
                                        //string degree = chk.Text;
                                        // this.SaveData(degree, collageName);
                                        chk.Checked = true;
                                        (item.FindControl("txtCollageName") as TextBox).Text = v[1];
                                    }
                                }
                            }
                        }
                    }
                }
            }
            con.Close();
        }
    }
    //protected void bindRestrictions(int PatientIE_Id)
    //{
    //    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString))
    //    {
    //        SqlCommand cmd = new SqlCommand("nusp_GetRestrictionDetails", con);
    //        cmd.Parameters.Add("@PatientIE_ID", PatientIE_Id);
    //        cmd.CommandType = CommandType.StoredProcedure;
    //        con.Open();
    //        SqlDataReader sdr = cmd.ExecuteReader();
    //        string freeForm = string.Empty, locationId = string.Empty, MAProviders = string.Empty;
    //        while (sdr.Read())
    //        {
    //            freeForm = Convert.ToString(sdr["FreeForm"]);
    //            locationId = Convert.ToString(sdr["Location_Id"]);
    //            MAProviders = Convert.ToString(sdr["MA_Providers"]);
    //            txtFollowedUpOn.Text = Convert.ToString(sdr["FollowedUpOn"]);
    //            break;
    //        }
    //        if (!string.IsNullOrEmpty(locationId))
    //            ddlLoaction.SelectedValue = locationId == "" ? Convert.ToString(Session["Location"]) : locationId;
    //        txtMAProviders.Text = MAProviders == "" ? Convert.ToString(Session["Providers"]) : MAProviders;
    //        if (!string.IsNullOrEmpty(freeForm))
    //        {
    //            string[] freeFormDetails = freeForm.Split(';');
    //            for (int i = 0; i < freeFormDetails.Length; i++)
    //            {
    //                if (freeFormDetails[i].Length > 0)
    //                {
    //                    string title = freeFormDetails[i].Split(':')[0].Trim();
    //                    if (title == "Restrictions")
    //                    {
    //                        string details = freeFormDetails[i].Split(':')[1].Trim();
    //                        string[] restrictionValues = details.Split(',');
    //                        for (int j = 0; j < restrictionValues.Length; j++)
    //                        {
    //                            for (int cblIndex = 0; cblIndex < cblRestictions.Items.Count; cblIndex++)
    //                            {
    //                                if (cblRestictions.Items[cblIndex].Value.Replace(" ", String.Empty).ToLower() == restrictionValues[j].Replace(" ", String.Empty).ToLower())
    //                                {
    //                                    cblRestictions.Items[cblIndex].Selected = true;
    //                                }
    //                            }
    //                        }
    //                    }
    //                    else if (title == "Others")
    //                    {
    //                        txtOtherRestrictions.Text = freeFormDetails[i].Split(':')[1].Trim();
    //                    }
    //                    else if (title == "Degree of Disability")
    //                    {
    //                        rblDOD.SelectedValue = freeFormDetails[i].Split(':')[1].Trim();
    //                    }
    //                    else if (title == "Notes")
    //                    {
    //                        txtFreeForm.Text = freeFormDetails[i].Split(':')[1].Trim();
    //                    }
    //                    else if (title == "Work Status")
    //                    {
    //                        string details = freeFormDetails[i].Split(':')[1].Trim();
    //                        string[] workStatusValues = details.Split(',');
    //                        for (int j = 0; j < workStatusValues.Length; j++)
    //                        {
    //                            foreach (RepeaterItem item in this.Repeater1.Items)
    //                            {
    //                                CheckBox chk = item.FindControl("cblWorkStatus") as CheckBox;
    //                                string[] v = null;
    //                                List<string> myCollection = new List<string>();
    //                                if (workStatusValues[j].Contains('-'))
    //                                {
    //                                    v = workStatusValues[j].Split('-');
    //                                }
    //                                else
    //                                {
    //                                    string ss = workStatusValues[j];
    //                                    myCollection.Add(ss);
    //                                    myCollection.Add("");
    //                                    v = myCollection.ToArray();
    //                                }
    //                                string og = chk.Text;
    //                                og = Regex.Replace(chk.Text, @"\s+", "");
    //                                string cm = v[0];
    //                                cm = Regex.Replace(v[0], @"\s+", "");
    //                                if (og.ToLower() == cm.ToLower())
    //                                {
    //                                    chk.Checked = true;
    //                                    (item.FindControl("txtCollageName") as TextBox).Text = v[1];
    //                                }
    //                            }
    //                        }
    //                    }
    //                }
    //            }
    //        }
    //        con.Close();
    //    }
    //}
    public void bindDropdown()
    {
        XmlDocument doc = new XmlDocument();
        doc.Load(Server.MapPath("~/xml/HSMData.xml"));

        foreach (XmlNode node in doc.SelectNodes("//HSM/RTNWORKS/RTNWORK"))
        {
            cboReturnToWork.Items.Add(new ListItem(node.Attributes["name"].InnerText, node.Attributes["name"].InnerText));
        }
        foreach (XmlNode node in doc.SelectNodes("//HSM/RECTPYS/RECTPY"))
        {
            cboRecevingPhyTherapy.Items.Add(new ListItem(node.Attributes["name"].InnerText, node.Attributes["name"].InnerText));
        }
        foreach (XmlNode node in doc.SelectNodes("//HSM/FEELPAINRLFS/FEELPAINRLF"))
        {
            cboFeelPainRelief.Items.Add(new ListItem(node.Attributes["name"].InnerText, node.Attributes["name"].InnerText));
        }
        foreach (XmlNode node in doc.SelectNodes("//HSM/Note4S/Note4"))
        {
            cbonote4.Items.Add(new ListItem(node.Attributes["name"].InnerText, node.Attributes["name"].InnerText));
        }
        foreach (XmlNode node in doc.SelectNodes("//HSM/Note5S/Note5"))
        {
            cbonote5.Items.Add(new ListItem(node.Attributes["name"].InnerText, node.Attributes["name"].InnerText));
        }

    }
    public void bindHtml()
    {

        string path = Server.MapPath("~/Template/Page1_degree.html");
        string body = File.ReadAllText(path);

        divdegreeHTML.InnerHtml = body;

        ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "hideWorkStatus();", true);

        path = Server.MapPath("~/Template/Page1_social.html");
        body = File.ReadAllText(path);

        divsocialHTML.InnerHtml = body;

        Int64 _ieID = Convert.ToInt64(Session["PatientIE_ID"].ToString());
        int _fuID = GetFUID(_ieID);

        if (_fuID == 0)
        {
            DataSet ds = db.selectData("select topSectionHTML from tblPage1HTMLContent where PatientIE_ID=" + _ieID.ToString());
            if (ds.Tables[0].Rows.Count > 0)
                divHistory.InnerHtml = ds.Tables[0].Rows[0]["topSectionHTML"].ToString();


            ds = db.selectData("select activityEffectedHTML from tblPage2HTMLContent where PatientIE_ID=" + _ieID.ToString());
            if (ds.Tables[0].Rows.Count > 0)
                divactivityeffected.InnerHtml = ds.Tables[0].Rows[0]["activityEffectedHTML"].ToString();
        }
        else
        {
            string query = "select top 1 activityEffectedHTML,topSectionHTML from tblPage1FUHTMLContent where PateintFU_ID=" + _fuID;
            DataSet ds = db.selectData(query);

            if (ds.Tables[0].Rows.Count > 0)
            {
                divHistory.InnerHtml = ds.Tables[0].Rows[0]["topSectionHTML"].ToString();
                divactivityeffected.InnerHtml = ds.Tables[0].Rows[0]["activityEffectedHTML"].ToString();
            }


        }

    }

    public void bindHTMLEdit()
    {
        //if (!string.IsNullOrEmpty(Session["patientFUId"].ToString()))
        if (hfPatientFUId.Value != "0" && Request["PID"] == null)
        {
            string query = "select top 1 * from tblPage1FUHTMLContent where PateintFU_ID=" + hfPatientFUId.Value;
            DataSet ds = db.selectData(query);
            if (ds.Tables[0].Rows.Count == 0)
            {
                bindHtml();
            }
            else
            {
                divdegreeHTML.InnerHtml = ds.Tables[0].Rows[0]["degreeSectionHTML"].ToString();
                divsocialHTML.InnerHtml = ds.Tables[0].Rows[0]["socialSectionHTML"].ToString();
                divHistory.InnerHtml = ds.Tables[0].Rows[0]["topSectionHTML"].ToString();
                divactivityeffected.InnerHtml = ds.Tables[0].Rows[0]["activityEffectedHTML"].ToString();

            }

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "hideWorkStatus();", true);
        }
        else
            bindHtml();
    }

    private void removeROM(string colName, string id, string tblName)
    {
        try
        {
            DBHelperClass db = new DBHelperClass();
            string query = "select * from " + tblName + " where PatientFU_ID=" + id;

            DataSet ds = db.selectData(query);

            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                string[] strname = ds.Tables[0].Rows[0]["NameROM"].ToString().Split(',');
                string str = "";
                for (int i = 0; i < strname.Length; i++)
                    str = ", " + str;


                query = "update " + tblName + " set " + colName + "='" + str.TrimStart(',') + "' where PatientFU_ID=" + id;

                new DBHelperClass().executeQuery(query);
            }
        }
        catch (Exception ex)
        {

        }
    }

    //private void deleteShoulder()
    //{
    //    try
    //    {
    //        if (chk_r_Shoulder.Checked == false && chk_L_Shoulder.Checked)
    //        {
    //            string query = "delete from  where PatientIE_ID=" + Session["PatientIE_ID"].ToString();

    //            int val = gDbhelperobj.executeQuery(query);
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //    }
    //}

    private void removeDaignosis(string id, string bodypart, string side = "")
    {
        try
        {
            DBHelperClass db = new DBHelperClass();
            string query = "delete from tblDiagCodesDetail where PatientFU_ID=" + id + "  AND BodyPart LIKE '%" + bodypart + "%'";

            if (!string.IsNullOrEmpty(side))
                query = query + " and Description Like '%" + side + "%'";

            db.executeQuery(query);
        }
        catch (Exception ex)
        {
            db.LogError(ex);
        }
    }

    private void removeBodyParts(string id, string tblName)
    {
        try
        {
            DBHelperClass db = new DBHelperClass();
            string query = "delete from " + tblName + " where PatientFU_ID=" + id;
            db.executeQuery(query);
        }
        catch (Exception ex)
        {
            db.LogError(ex);
        }
    }


    private void saveIE(string bodyPart, bool isDelete = false)
    {
        try
        {
            string query = "", tblName = "", ccHTML = "", peHTML = "";
            if (bodyPart == "neck")
            {
                tblName = "tblFUbpNeck";
                ccHTML = "NeckCC.html";
                peHTML = "NeckPE.html";
            }
            else if (bodyPart == "midback")
            {
                tblName = "tblFUbpMidback";
                ccHTML = "MidbackCC.html";
                peHTML = "MidbackPE.html";
            }
            else if (bodyPart == "lowback")
            {
                tblName = "tblFUbplowback";
                ccHTML = "LowbackCC.html";
                peHTML = "LowbackPE.html";
            }


            if (!isDelete)
            {


                string ccpath = Server.MapPath("~/Template/" + ccHTML);
                string ccbody = File.ReadAllText(ccpath);

                string pepath = Server.MapPath("~/Template/" + peHTML);
                string pebody = File.ReadAllText(pepath);

                query = "insert into " + tblName + " (PatientFU_ID,CCvalue,CCvalueoriginal,PEvalue,PEvalueoriginal)";
                query += "values(@PatientFU_ID,@CCvalue,@CCvalueoriginal,@PEvalue,@PEvalueoriginal)";

                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@CCvalue", ccbody);
                    command.Parameters.AddWithValue("@CCvalueoriginal", ccbody);
                    command.Parameters.AddWithValue("@PEvalue", pebody);
                    command.Parameters.AddWithValue("@PEvalueoriginal", pebody);

                    command.Parameters.AddWithValue("@PatientFU_ID", Session["patientFUId"].ToString());

                    connection.Open();
                    var results = command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            else
            {
                //  query = "delete from  " + tblName + " where PatientIE_ID=" + Session["PatientIE_ID"].ToString();
                // new DBHelperClass().executeQuery(query);
            }

        }
        catch (Exception ex)
        {
        }
    }

    private void saveSideIE(string bodyPart, bool left, bool right, bool isDelete)
    //    private void saveSideIE(string bodyPart, string side, bool isDelete, bool otherBody)
    {
        try
        {
            string query = "", tblName = "", ccHTML = "", peHTML = "";

            if (bodyPart == "shoulder")
            {
                tblName = "tblFUbpShoulder";
                ccHTML = "ShoulderCC.html";
                peHTML = "ShoulderPE.html";
            }
            else if (bodyPart == "wrist")
            {
                tblName = "tblFUbpWrist";
                ccHTML = "WristCC.html";
                peHTML = "WristPE.html";
            }
            else if (bodyPart == "ankle")
            {
                tblName = "tblFUbpAnkle";
                ccHTML = "AnkleCC.html";
                peHTML = "AnklePE.html";
            }
            else if (bodyPart == "elbow")
            {
                tblName = "tblFUbpEblow";
                ccHTML = "ElbowCC.html";
                peHTML = "ElbowPE.html";
            }
            else if (bodyPart == "hip")
            {
                tblName = "tblFUbpHip";
                ccHTML = "HipCC.html";
                peHTML = "HipPE.html";
            }
            else if (bodyPart == "knee")
            {
                tblName = "tblFUbpKnee";
                ccHTML = "KneeCC.html";
                peHTML = "KneePE.html";
            }


            if (!isDelete)
            {
                bool isExist = false;


                string ccpath = Server.MapPath("~/Template/" + ccHTML);
                string ccbody = File.ReadAllText(ccpath);
                string ccOrg = ccbody;




                string pepath = Server.MapPath("~/Template/" + peHTML);
                string pebody = File.ReadAllText(pepath);
                string peOrg = pebody;


                string sProvider = ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString;
                SqlConnection oSQLConn = new SqlConnection(sProvider);
                string SqlStr = "Select * from  " + tblName + " where PatientFU_ID=" + Session["patientFUId"].ToString();
                SqlDataAdapter sqlAdapt = new SqlDataAdapter(SqlStr, oSQLConn);
                DataTable sqlTbl = new DataTable();
                sqlAdapt.Fill(sqlTbl);
                if (sqlTbl.Rows.Count > 0)
                    isExist = true;
                else
                    isExist = false;

                if (!isExist)
                {
                    if (left)
                    {
                        ccbody = ccbody.Replace("#rigthtdiv", "style='display:none' ");
                        pebody = pebody.Replace("#rigthtdiv", "style='display:none' ");

                        ccbody = ccbody.Replace("#leftdiv", "style='display:block' ");
                        pebody = pebody.Replace("#leftdiv", "style='display:block' ");
                    }
                    else if (right)
                    {
                        ccbody = ccbody.Replace("#leftdiv", "style='display:none' ");
                        pebody = pebody.Replace("#leftdiv", "style='display:none' ");

                        ccbody = ccbody.Replace("#rigthtdiv", "style='display:block' ");
                        pebody = pebody.Replace("#rigthtdiv", "style='display:block' ");

                    }
                    query = "insert into " + tblName + " (PatientFU_ID,CCvalue,CCvalueoriginal,PEvalue,PEvalueoriginal)";
                    query += "values(@PatientFU_ID,@CCvalue,@CCvalueoriginal,@PEvalue,@PEvalueoriginal)";

                    using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString))
                    {
                        SqlCommand command = new SqlCommand(query, connection);
                        command.Parameters.AddWithValue("@CCvalue", ccbody);
                        command.Parameters.AddWithValue("@CCvalueoriginal", ccOrg);
                        command.Parameters.AddWithValue("@PEvalue", pebody);
                        command.Parameters.AddWithValue("@PEvalueoriginal", peOrg);

                        command.Parameters.AddWithValue("@PatientFU_ID", Session["patientFUId"].ToString());

                        connection.Open();
                        var results = command.ExecuteNonQuery();
                        connection.Close();
                    }

                }
                else
                {
                    pebody = sqlTbl.Rows[0]["pevalue"].ToString();
                    ccbody = sqlTbl.Rows[0]["ccvalue"].ToString();
                    if (right)
                    {
                        pebody = Regex.Replace(pebody, @"<div id=""WrapRightPE"" .*style='display:none'.*>", @"<div id=""WrapRightPE"" style='display:block'>");
                        ccbody = Regex.Replace(ccbody, @"<div id=""WrapRight"" .*style='display:none'.*>", @"<div id=""WrapRight"" style='display:block'>");

                    }
                    else
                    {
                        ccbody = ReplacePart(@"<div id=""WrapRight""", @"</div>", ccOrg, ccbody);
                        pebody = ReplacePart(@"<div id=""WrapRightPE""", @"</div>", peOrg, pebody);
                        ccbody = ccbody.Replace("#rigthtdiv", "style='display:none' ");
                        pebody = pebody.Replace("#rigthtdiv", "style='display:none' ");
                    }
                    if (left)
                    {
                        pebody = Regex.Replace(pebody, @"<div id=""WrapLeftPE"" .*style='display:none'.*>", @"<div id=""WrapLeftPE"" style='display:block'>");
                        ccbody = Regex.Replace(ccbody, @"<div id=""WrapLeft"" .*style='display:none'.*>", @"<div id=""WrapLeft"" style='display:block'>");

                    }
                    else
                    {
                        ccbody = ReplacePart(@"<div id=""WrapLeft""", @"</div>", ccOrg, ccbody);
                        pebody = ReplacePart(@"<div id=""WrapLeftPE""", @"</div>", peOrg, pebody);
                        ccbody = ccbody.Replace("#leftdiv", "style='display:none' ");
                        pebody = pebody.Replace("#leftdiv", "style='display:none' ");
                    }
                    //ccbody = ReplacePart ( ccbody.Replace("#leftdiv", "style='display:none'");
                    //pebody = pebody.Replace("#leftdiv", "style='display:none'");
                    //ccbody = ReplacePart(@"<div id=""WrapRight""", @"</div>", ccOrg, ccbody);
                    //pebody = ReplacePart(@"<div id=""WrapRightPE""", @"</div>", ccOrg, ccbody);

                    //ccbody = ccbody.Replace("#rigthtdiv", "style='display:none'");
                    //pebody = pebody.Replace("#rigthtdiv", "style='display:none'");

                    query = "update " + tblName + " set  CCvalue=@CCvalue,PEvalue=@PEvalue where ";
                    query += "  PatientFU_ID=@PatientFU_ID ";

                    using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString))
                    {
                        SqlCommand command = new SqlCommand(query, connection);
                        command.Parameters.AddWithValue("@CCvalue", ccbody);
                        command.Parameters.AddWithValue("@PEvalue", pebody);
                        command.Parameters.AddWithValue("@PatientFU_ID", Session["patientFUId"].ToString());

                        connection.Open();
                        var results = command.ExecuteNonQuery();
                        connection.Close();
                    }

                }



            }
            else
            {
                // if (isDelete == true && otherBody == false)
                //{
                //   query = "delete from  " + tblName + " where PatientIE_ID=" + Session["PatientIE_ID"].ToString();
                // new DBHelperClass().executeQuery(query);
                // }
            }

        }
        catch (Exception ex)
        {
        }
    }

    protected string ReplacePart(string start, string end, string tHtml, string Html)
    {

        //string tHtml = File.ReadAllText(Server.MapPath(TemplateFile));
        int righttstart = tHtml.IndexOf(start);
        int righttend = tHtml.IndexOf(end, righttstart);
        //string template = tHtml.Substring(righttstart, righttend - righttstart) + "\n </div>";
        string template = tHtml.Substring(righttstart, righttend - righttstart) ;
        int rightstart = Html.IndexOf(start);
        int rightend = Html.IndexOf(end, rightstart);
        Html = Html.Remove(rightstart, rightend - rightstart);
        Html = Html.Insert(rightstart, template);
        return Html;

    }
}
