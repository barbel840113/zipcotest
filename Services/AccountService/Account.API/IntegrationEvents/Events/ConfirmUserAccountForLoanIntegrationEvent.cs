using EventBusLibrary.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Account.API.IntegrationEvents.Events
{
    public class ConfirmUserAccountForLoanIntegrationEvent : IntegrationEvent
    {
        public Guid UserId { get; set; }

        public double loan { get; set; }

        public ConfirmUserAccountForLoanIntegrationEvent(Guid id, double loan)
        {
            this.UserId = id;
            this.loan = loan;
        }
    }
}
