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
    
    public partial class DeliveryType
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DeliveryType()
        {
            this.PurchaseOrder = new HashSet<PurchaseOrder>();
        }
    
        public int IdDelivery { get; set; }
        public Nullable<bool> DeliveryType1 { get; set; }
        public Nullable<int> IdUser { get; set; }
        public Nullable<int> IdWorkshop { get; set; }
        public Nullable<int> IdServiceWorkshop { get; set; }
        public Nullable<int> IdAppointmentsWorkshop { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public string Time { get; set; }
        public string Comments { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PurchaseOrder> PurchaseOrder { get; set; }
    }
}
