using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Core.DTOs
{
	public class EstateUpdateDTO
	{
		[Required]

		public Guid Id { get; set; }
		[Required]
		[MaxLength(30)]
		public string? Name { get; set; }
		[Required]
		public string? EstateNumber { get; set; }
		public string? Details { get; set; }
		[Required]
		public double Rate { get; set; }
		[Required]
		public int Sqft { get; set; }
		[Required]
		public int Occupancy { get; set; }
		public double Price { get; set; }
		public int Bedrooms { get; set; }
		public int Bathrooms { get; set; }

		public string? ImageLocalPath { get; set; }
		public IFormFile? Image { get; set; }
		public string? Amenity { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }
		public Guid CategoryId { get; set; } // Foreign key to Category
		public Guid CompanyId { get; set; } // Foreign key to Company

	}

}
