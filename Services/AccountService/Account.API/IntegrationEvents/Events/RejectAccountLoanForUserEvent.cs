using EventBusLibrary.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserManagement.API.IntegrationEvents.EventHandlers
{
    public class RejectAccountLoanForUserEvent : IntegrationEvent
    {
        public Guid UserId { get; set; }

        public double loan { get; set; }

        public RejectAccountLoanForUserEvent(Guid id, double loan)
        {
            this.UserId = id;
            this.loan = loan;
        }
    }
}
