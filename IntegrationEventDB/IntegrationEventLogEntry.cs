using EventBusLibrary.Events;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace IntegrationEventDB
{
    public class IntegrationEventLogEntry
    {
        private IntegrationEventLogEntry() { }
        public IntegrationEventLogEntry(IntegrationEvent @event, Guid transactionId)
        {
            this.EventId = @event.Id;
            this.CreationTime = @event.CreationDate;
            this.EventTypeName = @event.GetType().FullName;
            this.Content = JsonConvert.SerializeObject(@event);
            this.State = EventStateEnum.NotPublished;
            this.TimesSent = 0;
            this.TransactionId = transactionId.ToString();
        }

        public Guid EventId { get; private set; }
        public string EventTypeName { get; private set; }
        [NotMapped]
        public string EventTypeShortName => this.EventTypeName.Split('.')?.Last();
        [NotMapped]
        public IntegrationEvent IntegrationEvent { get; private set; }
        public EventStateEnum State { get; set; }
        public int TimesSent { get; set; }
        public DateTime CreationTime { get; private set; }
        public string Content { get; private set; }
        public string TransactionId { get; private set; }

        public IntegrationEventLogEntry DeserializeJsonContent(Type type)
        {
            this.IntegrationEvent = JsonConvert.DeserializeObject(Content, type) as IntegrationEvent;
            return this;
        }
    }
}
