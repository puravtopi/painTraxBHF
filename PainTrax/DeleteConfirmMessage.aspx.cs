using IntakeSheet.BLL;
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class DeleteConfirmMessage : System.Web.UI.Page
{
    ILog log = log4net.LogManager.GetLogger(typeof(DeleteConfirmMessage));
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            if (Request["id"] != null)
            {
                bindData();
                BindPatientIEDetails(Request["id"].ToString());
            }
        }
    }



    private void bindData()
    {
        DataSet lds = null;
        int totalrecords = 0;

        using (SqlConnection gConn = new SqlConnection(ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString))
        {
            try
            {
                string cnd = " where Patient_ID=" + Request["id"];
                SqlCommand gComm = new SqlCommand("nusp_GetPatientMaster", gConn);

                gComm.CommandType = CommandType.StoredProcedure;
                gComm.Parameters.AddWithValue("@PageIndex", 1);
                gComm.Parameters.AddWithValue("@cnd", cnd);

                gComm.Parameters.AddWithValue("@ordercolumn", "FirstName");
                gComm.Parameters.AddWithValue("@sortorder", "asc");

                gComm.Parameters.AddWithValue("@PageSize", 10);
                gComm.Parameters.Add("@RecordCount", SqlDbType.Int, 4);
                gComm.Parameters["@RecordCount"].Direction = ParameterDirection.Output;

                gConn.Open();
                SqlDataAdapter lda = new SqlDataAdapter(gComm);
                lds = new DataSet();
                lda.Fill(lds);


                totalrecords = Convert.ToInt32(gComm.Parameters["@RecordCount"].Value);
                gConn.Close();

                if (totalrecords > 0)
                {
                    txtAccountNo.Text = lds.Tables[0].Rows[0]["AccountNumber"].ToString();
                    txtAdd1.Text = lds.Tables[0].Rows[0]["Address1"].ToString();
                    txtAdd2.Text = lds.Tables[0].Rows[0]["Address2"].ToString();
                    txtCity.Text = lds.Tables[0].Rows[0]["City"].ToString();

                    txtDOB.Text = string.IsNullOrEmpty(lds.Tables[0].Rows[0]["DOB"].ToString()) ? "" : Convert.ToDateTime(lds.Tables[0].Rows[0]["DOB"].ToString()).ToString("MM/dd/yyyy");
                    txtEmail.Text = lds.Tables[0].Rows[0]["eMail"].ToString();
                    txtFirstName.Text = lds.Tables[0].Rows[0]["FirstName"].ToString();
                    txtLastName.Text = lds.Tables[0].Rows[0]["LastName"].ToString();
                    txtMiddleName.Text = lds.Tables[0].Rows[0]["MiddleName"].ToString();
                    txtPhone1.Text = lds.Tables[0].Rows[0]["Phone"].ToString();
                    txtPhone2.Text = lds.Tables[0].Rows[0]["Phone2"].ToString();
                    txtPolicyNo.Text = lds.Tables[0].Rows[0]["Phone2"].ToString();
                    txtSSN.Text = lds.Tables[0].Rows[0]["SSN"].ToString();
                    txtState.Text = lds.Tables[0].Rows[0]["State"].ToString();
                    txtWorkPhone.Text = lds.Tables[0].Rows[0]["work_phone"].ToString();
                    txtZip.Text = lds.Tables[0].Rows[0]["Zip"].ToString();


                    txtHandedness.Text = lds.Tables[0].Rows[0]["Handedness"].ToString();

                    txtSex.Text = lds.Tables[0].Rows[0]["Sex"].ToString();

                    ViewState["img"] = lds.Tables[0].Rows[0]["Photo"].ToString();

                }
            }

            catch (Exception ex)
            {
                gConn.Close();
                log.Error(ex.Message);
            }
        }

    }



    protected void BindPatientIEDetails(string patientId = null, string searchText = null)
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString))
        {
            SqlCommand cmd = new SqlCommand("nusp_GetPatientIEDetails", con);


            cmd.Parameters.AddWithValue("@Patient_Id", patientId);
            cmd.Parameters.AddWithValue("@SearchText", null);
            cmd.Parameters.AddWithValue("@LocationId", null);
            cmd.Parameters.AddWithValue("@SDate", null);
            cmd.Parameters.AddWithValue("@EDate", null);



            cmd.CommandType = CommandType.StoredProcedure;
            con.Open();
            DataTable dt = new DataTable();
            dt.Load(cmd.ExecuteReader());

            string _query = "";
            DataRow row;


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

        }
    }


    protected void gvPatientDetails_RowDataBound(object sender, GridViewRowEventArgs e)
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

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            string SP = "nusp_Delete_PatientMaster";
            SqlParameter[] param = new SqlParameter[1];



            param[0] = new SqlParameter("@Patient_ID", Request["id"].ToString());


            int val = new DBHelperClass().executeSP(SP, param);

            if (val > 0)
            {
                Response.Redirect("ViewPatientMaster.aspx");
            }
            else
            {
                divfail.Attributes.Add("style", "display:block");
            }
        }
        catch (Exception ex)
        {
            log.Error(ex.Message);
        }
    }
}