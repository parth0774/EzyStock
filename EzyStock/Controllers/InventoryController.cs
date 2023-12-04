using EzyStock.DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EzyStock.Controllers
{
    public class InventoryController : Controller
    {
        private readonly DB_Context _context;
        public InventoryController(DB_Context context)
        {
            _context = context;
        }
        // GET: Inventory
        public async Task<IActionResult> Index()
        {
            var inventories = await _context.Inventory
                .Include(i => i.Product)
                .ToListAsync();

            return View(inventories);
        }
        // POST: Inventory/UpdateQuantity/5
        [HttpPost]
        public async Task<IActionResult> UpdateQuantity(int id, int quantity)
        {
            var inventory = await _context.Inventory.FindAsync(id);

            if (inventory == null)
            {
                return NotFound();
            }

            inventory.Quantity = quantity;
            _context.Update(inventory);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}