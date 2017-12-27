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
using System.Xml;
using System.Xml.Linq;
using System.Web.Script.Serialization;
using Newtonsoft.Json;

namespace DekkOnline.engine
{

    class dkTireInformation
    {
        [JsonProperty("ProductSkuId")]
        public string ProductSkuId { get; set; }
        [JsonProperty("SkuCode")]
        public string SkuCode { get; set; }
        [JsonProperty("CoverPrice")]
        public double CoverPrice { get; set; }
        [JsonProperty("Description")]
        public string Description { get; set; }
        [JsonProperty("VendorId")]
        public string VendorId { get; set; }
        [JsonProperty("TempCreatedByStoreId")]
        public string TempCreatedByStoreId { get; set; }
        [JsonProperty("RollerCircumReference")]
        public int RollerCircumReference { get; set; }
        [JsonProperty("ImageFileInstanceId")]
        public string ImageFileInstanceId { get; set; }
        [JsonProperty("LoadIndex")]
        public string LoadIndex { get; set; }
        [JsonProperty("Speed")]
        public string Speed { get; set; }
        [JsonProperty("SuggestedPrice")]
        public double SuggestedPrice { get; set; }
        [JsonProperty("Code")]
        public string Code { get; set; }
        [JsonProperty("BackupCoverPrice")]
        public double BackupCoverPrice { get; set; }
        [JsonProperty("Dimension")]
        public dkDimensions Dimension { get; set; }
        [JsonProperty("TimeStamp")]
        public string TimeStamp { get; set; }

    }

    class dkDimensions
    {
        [JsonProperty("DimensionId")]
        public Guid DimensionId { get; set; }
        [JsonProperty("Profile")]
        public string Profile { get; set; }
        [JsonProperty("Width")]
        public string Width { get; set; }
        [JsonProperty("Diameter")]
        public string Diameter { get; set; }
        [JsonProperty("TimeStamp")]
        public string TimeStamp { get; set; }
    }

    class dkGeneralInformation
    {
        [JsonProperty("ProductId")]
        public Guid ProductId { get; set; }
        [JsonProperty("Name")]
        public string Name { get; set; }
        [JsonProperty("Description")]
        public string Description { get; set; }
        [JsonProperty("ProductCode")]
        public string ProductCode { get; set; }
        
    }

    class dkBrand
    {
        [JsonProperty("BrandId")]
        public string BrandId { get; set; }
        [JsonProperty("Name")]
        public string Name { get; set; }
        [JsonProperty("Description")]
        public string Description { get; set; }
        [JsonProperty("Code")]
        public string Code { get; set; }
        
    }

    class dkCategory
    {
        [JsonProperty("ProductCategoryId")]
        public string ProductCategoryId { get; set; }
        [JsonProperty("Name")]
        public string Name { get; set; }
        [JsonProperty("Description")]
        public string Description { get; set; }
        [JsonProperty("ProductCategoryParentId")]
        public string ProductCategoryParentId { get; set; }
    }

    class dkPricing
    {
        [JsonProperty("CoverPrice")]
        public decimal? CoverPrice { get; set; }
        [JsonProperty("SuggestedPrice")]
        public decimal? SuggestedPrice { get; set; }
    }

    class dkInventory
    {
        [JsonProperty("AvailableUnits")]
        public int AvailableUnits { get; set; }
    }


    class productDP
    {
        [JsonProperty("TireInformation")]
        public dkTireInformation TireInformation { get; set;}
        [JsonProperty("GeneralInformation")]
        public dkGeneralInformation GeneralInformation { get; set; }
        [JsonProperty("Brand")]
        public dkBrand Brand { get; set; }
        [JsonProperty("Category")]
        public dkCategory Category { get; set; }
        [JsonProperty("Pricing")]
        public dkPricing Pricing { get; set; }
        [JsonProperty("Inventory")]
        public List<dkInventory> Inventory { get; set; }
        
    }

    class productsDP
    {
        [JsonProperty("Products")]
        public List<productDP> productDP { get; set; }
    }

    class dkCategoriesDP
    {
        [JsonProperty("ProductCategories")]
        public List<dkCategory> CategoriesDekk { get; set; }
    }

    class dkBrandsDP
    {
        [JsonProperty("ProductBrands")]
        public List<dkBrand> BrandsDekk { get; set; }
    }

    public class BrandDekk
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }

    }

    public class CategoriesDekk
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid? ParentId { get; set; }
        public string ParentName { get; set; }
        public bool Status { get; set; }
        public int Number { get; set; }

    }

    public class dekkpro
    {
        static string generateToken()
        {
            dbDekkOnlineDataContext db = new dbDekkOnlineDataContext();
            DateTime now = DateTime.Now.Date;

            var cToken = new token();
            var checkTokens = (from tok in db.tokens where tok.tokDate >= now orderby tok.tokId descending select tok);
            string token = "";
            if (checkTokens.Count() > 0)
            {
                cToken = checkTokens.FirstOrDefault();
                token = cToken.tokData;
            }
            else
            {
                cToken.tokDate = now;
                string urlToken = "https://testapi.dekkpro.no/Token";
                HttpWebRequest myRT = (HttpWebRequest)WebRequest.Create(urlToken);
                myRT.ContentType = "application/x-www-form-urlencoded";

                //var keyValues = new List<KeyValuePair<string, string>>();
                //keyValues.Add(new KeyValuePair<string, string>("grant_type", "password"));
                //keyValues.Add(new KeyValuePair<string, string>("username", "ck@dekkskift.no"));
                //keyValues.Add(new KeyValuePair<string, string>("password", "cY^#ncT6"));

                var postData = "grant_type=password";
                postData += "&username=ck@dekkskift.no";
                postData += "&password=cY^#ncT6";
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
                db.tokens.InsertOnSubmit(cToken);
                db.SubmitChanges();
            }

            return token;
        }

        static string newToken()
        {
            dbDekkOnlineDataContext db = new dbDekkOnlineDataContext();
            DateTime now = DateTime.Now.Date;

            var cToken = new token();
            var checkTokens = (from tok in db.tokens where tok.tokDate >= now select tok);
            string token = "";

            cToken.tokDate = now;
            string urlToken = "https://testapi.dekkpro.no/Token";
            HttpWebRequest myRT = (HttpWebRequest)WebRequest.Create(urlToken);
            myRT.ContentType = "application/x-www-form-urlencoded";

        
            var postData = "grant_type=password";
            postData += "&username=ck@dekkskift.no";
            postData += "&password=cY^#ncT6";
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
            db.tokens.InsertOnSubmit(cToken);
            db.SubmitChanges();

            return token;

        }

        public static string updateCategories(bool overwrite)
        {
            dbDekkOnlineDataContext db = new dbDekkOnlineDataContext();
            string token = generateToken();
            string url = "https://testapi.dekkpro.no//api/DekkproIntegrationService/GetProductCategories";
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
            int count = 0;
            int nCount = 0;
            foreach (var cat in lis.CategoriesDekk)
            {
                var nCat = new categoriesDP();
                Guid cdpId = Guid.Parse(cat.ProductCategoryId);
                var cCat = db.categoriesDPs.Where(c => c.cdpId == cdpId);

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

            db.SubmitChanges();
            count = count - nCount;
            return count.ToString() + "|" + nCount.ToString();
        }

        public static string updateBrands(bool overwrite)
        {
            dbDekkOnlineDataContext db = new dbDekkOnlineDataContext();
            string token = generateToken();
            string url = "https://testapi.dekkpro.no//api/DekkproIntegrationService/GetProductBrands ";
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

            int count = 0;
            int nCount = 0;
            foreach (var bra in lis.BrandsDekk)
            {

                Guid braId = Guid.Parse(bra.BrandId);
                var cBra = db.brands.Where(c => c.braId == braId);
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
            db.SubmitChanges();

            count = count - nCount;
            return count.ToString() + "|" + nCount.ToString();

        }

        public static string updateProducts(bool overwrite)
        {
            dbDekkOnlineDataContext db = new dbDekkOnlineDataContext();
            string token = generateToken();
            string url = "https://testapi.dekkpro.no//api/DekkproIntegrationService/GetProducts";
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
                    if(fuel.Length > 5)
                        fuel = fuel.Substring(0, 4);
                    if (wet.Length > 5)
                        wet = wet.Substring(0, 4);

                    if (noise.Length > 5)
                        noise = noise.Substring(0, 4);
                    
                    //if (speed == "") continue;

                    Guid proUUID = dgi.ProductId;

                    var cProd = (from pro in db.products
                                 where pro.proUUID == proUUID
                                 select pro);

                    product nProd = new product();
                    if (cProd.FirstOrDefault() != null) nProd = cProd.FirstOrDefault();
                    else
                    {
                        nCount++;
                        db.products.InsertOnSubmit(nProd);
                    }

                    nProd.proUUID = dgi.ProductId;

                    Guid cdpId = Guid.Parse(dca.ProductCategoryId);

                    nProd.cdpId = cdpId;
                    if (db.categoriesDPs.Where(c => c.cdpId == cdpId).Count() == 0)
                        continue;

                    Guid braId = Guid.Parse(dbr.BrandId);
                    if (db.brands.Where(c => c.braId == braId).Count() == 0)
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
                    string tyreSize = nProd.proDimensionWidthDP.ToString() + nProd.proDimensionProfileDP.ToString() +  nProd.proDimensionDiameterDP.ToString();
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
            db.SubmitChanges();

            count = count - nCount;
            return count.ToString() + "|" + nCount.ToString();
        }

        public static string updateData(bool overwrite)
        {
            string cate = updateCategories(false);
            string brand = updateBrands(false);
            string prod = updateProducts(false);

            dbDekkOnlineDataContext db = new dbDekkOnlineDataContext();
            var dCategories = (from cat in db.categoriesDPs
                               where cat.cdpEdited == false && cat.products.Count() == 0
                               select cat).ToList();

            var dBrands = (from bra in db.brands
                           where bra.braEdited == false && bra.products.Count() == 0
                           select bra).ToList();

            db.categoriesDPs.DeleteAllOnSubmit(dCategories);
            db.brands.DeleteAllOnSubmit(dBrands);
            db.SubmitChanges();

            return prod;
        }

    }
}