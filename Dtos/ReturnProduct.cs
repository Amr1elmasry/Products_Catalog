using Product_Catalog.Models;

namespace Product_Catalog.Dtos
{
    public class ReturnProduct
    {
        public string? Message { get; set; } = string.Empty;    
        public Product? Product { get; set; }
    }
}
