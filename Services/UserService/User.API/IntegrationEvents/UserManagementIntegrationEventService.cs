using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using EventBusLibrary.Events;
using EventBusLibrary.Interfaces;
using IntegrationEventDB;
using IntegrationEventDB.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UserManagement.API.API;
using UserManagement.API.API.Infrastructure.DBContexts;

namespace UserManagement.API.IntegrationEvents
{
    public class UserManagementIntegrationEventService : IUserManagementIntegrationEventService
    {
        private readonly Func<DbConnection, IIntegrationEventLogService> _integrationEventLogServiceFactory;
        private readonly IEventBus _eventBus;
        private readonly UserManagementContext _usersContext;
        private readonly IIntegrationEventLogService _eventLogService;
        private readonly ILogger<UserManagementIntegrationEventService> _logger;

        public UserManagementIntegrationEventService(
           ILogger<UserManagementIntegrationEventService> logger,
           IEventBus eventBus,
           UserManagementContext userContext,
           Func<DbConnection, IIntegrationEventLogService> integrationEventLogServiceFactory)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _usersContext = userContext ?? throw new ArgumentNullException(nameof(userContext));
            _integrationEventLogServiceFactory = integrationEventLogServiceFactory ?? throw new ArgumentNullException(nameof(integrationEventLogServiceFactory));
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
            _eventLogService = _integrationEventLogServiceFactory(_usersContext.Database.GetDbConnection());
        }

        public async Task PublishThroughEventBusAsync(IntegrationEvent evt)
        {
            try
            {
                _logger.LogInformation("----- Publishing integration event: {IntegrationEventId_published} from {AppName} - ({@IntegrationEvent})", evt.Id, Program.ProgramName, evt);

                await _eventLogService.MarkEventAsInProgressAsync(evt.Id);
                _eventBus.Publish(evt);
                await _eventLogService.MarkEventAsPublishedAsync(evt.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ERROR Publishing integration event: {IntegrationEventId} from {AppName} - ({@IntegrationEvent})", evt.Id, Program.ProgramName, evt);
                await _eventLogService.MarkEventAsFailedAsync(evt.Id);
            }
        }

        public async Task SaveEventAndUserContextChangesAsync(IntegrationEvent evt)
        {
            _logger.LogInformation("----- UserManagemmentIntegrationEventService - Saving changes and integrationEvent: {IntegrationEventId}", evt.Id);

            //Use of an EF Core resiliency strategy when using multiple DbContexts within an explicit BeginTransaction():
            //See: https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency            
            await ResilientTransaction.New(this._usersContext).ExecuteAsync(async () =>
            {
                // Achieving atomicity between original catalog database operation and the IntegrationEventLog thanks to a local transaction
                await this._usersContext.SaveChangesAsync();
                await this._eventLogService.SaveEventAsync(evt, this._usersContext.Database.CurrentTransaction);
            });
        }
    }
}
