using RealEstate.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Core.DTOs
{
	public class UserResponseDTO
	{
		public string? Username { get; set; }
		public string? Email { get; set; }
		public string? Role { get; set; }
	}
}
