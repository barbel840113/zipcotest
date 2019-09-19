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
    public class OrderStatusChangedToAwaitingValidationIntegrationEventHandler :
        IIntegrationEventHandler<OrderStatusChangedToAwaitingValidationIntegrationEvent>
    {
        private readonly AccountContext _accountContext;
        private readonly IAccountManagementIntegrationEventService _accountIntegrationEventService;
        private readonly ILogger<OrderStatusChangedToAwaitingValidationIntegrationEventHandler> _logger;

        public OrderStatusChangedToAwaitingValidationIntegrationEventHandler(
            AccountContext accountContext,
            IAccountManagementIntegrationEventService userIntegrationEventService,
            ILogger<OrderStatusChangedToAwaitingValidationIntegrationEventHandler> logger)
        {
            _accountContext = accountContext;
            _accountIntegrationEventService = userIntegrationEventService;
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
                var confirmedIntegrationEvent = new OrderStatusChangedToPaidIntegrationEvent(@event.UserId, @event.loan);

                await this._accountIntegrationEventService.SaveEventAndAccountContextChangesAsync(confirmedIntegrationEvent);
                await this._accountIntegrationEventService.PublishThroughEventBusAsync(confirmedIntegrationEvent);

            }
        }
    }
}
