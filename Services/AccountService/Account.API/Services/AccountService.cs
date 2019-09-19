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

        public async Task<Guid> CreateAccountForAsync(Guid id, double loan, int accountType)
        {
            var account = new Account.API.Model.Account {
                AccountName = "Blank",
                UserId = id,
                Loan = (decimal)loan,
                AccountType = accountType
            };

            this._accountContext.Accounts.Add(account);
            await this._accountContext.SaveChangesAsync();

            return account.Id;
        }

        public async Task<List<Account.API.Model.Account>> ListAccountsAsync()
        {
            var accountList = await this._accountContext.Accounts.ToListAsync();
            return accountList;
        }
    }
}
