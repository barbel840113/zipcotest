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
    public class UserNotFoundEventHandler : IIntegrationEventHandler<UserNotFoundEvent>
    {
        private readonly AccountContext _accountContext;
        private readonly IAccountIntegrationEventService _accountIntegrationEventService;
        private readonly ILogger<UserNotFoundEventHandler> _logger;
        private readonly IEventBusSynchronizationService _eventBusSynchronizationService;

        public UserNotFoundEventHandler(
            AccountContext accountContext,
            IAccountIntegrationEventService userIntegrationEventService,
            IEventBusSynchronizationService eventBusSynchronizationService,
            ILogger<UserNotFoundEventHandler> logger)
        {
            _accountContext = accountContext;
            _accountIntegrationEventService = userIntegrationEventService;
            _logger = logger ?? throw new System.ArgumentNullException(nameof(logger));
            this._eventBusSynchronizationService = eventBusSynchronizationService;
        }

        public async Task Handle(UserNotFoundEvent @event)
        {
            using (LogContext.PushProperty("IntegrationEventContext", $"{@event.Id}-{Program.ProgramName}"))
            {
                _logger.LogInformation("----- Handling integration event: {IntegrationEventId} at {AppName} - ({@IntegrationEvent})", @event.Id, Program.ProgramName, @event);

                this._eventBusSynchronizationService.HasSynchronizationFinish = true;
                this._eventBusSynchronizationService.Message = "User not found";
                this._eventBusSynchronizationService.HttpStatusCode = System.Net.HttpStatusCode.NotFound;
            }
        }
    }
}
