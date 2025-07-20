using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace CodeTech_Task_1.Models
{
    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }

        [Required]
        [StringLength(100)]
        [ValidateNever]
        public string Name { get; set; }

        [Required]
        [StringLength(100)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 6)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password do not match")]
        [ValidateNever]
        public string ConfirmPassword { get; set; }

        [ValidateNever]
        public ICollection<Order> Orders { get; set; }

        [ValidateNever]
        public ICollection<Payment> Payments { get; set; }

        [ValidateNever]
        public ICollection<Cart> Carts { get; set; }
    }
}


