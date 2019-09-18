using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UserManagement.API.API.Model
{
    public class User
    {       
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string EmailAddress { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal MonthlySalary { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal MonthlyExpenses { get; set; }
    }
}
