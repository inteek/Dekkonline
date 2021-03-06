﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Libraies
{
    public class ResultPurchaseOrder
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
        public string ProductName { get; set; }
        public Nullable<decimal> Price { get; set; }
        public Nullable<int> Quantity { get; set; }
        public Nullable<decimal> TotalPrice1 { get; set; }
        public string ProductImage { get; set; }
        public string ShoppingCarts { get; set; }
    }
}
