using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeTech_Task_1.Models
{
    public class CartItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Product")]
        public int ProductId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [Range(0.01, 100000)]
        public decimal Price { get; set; }

        [Required]
        [Range(1, 100)]
        public int Quantity { get; set; }

        [StringLength(300)]
        public string ImageUrl { get; set; }

        // Optional navigation property (only if Product entity exists)
        public Product Product { get; set; }

        [NotMapped]
        public decimal TotalPrice => Price * Quantity;
    }

}
