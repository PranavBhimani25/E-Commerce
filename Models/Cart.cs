using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeTech_Task_1.Models
{
    public class Cart
    {
        [Key]
        public int Id { get; set; }

        public string UserId { get; set; } // Link to ApplicationUser

        public List<CartItem> Items { get; set; }

        [NotMapped]
        public decimal TotalAmount => Items?.Sum(i => i.Price * i.Quantity) ?? 0;
    }


}
