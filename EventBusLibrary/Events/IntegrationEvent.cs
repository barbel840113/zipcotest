using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace EventBusLibrary.Events
{
    public class IntegrationEvent
    {
        [JsonProperty]
        public Guid Id { get; private set; }

        [JsonProperty]
        public DateTime CreationDate { get; set; }

        [JsonConstructor]
        public IntegrationEvent(Guid id, DateTime createDate) {
            this.Id = id;
            this.CreationDate = createDate;
        }

        public IntegrationEvent()
        {
            this.Id = Guid.NewGuid();
            this.CreationDate = DateTime.Now;                
        }
    }
}
