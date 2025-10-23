// ============================================================================
// === File: Product.cs
// === Description: Defines the Product entity representing a single building block 
// ===              product in the inventory system, including its attributes 
// ===              (name, description, price, category, etc.) and relationships 
// ===              to purchases and sales.
// ============================================================================

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Klemmbausteine.Models
{
    public class Product
    {
        // --- Primary key of the product entity
        [Key]
        public int Id { get; set; }

        // --- Name of the product
        [Required]
        public string Name { get; set; }

        // --- Description of the product
        [Required]
        public string Description { get; set; }

        // --- Net price of the product, stored as a decimal with precision 10,2
        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal NettoPrice { get; set; }

        // --- Product category (e.g., theme, series, or set type)
        [Required]
        public string Category { get; set; }

        // --- Current stock quantity
        public int InStock { get; set; }

        // --- Optional image URL for the product
        public string? ImageLink { get; set; }

        // --- Navigation property: list of purchases associated with this product
        public List<Purchase>? Purchases { get; set; }

        // --- Navigation property: list of sales associated with this product
        public List<Sale>? Sales { get; set; }
    }
}
