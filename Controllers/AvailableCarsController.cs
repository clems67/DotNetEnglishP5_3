using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DotNetEnglishP5_3.Data;
using DotNetEnglishP5_3.Models;
using Microsoft.AspNetCore.Authorization;

namespace DotNetEnglishP5_3.Controllers
{
    [Authorize]
    public class AvailableCarsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AvailableCarsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: AvailableCars
        public async Task<IActionResult> Index()
        {
            return _context.Inventory != null ?
                        View(await _context.Inventory
                            .Where(items => items.PurchaseDate != DateTime.MinValue)
                            .ToListAsync()) :
                        Problem("Entity set 'ApplicationDbContext.Inventory'  is null.");
        }

    }
}
