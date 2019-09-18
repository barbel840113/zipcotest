using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserManagement.API.ViewModel;

namespace UserManagement.API.Services
{
    public interface IUserManagementService
    {
        Task<List<UserViewModel>> GetAllUsersAsync();

        Task<UserViewModel> GetUserByIdAsync(Guid id);

        Task<Guid> CreateUserAsync(CreateUserViewModel userViewModel);
    }
}
