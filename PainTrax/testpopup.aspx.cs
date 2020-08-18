using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class testpopup : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void lnkprint_Click(object sender, EventArgs e)
    {
        ////try
        ////{
        //PrintDocumentHelper helper = new PrintDocumentHelper();

        //String str = File.ReadAllText(Server.MapPath("~/Template/DocumentPrintIE.html"));

        //string prstrCC = "", prstrPE = "", docname = "";

        //LinkButton lnk = sender as LinkButton;
        //SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString);
        //DBHelperClass db = new DBHelperClass();




        ////page1 printing
        //string query = ("select * from View_PatientIE where PatientIE_ID= " + lnk.CommandArgument + "");
        //DataSet ds = db.selectData(query);


        //docname = CommonConvert.UppercaseFirst(ds.Tables[0].Rows[0]["LastName"].ToString()) + ", " + CommonConvert.UppercaseFirst(ds.Tables[0].Rows[0]["FirstName"].ToString()) + "_" + lnk.CommandArgument + "_IE_" + CommonConvert.DateFormatPrint(ds.Tables[0].Rows[0]["DOE"].ToString()) + "_" + CommonConvert.DateFormatPrint(ds.Tables[0].Rows[0]["DOA"].ToString());

        //string gender = ds.Tables[0].Rows[0]["Sex"].ToString() == "Mr." ? "He" : "She";
        //string sex = ds.Tables[0].Rows[0]["Sex"].ToString() == "Mr." ? "male" : "female";
        //string name = CommonConvert.UppercaseFirst(ds.Tables[0].Rows[0]["FirstName"].ToString()) + " " + ds.Tables[0].Rows[0]["MiddleName"].ToString() + " " + CommonConvert.UppercaseFirst(ds.Tables[0].Rows[0]["LastName"].ToString());
        //string doa = CommonConvert.DateFormat(ds.Tables[0].Rows[0]["DOA"].ToString());
        //string doe = CommonConvert.DateFormat(ds.Tables[0].Rows[0]["DOE"].ToString());
        //str = str.Replace("#patientname", name);
        //str = str.Replace("#dob", CommonConvert.DateFormat(ds.Tables[0].Rows[0]["DOB"].ToString()));
        //str = str.Replace("#doi", doa);
        //str = str.Replace("#dos", doe);
        //string age = ds.Tables[0].Rows[0]["AGE"].ToString();

        //string printpage1str = printPage1(lnk.CommandArgument, age, doa, ds.Tables[0].Rows[0]["Location_Id"].ToString());


        //printpage1str = printpage1str.Replace("#gender", gender);
        //printpage1str = printpage1str.Replace("#lgender", gender.ToLower());
        //printpage1str = printpage1str.Replace("#sex", sex);
        //printpage1str = printpage1str.Replace("#doe", doe);
        //printpage1str = printpage1str.Replace("#name", name);
        //str = str.Replace("#bodyparts", ViewState["bodypart"].ToString());

        //str = str.Replace("#history", printpage1str);

        //string cclist = getBodyPartswithnumber(lnk.CommandArgument, ds.Tables[0].Rows[0]["Compensation"].ToString(), doa);

        //str = str.Replace("#cclist", cclist);


        ////header printing

        //query = ("select * from tblLocations where Location_ID=" + ds.Tables[0].Rows[0]["Location_Id"]);
        //ds = db.selectData(query);

        //if (ds != null && ds.Tables[0].Rows.Count > 0)
        //{
        //    str = str.Replace("#drName", "Dr. " + ds.Tables[0].Rows[0]["DrFName"].ToString() + " " + ds.Tables[0].Rows[0]["DrLName"].ToString());
        //    str = str.Replace("#drlname", ds.Tables[0].Rows[0]["DrLName"].ToString());
        //    str = str.Replace("#address", ds.Tables[0].Rows[0]["Address"].ToString() + "<br/>" + ds.Tables[0].Rows[0]["City"].ToString() + ", " + ds.Tables[0].Rows[0]["State"].ToString() + " " + ds.Tables[0].Rows[0]["Zip"].ToString());
        //}


        //String strheader = File.ReadAllText(Server.MapPath("~/Template/Header/Default.html"));



        ////page1 priting
        //query = ("select topSectionHTML from tblPage1HTMLContent where PatientIE_ID= " + lnk.CommandArgument + "");
        //ds = db.selectData(query);

        //if (ds != null && ds.Tables[0].Rows.Count > 0)
        //{
        //    Dictionary<string, string> page1 = new PrintDocumentHelper().getPage1String(ds.Tables[0].Rows[0]["topSectionHTML"].ToString());

        //    str = str.Replace("#pastmedicalhistory", string.IsNullOrEmpty(page1["txt_PMH"]) ? "" : "<b>PAST MEDICAL HISTORY: </b>" + page1["txt_PMH"].TrimEnd('.') + ".<br />");
        //    str = str.Replace("#pastsurgicalhistory", string.IsNullOrEmpty(page1["PSH"]) ? "" : "<b>PAST SURGICAL HISTORY: </b>" + page1["PSH"].TrimEnd('.') + ".<br/>");
        //    str = str.Replace("#pastmedications", string.IsNullOrEmpty(page1["Medication"]) ? "" : "<b>MEDICATIONS: </b>" + page1["Medication"].TrimEnd('.') + ".<br/>");
        //    str = str.Replace("#allergies", string.IsNullOrEmpty(page1["Allergies"]) ? "" : "<b>ALLERGIES: </b>" + page1["Allergies"].TrimEnd('.').ToUpper() + ".<br/>");
        //    //str = str.Replace("#familyhistory", string.IsNullOrEmpty(page1["FamilyHistory"]) ? "" : "<b>Family History: </b>" + page1["FamilyHistory"].TrimEnd('.') + ".<br/><br/>");
        //    str = str.Replace("#familyhistory", "");

        //}

        //query = ("select socialSectionHTML from tblPage1HTMLContent where PatientIE_ID= " + lnk.CommandArgument + "");
        //ds = db.selectData(query);

        //if (ds != null && ds.Tables[0].Rows.Count > 0)
        //{
        //    string strstatus = new PrintDocumentHelper().getDocumentString(ds.Tables[0].Rows[0]["socialSectionHTML"].ToString());
        //    str = str.Replace("#socialhistory", "<b>SOCIAL HISTORY: </b>" + strstatus.TrimEnd('.').Replace(" .", "") + "<br/>");
        //}
        //else
        //{
        //    str = str.Replace("#socialhistory", "");
        //}

        //query = ("select accidentHTML from tblPage1HTMLContent where PatientIE_ID= " + lnk.CommandArgument + "");
        //ds = db.selectData(query);


        //if (ds != null && ds.Tables[0].Rows.Count > 0)
        //{
        //    Dictionary<string, string> page1_accident = new PrintDocumentHelper().getPage1String(ds.Tables[0].Rows[0]["accidentHTML"].ToString());

        //    string work_status = "", accidentdetails = "", strdod = "";

        //    if (page1_accident.ContainsKey("txt_details"))
        //    {
        //        if (!string.IsNullOrEmpty(page1_accident["txt_details"]))
        //            accidentdetails = accidentdetails + page1_accident["txt_details"].TrimEnd('.') + ". ";
        //    }


        //    if (page1_accident["rblPatial"] == "true")
        //        strdod = "Partial";
        //    else if (page1_accident["rbl25"] == "true")
        //        strdod = "25%";
        //    else if (page1_accident["rbl50"] == "true")
        //        strdod = "50%";
        //    else if (page1_accident["rbl75"] == "true")
        //        strdod = "75%";
        //    else if (page1_accident["rbl100"] == "true")
        //        strdod = "100%";
        //    else if (page1_accident["rblNone"] == "true")
        //        strdod = "None";

        //    if (!string.IsNullOrEmpty(strdod))
        //        str = str.Replace("#dod", "<b>DEGREE OF DISABILITY: </b>" + strdod);
        //    else
        //        str = str.Replace("#dod", "");

        //    //if (!string.IsNullOrEmpty(page1_accident["txt_accident_desc"]))
        //    //    accidentdetails = accidentdetails + gender + " " + page1_accident["txt_accident_desc"].TrimEnd('.') + ". ";

        //    //str = str.Replace("#accidentdetails", accidentdetails);

        //    if (page1_accident.ContainsKey("txt_vital"))
        //    {
        //        if (!string.IsNullOrEmpty(page1_accident["txt_vital"]))
        //            str = str.Replace("#vital", "<b>VITAL: </b>" + page1_accident["txt_vital"].TrimEnd('.').Replace(" .", "") + "<br/>");
        //    }

        //    //if (page1_accident.ContainsKey("txt_gait_desc"))
        //    //{
        //    //    if (!string.IsNullOrEmpty(page1_accident["txt_gait_desc"]))
        //    //        str = str.Replace("#gait", "<br/><br/><b>GAIT</b>: The patient " + page1_accident["txt_gait_desc"].Trim() + ".");
        //    //}
        //    //else
        //    //    str = str.Replace("#gait", "");

        //    if (page1_accident["rblStatus"] == "true")
        //        work_status = work_status + "Able to go back to work. ";
        //    else if (page1_accident["rblrblStatus1"] == "true")
        //        work_status = work_status + "Working. ";
        //    else if (page1_accident["rblStatus2"] == "true")
        //        work_status = work_status + "Not Working. ";
        //    else if (page1_accident["rblStatus3"] == "true")
        //        work_status = work_status + "Partially Working. ";


        //    if (!string.IsNullOrEmpty(page1_accident["txt_work_status"]))
        //        work_status = work_status + page1_accident["txt_work_status"].TrimEnd('.') + ". ";

        //    if (!string.IsNullOrEmpty(page1_accident["txtMissed"]))
        //        work_status = work_status + page1_accident["txtMissed"] + " ";




        //    str = str.Replace("#work_status", string.IsNullOrEmpty(work_status) ? "" : "<br/><b>WORK STATUS: </b>" + work_status);

        //    string pastinjury = "";
        //    if (page1_accident["rdbinjuyes"] == "true")
        //    {
        //        pastinjury = gender + " had an  injury to " + page1_accident["txt_injur_past_bp"] + " because of a " + page1_accident["txt_injur_past_how"].TrimEnd('.') + ". ";
        //    }

        //    str = str.Replace("#pastinjury", string.IsNullOrEmpty(pastinjury) ? "" : "<b><u>PAST INJURY</u>: </b>" + pastinjury + "<br/><br/>");
        //    //if (page1_accident["rdbdocyes"] == "true")
        //    //{
        //    //    work_status = work_status + gender + " was seen by " + page1_accident["txt_docname"] + " for that injury. ";
        //    //}


        //    if (!string.IsNullOrEmpty(page1_accident["txt_accident_desc_3"].Trim()))
        //        str = str.Replace("#cc1", page1_accident["txt_accident_desc_3"].Trim() + "<br/><br/>");
        //    else
        //        str = str.Replace("#cc1", "");


        //    //if (!string.IsNullOrEmpty(page1_accident["txt_accident_desc_4"].Trim()))
        //    //    str = str.Replace("#cc2", page1_accident["txt_accident_desc_4"].Trim() + "<br/><br/>");
        //    //else
        //    //    str = str.Replace("#cc2", "");

        //    str = str.Replace("#cc2", "");


        //}

        ////treatment priting
        //query = ("Select TreatMentDetails from tblbpOtherPart WHERE PatientIE_ID=" + lnk.CommandArgument + "");
        //ds = db.selectData(query);

        //string treatment = "";
        //if (ds != null && ds.Tables[0].Rows.Count > 0)
        //{
        //    treatment = ds.Tables[0].Rows[0]["TreatMentDetails"].ToString();
        //}

        //if (!string.IsNullOrEmpty(treatment))
        //    str = str.Replace("#treatment", treatment + "<br/>");
        //else
        //    str = str.Replace("#treatment", "");


        ////page2 printing
        //query = ("select * from tblPage2HTMLContent where PatientIE_ID= " + lnk.CommandArgument + "");
        //ds = db.selectData(query);

        //string strRos = "", strRosDenis = "", strComplain = "", strDOD = "", strRestriction = "", strWorkStatus = "", strMedi = "", strAffect = "";

        //if (ds != null && ds.Tables[0].Rows.Count > 0)
        //{
        //    strRos = helper.getDocumentString(ds.Tables[0].Rows[0]["rosSectionHTML"].ToString());


        //    if (!string.IsNullOrEmpty(strRos))
        //        strRos = "The patient admits to " + strRos.TrimEnd() + ". ";

        //    strRosDenis = helper.getDocumentStringDenies(ds.Tables[0].Rows[0]["rosSectionHTML"].ToString());
        //    if (!string.IsNullOrEmpty(strRosDenis))
        //        strRosDenis = "The patient denies " + strRosDenis.TrimEnd() + ".";
        //}
        //str = str.Replace("#ros", strRos + strRosDenis);

        ////strComplain = "";

        ////if (ds != null && ds.Tables[0].Rows.Count > 0)
        ////{
        ////    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["topSectionHTML"].ToString()))
        ////    {
        ////        string cmp = helper.getDocumentString(ds.Tables[0].Rows[0]["topSectionHTML"].ToString());
        ////        if (!string.IsNullOrEmpty(cmp))
        ////            strComplain = "The patient also complains of  " + helper.getDocumentString(ds.Tables[0].Rows[0]["topSectionHTML"].ToString()) + ".";
        ////    }
        ////}

        ////str = str.Replace("#complain", !string.IsNullOrEmpty(strComplain) ? strComplain + "<br/><br/>" : "");


        //if (ds != null && ds.Tables[0].Rows.Count > 0)
        //{
        //    Dictionary<string, string> page2 = new PrintDocumentHelper().getPage1String(ds.Tables[0].Rows[0]["degreeSectionHTML"].ToString());


        //    //if (!string.IsNullOrEmpty(page2["txtGeneral"]))
        //    str = str.Replace("#general", "<br/><b>GENERAL: </b> The patient presents in an uncomfortable state.<br/><br/>");

        //    //if (!string.IsNullOrEmpty(page2["txtGeneral"]))
        //    //    str = str.Replace("#general", string.IsNullOrEmpty(page2["txtGeneral"]) ? "" : "<b>GENERAL: </b>" + page2["txtGeneral"].TrimEnd('.') + ".<br/>");
        //    //if (!string.IsNullOrEmpty(page2["txtHEENT"]))
        //    //    str = str.Replace("#heent", string.IsNullOrEmpty(page2["txtHEENT"]) ? "" : "HEENT: " + page2["txtHEENT"].TrimEnd('.') + ".<br/>");
        //    //if (!string.IsNullOrEmpty(page2["txtOcc"]))
        //    //    str = str.Replace("#occ_head", string.IsNullOrEmpty(page2["txtOcc"]) ? "" : "Occpital headaches: " + page2["txtOcc"].TrimEnd('.') + ".<br/>");
        //    //if (!string.IsNullOrEmpty(page2["txtCCA"]))
        //    //    str = str.Replace("#cca", string.IsNullOrEmpty(page2["txtCCA"]) ? "" : "Chest, Cardiovascular, Abdomen: " + page2["txtCCA"].TrimEnd('.') + ".<br/>");
        //    //if (!string.IsNullOrEmpty(page2["txtPhy"]))
        //    //    str = str.Replace("#phy_ne", string.IsNullOrEmpty(page2["txtPhy"]) ? "" : "Psych/Neuro: " + page2["txtPhy"].Trim().TrimEnd('.') + ".<br/>");
        //    if (!string.IsNullOrEmpty(page2["txtgait"]))
        //        str = str.Replace("#gait", string.IsNullOrEmpty(page2["txtgait"]) ? "" : "<b>GAIT: </b>" + page2["txtgait"].TrimEnd('.') + ".<br/>");


        //    //if (page2["rblPatial"] == "true")
        //    //    strDOD = "Partial";
        //    //else if (page2["rbl25"] == "true")
        //    //    strDOD = "25%";
        //    //else if (page2["rbl50"] == "true")
        //    //    strDOD = "50%";
        //    //else if (page2["rbl75"] == "true")
        //    //    strDOD = "75%";
        //    //else if (page2["rbl100"] == "true")
        //    //    strDOD = "100%";
        //    //else if (page2["rblNone"] == "true")
        //    //    strDOD = "None";

        //    //if (!string.IsNullOrEmpty(strDOD))
        //    //    str = str.Replace("#dod", "<b><u>DEGREE OF DISABILITY</u>: </b>" + strDOD + "<br/>");
        //    //else
        //    //    str = str.Replace("#dod", "");

        //    //if (page2["chkhousework"] == "true")
        //    //    strAffect = "housework, ";
        //    //if (page2["chkwork-related"] == "true")
        //    //    strAffect = strAffect + "job work-related duties, ";
        //    //if (page2["chkdriving"] == "true")
        //    //    strAffect = strAffect + "driving, ";
        //    //if (page2["chksittingincar"] == "true")
        //    //    strAffect = strAffect + "sitting in car, ";
        //    //if (page2["chkwalking"] == "true")
        //    //    strAffect = strAffect + "walking up/down downstairs, ";

        //    if (!string.IsNullOrEmpty(strAffect))
        //        str = str.Replace("#activityaffected", "<b><u>Activities of Daily living affected</u>: </b>" + strAffect.TrimEnd(',') + "<br/>");
        //    else
        //        str = str.Replace("#activityaffected", "");





        //    //if (page2["chkBending"] == "true")
        //    //    strRestriction = "Bending, ";
        //    //if (page2["chkClimbing"] == "true")
        //    //    strRestriction = strRestriction + "Climbing stairs/ladders, ";
        //    //if (page2["chkEnvironmental"] == "true")
        //    //    strRestriction = strRestriction + "Environmental conditions, ";
        //    //if (page2["chkKneeling"] == "true")
        //    //    strRestriction = strRestriction + "Kneeling, ";
        //    //if (page2["chkLifting"] == "true")
        //    //    strRestriction = strRestriction + "Lifting, ";
        //    //if (page2["chkOperatingHeavy"] == "true")
        //    //    strRestriction = strRestriction + "Operating heavy equipment, ";
        //    //if (page2["chkOperatingofmotor"] == "true")
        //    //    strRestriction = strRestriction + "Operation of motor vehicles, ";
        //    //if (page2["chkPersonal"] == "true")
        //    //    strRestriction = strRestriction + "Personal protective equipment, ";
        //    //if (page2["chkSitting"] == "true")
        //    //    strRestriction = strRestriction + "Sitting, ";
        //    //if (page2["chkStanding"] == "true")
        //    //    strRestriction = strRestriction + "Standing, ";
        //    //if (page2["chkUseofPublic"] == "true")
        //    //    strRestriction = strRestriction + "Use of public transportation, ";
        //    //if (page2["chkUseofUpper"] == "true")
        //    //    strRestriction = strRestriction + "Use of upper extremities, ";

        //    if (!string.IsNullOrEmpty(strRestriction))
        //        str = str.Replace("#restriction", "<b><u>RESTRICTION</u>: </b>" + strRestriction.TrimEnd(',') + "<br/>");
        //    else
        //        str = str.Replace("#restriction", "");

        //    //if (page2["chkAbletoWork"] == "true")
        //    //    strWorkStatus = "Able to go back to work " + page2["txtbackwork"] + ", ";
        //    //if (page2["chkWorking"] == "true")
        //    //    strWorkStatus = strWorkStatus + "Working " + page2["txtWorking"] + ", ";
        //    //if (page2["chkNotWorking"] == "true")
        //    //    strWorkStatus = strWorkStatus + "Not Working " + page2["txtNotWorking"] + ", ";
        //    //if (page2["chkPartiallyWorking"] == "true")
        //    //    strWorkStatus = strWorkStatus + "Partially Working " + page2["txtPartiallyWorking"] + ", ";

        //    if (!string.IsNullOrEmpty(strWorkStatus))
        //        str = str.Replace("#workstatus", "<b><u>WORK STATUS</u>: </b>" + strWorkStatus.TrimEnd(',') + "<br/>");
        //    else
        //        str = str.Replace("#workstatus", "");

        //}
        //else
        //{
        //    str = str.Replace("#dod", "");
        //    str = str.Replace("#restriction", "");
        //    str = str.Replace("#workstatus", "");
        //    str = str.Replace("#activityaffected", "");
        //}
        ////page3 printing
        //query = ("select * from tblPage3HTMLContent where PatientIE_ID= " + lnk.CommandArgument + "");
        //ds = db.selectData(query);


        //if (ds != null && ds.Tables[0].Rows.Count > 0)
        //{


        //    Dictionary<string, string> page3 = new PrintDocumentHelper().getPage1String(ds.Tables[0].Rows[0]["topSectionHTML"].ToString());

        //    string strGAIT = !(string.IsNullOrEmpty(page3["txtGAIT"])) ? page3["txtGAIT"] + "." : "";

        //    if (!string.IsNullOrEmpty(page3["txtAmbulates"]))
        //    {
        //        strGAIT = strGAIT + page3["txtAmbulates"].ToString();

        //        if (page3["chkFootslap"] == "true")
        //            strGAIT = strGAIT + ", foot slap/drop";
        //        if (page3["chkKneehyperextension"] == "true")
        //            strGAIT = strGAIT + ", knee hyperextension";
        //        if (page3["chkUnabletohealwalk"] == "true")
        //            strGAIT = strGAIT + ", unable to heel walk";
        //        if (page3["chkUnabletotoewalk"] == "true")
        //            strGAIT = strGAIT + ", unable to toe walk";
        //        if (!string.IsNullOrEmpty(page3["txtOther"]))
        //            strGAIT = strGAIT + " and " + page3["txtOther"];
        //    }

        //    //if (!string.IsNullOrEmpty(strGAIT))
        //    //    str = str.Replace("#gait", "<b><u>GAIT</u>: </b>" + strGAIT + "<br/><br/>");
        //    //else
        //    //    str = str.Replace("#gait", "");


        //    Dictionary<string, string> page3_1 = new PrintDocumentHelper().getPage1String(ds.Tables[0].Rows[0]["HTMLContent"].ToString());

        //    string strNR = "The patient is alert and cooperative and responding appropriately. Cranial nerves - II-XII are grossly intact ";

        //    if (!string.IsNullOrEmpty(page3_1["txtIntactExcept"]))
        //        strNR = strNR + "except " + page3_1["txtIntactExcept"].TrimEnd('.');

        //    if (!string.IsNullOrEmpty(strNR))
        //        str = str.Replace("#nerologicalexam", "<b><u>NEUROLOGICAL EXAM</u>: </b>" + strNR.TrimEnd('.') + ".<br/><br/> ");
        //    else
        //        str = str.Replace("#nerologicalexam", "");






        //    string strExceptions = "";
        //    //if (!string.IsNullOrEmpty(page3_1["txtDTR1"]) && !string.IsNullOrEmpty(page3_1["txtDTR1"]))
        //    //{
        //    //    if (!string.IsNullOrEmpty(page3_1["txtDTR1"]))
        //    //        strExceptions = page3_1["txtDTR1"];
        //    //    if (!string.IsNullOrEmpty(page3_1["txtDTR2"]))
        //    //        strExceptions = strExceptions + " " + page3_1["txtDTR2"];
        //    //}

        //    if (!string.IsNullOrEmpty(page3_1["LTricepstxt"]) && page3_1["LTricepstxt"] != "2")
        //        strExceptions = " left triceps " + page3_1["LTricepstxt"] + "/2";
        //    if (!string.IsNullOrEmpty(page3_1["RTricepstxt"]) && page3_1["RTricepstxt"] != "2")
        //        strExceptions = strExceptions + ", " + "right triceps " + page3_1["RTricepstxt"] + "/2";
        //    if (!string.IsNullOrEmpty(page3_1["LBicepstxt"]) && page3_1["LBicepstxt"] != "2")
        //        strExceptions = strExceptions + ", " + "left biceps " + page3_1["LBicepstxt"] + "/2";
        //    if (!string.IsNullOrEmpty(page3_1["RBicepstxt"]) && page3_1["RBicepstxt"] != "2")
        //        strExceptions = strExceptions + ", " + "right biceps " + page3_1["RBicepstxt"] + "/2";
        //    if (!string.IsNullOrEmpty(page3_1["LBrachioradialis"]) && page3_1["LBrachioradialis"] != "2")
        //        strExceptions = strExceptions + ", " + "left brachioradialis " + page3_1["LBrachioradialis"] + "/2";
        //    if (!string.IsNullOrEmpty(page3_1["RBrachioradialis"]) && page3_1["RBrachioradialis"] != "2")
        //        strExceptions = strExceptions + ", " + "right brachioradialis " + page3_1["RBrachioradialis"] + "/2";

        //    if (!string.IsNullOrEmpty(page3_1["LKnee"]) && page3_1["LKnee"] != "2")
        //        strExceptions = strExceptions + ", left knee " + page3_1["LKnee"] + "/2";
        //    if (!string.IsNullOrEmpty(page3_1["RKnee"]) && page3_1["RKnee"] != "2")
        //        strExceptions = strExceptions + ", " + "right knee " + page3_1["RKnee"] + "/2";
        //    if (!string.IsNullOrEmpty(page3_1["LAnkle"]) && page3_1["LAnkle"] != "2")
        //        strExceptions = strExceptions + ", " + "left ankle " + page3_1["LAnkle"] + "/2";
        //    if (!string.IsNullOrEmpty(page3_1["RAnkle"]) && page3_1["RAnkle"] != "2")
        //        strExceptions = strExceptions + ", " + "right ankle " + page3_1["RAnkle"] + "/2";




        //    if (!string.IsNullOrEmpty(strExceptions))
        //    {
        //        strExceptions = this.FirstCharToUpper(strExceptions.TrimStart());
        //        str = str.Replace("#reflexexam", "<b>REFLEX EXAMINATION: </b>Deep tendon reflexes are 2+ and equal with the following exceptions: " + strExceptions.TrimEnd('.') + ".<br/><br/>");
        //    }
        //    else
        //        str = str.Replace("#reflexexam", "<b>REFLEX EXAMINATION: </b>Deep tendon reflexes are 2+ and equal.<br/><br/>");

        //    string strRE = "", strRElist = "";

        //    //if (page3_1["chkPinPrick"] == "true")
        //    //    strRElist = "pinprick";

        //    //if (page3_1["chkLighttouch"] == "true")
        //    //    strRElist = strRElist + "," + "light touch. ";

        //    //if (!string.IsNullOrEmpty(strRElist))
        //    //    strRElist = "Is checked by " + strRElist.TrimStart(',');


        //    //if (!string.IsNullOrEmpty(page3_1["txtSensory"]))
        //    //    strRElist = strRElist + " It is " + page3_1["txtSensory"];

        //    //strRE = strRElist;

        //    strExceptions = "";
        //    //if (!string.IsNullOrEmpty(page3_1["txtSensory"]))
        //    //    strExceptions = page3_1["txtSensory"].ToString();


        //    if (!string.IsNullOrEmpty(page3_1["LLateralarm"]))
        //        strExceptions = page3_1["LLateralarm"] + " at left lateral arm (C5)";
        //    if (!string.IsNullOrEmpty(page3_1["RLateralarm"]))
        //        strExceptions = page3_1["RLateralarm"] + " at right lateral arm (C5)";

        //    if (!string.IsNullOrEmpty(page3_1["LLateralforearm"]))
        //        strExceptions = strExceptions + ", " + page3_1["LLateralforearm"] + " at left lateral forearm, thumb, index (C6)";
        //    if (!string.IsNullOrEmpty(page3_1["RLateralforearm"]))
        //        strExceptions = strExceptions + ", " + page3_1["RLateralforearm"] + " at right lateral forearm, thumb, index (C6)";

        //    if (!string.IsNullOrEmpty(page3_1["LMiddlefinger"]))
        //        strExceptions = strExceptions + ", " + page3_1["LMiddlefinger"] + " at left middle finger (C7)";
        //    if (!string.IsNullOrEmpty(page3_1["RMiddlefinger"]))
        //        strExceptions = strExceptions + ", " + page3_1["RMiddlefinger"] + " at right middle finger (C7)";

        //    if (!string.IsNullOrEmpty(page3_1["LMidialForearm"]))
        //        strExceptions = strExceptions + ", " + page3_1["LMidialForearm"] + " at left medial forearm, ring, little finger (C8)";
        //    if (!string.IsNullOrEmpty(page3_1["RMidialForearm"]))
        //        strExceptions = strExceptions + ", " + page3_1["RMidialForearm"] + " at right medial forearm, ring, little finger (C8)";

        //    if (!string.IsNullOrEmpty(page3_1["LMidialarm"]))
        //        strExceptions = strExceptions + ", " + page3_1["LMidialarm"] + " at left medial arm (T1)";
        //    if (!string.IsNullOrEmpty(page3_1["RMidialarm"]))
        //        strExceptions = strExceptions + ", " + page3_1["RMidialarm"] + " at right medial arm (T1)";

        //    if (!string.IsNullOrEmpty(page3_1["LCervical"]))
        //        strExceptions = strExceptions + ", " + page3_1["LCervical"] + " at left cervical paraspinals";
        //    if (!string.IsNullOrEmpty(page3_1["RCervical"]))
        //        strExceptions = strExceptions + ", " + page3_1["RCervical"] + " at right cervical paraspinals";

        //    if (!string.IsNullOrEmpty(page3_1["LtxtDMTL3"]))
        //        strExceptions = strExceptions + ", " + page3_1["LtxtDMTL3"] + " at left distal medial thigh (L3)";
        //    if (!string.IsNullOrEmpty(page3_1["RtxtDMTL3"]))
        //        strExceptions = strExceptions + ", " + page3_1["RtxtDMTL3"] + " at right distal medial thigh (L3)";

        //    if (!string.IsNullOrEmpty(page3_1["LtxtMLFL4"]))
        //        strExceptions = strExceptions + ", " + page3_1["LtxtMLFL4"] + " at left medial left foot (L4)";
        //    if (!string.IsNullOrEmpty(page3_1["RtxtMLFL4"]))
        //        strExceptions = strExceptions + ", " + page3_1["RtxtMLFL4"] + " at right medial left foot (L4)";

        //    if (!string.IsNullOrEmpty(page3_1["LtxtDOFL5"]))
        //        strExceptions = strExceptions + ", " + page3_1["LtxtDOFL5"] + " at left dorsum of the foot (L5)";
        //    if (!string.IsNullOrEmpty(page3_1["RtxtDOFL5"]))
        //        strExceptions = strExceptions + ", " + page3_1["RtxtDOFL5"] + " at right dorsum of the foot (L5)";

        //    if (!string.IsNullOrEmpty(page3_1["LtxtLTS1"]))
        //        strExceptions = strExceptions + ", " + page3_1["LtxtLTS1"] + " at left lateral foot (S1)";
        //    if (!string.IsNullOrEmpty(page3_1["RtxtLTS1"]))
        //        strExceptions = strExceptions + ", " + page3_1["RtxtLTS1"] + " at right lateral foot (S1)";

        //    if (!string.IsNullOrEmpty(page3_1["LtxtLP"]))
        //        strExceptions = strExceptions + ", " + page3_1["LtxtLP"] + " at left lumbar paraspinals";
        //    if (!string.IsNullOrEmpty(page3_1["RtxtLP"]))
        //        strExceptions = strExceptions + ", " + page3_1["RtxtLP"] + " at right lumbar paraspinals";



        //    string senexam = strExceptions.Trim(',');

        //    if (!string.IsNullOrEmpty(senexam))
        //    {
        //        senexam = this.FirstCharToUpper(senexam.TrimStart());
        //        str = str.Replace("#sen_exm", "<b>SENSORY EXAMINATION: </b> It is intact to light touch with the exception: " + senexam + ".<br/><br/>");
        //    }
        //    else
        //        str = str.Replace("#sen_exm", "<b>SENSORY EXAMINATION: </b> It is intact to light touch.<br/><br/>");


        //    strExceptions = "";

        //    //if (!string.IsNullOrEmpty(page3_1["txtMST"]))
        //    //    strExceptions = page3_1["txtMST"].ToString();


        //    if (!string.IsNullOrEmpty(page3_1["LAbduction"]))
        //        strExceptions = "left shoulder abduction " + page3_1["LAbduction"] + "/5";
        //    if (!string.IsNullOrEmpty(page3_1["RAbduction"]))
        //        strExceptions = strExceptions + ", " + "right shoulder abduction  " + page3_1["RAbduction"] + "/5";

        //    if (!string.IsNullOrEmpty(page3_1["LFlexion"]))
        //        strExceptions = strExceptions + ", " + "left shoulder flexion " + page3_1["LFlexion"] + "/5";
        //    if (!string.IsNullOrEmpty(page3_1["RFlexion"]))
        //        strExceptions = strExceptions + ", " + "right shoulder flexion " + page3_1["RFlexion"] + "/5";


        //    if (!string.IsNullOrEmpty(page3_1["LElbowExtension"]))
        //        strExceptions = strExceptions + ", " + "left elbow extension " + page3_1["LElbowExtension"] + "/5";
        //    if (!string.IsNullOrEmpty(page3_1["RElbowExtension"]))
        //        strExceptions = strExceptions + ", " + "right elbow extension " + page3_1["RElbowExtension"] + "/5";

        //    if (!string.IsNullOrEmpty(page3_1["LElbowFlexion"]))
        //        strExceptions = strExceptions + ", " + "left elbow flexion " + page3_1["LElbowFlexion"] + "/5";
        //    if (!string.IsNullOrEmpty(page3_1["RElbowFlexion"]))
        //        strExceptions = strExceptions + ", " + "right elbow flexion " + page3_1["RElbowFlexion"] + "/5";

        //    if (!string.IsNullOrEmpty(page3_1["LSupination"]))
        //        strExceptions = strExceptions + ", " + "left elbow supination " + page3_1["LSupination"] + "/5";
        //    if (!string.IsNullOrEmpty(page3_1["RSupination"]))
        //        strExceptions = strExceptions + ", " + "right elbow supination " + page3_1["RSupination"] + "/5";


        //    if (!string.IsNullOrEmpty(page3_1["LPronation"]))
        //        strExceptions = strExceptions + ", " + "left elbow pronation " + page3_1["LPronation"] + "/5";
        //    if (!string.IsNullOrEmpty(page3_1["RPronation"]))
        //        strExceptions = strExceptions + ", " + "right elbow pronation " + page3_1["RPronation"] + "/5";


        //    if (!string.IsNullOrEmpty(page3_1["LWristFlexion"]))
        //        strExceptions = strExceptions + ", " + "left wrist flexion " + page3_1["LWristFlexion"] + "/5";
        //    if (!string.IsNullOrEmpty(page3_1["RWristFlexion"]))
        //        strExceptions = strExceptions + ", " + "right wrist flexion " + page3_1["RWristFlexion"] + "/5";

        //    if (!string.IsNullOrEmpty(page3_1["LWristExtension"]))
        //        strExceptions = strExceptions + ", " + "left wrist extension " + page3_1["LWristExtension"] + "/5";
        //    if (!string.IsNullOrEmpty(page3_1["RWristExtension"]))
        //        strExceptions = strExceptions + ", " + "right wrist extension " + page3_1["RWristExtension"] + "/5";


        //    if (!string.IsNullOrEmpty(page3_1["LGrip"]))
        //        strExceptions = strExceptions + ", " + "left hand grip strength " + page3_1["LGrip"] + "/5";
        //    if (!string.IsNullOrEmpty(page3_1["RGrip"]))
        //        strExceptions = strExceptions + ", " + "right hand grip strength " + page3_1["RGrip"] + "/5";

        //    if (!string.IsNullOrEmpty(page3_1["LFinger"]))
        //        strExceptions = strExceptions + ", " + "left hand finger abduction	 " + page3_1["LFinger"] + "/5";
        //    if (!string.IsNullOrEmpty(page3_1["RFinger"]))
        //        strExceptions = strExceptions + ", " + "right hand finger abduction	 " + page3_1["RFinger"] + "/5";

        //    if (!string.IsNullOrEmpty(page3_1["LHipFlexion"]))
        //        strExceptions = strExceptions + ", " + "left hip flexion " + page3_1["LHipFlexion"] + "/5";
        //    if (!string.IsNullOrEmpty(page3_1["RHipFlexion"]))
        //        strExceptions = strExceptions + ", " + "right hip flexion " + page3_1["RHipFlexion"] + "/5";

        //    if (!string.IsNullOrEmpty(page3_1["LHipAbduction"]))
        //        strExceptions = strExceptions + ", left hip abduction " + page3_1["LHipAbduction"] + "/5";
        //    if (!string.IsNullOrEmpty(page3_1["RHipAbduction"]))
        //        strExceptions = strExceptions + ", " + "right hip abduction " + page3_1["RHipAbduction"] + "/5";

        //    if (!string.IsNullOrEmpty(page3_1["LKneeExtension"]))
        //        strExceptions = strExceptions + ", left knee extension " + page3_1["LKneeExtension"] + "/5";
        //    if (!string.IsNullOrEmpty(page3_1["RKneeExtension"]))
        //        strExceptions = strExceptions + ", " + "right knee extension " + page3_1["RKneeExtension"] + "/5";

        //    if (!string.IsNullOrEmpty(page3_1["LKneeFlexion"]))
        //        strExceptions = strExceptions + ", left knee flexion " + page3_1["LKneeFlexion"] + "/5";
        //    if (!string.IsNullOrEmpty(page3_1["RKneeFlexion"]))
        //        strExceptions = strExceptions + ", " + "right knee flexion " + page3_1["RKneeFlexion"] + "/5";

        //    if (!string.IsNullOrEmpty(page3_1["LDorsiflexion"]))
        //        strExceptions = strExceptions + ", left ankle dorsiflexion " + page3_1["LDorsiflexion"] + "/5";
        //    if (!string.IsNullOrEmpty(page3_1["RDorsiflexion"]))
        //        strExceptions = strExceptions + ", " + "right ankle dorsiflexion " + page3_1["RDorsiflexion"] + "/5";

        //    if (!string.IsNullOrEmpty(page3_1["LPlantar"]))
        //        strExceptions = strExceptions + ", left ankle plantar flexion " + page3_1["LPlantar"] + "/5";
        //    if (!string.IsNullOrEmpty(page3_1["RPlantar"]))
        //        strExceptions = strExceptions + ", " + "right ankle plantar flexion " + page3_1["RPlantar"] + "/5";

        //    if (!string.IsNullOrEmpty(page3_1["LExtensor"]))
        //        strExceptions = strExceptions + ", left ankle extensor hallucis longus " + page3_1["LExtensor"] + "/5";
        //    if (!string.IsNullOrEmpty(page3_1["RExtensor"]))
        //        strExceptions = strExceptions + ", " + "right ankle extensor hallucis longus " + page3_1["RExtensor"] + "/5";


        //    if (!string.IsNullOrEmpty(strExceptions))
        //    {
        //        strExceptions = this.FirstCharToUpper(strExceptions.TrimStart());
        //        str = str.Replace("#mmst", "<b>MOTOR EXAMINATION: </b>Muscle strength is 5/5 normal with the following exceptions: " + strExceptions.TrimStart(',').TrimEnd('.') + ".<br/><br/>");
        //    }
        //    else
        //        str = str.Replace("#mmst", "<b>MOTOR EXAMINATION: </b>Muscle strength is 5/5 normal.<br/><br/>");

        //}
        //else
        //{
        //    str = str.Replace("#nerologicalexam", "");
        //    str = str.Replace("#reflexexam", "");
        //    str = str.Replace("#sensoryexam", "");
        //    str = str.Replace("#motorexam", "");
        //    str = str.Replace("#gait", "");
        //    str = str.Replace("#dtr-ue", "");
        //    str = str.Replace("#dtr-le", "");
        //    str = str.Replace("#sen_exm", "");
        //    str = str.Replace("#mmst", "");

        //}

        ////page4 printing
        //query = "Select * from tblPatientIEDetailPage3 WHERE PatientIE_ID=" + lnk.CommandArgument;
        //ds = db.selectData(query);

        //string strprocedures = "", strCare = "", strDaignosis = "", strshoulderrightmri = "", strshoulderleftmri = "", strkneerighttmri = "", strkneeleftmri = "", stradddaigno = "";

        //if (ds != null && ds.Tables[0].Rows.Count > 0)
        //{


        //    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagCervialBulgeDate"].ToString()))
        //    {
        //        strDaignosis = Convert.ToDateTime(ds.Tables[0].Rows[0]["DiagCervialBulgeDate"].ToString()).ToString("MM/dd/yyyy") + " - ";

        //        if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagCervialBulgeStudy"].ToString()))
        //            strDaignosis = strDaignosis + " " + ds.Tables[0].Rows[0]["DiagCervialBulgeStudy"].ToString() + " of the ";

        //        if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagCervialBulgeText"].ToString()))
        //        {

        //            strDaignosis = strDaignosis + " cervical spine: " + ds.Tables[0].Rows[0]["DiagCervialBulgeText"].ToString() + ",";
        //            stradddaigno = stradddaigno + "Bulges at " + ds.Tables[0].Rows[0]["DiagCervialBulgeText"].ToString().TrimEnd('.') + ".<br/><br/>";
        //        }

        //        if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagCervialBulgeHNP1"].ToString()))
        //        {
        //            strDaignosis = strDaignosis + " HNP at " + ds.Tables[0].Rows[0]["DiagCervialBulgeHNP1"].ToString().TrimEnd('.') + ".";
        //            stradddaigno = stradddaigno + " HNP at " + ds.Tables[0].Rows[0]["DiagCervialBulgeHNP1"].ToString().TrimEnd('.') + ".<br/><br/>";
        //        }

        //        if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagCervialBulgeHNP2"].ToString()))
        //        {
        //            strDaignosis = strDaignosis + ds.Tables[0].Rows[0]["DiagCervialBulgeHNP2"].ToString().TrimEnd('.') + ".";
        //            stradddaigno = stradddaigno + ds.Tables[0].Rows[0]["DiagCervialBulgeHNP2"].ToString().TrimEnd('.') + ".<br/><br/>";
        //        }

        //    }

        //    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagThoracicBulgeDate"].ToString()))
        //    {
        //        strDaignosis = (!string.IsNullOrEmpty(strDaignosis) ? (strDaignosis + "<br/>") : "") + Convert.ToDateTime(ds.Tables[0].Rows[0]["DiagThoracicBulgeDate"].ToString()).ToString("MM/dd/yyyy") + " - ";

        //        if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagThoracicBulgeStudy"].ToString()))
        //            strDaignosis = strDaignosis + " " + ds.Tables[0].Rows[0]["DiagThoracicBulgeStudy"].ToString() + " of the ";

        //        if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagThoracicBulgeText"].ToString()))
        //        {
        //            strDaignosis = strDaignosis + " Thoracic spine " + ds.Tables[0].Rows[0]["DiagThoracicBulgeText"].ToString() + ", ";
        //            stradddaigno = stradddaigno + "Bulges at " + ds.Tables[0].Rows[0]["DiagThoracicBulgeText"].ToString().TrimEnd('.') + ".<br/><br/>";
        //        }

        //        if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagThoracicBulgeHNP1"].ToString()))
        //        {
        //            strDaignosis = strDaignosis + " HNP at " + ds.Tables[0].Rows[0]["DiagThoracicBulgeHNP1"].ToString().TrimEnd('.') + ". ";
        //            stradddaigno = stradddaigno + "HNP at " + ds.Tables[0].Rows[0]["DiagThoracicBulgeHNP1"].ToString().TrimEnd('.') + ".<br/><br/>";
        //        }

        //        if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagThoracicBulgeHNP2"].ToString()))
        //        {
        //            strDaignosis = strDaignosis + ds.Tables[0].Rows[0]["DiagThoracicBulgeHNP2"].ToString().TrimEnd('.') + ". ";
        //            stradddaigno = stradddaigno + ds.Tables[0].Rows[0]["DiagThoracicBulgeHNP2"].ToString().TrimEnd('.') + ".<br/><br/>";
        //        }

        //    }

        //    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagLumberBulgeDate"].ToString()))
        //    {
        //        strDaignosis = (!string.IsNullOrEmpty(strDaignosis) ? (strDaignosis + "<br/>") : "") + Convert.ToDateTime(ds.Tables[0].Rows[0]["DiagLumberBulgeDate"].ToString()).ToString("MM/dd/yyyy") + " - ";

        //        if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagLumberBulgeStudy"].ToString()))
        //            strDaignosis = strDaignosis + " " + ds.Tables[0].Rows[0]["DiagLumberBulgeStudy"].ToString() + " of the ";

        //        if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagLumberBulgeText"].ToString()))
        //        {
        //            strDaignosis = strDaignosis + " Lumbar spine " + ds.Tables[0].Rows[0]["DiagLumberBulgeText"].ToString() + ", ";
        //            stradddaigno = stradddaigno + "Bulges at " + ds.Tables[0].Rows[0]["DiagLumberBulgeText"].ToString().TrimEnd('.') + ".<br/><br/>";
        //        }

        //        if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagLumberBulgeHNP1"].ToString()))
        //        {
        //            strDaignosis = strDaignosis + " HNP at " + ds.Tables[0].Rows[0]["DiagLumberBulgeHNP1"].ToString().TrimEnd('.') + ". ";
        //            stradddaigno = stradddaigno + "HNP at " + ds.Tables[0].Rows[0]["DiagLumberBulgeHNP1"].ToString().TrimEnd('.') + ".<br/><br/>";
        //        }

        //        if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagLumberBulgeHNP2"].ToString()))
        //        {
        //            strDaignosis = strDaignosis + ds.Tables[0].Rows[0]["DiagLumberBulgeHNP2"].ToString().TrimEnd('.') + ". ";
        //            stradddaigno = stradddaigno + ds.Tables[0].Rows[0]["DiagLumberBulgeHNP2"].ToString().TrimEnd('.') + ".<br/><br/>";
        //        }

        //    }

        //    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagLeftShoulderDate"].ToString()))
        //    {
        //        strDaignosis = (!string.IsNullOrEmpty(strDaignosis) ? (strDaignosis + "<br/>") : "") + Convert.ToDateTime(ds.Tables[0].Rows[0]["DiagLeftShoulderDate"].ToString()).ToString("MM/dd/yyyy") + " - ";

        //        if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagLeftShoulderStudy"].ToString()))
        //            strDaignosis = strDaignosis + " " + ds.Tables[0].Rows[0]["DiagLeftShoulderStudy"].ToString() + " of the ";

        //        // if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagLeftShoulderText"].ToString()))
        //        strDaignosis = strDaignosis + " left shoulder " + ds.Tables[0].Rows[0]["DiagLeftShoulderText"].ToString().TrimEnd('.') + ". ";

        //        //if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagLumberBulgeHNP1"].ToString()))
        //        //    strDaignosis = strDaignosis + " HNP at " + ds.Tables[0].Rows[0]["DiagLumberBulgeHNP1"].ToString() + ".";

        //        //if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagLumberBulgeHNP2"].ToString()))
        //        //    strDaignosis = strDaignosis + ds.Tables[0].Rows[0]["DiagLumberBulgeHNP2"].ToString() + ".";

        //    }
        //    //if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagLeftShoulderDate"].ToString()))
        //    //{
        //    //    // strshoulderleftmri = Convert.ToDateTime(ds.Tables[0].Rows[0]["DiagLeftShoulderDate"].ToString()).ToString("MM/dd/yyyy") + " - ";

        //    //    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagLeftShoulderStudy"].ToString()))
        //    //        strshoulderleftmri = "<b>" + ds.Tables[0].Rows[0]["DiagLeftShoulderStudy"].ToString() + " of the left shoulder:</b> ";


        //    //    strshoulderleftmri = strshoulderleftmri + ds.Tables[0].Rows[0]["DiagLeftShoulderText"].ToString().TrimEnd('.') + ". ";


        //    //}

        //    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagRightShoulderDate"].ToString()))
        //    {
        //        strDaignosis = (!string.IsNullOrEmpty(strDaignosis) ? (strDaignosis + "<br/>") : "") + Convert.ToDateTime(ds.Tables[0].Rows[0]["DiagRightShoulderDate"].ToString()).ToString("MM/dd/yyyy") + " - ";

        //        if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagRightShoulderStudy"].ToString()))
        //            strDaignosis = strDaignosis + " " + ds.Tables[0].Rows[0]["DiagRightShoulderStudy"].ToString() + " of the ";

        //        //if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagRightShoulderText"].ToString()))
        //        strDaignosis = strDaignosis + " right shoulder " + ds.Tables[0].Rows[0]["DiagRightShoulderText"].ToString().TrimEnd('.') + ". ";

        //    }

        //    //if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagRightShoulderDate"].ToString()))
        //    //{
        //    //    // strshoulderrightmri = Convert.ToDateTime(ds.Tables[0].Rows[0]["DiagRightShoulderDate"].ToString()).ToString("MM/dd/yyyy") + " - ";

        //    //    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagRightShoulderStudy"].ToString()))
        //    //        strshoulderrightmri = "<b>" + ds.Tables[0].Rows[0]["DiagRightShoulderStudy"].ToString() + " of the right shoulder:</b> ";

        //    //    //if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagRightShoulderText"].ToString()))
        //    //    strshoulderrightmri = strshoulderrightmri + ds.Tables[0].Rows[0]["DiagRightShoulderText"].ToString().TrimEnd('.') + ". ";

        //    //}

        //    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagLeftKneeDate"].ToString()))
        //    {
        //        strDaignosis = (!string.IsNullOrEmpty(strDaignosis) ? (strDaignosis + "<br/>") : "") + Convert.ToDateTime(ds.Tables[0].Rows[0]["DiagLeftKneeDate"].ToString()).ToString("MM/dd/yyyy") + " - ";

        //        if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagLeftKneeStudy"].ToString()))
        //            strDaignosis = strDaignosis + " " + ds.Tables[0].Rows[0]["DiagLeftKneeStudy"].ToString() + " of the ";

        //        // if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagLeftKneeText"].ToString()))
        //        strDaignosis = strDaignosis + " left knee " + ds.Tables[0].Rows[0]["DiagLeftKneeText"].ToString().TrimEnd('.') + ". ";

        //    }
        //    //if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagLeftKneeDate"].ToString()))
        //    //{
        //    //    str = (!string.IsNullOrEmpty(strDaignosis) ? (strDaignosis + "<br/>") : "") + Convert.ToDateTime(ds.Tables[0].Rows[0]["DiagLeftKneeDate"].ToString()).ToString("MM/dd/yyyy") + " - ";

        //    //    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagLeftKneeStudy"].ToString()))
        //    //        strDaignosis = strDaignosis + " " + ds.Tables[0].Rows[0]["DiagLeftKneeStudy"].ToString() + " of the ";

        //    //    // if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagLeftKneeText"].ToString()))
        //    //    strDaignosis = strDaignosis + " left knee " + ds.Tables[0].Rows[0]["DiagLeftKneeText"].ToString().TrimEnd('.') + ". ";

        //    //}
        //    //if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagLeftKneeDate"].ToString()))
        //    //{
        //    //    //strkneeleftmri = Convert.ToDateTime(ds.Tables[0].Rows[0]["DiagLeftKneeDate"].ToString()).ToString("MM/dd/yyyy") + " - ";

        //    //    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagLeftKneeStudy"].ToString()))
        //    //        strkneeleftmri = "<b>" + ds.Tables[0].Rows[0]["DiagLeftKneeStudy"].ToString() + " of the left knee:</b> ";

        //    //    // if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagLeftKneeText"].ToString()))
        //    //    strkneeleftmri = strkneeleftmri + ds.Tables[0].Rows[0]["DiagLeftKneeText"].ToString().TrimEnd('.') + ". ";

        //    //}

        //    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagRightKneeDate"].ToString()))
        //    {
        //        strDaignosis = (!string.IsNullOrEmpty(strDaignosis) ? (strDaignosis + "<br/>") : "") + Convert.ToDateTime(ds.Tables[0].Rows[0]["DiagRightKneeDate"].ToString()).ToString("MM/dd/yyyy") + " - ";

        //        if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagRightKneeStudy"].ToString()))
        //            strDaignosis = strDaignosis + " " + ds.Tables[0].Rows[0]["DiagRightKneeStudy"].ToString() + " of the ";

        //        //  if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagRightKneeText"].ToString()))
        //        strDaignosis = strDaignosis + " right knee " + ds.Tables[0].Rows[0]["DiagRightKneeText"].ToString().TrimEnd('.') + ". ";

        //    }

        //    //if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagRightKneeDate"].ToString()))
        //    //{
        //    //    // strkneerighttmri = Convert.ToDateTime(ds.Tables[0].Rows[0]["DiagRightKneeDate"].ToString()).ToString("MM/dd/yyyy") + " - ";

        //    //    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagRightKneeStudy"].ToString()))
        //    //        strkneerighttmri = "<b>" + ds.Tables[0].Rows[0]["DiagRightKneeStudy"].ToString() + " of the right knee:</b> ";

        //    //    //  if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagRightKneeText"].ToString()))
        //    //    strkneerighttmri = strkneerighttmri + ds.Tables[0].Rows[0]["DiagRightKneeText"].ToString().TrimEnd('.') + ". ";

        //    //}

        //    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["Other1Date"].ToString()))
        //    {
        //        strDaignosis = (!string.IsNullOrEmpty(strDaignosis) ? (strDaignosis + "<br/>") : "") + Convert.ToDateTime(ds.Tables[0].Rows[0]["Other1Date"].ToString()).ToString("MM/dd/yyyy") + " - ";

        //        if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["Other1Study"].ToString()))
        //            strDaignosis = strDaignosis + " " + ds.Tables[0].Rows[0]["Other1Study"].ToString() + " of the ";

        //        if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["Other1Text"].ToString()))
        //            strDaignosis = strDaignosis + ds.Tables[0].Rows[0]["Other1Text"].ToString().TrimEnd('.') + ". ";

        //    }

        //    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["Other2Date"].ToString()))
        //    {
        //        strDaignosis = (!string.IsNullOrEmpty(strDaignosis) ? (strDaignosis + "<br/>") : "") + Convert.ToDateTime(ds.Tables[0].Rows[0]["Other2Date"].ToString()).ToString("MM/dd/yyyy") + " - ";

        //        if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["Other2Study"].ToString()))
        //            strDaignosis = strDaignosis + " " + ds.Tables[0].Rows[0]["Other2Study"].ToString() + " of the ";

        //        if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["Other2Text"].ToString()))
        //            strDaignosis = strDaignosis + ds.Tables[0].Rows[0]["Other2Text"].ToString().TrimEnd('.') + ". ";

        //    }

        //    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["Other3Date"].ToString()))
        //    {
        //        strDaignosis = (!string.IsNullOrEmpty(strDaignosis) ? (strDaignosis + "<br/>") : "") + Convert.ToDateTime(ds.Tables[0].Rows[0]["Other3Date"].ToString()).ToString("MM/dd/yyyy") + " - ";

        //        if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["Other3Study"].ToString()))
        //            strDaignosis = strDaignosis + " " + ds.Tables[0].Rows[0]["Other3Study"].ToString() + " of the ";

        //        if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["Other3Text"].ToString()))
        //            strDaignosis = strDaignosis + ds.Tables[0].Rows[0]["Other3Text"].ToString().TrimEnd('.') + ". ";

        //    }

        //    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["Other4Date"].ToString()))
        //    {
        //        strDaignosis = (!string.IsNullOrEmpty(strDaignosis) ? (strDaignosis + "<br/>") : "") + Convert.ToDateTime(ds.Tables[0].Rows[0]["Other4Date"].ToString()).ToString("MM/dd/yyyy") + " - ";

        //        if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["Other4Study"].ToString()))
        //            strDaignosis = strDaignosis + " " + ds.Tables[0].Rows[0]["Other4Study"].ToString() + " of the ";

        //        if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["Other4Text"].ToString()))
        //            strDaignosis = strDaignosis + ds.Tables[0].Rows[0]["Other4Text"].ToString().TrimEnd('.') + ". ";

        //    }

        //    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["Other5Date"].ToString()))
        //    {
        //        strDaignosis = (!string.IsNullOrEmpty(strDaignosis) ? (strDaignosis + "<br/>") : "") + Convert.ToDateTime(ds.Tables[0].Rows[0]["Other5Date"].ToString()).ToString("MM/dd/yyyy") + " - ";

        //        if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["Other5Study"].ToString()))
        //            strDaignosis = strDaignosis + " " + ds.Tables[0].Rows[0]["Other5Study"].ToString() + " of the ";

        //        if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["Other5Text"].ToString()))
        //            strDaignosis = strDaignosis + ds.Tables[0].Rows[0]["Other5Text"].ToString().TrimEnd('.') + ". ";

        //    }

        //    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["Other6Date"].ToString()))
        //    {
        //        strDaignosis = (!string.IsNullOrEmpty(strDaignosis) ? (strDaignosis + "<br/>") : "") + Convert.ToDateTime(ds.Tables[0].Rows[0]["Other6Date"].ToString()).ToString("MM/dd/yyyy") + " - ";

        //        if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["Other6Study"].ToString()))
        //            strDaignosis = strDaignosis + " " + ds.Tables[0].Rows[0]["Other6Study"].ToString() + " of the ";

        //        if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["Other6Text"].ToString()))
        //            strDaignosis = strDaignosis + ds.Tables[0].Rows[0]["Other6Text"].ToString().TrimEnd('.') + ". ";

        //    }

        //    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["Other7Date"].ToString()))
        //    {
        //        strDaignosis = (!string.IsNullOrEmpty(strDaignosis) ? (strDaignosis + "<br/>") : "") + Convert.ToDateTime(ds.Tables[0].Rows[0]["Other7Date"].ToString()).ToString("MM/dd/yyyy") + " - ";

        //        if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["Other7Study"].ToString()))
        //            strDaignosis = strDaignosis + " " + ds.Tables[0].Rows[0]["Other7Study"].ToString() + " of the ";

        //        if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["Other7Text"].ToString()))
        //            strDaignosis = strDaignosis + ds.Tables[0].Rows[0]["Other7sText"].ToString().TrimEnd('.') + ". ";
        //    }

        //    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["OtherMedicine"].ToString()))
        //    {
        //        strDaignosis = (!string.IsNullOrEmpty(strDaignosis) ? (strDaignosis + "<br/>") : "") + ds.Tables[0].Rows[0]["OtherMedicine"].ToString();
        //    }

        //    query = "Select * from tblMedicationRx WHERE PatientIE_ID = " + lnk.CommandArgument + " Order By Medicine";
        //    DataSet dsDaig = db.selectData(query);

        //    if (dsDaig != null && dsDaig.Tables[0].Rows.Count > 0)
        //    {
        //        for (int i = 0; i < dsDaig.Tables[0].Rows.Count; i++)
        //        {
        //            strDaignosis = (!string.IsNullOrEmpty(strDaignosis) ? (strDaignosis + "<br/>") : "") + dsDaig.Tables[0].Rows[i]["Medicine"].ToString();
        //        }
        //    }


        //    if (!string.IsNullOrEmpty(strDaignosis))
        //        str = str.Replace("#diagnostic", "<b>DIAGNOSTIC STUDIES: </b><br/>" + strDaignosis + "<br/><br/>");
        //    else
        //        str = str.Replace("#diagnostic", "");

        //    if (ds.Tables[0].Rows[0]["IsGoal"].ToString().ToLower() == "true")
        //        str = str.Replace("#goal", "<b>GOALS: </b>" + ds.Tables[0].Rows[0]["GoalText"].ToString() + "<br/><br/>");
        //    else
        //        str = str.Replace("#goal", "");
        //    //str = str.Replace("#goal", "");

        //    strDaignosis = "";


        //    if (CommonConvert.ToBoolean(ds.Tables[0].Rows[0]["Procedures"].ToString()))
        //        strprocedures = "If the patient continues to have tender palpable taut bands/trigger points with referral patterns as noted in the future on examination, I will consider doing trigger point injections. ";

        //    str = str.Replace("#procedures", string.IsNullOrEmpty(strprocedures) ? "<b>PRECAUTIONS: </b>Universal.<br/><br/>" : "<b>PRECAUTIONS: </b>" + strprocedures + "<br/><br/>");

        //    if (CommonConvert.ToBoolean(ds.Tables[0].Rows[0]["Acupuncture"].ToString()))
        //        strCare = strCare + ", Acupuncture";

        //    if (CommonConvert.ToBoolean(ds.Tables[0].Rows[0]["Chiropratic"].ToString()))
        //        strCare = strCare + ", Chiropratic";

        //    if (CommonConvert.ToBoolean(ds.Tables[0].Rows[0]["PhysicalTherapy"].ToString()))
        //        strCare = strCare + ", Physical Therapy";

        //    if (CommonConvert.ToBoolean(ds.Tables[0].Rows[0]["AvoidHeavyLifting"].ToString()))
        //        strCare = strCare + ", Avoid Heavy Lifting";

        //    if (CommonConvert.ToBoolean(ds.Tables[0].Rows[0]["Carrying"].ToString()))
        //        strCare = strCare + ", Carrying";

        //    if (CommonConvert.ToBoolean(ds.Tables[0].Rows[0]["ExcessiveBend"].ToString()))
        //        strCare = strCare + ", ExcessiveBend";

        //    if (CommonConvert.ToBoolean(ds.Tables[0].Rows[0]["ProlongedSitStand"].ToString()))
        //        strCare = strCare + ", ProlongedSitStand";

        //    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["CareOther"].ToString()))
        //        strCare = strCare + ", " + ds.Tables[0].Rows[0]["CareOther"].ToString();

        //    strCare = strCare.TrimStart(',');

        //    StringBuilder sb = new StringBuilder();
        //    sb.Append(strCare);

        //    if (sb.ToString().LastIndexOf(",") > 0)
        //    {
        //        sb.Replace(",", " and ", sb.ToString().LastIndexOf(","), 1);
        //    }

        //    //str = str.Replace("#care", string.IsNullOrEmpty(strCare.TrimStart(',')) ? "" : "<b>CARE: </b>" + sb.ToString().TrimEnd('.') + ".<br/><br/>");


        //    if (ds.Tables[0].Rows[0]["IsCare"].ToString().ToLower() == "true")
        //        str = str.Replace("#care", "<b>CARE: </b>" + ds.Tables[0].Rows[0]["CareText"].ToString() + "<br/><br/>");
        //    else
        //        str = str.Replace("#care", "");

        //    //  str = str.Replace("#care", "<b>CARE: </b> Chiropractic and physical therapy. Avoid heavy lifting, carrying, excessive bending and prolonged sitting and standing.<br/><br/>");


        //    strprocedures = "";

        //    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["Precautions"].ToString()))
        //        strprocedures = ds.Tables[0].Rows[0]["Precautions"].ToString();

        //    string strproceduresTemp = "";

        //    //if (CommonConvert.ToBoolean(ds.Tables[0].Rows[0]["Cardiac"].ToString()))
        //    //    strproceduresTemp = strproceduresTemp + ", Cardiac";

        //    //if (CommonConvert.ToBoolean(ds.Tables[0].Rows[0]["WeightBearing"].ToString()))
        //    //    strproceduresTemp = strproceduresTemp + ", Weight Bearing";



        //    if (!string.IsNullOrEmpty(strproceduresTemp))
        //        strprocedures = strprocedures + strproceduresTemp.TrimStart(',');

        //    if (!string.IsNullOrEmpty(strprocedures))
        //    {
        //        sb = new StringBuilder();
        //        sb.Append(strprocedures);

        //        if (sb.ToString().LastIndexOf(",") > 0)
        //        {
        //            sb.Replace(",", " and ", sb.ToString().LastIndexOf(","), 1);
        //        }

        //        strprocedures = sb.ToString() + ". ";
        //    }

        //    if (CommonConvert.ToBoolean(ds.Tables[0].Rows[0]["EducationProvided"].ToString()))
        //        strprocedures = strprocedures + " Patient education provided via";

        //    if (CommonConvert.ToBoolean(ds.Tables[0].Rows[0]["ViaPhysician"].ToString()))
        //        strprocedures = strprocedures + ", physician";

        //    if (CommonConvert.ToBoolean(ds.Tables[0].Rows[0]["ViaPrintedMaterial"].ToString()))
        //        strprocedures = strprocedures + ", printed material";


        //    if (CommonConvert.ToBoolean(ds.Tables[0].Rows[0]["ViaWebsite"].ToString()))
        //        strprocedures = strprocedures + ", online website references";

        //    if (CommonConvert.ToBoolean(ds.Tables[0].Rows[0]["IsViaVedio"].ToString()))
        //    {
        //        strprocedures = strprocedures + ", video";

        //        if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["ViaVideo"].ToString()))
        //            strprocedures = strprocedures + " about " + ds.Tables[0].Rows[0]["ViaVideo"].ToString();
        //    }



        //    if (!string.IsNullOrEmpty(strprocedures))
        //    {
        //        strprocedures = strprocedures.Trim('.') + ".";

        //        sb = new StringBuilder();
        //        sb.Append(strprocedures);

        //        if (strprocedures.IndexOf("and") == 0)
        //        {
        //            if (sb.ToString().LastIndexOf(",") > 0)
        //            {
        //                sb.Replace(",", " and ", sb.ToString().LastIndexOf(","), 1);
        //            }
        //        }

        //        str = str.Replace("#precautions", string.IsNullOrEmpty(sb.ToString().TrimStart(',')) ? "" : "<b>PRECAUTIONS: </b>" + (sb.ToString().TrimStart(',').TrimEnd('.').Replace(",,", ",").Replace("..", ".")) + ".<br/><br/>");
        //    }
        //    else
        //    {
        //        str = str.Replace("#precautions", "");
        //    }

        //    strComplain = "";
        //    if (CommonConvert.ToBoolean(ds.Tables[0].Rows[0]["ConsultNeuro"].ToString()))
        //        strComplain = strComplain + ", Neurologist";

        //    if (CommonConvert.ToBoolean(ds.Tables[0].Rows[0]["ConsultOrtho"].ToString()))
        //        strComplain = strComplain + ", orthopedist";

        //    if (CommonConvert.ToBoolean(ds.Tables[0].Rows[0]["ConsultPsych"].ToString()))
        //        strComplain = strComplain + ", psychiatrist";

        //    if (CommonConvert.ToBoolean(ds.Tables[0].Rows[0]["ConsultPodiatrist"].ToString()))
        //        strComplain = strComplain + ", podiatrist";


        //    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["ConsultOther"].ToString()))
        //        strComplain = strComplain + ", " + ds.Tables[0].Rows[0]["ConsultOther"].ToString();

        //    sb = new StringBuilder();
        //    sb.Append(strComplain);

        //    if (sb.ToString().LastIndexOf(",") > 0)
        //    {
        //        sb.Replace(",", " and ", sb.ToString().LastIndexOf(","), 1);
        //    }


        //    str = str.Replace("#consultation", string.IsNullOrEmpty(sb.ToString().TrimStart(',')) ? "" : "<b><u>CONSULTATION</u>: </b>" + sb.ToString().ToLower().TrimStart(',') + ".<br/><br/> ");



        //    query = "Select * from tblMedicationRx WHERE PatientIE_ID=" + lnk.CommandArgument;
        //    ds = db.selectData(query);

        //    if (ds != null && ds.Tables[0].Rows.Count > 0)
        //    {
        //        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        //        {
        //            strMedi = strMedi + ds.Tables[0].Rows[i]["Medicine"].ToString() + "<br/>";
        //        }
        //    }

        //    str = str.Replace("#medications", string.IsNullOrEmpty(strMedi) ? "" : "<b><u>MEDICATIONS</u>: </b><br/>" + strMedi + "<br/><br/>");
        //}
        //else
        //{
        //    str = str.Replace("#medications", "");
        //    str = str.Replace("#follow-up", "");
        //    str = str.Replace("#precautions", "");
        //    str = str.Replace("#care", "");
        //    str = str.Replace("#procedures", "");
        //    str = str.Replace("#diagnostic", "");
        //    str = str.Replace("#consultation", "");
        //}

        ////diagnoses printing for all body parts

        //strDaignosis = "";
        //string strDaigNeck = "", strDaigMid = "", strDaigLow = "", strDaigRL = "";

        //strDaigNeck = this.getDiagnosis("Neck", lnk.CommandArgument);
        //strDaigMid = this.getDiagnosis("Midback", lnk.CommandArgument);
        //strDaigLow = this.getDiagnosis("Lowback", lnk.CommandArgument);
        //strDaigRL = this.getDiagnosisRightLeft(lnk.CommandArgument);

        //strDaignosis = strDaigNeck + strDaigMid + strDaigLow + strDaigRL;

        //if (!string.IsNullOrEmpty(stradddaigno))
        //    strDaignosis = "<br/>" + stradddaigno + strDaignosis;

        //if (!string.IsNullOrEmpty(strDaignosis))
        //{
        //    str = str.Replace("#diagnoses", "<b>DIAGNOSES: </b>" + strDaignosis + "<br/><br/>");
        //}
        //else
        //    str = str.Replace("#diagnoses", "");


        ////plan printing for all body parts


        //string strPlan = "";
        //if (!string.IsNullOrEmpty(this.getPOC("Neck", lnk.CommandArgument)))
        //    strPlan = strPlan + "<br/>";
        //strPlan = strPlan + this.getPOC("Neck", lnk.CommandArgument);

        //strPlan = strPlan + (string.IsNullOrEmpty(this.getPlan("tblbpNeck", lnk.CommandArgument)) == false ? this.getPlan("tblbpNeck", lnk.CommandArgument) : "");

        //if (!string.IsNullOrEmpty(this.getPOC("MidBack", lnk.CommandArgument)))
        //    strPlan = strPlan + "<br/>";
        //strPlan = strPlan + this.getPOC("MidBack", lnk.CommandArgument);

        //strPlan = strPlan + (string.IsNullOrEmpty(this.getPlan("tblbpMidback", lnk.CommandArgument)) == false ? this.getPlan("tblbpMidback", lnk.CommandArgument) : "");

        //if (!string.IsNullOrEmpty(this.getPOC("LowBack", lnk.CommandArgument)))
        //    strPlan = strPlan + "<br/>";
        //strPlan = strPlan + this.getPOC("LowBack", lnk.CommandArgument);

        //strPlan = strPlan + (string.IsNullOrEmpty(this.getPlan("tblbpLowback", lnk.CommandArgument)) == false ? this.getPlan("tblbpLowback", lnk.CommandArgument) : "");

        //if (!string.IsNullOrEmpty(this.getPOC("Shoulder", lnk.CommandArgument)))
        //    strPlan = strPlan + "<br/>";
        //strPlan = strPlan + this.getPOC("Shoulder", lnk.CommandArgument);

        //strPlan = strPlan + (string.IsNullOrEmpty(this.getPlan("tblbpShoulder", lnk.CommandArgument)) == false ? this.getPlan("tblbpShoulder", lnk.CommandArgument) : "");

        //if (!string.IsNullOrEmpty(this.getPOC("Knee", lnk.CommandArgument)))
        //    strPlan = strPlan + "<br/>";
        //strPlan = strPlan + this.getPOC("Knee", lnk.CommandArgument);

        //strPlan = strPlan + (string.IsNullOrEmpty(this.getPlan("tblbpKnee", lnk.CommandArgument)) == false ? this.getPlan("tblbpKnee", lnk.CommandArgument) : "");

        //if (!string.IsNullOrEmpty(this.getPOC("Elbow", lnk.CommandArgument)))
        //    strPlan = strPlan + "<br/>";
        //strPlan = strPlan + this.getPOC("Elbow", lnk.CommandArgument);

        //strPlan = strPlan + (string.IsNullOrEmpty(this.getPlan("tblbpElbow", lnk.CommandArgument)) == false ? this.getPlan("tblbpElbow", lnk.CommandArgument) : "");

        //if (!string.IsNullOrEmpty(this.getPOC("Wrist", lnk.CommandArgument)))
        //    strPlan = strPlan + "<br/>";
        //strPlan = strPlan + this.getPOC("Wrist", lnk.CommandArgument);

        //strPlan = strPlan + (string.IsNullOrEmpty(this.getPlan("tblbpWrist", lnk.CommandArgument)) == false ? this.getPlan("tblbpWrist", lnk.CommandArgument) : "");

        //if (!string.IsNullOrEmpty(this.getPOC("Hip", lnk.CommandArgument)))
        //    strPlan = strPlan + "<br/>";
        //strPlan = strPlan + this.getPOC("Hip", lnk.CommandArgument);

        //strPlan = strPlan + (string.IsNullOrEmpty(this.getPlan("tblbpHip", lnk.CommandArgument)) == false ? this.getPlan("tblbpHip", lnk.CommandArgument) : "");

        //if (!string.IsNullOrEmpty(this.getPOC("Ankle", lnk.CommandArgument)))
        //    strPlan = strPlan + "<br/>";
        //strPlan = strPlan + this.getPOC("Ankle", lnk.CommandArgument);

        //strPlan = strPlan + (string.IsNullOrEmpty(this.getPlan("tblbpAnkle", lnk.CommandArgument)) == false ? this.getPlan("tblbpAnkle", lnk.CommandArgument) : "");

        //if (!string.IsNullOrEmpty(this.getPOC("OtherPart", lnk.CommandArgument)))
        //    strPlan = strPlan + "<br/>";
        //strPlan = strPlan + this.getPOC("OtherPart", lnk.CommandArgument);

        //strPlan = strPlan + (string.IsNullOrEmpty(this.getPlan("tblbpOtherPart", lnk.CommandArgument)) == false ? this.getPlan("tblbpOtherPart", lnk.CommandArgument) : "");


        //str = str.Replace("#plan", string.IsNullOrEmpty(strPlan) ? "" : "<br/>" + "<b>RECOMMENDATIONS: </b>" + strPlan);


        ////neck printing string

        //query = ("select CCvalue from tblbpNeck where PatientIE_ID= " + lnk.CommandArgument + "");
        //SqlCommand cm = new SqlCommand(query, cn);
        //SqlDataAdapter da = new SqlDataAdapter(cm);
        //cn.Open();
        //ds = new DataSet();
        //da.Fill(ds);


        //string neckCC = "", neckTP = "", lowbackCC = "", shoudlerCC = "", kneeCC = "", elbowCC = "", wristCC = "", hipCC = "", ankleCC = "";



        //if (ds != null && ds.Tables[0].Rows.Count > 0)
        //{
        //    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["CCvalue"].ToString()))
        //    {
        //        neckCC = helper.getDocumentString(ds.Tables[0].Rows[0]["CCvalue"].ToString());
        //        neckCC = formatString(neckCC);
        //        str = str.Replace("#neck", neckCC + "<br/><br/>");
        //    }
        //    else
        //    {
        //        str = str.Replace("#neck", "");

        //    }
        //}
        //else
        //{
        //    str = str.Replace("#neck", "");

        //}

        ////neck PE printing string
        //query = ("select PEvalue,PESides,PESidesText,NameROM,LeftROM,RightROM,NormalROM,CNameROM,CROM,CNormalROM,TPDesc from tblbpNeck where PatientIE_ID= " + lnk.CommandArgument + "");
        //string neckPE = "";
        //cm = new SqlCommand(query, cn);
        //da = new SqlDataAdapter(cm);
        //ds = new DataSet();
        //da.Fill(ds);

        //if (ds != null && ds.Tables[0].Rows.Count > 0)
        //{
        //    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["PEvalue"].ToString()))
        //    {
        //        neckPE = helper.getDocumentString(ds.Tables[0].Rows[0]["PEvalue"].ToString());
        //        neckTP = ds.Tables[0].Rows[0]["TPDesc"].ToString();
        //        //  neckTP = this.getTPString(ds.Tables[0].Rows[0]["PESides"].ToString(), ds.Tables[0].Rows[0]["PESidesText"].ToString());
        //    }

        //    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["NameROM"].ToString()))
        //    {
        //        string romstrleft = this.getROMString(ds.Tables[0].Rows[0]["NameROM"].ToString(), ds.Tables[0].Rows[0]["LeftROM"].ToString(), ds.Tables[0].Rows[0]["NormalROM"].ToString(), "left", "IE", "Neck");

        //        string romstrright = this.getROMString(ds.Tables[0].Rows[0]["NameROM"].ToString(), ds.Tables[0].Rows[0]["RightROM"].ToString(), ds.Tables[0].Rows[0]["NormalROM"].ToString(), "right", "IE", "Neck");
        //        string romstrC = this.getROMString(ds.Tables[0].Rows[0]["CNameROM"].ToString(), ds.Tables[0].Rows[0]["CROM"].ToString(), ds.Tables[0].Rows[0]["CNormalROM"].ToString(), "", "IE");
        //        string romstr = romstrleft.Replace(".", ";") + " " + romstrright;

        //        string finalrom = "";
        //        if (!string.IsNullOrEmpty(romstrC))
        //        {
        //            romstrC = this.FirstCharToUpper(romstrC.TrimStart());
        //            finalrom = "ROM is as follows: " + romstrC;
        //        }

        //        if (!string.IsNullOrEmpty(romstr) && romstr != " ")
        //        {
        //            if (string.IsNullOrEmpty(romstrC))
        //            {
        //                romstr = this.FirstCharToUpper(romstr.TrimStart());
        //                finalrom = "ROM is as follows: " + romstr.TrimEnd(';') + ".";
        //            }
        //            else
        //                finalrom = finalrom.Replace(".", "") + ";" + romstr.TrimEnd(';') + ".";
        //        }

        //        if (!string.IsNullOrEmpty(neckTP))
        //        {
        //            neckTP = neckTP.TrimStart(',') + ". ";

        //            finalrom = finalrom.Replace("..", ".") + " " + neckTP;
        //        }

        //        if (!string.IsNullOrEmpty(finalrom))

        //            neckPE = neckPE.Replace("#romneck", this.formatString(finalrom));
        //        else
        //            neckPE = neckPE.Replace("#romneck", "");

        //        neckPE = neckPE.Replace("#necknotebp", "");

        //    }


        //    if (!string.IsNullOrEmpty(neckPE))
        //    {
        //        neckPE = formatString(neckPE);
        //        str = str.Replace("#PENeck", "<b>CERVICAL SPINE EXAMINATION: </b>" + neckPE + "<br/><br/>");
        //    }
        //    else
        //        str = str.Replace("#PENeck", "");

        //}
        //else
        //    str = str.Replace("#PENeck", neckPE);


        ////lowback printing string
        //query = ("select CCvalue from tblbpLowback where PatientIE_ID= " + lnk.CommandArgument + "");
        //cm = new SqlCommand(query, cn);
        //da = new SqlDataAdapter(cm);
        //ds = new DataSet();
        //da.Fill(ds);

        //if (ds != null && ds.Tables[0].Rows.Count > 0)
        //{
        //    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["CCvalue"].ToString()))
        //    {
        //        lowbackCC = helper.getDocumentString(ds.Tables[0].Rows[0]["CCvalue"].ToString());
        //        lowbackCC = formatString(lowbackCC);
        //        str = str.Replace("#lowback", lowbackCC + "<br/><br/>");

        //    }
        //    else
        //        str = str.Replace("#lowback", lowbackCC);
        //}
        //else
        //    str = str.Replace("#lowback", lowbackCC);


        ////lowback PE printing string
        //query = ("select PEvalue,PESides,PESidesText,NameROM,LeftROM,RightROM,NormalROM,CNameROM,CROM,CNormalROM,NameTest,LeftTest,RightTest,TextVal,TPDesc  from tblbpLowback where PatientIE_ID= " + lnk.CommandArgument + "");
        //string lowbackPE = "", lowbackTP = "";
        //cm = new SqlCommand(query, cn);
        //da = new SqlDataAdapter(cm);
        //ds = new DataSet();
        //da.Fill(ds);

        //if (ds != null && ds.Tables[0].Rows.Count > 0)
        //{
        //    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["PEvalue"].ToString()))
        //    {
        //        lowbackPE = helper.getDocumentString(ds.Tables[0].Rows[0]["PEvalue"].ToString());
        //        //  lowbackTP = this.getTPString(ds.Tables[0].Rows[0]["PESides"].ToString(), ds.Tables[0].Rows[0]["PESidesText"].ToString());
        //        lowbackTP = ds.Tables[0].Rows[0]["TPDesc"].ToString();


        //    }

        //    string finalrom = "";
        //    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["NameROM"].ToString()))
        //    {
        //        string romstrleft = this.getROMString(ds.Tables[0].Rows[0]["NameROM"].ToString(), ds.Tables[0].Rows[0]["LeftROM"].ToString(), ds.Tables[0].Rows[0]["NormalROM"].ToString(), "left", "IE", "Lowback");
        //        string romstrright = this.getROMString(ds.Tables[0].Rows[0]["NameROM"].ToString(), ds.Tables[0].Rows[0]["RightROM"].ToString(), ds.Tables[0].Rows[0]["NormalROM"].ToString(), "right", "IE", "Lowback");
        //        string romstrC = this.getROMString(ds.Tables[0].Rows[0]["CNameROM"].ToString(), ds.Tables[0].Rows[0]["CROM"].ToString(), ds.Tables[0].Rows[0]["CNormalROM"].ToString(), "", "IE");
        //        string romstr = romstrleft.Replace(",", ";").TrimEnd('.') + " " + romstrright.TrimStart(';');



        //        if (!string.IsNullOrEmpty(romstrC))
        //        {
        //            romstrC = this.FirstCharToUpper(romstrC.TrimStart());
        //            finalrom = "<br/>ROM is as follows: " + romstrC.TrimStart(';') + ". ";
        //        }

        //        if (!string.IsNullOrEmpty(romstr) && romstr != " ")
        //        {
        //            if (string.IsNullOrEmpty(romstrC))
        //            {
        //                romstr = this.FirstCharToUpper(romstr.TrimStart());
        //                finalrom = "ROM is as follows: " + romstr.TrimEnd(';') + ".";
        //            }
        //            else
        //                finalrom = finalrom.Replace(".", "") + ";" + romstr.TrimEnd(';') + ".";
        //        }

        //    }



        //    if (!string.IsNullOrEmpty(lowbackTP))
        //    {
        //        lowbackTP = lowbackTP.TrimStart(',') + ". ";

        //        finalrom = finalrom.Replace("..", ".") + lowbackTP;
        //    }

        //    if (!string.IsNullOrEmpty(finalrom))

        //        lowbackPE = lowbackPE.Replace("#romlowback", finalrom);
        //    else
        //        lowbackPE = lowbackPE.Replace("#romlowback", "");

        //    lowbackPE = lowbackPE.Replace("#lowbacknotebp", "");


        //    //get test string

        //    string strTest = helper.getLowbackTestString(ds.Tables[0].Rows[0]["NameTest"].ToString(), ds.Tables[0].Rows[0]["LeftTest"].ToString(), ds.Tables[0].Rows[0]["RightTest"].ToString(), ds.Tables[0].Rows[0]["TextVal"].ToString());

        //    if (!string.IsNullOrEmpty(strTest))
        //        lowbackPE = lowbackPE + "." + strTest.TrimStart(',') + ".";

        //    if (!string.IsNullOrEmpty(lowbackPE))
        //    {
        //        lowbackPE = formatString(lowbackPE);
        //        str = str.Replace("#PELowback", "<b>LUMBAR SPINE EXAMINATION: </b>" + lowbackPE + "<br/><br/>");
        //    }
        //    else
        //        str = str.Replace("#PELowback", "");

        //}
        //else
        //    str = str.Replace("#PELowback", lowbackPE);

        ////midback printing string
        //string midbackCC = "";
        //query = ("select CCvalue from tblbpMidback where PatientIE_ID= " + lnk.CommandArgument + "");
        //cm = new SqlCommand(query, cn);
        //da = new SqlDataAdapter(cm);
        //ds = new DataSet();
        //da.Fill(ds);

        //if (ds != null && ds.Tables[0].Rows.Count > 0)
        //{
        //    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["CCvalue"].ToString()))
        //    {
        //        midbackCC = helper.getDocumentString(ds.Tables[0].Rows[0]["CCvalue"].ToString());
        //        midbackCC = formatString(midbackCC);
        //        str = str.Replace("#midback", midbackCC.Replace(" /", "/") + "<br/><br/>");
        //    }
        //    else
        //        str = str.Replace("#midback", midbackCC);
        //}
        //else
        //    str = str.Replace("#midback", midbackCC);

        ////midback PE printing string
        //string midbackPE = "", midbackTP = "";
        //query = ("select PEvalue,PESides,PESidesText from tblbpMidback where PatientIE_ID= " + lnk.CommandArgument + "");
        //cm = new SqlCommand(query, cn);
        //da = new SqlDataAdapter(cm);
        //ds = new DataSet();
        //da.Fill(ds);

        //if (ds != null && ds.Tables[0].Rows.Count > 0)
        //{
        //    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["PEvalue"].ToString()))
        //    {
        //        midbackPE = helper.getDocumentString(ds.Tables[0].Rows[0]["PEvalue"].ToString());
        //        midbackPE = midbackPE.Replace(",,", ",");
        //        midbackPE = this.formatString(midbackPE);
        //        midbackTP = this.getTPString(ds.Tables[0].Rows[0]["PESides"].ToString(), ds.Tables[0].Rows[0]["PESidesText"].ToString());
        //        //if (!string.IsNullOrEmpty(midbackTP))
        //        //    midbackPE = midbackPE + "There are palpable taut bands/trigger points at " + midbackTP.TrimStart(',') + ". ";

        //        midbackPE = formatString(midbackPE);
        //        midbackPE = formatString(midbackPE);

        //        str = str.Replace("#PEMidback", "<b>THORACIC SPINE EXAMINATION: </b>" + midbackPE + "<br/><br/>");

        //    }
        //    else
        //        str = str.Replace("#PEMidback", midbackPE);
        //}
        //else
        //    str = str.Replace("#PEMidback", midbackPE);

        ////shoulder printing string
        //query = ("select CCvalue from tblbpShoulder where PatientIE_ID= " + lnk.CommandArgument + "");
        //cm = new SqlCommand(query, cn);
        //da = new SqlDataAdapter(cm);
        //ds = new DataSet();
        //da.Fill(ds);

        //if (ds != null && ds.Tables[0].Rows.Count > 0)
        //{
        //    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["CCvalue"].ToString()))
        //    {
        //        shoudlerCC = helper.getDocumentStringLeftRight(ds.Tables[0].Rows[0]["CCvalue"].ToString(), "Shoulder");

        //        shoudlerCC = formatString(shoudlerCC);

        //        str = str.Replace("#shoulder", shoudlerCC.Replace(" /", "/") + "<br/><br/>");
        //    }
        //    else
        //        str = str.Replace("#shoulder", shoudlerCC);
        //}
        //else
        //    str = str.Replace("#shoulder", shoudlerCC);

        ////shoulder PE printing string
        //query = ("select PEvalue,NameROM,LeftROM,RightROM,NormalROM,PESides,PESidesText from tblbpshoulder where PatientIE_ID= " + lnk.CommandArgument + "");
        //string shoulderPE = "", shoulderTP = "";
        //cm = new SqlCommand(query, cn);
        //da = new SqlDataAdapter(cm);
        //ds = new DataSet();
        //da.Fill(ds);


        //if (ds != null && ds.Tables[0].Rows.Count > 0)
        //{
        //    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["PEvalue"].ToString()))
        //    {
        //        shoulderPE = helper.getDocumentStringLeftRightPE(ds.Tables[0].Rows[0]["PEvalue"].ToString());
        //        shoulderPE = shoulderPE.Replace(",,", ", ").Replace(" ,", ", ");
        //        shoulderPE = shoulderPE.Replace("Positive for,", "Test positive for ").Replace("Positive for and ", "positive for ");
        //        //        shoulderTP = this.getTPString(ds.Tables[0].Rows[0]["PESides"].ToString(), ds.Tables[0].Rows[0]["PESidesText"].ToString());

        //    }

        //    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["LeftROM"].ToString()))
        //    {
        //        string romstr = this.getROMString(ds.Tables[0].Rows[0]["NameROM"].ToString(), ds.Tables[0].Rows[0]["LeftROM"].ToString(), ds.Tables[0].Rows[0]["NormalROM"].ToString(), "left");
        //        if (!string.IsNullOrEmpty(romstr))
        //        {
        //            romstr = this.FirstCharToUpper(romstr.TrimStart());
        //            shoulderPE = shoulderPE.Replace("#shoulderleftrom", "<br/>ROM is as follows: " + romstr);
        //        }
        //        else
        //            shoulderPE = shoulderPE.Replace("#shoulderleftrom", "");
        //    }

        //    shoulderPE = shoulderPE.Replace("#shoulderleftmri", string.IsNullOrEmpty(strshoulderleftmri) ? "" : "<br/><br/>" + strshoulderleftmri);


        //    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["RightROM"].ToString()))
        //    {
        //        string romstr = this.getROMString(ds.Tables[0].Rows[0]["NameROM"].ToString(), ds.Tables[0].Rows[0]["RightROM"].ToString(), ds.Tables[0].Rows[0]["NormalROM"].ToString(), "right");
        //        if (!string.IsNullOrEmpty(romstr))
        //        {
        //            romstr = this.FirstCharToUpper(romstr.TrimStart());
        //            shoulderPE = shoulderPE.Replace("#shoulderrightrom", "<br/>ROM is as follows: " + romstr);
        //        }
        //        else
        //            shoulderPE = shoulderPE.Replace("#shoulderrightrom", "");
        //    }

        //    shoulderPE = shoulderPE.Replace("#shoulderrightmri", string.IsNullOrEmpty(strshoulderrightmri) ? "" : "<br/><br/>" + strshoulderrightmri);


        //    //if (!string.IsNullOrEmpty(shoulderTP))
        //    //    shoulderPE = shoulderPE + "There are palpable taut bands/trigger points at " + shoulderTP.TrimStart(',') + " with referral to the scapula. " +
        //    //        "";

        //    if (!string.IsNullOrEmpty(shoulderPE))
        //    {
        //        shoulderPE = shoulderPE.Replace("#rightshouldertitle", "<b>RIGHT SHOULDER EXAMINATION: </b> ");
        //        shoulderPE = shoulderPE.Replace("#leftshouldertitle", "<b>LEFT SHOULDER EXAMINATION: </b>");

        //        shoulderPE = formatString(shoulderPE);
        //        str = str.Replace("#PEShoudler", shoulderPE + "<br/><br/>");
        //    }
        //    else
        //    {
        //        str = str.Replace("#PEShoudler", "");
        //        shoulderPE = shoulderPE.Replace("#rightshouldertitle", "");
        //        shoulderPE = shoulderPE.Replace("#leftshouldertitle", "");
        //    }

        //}
        //else
        //    str = str.Replace("#PEShoudler", shoulderPE);



        ////knee printing string
        //query = ("select CCvalue from tblbpKnee where PatientIE_ID= " + lnk.CommandArgument + "");
        //cm = new SqlCommand(query, cn);
        //da = new SqlDataAdapter(cm);
        //ds = new DataSet();
        //da.Fill(ds);

        //if (ds != null && ds.Tables[0].Rows.Count > 0)
        //{
        //    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["CCvalue"].ToString()))
        //    {
        //        kneeCC = helper.getDocumentStringLeftRight(ds.Tables[0].Rows[0]["CCvalue"].ToString(), "Knee");

        //        kneeCC = formatString(kneeCC);

        //        str = str.Replace("#knee", kneeCC.Replace(" /", "/") + "<br/><br/>");
        //    }
        //    else
        //        str = str.Replace("#knee", kneeCC);
        //}
        //else
        //    str = str.Replace("#knee", kneeCC);

        ////knee PE printing string
        //query = ("select PEvalue,NameROM,LeftROM,RightROM,NormalROM from tblbpKnee where PatientIE_ID= " + lnk.CommandArgument + "");
        //string kneePE = "";
        //cm = new SqlCommand(query, cn);
        //da = new SqlDataAdapter(cm);
        //ds = new DataSet();
        //da.Fill(ds);

        //if (ds != null && ds.Tables[0].Rows.Count > 0)
        //{
        //    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["PEvalue"].ToString()))
        //    {

        //        kneePE = helper.getDocumentStringLeftRightPE(ds.Tables[0].Rows[0]["PEvalue"].ToString());
        //        kneePE = kneePE.Replace(",,", ",");
        //        kneePE = kneePE.Replace("Positive for,", "Test positive for ").Replace("Positive for and ", "positive for ");
        //    }

        //    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["LeftROM"].ToString()))
        //    {
        //        string romstr = this.getROMString(ds.Tables[0].Rows[0]["NameROM"].ToString(), ds.Tables[0].Rows[0]["LeftROM"].ToString(), ds.Tables[0].Rows[0]["NormalROM"].ToString(), "left");

        //        if (!string.IsNullOrEmpty(romstr))
        //        {
        //            romstr = this.FirstCharToUpper(romstr.TrimStart());
        //            kneePE = kneePE.Replace("#kneeleftrom", "<br/>ROM is as follows: " + romstr);
        //        }
        //        else
        //            kneePE = kneePE.Replace("#kneeleftrom", "");


        //    }

        //    kneePE = kneePE.Replace("#kneerightmri", string.IsNullOrEmpty(strkneerighttmri) ? "" : "<br/><br/>" + strkneerighttmri);


        //    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["RightROM"].ToString()))
        //    {
        //        string romstr = this.getROMString(ds.Tables[0].Rows[0]["NameROM"].ToString(), ds.Tables[0].Rows[0]["RightROM"].ToString(), ds.Tables[0].Rows[0]["NormalROM"].ToString(), "right");
        //        if (!string.IsNullOrEmpty(romstr))
        //        {
        //            romstr = this.FirstCharToUpper(romstr.TrimStart());
        //            kneePE = kneePE.Replace("#kneerightrom", "<br/>ROM is as follows: " + romstr);
        //        }
        //        else
        //            kneePE = kneePE.Replace("#kneerightrom", "");
        //    }

        //    kneePE = kneePE.Replace("#kneeleftmri", string.IsNullOrEmpty(strkneeleftmri) ? "" : "<br/><br/>" + strkneeleftmri);


        //    if (!string.IsNullOrEmpty(kneePE))
        //    {
        //        kneePE = kneePE.Replace("#leftkneetitle", "<b>LEFT KNEE EXAMINATION: </b>");
        //        kneePE = kneePE.Replace("#rightkneetitle", "<b>RIGHT KNEE EXAMINATION: </b>");

        //        kneePE = formatString(kneePE);

        //        str = str.Replace("#PEKnee", kneePE + "<br/><br/>");

        //    }
        //    else
        //    {
        //        kneePE = kneePE.Replace("#leftkneetitle", "");
        //        kneePE = kneePE.Replace("#rightkneetitle", "");
        //        str = str.Replace("#PEKnee", "");
        //    }

        //}
        //else
        //    str = str.Replace("#PEKnee", kneePE);

        ////elbow printing string
        //query = ("select CCvalue from tblbpElbow where PatientIE_ID= " + lnk.CommandArgument + "");
        //cm = new SqlCommand(query, cn);
        //da = new SqlDataAdapter(cm);
        //ds = new DataSet();
        //da.Fill(ds);

        //if (ds != null && ds.Tables[0].Rows.Count > 0)
        //{
        //    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["CCvalue"].ToString()))
        //    {
        //        elbowCC = helper.getDocumentStringLeftRight(ds.Tables[0].Rows[0]["CCvalue"].ToString(), "Elbow");

        //        elbowCC = formatString(elbowCC);

        //        str = str.Replace("#elbow", elbowCC.Replace(" /", "/") + "<br/><br/>");
        //    }
        //    else
        //        str = str.Replace("#elbow", elbowCC);
        //}
        //else
        //    str = str.Replace("#elbow", elbowCC);

        ////elbow PE printing string
        //string elbowPE = "";
        //query = ("select  PEvalue,NameROM,LeftROM,RightROM,NormalROM  from tblbpElbow where PatientIE_ID= " + lnk.CommandArgument + "");
        //cm = new SqlCommand(query, cn);
        //da = new SqlDataAdapter(cm);
        //ds = new DataSet();
        //da.Fill(ds);

        //if (ds != null && ds.Tables[0].Rows.Count > 0)
        //{
        //    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["PEvalue"].ToString()))
        //    {
        //        elbowPE = helper.getDocumentStringLeftRightPE(ds.Tables[0].Rows[0]["PEvalue"].ToString());
        //        elbowPE = elbowPE.Replace(",,", ",");
        //    }

        //    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["LeftROM"].ToString()))
        //    {
        //        string romstr = this.getROMString(ds.Tables[0].Rows[0]["NameROM"].ToString(), ds.Tables[0].Rows[0]["LeftROM"].ToString(), ds.Tables[0].Rows[0]["NormalROM"].ToString(), "left");
        //        if (!string.IsNullOrEmpty(romstr))
        //        {
        //            romstr = this.FirstCharToUpper(romstr.TrimStart());
        //            elbowPE = elbowPE + " ROM is as follows: " + romstr;
        //        }
        //    }

        //    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["RightROM"].ToString()))
        //    {
        //        string romstr = this.getROMString(ds.Tables[0].Rows[0]["NameROM"].ToString(), ds.Tables[0].Rows[0]["RightROM"].ToString(), ds.Tables[0].Rows[0]["NormalROM"].ToString(), "right");
        //        if (!string.IsNullOrEmpty(romstr))
        //        {
        //            romstr = this.FirstCharToUpper(romstr.TrimStart());
        //            elbowPE = elbowPE + " ROM is as follows: " + romstr;
        //        }
        //    }



        //    if (!string.IsNullOrEmpty(elbowPE))
        //    {
        //        elbowPE = elbowPE.Replace("#leftelbowtitle", "<b>LEFT ELBOW EXAMINATION: </b>");
        //        elbowPE = elbowPE.Replace("#rightelbowtitle", "<b>RIGHT ELBOW EXAMINATION: </b>");
        //        elbowPE = formatString(elbowPE);
        //        str = str.Replace("#PEElbow", elbowPE + "<br/><br/>");
        //    }
        //    else
        //    {
        //        str = str.Replace("#PEElbow", "");
        //        elbowPE = elbowPE.Replace("#leftelbowtitle", "");
        //        elbowPE = elbowPE.Replace("#rightelbowtitle", "");
        //    }

        //}
        //else
        //    str = str.Replace("#PEElbow", elbowPE);

        ////wrist printing string
        //query = ("select CCvalue from tblbpWrist where PatientIE_ID= " + lnk.CommandArgument + "");
        //cm = new SqlCommand(query, cn);
        //da = new SqlDataAdapter(cm);
        //ds = new DataSet();
        //da.Fill(ds);

        //if (ds != null && ds.Tables[0].Rows.Count > 0)
        //{
        //    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["CCvalue"].ToString()))
        //    {
        //        wristCC = helper.getDocumentStringLeftRight(ds.Tables[0].Rows[0]["CCvalue"].ToString(), "Wrist");
        //        wristCC = formatString(wristCC);
        //        str = str.Replace("#wrist", wristCC.Replace(" /", "/") + "<br/><br/>");

        //    }
        //    else
        //        str = str.Replace("#wrist", wristCC);
        //}
        //else
        //    str = str.Replace("#wrist", wristCC);

        ////hip printing string
        //query = ("select CCvalue from tblbpHip where PatientIE_ID= " + lnk.CommandArgument + "");
        //cm = new SqlCommand(query, cn);
        //da = new SqlDataAdapter(cm);
        //ds = new DataSet();
        //da.Fill(ds);

        //if (ds != null && ds.Tables[0].Rows.Count > 0)
        //{
        //    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["CCvalue"].ToString()))
        //    {
        //        hipCC = helper.getDocumentStringLeftRight(ds.Tables[0].Rows[0]["CCvalue"].ToString(), "Hip");
        //        hipCC = formatString(hipCC);
        //        str = str.Replace("#hip", hipCC.Replace(" /", "/") + "<br/><br/>");

        //    }
        //    else
        //        str = str.Replace("#hip", hipCC);
        //}
        //else
        //    str = str.Replace("#hip", hipCC);

        ////hip PE printing string
        //string hipPE = "";
        //query = ("select PEvalue,NameROM,LeftROM,RightROM,NormalROM from tblbpHip where PatientIE_ID= " + lnk.CommandArgument + "");
        //cm = new SqlCommand(query, cn);
        //da = new SqlDataAdapter(cm);
        //ds = new DataSet();
        //da.Fill(ds);

        //if (ds != null && ds.Tables[0].Rows.Count > 0)
        //{
        //    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["PEvalue"].ToString()))
        //    {
        //        hipPE = helper.getDocumentStringLeftRightPE(ds.Tables[0].Rows[0]["PEvalue"].ToString());
        //        hipPE = hipPE.Replace(",,", ",");
        //    }

        //    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["LeftROM"].ToString()))
        //    {
        //        string romstr = this.getROMString(ds.Tables[0].Rows[0]["NameROM"].ToString(), ds.Tables[0].Rows[0]["LeftROM"].ToString(), ds.Tables[0].Rows[0]["NormalROM"].ToString(), "left");
        //        if (!string.IsNullOrEmpty(romstr))
        //        {
        //            romstr = this.FirstCharToUpper(romstr.TrimStart());
        //            hipPE = hipPE + " ROM is as follows: " + romstr;
        //        }
        //    }

        //    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["RightROM"].ToString()))
        //    {
        //        string romstr = this.getROMString(ds.Tables[0].Rows[0]["NameROM"].ToString(), ds.Tables[0].Rows[0]["RightROM"].ToString(), ds.Tables[0].Rows[0]["NormalROM"].ToString(), "right");
        //        if (!string.IsNullOrEmpty(romstr))
        //        {
        //            romstr = this.FirstCharToUpper(romstr.TrimStart());
        //            hipPE = hipPE + " ROM is as follows: " + romstr;
        //        }
        //    }

        //    if (!string.IsNullOrEmpty(hipPE))
        //    {

        //        hipPE = hipPE.Replace("#lefthiptitle", "<b>LEFT HIP EXAMINATION: </b>");
        //        hipPE = hipPE.Replace("#rigthhiptitle", "<b>RIGHT HIP EXAMINATION: </b>");

        //        hipPE = formatString(hipPE);

        //        str = str.Replace("#PEHip", hipPE + "<br/><br/>");
        //    }
        //    else
        //    {
        //        hipPE = hipPE.Replace("#lefthiptitle", "");
        //        hipPE = hipPE.Replace("#rigthhiptitle", "");
        //        str = str.Replace("#PEHip", "");
        //    }
        //}
        //else
        //    str = str.Replace("#PEHip", "");


        ////ankle printing string
        //query = ("select CCvalue from tblbpAnkle where PatientIE_ID= " + lnk.CommandArgument + "");
        //cm = new SqlCommand(query, cn);
        //da = new SqlDataAdapter(cm);
        //ds = new DataSet();
        //da.Fill(ds);

        //if (ds != null && ds.Tables[0].Rows.Count > 0)
        //{
        //    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["CCvalue"].ToString()))
        //    {
        //        ankleCC = helper.getDocumentStringLeftRight(ds.Tables[0].Rows[0]["CCvalue"].ToString(), "Ankle");
        //        ankleCC = formatString(ankleCC);

        //        str = str.Replace("#ankle", ankleCC.Replace(" /", "/") + "<br/><br/>");

        //    }
        //    else
        //        str = str.Replace("#ankle", ankleCC);
        //}
        //else
        //    str = str.Replace("#ankle", ankleCC);


        ////ankle PE printing string
        //string anklePE = "";
        //query = ("select PEvalue,NameROM,LeftROM,RightROM,NormalROM from tblbpAnkle where PatientIE_ID= " + lnk.CommandArgument + "");
        //cm = new SqlCommand(query, cn);
        //da = new SqlDataAdapter(cm);
        //ds = new DataSet();
        //da.Fill(ds);

        //if (ds != null && ds.Tables[0].Rows.Count > 0)
        //{
        //    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["PEvalue"].ToString()))
        //    {
        //        anklePE = helper.getDocumentStringLeftRightPE(ds.Tables[0].Rows[0]["PEvalue"].ToString());
        //        anklePE = anklePE.Replace(",,", ",");
        //    }

        //    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["LeftROM"].ToString()))
        //    {
        //        string romstr = this.getROMString(ds.Tables[0].Rows[0]["NameROM"].ToString(), ds.Tables[0].Rows[0]["LeftROM"].ToString(), ds.Tables[0].Rows[0]["NormalROM"].ToString(), "left");
        //        if (!string.IsNullOrEmpty(romstr))
        //        {
        //            romstr = this.FirstCharToUpper(romstr.TrimStart());
        //            anklePE = anklePE + " ROM is as follows: " + romstr;
        //        }
        //    }

        //    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["RightROM"].ToString()))
        //    {
        //        string romstr = this.getROMString(ds.Tables[0].Rows[0]["NameROM"].ToString(), ds.Tables[0].Rows[0]["RightROM"].ToString(), ds.Tables[0].Rows[0]["NormalROM"].ToString(), "right");
        //        if (!string.IsNullOrEmpty(romstr))
        //        {
        //            romstr = this.FirstCharToUpper(romstr.TrimStart());
        //            anklePE = anklePE + " ROM is as follows: " + romstr;
        //        }
        //    }

        //    if (!string.IsNullOrEmpty(anklePE))
        //    {

        //        anklePE = anklePE.Replace("#leftankletitle", "<b>LEFT ANKLE EXAMINATION: </b>");
        //        anklePE = anklePE.Replace("#rigthankletitle", "<b>RIGHT ANKLE EXAMINATION: </b>");
        //        anklePE = formatString(anklePE);
        //        str = str.Replace("#PEAnkle", anklePE + "<br/><br/>");

        //    }
        //    else
        //    {
        //        anklePE = anklePE.Replace("#leftankletitle", "");
        //        anklePE = anklePE.Replace("#rigthankletitle", "");
        //        str = str.Replace("#PEAnkle", "");
        //    }

        //}
        //else
        //    str = str.Replace("#PEAnkle", anklePE);



        ////wrist PE printing string
        //string wristPE = "";
        //query = ("select PEvalue,NameROM,LeftROM,RightROM,NormalROM from tblbpWrist where PatientIE_ID= " + lnk.CommandArgument + "");
        //cm = new SqlCommand(query, cn);
        //da = new SqlDataAdapter(cm);
        //ds = new DataSet();
        //da.Fill(ds);

        //if (ds != null && ds.Tables[0].Rows.Count > 0)
        //{
        //    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["PEvalue"].ToString()))
        //    {
        //        wristPE = helper.getDocumentStringLeftRightPE(ds.Tables[0].Rows[0]["PEvalue"].ToString());
        //        wristPE = wristPE.Replace(",,", ",");
        //    }
        //    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["LeftROM"].ToString()))
        //    {
        //        string romstr = this.getROMString(ds.Tables[0].Rows[0]["NameROM"].ToString(), ds.Tables[0].Rows[0]["LeftROM"].ToString(), ds.Tables[0].Rows[0]["NormalROM"].ToString(), "left");
        //        if (!string.IsNullOrEmpty(romstr))
        //        {
        //            romstr = this.FirstCharToUpper(romstr.TrimStart());
        //            wristPE = wristPE + " ROM is as follows: " + romstr.TrimStart(';');
        //        }
        //    }

        //    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["RightROM"].ToString()))
        //    {
        //        string romstr = this.getROMString(ds.Tables[0].Rows[0]["NameROM"].ToString(), ds.Tables[0].Rows[0]["RightROM"].ToString(), ds.Tables[0].Rows[0]["NormalROM"].ToString(), "right");
        //        if (!string.IsNullOrEmpty(romstr))
        //        {
        //            romstr = this.FirstCharToUpper(romstr.TrimStart());
        //            wristPE = wristPE + " ROM is as follows: " + romstr.TrimStart(';');
        //        }


        //    }



        //    if (!string.IsNullOrEmpty(wristPE))
        //    {
        //        wristPE = wristPE.Replace("#leftwristtitle", "<b>LEFT WRIST EXAMINATION: </b>");
        //        wristPE = wristPE.Replace("#rightwristtitle", "<b>RIGHT WRIST EXAMINATION: </b>");
        //        wristPE = formatString(wristPE);
        //        str = str.Replace("#PEWrist", wristPE + "<br/><br/>");
        //    }
        //    else
        //    {
        //        str = str.Replace("#PEWrist", "");
        //        wristPE = wristPE.Replace("#leftwristtitle", "");
        //        wristPE = wristPE.Replace("#rightwristtitle", "");
        //    }
        //}
        //else
        //    str = str.Replace("#PEWrist", "");

        //query = ("Select * from tblbpOtherPart WHERE PatientIE_ID=" + lnk.CommandArgument + "");
        //cm = new SqlCommand(query, cn);
        //da = new SqlDataAdapter(cm);
        //ds = new DataSet();
        //da.Fill(ds);

        //if (ds != null && ds.Tables[0].Rows.Count > 0)
        //{
        //    str = str.Replace("#otherCC", !string.IsNullOrEmpty(ds.Tables[0].Rows[0]["OthersCC"].ToString()) ? ds.Tables[0].Rows[0]["OthersCC"].ToString() + "<br /><br />" : "");
        //    str = str.Replace("#otherPE", !string.IsNullOrEmpty(ds.Tables[0].Rows[0]["OthersPE"].ToString()) ? ds.Tables[0].Rows[0]["OthersPE"].ToString() + "<br /><br />" : "");


        //    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["FollowUpIn"].ToString().Trim()))
        //        str = str.Replace("#follow-up", "<b>FOLLOW-UP: </b>" + ds.Tables[0].Rows[0]["FollowUpIn"].ToString().Trim() + "<br/><br/>");
        //    else
        //        str = str.Replace("#follow-up", "");
        //}
        //else
        //{
        //    str = str.Replace("#otherCC", "");
        //    str = str.Replace("#otherPE", "");
        //    str = str.Replace("#follow-up", "");
        //}


        ////print sign

        ////string path = "http://aeiuat.dynns.com:82/V3_Test/sign/21.jpg";
        //str = str.Replace("#signsrc", "");



        //string printStr = str;

        //divPrint.InnerHtml = printStr;

        //printStr = prstrCC + "\n" + prstrPE;


        //createWordDocument(str, docname, lnk.CommandArgument, "");

        //string folderPath = Server.MapPath("~/Reports/" + lnk.CommandArgument);

        //// DownloadFiles(folderPath, "IE");

        //savePrintRequest(lnk.CommandArgument, "0");

        //BindPatientIEDetails();
        //// ClientScript.RegisterStartupScript(this.GetType(), "Popup", "alert('Documents will be available for download after 5 min.')", true);
        ////}
        ////catch (Exception ex)
        ////{
        ////}
    }
}