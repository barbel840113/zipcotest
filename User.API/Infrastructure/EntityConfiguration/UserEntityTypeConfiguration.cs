using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserManagement.API.API.Model;

namespace UserManagement.API.Infrastructure.EntityConfiguration
{
    public class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("User");
            builder.HasKey(entity => entity.Id);

            // Setup Email Address as required
            builder.Property(entity => entity.EmailAddress).IsRequired(required: true);

            // Setup Unique Index for Email Address
            builder.HasIndex(entity => entity.EmailAddress)
                    .IsUnique(true);
        }
    }
}
