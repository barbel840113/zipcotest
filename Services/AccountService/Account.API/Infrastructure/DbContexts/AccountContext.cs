using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Account.API.Infrastructure.EntityTypeConfiguration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Account.API.Infrastructure.DbContexts
{
    public class AccountContext : DbContext
    {
        public DbSet<Account.API.Model.Account> Accounts { get; set; }

        public AccountContext(DbContextOptions<AccountContext> dbContextOptions) : base(dbContextOptions)
        {}

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new AccountEntityTypeConfiguration());
        }
    }

    public class AccountDesignFactory : IDesignTimeDbContextFactory<AccountContext>
    {
        public AccountContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AccountContext>()
                .UseSqlServer("Server=.;Initial Catalog=ZipCoTest.Services.AccountDB;Integrated Security=true");
            return new AccountContext(optionsBuilder.Options);
        }
    }
}
