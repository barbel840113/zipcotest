using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserManagement.API.Infrastructure.Exceptions
{
    public class UserDomainException : Exception
    {
        public UserDomainException() { }

        public UserDomainException(string message): base(message) { }

        public UserDomainException(string message, Exception innerexception) : base(message,innerexception){  }
    }
}
