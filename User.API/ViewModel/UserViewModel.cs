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

        [Required]
        public string Name { get; set; }

        [Required]
        public string EmailAddress { get; set; }

        [Required]
        [Range(0, double.MaxValue,ErrorMessage = "Monthly Salary must be positive number")]
        public decimal MonthlySalary { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Monthly Expenses must be positive number")]
        public decimal MonthlyExpenses { get; set; }
    }
}
