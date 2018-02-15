using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Libraies
{
  public  class ResultOrderProductsUser
    {
        public string Image { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string IdUser { get; set; }
        public Nullable<int> proId { get; set; }
        public Nullable<int> quantity { get; set; }
        public Nullable<double> totalpriceprod { get; set; }
        public string orders { get; set; }
        public string workshop { get; set; }
        public string duedate { get; set; }
        public Nullable<DateTime> orderdte1 { get; set; }
        public Nullable<DateTime> estimated1 { get; set; }
        public string orderdte2 { get; set; }
        public string estimated2 { get; set; }
        public Nullable<DateTime> Datedelivered1 { get; set; }
        public string Datedelivered2 { get; set; }
        public int idWorkShop { get; set; }
        public string Address { get; set; }
    }
}
