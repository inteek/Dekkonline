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
    
    public partial class ShoppingCart
    {
        public int Id { get; set; }
        public string IdUser { get; set; }
        public Nullable<int> proId { get; set; }
        public Nullable<int> quantity { get; set; }
        public Nullable<decimal> Price { get; set; }
        public Nullable<bool> Status { get; set; }
    
        public virtual products products { get; set; }
        public virtual AspNetUsers AspNetUsers { get; set; }
    }
}
