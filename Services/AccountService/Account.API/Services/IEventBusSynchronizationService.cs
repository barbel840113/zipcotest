using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Account.API.Services
{
    public interface IEventBusSynchronizationService
    {
        Task CheckIFHasSynchronizationFinish(Guid eventId);

        Dictionary<Guid, SynchronizationDetails> EventSynchronizationList { get; set; }
    }

    public class SynchronizationDetails : IDisposable
    {       

        public CancellationTokenSource Token { get; set; }

        public string Message { get; set; }

        public HttpStatusCode HttpStatusCode { get; set; }

        public Guid NewCreatedAccountId { get; set; }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }

}
