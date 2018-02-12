using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Libraies
{
    public class ResultWorkshopAppointment
    {
        public int Id { get; set; }
        public int IdWorkshop { get; set; }
        public int IdAppointment { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; }
        public string Comments { get; set; }
        public string dateGet { get; set; }
    }
}
