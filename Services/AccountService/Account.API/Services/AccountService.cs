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
        private readonly IAccountManagementIntegrationEventService _accountManagementIntegrationEventService;

        public AccountService(AccountContext accountContext,
            IAccountManagementIntegrationEventService accountIntegrationEventService )
        {
            this._accountContext = accountContext;
            this._accountManagementIntegrationEventService = accountIntegrationEventService;
        }

        public async Task<Guid> CreateAccountForAsync(Guid id, double loan)
        {

            var checkUserSalaryPackage = new OrderStatusChangedToPaidIntegrationEvent(id, loan);

            await this._accountManagementIntegrationEventService.SaveEventAndAccountContextChangesAsync(checkUserSalaryPackage);
            await this._accountManagementIntegrationEventService.PublishThroughEventBusAsync(checkUserSalaryPackage);

            return Guid.NewGuid();
        }

        public async Task<List<Account.API.Model.Account>> ListAccountsAsync()
        {
            var accountList = await this._accountContext.Accounts.ToListAsync();
            return accountList;
        }
    }
}
