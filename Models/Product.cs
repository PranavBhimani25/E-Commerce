using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeTech_Task_1.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }

        public int Stock { get; set; }

        public string ImageUrl { get; set; }



        [ValidateNever]
        public ICollection<Order> Orders { get; set; }


        [ValidateNever]
        public ICollection<Cart> Carts { get; set; }

    }


}
