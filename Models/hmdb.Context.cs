﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Housemate.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class hmdbEntities : DbContext
    {
        public hmdbEntities()
            : base("name=hmdbEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Admin> Admins { get; set; }
        public virtual DbSet<AdminLogin> AdminLogins { get; set; }
        public virtual DbSet<BuyHistory> BuyHistories { get; set; }
        public virtual DbSet<BuyRecord> BuyRecords { get; set; }
        public virtual DbSet<Cart> Carts { get; set; }
        public virtual DbSet<CartRecord> CartRecords { get; set; }
        public virtual DbSet<Checkout> Checkouts { get; set; }
        public virtual DbSet<CustomerInfo> CustomerInfoes { get; set; }
        public virtual DbSet<CustomerLogin> CustomerLogins { get; set; }
        public virtual DbSet<OrderRecord> OrderRecords { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<PaymentMethod> PaymentMethods { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Registered> Registereds { get; set; }
        public virtual DbSet<Report> Reports { get; set; }
        public virtual DbSet<Review> Reviews { get; set; }
        public virtual DbSet<ServiceRequested> ServiceRequesteds { get; set; }
        public virtual DbSet<Service> Services { get; set; }
        public virtual DbSet<ShippingAddress> ShippingAddresses { get; set; }
        public virtual DbSet<Wishlist> Wishlists { get; set; }
        public virtual DbSet<WishlistRecord> WishlistRecords { get; set; }
    }
}
