// ============================================================================
// === File: KlemmbausteineContext.cs
// === Description: Defines the Entity Framework Core database context for the 
// ===              Klemmbausteine application. It manages the connection to the 
// ===              database and provides DbSet properties for Products, Purchases, 
// ===              and Sales entities.
// ============================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Klemmbausteine.Models;

namespace Klemmbausteine.Data
{
    public class KlemmbausteineContext : DbContext
    {
        // --- Initializes the database context with the specified options
        public KlemmbausteineContext(DbContextOptions<KlemmbausteineContext> options)
            : base(options)
        {
        }

        // --- DbSet representing the collection of product entities
        public DbSet<Product> Products { get; set; } = default!;

        // --- DbSet representing the collection of purchase records
        public DbSet<Purchase> Purchases { get; set; } = default!;

        // --- DbSet representing the collection of sales records
        public DbSet<Sale> Sales { get; set; } = default!;
    }
}
