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
using UserManagement.API.ViewModel;

namespace UserManagement.API.API.Controllers
{
    [Route("api/[controller]")]
    [AllowAnonymous]
    [ApiController]
    public class UserManagementController : ControllerBase
    {
        private readonly UserManagementContext _userContext;
        private readonly UserManagementOptions _settings;
        private readonly ILogger<UserManagementController> _logger;
        private readonly IUserManagementIntegrationEventService _usermanagementIntegrationEventService;
        private readonly IMapper _autoMapper;

        public UserManagementController(UserManagementContext userContext,
            IOptionsSnapshot<UserManagementOptions> settings,
            IMapper autoMapper,
            IUserManagementIntegrationEventService usermanagementIntegrationEventService)
        {
            this._userContext = userContext ?? throw new ArgumentNullException(nameof(userContext));
            this._usermanagementIntegrationEventService = usermanagementIntegrationEventService ?? throw new ArgumentNullException(nameof(usermanagementIntegrationEventService));
            _settings = settings.Value;
            this._autoMapper = autoMapper;
            // Do not track entity changes
            this._userContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            
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

                List<User> userList = await this._userContext.Users.ToListAsync();
                List<UserViewModel> userViewModelList  = this._autoMapper.Map<List<UserViewModel>>(userList);

                return this.Ok(userViewModelList);

            }catch(Exception ex)
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
                var user = await this._userContext.Users.SingleOrDefaultAsync(u => u.Id.Equals(id));

                if (user == null || user == default) {
                    return this.NotFound();
                }

                UserViewModel userViewModel = this._autoMapper.Map<UserViewModel>(user);

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
        public async Task<ActionResult> CreateUserAsync([FromBody] UserViewModel userViewModel)
        {
            try
            {
                if (!this.ModelState.IsValid)
                {
                    // This is handled by Middle Ware
                    return this.BadRequest();
                }

                this._logger.LogInformation($"Creating User");        
                User user = this._autoMapper.Map<User>(userViewModel);

                this._userContext.Add(user);
                await this._userContext.SaveChangesAsync();

                return this.Ok(userViewModel);
            }
            catch (Exception ex)
            {
                this._logger.LogError(" Error has occured while retrieving all users", ex.Message);
                return this.BadRequest();
            }
        }
    }
}
