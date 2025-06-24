using RealEstate.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Core.DTOs
{
	public class RegisterRequestDTO
	{
		[Required]
		public string? PersonName { get; set; }
		[Required]
		[EmailAddress]
		public string? Email { get; set; }
		[Required]
		[DataType(DataType.PhoneNumber)]
		public string? PhoneNumber { get; set; }
		[Required]
		[DataType(DataType.Password)]
		public string? Password { get; set; }
		[Required]
		[DataType(DataType.Password)]
		[Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
		public string? ConfirmPassword { get; set; }
		[Required]
		public RoleOptions Role { get; set; }
	}
}
