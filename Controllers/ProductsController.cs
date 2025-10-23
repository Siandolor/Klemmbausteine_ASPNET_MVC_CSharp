// ============================================================================
// === File: ProductsController.cs
// === Description: Handles all CRUD and business logic related to Product 
// ===              entities, including filtering, purchase/sale management, 
// ===              price updates, and stock control for the Klemmbausteine app.
// ===              All actions return English outputs or messages for clarity.
// ============================================================================

using Klemmbausteine.Data;
using Klemmbausteine.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Klemmbausteine.Controllers
{
    public class ProductsController : Controller
    {
        private readonly KlemmbausteineContext _context;

        // --- Initializes the controller with a database context
        public ProductsController(KlemmbausteineContext context)
        {
            _context = context;
        }

        // --- Displays a list of products with optional filters for search, category, stock, and price range
        public async Task<IActionResult> Index(string? search, string? category, int? stockFilter, decimal? minPrice, decimal? maxPrice)
        {
            var products = _context.Products.AsQueryable();

            if (!string.IsNullOrEmpty(search))
                products = products.Where(p => p.Name.Contains(search) || p.Description.Contains(search));

            if (!string.IsNullOrEmpty(category))
                products = products.Where(p => p.Category == category);

            if (stockFilter.HasValue)
            {
                if (stockFilter == 0)
                    products = products.Where(p => p.InStock == 0);
                else if (stockFilter == 1)
                    products = products.Where(p => p.InStock > 0);
            }

            if (minPrice.HasValue)
                products = products.Where(p => p.NettoPrice >= minPrice);

            if (maxPrice.HasValue)
                products = products.Where(p => p.NettoPrice <= maxPrice);

            ViewBag.Categories = await _context.Products
                .Select(p => p.Category)
                .Distinct()
                .ToListAsync();

            return View(await products.ToListAsync());
        }

        // --- Displays details for a specific product, including related purchases and sales
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var product = await _context.Products
                .Include(p => p.Purchases)
                .Include(p => p.Sales)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
                return NotFound();

            return View(product);
        }

        // --- Updates the price of a product and saves it to the database
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdatePrice(int id, decimal nettoPrice)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return NotFound();

            product.NettoPrice = nettoPrice;

            try
            {
                _context.Update(product);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Products.Any(e => e.Id == product.Id))
                    return NotFound();
                else
                    throw;
            }

            return RedirectToAction(nameof(Details), new { id = product.Id });
        }

        // --- Adds a new purchase order with a randomized price variation
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPurchase(int productId, int quantity, DateTime expectedDelivery)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null)
                return NotFound();

            var rand = new Random();
            var percent = rand.Next(5, 10 + 1);
            var isMinus = rand.Next(0, 1 + 1) == 0;
            var modifier = 1 + (isMinus ? -percent / 100m : percent / 100m);
            var price = Math.Round(product.NettoPrice * modifier, 2);

            var purchase = new Purchase
            {
                ProductId = productId,
                Quantity = quantity,
                UnitPrice = price,
                ExpectedDelivery = expectedDelivery
            };

            _context.Purchases.Add(purchase);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id = productId });
        }

        // --- Marks a purchase as delivered and updates stock accordingly
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkDelivered(int id)
        {
            var purchase = await _context.Purchases
                .Include(p => p.Product)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (purchase == null)
                return NotFound();

            if (!purchase.Delivered)
            {
                purchase.ActualDelivery = DateTime.Now;
                purchase.Product.InStock += purchase.Quantity;

                _context.Update(purchase);
                _context.Update(purchase.Product);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Details), new { id = purchase.ProductId });
        }

        // --- Deletes a pending (not yet delivered) purchase from the database
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePurchase(int id)
        {
            var purchase = await _context.Purchases.FindAsync(id);
            if (purchase == null)
                return NotFound();

            if (!purchase.Delivered)
            {
                _context.Purchases.Remove(purchase);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Details), new { id = purchase.ProductId });
        }

        // --- Adds a sale transaction, updates product stock, and stores the sale record
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddSale(int productId, int quantity, decimal unitPrice, string buyerCompany, DateTime saleDate)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null)
                return NotFound();

            if (quantity <= 0 || product.InStock < quantity)
            {
                TempData["Error"] = "Not enough stock available for this sale.";
                return RedirectToAction(nameof(Details), new { id = productId });
            }

            var sale = new Sale
            {
                ProductId = productId,
                Quantity = quantity,
                UnitPrice = Math.Round(unitPrice, 2),
                BuyerCompany = buyerCompany,
                SaleDate = saleDate
            };

            _context.Sales.Add(sale);
            product.InStock -= quantity;

            _context.Update(product);
            await _context.SaveChangesAsync();

            TempData["Message"] = "Sale successfully recorded.";
            return RedirectToAction(nameof(Details), new { id = productId });
        }

        // --- Displays the create product form
        public IActionResult Create()
        {
            return View();
        }

        // --- Creates a new product entry and saves it to the database
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Description,NettoPrice,Category,ImageLink")] Product product)
        {
            if (ModelState.IsValid)
            {
                product.InStock = 0;
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // --- Displays the edit form for a product
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return NotFound();

            return View(product);
        }

        // --- Updates product data based on user input and saves changes
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,NettoPrice,Category,InStock,ImageLink")] Product product)
        {
            if (id != product.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Products.Any(e => e.Id == product.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }
    }
}
