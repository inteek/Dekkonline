//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
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
    
        public virtual AspNetUsers AspNetUsers { get; set; }
        public virtual products products { get; set; }
    }
}
