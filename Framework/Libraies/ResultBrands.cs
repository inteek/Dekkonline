using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Libraies
{
    public class ResultBrands
    {
        public System.Guid braId { get; set; }
        public string braName { get; set; }
        public string braNameDP { get; set; }
        public string braCodeDP { get; set; }
        public string barDescriptionDP { get; set; }
        public string braDescription { get; set; }
        public string braCode { get; set; }
        public string braImage { get; set; }
        public bool braEdited { get; set; }
        public Nullable<int> braPercent { get; set; }
    }
}
