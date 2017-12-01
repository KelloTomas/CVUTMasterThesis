﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ServerApp
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class CVUTdbEntities : DbContext
    {
        public CVUTdbEntities()
            : base("name=CVUTdbEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Automat> Automats { get; set; }
        public virtual DbSet<AutomatType> AutomatTypes { get; set; }
        public virtual DbSet<Card> Cards { get; set; }
        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<Desert> Deserts { get; set; }
        public virtual DbSet<Device> Devices { get; set; }
        public virtual DbSet<Meal> Meals { get; set; }
        public virtual DbSet<Menu> Menus { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Soup> Soups { get; set; }
    
        public virtual int CreateOrder(Nullable<int> clientId, Nullable<System.DateTime> forDate, Nullable<int> menuId)
        {
            var clientIdParameter = clientId.HasValue ?
                new ObjectParameter("ClientId", clientId) :
                new ObjectParameter("ClientId", typeof(int));
    
            var forDateParameter = forDate.HasValue ?
                new ObjectParameter("ForDate", forDate) :
                new ObjectParameter("ForDate", typeof(System.DateTime));
    
            var menuIdParameter = menuId.HasValue ?
                new ObjectParameter("MenuId", menuId) :
                new ObjectParameter("MenuId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("CreateOrder", clientIdParameter, forDateParameter, menuIdParameter);
        }
    
        public virtual int ServeOrder(Nullable<int> orderId)
        {
            var orderIdParameter = orderId.HasValue ?
                new ObjectParameter("OrderId", orderId) :
                new ObjectParameter("OrderId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("ServeOrder", orderIdParameter);
        }
    }
}
