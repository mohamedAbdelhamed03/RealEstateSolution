using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Core.DTO
{
	public class EstateResponseDTO
	{
		public Guid Id { get; set; }
		[Required]
		[MaxLength(30)]
		public string? Name { get; set; }
		[Required]
		public string? EstateNumber { get; set; }
		public string? Details { get; set; }
		[Required]
		public double Rate { get; set; }
		public int Sqft { get; set; }
		public int Occupancy { get; set; }
		public double Price { get; set; }
		public int Bedrooms { get; set; }
		public int Bathrooms { get; set; }

		public string? ImageUrl { get; set; }
		public string? ImageLocalPath { get; set; }
		public string? Amenity { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }
		public Guid CategoryId { get; set; } // Foreign key to Category
		public string? CategoryName { get; set; }
		public Guid CompanyId { get; set; }
		public string? CompanyName { get; set; }
	}
}
