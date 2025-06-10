using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Core.Domain.Entities
{
	public class Category
	{
		[Key]
		public Guid Id { get; set; }
		public string? Name { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }
		public virtual ICollection<Estate>? Estates { get; set; }

	}
}
