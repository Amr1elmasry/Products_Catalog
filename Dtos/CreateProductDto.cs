using Product_Catalog.Models;
using System.ComponentModel.DataAnnotations;

namespace Product_Catalog.Dtos
{
    public class CreateProductDto
    {
        [Required(ErrorMessage = "You must enter the name of the product!")]
        public string? Name { get; set; }
        public string? Description { get; set; }
        public double? Price { get; set; }
        [DataType(DataType.Date)]
        public DateTime? StartDate { get; set; } = DateTime.Now;
        [Required(ErrorMessage = "You must add the Duration Days")]
        public int DurationInDays { get; set; }
        [Required]
        public int CategoryId { get; set; }
        public string? CreatedByUserId { get; set; }
    }
}
