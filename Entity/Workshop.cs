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
    
    public partial class Workshop
    {
        public int IdWorkshop { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public Nullable<int> ZipCode { get; set; }
        public string Latitude { get; set; }
        public string Length { get; set; }
        public string WorkImage { get; set; }
        public Nullable<int> Average { get; set; }
        public Nullable<bool> Status { get; set; }
        public Nullable<System.DateTime> RegistrationDate { get; set; }
    }
}
