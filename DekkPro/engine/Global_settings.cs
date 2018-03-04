using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Net.Mail;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Text.RegularExpressions;
using System.Net;


using System.Text.RegularExpressions;
using System.Net;

namespace DekkOnline2.engine2
{
    public class Global_settings
    {
        #region Methods
        public static string cssVersion = "?v=1.4";
        /// <summary>
        /// Method that save the error massage from the user context
        ///</summary>
        /// <param name="exception">Error massage</param>
        /// <param name="redirect">True if the page is to be redirected</param>
        public static void saveErrors(String exception, Boolean redirect)
        {
            String filename = System.Web.HttpContext.Current.Application["loggSti"].ToString();
            StreamWriter sw = File.AppendText(filename);
            sw.WriteLine("****** " + DateTime.Now.ToString() + " " + exception.ToString());
            sw.Close();
            if (redirect)
                HttpContext.Current.Response.Redirect("~/error.aspx");
        }

        public static bool IsMail(string p_email)
        {

            System.Text.RegularExpressions.Regex l_reg = new System.Text.RegularExpressions.Regex("^(([^<;>;()[\\]\\\\.,;:\\s@\\\"]+" + "(\\.[^<;>;()[\\]\\\\.,;:\\s@\\\"]+)*)|(\\\".+\\\"))@" + "((\\[[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}" + "\\.[0-9]{1,3}\\])|(([a-zA-Z\\-0-9]+\\.)+" + "[a-zA-Z]{2,}))$");
            return (l_reg.IsMatch(p_email));
        }

        /// <summary>
        /// Method that send the emails to new users
        ///</summary>
        public static bool sendEmail(String body, bool Is_PwsRecovery)
        {
            string port = string.Empty;
            string SMTP_Server = string.Empty;
            string PwordEmail = string.Empty;
            string SendingEmail = string.Empty;
            string NoticeToEmail = string.Empty;
            string Security = string.Empty;
            bool enviado = false;

            try
            {
                SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["CMSCLIENTSConnectionString"].ConnectionString);
                SqlCommand commandGetEmailSettings = new SqlCommand("SELECT port, SMTP_Server, PwordEmail, SendingEmail, NoticeToEmail, SecurityEmail FROM aspnet_SiteConfig where Numb_client = '" + ConfigurationManager.AppSettings["ClientNumber"].ToString() + "'", connection);
                connection.Open();
                SqlDataReader dataReader2 = commandGetEmailSettings.ExecuteReader();
                while (dataReader2.Read())
                {
                    port = dataReader2["port"].ToString().Trim();
                    SMTP_Server = dataReader2["SMTP_Server"].ToString().Trim();
                    PwordEmail = dataReader2["PwordEmail"].ToString().Trim();
                    SendingEmail = dataReader2["SendingEmail"].ToString().Trim();
                    NoticeToEmail = dataReader2["NoticeToEmail"].ToString().Trim();
                    Security = dataReader2["SecurityEmail"].ToString().Trim();
                }
                dataReader2.Close();
                connection.Close();
            }
            catch (Exception ee)
            {
                saveErrors(ee.ToString() + " GetMenuTxt", false);
                return enviado;
            }

            try
            {
                string tomail = string.Empty;
                if (Is_PwsRecovery)
                {
                    if (HttpContext.Current.Session["e"] != null)
                        tomail = HttpContext.Current.Session["e"].ToString();
                }
                else
                    tomail = NoticeToEmail;

                MailMessage message = new MailMessage(SendingEmail, tomail, "Melding fra websiden", body);
                message.IsBodyHtml = true;
                message.BodyEncoding = System.Text.Encoding.UTF8;
                SmtpClient mail = new SmtpClient();

                if (Security != "None" && Security != "")
                {
                    mail.EnableSsl = true;

                    if (Security == "TLS") System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
                }

                mail.Host = SMTP_Server;
                mail.Port = Convert.ToInt16(port);
                mail.Credentials = new System.Net.NetworkCredential(SendingEmail, PwordEmail);
                mail.Send(message);
                enviado = true;
                return enviado;
            }
            catch (SmtpException exception)
            {
                saveErrors(exception.ToString() + " SendEmail, Global_settings", false);
                return enviado;
            }

            return enviado;
        }

        public static bool sendMail(string emailTo, string subject, string body, Stream document = null, string documentFileName = "",
                                  string cssStyle = "", bool bodyIsHtml = false, string CcoTo = "", string From = "")
        {
            bool pass = false;
            body = erstattNorskeTegn(body);
            string strError = "";
            string htmlBody;

            if (bodyIsHtml)
                htmlBody = body;
            else
            {
                htmlBody = "" + "<html xmlns='http://www.w3.org/1999/xhtml'>" +
                                        "<head>" +
                                            "<meta http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
                                            "<title></title>" +
                                        "</head>" +
                                        "<body>" + cssStyle + body + "</body> </html>";
            }

            strError = "";

            try
            {
                SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["CMSCLIENTSConnectionString"].ConnectionString);
                SqlCommand commandGetEmailSettings = new SqlCommand("SELECT port, SMTP_Server, PwordEmail, SendingEmail, NoticeToEmail, SecurityEmail FROM aspnet_SiteConfig where Numb_client = '" + ConfigurationManager.AppSettings["ClientNumber"].ToString() + "'", connection);
                connection.Open();
                SqlDataReader dataReader2 = commandGetEmailSettings.ExecuteReader();
                while (dataReader2.Read())
                {
                    string port = dataReader2["port"].ToString().Trim();
                    string SMTP_Server = dataReader2["SMTP_Server"].ToString().Trim();
                    string PwordEmail = dataReader2["PwordEmail"].ToString().Trim();
                    string SendingEmail = dataReader2["SendingEmail"].ToString().Trim();
                    string noticeToEmail = dataReader2["NoticeToEmail"].ToString().Trim();
                    string Security = dataReader2["SecurityEmail"].ToString().Trim();
                    System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
                    mail.From = new System.Net.Mail.MailAddress(SendingEmail, From == "" ? subject : From);
                    mail.To.Add(new System.Net.Mail.MailAddress(emailTo));
                    if (CcoTo != "")
                        mail.Bcc.Add(new System.Net.Mail.MailAddress(CcoTo));
                    if (noticeToEmail != "")
                        mail.Bcc.Add(new System.Net.Mail.MailAddress(noticeToEmail));

                    mail.Subject = subject;
                    mail.Body = htmlBody;
                    mail.IsBodyHtml = true;
                    mail.Priority = System.Net.Mail.MailPriority.Normal;
                    mail.BodyEncoding = System.Text.Encoding.UTF8;
                    //correo.HeadersEncoding = System.Text.Encoding.UTF8
                    mail.SubjectEncoding = System.Text.Encoding.UTF8;

                    if (document != null)
                    {
                        mail.Attachments.Add(new Attachment(document, documentFileName));
                    }

                    System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient();
                    if (Security != "None" && Security != "")
                    {
                        smtp.EnableSsl = true;

                        if (Security == "TLS") System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
                    }
                    smtp.Host = SMTP_Server;
                    smtp.Port = int.Parse(port);
                    // 464 si la cuenta es de Gmail
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new System.Net.NetworkCredential(SendingEmail, PwordEmail);
                    //smtp.EnableSsl = false;
                    // True si la cuenta es de Gmail
                    try
                    {
                        smtp.Send(mail);
                        pass = true;
                    }
                    catch (System.Exception ex)
                    {
                        saveErrors(ex.Message, false);
                    }
                }
                dataReader2.Close();
                connection.Close();
            }
            catch (Exception ee)
            {
                saveErrors(strError, false);
                return false;
            }

            return pass;

        }
        /// <summary>
        /// Metode erstatter æ,ø og å med ASCII-tegn.
        ///</summary>
        ///<param name="body">Teksten som skal oppdateres.</param>
        ///<returns>Returnern den oppdaterte teksten</returns>
        public static String erstattNorskeTegn(String body)
        {
            body = body.Replace("å", "&aring;");
            body = body.Replace("ø", "&oslash;");
            body = body.Replace("æ", "&aelig;");

            body = body.Replace("Å", "&aring;");
            body = body.Replace("Ø", "&oslash;");
            body = body.Replace("Æ", "&aelig;");

            return body;
        }

        /// <summary>
        /// Get the url of the scripts files and add it to the page.
        /// </summary>
        /// <param name="headPlaceHolder"></param>
        public static void addScriptsIntoHead(ref ContentPlaceHolder headPlaceHolder)
        {
            Literal css = new Literal();
            string currentTheme = ConfigurationManager.AppSettings["currentTheme"];
            string[] cssFiles = Directory.GetFiles(HttpContext.Current.Server.MapPath("~/App_Themes/Scripts/"));
            string segmentPoints = "";
            int count = -2;
            foreach (string segment in HttpContext.Current.Request.Url.Segments)
            {
                if (count >= 0)
                    segmentPoints += "../";
                count++;
            }

            foreach (string File in cssFiles)
            {
                if (File.IndexOf(".js") > 0)
                {
                    int index = File.IndexOf("App_Themes");
                    Literal lit = new Literal();
                    lit.Text = "<script src='" + segmentPoints + File.Substring(index, File.Length - index).Replace("\\", "/") + "' type='text/javascript' language='javascript'></script>";
                    headPlaceHolder.Page.Header.Controls.Add(lit);
                    //ScriptManager.RegisterClientScriptInclude(headPlaceHolder.Page, headPlaceHolder.Page.GetType(), Guid.NewGuid().ToString(), segmentPoints + File.Substring(index, File.Length - index).Replace("\\", "/"));
                }
            }
            //headPlaceHolder.Controls.Add(css);
        }

        public static string replaceSpecialCharacters(string word)
        {
            word = replaceNorwayCharacters(word);
            word = Regex.Replace(word, @"[^0-9a-zA-Z\s]", "");
            word = word.Replace(" ", "_");
            return word;
        }

        public static String replaceNorwayCharacters(String body)
        {
            body = body.Replace("å", "a");
            body = body.Replace("ø", "o");
            body = body.Replace("æ", "ae");

            body = body.Replace("Å", "A");
            body = body.Replace("Ø", "O");
            body = body.Replace("Æ", "AE");

            return body;
        }

        public static void AvoidMultipleSubmit(Page page, Button button, string text = "wait...")
        {
            page.ClientScript.RegisterOnSubmitStatement(page.GetType(), "ServerForm", "if(this.submitted) return false; this.submitted = true;");
            button.Attributes.Add("onclick", string.Format("if(typeof(Page_ClientValidate)=='function' && !Page_ClientValidate()){{return true;}}this.value='{1}';this.disabled=true;{0}",
                page.ClientScript.GetPostBackEventReference(button, string.Empty), text));
        }

        #endregion
    }
}