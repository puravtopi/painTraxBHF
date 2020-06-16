using IntakeSheet.DAL;
using IntakeSheet.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace IntakeSheet.BLL
{



    public class ProcedureEqualityComparer : IEqualityComparer<Procedure>
    {
        #region IEqualityComparer<Procedure> Members

        public bool Equals(Procedure x, Procedure y)
        {
            return x.MCode == y.MCode;
        }

        public int GetHashCode(Procedure obj)
        {
            //throw new NotImplementedException();
            return 0;
        }

        #endregion
    }

    public class subProcedureEqualityComparer : IEqualityComparer<ProcedureDetail>
    {
        #region IEqualityComparer<Procedure> Members

        public bool Equals(ProcedureDetail x, ProcedureDetail y)
        {
            return x.SubProcedure == y.SubProcedure;
        }

        public int GetHashCode(ProcedureDetail obj)
        {
            //throw new NotImplementedException();
            return 0;
        }

        #endregion
    }

    public class SubProcedureEqualityComparer : IEqualityComparer<SubProcedure>
    {
        public bool Equals(SubProcedure x, SubProcedure y)
        {
            return x.SubProcedureName == y.SubProcedureName;
        }

        public int GetHashCode(SubProcedure obj)
        {
            //throw new NotImplementedException();
            return 0;
        }
    }
    public class BusinessLogic
    {
        public List<DateTime> getDOES()
        {
            List<DateTime> _dates = new List<DateTime>();
            DataAccess _dal = new DataAccess();
            DataTable _dt = _dal.getDataTable("nusp_GetALLDOE", null);

            foreach (DataRow _dr in _dt.Rows)
            {
                _dates.Add(Convert.ToDateTime(_dr["DOE"]));
            }

            return _dates;
        }
        public List<string[]> getLocations()
        {
            List<string[]> _locations = new List<string[]>();
            DataAccess _dal = new DataAccess();
            DataTable _dt = _dal.getDataTable("nusp_GetALLLocation", null);

            foreach (DataRow _dr in _dt.Rows)
            {
                string[] _location = new string[2];
                _location[1] = _dr["Location"].ToString();
                _location[0] = _dr["Location_ID"].ToString();
                _locations.Add(_location);
            }
            return _locations;
        }
        public List<PatientsByDOE_Result> getPatientsByDOE(DateTime _doe, string _location)
        {
            List<PatientsByDOE_Result> _patients = new List<PatientsByDOE_Result>();
            DataAccess _dal = new DataAccess();
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@DOE", _doe));
            if (_location != "All")
            {
                //string[] _locations = _location.Split('_');
                param.Add(new SqlParameter("@Location_ID", _location));
                //param.Add(new SqlParameter("@City", _locations[0]));
            }
            DataTable _dt = _dal.getDataTable("nusp_GetPatientsByDOE", param);
            foreach (DataRow _dr in _dt.Rows)
            {
                PatientsByDOE_Result _patient = new PatientsByDOE_Result();
                _patient.PatientIEId = Convert.ToInt64(_dr["PatientIE_ID"]);
                _patient.PatientFUId = Convert.ToInt64(_dr["PatientFU_ID"]);
                _patient.FirstName = _dr["FirstName"].ToString();
                _patient.LastName = _dr["LastName"].ToString();
                _patient.DOE = Convert.ToDateTime(_dr["DOE"]);
                _patient.DOA = Convert.IsDBNull(_dr["DOA"]) ? null : (DateTime?)Convert.ToDateTime(_dr["DOA"]);
                _patient.ExamType = _dr["ExamType"].ToString();
                _patient.Location = _dr["Location"].ToString();
                _patient.State = _dr["State"].ToString();
                _patient.City = _dr["City"].ToString();
                _patient.Compensation = _dr["Compensation"].ToString();
                _patients.Add(_patient);
            }
            return _patients;
        }

        public List<PatientsByDOE_Result> getPatientsByDOENew(DateTime _fromdoe, DateTime _todoe, string _location)
        {
            List<PatientsByDOE_Result> _patients = new List<PatientsByDOE_Result>();
            DataAccess _dal = new DataAccess();
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@FROM_DOE", _fromdoe));
            param.Add(new SqlParameter("@TO_DOE", _todoe));
            if (_location != "All")
            {
                //string[] _locations = _location.Split('_');
                param.Add(new SqlParameter("@Location_ID", _location));
                //param.Add(new SqlParameter("@City", _locations[0]));
            }
            DataTable _dt = _dal.getDataTable("nusp_GetPatientsByDOE_new", param);
            foreach (DataRow _dr in _dt.Rows)
            {
                PatientsByDOE_Result _patient = new PatientsByDOE_Result();
                _patient.PatientIEId = Convert.ToInt64(_dr["PatientIE_ID"]);
                _patient.PatientFUId = Convert.ToInt64(_dr["PatientFU_ID"]);
                _patient.FirstName = _dr["FirstName"].ToString();
                _patient.LastName = _dr["LastName"].ToString();
                _patient.DOE = Convert.ToDateTime(_dr["DOE"]);
                _patient.DOA = Convert.IsDBNull(_dr["DOA"]) ? null : (DateTime?)Convert.ToDateTime(_dr["DOA"]);
                _patient.ExamType = _dr["ExamType"].ToString();
                _patient.Location = _dr["Location"].ToString();
                _patient.State = _dr["State"].ToString();
                _patient.City = _dr["City"].ToString();
                _patient.Compensation = _dr["Compensation"].ToString();
                _patients.Add(_patient);
            }
            return _patients;
        }

        public List<PatientsByDOE_Result> getPatientByIE(int patientIE_ID)
        {
            List<PatientsByDOE_Result> _patients = new List<PatientsByDOE_Result>();
            DataAccess _dal = new DataAccess();
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@PatientIE_ID", patientIE_ID));

            DataTable _dt = _dal.getDataTable("nusp_GetPatientDetailsByIE_ID", param);
            foreach (DataRow _dr in _dt.Rows)
            {
                PatientsByDOE_Result _patient = new PatientsByDOE_Result();
                _patient.PatientIEId = Convert.ToInt64(_dr["PatientIE_ID"]);
                _patient.PatientFUId = Convert.ToInt64(_dr["PatientFU_ID"]);
                _patient.FirstName = _dr["FirstName"].ToString();
                _patient.LastName = _dr["LastName"].ToString();
                _patient.DOE = _dr["DOE"] != null ? Convert.ToDateTime(_dr["DOE"]) : DateTime.Now;
                _patient.DOA = Convert.IsDBNull(_dr["DOA"]) ? null : (DateTime?)Convert.ToDateTime(_dr["DOA"]);
                _patient.ExamType = _dr["ExamType"].ToString();
                _patient.Location = _dr["Location"].ToString();
                _patient.State = _dr["State"].ToString();
                _patient.City = _dr["City"].ToString();
                _patient.Compensation = _dr["Compensation"].ToString();
                _patients.Add(_patient);
            }
            return _patients;
        }
        public DataTable getSIDownload(long locationId, DateTime date, out string providerMA)
        {
            List<User> users = new List<User>();
            DataAccess dal = new DataAccess();
            List<SqlParameter> param = new List<SqlParameter>();
            Dictionary<string, string> outputResult = null;

            param.Add(new SqlParameter("@LocationId", locationId));
            param.Add(new SqlParameter("@Date", date));
            SqlParameter oparam = new SqlParameter();
            oparam.DbType = DbType.String;
            oparam.Size = 500;
            oparam.ParameterName = "@ProviderMA";
            oparam.Direction = ParameterDirection.Output;
            param.Add(oparam);
            DataTable dt = dal.getOutParamDataTable("nusp_GetSISheetDetails", param, out outputResult);
            outputResult.TryGetValue("@ProviderMA", out providerMA);
            return dt;
        }
        public List<string> getInjuredParts(Int64 _patientIEId)
        {
            DataAccess _dal = new DataAccess();
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@PatientIE_ID", _patientIEId));
            DataTable _dt = _dal.getDataTable("nusp_GetInjuredBodyParts", param);
            BodyParts _bodyparts = new BodyParts();
            DataRow _newdr = _dt.NewRow();
            foreach (DataRow dr in _dt.Rows)
            {
                _bodyparts.Neck = (Convert.ToBoolean(dr["Neck"])) ? true : _bodyparts.Neck;
                _bodyparts.MidBack = Convert.ToBoolean(dr["MidBack"]) ? true : _bodyparts.MidBack;
                _bodyparts.LowBack = Convert.ToBoolean(dr["LowBack"]) ? true : _bodyparts.LowBack;
                _bodyparts.LeftShoulder = Convert.ToBoolean(dr["LeftShoulder"]) ? true : _bodyparts.LeftShoulder;
                _bodyparts.RightShoulder = Convert.ToBoolean(dr["RightShoulder"]) ? true : _bodyparts.RightShoulder;
                _bodyparts.LeftKnee = Convert.ToBoolean(dr["LeftKnee"]) ? true : _bodyparts.LeftKnee;
                _bodyparts.RightKnee = Convert.ToBoolean(dr["RightKnee"]) ? true : _bodyparts.RightKnee;
                _bodyparts.LeftElbow = Convert.ToBoolean(dr["LeftElbow"]) ? true : _bodyparts.LeftElbow;
                _bodyparts.RightElbow = Convert.ToBoolean(dr["RightElbow"]) ? true : _bodyparts.RightElbow;
                _bodyparts.LeftWrist = Convert.ToBoolean(dr["LeftWrist"]) ? true : _bodyparts.LeftWrist;
                _bodyparts.RightWrist = Convert.ToBoolean(dr["RightWrist"]) ? true : _bodyparts.RightWrist;
                _bodyparts.LeftHip = Convert.ToBoolean(dr["LeftHip"]) ? true : _bodyparts.LeftHip;
                _bodyparts.RightHip = Convert.ToBoolean(dr["RightHip"]) ? true : _bodyparts.RightHip;
                _bodyparts.LeftAnkle = Convert.ToBoolean(dr["LeftAnkle"]) ? true : _bodyparts.LeftAnkle;
                _bodyparts.RightAnkle = Convert.ToBoolean(dr["RightAnkle"]) ? true : _bodyparts.RightAnkle;
                _bodyparts.LeftArm = Convert.ToBoolean(dr["LeftArm"]) ? true : _bodyparts.LeftArm;
                _bodyparts.RightArm = Convert.ToBoolean(dr["RightArm"]) ? true : _bodyparts.RightArm;
                _bodyparts.LeftHand = Convert.ToBoolean(dr["LeftHand"]) ? true : _bodyparts.LeftHand;
                _bodyparts.RightHand = Convert.ToBoolean(dr["RightHand"]) ? true : _bodyparts.RightHand;
                _bodyparts.LeftLeg = Convert.ToBoolean(dr["LeftLeg"]) ? true : _bodyparts.LeftLeg;
                _bodyparts.RightLeg = Convert.ToBoolean(dr["RightLeg"]) ? true : _bodyparts.RightLeg;
                _bodyparts.LeftFoot = Convert.ToBoolean(dr["LeftFoot"]) ? true : _bodyparts.LeftFoot;
                _bodyparts.RightFoot = Convert.ToBoolean(dr["RightFoot"]) ? true : _bodyparts.RightFoot;
                _bodyparts.Face = Convert.ToBoolean(dr["Face"]) ? true : _bodyparts.Face;
                _bodyparts.Ribs = Convert.ToBoolean(dr["Ribs"]) ? true : _bodyparts.Ribs;
                _bodyparts.Chest = Convert.ToBoolean(dr["Chest"]) ? true : _bodyparts.Chest;
                _bodyparts.Abdomen = Convert.ToBoolean(dr["Abdomen"]) ? true : _bodyparts.Abdomen;
                _bodyparts.Pelvis = Convert.ToBoolean(dr["Pelvis"]) ? true : _bodyparts.Pelvis;
                _bodyparts.Other = Convert.ToBoolean(dr["Others"]) ? true : _bodyparts.Other;
            }
            _dt.Rows.Clear();
            if (_bodyparts != null)
            {
                _newdr["Neck"] = _bodyparts.Neck;
                _newdr["MidBack"] = _bodyparts.MidBack;
                _newdr["LowBack"] = _bodyparts.LowBack;
                _newdr["LeftShoulder"] = _bodyparts.LeftShoulder;
                _newdr["RightShoulder"] = _bodyparts.RightShoulder;
                _newdr["LeftKnee"] = _bodyparts.LeftKnee;
                _newdr["RightKnee"] = _bodyparts.RightKnee;
                _newdr["LeftElbow"] = _bodyparts.LeftElbow;
                _newdr["RightElbow"] = _bodyparts.RightElbow;
                _newdr["LeftWrist"] = _bodyparts.LeftWrist;
                _newdr["RightWrist"] = _bodyparts.RightWrist;
                _newdr["LeftHip"] = _bodyparts.LeftHip;
                _newdr["RightHip"] = _bodyparts.RightHip;
                _newdr["LeftAnkle"] = _bodyparts.LeftAnkle;
                _newdr["RightAnkle"] = _bodyparts.RightAnkle;
                _newdr["LeftArm"] = _bodyparts.LeftArm;
                _newdr["RightArm"] = _bodyparts.RightArm;
                _newdr["LeftHand"] = _bodyparts.LeftHand;
                _newdr["RightHand"] = _bodyparts.RightHand;
                _newdr["LeftLeg"] = _bodyparts.LeftLeg;
                _newdr["RightLeg"] = _bodyparts.RightLeg;
                _newdr["LeftFoot"] = _bodyparts.LeftFoot;
                _newdr["RightFoot"] = _bodyparts.RightFoot;
                _newdr["Face"] = _bodyparts.Face;
                _newdr["Ribs"] = _bodyparts.Ribs;
                _newdr["Chest"] = _bodyparts.Chest;
                _newdr["Abdomen"] = _bodyparts.Abdomen;
                _newdr["Pelvis"] = _bodyparts.Pelvis;
                _newdr["Others"] = _bodyparts.Other;

            }
            _dt.Rows.Add(_newdr);
            List<string> _injuredParts = new List<string>();
            foreach (DataRow dr in _dt.Rows)
            {
                foreach (DataColumn dc in _dt.Columns)
                {
                    if (Convert.ToBoolean(dr[dc]))
                    {
                        if (dc.ColumnName.Equals("Others"))
                        { _injuredParts.Add("Other"); }
                        else
                        { _injuredParts.Add(dc.ColumnName); }
                    }
                }
            }


            return _injuredParts;

        }

        public List<string> getFUInjuredParts(Int64 PatientFU_ID)
        {
            DataAccess _dal = new DataAccess();
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@PatientFU_ID", PatientFU_ID));
            DataTable _dt = _dal.getDataTable("nusp_GetFUInjuredBodyParts", param);
            BodyParts _bodyparts = new BodyParts();
            DataRow _newdr = _dt.NewRow();
            foreach (DataRow dr in _dt.Rows)
            {
                _bodyparts.Neck = (Convert.ToBoolean(dr["Neck"])) ? true : _bodyparts.Neck;
                _bodyparts.MidBack = Convert.ToBoolean(dr["MidBack"]) ? true : _bodyparts.MidBack;
                _bodyparts.LowBack = Convert.ToBoolean(dr["LowBack"]) ? true : _bodyparts.LowBack;
                _bodyparts.LeftShoulder = Convert.ToBoolean(dr["LeftShoulder"]) ? true : _bodyparts.LeftShoulder;
                _bodyparts.RightShoulder = Convert.ToBoolean(dr["RightShoulder"]) ? true : _bodyparts.RightShoulder;
                _bodyparts.LeftKnee = Convert.ToBoolean(dr["LeftKnee"]) ? true : _bodyparts.LeftKnee;
                _bodyparts.RightKnee = Convert.ToBoolean(dr["RightKnee"]) ? true : _bodyparts.RightKnee;
                _bodyparts.LeftElbow = Convert.ToBoolean(dr["LeftElbow"]) ? true : _bodyparts.LeftElbow;
                _bodyparts.RightElbow = Convert.ToBoolean(dr["RightElbow"]) ? true : _bodyparts.RightElbow;
                _bodyparts.LeftWrist = Convert.ToBoolean(dr["LeftWrist"]) ? true : _bodyparts.LeftWrist;
                _bodyparts.RightWrist = Convert.ToBoolean(dr["RightWrist"]) ? true : _bodyparts.RightWrist;
                _bodyparts.LeftHip = Convert.ToBoolean(dr["LeftHip"]) ? true : _bodyparts.LeftHip;
                _bodyparts.RightHip = Convert.ToBoolean(dr["RightHip"]) ? true : _bodyparts.RightHip;
                _bodyparts.LeftAnkle = Convert.ToBoolean(dr["LeftAnkle"]) ? true : _bodyparts.LeftAnkle;
                _bodyparts.RightAnkle = Convert.ToBoolean(dr["RightAnkle"]) ? true : _bodyparts.RightAnkle;
                _bodyparts.LeftArm = Convert.ToBoolean(dr["LeftArm"]) ? true : _bodyparts.LeftArm;
                _bodyparts.RightArm = Convert.ToBoolean(dr["RightArm"]) ? true : _bodyparts.RightArm;
                _bodyparts.LeftHand = Convert.ToBoolean(dr["LeftHand"]) ? true : _bodyparts.LeftHand;
                _bodyparts.RightHand = Convert.ToBoolean(dr["RightHand"]) ? true : _bodyparts.RightHand;
                _bodyparts.LeftLeg = Convert.ToBoolean(dr["LeftLeg"]) ? true : _bodyparts.LeftLeg;
                _bodyparts.RightLeg = Convert.ToBoolean(dr["RightLeg"]) ? true : _bodyparts.RightLeg;
                _bodyparts.LeftFoot = Convert.ToBoolean(dr["LeftFoot"]) ? true : _bodyparts.LeftFoot;
                _bodyparts.RightFoot = Convert.ToBoolean(dr["RightFoot"]) ? true : _bodyparts.RightFoot;
                _bodyparts.Face = Convert.ToBoolean(dr["Face"]) ? true : _bodyparts.Face;
                _bodyparts.Ribs = Convert.ToBoolean(dr["Ribs"]) ? true : _bodyparts.Ribs;
                _bodyparts.Chest = Convert.ToBoolean(dr["Chest"]) ? true : _bodyparts.Chest;
                _bodyparts.Abdomen = Convert.ToBoolean(dr["Abdomen"]) ? true : _bodyparts.Abdomen;
                _bodyparts.Pelvis = Convert.ToBoolean(dr["Pelvis"]) ? true : _bodyparts.Pelvis;
                _bodyparts.Other = Convert.ToBoolean(dr["Others"]) ? true : _bodyparts.Other;
            }
            _dt.Rows.Clear();
            if (_bodyparts != null)
            {
                _newdr["Neck"] = _bodyparts.Neck;
                _newdr["MidBack"] = _bodyparts.MidBack;
                _newdr["LowBack"] = _bodyparts.LowBack;
                _newdr["LeftShoulder"] = _bodyparts.LeftShoulder;
                _newdr["RightShoulder"] = _bodyparts.RightShoulder;
                _newdr["LeftKnee"] = _bodyparts.LeftKnee;
                _newdr["RightKnee"] = _bodyparts.RightKnee;
                _newdr["LeftElbow"] = _bodyparts.LeftElbow;
                _newdr["RightElbow"] = _bodyparts.RightElbow;
                _newdr["LeftWrist"] = _bodyparts.LeftWrist;
                _newdr["RightWrist"] = _bodyparts.RightWrist;
                _newdr["LeftHip"] = _bodyparts.LeftHip;
                _newdr["RightHip"] = _bodyparts.RightHip;
                _newdr["LeftAnkle"] = _bodyparts.LeftAnkle;
                _newdr["RightAnkle"] = _bodyparts.RightAnkle;
                _newdr["LeftArm"] = _bodyparts.LeftArm;
                _newdr["RightArm"] = _bodyparts.RightArm;
                _newdr["LeftHand"] = _bodyparts.LeftHand;
                _newdr["RightHand"] = _bodyparts.RightHand;
                _newdr["LeftLeg"] = _bodyparts.LeftLeg;
                _newdr["RightLeg"] = _bodyparts.RightLeg;
                _newdr["LeftFoot"] = _bodyparts.LeftFoot;
                _newdr["RightFoot"] = _bodyparts.RightFoot;
                _newdr["Face"] = _bodyparts.Face;
                _newdr["Ribs"] = _bodyparts.Ribs;
                _newdr["Chest"] = _bodyparts.Chest;
                _newdr["Abdomen"] = _bodyparts.Abdomen;
                _newdr["Pelvis"] = _bodyparts.Pelvis;
                _newdr["Others"] = _bodyparts.Other;

            }
            _dt.Rows.Add(_newdr);
            List<string> _injuredParts = new List<string>();
            foreach (DataRow dr in _dt.Rows)
            {
                foreach (DataColumn dc in _dt.Columns)
                {
                    if (Convert.ToBoolean(dr[dc]))
                    {
                        _injuredParts.Add(dc.ColumnName);
                    }
                }
            }


            return _injuredParts;

        }

        public List<Procedure> GetProceduresByBody(string _bodypart, string Position)
        {
            List<Procedure> _procedures = new List<Procedure>();
            DataAccess _dal = new DataAccess();
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@BodyPart", _bodypart));
            param.Add(new SqlParameter("@Position", Position));
            DataTable _dt = _dal.getDataTable("nusp_getProcedureCodesByParts", param);
            foreach (DataRow _dr in _dt.Rows)
            {
                Procedure _procedure = new Procedure();
                _procedure.ProcedureId = Convert.ToInt64(_dr["Procedure_ID"]);
                _procedure.MCode = _dr["MCode"].ToString();
                _procedure.DateType = _dr["DateType"].ToString();
                _procedure.BodyPart = _dr["BodyPart"].ToString();
                _procedure.Heading = _dr["Heading"].ToString();
                _procedure.CCDesc = _dr["CCDesc"].ToString();
                _procedure.PEDesc = _dr["PEDesc"].ToString();
                _procedure.ADesc = _dr["ADesc"].ToString();
                _procedure.PDesc = _dr["PDesc"].ToString();
                _procedure.CF = _dr["CF"] != null ? false : Convert.ToBoolean(_dr["CF"]);
                _procedure.PN = _dr["PN"] != null ? false : Convert.ToBoolean(_dr["PN"]);
                //Convert.ToBoolean(_dr["PN"]);
                _procedures.Add(_procedure);
            }

            return _procedures;
        }

        public List<ProcedureDetail> getProcedureDetail(long _patientIEID, long _patientFUID, string MCODE, string bodyPart)
        {
            List<ProcedureDetail> _proceduredetails = new List<ProcedureDetail>();
            DataAccess _dal = new DataAccess();
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@PatientIE_ID", _patientIEID));
            if (_patientFUID != 0)
                param.Add(new SqlParameter("@PatientFU_ID", _patientFUID));
            param.Add(new SqlParameter("@MCODE", MCODE));
            param.Add(new SqlParameter("@bodyPart", bodyPart));
            DataTable _dt = _dal.getDataTable("nusp_GetProceduresDetail", param);

            foreach (DataRow _dr in _dt.Rows)
            {
                ProcedureDetail _proceduredetail = new ProcedureDetail();
                _proceduredetail.PatientIE_ID = Convert.ToInt64(_dr["PatientIE_ID"]);
                _proceduredetail.PatientFU_ID = Convert.IsDBNull(_dr["PatientFU_ID"]) ? null : (long?)Convert.ToInt64(_dr["PatientFU_ID"]);
                _proceduredetail.ProcedureDetail_ID = Convert.ToInt64(_dr["ProcedureDetail_ID"]);
                _proceduredetail.Procedure_ID = Convert.ToInt64(_dr["Procedure_Master_ID"]);
                _proceduredetail.MCODE = _dr["MCODE"].ToString();
                _proceduredetail.Date = Convert.IsDBNull(_dr["Date"]) ? null : (DateTime?)Convert.ToDateTime(_dr["Date"]);
                _proceduredetail.DateType = _dr["DateType"].ToString();
                _proceduredetail.SubProcedure = _dr["SubProcedure"].ToString();
                long createdby = 0;
                _proceduredetail.CreatedBy = Int64.TryParse(Convert.ToString(_dr["CreatedBy"]), out createdby) ? (long?)createdby : null;
                _proceduredetails.Add(_proceduredetail);
            }

            return _proceduredetails;

        }

        public List<SubProcedure> getSubProcedure(long _procedureId)
        {
            List<SubProcedure> _subprocedures = new List<SubProcedure>();
            DataAccess _dal = new DataAccess();
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Procedure_ID", _procedureId));
            DataTable _dt = _dal.getDataTable("nusp_GetSubProcedures", param);

            foreach (DataRow _dr in _dt.Rows)
            {
                SubProcedure _subprocedure = new SubProcedure();
                _subprocedure.Id = Convert.ToInt64(_dr["SubProcedure_ID"]);
                _subprocedure.SubProcedureName = _dr["SubProcedure"].ToString();
                _subprocedures.Add(_subprocedure);
            }

            return _subprocedures;

        }


        public string saveProcedureDetail(long _procedureId, long _patientIEID, long? _patientFUID, string _MCODE, string _bodyPart, DateTime _date, string _pDesc, long _createdBy, long ProcedureDetail_ID, bool _isFromNew = false)
        {

            DataAccess _dal = new DataAccess();
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Date", _date));
            param.Add(new SqlParameter("@ProcedureMaster_ID", _procedureId));
            param.Add(new SqlParameter("@PatientIE_ID", _patientIEID));

            if (_patientFUID != 0)
                param.Add(new SqlParameter("@PatientFU_ID", _patientFUID));

            param.Add(new SqlParameter("@MCODE", _MCODE));
            param.Add(new SqlParameter("@BodyPart", _bodyPart));
            param.Add(new SqlParameter("@PDesc", _pDesc));
            param.Add(new SqlParameter("@CreatedBy", _createdBy));
            param.Add(new SqlParameter("@IsFromNew", _isFromNew));
            param.Add(new SqlParameter("@ProcedureDetail_ID", ProcedureDetail_ID));
            return _dal.putData("nusp_SaveProceduresDetail", param);

        }

        public string savePatientProcedureDetail(long ProcedureDetailID, long ProcedureMasterID, long _patientIEID, long? _patientFUID, long BodyPartID, long? ProcedureID, DateTime? Consider, DateTime? Requested, DateTime? Scheduled, DateTime? Followup, DateTime? Executed, string BodyPart, string Medication, string Muscle, string Level, string Req_Pos, string Sch_Pos, string Exe_Pos, string FU_Pos, int CreatedBy, int IsFromNew, int? PatientProcedureID, string req, Int16 IsConsidered, string Side, string SubCode, string SignPath = null)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@ProcedureDetailID", ProcedureDetailID));
            param.Add(new SqlParameter("@ProcedureMasterID", ProcedureMasterID));
            //param.Add(new SqlParameter("@SubProcedureID", SubProcedureID));
            //param.Add(new SqlParameter("@BodyPartID", BodyPartID));
            param.Add(new SqlParameter("@PatientIEID", _patientIEID));
            //if (_patientFUID != 0)
            param.Add(new SqlParameter("@ProcedureID", ProcedureID));
            param.Add(new SqlParameter("@PatientFuID", _patientFUID));
            param.Add(new SqlParameter("@Consider", Consider));
            param.Add(new SqlParameter("@Requested", Requested));
            param.Add(new SqlParameter("@Scheduled", Scheduled));
            param.Add(new SqlParameter("@Followup", Followup));
            param.Add(new SqlParameter("@Executed", Executed));
            param.Add(new SqlParameter("@BodyPart", BodyPart));
            param.Add(new SqlParameter("@CreatedBy", CreatedBy));
            param.Add(new SqlParameter("@Medication", Medication));
            param.Add(new SqlParameter("@Muscle", Muscle));
            param.Add(new SqlParameter("@Level", Level));
            param.Add(new SqlParameter("@Req_Pos", Req_Pos));
            param.Add(new SqlParameter("@Sch_Pos", Sch_Pos));
            param.Add(new SqlParameter("@Exe_Pos", Exe_Pos));
            param.Add(new SqlParameter("@FU_Pos", FU_Pos));
            param.Add(new SqlParameter("@IsFromNew", IsFromNew));
            param.Add(new SqlParameter("@PatientProceduresID", PatientProcedureID));
            param.Add(new SqlParameter("@Category", req));
            param.Add(new SqlParameter("@IsConsidered", IsConsidered));
            param.Add(new SqlParameter("@Side", Side));
            param.Add(new SqlParameter("@Subcode", SubCode));
            param.Add(new SqlParameter("@SignPath", SignPath));

            DataAccess _dal = new DataAccess();
            return _dal.putData("nusp_SavePatientProceduresDetailnew", param);

        }


        public string DeletePatientProcedureDetail(long ProcedureDetailID, long ProcedureMasterID, string req)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@ProcedureMasterID", ProcedureMasterID));
            param.Add(new SqlParameter("@ProcedureDetailID", ProcedureDetailID));
            param.Add(new SqlParameter("@Category", req));
            DataAccess _dal = new DataAccess();
            return _dal.putData("nusp_DeletePatientProceduresDetailnew", param);
        }

        public string Saveconsider(long ProcedureMasterID, string BodyPart, int patientIEID)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@procedureDetailID", ProcedureMasterID));
            param.Add(new SqlParameter("@BodyPart", BodyPart));
            param.Add(new SqlParameter("@patientIEID", patientIEID));

            DataAccess _dal = new DataAccess();
            return _dal.putData("nusp_SaveConsider", param);

        }

        public int CheckExecuteStatus(string MCode, string BodyPart, out string premcode, string patientIEID = "0", string patientFUID = "0")
        {
            premcode = "";
                       
            DataAccess _dal = new DataAccess();
            return Convert.ToInt32(  _dal.getScalarDataNew("nusp_check_execute_status", MCode,BodyPart,out premcode,patientIEID,patientFUID));

        }


        public string deleteProcedureDetail(long _procedureId, long _patientIEID, long _patientFUID, string _MCODE, string _bodyPart, long ProcedureDetail_ID)
        {
            DataAccess _dal = new DataAccess();
            List<SqlParameter> param = new List<SqlParameter>();

            param.Add(new SqlParameter("@ProcedureMaster_ID", _procedureId));
            param.Add(new SqlParameter("@PatientIE_ID", _patientIEID));

            if (_patientFUID != 0)
                param.Add(new SqlParameter("@PatientFU_ID", _patientFUID));

            param.Add(new SqlParameter("@MCODE", _MCODE));
            param.Add(new SqlParameter("@BodyPart", _bodyPart));
            param.Add(new SqlParameter("@ProcedureDetail_ID", ProcedureDetail_ID));

            return _dal.putData("nusp_DeleteProcedureDetail", param);
        }

        public string saveProceduresDetail(long _procedureId, long _patientIEID, long _patientFUID, string _MCODE, string _bodyPart, DateTime? _date, string _dateType, string _pDesc, long _createdBy)
        {

            DataAccess _dal = new DataAccess();
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@DateType", ""));

            param.Add(new SqlParameter("@Date", _date));
            param.Add(new SqlParameter("@DateType", _dateType));

            param.Add(new SqlParameter("@ProcedureMaster_ID", _procedureId));
            param.Add(new SqlParameter("@PatientIE_ID", _patientIEID));

            if (_patientFUID != 0)
                param.Add(new SqlParameter("@PatientFU_ID", _patientFUID));

            param.Add(new SqlParameter("@MCODE", _MCODE));
            param.Add(new SqlParameter("@BodyPart", _bodyPart));
            param.Add(new SqlParameter("@PDesc", _pDesc));
            param.Add(new SqlParameter("@CreatedBy", _createdBy));
            return _dal.putData("nusp_SaveProceduresDetail", param);

        }

        public string login(string username, string password)
        {
            DataAccess _dal = new DataAccess();
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@uname", username));
            DataTable _dt = _dal.getDataTable("nusp_Login", param);
            foreach (DataRow _dr in _dt.Rows)
            {
                if (_dr["Password"].ToString() == password)
                {
                    return _dr["User_ID"].ToString();
                }
            }
            return null;
        }

        public List<long> getAllUserIDs()
        {
            DataAccess _dal = new DataAccess();
            List<SqlParameter> param = new List<SqlParameter>();
            DataTable _dt = _dal.getDataTable("nsp_SelectUserMaster", param);
            List<long> _userIDs = new List<long>();
            foreach (DataRow _dr in _dt.Rows)
            {
                _userIDs.Add(Convert.ToInt64(_dr["User_ID"]));
            }
            return _userIDs;
        }
        public int GetProceduresCount(string Mcode, long PatientIE_Id)
        {
            DataAccess _dal = new DataAccess();
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@MCODE", Mcode));
            param.Add(new SqlParameter("@PatientIE_Id", PatientIE_Id));
            DataTable _dt = _dal.getDataTable("nusp_GetProcedureCount", param);
            var data = _dt.Rows[0]["TotalCount"];
            return (data != DBNull.Value) ? Convert.ToInt32(data) : 0;
        }

        public List<User> getAllProvidersAndMA()
        {
            List<User> users = new List<User>();
            DataAccess dal = new DataAccess();
            List<SqlParameter> param = new List<SqlParameter>();
            DataTable dt = dal.getDataTable("nusp_GetProvidersAndMA", param);

            foreach (DataRow dr in dt.Rows)
            {
                User user = new User();
                user.UserId = Convert.ToInt64(dr["User_Id"]);
                user.LoginId = dr["LoginId"].ToString();
                user.Password = dr["Password"].ToString();
                user.FirstName = dr["FirstName"].ToString();
                user.LastName = dr["LastName"].ToString();
                user.MiddleName = dr["MiddleName"].ToString();
                user.EmailId = dr["EmailId"].ToString();
                user.CreatedBy = dr["CreatedBy"].ToString();
                users.Add(user);
            }

            return users;
        }


        public List<GetFuDetailsResult> GetFUDetails(int PatientIEId, string fromDate = "", string toDate = "")
        {
            List<GetFuDetailsResult> getFuDetailsResults = new List<GetFuDetailsResult>();
            DataAccess dal = new DataAccess();
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@PatientIE_Id", PatientIEId));
            DataTable dt = dal.getDataTable("nusp_GetPatientsFUDetails", param);


            string _query = "";

            if (!string.IsNullOrEmpty(fromDate) && !string.IsNullOrEmpty(toDate))
            {
                if (string.IsNullOrEmpty(_query))
                    _query = " DOE between '" + fromDate + "' and '" + toDate + "'";
                else
                    _query = _query + " and (DOE>='" + fromDate + "' and DOE<='" + toDate + "')";
            }
            else if (!string.IsNullOrEmpty(fromDate) && string.IsNullOrEmpty(toDate))
            {
                if (string.IsNullOrEmpty(_query))
                    _query = " DOE=" + fromDate + "'";
                else
                    _query = _query + " and DOE='" + fromDate + "'";
            }

            if (!string.IsNullOrEmpty(_query))
            {
                try
                {
                    dt = dt.Select(_query).CopyToDataTable();
                }
                catch (Exception ex)
                {
                    dt = null;
                }
            }

            foreach (DataRow dr in dt.Rows)
            {
                GetFuDetailsResult getFuDetailsResult = new GetFuDetailsResult();
                getFuDetailsResult.Sex = dr["Sex"].ToString();
                getFuDetailsResult.PatientId = Convert.ToInt32(dr["Patient_ID"]);
                getFuDetailsResult.PatientFUId = Convert.ToInt32(dr["PatientFU_ID"]);
                getFuDetailsResult.PatientIEId = Convert.ToInt32(dr["PatientIE_ID"]);
                getFuDetailsResult.FirstName = dr["FirstName"].ToString();
                getFuDetailsResult.LastName = dr["LastName"].ToString();
                getFuDetailsResult.Location = dr["Location"].ToString();
                getFuDetailsResult.MAProviders = dr["MA_Providers"].ToString();
                getFuDetailsResult.PrintStatus = dr["PrintStatus"].ToString();
                getFuDetailsResult.DOE = (dr["DOE"] != DBNull.Value) ? Convert.ToDateTime(dr["DOE"]) : new DateTime();
                getFuDetailsResult.DOA = (dr["DOA"] != DBNull.Value) ? Convert.ToDateTime(dr["DOA"]) : new DateTime();
                getFuDetailsResult.DOEIE = (dr["DOEIE"] != DBNull.Value) ? Convert.ToDateTime(dr["DOEIE"]) : new DateTime();
                getFuDetailsResult.Compensation = Convert.ToString(dr["Compensation"]);
                getFuDetailsResult.PrintStatusRod = Convert.ToString(dr["PrintStatusRod"]);
                getFuDetailsResults.Add(getFuDetailsResult);
            }

            return getFuDetailsResults;
        }

        public void testCF(string id, string val, string orgval, string painradiation, string painradiation1)
        {
            SqlConnection oSQLConn = new SqlConnection();
            SqlCommand oSQLCmd = new SqlCommand();

            string _ieMode = "";

            string sProvider = System.Configuration.ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString;
            string SqlStr = "";
            oSQLConn.ConnectionString = sProvider;
            oSQLConn.Open();
            SqlStr = "Select * from tblTest WHERE id = " + id;
            SqlDataAdapter sqlAdapt = new SqlDataAdapter(SqlStr, oSQLConn);
            SqlCommandBuilder sqlCmdBuilder = new SqlCommandBuilder(sqlAdapt);
            DataTable sqlTbl = new DataTable();
            sqlAdapt.Fill(sqlTbl);
            DataRow TblRow;

            if (sqlTbl.Rows.Count == 0)
                _ieMode = "New";
            else
                _ieMode = "Update";

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
                TblRow["id"] = id;
                TblRow["value"] = val;
                TblRow["valueoriginal"] = orgval;
                TblRow["painradiation"] = painradiation;

                TblRow["painradiation1"] = painradiation1;

                if (_ieMode == "New")
                {
                    sqlTbl.Rows.Add(TblRow);
                }
                sqlAdapt.Update(sqlTbl);
            }

            if (TblRow != null)
                TblRow.Table.Dispose();
            sqlTbl.Dispose();
            sqlCmdBuilder.Dispose();
            sqlAdapt.Dispose();
            oSQLConn.Close();


        }

    }
}
