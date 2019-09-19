using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Account.API.Services
{
    public class EventBusSynchronizationService : IEventBusSynchronizationService
    {
        public bool HasSynchronizationFinish { get; set; }
        public string Message {get; set; }
        public HttpStatusCode HttpStatusCode { get; set ; }
        public Guid NewCreatedAccountId { get; set; }
    }
}
