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
using Humanizer;

namespace DotNetEnglishP5_3.Controllers
{
    [Authorize(Roles = "SuperAdmin,Admin")]
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
            try
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
                        SellingPrice = inventoryViewModel.PurchasePrice + inventoryViewModel.RepairCost + 500,
                        SaleDate = inventoryViewModel.SaleDate,
                        Picture = uniqueFileName,
                    };
                    _context.Add(inventory);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("InventoryController Create :", e);
            };
            return View();
        }

        private string UploadedFile(InventoryViewModel inventoryViewModel)
        {
            string uniqueFileName = null;
            try
            {


                if (inventoryViewModel.Picture != null)
                {
                    string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "images/inventory");
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + inventoryViewModel.Picture.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        inventoryViewModel.Picture.CopyTo(fileStream);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("InventoryController UploadedFile :", e);
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
        public async Task<IActionResult> Edit(int id, InventoryViewModel inventoryViewModel)
        {
            if (id != inventoryViewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    string uniqueFileName = UploadedFile(inventoryViewModel);
                    Inventory inventory = new Inventory
                    {
                        Id = inventoryViewModel.Id,
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
                        SellingPrice = inventoryViewModel.PurchasePrice + inventoryViewModel.RepairCost + 500,
                        SaleDate = inventoryViewModel.SaleDate,
                        Picture = uniqueFileName,
                    };
                    _context.Update(inventory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InventoryExists(inventoryViewModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        Console.WriteLine("InventoryController POST Edit");
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(inventoryViewModel);
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
                if (inventory.Picture != null)
                {
                    DeletePicture(inventory.Picture);
                }
                _context.Inventory.Remove(inventory);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> DeletePictureView(string Picture, int id)
        {
            var inventory = await _context.Inventory.FindAsync(id);
            if (inventory != null)
            {
                inventory.Picture = null;
                _context.Update(inventory);
                await _context.SaveChangesAsync();
                DeletePicture(Picture);
            }
            return RedirectToAction(nameof(Edit), new { id });
        }

        public void DeletePicture(string Picture)
        {
            string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "images/inventory");
            string filePath = Path.Combine(uploadsFolder, Picture);
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
        }

        private bool InventoryExists(int id)
        {
            return _context.Inventory.Any(e => e.Id == id);
        }
    }
}
