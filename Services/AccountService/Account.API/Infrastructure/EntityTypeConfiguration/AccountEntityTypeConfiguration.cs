using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Account.API.Infrastructure.EntityTypeConfiguration
{
    public class AccountEntityTypeConfiguration : IEntityTypeConfiguration<Account.API.Model.Account>
    {
        public void Configure(EntityTypeBuilder<Account.API.Model.Account> builder)
        {
           // Additional Configuratio for Account Model
        
        }
    }
}
