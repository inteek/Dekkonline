using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Libraies
{
    public class ResultProduct
    {
        public int proId { get; set; }
        public Nullable<System.Guid> proUUID { get; set; }
        public int braId { get; set; }
        public System.Guid cdpId { get; set; }
        public System.Guid proSkuId { get; set; }
        public string proSkuDP { get; set; }
        public string proNameDP { get; set; }
        public string proDescriptionDP { get; set; }
        public string proCodeDP { get; set; }
        public Nullable<int> proDimensionProfileDP { get; set; }
        public Nullable<int> proDimensionWidthDP { get; set; }
        public Nullable<int> proDimensionDiameterDP { get; set; }
        public Nullable<decimal> proCoverPriceDP { get; set; }
        public Nullable<decimal> proSuggestedPriceDP { get; set; }
        public Nullable<int> proRCRDP { get; set; }
        public string proLoadIndexDP { get; set; }
        public string proSpeedDP { get; set; }
        public string proProductCodeDP { get; set; }
        public string proFuelDP { get; set; }
        public string proWetDP { get; set; }
        public string proNoiseDP { get; set; }
        public string proTyreSizeDP { get; set; }
        public string proSku { get; set; }
        public string proName { get; set; }
        public string proDescription { get; set; }
        public string proCode { get; set; }
        public Nullable<int> proDimensionProfile { get; set; }
        public Nullable<int> proDimensionWidth { get; set; }
        public Nullable<int> proDimensionDiameter { get; set; }
        public Nullable<decimal> proCoverPrice { get; set; }
        public Nullable<decimal> proSuggestedPrice { get; set; }
        public Nullable<int> proInventory { get; set; }
        public Nullable<int> proRCR { get; set; }
        public string proLoadIndex { get; set; }
        public string proSpeed { get; set; }
        public string proProductCode { get; set; }
        public string proFuel { get; set; }
        public string proWet { get; set; }
        public string proNoise { get; set; }
        public string proTyreSize { get; set; }
        public bool proEdited { get; set; }
        public string proImage { get; set; }
        public bool proStatus { get; set; }
        public Nullable<int> catId { get; set; }
        public System.DateTime proLastUpdateDP { get; set; }
        public Nullable<System.DateTime> proLastUpdate { get; set; }
        public Nullable<decimal> proDiscount { get; set; }
    }
}
