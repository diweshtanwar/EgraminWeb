﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EgraminWeb.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class ApplicationWebEntities : DbContext
    {
        public ApplicationWebEntities()
            : base("name=ApplicationWebEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<tblDownloadDetail> tblDownloadDetails { get; set; }
        public virtual DbSet<tblEnquiry> tblEnquiries { get; set; }
        public virtual DbSet<tblGalleryCategory> tblGalleryCategories { get; set; }
        public virtual DbSet<tblLatestNew> tblLatestNews { get; set; }
        public virtual DbSet<tblMailConfiguration> tblMailConfigurations { get; set; }
        public virtual DbSet<tblNotification> tblNotifications { get; set; }
    }
}