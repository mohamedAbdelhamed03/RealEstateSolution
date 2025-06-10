using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Core.Domain.Entities
{
	public class Estate
	{
		[Key]
		public Guid Id { get; set; }
		public string? Name { get; set; }
		public string? EstateNumber { get; set; }
		public string? Details { get; set; }
		public double Rate { get; set; }
		public int Sqft { get; set; }
		public int Occupancy { get; set; }
		public string? ImageUrl { get; set; }
		public string? ImageLocalPath { get; set; }
		public string? Amenity { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }
		public Guid CategoryId { get; set; }
		[ForeignKey("CategoryId")]
		public Category? Category { get; set; }
	}
}
