using System;
using Xunit;
using Account.API.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;
using Account.API.Services;
using System.Threading.Tasks;

namespace Account.API.UnitTest
{
    public class AccountServiceTest
    {
        private readonly AccountContext _accountContext;

        public AccountServiceTest()
        {
            DbContextOptionsBuilder<AccountContext> dbContextOptionsBuilder = new DbContextOptionsBuilder<AccountContext>()
                .UseInMemoryDatabase(databaseName:"temp");
            this._accountContext = new AccountContext(dbContextOptionsBuilder.Options);
        }

        [Fact]
        public async Task Create_Account_Async_Test()
        {
            var accountService = new AccountService(this._accountContext);
            var result = await accountService.CreateAccountForAsync(new Guid(), 100, 2);
            Assert.NotEqual(Guid.Empty, result);
        }

        [Fact]
        public async Task GetAllAccounts_Async_Test()
        {
            var accountService = new AccountService(this._accountContext);
            var result = await accountService.CreateAccountForAsync(new Guid(), 100, 2);
            // create a user to test get 
            Assert.NotEqual(Guid.Empty, result);

            var findAccount = await accountService.ListAccountsAsync();
            Assert.Single(findAccount);
        }
    }
}
