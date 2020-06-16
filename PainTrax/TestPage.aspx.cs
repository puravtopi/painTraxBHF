using iTextSharp.text.pdf;
using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
//using Microsoft.VisualBasic.Devices;
//using Microsoft.VisualBasic;

public partial class TestPage : System.Web.UI.Page
{


    [DllImport("winmm.dll", EntryPoint = "mciSendStringA", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
    private static extern int mciSendString(string lpstrCommand, string lpstrReturnString, int uReturnLength, int hwndCallback);
    protected void Page_PreInit(object sender, EventArgs e)
    {


    }

    protected void Page_Load(object sender, EventArgs e)
    {
        demoText();
        //mciSendString("open new Type waveaudio alias recsound", null, 0, 0);
        //this.createDB();
        // demoPDF();
        //string path = Server.MapPath("~/Template/Demo.txt");
        //string body = File.ReadAllText(path);

        //string temp = new PrintDocumentHelper().getDocumentStringDenies(body);

        //  PdfStampInExistingFile("Hello world");

        // setPDF();
        //var word = new Microsoft.Office.Interop.Word.Application();
        //word.Visible = false;

        //var filePath = Server.MapPath("~/MyFiles/123.doc");
        //var savePathDocx = Server.MapPath("~/MyFiles/Demo.docx");
        //var wordDoc = word.Documents.Open(FileName: filePath, ReadOnly: false);
        //wordDoc.SaveAs2(FileName: savePathDocx, FileFormat: WdSaveFormat.wdFormatXMLDocument);
        //wordDoc.Close();

    }


    public void demoPDF()
    {
        using (Stream inputPdfStream = new FileStream(Server.MapPath("~") + "/MyFiles/demo.pdf", FileMode.Open, FileAccess.Read, FileShare.Read))
        using (Stream inputImageStream = new FileStream(Server.MapPath("~") + "/Sign/Sign.png", FileMode.Open, FileAccess.Read, FileShare.Read))
        using (Stream outputPdfStream = new FileStream(Server.MapPath("~") + "/MyFiles/Result.pdf", FileMode.Create, FileAccess.Write, FileShare.None))
        {
            var reader = new PdfReader(inputPdfStream);

            int pages = reader.NumberOfPages;

            for (int i = 0; i < pages; i++)
            {
                var stamper = new PdfStamper(reader, outputPdfStream);
                var pdfContentByte = stamper.GetOverContent(1);

                iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(inputImageStream);
                // image.ScalePercent(18f);
                image.ScaleAbsolute(100, 30);

                image.SetAbsolutePosition(300, 310);
                pdfContentByte.AddImage(image);


                stamper.Close();
            }
        }
    }

    protected void btnTest_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(hidval.Value))
            Response.Write(hidval.Value.TrimStart(','));
    }

    protected void btnLoadcheckbox_Click(object sender, EventArgs e)
    {
        hidval.Value = "";
        CheckBoxList cbList = new CheckBoxList();
        cbList.ID = "chkList";

        for (int i = 0; i < 10; i++)
        {
            cbList.Items.Add(new ListItem("Checkbox " + i.ToString(), i.ToString()));
            cbList.Items[i].Attributes.Add("onclick", "chekcVal(this," + i.ToString() + ")");
            //cbList.Items[i].Attributes.Add("text", i.ToString());
        }

        placeHolder.Controls.Add(cbList);
    }

    public void demoFunction()
    {
        string NameTest = ",,Strainght leg raise,Braggard's test,Kernig's sign,Brudzinski's ,Sacroiliac compression,Sacral notch tenderness,Ober's test causing pain at the SI joint",
            LeftTest = ",1,1,1,0,1,0,1", RightTest = ",1,1,1,1,1,1,1", TextVal = ",23";

        NameTest = NameTest.TrimStart(',');
        string[] NameTestVal = NameTest.TrimStart(',').Split(',');
        string[] LeftTestVal = LeftTest.TrimStart(',').Split(',');
        string[] RightTestVal = RightTest.TrimStart(',').Split(',');
        string[] TextValVal = TextVal.Split(',');

        string str = "", leftval = "", rightval = "";
        for (int i = 0; i < NameTestVal.Length; i++)
        {

            if (i == 0)
            {
                if (!string.IsNullOrEmpty(TextValVal[0]))
                {
                    leftval = " at " + TextValVal[0] + " degrees ";
                }
                if (!string.IsNullOrEmpty(TextValVal[1]))
                {
                    rightval = " at " + TextValVal[1] + " degrees ";
                }
            }
            else
            {
                leftval = ""; rightval = "";
            }

            if (LeftTestVal[i] == "1")
            {
                str = str + "," + NameTestVal[i] + " is positive on left " + leftval;
            }
            if (RightTestVal[i] == "1")
            {
                if (LeftTestVal[i] == "1")
                    str = str + " and on the right" + rightval;
                else
                    str = str + "," + NameTestVal[i] + " is positive on right" + rightval;
            }
        }
        Response.Write(str);

    }


    public void setPDF()
    {
        string imgpath = Server.MapPath("~/Sign/21.jpg");
        string pdfpath = Server.MapPath("~/TemplateStore/Forms/Nf packet.pdf");
        string pdfpathourput = Server.MapPath("~/TemplateStore/Forms/Demo.pdf");

        using (Stream inputPdfStream = new FileStream(pdfpath, FileMode.Open, FileAccess.Read, FileShare.Read))
        using (Stream inputImageStream = new FileStream(imgpath, FileMode.Open, FileAccess.Read, FileShare.Read))
        using (Stream outputPdfStream = new FileStream(pdfpathourput, FileMode.Create, FileAccess.Write, FileShare.None))
        {
            var reader = new iTextSharp.text.pdf.PdfReader(inputPdfStream);

            int val = reader.NumberOfPages;

            var stamper = new iTextSharp.text.pdf.PdfStamper(reader, outputPdfStream);

            var pdfContentByte = stamper.GetOverContent(1);

            iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(inputImageStream);

            image.SetAbsolutePosition(759f, 459f);

            pdfContentByte.AddImage(image);
            stamper.Close();
        }
    }

    private void PdfStampInExistingFile(string text)
    {
        // string sourceFilePath = @"C:\Users\anand\Desktop\Test.pdf";

        string sourceFilePath = Server.MapPath("~/TemplateStore/Forms/Nf packet.pdf");
        byte[] bytes = File.ReadAllBytes(sourceFilePath);
        Bitmap bitmap = new Bitmap(200, 30, System.Drawing.Imaging.PixelFormat.Format64bppArgb);
        Graphics graphics = Graphics.FromImage(bitmap);
        graphics.Clear(Color.White);
        graphics.DrawString(text, new System.Drawing.Font("Arial", 12, FontStyle.Bold), new SolidBrush(Color.Red), new PointF(0.4F, 2.4F));
        bitmap.Save(Server.MapPath("~/Image.jpg"), ImageFormat.Jpeg);
        bitmap.Dispose();
        var img = iTextSharp.text.Image.GetInstance(Server.MapPath("~/Sign/21.jpg"));
        img.SetAbsolutePosition(200, 400);
        PdfContentByte waterMark;
        using (MemoryStream stream = new MemoryStream())
        {
            PdfReader reader = new PdfReader(bytes);
            PdfStamper stamper = new PdfStamper(reader, stream);

            int pages = reader.NumberOfPages;
            for (int i = 1; i <= pages; i++)
            {
                waterMark = stamper.GetUnderContent(i);
                waterMark.AddImage(img);
            }

            bytes = stream.ToArray();
        }
        //File.Delete(Server.MapPath("~/Image.jpg"));
        File.WriteAllBytes(sourceFilePath, bytes);
    }

    private void createDB()
    {
        string connectionString = "Data Source=DESKTOP-FB4A3RR;initial catalog=master;integrated security=True;MultipleActiveResultSets=True;";

        // your query:
        var query = GetDbCreationQuery();

        var conn = new SqlConnection(connectionString);
        var command = new SqlCommand(query, conn);

        try
        {
            conn.Open();
            command.ExecuteNonQuery();

        }
        catch (Exception ex)
        {

        }
        finally
        {
            if ((conn.State == ConnectionState.Open))
            {
                conn.Close();
            }
        }
    }

    static string GetDbCreationQuery()
    {
        // your db name
        string dbName = "DbCompany1";

        // db creation query
        string query = "CREATE DATABASE " + dbName + ";";

        return query;
    }

    protected void btnRecord_Click(object sender, EventArgs e)
    {
       // mciSendString("open new Type waveaudio Alias recsound", null, 0, 0);
        mciSendString("record recsound", null, 0, 0);
    }

    protected void btnStop_Click(object sender, EventArgs e)
    {
        mciSendString("save recsound c:\\Demo\\record.wav", null, 0, 0);
        mciSendString("close recsound ", "", 0, 0);
        //Computer c = new Computer();
        //c.Audio.Stop();
    }

    protected void btnPlay_Click(object sender, EventArgs e)
    {

    }

    public void demoText()
    {
        string bodypart = "";
        String strfile = File.ReadAllText(Server.MapPath("demo.txt")); ;

        String[] str = new String[2];
        StringBuilder sb = new StringBuilder();
        int leftstart, leftend, rightstart, rightend;
        leftstart = strfile.IndexOf(@"<div id=""WrapLeft""");
        leftend = strfile.IndexOf(@"</div>", leftstart);
        str[0] = strfile.Substring(leftstart, leftend - leftstart);
        rightstart = strfile.IndexOf(@"<div id=""WrapRight""");
        rightend = strfile.IndexOf(@"</div>", rightstart);
        str[1] = strfile.Substring(rightstart, rightend - rightstart);
        Regex rex = new Regex(@"<div.*display:[ ]*none.*>");
        Response.Write(rex.Match(str[0]));
        if (rex.IsMatch(str[0]))
            str[0] = "";
        if (rex.IsMatch(str[1]))
            str[1] = "";

        Response.Write(str[0].Length);
        Response.Write("<br>");
        Response.Write(str[1].Length);

        for (int i = 0; i < 2; i++)
        {
            //String pattern = @"(<input\s*(.+?)\s*/>|<label>\s*(.+?)\s*</label>)";
            String pattern = @"(<input\s*(.+?)\s*>|<label.*>\s*(.+?)\s*</label>|<textarea.*>\s*(.+?)\s*</textarea>)";
            //String pattern = @"<input\s*(.+?)\s*/>";
            //String pattern = @"<label>\s*(.+?)\s*</label>";
            RegexOptions regexOptions = RegexOptions.Multiline;
            Regex regex = new Regex(pattern, regexOptions);

            Dictionary<string, string> d = new Dictionary<string, string>();
            string prevtype = "", type = "";
            int chkgrp = 0;

            if (!string.IsNullOrEmpty(bodypart))
            {
                if (i == 0)
                {
                    sb.Append("<b>LEFT " + bodypart.ToUpper() + ": </b> ");
                }
                else if (i == 1)
                {
                    sb.Append("<b>RIGHT " + bodypart.ToUpper() + ": </b> ");
                }
            }

            foreach (Match match in regex.Matches(str[i]))
            {
                prevtype = type;
                if (match.Success)
                {

                    /*     for (int j = 0; j < match.Groups.Count - 1; j++)
                            {
                                Response.Write("Match ["+j.ToString()+"]: " + match.Groups[j].Value + "<br>");
                            }*/
                    if (match.Groups[2].Value.Length != 0)
                    {
                        String tagvalue = match.Groups[2].Value;

                        String[] attrib = tagvalue.Split(' ');
                        string id = "", value = "";
                        bool check = false;



                        foreach (string att in attrib)
                        {
                            if (att.ToLower().StartsWith("id"))
                            {
                                String[] test = att.Split('=');
                                id = test[1];
                            }
                            if (att.ToLower().StartsWith("value"))
                            {
                                String[] test = att.Split('=');
                                int startindex = tagvalue.IndexOf("value=") + 7;
                                int endindex = tagvalue.IndexOf("\"", startindex);
                                value = tagvalue.Substring(startindex, endindex - startindex);
                                //        Response.Write("value=" + value  + "<br>");
                                //        Response.Write(value + "<br>");
                            }
                            if (att.ToLower().StartsWith("checked"))
                            {
                                check = true;
                            }

                            if (att.ToLower().StartsWith("type"))
                            {
                                String[] test = att.Split('=');
                                type = test[1].ToLower();
                                Regex rx = new Regex(@"^\s*""?|""?\s*$");

                                type = rx.Replace(type, "");
                                //      Response.Write("type="+type+"<br>");
                            }
                            // d.ToString();
                        }

                        //Response.Write(prevtype);
                        if (type == "checkbox" || type == "radio")
                        {

                            Regex rx = new Regex(@"^\s*""?|""?\s*$");
                            id = rx.Replace(id, "");
                            value = rx.Replace(value, "");
                            if (value.Length > 0 && check)
                            {
                                chkgrp += 1;
                                sb.Append(value + ", ");
                            }
                        }
                        else if (type == "text")
                        {
                            //  Regex rx = new Regex(@"^\s*""?|""?\s*$");
                            // value = rx.Replace(value, "");
                            //    Response.Write(value);
                            sb.Append(value + " ");
                        }


                    }
                    else if (match.Groups[2].Value.Length == 0)
                    {
                        if ((prevtype == "checkbox" || prevtype == "radio") && chkgrp > 0)
                        {
                            sb.Remove(sb.Length - 2, 2).Append(" ");
                            if (chkgrp > 0)
                            {
                                if (sb.ToString().LastIndexOf(",") >= 0)
                                    sb.Replace(",", " and ", sb.ToString().LastIndexOf(","), 1);
                            }
                            chkgrp = 0;
                        }
                        String tagvalue = match.Groups[1].Value;
                        Regex rx = new Regex("<[^>]*>");
                        tagvalue = rx.Replace(tagvalue, "");
                        sb.Append(tagvalue);
                    }
                }
            }
            if ((prevtype == "checkbox" || prevtype == "radio") && chkgrp > 0)
            {
                sb.Remove(sb.Length - 2, 2).Append(" ");
                if (chkgrp > 0)
                {
                    if (sb.ToString().LastIndexOf(",") >= 0)
                        sb.Replace(",", " and ", sb.ToString().LastIndexOf(","), 1);
                }
                chkgrp = 0;
            }
            if (i == 0)
                sb.Append("<br/><br/>");
        }
        Response.Write(sb.Replace(" .", ". ").ToString());
    }
}