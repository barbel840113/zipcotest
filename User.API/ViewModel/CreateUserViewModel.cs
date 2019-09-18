using System.ComponentModel.DataAnnotations;

namespace UserManagement.API.ViewModel
{
    public class CreateUserViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string EmailAddress { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Monthly Salary must be positive number")]
        public double MonthlySalary { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Monthly Expenses must be positive number")]
        public double MonthlyExpenses { get; set; }
    }
}
