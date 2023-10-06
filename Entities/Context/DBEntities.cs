using System.Linq;
using System.Web;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace ASIAssignment.Entities.Context
{
    public class DBEntities :DbContext//: IdentityDbContext<ApplicationUsers, ApplicationRoles, int>
    {
        public DBEntities(DbContextOptions options)
            : base(options)
        {
            //Options = options;
        }
        public virtual DbSet<Contact> Contact { get; set; }
        public virtual DbSet<Email> Email { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // This needs to go before the other rules!
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }
    }
}
