using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Libraies
{
   public class ResultAllCart
    {

        public List<ResultShoppingCartProduct> cart { get; set; }
        public decimal subtotal { get; set; }
        public string promocode { get; set; }
        public bool promocodeapp { get; set; }
        public int points { get; set; }
        public decimal total { get; set; }
    }
}
