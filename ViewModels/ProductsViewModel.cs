using Product_Catalog.Models;
using System.ComponentModel.DataAnnotations;

namespace Product_Catalog.ViewModels
{
    public class ProductsViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "You must enter the name of the product!")]
        public string? Name { get; set; }
        public string? Description { get; set; }
        public double? Price { get; set; }
        public DateTime? CreationDate { get; set; } = DateTime.Now;
        public DateTime? StartDate { get; set; } = DateTime.Now;
        [Required(ErrorMessage = "You must add the Duration Days")]
        public int DurationInDays { get; set; }
        //public ApplicationUser? CreatedByUser { get; set; }
        [Required]
        public string? CreatedByUserId { get; set; }
        //public Category? Category { get; set; }
        [Required]
        public int CategoryId { get; set; }
    }
}
