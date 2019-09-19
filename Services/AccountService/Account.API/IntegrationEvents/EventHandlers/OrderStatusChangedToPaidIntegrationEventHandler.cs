using Account.API.Infrastructure.DbContexts;
using Account.API.IntegrationEvents.Events;
using EventBusLibrary.Interfaces;
using Microsoft.Extensions.Logging;
using Serilog.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Account.API.IntegrationEvents.EventHandlers
{
    public class OrderStatusChangedToPaidIntegrationEventHandler :
         IIntegrationEventHandler<OrderStatusChangedToPaidIntegrationEvent>
    {
        private readonly AccountContext _accountContext;
        private readonly ILogger<OrderStatusChangedToPaidIntegrationEventHandler> _logger;

        public OrderStatusChangedToPaidIntegrationEventHandler(
            AccountContext accountContext,
            ILogger<OrderStatusChangedToPaidIntegrationEventHandler> logger)
        {
            this._accountContext = accountContext;
            _logger = logger ?? throw new System.ArgumentNullException(nameof(logger));
        }

        public async Task Handle(OrderStatusChangedToPaidIntegrationEvent @event)
        {
            using (LogContext.PushProperty("IntegrationEventContext", $"{@event.Id}-{Program.ProgramName}"))
            {
                _logger.LogInformation("----- Handling integration event: {IntegrationEventId} at {AppName} - ({@IntegrationEvent})", @event.Id, Program.ProgramName, @event);

                //we're not blocking stock/inventory     

            }
        }
    }
}
