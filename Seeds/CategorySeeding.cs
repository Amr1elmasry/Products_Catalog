using Product_Catalog.Models;

namespace Product_Catalog.Seeds
{
    public class CategorySeeding
    {
        public static async Task SeedCategories(ProductsCatalogDbContext  context)
        {
            var categories = context.Categories.Count();
            if (categories == 0)
            {
                var Electronics = new Category
                {
                    CategoryName = "Electronics"
                };
                var Computers = new Category
                {
                    CategoryName = "Computers"
                };
                var Home = new Category
                {
                    CategoryName = "Home"
                };

                await context.AddAsync(Electronics);
                await context.AddAsync(Computers);
                await context.AddAsync(Home);
                await context.SaveChangesAsync();
            }
        }
    }
}
