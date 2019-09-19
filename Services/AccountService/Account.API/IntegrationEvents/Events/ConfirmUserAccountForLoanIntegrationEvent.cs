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

        public double Loan { get; set; }

        public int AccountType { get; set; }

        public ConfirmUserAccountForLoanIntegrationEvent(Guid id, double loan, int accountType)
        {
            this.UserId = id;
            this.Loan = loan;
            this.AccountType = accountType;
        }
    }
}
