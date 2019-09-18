using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserManagement.API.API.Controllers;
using UserManagement.API.API.Infrastructure.DBContexts;
using UserManagement.API.API.Model;
using UserManagement.API.IntegrationEvents;
using UserManagement.API.ViewModel;

namespace UserManagement.API.Services
{
    public class UserManagementService : IUserManagementService
    {
        private readonly UserManagementContext _userContext;
        private readonly IMapper _autoMapper;

        public UserManagementService(
            UserManagementContext userContext,          
            IMapper autoMapper)
        {
            this._userContext = userContext ?? throw new ArgumentNullException(nameof(userContext));          
            this._autoMapper = autoMapper;
          
            // Do not track entity changes
            this._userContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        public async Task<Guid> CreateUserAsync(CreateUserViewModel userViewModel)
        {
            User user = this._autoMapper.Map<User>(userViewModel);

            this._userContext.Add(user);
            await this._userContext.SaveChangesAsync();
            return user.Id;
        }

        public async Task<List<UserViewModel>> GetAllUsersAsync()
        {
            List<User> userList = await this._userContext.Users.ToListAsync();
            List<UserViewModel> userViewModelList = this._autoMapper.Map<List<UserViewModel>>(userList);
            return userViewModelList; 
        }

        public async Task<UserViewModel> GetUserByIdAsync(Guid id)
        {
            var user = await this._userContext.Users.SingleOrDefaultAsync(u => u.Id.Equals(id));

            UserViewModel userViewModel = this._autoMapper.Map<UserViewModel>(user);

            return userViewModel;
        }
    }
}
