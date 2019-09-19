using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using UserManagement.API.ViewModel;
using Xunit;

namespace UserManagement.API.FunctionalTest
{
    public class UserManagementControllerTest : UserManagementScenariosBase
    {

        [Fact]
        public async Task Get_All_Users_And_Response_Ok_Status_Code()
        {
            using(var server = base.CreateTestServer())
            {
                var response = await server.CreateClient().GetAsync("api/usermanagement/allusers");

                response.EnsureSuccessStatusCode();
            }
        }

        [Fact]
        public async Task Get_User_By_Innvalid_ID_And_Response_Bad_RequestCode()
        {
            using (var server = base.CreateTestServer())
            {
                var response = await server.CreateClient().GetAsync("api/usermanagement/getuser/0");

                Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            }
        }

        [Fact]
        public async Task Get_User_By_ID_Who_DoesntExists_And_Response_NotFound()
        {
            using (var server = base.CreateTestServer())
            {
                var response = await server.CreateClient().GetAsync($"api/usermanagement/getuser/{Guid.Empty}");

                Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            }
        }

        [Fact]
        public async Task Create_User_And_Response_Ok_Status_Code()
        {
            using (var server = base.CreateTestServer())
            {              
                var response = await server.CreateClient().PostAsJsonAsync($"api/usermanagement/user", new CreateUserViewModel {
                    EmailAddress = $"test{Guid.NewGuid()}@email.com",
                    MonthlyExpenses = 300,
                    MonthlySalary = 3456.56,
                    Name = "Test User"
                });

                Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            }
        }

        [Fact]
        public async Task Create_DuplicateUser_And_Response_BadRequest_Status_Code()
        {
            var guid = Guid.NewGuid();

            using (var server = base.CreateTestServer())
            {
                var response1 = await server.CreateClient().PostAsJsonAsync($"api/usermanagement/user", new CreateUserViewModel
                {
                    EmailAddress = $"test{guid}@email.com",
                    MonthlyExpenses = 300,
                    MonthlySalary = 3456.56,
                    Name = "Test User"
                });

                var response2 = await server.CreateClient().PostAsJsonAsync($"api/usermanagement/user", new CreateUserViewModel
                {
                    EmailAddress = $"test{guid}@email.com",
                    MonthlyExpenses = 300,
                    MonthlySalary = 3456.56,
                    Name = "Test User"
                });

                Assert.Equal(HttpStatusCode.Created, response1.StatusCode);
                Assert.Equal(HttpStatusCode.BadRequest, response2.StatusCode);
            }
        }

        [Fact]
        public async Task Create_User_And_Response_BadRequest_EmptyModel_Status_Code()
        {
            using (var server = base.CreateTestServer())
            {
                var response = await server.CreateClient().PostAsJsonAsync($"api/usermanagement/user", new CreateUserViewModel { });

                Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            }
        }
    }
}
