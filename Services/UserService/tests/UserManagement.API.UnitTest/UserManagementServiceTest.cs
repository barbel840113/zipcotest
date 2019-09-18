using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UserManagement.API.API.Infrastructure.DBContext;
using UserManagement.API.API.Model;
using UserManagement.API.Services;
using UserManagement.API.ViewModel;
using Xunit;

namespace UserManagement.API.UnitTest
{
    public class UserManagementServiceTest
    {
        private readonly IUserManagementService userManagementService;
        private readonly AutoMapper.IMapper mapper;

        public UserManagementServiceTest()
        {
            var dbContextOptionBuilder = new DbContextOptionsBuilder<UserManagementContext>().UseInMemoryDatabase(databaseName:"temp");
            var userContext = new UserManagementContext(dbContextOptionBuilder.Options);
            var mappingConfiguration = new MapperConfiguration(mc => {
                mc.CreateMap<List<User>, List<UserViewModel>>();
                mc.CreateMap<CreateUserViewModel, User>();
            });
            this.mapper = new AutoMapper.Mapper(mappingConfiguration);
            this.userManagementService = new UserManagementService(userContext, this.mapper);
        }

        [Fact]
        public async Task Test_CreateByUserAsync_Success()
        {
            var userModel = new CreateUserViewModel {
                EmailAddress = "test@email.com",
                Name = "Test User",
                MonthlyExpenses = 1456.45,
                MonthlySalary = 234.53
            };

            var userId = await this.userManagementService.CreateUserAsync(userModel);
            Assert.NotEqual(Guid.Empty, userId);
        }

        [Fact]
        public async Task Test_CreateByUserAsync_Error_Missing_EmailAddress()
        {
            var userModel = new CreateUserViewModel
            {             
                Name = "Test User",
                MonthlyExpenses = 1456.45,
                MonthlySalary = 234.53
            };

            var userId = await this.userManagementService.CreateUserAsync(userModel);
            Assert.Equal(Guid.Empty, userId);
        }

        [Fact]
        public async Task Test_CreateByUserAsync_Error_Duplicate_EmailAddresses()
        {
            var userModel1 = new CreateUserViewModel
            {
                EmailAddress = "test@user.com",
                Name = "Test User",
                MonthlyExpenses = 1456.45,
                MonthlySalary = 234.53
            };

            var userModel2 = new CreateUserViewModel
            {
                EmailAddress = "test@user.com",
                Name = "Test User",
                MonthlyExpenses = 1456.45,
                MonthlySalary = 234.53
            };

            var userId1 = await this.userManagementService.CreateUserAsync(userModel1);
            Assert.NotEqual(Guid.Empty, userId1);

            var userId2 = await this.userManagementService.CreateUserAsync(userModel2);
            Assert.Equal(Guid.Empty, userId2);
        }

    }
}
