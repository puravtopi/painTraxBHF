using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace IntakeSheet.DAL
{
    public class DataAccess
    {
        string constr;
        public DataAccess()
        {
            constr = System.Configuration.ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString;
        }
        //string constr = ConfigurationManager.ConnectionStrings["constrdbSSS"].ConnectionString;
        // string constr = "Data Source=.;Initial Catalog=dbPainTraxX2;uid=sa;pwd=sql";
        //string constr = "Data Source=.;Initial Catalog=dbPainTraxX2;Integrated Security=true";
        //string constr = "Data Source=.;uid=sa;pwd=Annie123;Initial Catalog=dbPainTraxXUAT";
        public string putData(string sp, List<SqlParameter> param)
        {
            using (SqlConnection con = new SqlConnection(constr))
            {
                //string alert = "0";
                SqlCommand cmd = new SqlCommand(sp, con);
                cmd.CommandType = CommandType.StoredProcedure;
                foreach (SqlParameter sparam in param)
                {
                    cmd.Parameters.Add(sparam);
                }
                //SqlParameter p = new SqlParameter("@palert", SqlDbType.Char, 1);
                //p.Direction = ParameterDirection.Output;
                //cmd.Parameters.Add(p);
                try
                {
                    con.Open();
                    //if (cmd.ExecuteNonQuery() > 0)
                    //{
                    //    alert = cmd.Parameters["@palert"].Value.ToString();
                    //}
                    return cmd.ExecuteNonQuery().ToString();
                }
                catch (Exception ex)
                {
                    return ex.Message.ToString();
                }
                finally
                {
                    con.Close();
                }
            }

        }
        public DataTable getDataTable(string sp, List<SqlParameter> param)
        {
            using (SqlConnection con = new SqlConnection(constr))
            {
                SqlCommand cmd = new SqlCommand(sp, con);
                cmd.CommandType = CommandType.StoredProcedure;
                if (param != null)
                    foreach (SqlParameter sparam in param)
                    {
                        cmd.Parameters.Add(sparam);
                    }
                DataTable dt = new DataTable();
                con.Open();
                SqlDataReader sdr = cmd.ExecuteReader();
                dt.Load(sdr);
                return dt;
            }
        }

        public string getchar13string(string sp, List<SqlParameter> param)
        {
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand(sp, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (param != null)
                        foreach (SqlParameter sparam in param)
                        {
                            cmd.Parameters.Add(sparam);
                        }
                    cmd.Parameters["@result"].Direction = ParameterDirection.Output;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                    return cmd.Parameters["@result"].Value.ToString();
                }
            }
        }

        public DataTable getOutParamDataTable(string sp, List<SqlParameter> param, out Dictionary<string, string> outparam)
        {
            outparam = new Dictionary<string, string>();
            using (SqlConnection con = new SqlConnection(constr))
            {
                SqlCommand cmd = new SqlCommand(sp, con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddRange(param.ToArray());
                DataTable dt = new DataTable();
                con.Open();
                SqlDataReader sdr = cmd.ExecuteReader();
                dt.Load(sdr);
                foreach (SqlParameter oparam in param.Where(s => s.Direction == ParameterDirection.Output))
                {
                    outparam.Add(oparam.ParameterName, Convert.ToString(oparam.Value));
                }
                return dt;
            }
        }
        public Object getScalarData(string sp, List<SqlParameter> param)
        {
            SqlConnection con = new SqlConnection(constr);
            SqlCommand cmd = new SqlCommand(sp, con);
            cmd.CommandType = CommandType.StoredProcedure;
            foreach (SqlParameter sparam in param)
            {
                cmd.Parameters.Add(sparam);
            }
            SqlParameter p = new SqlParameter("@palert", SqlDbType.Char, 1);
            p.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(p);
            try
            {
                con.Open();
                Object data = cmd.ExecuteScalar();
                return data;
            }
            finally
            {
                con.Close();
            }
        }

        public Object getScalarDataNew(string sp,string mcode,string bodypart,out string premcode,string patientie_id="0",string patientfu_id="0")
        {
            premcode = "";
            SqlConnection con = new SqlConnection(constr);
            SqlCommand cmd = new SqlCommand(sp, con);
            cmd.CommandType = CommandType.StoredProcedure;


            cmd.Parameters.AddWithValue("@mcode", mcode);
            cmd.Parameters.AddWithValue("@bodypart", bodypart);

            cmd.Parameters.AddWithValue("@patientie_id", patientie_id);
            cmd.Parameters.AddWithValue("@patientfu_id", patientfu_id);

          
            cmd.Parameters.Add("@premcode", SqlDbType.NVarChar, 100);
            cmd.Parameters["@premcode"].Direction = ParameterDirection.Output;

            try
            {
                con.Open();
                Object data = cmd.ExecuteScalar();

                premcode = cmd.Parameters["@premcode"].Value.ToString();
                return data;
            }
            finally
            {
                con.Close();
            }
        }
    }
}
