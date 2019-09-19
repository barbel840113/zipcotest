using EventBusLibrary.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserManagement.API.IntegrationEvents.Events
{
    public class RequestToConfirmUserAccountLoandEvent :  IntegrationEvent
    {
        public Guid UserId { get; set; }

        public double Loan { get; set; }

        public int AccountType { get; set; }

        public RequestToConfirmUserAccountLoandEvent(Guid id, double loan, int accounttype)
        {
            this.UserId = id;
            this.Loan = loan;
            this.AccountType = accounttype;
        }
    }
}
