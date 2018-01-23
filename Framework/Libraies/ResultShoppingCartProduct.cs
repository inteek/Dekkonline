using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Framework.Libraies
{
   public class ResultShoppingCartProduct
    {

        public string Image { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string IdUser { get; set; }
        public Nullable<int> proId { get; set; }
        public Nullable<int> quantity { get; set; }
        public Nullable<decimal> totalpriceprod { get; set; }

    }
}
