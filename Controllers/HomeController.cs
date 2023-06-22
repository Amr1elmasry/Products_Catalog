using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using Product_Catalog.Classes;
using Product_Catalog.Dtos;
using Product_Catalog.Interfaces;
using Product_Catalog.Models;
using Product_Catalog.ViewModels;
using System.Diagnostics;

namespace Product_Catalog.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductService _productService;
        private readonly ProductsCatalogDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IToastNotification _toastNotification;

        public HomeController(IProductService productService, ProductsCatalogDbContext context, UserManager<ApplicationUser> userManager, IToastNotification toastNotification, RoleManager<IdentityRole> roleManager)
        {
            _productService = productService;
            _context = context;
            _userManager = userManager;
            _toastNotification = toastNotification;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if(user==null || (await _userManager.GetRolesAsync(user)).Any(r=>r != RolesNames.Admin))
            {
                return RedirectToAction("Index","Products");
            }

            var products = await _productService.GetAll();
            var output = products.Adapt<IEnumerable<ProductsViewModel>>();
            return View(output);
        }

        [Authorize(Roles = RolesNames.Admin)]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var admins = await _userManager.GetUsersInRoleAsync("Admin");
            ViewData["admins"] = new SelectList(admins, "Id", "FirstName");
            var categories = await _context.Categories.ToListAsync();
            ViewData["categories"] = new SelectList(categories, "Id", "CategoryName");
            return View();
        }

        [Authorize(Roles = RolesNames.Admin)]
        [HttpPost]
        public async Task<IActionResult> Create(CreateProductDto productDto)
        {
            var admins = await _userManager.GetUsersInRoleAsync("Admin");
            ViewData["admins"] = new SelectList(admins, "Id", "FirstName");
            var categories = await _context.Categories.ToListAsync();
            ViewData["categories"] = new SelectList(categories, "Id", "CategoryName");
            productDto.CreatedByUserId = _userManager.GetUserId(User);
            var product = await _productService.CreateProduct(productDto);
            if (product.Product == null || product.Message != string.Empty)
            {
                //ModelState.AddModelError("Model", product.Message);
                _toastNotification.AddErrorToastMessage(product.Message);
                return View(productDto);
            }
            _toastNotification.AddSuccessToastMessage("Product Add Successfully");
            return RedirectToAction("Index");
        }


        [Authorize(Roles = RolesNames.Admin)]
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            var admins = await _userManager.GetUsersInRoleAsync("Admin");
            ViewData["admins"] = new SelectList(admins, "Id", "FirstName");
            var categories = await _context.Categories.ToListAsync();
            ViewData["categories"] = new SelectList(categories, "Id", "CategoryName");
            var product = await _productService.FindById(id!.Value);
            if (product == null)
            {
                _toastNotification.AddErrorToastMessage("No Product Found With this id!");
                return RedirectToAction("Index");

            }
            return View(product.Adapt<ProductsViewModel>());
        }

        [Authorize(Roles = RolesNames.Admin)]
        [HttpPost]
        public async Task<IActionResult> Edit(ProductsViewModel model)
        {
            var admins = await _userManager.GetUsersInRoleAsync("Admin");
            ViewData["admins"] = new SelectList(admins, "Id", "FirstName");
            var categories = await _context.Categories.ToListAsync();
            ViewData["categories"] = new SelectList(categories, "Id", "CategoryName");
            var product = await _productService.EditProduct(model);
            if(product.Product ==null || product.Message != string.Empty)
            {
                //ModelState.AddModelError("Model", product.Message);
                _toastNotification.AddErrorToastMessage(product.Message);
                return View(model);
            }
            _toastNotification.AddSuccessToastMessage("Product Updated Successfully");
            return View(product.Product.Adapt<ProductsViewModel>());
        }


        [Authorize(Roles = RolesNames.Admin)]
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            var product = await _productService.FindById(id!.Value);
            if(product == null)
            {
                _toastNotification.AddErrorToastMessage("No product found with this id");
                return RedirectToAction("Index");
            }
            return View(product.Adapt<ProductsViewModel>());
        }

        [Authorize(Roles = RolesNames.Admin)]
        [HttpDelete]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            var product = await _productService.FindById(id!.Value);
            if(product == null)
            {
                _toastNotification.AddErrorToastMessage("No product found with this id");
                return RedirectToAction("Index");
            }
            await _productService.Delete(product);
            await _productService.CommitChanges();
            _toastNotification.AddSuccessToastMessage("Product Deleted Successfully");
            return RedirectToAction("Index");
        }
    }
}