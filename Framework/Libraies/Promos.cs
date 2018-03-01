using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Framework.Libraies
{
    public class Promos
    {
        public string idUser { get; set; }
        public string PromoCode { get; set; }
        public bool? Used { get; set; }
        public string Date { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public int? Points { get; set; }

    }
}