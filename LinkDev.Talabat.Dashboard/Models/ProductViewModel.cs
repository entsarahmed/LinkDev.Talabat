using LinkDev.Talabat.Core.Domain.Entities.Products;
using System.ComponentModel.DataAnnotations;

namespace LinkDev.Talabat.Dashboard.Models
{
	public class ProductViewModel
	{
		public int Id { get; set; }
		[Required(ErrorMessage = "Name is Required")]
		public required string Name { get; set; }
		[Required(ErrorMessage = "Description is Required")]
		public required string Description { get; set; }
		public IFormFile? Image { get; set; }

		public string? PictureUrl { get; set; }
		[Required(ErrorMessage = "price is Required")]
		[Range(1, 100000)]
		public decimal Price { get; set; }
		public ProductBrand? Brand { get; set; }
		public int? BrandId { get; set; }
		public ProductCategory? Category { get; set; }
		public int? CategoryId { get; set; }
	}
}
