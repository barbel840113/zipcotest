using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Account.API.ViewModels
{
    public class AccountViewModel
    {      
        public Guid Id { get; set; }
     
        public string AccountName { get; set; }
        
        public decimal Loan { get; set; }
       
        public Guid UserId { get; set; }
    }
}
