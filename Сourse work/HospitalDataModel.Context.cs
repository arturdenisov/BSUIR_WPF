//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DenisovArt_Kurs
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class ProjectEntitiesContext : DbContext
    {
        public ProjectEntitiesContext()
            : base("name=ProjectEntitiesContext")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<CatalogOfService> CatalogOfServices { get; set; }
        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<DocSpeciality> DocSpecialities { get; set; }
        public virtual DbSet<DocTimeTable> DocTimeTables { get; set; }
        public virtual DbSet<Doctor> Doctors { get; set; }
        public virtual DbSet<OrderedService> OrderedServices { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<Ticket> Tickets { get; set; }
        public virtual DbSet<VisitList> VisitLists { get; set; }
    }
}
