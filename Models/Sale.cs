// ============================================================================
// === File: Sale.cs
// === Description: Defines the Sale entity representing the sale transaction 
// ===              of building block products, including pricing, quantity, 
// ===              buyer information, and sale date.
// ============================================================================

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Klemmbausteine.Models
{
    public class Sale
    {
        // --- Primary key of the sale record
        [Key]
        public int Id { get; set; }

        // --- Foreign key referencing the related product
        [ForeignKey("Product")]
        public int ProductId { get; set; }

        // --- Navigation property linking to the sold product
        public Product Product { get; set; } = default!;

        // --- Unit price at which the product was sold
        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal UnitPrice { get; set; }

        // --- Number of units sold in this transaction
        [Required]
        public int Quantity { get; set; }

        // --- Name of the buyer company purchasing the product
        [Required]
        [StringLength(200)]
        public string BuyerCompany { get; set; } = string.Empty;

        // --- Date on which the sale occurred
        [DataType(DataType.Date)]
        public DateTime SaleDate { get; set; }
    }
}
