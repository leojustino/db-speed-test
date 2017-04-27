using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpeedTest.Entities;

namespace SpeedTest.Drivers.SqlServerEntityFramework
{
    class SqlServerEntityContext : DbContext
    {
        public SqlServerEntityContext() : base("SqlServerSpeedTestConnection") { }

        protected override void OnModelCreating(DbModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Conventions.Remove<PluralizingTableNameConvention>();
            builder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            builder
                .Properties<string>()
                .Configure(a =>
                {
                    a.IsUnicode(false);
                    a.HasMaxLength(120);
                    a.IsVariableLength();
                });

            builder
                .Entity<Customer>()
                .HasMany(a => a.Orders)
                .WithMany()
                .Map(a =>
                {
                    a.ToTable("CustomerOrderAssociation");
                    a.MapLeftKey("CustomerId");
                    a.MapRightKey("OrderId");
                });
        }

        public DbSet<Basic> Basic { get; set; }
        public DbSet<Customer> Customer { get; set; }
    }
}
