using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Account.API.Infrastructure.DbContexts;
using Account.API.IntegrationEvents;
using Account.API.IntegrationEvents.Events;
using Account.API.Model;
using Microsoft.EntityFrameworkCore;

namespace Account.API.Services
{
    public class AccountService : IAccountService
    {
        private readonly AccountContext _accountContext;


        public AccountService(AccountContext accountContext)
        {
            this._accountContext = accountContext;          
        }

        public async Task<Guid> CreateAccountForAsync(Guid id, double loan)
        {
          

            return Guid.NewGuid();
        }

        public async Task<List<Account.API.Model.Account>> ListAccountsAsync()
        {
            var accountList = await this._accountContext.Accounts.ToListAsync();
            return accountList;
        }
    }
}
