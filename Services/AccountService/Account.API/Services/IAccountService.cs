using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Account.API.Services
{
    public interface IAccountService
    {
        Task<Guid> CreateAccountForAsync(Guid id, double loan, int accountType);

        Task<List<Account.API.Model.Account>> ListAccountsAsync();
    }
}
