using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RealEstate.Core.Domain.IdentityEntities;
using RealEstate.Core.Domain.RepositoryContracts;
using RealEstate.Core.Helpers;
using RealEstate.Core.ServiceContracts;
using RealEstate.Core.Services;
using RealEstate.Infrastructure.DbContext;
using RealEstate.Infrastructure.Repositories;
using System.Text;

namespace RealEstate.WebAPI
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.
			builder.Services.AddAutoMapper(typeof(MappingConfig));
			builder.Services.AddControllers().AddNewtonsoftJson();
			builder.Services.AddSwaggerGen(options =>
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
			builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
			builder.Services.AddScoped<IAuthService, AuthService>();
			builder.Services.AddScoped<IJwtService, JwtService>();
			builder.Services.AddDbContext<ApplicationDbContext>(options =>
			{
				options.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
			});
			builder.Services.Configure<JWT>(builder.Configuration.GetSection("JWT"));


			builder.Services.AddIdentity<ApplicationUser, ApplicationRole>((options) =>
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
			builder.Services.AddAuthentication(options =>
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
						ValidIssuer = builder.Configuration["JWT:Issuer"],
						ValidAudience = builder.Configuration["JWT:Audience"],
						IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]!)),
						ClockSkew = TimeSpan.Zero
					};
				});

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}
			app.UseStaticFiles();
			app.UseHttpsRedirection();
			app.UseAuthentication();
			app.UseAuthorization();


			app.MapControllers();

			app.Run();
		}
	}
}
