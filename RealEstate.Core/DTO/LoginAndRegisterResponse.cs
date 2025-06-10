using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RealEstate.Core.DTO
{
	public class LoginAndRegisterResponse
	{
		public UserResponseDTO? User { get; set; }
		public string? Token { get; set; }
		public DateTime ExpiresOn { get; set; }
		public bool IsAuthenticated { get; set; } = false;
		[JsonIgnore]
		public string? RefreshToken { get; set; }
		public DateTime? RefreshTokenExpiration { get; set; }

	}
}
