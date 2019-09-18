using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserManagement.API.Infrastructure.Exceptions
{
    public class AccountDomainException : Exception
    {
        public AccountDomainException() { }

        public AccountDomainException(string message): base(message) { }

        public AccountDomainException(string message, Exception innerexception) : base(message,innerexception){  }
    }
}
