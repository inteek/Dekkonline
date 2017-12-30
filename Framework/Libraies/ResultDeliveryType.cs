using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Libraies
{
    public class ResultDeliveryType
    {
        public int IdDelivery { get; set; }
        public Nullable<bool> DeliveryType1 { get; set; }
        public Nullable<int> IdUser { get; set; }
        public Nullable<int> IdWorkshop { get; set; }
        public Nullable<int> IdServiceWorkshop { get; set; }
        public Nullable<int> IdAppointmentsWorkshop { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public string Time { get; set; }
        public string Comments { get; set; }
    }
}
