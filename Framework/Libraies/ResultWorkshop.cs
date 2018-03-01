using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Libraies
{
    public class ResultWorkshop
    {
        public int IdWorkshop { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public Nullable<int> ZipCode { get; set; }
        public string Latitude { get; set; }
        public string Length { get; set; }
        public string WorkImage { get; set; }
        public string Email { get; set; }
        public int Average { get; set; }
    }
}
