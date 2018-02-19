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


        protected void xcpCategories_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
        {
            xgvCategories.DataBind();
        }

        protected void btnUpdateCats_Click(object sender, EventArgs e)
        {
           
            xgvCategories.DataBind();
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
                Session["imgOurCategoriesURL"] = strCategoriesImage;
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
            string sFilenameF = Session["lastCatId"].ToString() + sFilename.Substring(fileName.LastIndexOf('.'));
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

        protected void popCategories_WindowCallback(object source, PopupWindowCallbackArgs e)
        {
            int catId = int.Parse(e.Parameter.Split('|')[1]);
            Session["lastCatId"] = catId;
            var cCat = new category();

            if(catId > 0) cCat = (from cdp in db.categories where cdp.catId == catId select cdp).FirstOrDefault();

            if (e.Parameter.Split('|')[0] == "Load")
            {
                if (catId == 0)
                {
                    txtCategories.Text = "";
                    txtDescription.Text = "";
                    Session["imgOurCategoriesURL"] = "";
                    popCategories.JSProperties.Add("cpImage", "");
                }
                else
                {
                    txtCategories.Text = cCat.catName;
                    txtDescription.Text = cCat.catDescription;
                    Session["imgOurCategoriesURL"] = cCat.catImage;
                    popCategories.JSProperties.Add("cpImage", cCat.catImage);
                }
            }
            else if (e.Parameter.Split('|')[0] == "Save")
            {
                cCat.catName = txtCategories.Text;
                cCat.catDescription = txtDescription.Text;
                cCat.catImage = Session["imgOurCategoriesURL"].ToString();

                if (catId == 0)
                {
                    cCat.catStatus = true;
                    db.categories.InsertOnSubmit(cCat);
                }

                db.SubmitChanges();
            }
        }

        protected void xcaStatus_Callback(object source, CallbackEventArgs e)
        {


            int catId = int.Parse(e.Parameter);

            var cCategorie = db.categories.Where(c => c.catId == catId).FirstOrDefault();
            cCategorie.catStatus = !cCategorie.catStatus;

            db.SubmitChanges();
            xgvCategories.DataBind();
            
        }
    }
}