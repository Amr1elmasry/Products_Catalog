using Microsoft.AspNetCore.Mvc;

namespace Product_Catalog.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
