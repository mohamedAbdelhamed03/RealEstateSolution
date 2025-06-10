using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RealEstate.Core.DTO;
using RealEstate.Core.Helpers;
using RealEstate.Core.ServiceContracts;
using System.IdentityModel.Tokens.Jwt;

namespace RealEstate.WebAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private readonly IAuthService _authService;
		public AuthController(IAuthService authService)
		{
			_authService = authService;

		}
		[HttpPost("register")]
		public async Task<IActionResult> RegisterAsync([FromBody] RegisterRequestDTO registerRequestDTO)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			var result = await _authService.RegisterAsync(registerRequestDTO);
			var response = result.Result as LoginAndRegisterResponse;

			if (result.Result == null)
			{
				return BadRequest(result.Message);
			}
			return Ok(result);
		}

		[HttpPost("login")]
		public async Task<IActionResult> LoginAsync([FromBody] LoginRequestDTO loginRequestDTO)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			var result = await _authService.LoginAsync(loginRequestDTO);
			var response = result.Result as LoginAndRegisterResponse;

			if (result.Result == null)
			{
				return BadRequest(result.Message);
			}
			if (!string.IsNullOrEmpty(response!.RefreshToken))
			{
				SetRefreshTokenInCooKie(response.RefreshToken, response.RefreshTokenExpiration!.Value);
			}
			return Ok(result);
		}
		[HttpGet("refreshToken")]
		public async Task<IActionResult> RefreshTokenAsync()
		{
			var refreshToken = Request.Cookies["refreshToken"];
			if (string.IsNullOrEmpty(refreshToken))
			{
				return Unauthorized("Refresh token is missing or invalid.");
			}
			var result = await _authService.RefreshTokenAsync(refreshToken);
			var response = result.Result as LoginAndRegisterResponse;
			if (!response!.IsAuthenticated)
			{
				return BadRequest(result);
			}
			SetRefreshTokenInCooKie(response.RefreshToken, response.RefreshTokenExpiration!.Value);

			return Ok(result);
		}
		[HttpPost("revokeRefreshToken")]
		public async Task<IActionResult> RevokeRefreshTokenAsync([FromBody] RevokeRefreshTokenRequestDTO revokeRefreshTokenRequestDTO)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			var token = revokeRefreshTokenRequestDTO.Token ?? Request.Cookies["refreshToken"];
			if (string.IsNullOrEmpty(token))
			{
				return BadRequest("Refresh token is missing or invalid.");
			}
			var result = await _authService.RevokeRefreshTokenAsync(token);
			if (!result)
			{
				return BadRequest("Token is invalid");
			}
			Response.Cookies.Delete("refreshToken");
			return Ok();
		}


		private void SetRefreshTokenInCooKie(string refreshToken, DateTime expires)
		{
			var cookieOptions = new CookieOptions
			{
				HttpOnly = true,
				Expires = expires.ToLocalTime()
			};
			Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
		}
	}
}
