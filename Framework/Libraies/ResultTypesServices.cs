using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Libraies
{
    public class ResultTypesServices
    {
        public int IdService { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public Nullable<double> Price { get; set; }
        public int idWorkshop { get; set; }
        public string WorkshopName { get; set; }
        public string WorkshopImage { get; set; }
        public Nullable<double> TaxIva { get; set; }

    }
}
