using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Pricing.DataAccess.PostgreSql
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<DomainModelPostgreSqlContext>
    {
        DomainModelPostgreSqlContext IDesignTimeDbContextFactory<DomainModelPostgreSqlContext>.CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<DomainModelPostgreSqlContext>();
            optionsBuilder.UseNpgsql("User ID=microdemo;Password=microdemo;Host=localhost;Port=5432;Database=MicroDemo_Pricing;Pooling=true");

            return new DomainModelPostgreSqlContext(optionsBuilder.Options);
        }
    }

}
