using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UserManagement.API.ViewModel
{
    public class UserViewModel
    {
        public Guid Id { get; set; }
        
        public string Name { get; set; }
      
        public string EmailAddress { get; set; }

       
        public double MonthlySalary { get; set; }
       
        public double MonthlyExpenses { get; set; }
    }
}
