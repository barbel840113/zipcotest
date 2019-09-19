using EventBusLibrary.Interfaces;
using Microsoft.Extensions.Logging;
using Serilog.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserManagement.API.API;
using UserManagement.API.API.Infrastructure.DBContexts;
using UserManagement.API.IntegrationEvents.Events;

namespace UserManagement.API.IntegrationEvents.EventHandlers
{
    public class OrderStatusChangedToPaidIntegrationEventHandler :
         IIntegrationEventHandler<OrderStatusChangedToPaidIntegrationEvent>
    {
        private readonly UserManagementContext _userContext;
        private readonly ILogger<OrderStatusChangedToPaidIntegrationEventHandler> _logger;

        public OrderStatusChangedToPaidIntegrationEventHandler(
            UserManagementContext userContext,
            ILogger<OrderStatusChangedToPaidIntegrationEventHandler> logger)
        {
            this._userContext = userContext;
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
