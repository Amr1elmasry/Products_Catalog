using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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

        public HomeController(IProductService productService, ProductsCatalogDbContext context, UserManager<ApplicationUser> userManager)
        {
            _productService = productService;
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _productService.GetAll();
            var output = products.Adapt<IEnumerable<ProductsViewModel>>();
            return View(output);
        }


        public async Task<IActionResult> Edit(int? id)
        {
            var product = await _productService.FindById(id!.Value);
            if (product == null)
            {
                return BadRequest("No Product Found With this id!");
            }
            var admins = await _userManager.GetUsersInRoleAsync("Admin");
            ViewData["admins"] = new SelectList(admins , "Id" , "FirstName");
            return View(product.Adapt<ProductsViewModel>());
        }
    }
}