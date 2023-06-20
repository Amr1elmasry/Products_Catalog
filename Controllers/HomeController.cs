using Mapster;
using Microsoft.AspNetCore.Mvc;
using Product_Catalog.Interfaces;
using Product_Catalog.Models;
using Product_Catalog.ViewModels;
using System.Diagnostics;

namespace Product_Catalog.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductService _productService;

        public HomeController(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _productService.GetAll();
            var output = products.Adapt<IEnumerable<ProductsViewModel>>();
            return View(output);
        }
    }
}