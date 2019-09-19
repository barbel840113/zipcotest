using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Account.API.Model
{
    public class Account
    {
        [Key]
        public Guid Id { get; set; }       

        [Required]
        public string AccountName { get; set; }

        [Required]
        [Range(0,1000)]
        public decimal Loan { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [Required]
        public int AccountType { get; set; }
    }
}
