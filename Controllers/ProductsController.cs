﻿using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using Product_Catalog.Classes;
using Product_Catalog.Interfaces;
using Product_Catalog.Models;
using Product_Catalog.ViewModels;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Product_Catalog.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductService _productService;
        private readonly ProductsCatalogDbContext _context;
        private readonly IToastNotification _toastNotification;

        public ProductsController(IProductService productService, ProductsCatalogDbContext context, IToastNotification toastNotification)
        {
            _productService = productService;
            _context = context;
            _toastNotification = toastNotification;
        }

        public async Task<IActionResult> Index([Optional , FromQuery]int? CategoryId)
        {
            var categories = await _context.Categories.ToListAsync();
            ViewData["categories"] = categories.Select(c=> new CategoryFilter { Id =c.Id , Name =c.CategoryName });
            if (CategoryId != null || CategoryId != 0)
                ViewData["SelectedCategory"] = categories.Find(c => c.Id == CategoryId)?.CategoryName;
            var products = await _productService.ActiveProducts(CategoryId);
            var output = products.Adapt<IEnumerable<ActiveProductViewModel>>().ToList();
            output.ForEach(p => p.CategoryName = _context.Categories.Find(p.CategoryId)?.CategoryName);
            return View(output);
        }

        public async Task<IActionResult> Details(int? id)
        {
            var product = await _productService.FindById(id!.Value);
            if (product == null)
            {
                _toastNotification.AddErrorToastMessage("No product found with this Id !");
                return RedirectToAction("Index");
            }
            var output = product.Adapt<ActiveProductViewModel>();
            output.CategoryName = _context.Categories.Find(product.CategoryId)?.CategoryName;
            return View(output);
        }
    }
}
