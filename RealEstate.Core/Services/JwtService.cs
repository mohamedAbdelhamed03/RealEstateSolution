using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RealEstate.Core.Domain.Entities;
using RealEstate.Core.Domain.IdentityEntities;
using RealEstate.Core.Helpers;
using RealEstate.Core.ServiceContracts;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Core.Services
{
	public class JwtService : IJwtService
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly JWT _jwt;
		public JwtService(UserManager<ApplicationUser> userManager, IOptions<JWT> jwt)
		{
			_userManager = userManager;
			_jwt = jwt.Value;
		}
		public async Task<JwtSecurityToken> CreateJwtToken(ApplicationUser user)
		{
			var userClaims = await _userManager.GetClaimsAsync(user);
			var roles = await _userManager.GetRolesAsync(user);
			var roleClaims = new List<Claim>();

			foreach (var role in roles)
				roleClaims.Add(new Claim("roles", role));

			var claims = new[]
			{
				new Claim(JwtRegisteredClaimNames.Sub, user.UserName!),
				new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
				new Claim(JwtRegisteredClaimNames.Email, user.Email!),
			}
			.Union(userClaims)
			.Union(roleClaims);

			var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key!));
			var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

			var jwtSecurityToken = new JwtSecurityToken(
				issuer: _jwt.Issuer,
				audience: _jwt.Audience,
				claims: claims,
				expires: DateTime.Now.AddHours(_jwt.DurationInHours),
				signingCredentials: signingCredentials);

			return jwtSecurityToken;
		}

		public RefreshToken GenerateRefreshToken()
		{
			var randomNumber = new byte[32];
			RandomNumberGenerator.Fill(randomNumber); // Updated to use RandomNumberGenerator static method
			return new RefreshToken()
			{
				Token = Convert.ToBase64String(randomNumber),
				ExpiresOn = DateTime.UtcNow.AddDays(10),
				CreatedOn = DateTime.UtcNow,
			};

		}
	}
}
