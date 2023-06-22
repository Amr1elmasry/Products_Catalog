using Mapster;
using NuGet.Protocol;
using Product_Catalog.Dtos;
using Product_Catalog.Interfaces;
using Product_Catalog.Models;
using Product_Catalog.ViewModels;
using System.Runtime.InteropServices;

namespace Product_Catalog.Services
{
    public class ProductService : BaseRepository<Product>, IProductService
    {
        public ProductService(ProductsCatalogDbContext Context) : base(Context)
        {
        }

        public async Task<IEnumerable<Product>> ActiveProducts([Optional] int? CategoryId)
        {
            var products = Enumerable.Empty<Product>();
            if (CategoryId == null || CategoryId == 0)
            {
                products = await FindAll(p => p.StartDate!.Value.AddDays(p.DurationInDays) > DateTime.Now);
            }
            else
            {
                products = await FindAll(p => p.StartDate!.Value.AddDays(p.DurationInDays) > DateTime.Now &&
                    p.CategoryId == CategoryId);
            }
            if (products == null)
                return Enumerable.Empty<Product>();
            return products;
        }

        public async Task<IEnumerable<Product>> GetAllProducts([Optional] int? CategoryId)
        {
            var products = Enumerable.Empty<Product>();
            if (CategoryId == null || CategoryId == 0)
            {
                products = await GetAll();
            }
            else
            {
                products = await FindAll(p =>p.CategoryId == CategoryId);
            }
            if (products == null)
                return Enumerable.Empty<Product>();
            return products;
        }

        public async Task<ReturnProduct> CreateProduct(CreateProductDto productDto)
        {

            var output = new ReturnProduct();
            if (productDto == null) { output.Message = "No Data Found"; }
            else
            {
                var checkProduct = await Find(p=>p.Name == productDto.Name);
                if (checkProduct != null)
                {
                    output.Message = "There is another product with the same name !";
                }
                else
                {
                    var product = productDto.Adapt<Product>();
                    await Add(product);
                    await CommitChanges();
                    output.Product = product;
                }
            }
            return output;
        }
        public async Task<ReturnProduct> EditProduct(ProductsViewModel model)
        {
            var output = new ReturnProduct();
            if(model == null) { output.Message = "No Data Found"; }
            else
            {
                var checkProduct = await FindById(model.Id);
                if (checkProduct == null)
                {
                    output.Message = "No Product Found With this Id!";
                }
                else
                {
                    var product = model.Adapt<Product>();
                    checkProduct.CreationDate = product.CreationDate;
                    if (product.ToJson() == checkProduct.ToJson())
                    {
                        output.Message = "No Changes Found";
                    }
                    else
                    {
                        checkProduct = await Find(p => p.Name == model.Name && p.Id!=model.Id);
                        if (checkProduct != null)
                        {
                            output.Message = "There is another product with the same name !";
                        }
                        else
                        {
                            var updatedProduct = await Update(product);
                            await CommitChanges();
                            output.Product = updatedProduct;
                        }
                    }
                }
            }
            return output;
        }
    }
}
