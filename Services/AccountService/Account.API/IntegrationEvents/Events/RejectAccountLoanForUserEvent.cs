using EventBusLibrary.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Account.API.IntegrationEvents.EventHandlers
{
    public class RejectAccountLoanForUserEvent : IntegrationEvent
    {
        public Guid UserId { get; set; }

        public double Loan { get; set; }

        public int AccountType { get; set; }

        public RejectAccountLoanForUserEvent(Guid id, double loan, int accountType)
        {
            this.UserId = id;
            this.Loan = loan;
            this.AccountType = accountType;
        }
    }
}
