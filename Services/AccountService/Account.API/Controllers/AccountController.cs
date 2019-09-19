using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Account.API.IntegrationEvents;
using Account.API.IntegrationEvents.Events;
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
        private readonly IAccountIntegrationEventService _accountManagementIntegrationEventService;
        private readonly IEventBusSynchronizationService _eventBusSynchronizationService;

        public AccountController(IAccountService accountService,
            ILogger<AccountController> logger,
            IMapper mapper,
               IEventBusSynchronizationService eventBusSynchronizationService,
            IAccountIntegrationEventService accountIntegrationEventService)
        {
            this._accountService = accountService;
            this._mapper = mapper;
            this._logger = logger;
            this._accountManagementIntegrationEventService = accountIntegrationEventService;
            this._eventBusSynchronizationService = eventBusSynchronizationService;
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

                return this.Ok(accountList);

            }
            catch (Exception ex)
            {
                this._logger.LogError(" Error has occured while retrieving all account", ex.Message);
                return this.BadRequest();
            }
        }

        [HttpPost]
        [Route("account/{Id}/{loan}/accountType")]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> CreateAccountAsync(Guid Id, double loan, int accountType)
        {
            try
            {
                this._logger.LogInformation("Creating Account for User");

                if (loan > 1000.00)
                {
                    return this.BadRequest("The credit exceeds allowed amount.");
                }

                var checkUserSalaryPackage = new RequestToConfirmUserAccountLoandEvent(Id, loan, accountType);
                var trackingEventId = Guid.NewGuid();
                checkUserSalaryPackage.EventIdSynchronizationId = trackingEventId;

                this._eventBusSynchronizationService.EventSynchronizationList.Add(trackingEventId, new SynchronizationDetails { HasSynchronizationFinish = false });
                await this._accountManagementIntegrationEventService.SaveEventAndAccountContextChangesAsync(checkUserSalaryPackage);
                await this._accountManagementIntegrationEventService.PublishThroughEventBusAsync(checkUserSalaryPackage);

                // wait to synchronize all events 
                await this._eventBusSynchronizationService.CheckIFHasSynchronizationFinish(trackingEventId);

                // find the sychnronizaiton evnet
                var eventObject = this._eventBusSynchronizationService.EventSynchronizationList.Where(x => x.Key.Equals(trackingEventId)).FirstOrDefault().Value;

                if (eventObject.HttpStatusCode.Equals(HttpStatusCode.NotFound))
                {
                    return this.BadRequest(eventObject.Message);
                }
                else if(eventObject.HttpStatusCode.Equals(HttpStatusCode.Created))
                {
                    return this.CreatedAtAction(null, eventObject.NewCreatedAccountId.ToString());
                }       
                else if(eventObject.HttpStatusCode.Equals(HttpStatusCode.BadRequest))
                {
                    return this.BadRequest( eventObject.Message);
                }
                else
                {
                    return this.BadRequest();
                }

            }
            catch (Exception ex)
            {
                this._logger.LogError(" Error has occured while retrieving all account", ex.Message);
                return this.BadRequest();
            }
        }
    }
}