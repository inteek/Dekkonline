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
using DekkOnline.engine;

namespace DekkOnline.Admin
{
    public partial class Brands : System.Web.UI.Page
    {
        dbDekkOnlineDataContext db = new dbDekkOnlineDataContext();

        protected void Page_Load(object sender, EventArgs e)
        {

        }


        protected void xcpBrands_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
        {
           
            string cats = "";
            string c1 = "";
            string c2 = "";
            if (e.Parameter == "Update")
            {
                cats = dekkpro.updateBrands(false);
            }

            if (cats != "")
            {
                c1 = cats.Split('|')[0];
                c2 = cats.Split('|')[1];
            }

            xcpBrands.JSProperties.Add("cpC1", c1);
            xcpBrands.JSProperties.Add("cpC2", c2);
            xgvBrands.DataBind();
        }

      



        protected void UploadControl_FileUploadComplete(object sender, FileUploadCompleteEventArgs e)
        {

            if (!e.IsValid)
            {
                return;
            }
            try
            {

                string strBrandsImage = dekkpro.uploadSelectedImage("/photos/Brands/", e.UploadedFile, e.UploadedFile.FileName, Server.MapPath("~/"), Session["lastBraId"].ToString());
                Session["imgBrandsURL"] = strBrandsImage;
                e.CallbackData = strBrandsImage;

            }
            catch (Exception ex)
            {
                engine.Global_settings.saveErrors(ex.ToString() + " UploadControl_FileUploadComplete admin/Brands.aspx", true);
            }
        }

        

        protected void popBrands_WindowCallback(object source, PopupWindowCallbackArgs e)
        {
            Guid braId = Guid.Parse(e.Parameter.Split('|')[1]);
            Session["lastBraId"] = braId;
            var cBra = (from bra in db.brands where bra.braId == braId select bra).FirstOrDefault();

            if (e.Parameter.Split('|')[0] == "Load")
            {


                txtBrands.Text = cBra.braName;
                txtDescriptionDP.Text = "";
                txtDescription.Text = cBra.braDescription;
                txtCodeDP.Text = cBra.braCodeDP;
                txtCode.Text = cBra.braCode;
                spPriceP.Text = cBra.braPercent.HasValue == true ? cBra.braPercent.Value.ToString() : "";

                Session["imgBrandsURL"] = cBra.braImage;
                popBrands.JSProperties.Add("cpImage", cBra.braImage);
            }
            else if (e.Parameter.Split('|')[0] == "Copy")
            {

                txtDescription.Text = txtCodeDP.Text;
                txtCode.Text = cBra.braCodeDP;

            }
            else if (e.Parameter.Split('|')[0] == "Save")
            {
                cBra.braName = txtBrands.Text;
                cBra.braDescription = txtDescription.Text;
                cBra.braCode = txtCode.Text;
                cBra.braImage = Session["imgBrandsURL"].ToString();
                cBra.braEdited = true;

                if (spPriceP.Text != "") cBra.braPercent = int.Parse(spPriceP.Text);
                else cBra.braPercent = 0;

                db.SubmitChanges();
            }
        }

        protected void popSection_WindowCallback(object source, PopupWindowCallbackArgs e)
        {
            var lstBraIds = xgvBrands.GetSelectedFieldValues("braId");
            foreach (Guid braId in lstBraIds)
            {
                try
                {
                    var cBra = (from bra in db.brands where bra.braId == braId select bra).FirstOrDefault();
                    if (spPricePM.Text != "") cBra.braPercent = int.Parse(spPricePM.Text);
                    else cBra.braPercent = 0;

                }
                catch { }

            }

            db.SubmitChanges();

        }

        protected void lnqBrands_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            e.Result = dekkpro.getBrandList();
        }
    }
}