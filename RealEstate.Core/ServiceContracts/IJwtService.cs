using RealEstate.Core.Domain.Entities;
using RealEstate.Core.Domain.IdentityEntities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Core.ServiceContracts
{
	public interface IJwtService
	{
		Task<JwtSecurityToken> CreateJwtToken(ApplicationUser user);
		RefreshToken GenerateRefreshToken();
	}
}
