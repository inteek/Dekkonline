using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Libraies
{
    public class ResultOrders
    {
        public int IdOrderDetail { get; set; }
        public string IdUser { get; set; }
        public string Products { get; set; }
        public decimal TotalPrice { get; set; }
        public string Paymentmethod { get; set; }
        public Nullable<System.DateTime> OrderDate { get; set; }
        public Nullable<bool> Orderstatus { get; set; }
        public Nullable<int> IdDelivery { get; set; }
        public Nullable<System.DateTime> DeliveredDate { get; set; }
        public string UsedPromo { get; set; }
        public string Comments { get; set; }
        public int Total { get; set; }
        public Nullable<System.DateTime> datesale { get; set; }
        public Nullable<System.DateTime> dateest { get; set; }
        public string datesale2 { get; set; }
        public string dateest2 { get; set; }

    }
}
