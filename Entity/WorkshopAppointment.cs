//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Entity
{
    using System;
    using System.Collections.Generic;
    
    public partial class WorkshopAppointment
    {
        public int Id { get; set; }
        public Nullable<int> IdWorkshop { get; set; }
        public Nullable<int> IdAppointment { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public Nullable<System.TimeSpan> Time { get; set; }
        public string Comments { get; set; }
        public Nullable<int> DayAppointment { get; set; }
    }
}
