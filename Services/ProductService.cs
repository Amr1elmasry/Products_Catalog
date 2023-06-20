using Product_Catalog.Interfaces;
using Product_Catalog.Models;

namespace Product_Catalog.Services
{
    public class ProductService : BaseRepository<Product>, IProductService
    {
        public ProductService(ProductsCatalogDbContext Context) : base(Context)
        {
        }
    }
}
