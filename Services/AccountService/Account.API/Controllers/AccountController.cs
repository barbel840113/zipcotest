using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Account.API.Services;
using Account.API.ViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Account.API.Controllers
{
    [Route("api/[controller]")]    
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly ILogger<AccountController> _logger;
        private readonly IMapper _mapper;

        public AccountController(IAccountService accountService,
            ILogger<AccountController> logger,
            IMapper mapper)
        {
            this._accountService = accountService;
            this._mapper = mapper;
            this._logger = logger;
        }

        [HttpGet]
        [Route("accounts")]
        [ProducesResponseType(typeof(List<AccountViewModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> GetUsersAsync()
        {
            try
            {
                this._logger.LogInformation(" Retrieving All Account from the Databaqse");
                List<Account.API.Model.Account> accountList = await this._accountService.ListAccountsAsync();
                
                var resultList = this._mapper.Map<AccountViewModel>(accountList);

                return this.Ok(resultList);

            }
            catch (Exception ex)
            {
                this._logger.LogError(" Error has occured while retrieving all account", ex.Message);
                return this.BadRequest();
            }
        }

        [HttpPost]
        [Route("account")]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> CreateAccountAsync(Guid Id, double loan)
        {
            try
            {
                this._logger.LogInformation("Creating Account for User");

                if(loan > 1000.00)
                {
                    return this.BadRequest("The credit exceeds allowed amount.");
                }

                var accountId = await this._accountService.CreateAccountForAsync(Id, loan);

                return this.CreatedAtAction(null, accountId.ToString());

            }
            catch (Exception ex)
            {
                this._logger.LogError(" Error has occured while retrieving all account", ex.Message);
                return this.BadRequest();
            }
        }
    }
}