using Product_Catalog.Dtos;
using Product_Catalog.Models;
using Product_Catalog.ViewModels;

namespace Product_Catalog.Interfaces
{
    public interface IProductService : IBaseRepository<Product>
    {
        Task<ReturnProduct> CreateProduct(CreateProductDto productDto);
        Task<ReturnProduct> EditProduct(ProductsViewModel model);
    }
}
