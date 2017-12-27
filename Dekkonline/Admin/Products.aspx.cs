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
    public partial class Products : System.Web.UI.Page
    {
        dbDekkOnlineDataContext db = new dbDekkOnlineDataContext();

        protected void Page_Load(object sender, EventArgs e)
        {
            if(!Page.IsPostBack && !Page.IsCallback)
            {
                loadProducts(true);
            }


            string folder = "";
            try
            {
                folder = Session["lastProdId"].ToString();
            }
            catch { }
            if (folder == "") folder = "tmp";


            string multipleFolder = "";
            try
            {
                multipleFolder = Session["multipleFolder"].ToString();
            }
            catch { }
            if (multipleFolder == "") multipleFolder = "tmp";


            xGalleries.Settings.InitialFolder = folder;
            xGalleries.Refresh();

            xGalleriesM.Settings.InitialFolder = multipleFolder;
            xGalleriesM.Refresh();

            xgvProducts.DataSource = Session["dsProducts"];
            xgvProducts.DataBind();
        }

      
        protected void xcpProducts_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
        {
            string cats = "";
            string c1 = "";
            string c2 = "";
            if (e.Parameter == "Update")
            {
                cats = dekkpro.updateData(false);
                loadProducts(true);
            }

            if (cats != "")
            {
                c1 = cats.Split('|')[0];
                c2 = cats.Split('|')[1];
            }

            xcpProducts.JSProperties.Add("cpC1", c1);
            xcpProducts.JSProperties.Add("cpC2", c2);

            loadProducts(true);
            xgvProducts.DataSource = Session["dsProducts"];
            xgvProducts.DataBind();
        }

      


        void loadProducts(bool? status)
        {
            var products = from pro in db.products
                           let disc = pro.proDiscount.HasValue ? pro.proDiscount.Value : 0
                           where (status.HasValue == false || pro.proStatus == status.Value)
                           && pro.categoriesDP.cdpStatus == true
                           select new
                           {
                               Id = pro.proId,
                               ImageLink = pro.proImage,
                               Brand = pro.brand.braName,
                               TyreSize = pro.proTyreSize,
                               Type = pro.proName,
                               LI = pro.proLoadIndex,
                               SI = pro.proSpeed,
                               Code = pro.proCode,
                               OurCategory = pro.category.catName,
                               Category = pro.categoriesDP.cdpName,
                               F = pro.proFuel,
                               W = pro.proWet,
                               N = pro.proNoise,
                               SuggestedPrice = (pro.proCoverPrice * ((100 + (pro.brand.braPercent.HasValue ? pro.brand.braPercent.Value : 0)) / 100)) - disc,
                               Stock = pro.proInventory
                           };
            Session["dsProducts"] = products.ToList();

        }

        protected void UploadControl_FileUploadComplete(object sender, FileUploadCompleteEventArgs e)
        {

            if (!e.IsValid)
            {
                return;
            }
            try
            {

                string strProductImage = uploadSelectedImage(e.UploadedFile, e.UploadedFile.FileName);
                Session["imgURL"] = strProductImage;
                e.CallbackData = strProductImage;

            }
            catch (Exception ex)
            {
                engine.Global_settings.saveErrors(ex.ToString() + " UploadControl_FileUploadComplete admin/products.aspx", true);
            }
        }


        protected void UploadControlM_FileUploadComplete(object sender, FileUploadCompleteEventArgs e)
        {

            if (!e.IsValid)
            {
                return;
            }
            try
            {

                string strProductImage = uploadSelectedImage(e.UploadedFile, e.UploadedFile.FileName, true);
                Session["imgURL"] = strProductImage;
                e.CallbackData = strProductImage;

            }
            catch (Exception ex)
            {
                engine.Global_settings.saveErrors(ex.ToString() + " UploadControl_FileUploadComplete admin/products.aspx", true);
            }
        }

        protected string uploadSelectedImage(UploadedFile myFile, string fileName, bool multi = false)
        {

            string posted = "notPosted";

            string sSavePath = "/photos/products/";
            
            long nFileLen = myFile.PostedFile.ContentLength;

            byte[] myData = new Byte[nFileLen];
            myFile.PostedFile.InputStream.Read(myData, 0, int.Parse(nFileLen.ToString()));

            string sFilename = System.IO.Path.GetFileName(myFile.FileName);
            int file_append = 0;
            string sFilenameF = "";

            if (multi) sFilenameF = Session["multipleFolder"].ToString() + sFilename.Substring(fileName.LastIndexOf('.'));
            else sFilenameF = Session["lastProdId"].ToString() + sFilename.Substring(fileName.LastIndexOf('.'));

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

        protected void popProduct_WindowCallback(object source, PopupWindowCallbackArgs e)
        {
            int proId = int.Parse(e.Parameter.Split('|')[1]);
            Session["lastProdId"] = proId;
            var cProd = (from pro in db.products where pro.proId == proId select pro).FirstOrDefault();

            string folder = "~\\photos\\products\\" + proId.ToString();
            if (!System.IO.Directory.Exists(Server.MapPath(folder)))
            {
                Directory.CreateDirectory(Server.MapPath(folder));
            }
            
            xGalleries.Settings.InitialFolder = proId.ToString();
            xGalleries.Refresh();

            if (e.Parameter.Split('|')[0] == "Load")
            {
                cmbBrands.Value = cProd.braId.ToString();
                if (cProd.categoriesDP.cdpStatus == true) cmbCategories.Value = cProd.cdpId;
                cmbOurCategories.Value = cProd.catId.ToString();
                
                txtProduct.Text = cProd.proName;
                memDescriptionDP.Text = cProd.proDescriptionDP;
                txtCodeDP.Text = cProd.proCodeDP;
                spProfileDP.Text = cProd.proDimensionProfileDP.ToString();
                spWidthDP.Text = cProd.proDimensionWidthDP.ToString();
                spDiameterDP.Text = cProd.proDimensionDiameterDP.ToString();
                spLoadIndexDP.Text = cProd.proLoadIndexDP.ToString();
                txtSpeedDP.Text = cProd.proSpeed;
                txtFuelDP.Text = cProd.proFuelDP;
                txtWetDP.Text = cProd.proWetDP;
                txtNoiseDP.Text = cProd.proNoiseDP;
                spCoverPriceDP.Text = cProd.proCoverPriceDP.ToString();
                spSuggestedPriceDP.Text = cProd.proSuggestedPriceDP.ToString();

                memDescription.Text = cProd.proDescription;
                txtCode.Text = cProd.proCode;
                spProfile.Text = cProd.proDimensionProfile.ToString();
                spWidth.Text = cProd.proDimensionWidth.ToString();
                spDiameter.Text = cProd.proDimensionDiameter.ToString();
                spLoadIndex.Text = cProd.proLoadIndex.ToString();
                txtSpeed.Text = cProd.proSpeed;
                txtFuel.Text = cProd.proFuel;
                txtWet.Text = cProd.proWet;
                txtNoise.Text = cProd.proNoise;
                spCoverPrice.Text = cProd.proCoverPrice.ToString();
                var disc = cProd.proDiscount.HasValue ? cProd.proDiscount.Value : 0;
                spSuggestedPrice.Text = (cProd.proCoverPrice * ((100 + (cProd.brand.braPercent.HasValue ? cProd.brand.braPercent.Value : 0)) / 100)-disc).ToString();

                Session["imgURL"] = cProd.proImage;
                popProduct.JSProperties.Add("cpImage", cProd.proImage);
            }else if (e.Parameter.Split('|')[0] == "Copy")
            {

                memDescription.Text = cProd.proDescriptionDP;
                txtCode.Text = cProd.proCodeDP;
                spProfile.Text = cProd.proDimensionProfileDP.ToString();
                spWidth.Text = cProd.proDimensionWidthDP.ToString();
                spDiameter.Text = cProd.proDimensionDiameterDP.ToString();
                spLoadIndex.Text = cProd.proLoadIndexDP.ToString();
                txtSpeed.Text = cProd.proSpeedDP;
                txtFuel.Text = cProd.proFuelDP;
                txtWet.Text = cProd.proWetDP;
                txtNoise.Text = cProd.proNoiseDP;
                spCoverPrice.Text = cProd.proCoverPriceDP.ToString();
                //spSuggestedPrice.Text = cProd.proSuggestedPriceDP.ToString();
            }
            else if (e.Parameter.Split('|')[0] == "Save")
            {
                cProd.proName = txtProduct.Text;

                cProd.proDescription = memDescription.Text;
                cProd.proCode= txtCode.Text;
                try { cProd.proDimensionProfile = int.Parse(spProfile.Text); } catch { }
                try { cProd.proDimensionWidth = int.Parse(spWidth.Text); } catch { }
                try { cProd.proDimensionDiameter = int.Parse(spDiameter.Text); } catch { }
                cProd.proSpeed=txtSpeed.Text;
                cProd.proFuel=txtFuel.Text;
                cProd.proWet=txtWet.Text;
                cProd.proNoise=txtNoise.Text;
                try { cProd.proCoverPrice = decimal.Parse(spCoverPrice.Text); } catch { }
                try {
                    var disc = cProd.proDiscount.HasValue ? cProd.proDiscount.Value : 0;
                    var spLast = (cProd.proCoverPrice * ((100 + (cProd.brand.braPercent.HasValue ? cProd.brand.braPercent.Value : 0)) / 100) - disc);
                    var spActual = decimal.Parse(spSuggestedPrice.Text);
                    if (spLast != spActual){
                        var dif = spLast - spActual;
                        cProd.proDiscount = disc - dif;
                    }
                    cProd.proSuggestedPrice = decimal.Parse(spSuggestedPrice.Text);
                } catch { }
                cProd.proTyreSize = cProd.proDimensionWidth.ToString() + cProd.proDimensionProfile.ToString() + cProd.proDimensionDiameter.ToString();

                cProd.proImage = Session["imgURL"].ToString();
                cProd.proEdited = true;

                Guid braId = Guid.Parse(cmbBrands.Value.ToString());
                //Guid cdpId = Guid.Parse(cmbCategories.Value.ToString());
                int? catId = null;
                if (cmbOurCategories.Value.ToString() != "") catId = int.Parse(cmbOurCategories.Value.ToString());

                cProd.braId = braId;
                //cProd.cdpId = cdpId;
                cProd.catId = catId;
                cProd.proLastUpdate = DateTime.Now;
                db.SubmitChanges();
            }
        }

        protected void popMultiple_WindowCallback(object source, PopupWindowCallbackArgs e)
        {
            var lstProIds = xgvProducts.GetSelectedFieldValues("Id");
            var lstFiles = new List<string>();

            if (e.Parameter == "Load")
            {
                List<string> lstProducts = new List<string>();
                List<string> lstImages = new List<string>();
                List<string> lstDescriptions = new List<string>();
                List<string> lstLIs = new List<string>();
                List<string> lstSpeeds = new List<string>();
                List<string> lstFuels = new List<string>();
                List<string> lstWets = new List<string>();
                List<string> lstNoises = new List<string>();
                List<Guid> lstBrands = new List<Guid>();
                List<int?> lstOCs = new List<int?>();

                string folder = "";
                foreach (int proId in lstProIds)
                {
                    var cProd = (from pro in db.products where pro.proId == proId select pro).FirstOrDefault();

                    folder = "~\\photos\\products\\" + proId.ToString();
                    if (!System.IO.Directory.Exists(Server.MapPath(folder)))
                    {
                        Directory.CreateDirectory(Server.MapPath(folder));
                    }
                    else
                    {
                        foreach (string file in Directory.GetFiles(Server.MapPath(folder)))
                        {
                            lstFiles.Add(file.Substring(file.LastIndexOf("\\")));
                        }
                    }

                    lstProducts.Add(cProd.proName);
                    lstImages.Add(cProd.proImage);
                    lstDescriptions.Add(cProd.proDescription);
                    lstLIs.Add(cProd.proLoadIndex);
                    lstFuels.Add(cProd.proFuel);
                    lstWets.Add(cProd.proWet);
                    lstNoises.Add(cProd.proNoise);
                    lstBrands.Add(cProd.braId);
                    lstOCs.Add(cProd.catId);
                }


                var dFiles = lstFiles.Distinct();
                var dProducts = lstProducts.Distinct();
                var dImages = lstImages.Distinct();
                var dDescriptions = lstDescriptions.Distinct();
                var dLIs = lstLIs.Distinct();
                var dFuels = lstFuels.Distinct();
                var dWets = lstWets.Distinct();
                var dNoises = lstNoises.Distinct();
                var dBrands = lstBrands.Distinct();
                var dOCs = lstOCs.Distinct(); ;


                if (dProducts.Count() == 1) txtProductM.Text = dProducts.FirstOrDefault();
                else txtProductM.Text = "";
                if (dDescriptions.Count() == 1) memDescriptionM.Text = dDescriptions.FirstOrDefault();
                else memDescriptionM.Text = "";
                if (dLIs.Count() == 1) spLoadIndexM.Text = dLIs.FirstOrDefault();
                else spLoadIndexM.Text = "";
                if (dFuels.Count() == 1) txtFuelM.Text = dFuels.FirstOrDefault();
                else txtFuelM.Text = "";
                if (dWets.Count() == 1) txtWetM.Text = dWets.FirstOrDefault();
                else txtWetM.Text = "";
                if (dNoises.Count() == 1) txtNoiseM.Text = dNoises.FirstOrDefault();
                else txtNoiseM.Text = "";
                if (dBrands.Count() == 1) cmbBrandM.Value = dBrands.FirstOrDefault().ToString();
                else cmbBrandM.Text = "";
                if (dOCs.Count() == 1) cmbOurCategoriesM.Value = dOCs.FirstOrDefault().ToString();
                else cmbOurCategoriesM.Text = "";

                if (dImages.Count() == 1) txtImgUrl.Text = dImages.FirstOrDefault();
                Session["imgURL"] = txtImgUrl.Text;
                popMultiple.JSProperties.Add("cpImage", txtImgUrl.Text);

                int allProd = lstProIds.Count();

                string folderName = DateTime.Now.ToString("yyMMddHHmmss");
                string tmpFolder = "~\\photos\\products\\multiple\\" + folderName;
                Session["multipleFolder"] = folderName;

                Directory.CreateDirectory(Server.MapPath(tmpFolder));

                foreach (string cFile in dFiles)
                {
                    int count = (from fil in lstFiles where fil == cFile select fil).Count();

                    if (count == allProd)
                    {
                        System.IO.File.Copy(Server.MapPath(folder + "\\" + cFile), Server.MapPath(tmpFolder + "\\" + cFile));
                    }
                }

                xGalleriesM.Settings.InitialFolder = folderName;
                xGalleriesM.Refresh();
            }
            else if (e.Parameter == "Save")
            {
                string folderName = Session["multipleFolder"].ToString();
                string tmpFolder = "~\\photos\\products\\multiple\\" + folderName;
                foreach (int proId in lstProIds)
                {
                    try
                    {
                        var cProd = (from pro in db.products where pro.proId == proId select pro).FirstOrDefault();

                        if(txtProductM.Text != "") cProd.proName = txtProductM.Text;
                        if(memDescriptionM.Text != "")cProd.proDescription = memDescriptionM.Text;
                        if (spLoadIndexM.Text != "") cProd.proLoadIndex = spLoadIndexM.Text;
                        if (txtSpeedM.Text != "") cProd.proSpeed = txtSpeedM.Text;
                        if (txtFuelM.Text != "") cProd.proFuel = txtFuelM.Text;
                        if (txtWetM.Text != "") cProd.proWet = txtWetM.Text;
                        if (txtNoiseM.Text != "") cProd.proNoise = txtNoiseM.Text;
                        if (cmbBrandM.Text != "")
                        if (spPriceP.Text != "") cProd.proDiscount = cProd.proCoverPrice * ((decimal.Parse(spPriceP.Text) / 100));
                        if (spPriceM.Text != "") cProd.proDiscount =  decimal.Parse(spPriceM.Text);


                        {
                            try
                            {
                                Guid braId = Guid.Parse(cmbBrandM.Value.ToString());
                                cProd.braId = braId;
                            }
                            catch { }
                        }

                        if (cmbOurCategoriesM.Text != "")
                        {
                            try
                            {
                                int catId = int.Parse(cmbOurCategoriesM.Value.ToString());
                                cProd.catId = int.Parse(cmbOurCategoriesM.Value.ToString());
                            }
                            catch { }
                        }
                        
                        if (Session["imgURL"].ToString() != "") cProd.proImage = Session["imgURL"].ToString();
                      

                        foreach (string file in Directory.GetFiles(Server.MapPath(tmpFolder)))
                        {
                            string folder = "~\\photos\\products\\" + proId.ToString();
                            string fileName = file.Substring(file.LastIndexOf("\\"));
                            System.IO.File.Copy(Server.MapPath(tmpFolder + "\\" + fileName), Server.MapPath(folder + "\\" + fileName), true);
                        }
                    }
                    catch { }

                }

                db.SubmitChanges();
                try
                {
                    System.IO.Directory.Delete(tmpFolder);
                }
                catch { }
            }
        }

        protected void popSection_WindowCallback(object source, PopupWindowCallbackArgs e)
        {
            var lstProIds = xgvProducts.GetSelectedFieldValues("Id");
            foreach (int proId in lstProIds)
            {
                try
                {
                    var cProd = (from pro in db.products where pro.proId == proId select pro).FirstOrDefault();

                  
                    cProd.catId = int.Parse(cmbSection.Value.ToString());
                  
                }
                catch { }

            }

            db.SubmitChanges();
         
        }
    
    }
}