using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Account.API.Services
{
    public class EventBusSynchronizationService : IEventBusSynchronizationService
    {

        public EventBusSynchronizationService()
        {
            this.EventSynchronizationList = new Dictionary<Guid, SynchronizationDetails>();
        }
      
        public Dictionary<Guid, SynchronizationDetails> EventSynchronizationList { get; set; }

        public async Task CheckIFHasSynchronizationFinish(Guid eventId)
        {
            var findEvent = this.EventSynchronizationList.Where(x => x.Key.Equals(eventId)).FirstOrDefault();

            var value = findEvent.Value;
           
            while (!value.HasSynchronizationFinish)
            { 
                // enough to synchronize
                await Task.Delay(5000);
                if (!value.HasSynchronizationFinish)
                {
                    value.HasSynchronizationFinish = true;
                }

            }
        }       
    }
}
