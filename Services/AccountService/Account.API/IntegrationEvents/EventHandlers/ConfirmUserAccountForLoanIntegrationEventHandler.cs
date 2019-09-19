using Account.API.Infrastructure.DbContexts;
using Account.API.IntegrationEvents.Events;
using Account.API.Services;
using EventBusLibrary.Interfaces;
using Microsoft.Extensions.Logging;
using Serilog.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Account.API.IntegrationEvents.EventHandlers
{
    public class ConfirmUserAccountForLoanIntegrationEventHandler :
        IIntegrationEventHandler<ConfirmUserAccountForLoanIntegrationEvent>
    {
        private readonly AccountContext _accountContext;
        private readonly IAccountIntegrationEventService _accountIntegrationEventService;
        private readonly ILogger<ConfirmUserAccountForLoanIntegrationEventHandler> _logger;
        private readonly IAccountService _accountService;
        private readonly IEventBusSynchronizationService _eventBusSynchronizationService;

        public ConfirmUserAccountForLoanIntegrationEventHandler(
            AccountContext accountContext,
            IAccountService accountService,
            IAccountIntegrationEventService userIntegrationEventService,
            IEventBusSynchronizationService eventBusSynchronizationService,
            ILogger<ConfirmUserAccountForLoanIntegrationEventHandler> logger)
        {
            _accountContext = accountContext;
            this._accountService = accountService;
            _accountIntegrationEventService = userIntegrationEventService;
            this._eventBusSynchronizationService = eventBusSynchronizationService;
            _logger = logger ?? throw new System.ArgumentNullException(nameof(logger));
        }

        public async Task Handle(ConfirmUserAccountForLoanIntegrationEvent @event)
        {
            using (LogContext.PushProperty("IntegrationEventContext", $"{@event.Id}-{Program.ProgramName}"))
            {
                _logger.LogInformation("----- Handling integration event: {IntegrationEventId} at {AppName} - ({@IntegrationEvent})", @event.Id, Program.ProgramName, @event);

                // create user
                try
                {
                   var accountId =  await this._accountService.CreateAccountForAsync(@event.UserId, @event.Loan, @event.AccountType);
                    var eventObject = this._eventBusSynchronizationService.EventSynchronizationList.Where(x => x.Key.Equals(@event.EventIdSynchronizationId)).FirstOrDefault().Value;
                    eventObject.HasSynchronizationFinish = true;
                    eventObject.Message = "Account has been created";
                    eventObject.NewCreatedAccountId = accountId;
                    eventObject.HttpStatusCode = System.Net.HttpStatusCode.Created;
                }
                catch
                {
                    await Task.Delay(1);
                    var eventObject = this._eventBusSynchronizationService.EventSynchronizationList.Where(x => x.Key.Equals(@event.EventIdSynchronizationId)).FirstOrDefault().Value;
                    eventObject.HasSynchronizationFinish = true;
                    eventObject.Message = "There was an error while creating account";                    
                    eventObject.HttpStatusCode = System.Net.HttpStatusCode.BadRequest;
                }               

            }
        }
    }
}
