﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace API2PSMaster.EF
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class AdaAccEntities : DbContext
    {
        public AdaAccEntities()
            : base("name=AdaAccEntities")
        {
        }
        public AdaAccEntities(string ptConnStr) : base(ptConnStr)
        {
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<TCNTPdtTnfDT> TCNTPdtTnfDTs { get; set; }
        public virtual DbSet<TCNTPdtTnfDTSrn> TCNTPdtTnfDTSrns { get; set; }
        public virtual DbSet<TCNTPdtTnfHD> TCNTPdtTnfHDs { get; set; }
        public virtual DbSet<TCNTPdtTnfHDRef> TCNTPdtTnfHDRefs { get; set; }
    }
}
