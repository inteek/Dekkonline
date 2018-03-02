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
    public class dekkproObject
    {
        #region "Json"

        public class dkTireInformation
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

        public class dkDimensions
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

        public class dkGeneralInformation
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

        public class dkBrand
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

        public class dkCategory
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

        public class dkPricing
        {
            [JsonProperty("CoverPrice")]
            public decimal? CoverPrice { get; set; }
            [JsonProperty("SuggestedPrice")]
            public decimal? SuggestedPrice { get; set; }
        }

        public class dkInventory
        {
            [JsonProperty("AvailableUnits")]
            public int AvailableUnits { get; set; }
        }
        
        public class productDP
        {
            [JsonProperty("TireInformation")]
            public dkTireInformation TireInformation { get; set; }
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

        public class productsDP
        {
            [JsonProperty("Products")]
            public List<productDP> productDP { get; set; }
        }

        public class dkCategoriesDP
        {
            [JsonProperty("ProductCategories")]
            public List<dkCategory> CategoriesDekk { get; set; }
        }

        public class dkBrandsDP
        {
            [JsonProperty("ProductBrands")]
            public List<dkBrand> BrandsDekk { get; set; }
        }
        #endregion

        #region "Dekk Objects"

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

        public class ProductList
        {
            public int Id { get; set; }
            public string ImageLink { get; set; }
            public string Brand { get; set; }
            public string TyreSize { get; set; }
            public string Type { get; set; }
            public string LI { get; set; }
            public string SI { get; set; }
            public string Code { get; set; }
            public string OurCategory { get; set; }
            public string Category { get; set; }
            public string F { get; set; }
            public string W { get; set; }
            public string N { get; set; }
            public decimal? SuggestedPrice { get; set; }
            public int? Stock { get; set; }
        }
        #endregion


    }
}