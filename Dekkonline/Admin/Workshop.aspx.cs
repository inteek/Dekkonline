using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.IO;
using System.Text;
using System.Data;
using DekkOnline.engine;
using System.Configuration;
using System.Data.SqlClient;
using DevExpress.Web;
using DevExpress.Web.Internal;

namespace DekkOnline.Admin
{
    public partial class Workshop : System.Web.UI.Page
    {
        dbDekkOnlineDataContext db = new dbDekkOnlineDataContext();

        protected void Page_Load(object sender, EventArgs e)
        {

        }


        protected void xcpWorkshop_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
        {
            xgvWorkshop.DataBind();
        }

        protected void btnUpdateCats_Click(object sender, EventArgs e)
        {
           
            xgvWorkshop.DataBind();
        }



        protected void UploadControl_FileUploadComplete(object sender, FileUploadCompleteEventArgs e)
        {

            if (!e.IsValid)
            {
                return;
            }
            try
            {

                string strCategoriesImage = uploadSelectedImage(e.UploadedFile, e.UploadedFile.FileName);
                Session["imgOurWorkshopURL"] = strCategoriesImage;
                e.CallbackData = strCategoriesImage;

            }
            catch (Exception ex)
            {
                engine.Global_settings.saveErrors(ex.ToString() + " UploadControl_FileUploadComplete admin/ourCategories.aspx", true);
            }
        }

        protected string uploadSelectedImage(UploadedFile myFile, string fileName)
        {

            string posted = "notPosted";

            string sSavePath = "/photos/ourCategories/";

            long nFileLen = myFile.PostedFile.ContentLength;

            byte[] myData = new Byte[nFileLen];
            myFile.PostedFile.InputStream.Read(myData, 0, int.Parse(nFileLen.ToString()));

            string sFilename = System.IO.Path.GetFileName(myFile.FileName);
            int file_append = 0;
            string sFilenameF = Session["lastWorkshopId"].ToString() + sFilename.Substring(fileName.LastIndexOf('.'));
            while (System.IO.File.Exists(Server.MapPath("~/" + sSavePath + sFilenameF)))
            {
                file_append++;
                sFilenameF = System.IO.Path.GetFileNameWithoutExtension(sFilenameF)
                                 + file_append.ToString() + sFilenameF.Substring(sFilenameF.LastIndexOf('.'));
            }

            System.IO.FileStream newFile
                    = new System.IO.FileStream(Server.MapPath("~/" + sSavePath + sFilenameF),
                                               System.IO.FileMode.Create);

            newFile.Write(myData, 0, myData.Length);

            using (System.Drawing.Image image = System.Drawing.Image.FromStream(newFile))
            {
                int fileWidth = image.Width;
                int fileHeight = image.Height;
                if (!((fileWidth <= 4000) && (fileHeight <= 4000)))
                {
                    posted = "notPostedCheckResolution";
                }
                newFile.Close();
                if (posted == "notPostedCheckResolution")
                {
                    FileInfo imageForDelete = new FileInfo(Server.MapPath("~/" + sSavePath + sFilename));
                    imageForDelete.Delete();
                    sFilename = posted;
                }
            }
            posted = sSavePath + sFilenameF;

            return posted;
        }

        protected void popWorkshop_WindowCallback(object source, PopupWindowCallbackArgs e)
        {
            int workshopId = int.Parse(e.Parameter.Split('|')[1]);
            Session["lastWorkshopId"] = workshopId;
            var wWorkshop = new DekkOnline.Workshop();

            if (workshopId > 0) wWorkshop = (from w in db.Workshop where w.IdWorkshop == workshopId select w).FirstOrDefault();

            if (e.Parameter.Split('|')[0] == "Load")
            {
                if (workshopId == 0)
                {
                    txtName.Text = "";
                    txtAddress.Text = "";
                    txtPhone.Text = "";
                    txtEmail.Text = "";
                    Session["imgOurWorkshopURL"] = "";
                    popWorkshop.JSProperties.Add("cpImage", "");
                }
                else
                {
                    txtName.Text = wWorkshop.Name;
                    txtAddress.Text = wWorkshop.Address;
                    txtPhone.Text = wWorkshop.Phone;
                    txtEmail.Text = wWorkshop.Email;
                    Session["imgOurWorkshopURL"] = wWorkshop.WorkImage == null || wWorkshop.WorkImage == "" ? "" : wWorkshop.WorkImage;

                    if (wWorkshop.WorkImage != null && wWorkshop.WorkImage.IndexOf("/photos/ourCategories/") == 0)
                    {
                        popWorkshop.JSProperties.Add("cpImage", wWorkshop.WorkImage);
                    }
                    else
                    {
                        popWorkshop.JSProperties.Add("cpImage", wWorkshop.WorkImage == null || wWorkshop.WorkImage == "" ? "" : ConfigurationManager.AppSettings["URLSTORE"] + wWorkshop.WorkImage);
                    }

                }
            }
            else if (e.Parameter.Split('|')[0] == "Save")
            {
                wWorkshop.Name = txtName.Text;
                wWorkshop.Address = txtAddress.Text;
                wWorkshop.Phone = txtPhone.Text;
                wWorkshop.Email = txtEmail.Text;
                wWorkshop.WorkImage = Session["imgOurWorkshopURL"].ToString();

                if (workshopId == 0)
                {
                    wWorkshop.Status = true;
                    db.Workshop.InsertOnSubmit(wWorkshop);
                }

                db.SubmitChanges();
            }
        }

        protected void xcaStatus_Callback(object source, CallbackEventArgs e)
        {


            int WorkshopId = int.Parse(e.Parameter);

            var wWorkshop = db.Workshop.Where(w => w.IdWorkshop == WorkshopId).FirstOrDefault();
            wWorkshop.Status = !wWorkshop.Status;

            db.SubmitChanges();
            xgvWorkshop.DataBind();
            
        }
    }
}