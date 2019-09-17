using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserManagement.API.API.Model;
using UserManagement.API.Infrastructure.EntityConfiguration;

namespace UserManagement.API.API.Infrastructure.DBContext
{
    public class UserManagementContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public UserManagementContext(DbContextOptions<UserManagementContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new UserEntityTypeConfiguration());
        }
    }

    public class UserManagementDesignFactory : IDesignTimeDbContextFactory<UserManagementContext>
    {
        public UserManagementContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<UserManagementContext>()
                .UseSqlServer("Server=.;Initial Catalog=ZipCoTest.Services.UserManagementDb;Integrated Security=true");
            return new UserManagementContext(optionsBuilder.Options);
        }
    }
}
