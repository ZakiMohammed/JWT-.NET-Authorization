using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using Woo.Model;

namespace Woo.Data
{
    public class WooContext : DbContext
    {
        public WooContext() : base("name=WooConnection")
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
