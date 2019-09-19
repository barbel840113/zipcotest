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
    public class OrderStatusChangedToAwaitingValidationIntegrationEventHandler :
        IIntegrationEventHandler<OrderStatusChangedToAwaitingValidationIntegrationEvent>
    {
        private readonly UserManagementContext _userContext;
        private readonly IUserManagementIntegrationEventService _userIntegrationEventService;
        private readonly ILogger<OrderStatusChangedToAwaitingValidationIntegrationEventHandler> _logger;

        public OrderStatusChangedToAwaitingValidationIntegrationEventHandler(
            UserManagementContext userContext,
            IUserManagementIntegrationEventService userIntegrationEventService,
            ILogger<OrderStatusChangedToAwaitingValidationIntegrationEventHandler> logger)
        {
            _userContext = userContext;
            _userIntegrationEventService = userIntegrationEventService;
            _logger = logger ?? throw new System.ArgumentNullException(nameof(logger));
        }

        public async Task Handle(OrderStatusChangedToAwaitingValidationIntegrationEvent @event)
        {
            using (LogContext.PushProperty("IntegrationEventContext", $"{@event.Id}-{Program.ProgramName}"))
            {
                _logger.LogInformation("----- Handling integration event: {IntegrationEventId} at {AppName} - ({@IntegrationEvent})", @event.Id, Program.ProgramName, @event);



                //var confirmedIntegrationEvent = confirmedOrderStockItems.Any(c => !c.HasStock)
                //    ? (IntegrationEvent)new OrderStockRejectedIntegrationEvent(@event.OrderId, confirmedOrderStockItems)
                //    : new OrderStockConfirmedIntegrationEvent(@event.OrderId);

                var confirmedIntegratonEvent = new OrderStatusChangedToPaidIntegrationEvent(@event.UserId, @event.loan);

                await this._userIntegrationEventService.SaveEventAndUserContextChangesAsync(confirmedIntegratonEvent);
                await this._userIntegrationEventService.PublishThroughEventBusAsync(confirmedIntegratonEvent);

            }
        }
    }
}
