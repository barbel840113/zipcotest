using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using UserManagement.API.API.Infrastructure.DBContext;
using UserManagement.API.API.Model;
using UserManagement.API.Infrastructure.Exceptions;
using UserManagement.API.Infrastructure.IntegrationEvents;
using UserManagement.API.Services;
using UserManagement.API.ViewModel;

namespace UserManagement.API.API.Controllers
{
    [Route("api/[controller]")]
    [AllowAnonymous]
    [ApiController]
    public class UserManagementController : ControllerBase
    {
      
        private readonly IUserManagementService _userManagementService;
        private readonly ILogger<UserManagementController> _logger;

        public UserManagementController(IUserManagementService userManagementService, ILogger<UserManagementController> logger)
        {
            this._userManagementService = userManagementService;
            this._logger = logger;
        }

        // GET api/values
        [HttpGet]
        [Route("allusers")]
        [ProducesResponseType(typeof(List<UserViewModel>),(int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> GetUsersAsync()
        {
            try
            {
                this._logger.LogInformation(" Retrieving All Users from the Databaqse");
                List<UserViewModel> userViewModelList = await this._userManagementService.GetAllUsersAsync();

                return this.Ok(userViewModelList);

            }
            catch (Exception ex)
            {
                this._logger.LogError(" Error has occured while retrieving all users",ex.Message);
                return this.BadRequest();
            }
        }    

        // GET api/values/5
        [HttpGet("getuser/{id}")]
        [ProducesResponseType(typeof(UserViewModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<UserViewModel>> GetUserByIdAsync(Guid? id)
        {
            if(!id.HasValue)
            {
                return this.BadRequest(new UserDomainException("User Id cannot be null"));
            }

            try
            {
                this._logger.LogInformation($"Retrieving Information User with {id.Value} from the Databaqse");

                var userViewModel = await this._userManagementService.GetUserByIdAsync(id.Value);

                if (userViewModel == null || 
                    userViewModel == default)
                {
                    return this.NotFound();
                }

                return this.Ok(userViewModel);

            }
            catch(Exception ex)
            {
                this._logger.LogError(" Error has occured while retrieving all users", ex.Message);
                return this.BadRequest();
            }         
        }

        [Route("user")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> CreateUserAsync([FromBody] CreateUserViewModel userViewModel)
        {
            try
            {
                if (!this.ModelState.IsValid)
                {
                    // This is handled by Middle Ware
                    return this.BadRequest();
                }

                this._logger.LogInformation($"Creating User");
                var userID = await this._userManagementService.CreateUserAsync(userViewModel);

                if(userID == default 
                  || userID == Guid.Empty)
                {
                    return this.BadRequest();
                }

                return this.Ok();
            }
            catch (Exception ex)
            {
                this._logger.LogError(" Error has occured while retrieving all users", ex.Message);
                return this.BadRequest();
            }
        }     
    }
}
