﻿using IntakeSheet.BLL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class EditUser : System.Web.UI.Page
{
    DBHelperClass db = new DBHelperClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["uname"] == null)
            Response.Redirect("Login.aspx");

        if (!IsPostBack)
        {
            GetDesignations();
            GetGroups();
            if (Request["id"] != null)
            {
                string id = Request.QueryString["id"];
                BindUserDetails(id);

            }


        }
    }

    protected void BindUserDetails(string userId = null)
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString))
        {
            SqlCommand cmd = new SqlCommand("GetAllUser", con);
            cmd.Parameters.AddWithValue("@UserID", userId);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);

            if (ds.Tables[0].Rows.Count > 0)
            {
                txtEmail.Text = ds.Tables[0].Rows[0]["eMailID"].ToString();
                txtFirstName.Text = ds.Tables[0].Rows[0]["FirstName"].ToString();
                txtLastName.Text = ds.Tables[0].Rows[0]["LastName"].ToString();
                txtLoginID.Text = ds.Tables[0].Rows[0]["LoginID"].ToString();
                txtMiddleName.Text = ds.Tables[0].Rows[0]["MiddleName"].ToString();
                txtPhoneNo.Text = ds.Tables[0].Rows[0]["Ph_No"].ToString();
                txtAddress.Text = ds.Tables[0].Rows[0]["Address"].ToString();
                //Session["chkreport"] = ds.Tables[0].Rows[0]["reports"].ToString();
                //Session["chkrole"] = ds.Tables[0].Rows[0]["role_id"].ToString();
                //  txtDesignation.Text = ds.Tables[0].Rows[0]["Designation"].ToString();

                ddlDesig.ClearSelection();
                if (ds.Tables[0].Rows[0]["desig_id"] != null && ds.Tables[0].Rows[0]["desig_id"].ToString() != "")
                    ddlDesig.Items.FindByValue(ds.Tables[0].Rows[0]["desig_id"].ToString()).Selected = true;

                ddlGroup.ClearSelection();
                if (ds.Tables[0].Rows[0]["GroupId"] != null && ds.Tables[0].Rows[0]["GroupId"].ToString() != "")
                    ddlGroup.Items.FindByValue(ds.Tables[0].Rows[0]["GroupId"].ToString()).Selected = true;
                txtuserpass.Attributes.Add("value", ds.Tables[0].Rows[0]["Password"].ToString());
            }
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            string query = "";






            if (Request["id"] != null)
            {
                query = "update tblUserMaster set LoginID='" + txtLoginID.Text + "',Password='" + txtuserpass.Text + "',GroupId=" + ddlGroup.SelectedItem.Value + ",designation='" + ddlDesig.SelectedItem.Text + "',";
                query = query + " desig_id=" + ddlDesig.SelectedItem.Value + ",FirstName='" + txtFirstName.Text + "',";
                query = query + " LastName='" + txtLastName.Text + "',MiddleName='" + txtMiddleName.Text + "',";
                query = query + " Address='" + txtAddress.Text + "',Ph_No='" + txtPhoneNo.Text + "',";
                query = query + " eMailID='" + txtEmail.Text + "' where User_ID=" + Request["id"].ToString();
            }
            else
            {
                query = "insert into tblUserMaster(LoginID,Password,Designation,FirstName,LastName,MiddleName,eMailID,Signature,CreatedBy,CreatedDate,desig_id,GroupId,UserMasterId,,Address,Ph_No,designation) values('" + txtLoginID.Text + "','" + txtuserpass.Text + "','',";
                query = query + " '" + txtFirstName.Text + "','" + txtLastName.Text + "','" + txtMiddleName.Text + "','" + txtEmail.Text + "',Null,'admin',GETDATE()," + ddlDesig.SelectedItem.Value + ",'BHFPC'," + ddlGroup.SelectedItem.Value + ",'" + txtAddress.Text + "','" + txtPhoneNo.Text + "','" + ddlDesig.SelectedItem.Text + "') ";
            }

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString))
            {
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.CommandType = CommandType.Text;

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();


            }
            Response.Redirect("ManageUser.aspx");
        }
        catch (Exception ex)
        {
        }
    }

    public void GetDesignations()
    {
        try
        {
            DataSet ds = db.selectData("select * from tbl_designation");

            ddlDesig.DataSource = ds;
            ddlDesig.DataBind();
        }
        catch (Exception ex)
        {

        }

    }

    public void GetGroups()
    {
        try
        {
            DataSet ds = db.selectData("select * from tblGroups");

            ddlGroup.DataSource = ds;
            ddlGroup.DataBind();
        }
        catch (Exception ex)
        {

        }

    }

}