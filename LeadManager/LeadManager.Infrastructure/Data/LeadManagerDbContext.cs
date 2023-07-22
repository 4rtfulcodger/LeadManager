using LeadManager.Core.Entities;
using LeadManager.Core.Entities.Source;
using LeadManager.Core.Entities.Supplier;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeadManager.Infrastructure.Data
{
    public class LeadManagerDbContext : IdentityDbContext<User>
    {        
        public DbSet<Source> Sources { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }

        public DbSet<Lead> Leads { get; set; }

        public DbSet<Contact> Contacts { get; set; }

        public DbSet<PhoneNumber> PhoneNumber { get; set; }

        public DbSet<Address> Address { get; set; }


        public LeadManagerDbContext(DbContextOptions<LeadManagerDbContext> options) : base(options)
        {         
            //Below can be used in read only scenarios to improve performance
            //ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Source>().HasData(
            //       new Source("Source1", "Source1 description") { SourceId = 1 },
            //       new Source("Source2", "Source2 description") { SourceId = 2 },
            //       new Source("Source3", "Source3 description") { SourceId = 3 }
            //   );

            //modelBuilder.Entity<Supplier>().HasData(
            //       new Supplier("Supplier1", "Supplier1 description") { SupplierId = 1 },
            //       new Supplier("Supplier2", "Supplier2 description") { SupplierId = 2 },
            //       new Supplier("Supplier3", "Supplier3 description") { SupplierId = 3 }
            //   );

            //modelBuilder.Entity<Lead>().HasData(
            //       new Lead(1,1, "Lead1", "Lead1 with Source1 and Supplier1") { LeadId = 1},
            //       new Lead(2,2, "Lead2", "Lead2 with Source2 and Supplier2") { LeadId = 2 },
            //       new Lead(3,3, "Lead3", "Lead3 with Source3 and Supplier3") { LeadId = 3 },
            //       new Lead(1, 1, "Lead4", "Lead4 with Source1 and Supplier1") { LeadId = 4},
            //       new Lead(1, 1, "Lead5", "Lead5 with Source1 and Supplier1") { LeadId = 5 }
            //   );



            //base.OnModelCreating is required for IdentityDbContext
            base.OnModelCreating(modelBuilder);

           

        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer(
        //        "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=LeadManager;Integrated Security=True;");
        //}
    }
}
