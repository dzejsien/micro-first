using Microsoft.EntityFrameworkCore;
using Pricing.Domain;
using System;
using System.Linq;

namespace Pricing.DataAccess.PostgreSql
{
    // >dotnet ef migration add testMigration in AspNet5MultipleProject
    public class DomainModelPostgreSqlContext : DbContext
    {
        public DomainModelPostgreSqlContext(DbContextOptions<DomainModelPostgreSqlContext> options) : base(options)
        {
        }

        public DbSet<Price> Prices { get; set; }

        public DbSet<Tarrif> Tarrifs { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Price>().HasKey(m => m.PriceId);
            builder.Entity<Tarrif>().HasKey(m => m.TarrifId);

            // shadow properties
            builder.Entity<Price>().Property<DateTime>("UpdatedTimestamp");
            builder.Entity<Tarrif>().Property<DateTime>("UpdatedTimestamp");

            base.OnModelCreating(builder);
        }

        public override int SaveChanges()
        {
            ChangeTracker.DetectChanges();

            updateUpdatedProperty<Price>();
            updateUpdatedProperty<Tarrif>();

            return base.SaveChanges();
        }

        private void updateUpdatedProperty<T>() where T : class
        {
            var modifiedSourceInfo =
                ChangeTracker.Entries<T>()
                    .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

            foreach (var entry in modifiedSourceInfo)
            {
                entry.Property("UpdatedTimestamp").CurrentValue = DateTime.UtcNow;
            }
        }
    }
}
