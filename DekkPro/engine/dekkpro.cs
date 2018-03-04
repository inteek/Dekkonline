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
using Newtonsoft.Json;
using DevExpress.Web;
using System.Configuration;

namespace DekkOnline2.engine2
{

    #region "Functions"

    public class dekkpro : engine2.dekkproObject
    {
        public static DateTime lastUpdate;
        public static DateTime lastUpdateCats;
        public static DateTime lastUpdateBrands;
        public static string dekkProURL;
        public static dbDekkOnlineDataContext db = new dbDekkOnlineDataContext();

        #region "Update from carrier"

        static string generateToken()
        {
            dbDekkOnlineDataContext db = new dbDekkOnlineDataContext();
            DateTime now = DateTime.Now.Date;

            string username = ConfigurationManager.AppSettings["dekkProUsername"];
            string password = ConfigurationManager.AppSettings["dekkProPassword"];
            dekkProURL = ConfigurationManager.AppSettings["dekkProUrl"];

            var cToken = new token();
            var checkTokens = (from tok in db.tokens where tok.tokDate >= now orderby tok.tokId descending select tok);
            string token = "";
            if (checkTokens.Count() > 0)
            {
                cToken = checkTokens.FirstOrDefault();
                token = cToken.tokData;
                lastUpdate = cToken.tokStockLastUpdate;
                lastUpdateCats = cToken.tokCategoriesLastUpdate;
                lastUpdateBrands = cToken.tokBrandsLastUpdate;
            }
            else
            {
                cToken = db.tokens.FirstOrDefault();
                if (cToken == null)
                {
                    cToken = new token();
                    db.tokens.InsertOnSubmit(cToken);
                }
                else
                {
                    lastUpdate = cToken.tokStockLastUpdate;
                }
                cToken.tokDate = now;
               

                string urlToken = ConfigurationManager.AppSettings["dekkProUrlTokken"]; ;
                HttpWebRequest myRT = (HttpWebRequest)WebRequest.Create(urlToken);
                myRT.ContentType = "application/x-www-form-urlencoded";


                var postData = "grant_type=password";
                postData += "&username=" + username;
                postData += "&password=" + password;
                var data = Encoding.ASCII.GetBytes(postData);

                myRT.Method = "POST";
                myRT.ContentLength = data.Length;


                Stream postStream = myRT.GetRequestStream();
                postStream.Write(data, 0, data.Length);
                postStream.Flush();
                postStream.Close();

                var response = (HttpWebResponse)myRT.GetResponse();

                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                token = responseString.Split(',')[0].Split(':')[1].Substring(1).TrimEnd('"');

                cToken.tokData = token;
                db.SubmitChanges();
            }

            return token;
        }


        public static string updateCategories(bool overwrite)
        {
            dbDekkOnlineDataContext db = new dbDekkOnlineDataContext();
            string token = generateToken();
            string url = dekkProURL + "/GetProductCategories?";
            url += "request.timeStampFrom=" + lastUpdateCats.ToString("yyyy-MM-ddTHH:mm:ss");

            HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create(url);
            myReq.Accept = "application/json";
            myReq.ContentType = "application/json";
            //myReq.Method = "POST";
            myReq.Headers["Authorization"] = "Bearer " + token;
            WebResponse wr = myReq.GetResponse();
            Stream receiveStream = wr.GetResponseStream();
            StreamReader reader = new StreamReader(receiveStream, Encoding.UTF8);
            //string content = reader.ReadToEnd();
            string json = reader.ReadToEnd();
            dkCategoriesDP lis = JsonConvert.DeserializeObject<dkCategoriesDP>(json);
            var categories = db.categoriesDPs.ToList();
            int count = 0;
            int nCount = 0;
            foreach (var cat in lis.CategoriesDekk)
            {
                var nCat = new categoriesDP();
                Guid cdpId = Guid.Parse(cat.ProductCategoryId);
                var cCat = categories.Where(c => c.cdpId == cdpId);

                if (cCat.FirstOrDefault() != null) nCat = cCat.FirstOrDefault();
                else
                {
                    nCount++;
                    db.categoriesDPs.InsertOnSubmit(nCat);
                    nCat.cdpStatus = true;
                }


                nCat.cdpId = cdpId;
                nCat.cdpDescriptionDP = cat.Description;
                if (nCat.cdpDescriptionDP == null) nCat.cdpDescriptionDP = "";
                nCat.cdpNameDP = cat.Name;
                try { if (cat.ProductCategoryParentId != "") nCat.cdpParentId = Guid.Parse(cat.ProductCategoryParentId); } catch { }

                if (nCat.cdpEdited != true)
                {
                    nCat.cdpDescription = nCat.cdpDescriptionDP;
                    nCat.cdpName = nCat.cdpNameDP;
                    nCat.cdpImage = "";
                }
                else
                {
                    nCat.cdpStatus = false;
                }

                count++;

            }
            db.tokens.FirstOrDefault().tokCategoriesLastUpdate = DateTime.Now;
            db.SubmitChanges();
            count = count - nCount;
            return count.ToString() + "|" + nCount.ToString();
        }

        public static string updateBrands(bool overwrite)
        {
            dbDekkOnlineDataContext db = new dbDekkOnlineDataContext();
            string token = generateToken();
            string url = dekkProURL + "/GetProductBrands?";
            url += "request.timeStampFrom=" + lastUpdateBrands.ToString("yyyy-MM-ddTHH:mm:ss");

            HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create(url);
            myReq.Accept = "application/json";
            myReq.ContentType = "application/json";
            //myReq.Method = "POST";
            myReq.Headers["Authorization"] = "Bearer " + token;
            WebResponse wr = myReq.GetResponse();
            Stream receiveStream = wr.GetResponseStream();
            StreamReader reader = new StreamReader(receiveStream, Encoding.UTF8);
            //string content = reader.ReadToEnd();
            string json = reader.ReadToEnd();
            dkBrandsDP lis = JsonConvert.DeserializeObject<dkBrandsDP>(json);
            var brands = db.brands.ToList();
            int count = 0;
            int nCount = 0;
            foreach (var bra in lis.BrandsDekk)
            {

                Guid braId = Guid.Parse(bra.BrandId);
                var cBra = brands.Where(c => c.braId == braId);
                var nBra = new brand();
                if (cBra.FirstOrDefault() != null) nBra = cBra.FirstOrDefault();
                else
                {
                    nCount++;
                    db.brands.InsertOnSubmit(nBra);
                }

                nBra.braId = braId;
                nBra.barDescriptionDP = bra.Description;
                if (nBra.barDescriptionDP == null) nBra.barDescriptionDP = "";
                nBra.braNameDP = bra.Name;
                nBra.braCodeDP = bra.Code;


                if (nBra.braEdited != true)
                {
                    nBra.braName = nBra.braNameDP;
                    nBra.braCode = nBra.braCodeDP;
                    nBra.braDescription = nBra.barDescriptionDP;
                    nBra.braImage = "";
                }
                count++;
            }
            db.tokens.FirstOrDefault().tokBrandsLastUpdate = DateTime.Now;
            db.SubmitChanges();

            count = count - nCount;


            return count.ToString() + "|" + nCount.ToString();

        }

        public static string updateProducts(bool overwrite)
        {
            dbDekkOnlineDataContext db = new dbDekkOnlineDataContext();
            var categories = db.categoriesDPs.Where(c => c.cdpStatus == true).Select(c => c.cdpId.ToString().ToUpper()).ToList();

            int tUpdates = 0;
            int tNews = 0;

            updateProductsByCategory(categories, ref tUpdates, ref tNews);

            return tUpdates.ToString() + "|" + tNews.ToString();
        }

        public static string updateData(bool overwrite)
        {
            string cate = updateCategories(false);
            string brand = updateBrands(false);
            string prod = updateProductsAll(false);

            dbDekkOnlineDataContext db = new dbDekkOnlineDataContext();
            var dCategories = (from cat in db.categoriesDPs
                               where cat.cdpEdited == false && cat.products.Count() == 0
                               select cat).ToList();

            var dBrands = (from bra in db.brands
                           where bra.braEdited == false && bra.products.Count() == 0
                           select bra).ToList();

            db.categoriesDPs.DeleteAllOnSubmit(dCategories);
            db.brands.DeleteAllOnSubmit(dBrands);


            int cateU = int.Parse(cate.Split('|')[0]);
            int cateI = int.Parse(cate.Split('|')[1]);
            int brandU = int.Parse(brand.Split('|')[0]);
            int brandI = int.Parse(brand.Split('|')[1]);
            int prodU = int.Parse(prod.Split('|')[0]);
            int prodI = int.Parse(prod.Split('|')[1]);
            int total = cateU + cateI + brandU + brandI + prodU + prodI;

            var nBS = new bitacoraSync();
            DateTime now = DateTime.Now;
            nBS.bitDate = now;
            nBS.bitCategoriesInserted = cateI;
            nBS.bitCategoriesUpdated = cateU;
            nBS.bitBrandsInserted = brandI;
            nBS.bitBrandsUpdated = brandU;
            nBS.bitProductsInserted = prodI;
            nBS.bitProductsUpdated = prodU;

            db.bitacoraSyncs.InsertOnSubmit(nBS);
            db.SubmitChanges();

            return prod;
        }

        public static string updateProductsAll(bool overwrite)
        {
            dbDekkOnlineDataContext db = new dbDekkOnlineDataContext();
            string token = generateToken();

            string url = dekkProURL + "/GetProducts?";
            url += "request.timeStampFrom=" + lastUpdate.ToString("yyyy-MM-ddTHH:mm:ss");

            HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create(url);
            myReq.Accept = "application/json";
            myReq.ContentType = "application/json";
            //myReq.Method = "POST";
            myReq.Headers["Authorization"] = "Bearer " + token;
            WebResponse wr = myReq.GetResponse();
            Stream receiveStream = wr.GetResponseStream();
            StreamReader reader = new StreamReader(receiveStream, Encoding.UTF8);
            //string content = reader.ReadToEnd();
            string json = reader.ReadToEnd();
            productsDP lis = JsonConvert.DeserializeObject<productsDP>(json);

            int count = 0;
            int nCount = 0;

            var cProds = (from pro in db.products
                          select pro).ToList();

            var cBrands = db.brands.ToList();

            string error = "";
            foreach (productDP pdp in lis.productDP)
            {
                try
                {

                    dkGeneralInformation dgi = pdp.GeneralInformation;
                    dkTireInformation dti = pdp.TireInformation;
                    dkCategory dca = pdp.Category;
                    dkBrand dbr = pdp.Brand;
                    dkPricing dbp = pdp.Pricing;
                    Guid proUUID = dgi.ProductId;


                    int inventory = 0;


                    try { inventory = pdp.Inventory.Sum(c => c.AvailableUnits); } catch { }



                    string fuel = "";
                    string wet = "";
                    string noise = "";
                    string speed = "";
                    try
                    {
                        var FWD = dti.Description.Split('-').ToList();
                        try { fuel = FWD[0].Replace(" ", "").Substring(FWD[0].IndexOf(':') + 1); } catch { }
                        try { wet = FWD[1].Replace(" ", ""); } catch { }
                        try { noise = FWD[2].Replace(" ", ""); } catch { }
                        try { speed = dti.Speed; } catch { }
                    }
                    catch
                    {

                    }
                    if (fuel.Length > 5)
                        fuel = fuel.Substring(0, 4);
                    if (wet.Length > 5)
                        wet = wet.Substring(0, 4);

                    if (noise.Length > 5)
                        noise = noise.Substring(0, 4);

                    //if (speed == "") continue;




                    product nProd = new product();
                    var cProd = cProds.Where(c => c.proUUID == proUUID);
                    if (cProd.FirstOrDefault() != null) nProd = cProd.FirstOrDefault();
                    else
                    {
                        nCount++;
                        db.products.InsertOnSubmit(nProd);
                    }

                    nProd.proUUID = dgi.ProductId;

                    Guid cdpId = Guid.Parse(dca.ProductCategoryId);

                    nProd.cdpId = cdpId;
                    Guid braId = Guid.Parse(dbr.BrandId);
                    if (cBrands.Where(c => c.braId == braId).Count() == 0)
                    {
                        brand nBra = new brand();
                        nBra.braId = Guid.Parse(dbr.BrandId);
                        nBra.braCode = dbr.Code;
                        nBra.braName = dbr.Name;
                        nBra.braNameDP = dbr.Name;
                        nBra.braCodeDP = dbr.Code;
                        nBra.barDescriptionDP = "";
                        nBra.braDescription = "";
                        nBra.braImage = "";
                        nBra.braEdited = false;
                        db.brands.InsertOnSubmit(nBra);
                        nProd.brand = nBra;
                    }
                    else
                        nProd.braId = braId;

                    try { nProd.proSkuId = Guid.Parse(dti.ProductSkuId); } catch { }
                    nProd.proSkuDP = dti.SkuCode;
                    if (nProd.proSkuDP == null) nProd.proSkuDP = "";
                    nProd.proNameDP = dgi.Name;
                    nProd.proDescriptionDP = dgi.Name;
                    nProd.proCodeDP = dti.Code;
                    try { nProd.proDimensionProfileDP = int.Parse(dti.Dimension.Profile.Replace(".00", "")); } catch { };
                    try { nProd.proDimensionWidthDP = int.Parse(dti.Dimension.Width.Replace(".00", "")); } catch { };
                    try { nProd.proDimensionDiameterDP = int.Parse(dti.Dimension.Diameter.Replace(".00", "")); } catch { };
                    nProd.proCoverPriceDP = dbp.CoverPrice;
                    nProd.proSuggestedPriceDP = dbp.SuggestedPrice;
                    nProd.proRCRDP = dti.RollerCircumReference;
                    nProd.proLoadIndexDP = dti.LoadIndex;
                    nProd.proSpeedDP = dti.Speed;
                    nProd.proProductCodeDP = dgi.ProductCode;
                    nProd.proFuelDP = fuel;
                    nProd.proWetDP = wet;
                    nProd.proNoiseDP = noise;
                    string tyreSize = nProd.proDimensionWidthDP.ToString() + nProd.proDimensionProfileDP.ToString() + nProd.proDimensionDiameterDP.ToString();
                    nProd.proLastUpdateDP = DateTime.Now;
                    if (tyreSize.Length > 10)
                        tyreSize = tyreSize.Substring(0, 9);

                    nProd.proTyreSizeDP = tyreSize;

                    if (nProd.proEdited != true)
                    {
                        nProd.proSku = nProd.proSkuDP;
                        nProd.proName = nProd.proNameDP;
                        nProd.proDescription = nProd.proDescriptionDP;
                        nProd.proCode = nProd.proCodeDP;
                        nProd.proDimensionProfile = nProd.proDimensionProfileDP;
                        nProd.proDimensionWidth = nProd.proDimensionWidthDP;
                        nProd.proDimensionDiameter = nProd.proDimensionDiameterDP;
                        nProd.proCoverPrice = nProd.proCoverPriceDP;
                        nProd.proSuggestedPrice = nProd.proSuggestedPriceDP;
                        nProd.proRCR = nProd.proRCRDP;
                        nProd.proLoadIndex = nProd.proLoadIndexDP;
                        nProd.proSpeed = nProd.proSpeedDP;
                        nProd.proProductCode = nProd.proProductCodeDP;
                        nProd.proFuel = nProd.proFuelDP;
                        nProd.proWet = nProd.proWetDP;
                        nProd.proNoise = nProd.proNoiseDP;
                        nProd.proTyreSize = nProd.proTyreSizeDP;
                        nProd.proEdited = false;
                        nProd.proStatus = true;
                        nProd.proImage = "";
                    }

                    nProd.proInventory = inventory;
                    count++;
                }
                catch (Exception ex)
                {
                    error += ex.ToString() + " | ";
                }
            }
            db.tokens.FirstOrDefault().tokStockLastUpdate = DateTime.Now;
            db.SubmitChanges();

            count = count - nCount;

            return count.ToString() + "|" + nCount.ToString();
        }

        public static void updateProductsByCategory(List<string> categories, ref int updates, ref int news, string find = "")
        {
            dbDekkOnlineDataContext db = new dbDekkOnlineDataContext();
            string token = generateToken();
            string url = dekkProURL + "/GetProducts?";

            url += "request.timeStampFrom=" + lastUpdate.ToString("yyyy-MM-ddTHH:mm:ss") + "&";
            foreach (string cat in categories) url += "request.productCategoryIds=" + cat + "&";
            url = url.TrimEnd('&');
            HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create(url);
            myReq.Accept = "application/json";
            myReq.ContentType = "application/json";
            //myReq.Method = "POST";
            myReq.Headers["Authorization"] = "Bearer " + token;
            WebResponse wr = myReq.GetResponse();
            Stream receiveStream = wr.GetResponseStream();
            StreamReader reader = new StreamReader(receiveStream, Encoding.UTF8);
            //string content = reader.ReadToEnd();
            string json = reader.ReadToEnd();
            productsDP lis = JsonConvert.DeserializeObject<productsDP>(json);

            int count = 0;
            int nCount = 0;

            var cProds = (from pro in db.products
                          where categories.Contains(pro.categoriesDP.cdpId.ToString())
                          select pro).ToList();

            var cBrands = db.brands.ToList();

            string error = "";
            foreach (productDP pdp in lis.productDP)
            {
                try
                {

                    dkGeneralInformation dgi = pdp.GeneralInformation;
                    dkTireInformation dti = pdp.TireInformation;
                    dkCategory dca = pdp.Category;
                    dkBrand dbr = pdp.Brand;
                    dkPricing dbp = pdp.Pricing;
                    Guid proUUID = dgi.ProductId;

                    bool findProduct = false;
                    if (dti.Code == find) findProduct = true;
                    if (findProduct)
                        Console.Write(dgi.ProductId);
                    int inventory = 0;
                    try
                    {
                        string cdpIdCheck = dca.ProductCategoryId.ToUpper();
                        if (!categories.Contains(cdpIdCheck)) continue;
                    }
                    catch
                    {
                        continue;
                    }



                    try { inventory = pdp.Inventory.Sum(c => c.AvailableUnits); } catch { }



                    string fuel = "";
                    string wet = "";
                    string noise = "";
                    string speed = "";
                    try
                    {
                        var FWD = dti.Description.Split('-').ToList();
                        try { fuel = FWD[0].Replace(" ", "").Substring(FWD[0].IndexOf(':') + 1); } catch { }
                        try { wet = FWD[1].Replace(" ", ""); } catch { }
                        try { noise = FWD[2].Replace(" ", ""); } catch { }
                        try { speed = dti.Speed; } catch { }
                    }
                    catch
                    {

                    }
                    if (fuel.Length > 5)
                        fuel = fuel.Substring(0, 4);
                    if (wet.Length > 5)
                        wet = wet.Substring(0, 4);

                    if (noise.Length > 5)
                        noise = noise.Substring(0, 4);

                    //if (speed == "") continue;




                    product nProd = new product();
                    var cProd = cProds.Where(c => c.proUUID == proUUID);
                    if (cProd.FirstOrDefault() != null) nProd = cProd.FirstOrDefault();
                    else
                    {
                        nCount++;
                        db.products.InsertOnSubmit(nProd);
                    }

                    nProd.proUUID = dgi.ProductId;

                    Guid cdpId = Guid.Parse(dca.ProductCategoryId);

                    nProd.cdpId = cdpId;
                    Guid braId = Guid.Parse(dbr.BrandId);
                    if (cBrands.Where(c => c.braId == braId).Count() == 0)
                    {
                        brand nBra = new brand();
                        nBra.braId = Guid.Parse(dbr.BrandId);
                        nBra.braCode = dbr.Code;
                        nBra.braName = dbr.Name;
                        nBra.braNameDP = dbr.Name;
                        nBra.braCodeDP = dbr.Code;
                        nBra.barDescriptionDP = "";
                        nBra.braDescription = "";
                        nBra.braImage = "";
                        nBra.braEdited = false;
                        db.brands.InsertOnSubmit(nBra);
                        nProd.brand = nBra;
                    }
                    else
                        nProd.braId = braId;

                    try { nProd.proSkuId = Guid.Parse(dti.ProductSkuId); } catch { }
                    nProd.proSkuDP = dti.SkuCode;
                    if (nProd.proSkuDP == null) nProd.proSkuDP = "";
                    nProd.proNameDP = dgi.Name;
                    nProd.proDescriptionDP = dgi.Name;
                    nProd.proCodeDP = dti.Code;
                    try { nProd.proDimensionProfileDP = int.Parse(dti.Dimension.Profile.Replace(".00", "")); } catch { };
                    try { nProd.proDimensionWidthDP = int.Parse(dti.Dimension.Width.Replace(".00", "")); } catch { };
                    try { nProd.proDimensionDiameterDP = int.Parse(dti.Dimension.Diameter.Replace(".00", "")); } catch { };
                    nProd.proCoverPriceDP = dbp.CoverPrice;
                    nProd.proSuggestedPriceDP = dbp.SuggestedPrice;
                    nProd.proRCRDP = dti.RollerCircumReference;
                    nProd.proLoadIndexDP = dti.LoadIndex;
                    nProd.proSpeedDP = dti.Speed;
                    nProd.proProductCodeDP = dgi.ProductCode;
                    nProd.proFuelDP = fuel;
                    nProd.proWetDP = wet;
                    nProd.proNoiseDP = noise;
                    string tyreSize = nProd.proDimensionWidthDP.ToString() + nProd.proDimensionProfileDP.ToString() + nProd.proDimensionDiameterDP.ToString();
                    nProd.proLastUpdateDP = DateTime.Now;
                    if (tyreSize.Length > 10)
                        tyreSize = tyreSize.Substring(0, 9);

                    nProd.proTyreSizeDP = tyreSize;

                    if (nProd.proEdited != true)
                    {
                        nProd.proSku = nProd.proSkuDP;
                        nProd.proName = nProd.proNameDP;
                        nProd.proDescription = nProd.proDescriptionDP;
                        nProd.proCode = nProd.proCodeDP;
                        nProd.proDimensionProfile = nProd.proDimensionProfileDP;
                        nProd.proDimensionWidth = nProd.proDimensionWidthDP;
                        nProd.proDimensionDiameter = nProd.proDimensionDiameterDP;
                        nProd.proCoverPrice = nProd.proCoverPriceDP;
                        nProd.proSuggestedPrice = nProd.proSuggestedPriceDP;
                        nProd.proRCR = nProd.proRCRDP;
                        nProd.proLoadIndex = nProd.proLoadIndexDP;
                        nProd.proSpeed = nProd.proSpeedDP;
                        nProd.proProductCode = nProd.proProductCodeDP;
                        nProd.proFuel = nProd.proFuelDP;
                        nProd.proWet = nProd.proWetDP;
                        nProd.proNoise = nProd.proNoiseDP;
                        nProd.proTyreSize = nProd.proTyreSizeDP;
                        nProd.proEdited = false;
                        nProd.proStatus = true;
                        nProd.proImage = "";
                    }

                    nProd.proInventory = inventory;
                    count++;
                }
                catch (Exception ex)
                {
                    error += ex.ToString() + " | ";
                }
            }
            db.tokens.FirstOrDefault().tokStockLastUpdate = DateTime.Now;
            db.SubmitChanges();

            count = count - nCount;
            news = nCount;
            updates = count;

        }

        public static int updateProductsBySku(Guid skuId)
        {

            dbDekkOnlineDataContext db = new dbDekkOnlineDataContext();
            string token = generateToken();
            string url = dekkProURL +"/GetProducts?";

            url += "request.productSkuIds=" + skuId;

            HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create(url);
            myReq.Accept = "application/json";
            myReq.ContentType = "application/json";
            //myReq.Method = "POST";
            myReq.Headers["Authorization"] = "Bearer " + token;
            WebResponse wr = myReq.GetResponse();
            Stream receiveStream = wr.GetResponseStream();
            StreamReader reader = new StreamReader(receiveStream, Encoding.UTF8);
            //string content = reader.ReadToEnd();
            string json = reader.ReadToEnd();
            productsDP lis = JsonConvert.DeserializeObject<productsDP>(json);
            int inventory = 0;
            var pdp = lis.productDP.FirstOrDefault();

            var cBrands = db.brands.ToList();

            string error = "";

            try
            {

                dkGeneralInformation dgi = pdp.GeneralInformation;
                dkTireInformation dti = pdp.TireInformation;
                dkCategory dca = pdp.Category;
                dkBrand dbr = pdp.Brand;
                dkPricing dbp = pdp.Pricing;
                Guid proUUID = dgi.ProductId;



                try { inventory = pdp.Inventory.Sum(c => c.AvailableUnits); } catch { }



                string fuel = "";
                string wet = "";
                string noise = "";
                string speed = "";
                try
                {
                    var FWD = dti.Description.Split('-').ToList();
                    try { fuel = FWD[0].Replace(" ", "").Substring(FWD[0].IndexOf(':') + 1); } catch { }
                    try { wet = FWD[1].Replace(" ", ""); } catch { }
                    try { noise = FWD[2].Replace(" ", ""); } catch { }
                    try { speed = dti.Speed; } catch { }
                }
                catch
                {

                }
                if (fuel.Length > 5)
                    fuel = fuel.Substring(0, 4);
                if (wet.Length > 5)
                    wet = wet.Substring(0, 4);

                if (noise.Length > 5)
                    noise = noise.Substring(0, 4);

                //if (speed == "") continue;




                product nProd = new product();
                var cProd = db.products.Where(c => c.proSkuId == skuId);
                if (cProd.FirstOrDefault() != null) nProd = cProd.FirstOrDefault();


                nProd.proUUID = dgi.ProductId;

                Guid cdpId = Guid.Parse(dca.ProductCategoryId);

                nProd.cdpId = cdpId;
                Guid braId = Guid.Parse(dbr.BrandId);
                if (cBrands.Where(c => c.braId == braId).Count() == 0)
                {
                    brand nBra = new brand();
                    nBra.braId = Guid.Parse(dbr.BrandId);
                    nBra.braCode = dbr.Code;
                    nBra.braName = dbr.Name;
                    nBra.braNameDP = dbr.Name;
                    nBra.braCodeDP = dbr.Code;
                    nBra.barDescriptionDP = "";
                    nBra.braDescription = "";
                    nBra.braImage = "";
                    nBra.braEdited = false;
                    db.brands.InsertOnSubmit(nBra);
                    nProd.brand = nBra;
                }
                else
                    nProd.braId = braId;

                try { nProd.proSkuId = Guid.Parse(dti.ProductSkuId); } catch { }
                nProd.proSkuDP = dti.SkuCode;
                if (nProd.proSkuDP == null) nProd.proSkuDP = "";
                nProd.proNameDP = dgi.Name;
                nProd.proDescriptionDP = dgi.Name;
                nProd.proCodeDP = dti.Code;
                try { nProd.proDimensionProfileDP = int.Parse(dti.Dimension.Profile.Replace(".00", "")); } catch { };
                try { nProd.proDimensionWidthDP = int.Parse(dti.Dimension.Width.Replace(".00", "")); } catch { };
                try { nProd.proDimensionDiameterDP = int.Parse(dti.Dimension.Diameter.Replace(".00", "")); } catch { };
                nProd.proCoverPriceDP = dbp.CoverPrice;
                nProd.proSuggestedPriceDP = dbp.SuggestedPrice;
                nProd.proRCRDP = dti.RollerCircumReference;
                nProd.proLoadIndexDP = dti.LoadIndex;
                nProd.proSpeedDP = dti.Speed;
                nProd.proProductCodeDP = dgi.ProductCode;
                nProd.proFuelDP = fuel;
                nProd.proWetDP = wet;
                nProd.proNoiseDP = noise;
                string tyreSize = nProd.proDimensionWidthDP.ToString() + nProd.proDimensionProfileDP.ToString() + nProd.proDimensionDiameterDP.ToString();
                nProd.proLastUpdateDP = DateTime.Now;
                if (tyreSize.Length > 10)
                    tyreSize = tyreSize.Substring(0, 9);

                nProd.proTyreSizeDP = tyreSize;

                if (nProd.proEdited != true)
                {
                    nProd.proSku = nProd.proSkuDP;
                    nProd.proName = nProd.proNameDP;
                    nProd.proDescription = nProd.proDescriptionDP;
                    nProd.proCode = nProd.proCodeDP;
                    nProd.proDimensionProfile = nProd.proDimensionProfileDP;
                    nProd.proDimensionWidth = nProd.proDimensionWidthDP;
                    nProd.proDimensionDiameter = nProd.proDimensionDiameterDP;
                    nProd.proCoverPrice = nProd.proCoverPriceDP;
                    nProd.proSuggestedPrice = nProd.proSuggestedPriceDP;
                    nProd.proRCR = nProd.proRCRDP;
                    nProd.proLoadIndex = nProd.proLoadIndexDP;
                    nProd.proSpeed = nProd.proSpeedDP;
                    nProd.proProductCode = nProd.proProductCodeDP;
                    nProd.proFuel = nProd.proFuelDP;
                    nProd.proWet = nProd.proWetDP;
                    nProd.proNoise = nProd.proNoiseDP;
                    nProd.proTyreSize = nProd.proTyreSizeDP;
                    nProd.proEdited = false;
                    nProd.proStatus = true;
                    nProd.proImage = "";
                }

                nProd.proInventory = inventory;
            }
            catch (Exception ex)
            {
                error += ex.ToString() + " | ";
            }

            db.SubmitChanges();

            return inventory;
        }

        #endregion

        #region "Common Functions"

        public static string uploadSelectedImage(string sSavePath, UploadedFile myFile, string fileName, string mapPath, string objectId, bool multi = false, string multipleFolder = "")
        {

            string posted = "notPosted";

            long nFileLen = myFile.PostedFile.ContentLength;

            byte[] myData = new Byte[nFileLen];
            myFile.PostedFile.InputStream.Read(myData, 0, int.Parse(nFileLen.ToString()));

            string sFilename = System.IO.Path.GetFileName(myFile.FileName);
            int file_append = 0;
            string sFilenameF = "";

            if (multi) sFilenameF = multipleFolder + sFilename.Substring(fileName.LastIndexOf('.'));
            else sFilenameF = objectId + sFilename.Substring(fileName.LastIndexOf('.'));


            while (System.IO.File.Exists(mapPath + sSavePath + sFilenameF))
            {
                file_append++;
                sFilenameF = System.IO.Path.GetFileNameWithoutExtension(sFilenameF)
                                 + file_append.ToString() + sFilenameF.Substring(sFilenameF.LastIndexOf('.'));
            }

            System.IO.FileStream newFile
                    = new System.IO.FileStream(mapPath + sSavePath + sFilenameF,
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
                    FileInfo imageForDelete = new FileInfo(mapPath + sSavePath + sFilename);
                    imageForDelete.Delete();
                    sFilename = posted;
                }
            }
            posted = sSavePath + sFilenameF;

            return posted;
        }

        #endregion

        #region "Admin Pages"

        public static List<brand> getBrandList()
        {
            db = new dbDekkOnlineDataContext();
            return db.products.Where(c => c.categoriesDP.cdpStatus == true).Select(c => c.brand).Distinct().ToList();
        }

        public static List<ProductList> getProductsList(bool? status)
        {
            db = new dbDekkOnlineDataContext();

            var lstProducts = (from pro in db.products
                               let disc = pro.proDiscount.HasValue ? pro.proDiscount.Value : 0
                               where (status.HasValue == false || pro.proStatus == status.Value)
                               && pro.categoriesDP.cdpStatus == true
                               select new ProductList()
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
                               }).ToList();

            return lstProducts;
        }

        public static void changeStatusCategories(Guid cdpId)
        {

            var cCategorie = db.categoriesDPs.Where(c => c.cdpId == cdpId).FirstOrDefault();
            cCategorie.cdpStatus = !cCategorie.cdpStatus;
            cCategorie.cdpChangeStatus = DateTime.Now;
            db.SubmitChanges();
        }

        public static int updateProductStock(int proId)
        {
            Guid skuId = db.products.Where(c => c.proId == proId).FirstOrDefault().proSkuId;
            return updateProductsBySku(skuId);
        }

        #endregion
    }
    #endregion
}