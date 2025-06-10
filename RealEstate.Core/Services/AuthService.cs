using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RealEstate.Core.Domain.IdentityEntities;
using RealEstate.Core.DTO;
using RealEstate.Core.Enums;
using RealEstate.Core.Helpers;
using RealEstate.Core.ServiceContracts;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Core.Services
{
	public class AuthService : IAuthService
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly RoleManager<ApplicationRole> _roleManager;
		private readonly IJwtService _jwtService;
		private readonly IMapper _mapper;
		public AuthService(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, IMapper mapper, IJwtService jwtService)
		{
			_userManager = userManager;
			_roleManager = roleManager;
			_mapper = mapper;
			_jwtService = jwtService;
		}
		public async Task<APIResponse> RegisterAsync(RegisterRequestDTO registerRequestDTO)
		{
			if (await _userManager.FindByEmailAsync(registerRequestDTO.Email!) != null)
			{
				return new APIResponse
				{
					Message = "Email already exists."
				};
			}
			if (await _userManager.FindByEmailAsync(registerRequestDTO.PersonName!) != null)
			{
				return new APIResponse
				{
					Message = "Username already exists."
				};
			}
			var user = _mapper.Map<ApplicationUser>(registerRequestDTO);
			user.Id = new Guid();
			IdentityResult result = await _userManager.CreateAsync(user, registerRequestDTO.Password!);
			if (!result.Succeeded)
			{
				var errors = string.Empty;
				foreach (var error in result.Errors)
				{
					errors += $"{error.Description},";
				}
				return new APIResponse
				{
					Message = errors.TrimEnd(',')
				};
			}
			if (registerRequestDTO.Role == RoleOptions.Admin)
			{
				if (await _roleManager.FindByNameAsync(RoleOptions.Admin.ToString()) == null)
				{
					ApplicationRole role = new ApplicationRole()
					{
						Name = RoleOptions.Admin.ToString(),
					};
					await _roleManager.CreateAsync(role);

				}
				await _userManager.AddToRoleAsync(user, RoleOptions.Admin.ToString());
			}
			else
			{
				if (await _roleManager.FindByNameAsync(RoleOptions.Customer.ToString()) == null)
				{
					ApplicationRole role = new ApplicationRole()
					{
						Name = RoleOptions.Customer.ToString(),
					};
					await _roleManager.CreateAsync(role);
				}
				await _userManager.AddToRoleAsync(user, RoleOptions.Customer.ToString());
			}
			var jwtSecurityToken = await _jwtService.CreateJwtToken(user);
			var userResponse = _mapper.Map<UserResponseDTO>(user);
			userResponse.Role = registerRequestDTO.Role.ToString();
			var registerResponse = new LoginAndRegisterResponse
			{
				User = userResponse,
				Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
				ExpiresOn = jwtSecurityToken.ValidTo,
				IsAuthenticated = true,
			};
			return new APIResponse
			{
				Result = registerResponse,
			};
		}
		public async Task<APIResponse> LoginAsync(LoginRequestDTO loginRequestDTO)
		{
			var user = await _userManager.FindByEmailAsync(loginRequestDTO.Email!);

			if (user is null || !await _userManager.CheckPasswordAsync(user, loginRequestDTO.Password!))
			{
				return new APIResponse
				{
					Message = "Email or Password is incorrect!",
				};
			}
			var jwtSecurityToken = await _jwtService.CreateJwtToken(user);
			var rolesList = await _userManager.GetRolesAsync(user);
			var userResponse = _mapper.Map<UserResponseDTO>(user);
			userResponse.Role = rolesList.FirstOrDefault();

			var loginResponse = new LoginAndRegisterResponse
			{
				User = userResponse,
				Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
				ExpiresOn = jwtSecurityToken.ValidTo,
				IsAuthenticated = true,
			};
			if (user.RefreshTokens!.Any(t => t.IsActive))
			{
				var activeRefreshToken = user.RefreshTokens!.FirstOrDefault(t => t.IsActive);
				loginResponse.RefreshToken = activeRefreshToken!.Token;
				loginResponse.RefreshTokenExpiration = activeRefreshToken.ExpiresOn;
			}
			else
			{
				var refreshToken = _jwtService.GenerateRefreshToken();

				loginResponse.RefreshToken = refreshToken.Token;
				loginResponse.RefreshTokenExpiration = refreshToken.ExpiresOn;
				user.RefreshTokens!.Add(refreshToken);
				await _userManager.UpdateAsync(user);
			}
			return new APIResponse
			{
				Result = loginResponse,

			};
		}
		public async Task<APIResponse> RefreshTokenAsync(string token)
		{
			APIResponse APIresponse = new APIResponse();
			var user = await _userManager.Users.SingleOrDefaultAsync(u => u.RefreshTokens!.Any(t => t.Token == token));
			if (user is null)
			{
				APIresponse.Result = new LoginAndRegisterResponse(
					)
				{
					IsAuthenticated = false,

				};
				APIresponse.Message = "Invalid Refresh Token!";
				return APIresponse;
			}
			var refreshToken = user!.RefreshTokens!.Single(t => t.Token == token);
			if (!refreshToken.IsActive)
			{
				APIresponse.Result = new LoginAndRegisterResponse(
					)
				{
					IsAuthenticated = false,

				};
				APIresponse.Message = "Inactive Refresh Token!";
				return APIresponse;
			}
			refreshToken.RevokedOn = DateTime.UtcNow;
			var newRefreshToken = _jwtService.GenerateRefreshToken();
			user.RefreshTokens!.Add(newRefreshToken);
			await _userManager.UpdateAsync(user);
			var jwtSecurityToken = await _jwtService.CreateJwtToken(user);
			var rolesList = await _userManager.GetRolesAsync(user);
			var userResponse = _mapper.Map<UserResponseDTO>(user);
			userResponse.Role = rolesList.FirstOrDefault();
			var loginResponse = new LoginAndRegisterResponse
			{

				User = userResponse,
				Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
				ExpiresOn = jwtSecurityToken.ValidTo,
				RefreshToken = newRefreshToken.Token,
				RefreshTokenExpiration = newRefreshToken.ExpiresOn,
				IsAuthenticated = true,
			};
			APIresponse.Result = loginResponse;

			return APIresponse;
		}
		public async Task<bool> RevokeRefreshTokenAsync(string token)
		{
			var user = await _userManager.Users.SingleOrDefaultAsync(u => u.RefreshTokens!.Any(t => t.Token == token));
			if (user is null)
			{
				return false;
			}
			var refreshToken = user!.RefreshTokens!.Single(t => t.Token == token);
			if (!refreshToken.IsActive)
			{
				return false;
			}
			refreshToken.RevokedOn = DateTime.UtcNow;
			await _userManager.UpdateAsync(user);
			return true;
		}
	}
}
