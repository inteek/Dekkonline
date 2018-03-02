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
    public partial class Categories : System.Web.UI.Page
    {
        dbDekkOnlineDataContext db = new dbDekkOnlineDataContext();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsCallback)
            {
                txtCC1.Text = db.categoriesDPs.Where(c=>c.cdpStatus ==false).Count().ToString();
                txtCC2.Text = db.categoriesDPs.Where(c => c.cdpStatus == true).Count().ToString();
            }
            

        }


        protected void xcpCategories_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
        {
            string cats = "";
            string c1 = "";
            string c2 = "";
            if (e.Parameter == "Update")
            {
                cats = dekkpro.updateCategories(false);
                xgvCategories1.DataBind();
                xgvCategories2.DataBind();
            }

            if(cats != "")
            {
                c1 = cats.Split('|')[0];
                c2 = cats.Split('|')[1];
            }

            xcpCategories.JSProperties.Add("cpC1", c1);
            xcpCategories.JSProperties.Add("cpC2", c2);

            xgvCategories1.DataBind();
            xgvCategories2.DataBind();
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
                Session["imgCategoriesURL"] = strCategoriesImage;
                e.CallbackData = strCategoriesImage;

            }
            catch (Exception ex)
            {
                engine.Global_settings.saveErrors(ex.ToString() + " UploadControl_FileUploadComplete admin/Categories.aspx", true);
            }
        }

        protected string uploadSelectedImage(UploadedFile myFile, string fileName)
        {
            return dekkpro.uploadSelectedImage("/photos/Categories/", myFile, fileName, Server.MapPath("~/"), Session["lastCdpId"].ToString());
        }

        protected void popCategories_WindowCallback(object source, PopupWindowCallbackArgs e)
        {
            Guid cdpId = Guid.Parse(e.Parameter.Split('|')[1]);
            Session["lastCdpId"] = cdpId;
            var cCat = (from cdp in db.categoriesDPs where cdp.cdpId == cdpId select cdp).FirstOrDefault();

            if (e.Parameter.Split('|')[0] == "Load")
            {


                txtCategories.Text = cCat.cdpName;
                txtDescriptionDP.Text = cCat.cdpDescriptionDP;
                txtDescription.Text = cCat.cdpDescription;

                Session["imgCategoriesURL"] = cCat.cdpImage;
                popCategories.JSProperties.Add("cpImage", cCat.cdpImage);
            }
            else if (e.Parameter.Split('|')[0] == "Copy")
            {

                txtDescription.Text = cCat.cdpDescriptionDP;
             
            }
            else if (e.Parameter.Split('|')[0] == "Save")
            {
                cCat.cdpName = txtCategories.Text;
                cCat.cdpDescription = txtDescription.Text;
                cCat.cdpImage = Session["imgCategoriesURL"].ToString();
                cCat.cdpEdited = true;
                db.SubmitChanges();
            }
        }

        protected void xcaStatus_Callback(object source, CallbackEventArgs e)
        {

          
            Guid cdpId = Guid.Parse(e.Parameter);
            dekkpro.changeStatusCategories(cdpId);
         
            xgvCategories1.DataBind();
            xgvCategories2.DataBind();

            xcaStatus.JSProperties.Add("cpC1", xgvCategories1.VisibleRowCount);
            xcaStatus.JSProperties.Add("cpC2", xgvCategories2.VisibleRowCount);
        }
    }
}