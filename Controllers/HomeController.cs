using DotNetEnglishP5_3.Data;
using DotNetEnglishP5_3.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace DotNetEnglishP5_3.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return _context.Inventory != null ?
            View(await _context.Inventory
                .Where(items => items.PurchaseDate != DateTime.MinValue)
                .Where(items => items.RepairCost != null)
                .ToListAsync()) :
            Problem("Entity set 'ApplicationDbContext.Inventory'  is null.");
        }

        // GET: Home/Details
        public async Task<IActionResult> Details(int? id)
        {
            Inventory inventory = new Inventory();
            try
            {
                if (id == null || _context.Inventory == null)
                {
                    return NotFound();
                }

                inventory = await _context.Inventory
                    .FirstOrDefaultAsync(m => m.Id == id);
                if (inventory == null)
                {
                    return NotFound();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("InventoryController Create :", e);
            };
            return View(inventory);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}