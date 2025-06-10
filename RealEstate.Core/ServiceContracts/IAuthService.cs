using RealEstate.Core.DTO;
using RealEstate.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Core.ServiceContracts
{
	public interface IAuthService
	{
		Task<APIResponse> RegisterAsync(RegisterRequestDTO registerRequestDTO);
		Task<APIResponse> LoginAsync(LoginRequestDTO loginRequestDTO);
		Task<APIResponse> RefreshTokenAsync(string token);
		Task<bool> RevokeRefreshTokenAsync(string token);

	}
}
