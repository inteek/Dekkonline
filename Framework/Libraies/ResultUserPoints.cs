using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Libraies
{
    public class ResultUserPoints
    {
        public int Id { get; set; }
        public string IdUser { get; set; }
        public string UserFriendEmail { get; set; }
        public Nullable<System.DateTime> DayInvited { get; set; }
        public Nullable<bool> StatusofPromo { get; set; }
        public Nullable<int> PointsEarned { get; set; }
        public virtual AspNetUsers AspNetUsers { get; set; }

    }
}
