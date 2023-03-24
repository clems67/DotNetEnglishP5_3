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
using DotNetEnglishP5_3.ViewModels;
using Microsoft.AspNetCore.Hosting;

namespace DotNetEnglishP5_3.Controllers
{
    [Authorize (Roles = "SuperAdmin,Admin")]
    public class InventoryController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment webHostEnvironment;

        public InventoryController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            this.webHostEnvironment = webHostEnvironment;
        }

        // GET: Inventory
        public async Task<IActionResult> Index()
        {
            return View(await _context.Inventory.ToListAsync());
        }

        // GET: Inventory/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Inventory == null)
            {
                return NotFound();
            }

            var inventory = await _context.Inventory
                .FirstOrDefaultAsync(m => m.Id == id);
            if (inventory == null)
            {
                return NotFound();
            }

            return View(inventory);
        }

        // GET: Inventory/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Inventory/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(InventoryViewModel inventoryViewModel)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = UploadedFile(inventoryViewModel);
                Inventory inventory = new Inventory
                {
                    VIN = inventoryViewModel.VIN,
                    Year = inventoryViewModel.Year,
                    Make = inventoryViewModel.Make,
                    Model = inventoryViewModel.Model,
                    Trim = inventoryViewModel.Trim,
                    PurchaseDate = inventoryViewModel.PurchaseDate,
                    PurchasePrice = inventoryViewModel.PurchasePrice,
                    Repairs = inventoryViewModel.Repairs,
                    RepairCost = inventoryViewModel.RepairCost,
                    LotDate = inventoryViewModel.LotDate,
                    SellingPrice = inventoryViewModel.SellingPrice,
                    SaleDate = inventoryViewModel.SaleDate,
                    Picture = uniqueFileName,
                };
                _context.Add(inventory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        private string UploadedFile(InventoryViewModel inventoryViewModel)
        {
            string uniqueFileName = null;

            if (inventoryViewModel.Picture != null)
            {
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + inventoryViewModel.Picture.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    inventoryViewModel.Picture.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }


        // GET: Inventory/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Inventory == null)
            {
                return NotFound();
            }

            var inventory = await _context.Inventory.FindAsync(id);
            if (inventory == null)
            {
                return NotFound();
            }
            return View(inventory);
        }

        // POST: Inventory/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,VIN,Year,Make,Model,Trim,PurchaseDate,PurchasePrice,Repairs,RepairCost,LotDate,SellingPrice,SaleDate,Picture")] Inventory inventory)
        {
            if (id != inventory.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(inventory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InventoryExists(inventory.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(inventory);
        }

        // GET: Inventory/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Inventory == null)
            {
                return NotFound();
            }

            var inventory = await _context.Inventory
                .FirstOrDefaultAsync(m => m.Id == id);
            if (inventory == null)
            {
                return NotFound();
            }

            return View(inventory);
        }

        // POST: Inventory/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Inventory == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Inventory'  is null.");
            }
            var inventory = await _context.Inventory.FindAsync(id);
            if (inventory != null)
            {
                _context.Inventory.Remove(inventory);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InventoryExists(int id)
        {
            return _context.Inventory.Any(e => e.Id == id);
        }
    }
}
