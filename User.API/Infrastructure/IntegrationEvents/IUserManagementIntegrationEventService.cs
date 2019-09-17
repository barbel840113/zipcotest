using EventBusLibrary.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserManagement.API.Infrastructure.IntegrationEvents
{
    public interface IUserManagementIntegrationEventService
    {
        Task SaveEventAndCatalogContextChangesAsync(IntegrationEvent evt);
        Task PublishThroughEventBusAsync(IntegrationEvent evt);
    }
}
