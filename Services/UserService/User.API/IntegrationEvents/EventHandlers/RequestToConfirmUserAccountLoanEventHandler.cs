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
    public class RequestToConfirmUserAccountLoanEventHandler :
        IIntegrationEventHandler<RequestToConfirmUserAccountLoandEvent>
    {
        private readonly UserManagementContext _userContext;
        private readonly IUserManagementIntegrationEventService _userIntegrationEventService;
        private readonly ILogger<RequestToConfirmUserAccountLoanEventHandler> _logger;

        public RequestToConfirmUserAccountLoanEventHandler(
            UserManagementContext userContext,
            IUserManagementIntegrationEventService userIntegrationEventService,
            ILogger<RequestToConfirmUserAccountLoanEventHandler> logger)
        {
            _userContext = userContext;
            _userIntegrationEventService = userIntegrationEventService;
            _logger = logger ?? throw new System.ArgumentNullException(nameof(logger));
        }

        public async Task Handle(RequestToConfirmUserAccountLoandEvent @event)
        {
            using (LogContext.PushProperty("IntegrationEventContext", $"{@event.Id}-{Program.ProgramName}"))
            {
                _logger.LogInformation("----- Handling integration event: {IntegrationEventId} at {AppName} - ({@IntegrationEvent})", @event.Id, Program.ProgramName, @event);

                // find the user in the database 
                var user = this._userContext.Users.Where(X => X.Id.Equals(@event.UserId)).FirstOrDefault();

                if(user != default || user != null)
                {
                    // check the salary and expensive 
                    if(user.MonthlySalary - user.MonthlyExpenses < 1000)
                    {
                        var rejectIntegrationEvent = new RejectAccountLoanForUserEvent(@event.UserId, @event.loan);
                        await this._userIntegrationEventService.SaveEventAndUserContextChangesAsync(rejectIntegrationEvent);
                        await this._userIntegrationEventService.PublishThroughEventBusAsync(rejectIntegrationEvent);
                    }
                    else
                    {
                        var confirmedIntegratonEvent = new ConfirmUserAccountForLoanIntegrationEvent(@event.UserId, @event.loan);

                        await this._userIntegrationEventService.SaveEventAndUserContextChangesAsync(confirmedIntegratonEvent);
                        await this._userIntegrationEventService.PublishThroughEventBusAsync(confirmedIntegratonEvent);
                    }
                }
                else
                {
                    var usernotfoundEvent = new UserNotFoundEvent(@event.UserId, @event.loan);
                    await this._userIntegrationEventService.SaveEventAndUserContextChangesAsync(usernotfoundEvent);
                    await this._userIntegrationEventService.PublishThroughEventBusAsync(usernotfoundEvent);
                }          
            }
        }
    }
}
