using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using Product_Catalog.Classes;
using Product_Catalog.Models;

namespace Product_Catalog.Controllers
{
    [Authorize(Roles =RolesNames.Admin)]
    public class CategoriesController : Controller
    {
        private readonly ProductsCatalogDbContext _context;
        private readonly IToastNotification _toastNotification;


        public CategoriesController(ProductsCatalogDbContext context, IToastNotification toastNotification)
        {
            _context = context;
            _toastNotification = toastNotification;
        }

        public async Task<IActionResult> Index()
        {
            var category = await _context.Categories.ToListAsync();
            return View(category);
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Categories == null)
            {
                _toastNotification.AddErrorToastMessage("No category found !");
                return RedirectToAction(nameof(Index));
            }

            var category = await _context.Categories.FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                _toastNotification.AddErrorToastMessage("No category found with this id !");
                return RedirectToAction(nameof(Index));
            }

            return View(category);
        }

        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CategoryName")] Category category)
        {
            if (ModelState.IsValid)
            {
                _context.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Categories == null)
            {
                _toastNotification.AddErrorToastMessage("No category found !");
                return RedirectToAction(nameof(Index));
            }

            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                _toastNotification.AddErrorToastMessage("No category found with this id !");
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CategoryName")] Category category)
        {
            if (id != category.Id)
            {
                _toastNotification.AddErrorToastMessage("this two ids are not match !");
                return RedirectToAction(nameof(Index));
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(category);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.Id))
                    {
                        _toastNotification.AddErrorToastMessage("No category found with this id !");
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        _toastNotification.AddErrorToastMessage("Something went wrong !");
                        return RedirectToAction(nameof(Index));
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Categories == null)
            {
                _toastNotification.AddErrorToastMessage("Something went wrong !");
                return RedirectToAction(nameof(Index));
            }

            var category = await _context.Categories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                _toastNotification.AddErrorToastMessage("No category found with this id !");
                return RedirectToAction(nameof(Index));
            }

            return View(category);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Categories == null)
            {
                _toastNotification.AddErrorToastMessage("Something went wrong !");
                return RedirectToAction(nameof(Index));
            }
            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(int id)
        {
          return (_context.Categories?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
