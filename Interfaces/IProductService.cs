using Product_Catalog.Dtos;
using Product_Catalog.Models;
using Product_Catalog.ViewModels;
using System.Runtime.InteropServices;

namespace Product_Catalog.Interfaces
{
    public interface IProductService : IBaseRepository<Product>
    {
        Task<IEnumerable<Product>> ActiveProducts([Optional] int? CategoryId);
        Task<ReturnProduct> CreateProduct(CreateProductDto productDto);
        Task<ReturnProduct> EditProduct(ProductsViewModel model);
        Task<IEnumerable<Product>> GetAllProducts([Optional] int? CategoryId);
    }
}
