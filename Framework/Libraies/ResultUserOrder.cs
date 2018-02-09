using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Libraies
{
  public  class ResultUserOrder
    {
        public List<ResultOrderProductsUser> product { get; set; }
        public List<ResultDataUser> user { get; set; }

    }
}
