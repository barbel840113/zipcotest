using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Account.API.Services
{
    public interface IEventBusSynchronizationService
    {
        bool HasSynchronizationFinish { get; set; }

        string Message { get; set; }

        HttpStatusCode HttpStatusCode {get;set;}

        Guid NewCreatedAccountId { get; set; }
    }
}
