using EventBusLibrary.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Account.API.IntegrationEvents.EventHandlers;
using Account.API.Infrastructure.DbContexts;
using Account.API.Services;
using Serilog.Context;

namespace Account.API.IntegrationEvents.EventHandlers
{
    public class RejectAccountLoanForUserEventHandler : IIntegrationEventHandler<RejectAccountLoanForUserEvent>
    {
        private readonly AccountContext _accountContext;
        private readonly IAccountIntegrationEventService _accountIntegrationEventService;
        private readonly ILogger<RejectAccountLoanForUserEventHandler> _logger;
        private readonly IEventBusSynchronizationService _eventBusSynchronizationService;

        public RejectAccountLoanForUserEventHandler(
            AccountContext accountContext,
            IAccountIntegrationEventService userIntegrationEventService,
            IEventBusSynchronizationService eventBusSynchronizationService,
            ILogger<RejectAccountLoanForUserEventHandler> logger)
        {
            _accountContext = accountContext;
            _accountIntegrationEventService = userIntegrationEventService;
            _logger = logger ?? throw new System.ArgumentNullException(nameof(logger));
            this._eventBusSynchronizationService = eventBusSynchronizationService;
        }

        public async Task Handle(RejectAccountLoanForUserEvent @event)
        {
            using (LogContext.PushProperty("IntegrationEventContext", $"{@event.Id}-{Program.ProgramName}"))
            {
                _logger.LogInformation("----- Handling integration event: {IntegrationEventId} at {AppName} - ({@IntegrationEvent})", @event.Id, Program.ProgramName, @event);

                await Task.Delay(1);
                var eventObject = this._eventBusSynchronizationService.EventSynchronizationList.Where(x => x.Key.Equals(@event.EventIdSynchronizationId)).FirstOrDefault().Value;
                eventObject.HasSynchronizationFinish = true;
                eventObject.Message = "User has not enought money";
                eventObject.HttpStatusCode = System.Net.HttpStatusCode.BadRequest;
            }
        }
    }
  
}
