using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RealEstate.Core.Domain.IdentityEntities;
using RealEstate.Core.Domain.RepositoryContracts;
using RealEstate.Core.Helpers;
using RealEstate.Core.ServiceContracts;
using RealEstate.Core.Services;
using RealEstate.Infrastructure.DbContext;
using RealEstate.Infrastructure.Repositories;
using RealEstate.WebAPI;
using System.Text;

namespace Templet.WebAPI.StartupExtensions
{
	public static class ConfigureServicesExtension
	{
		public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration)
		{
			// Add services to the container.
			services.AddAutoMapper(typeof(MappingConfig));
			services.AddControllers().AddNewtonsoftJson();
			services.AddSwaggerGen(options =>
			{
				options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
				{
					Name = "Authorization",
					Type = SecuritySchemeType.ApiKey,
					Scheme = "Bearer",
					BearerFormat = "JWT",
					In = ParameterLocation.Header,
					Description = "JWT Authorization header using the Bearer scheme"
				});
				options.AddSecurityRequirement(new OpenApiSecurityRequirement()
				{
					{
					new OpenApiSecurityScheme()
					{
						Reference = new OpenApiReference()
						{
							Type=ReferenceType.SecurityScheme,
							Id = "Bearer"

						},
						Name="Bearer",
						In = ParameterLocation.Header
					},
					new List<string>()
					}
				});


			});
			services.AddScoped<IUnitOfWork, UnitOfWork>();
			services.AddScoped<IAuthService, AuthService>();
			services.AddScoped<IJwtService, JwtService>();
			services.AddDbContext<ApplicationDbContext>(options =>
			{
				options.UseSqlServer(configuration.GetConnectionString("Default"));
			});
			services.Configure<JWT>(configuration.GetSection("JWT"));


			services.AddIdentity<ApplicationUser, ApplicationRole>((options) =>
			{
				options.Password.RequiredLength = 8;
				options.Password.RequireNonAlphanumeric = true;
				options.Password.RequireUppercase = true;
				options.Password.RequireLowercase = true;
				options.Password.RequireDigit = true;
				options.Password.RequiredUniqueChars = 3;
			})
				.AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders()
				.AddUserStore<UserStore<ApplicationUser, ApplicationRole, ApplicationDbContext, Guid>>()
				.AddRoleStore<RoleStore<ApplicationRole, ApplicationDbContext, Guid>>();
			services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			})
				.AddJwtBearer(o =>
				{
					o.RequireHttpsMetadata = false;
					o.SaveToken = true;
					o.TokenValidationParameters = new TokenValidationParameters
					{
						ValidateIssuerSigningKey = true,
						ValidateIssuer = true,
						ValidateAudience = true,
						ValidateLifetime = true,
						ValidIssuer = configuration["JWT:Issuer"],
						ValidAudience = configuration["JWT:Audience"],
						IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"]!)),
						ClockSkew = TimeSpan.Zero
					};
				});


			return services;
		}
	}
}
