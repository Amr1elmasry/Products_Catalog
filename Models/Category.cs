namespace Product_Catalog.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string? CategoryName { get; set; }
        public IList<Product>? Products { get; set; }
    }
}
