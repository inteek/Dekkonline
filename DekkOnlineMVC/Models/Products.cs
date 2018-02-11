using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DekkOnlineMVC.Models
{
    public class Products
    {
        public int Id { get; set; }
        public string Image { get; set; }
        public int? CatId { get; set; }
        public string CategoryImage { get; set; }
        public string CategoryName { get; set; }
        public string Brand { get; set; }
        public string BrandImage { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? Width { get; set; }
        public int? Profile { get; set; }
        public int? Diameter { get; set; }
        public string TyreSize { get; set; }
        public string Fuel { get; set; }
        public string Wet { get; set; }
        public string Noise { get; set; }
        public decimal? Price { get; set; }
        public int? Stock { get; set; }
        public string SpeedIndex { get; set; }
        public string LoadIndex { get; set; }
    }
}

