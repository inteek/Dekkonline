﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class dekkOnlineEntities : DbContext
    {
        public dekkOnlineEntities()
            : base("name=dekkOnlineEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<C__MigrationHistory> C__MigrationHistory { get; set; }
        public virtual DbSet<AspNetRoles> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaims> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogins> AspNetUserLogins { get; set; }
        public virtual DbSet<brands> brands { get; set; }
        public virtual DbSet<categories> categories { get; set; }
        public virtual DbSet<categoriesDP> categoriesDP { get; set; }
        public virtual DbSet<products> products { get; set; }
        public virtual DbSet<tokens> tokens { get; set; }
        public virtual DbSet<ShoppingCart> ShoppingCart { get; set; }
        public virtual DbSet<PromotionCode> PromotionCode { get; set; }
        public virtual DbSet<PromotionCodeUser> PromotionCodeUser { get; set; }
        public virtual DbSet<TypesServices> TypesServices { get; set; }
        public virtual DbSet<WorkshopAppointment> WorkshopAppointment { get; set; }
        public virtual DbSet<DetailUserPoints> DetailUserPoints { get; set; }
        public virtual DbSet<UserPoints> UserPoints { get; set; }
        public virtual DbSet<AspNetUsers> AspNetUsers { get; set; }
        public virtual DbSet<PurchaseOrder> PurchaseOrder { get; set; }
        public virtual DbSet<WorkshopServices> WorkshopServices { get; set; }
        public virtual DbSet<UserAddress> UserAddress { get; set; }
        public virtual DbSet<Appointments> Appointments { get; set; }
        public virtual DbSet<OrdersDetail> OrdersDetail { get; set; }
        public virtual DbSet<Payment> Payment { get; set; }
        public virtual DbSet<PromoCodeUsed> PromoCodeUsed { get; set; }
        public virtual DbSet<DeliveryType> DeliveryType { get; set; }
        public virtual DbSet<Orders> Orders { get; set; }
        public virtual DbSet<Workshop> Workshop { get; set; }
    }
}
